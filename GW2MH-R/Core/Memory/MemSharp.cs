using GW2MH.Core.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace GW2MH.Core.Memory
{
    public class MemSharp : IDisposable
    {

        public Process TargetProcess { get; private set; }
        public IntPtr ElevatedHandle { get; private set; }

        public MemSharp(Process process)
        {
            TargetProcess = process;

            ElevatedHandle = Native.OpenProcess(Native.ProcessAccessFlags.All, false, (uint)TargetProcess.Id);
        }

        public MemSharp(int processId) : this(Process.GetProcessById(processId)) { }

        public bool IsRunning => !TargetProcess.HasExited;

        public void Write<T>(IntPtr address, T value, bool relative = false)
        {
            address = relative ? MakeAbsoluteAddress(address) : address;

            var bytesWritten = UIntPtr.Zero;
            if (typeof(T) == typeof(byte[]))
            {
                var b = (byte[])(object)value;

                if (!Native.WriteProcessMemory(ElevatedHandle, address, b, (uint)b.Length, out bytesWritten))
                    throw new Exception("Win32 Last Error: " + Marshal.GetLastWin32Error().ToString());
            }
            else
            {
                var size = (uint)Marshal.SizeOf(typeof(T));
                IntPtr pointerToValue = Marshal.AllocHGlobal((int)size);
                Marshal.StructureToPtr(value, pointerToValue, true);

                if (!Native.WriteProcessMemory(ElevatedHandle, address, pointerToValue, size, out bytesWritten))
                    throw new Exception("Win32 Last Error: " + Marshal.GetLastWin32Error().ToString());
            }            
        }

        public void Write<T>(IntPtr address, int[] offsets, T value)
        {
            Write(ReadMultiLevelPointer(address, offsets), value, false);
        }

        public IntPtr ReadMultiLevelPointer(IntPtr address, int[] offsets)
        {
            var pointer = address;

            for(int i = 0; i < offsets.Length; i++)
            {
                if (i == offsets.Length - 1)
                    return pointer + offsets[i];
                else
                {
                    pointer = Read<IntPtr>(pointer + offsets[i]);
                    if (pointer == IntPtr.Zero)
                        return IntPtr.Zero;
                }
            }

            return IntPtr.Zero;
        }

        public T Read<T>(IntPtr address, int[] offsets)
        {
            return Read<T>(ReadMultiLevelPointer(address, offsets), false);
        }

        public T Read<T>(IntPtr address, bool relative = false)
        {
            var size = (uint)Marshal.SizeOf(typeof(T));
            return Read<T>(address, size, relative);
        }

        public T Read<T>(IntPtr address, uint size, bool relative = false)
        {
            address = relative ? MakeAbsoluteAddress(address) : address;

            var buffer = new byte[size];
            var bytesRead = UIntPtr.Zero;
            if (!Native.ReadProcessMemory(ElevatedHandle, address, buffer, (uint)buffer.Length, out bytesRead))
                return default(T);

            if (typeof(T) == typeof(byte[]))
                return (T)(object)buffer;
            else if (typeof(T) == typeof(byte) || typeof(T) == typeof(char))
                return (T)(object)buffer[0];
            else if (typeof(T) == typeof(double))
                return (T)(object)BitConverter.ToDouble(buffer, 0);
            else if (typeof(T) == typeof(float))
                return (T)(object)BitConverter.ToSingle(buffer, 0);
            else if (typeof(T) == typeof(int))
                return (T)(object)BitConverter.ToInt32(buffer, 0);
            else if (typeof(T) == typeof(uint))
                return (T)(object)BitConverter.ToUInt32(buffer, 0);
            else if (typeof(T) == typeof(long))
                return (T)(object)BitConverter.ToInt64(buffer, 0);
            else if (typeof(T) == typeof(ulong))
                return (T)(object)BitConverter.ToUInt64(buffer, 0);
            else if (typeof(T) == typeof(short))
                return (T)(object)BitConverter.ToInt16(buffer, 0);
            else if (typeof(T) == typeof(ushort))
                return (T)(object)BitConverter.ToUInt16(buffer, 0);
            else if (typeof(T) == typeof(IntPtr))
                return IntPtr.Size == sizeof(int) ? (T)(object)new IntPtr(BitConverter.ToInt32(buffer, 0)) : (T)(object)new IntPtr(BitConverter.ToInt64(buffer, 0));
            else
                throw new NotSupportedException(string.Format("The Type: {0} is not supported.", typeof(T).ToString()));
        }

        public IntPtr MakeRelativeAddress(IntPtr absoluteAddress)
        {
#if WIN64
            return new IntPtr(absoluteAddress.ToInt64() - TargetProcess.MainModule.BaseAddress.ToInt64());
#else
            return new IntPtr(absoluteAddress.ToInt32() - TargetProcess.MainModule.BaseAddress.ToInt32());
#endif
        }

        public IntPtr MakeAbsoluteAddress(IntPtr relativeAddress)
        {
#if WIN64
            return new IntPtr(relativeAddress.ToInt64() + TargetProcess.MainModule.BaseAddress.ToInt64());
#else
            return new IntPtr(relativeAddress.ToInt32() + TargetProcess.MainModule.BaseAddress.ToInt32());
#endif
        }

        public IntPtr Pattern(ProcessModule module, byte[] pattern, string mask)
        {
            var processMemoryDump = Read<byte[]>(module.BaseAddress, (uint)module.ModuleMemorySize);

            var block = new byte[pattern.Length];
            for(var i = 0; i < processMemoryDump.Length - pattern.Length + 1; i++)
            {
                Buffer.BlockCopy(processMemoryDump, i, block, 0, pattern.Length);

                if (Compare(block, pattern, mask))
                    return module.BaseAddress + i;
            }

            return IntPtr.Zero;
        }

        public IntPtr Pattern(ProcessModule module, string pattern)
        {
            var data = BuildPatternScanData(pattern);
            var bPattern = data.Pattern;
            var mask = data.Mask;

            return Pattern(module, bPattern, mask);
        }

        private PatternScanData BuildPatternScanData(string pattern)
        {
            StringBuilder s = new StringBuilder();
            List<byte> patternData = new List<byte>();

            pattern = pattern.RemoveWhiteSpaces();
            for(int i = 0; i < pattern.Length; i++)
            {
                var b = pattern.Substring(i, 2);
                if (!b.Contains("?"))
                {
                    s.Append("x");
                    patternData.Add(Convert.ToByte(b, 16));
                    i++;
                }
                else
                {
                    s.Append("?");
                    patternData.Add(0x00);
                }
            }

            return new PatternScanData() { Pattern = patternData.ToArray(), Mask = s.ToString() };
        }

        private bool Compare(byte[] memory, byte[] pattern, string mask)
        {
            for (var i = 0; i < memory.Length; i++)
            {
                if (memory[i] != pattern[i] && mask[i] == 'x')
                    return false;
            }
            return true;
        }

        public void Dispose()
        {
            Native.CloseHandle(ElevatedHandle);
        }

        private struct PatternScanData
        {
            public byte[] Pattern;
            public string Mask;
        }
    }
}