using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

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

        public bool IsRunning => TargetProcess.HasExited;

        public void Write<T>(IntPtr address, T value, bool relative = false)
        {
            var size = (uint)Marshal.SizeOf(typeof(T));
            IntPtr pointerToValue = Marshal.AllocHGlobal((int)size);
            Marshal.StructureToPtr(value, pointerToValue, true);

            var bytesWritten = UIntPtr.Zero;
            if (!Native.WriteProcessMemory(ElevatedHandle, relative ? MakeAbsoluteAddress(address) : address, pointerToValue, size, out bytesWritten))
                throw new Exception("Win32 Last Error: " + Marshal.GetLastWin32Error().ToString());
        }

        public T Read<T>(IntPtr address, bool relative = false)
        {
            var size = (uint)Marshal.SizeOf(typeof(T));
            return Read<T>(address, size, relative);
        }

        public T Read<T>(IntPtr address, uint size, bool relative = false)
        {
            address = relative ? MakeAbsoluteAddress(address) : address;

            object obj = null;
            var bytesRead = UIntPtr.Zero;

            byte[] buffer = null;

            if (!Native.ReadProcessMemory(ElevatedHandle, address, buffer, size, out bytesRead))
                throw new Exception("Win32 Last Error: " + Marshal.GetLastWin32Error().ToString());
            //if (!Native.ReadProcessMemory(ElevatedHandle, address, obj, size, out bytesRead))
            //    throw new Exception("Win32 Last Error: " + Marshal.GetLastWin32Error().ToString());

            return (T)obj;
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
            var processMemoryDump = Read<byte[]>(module.BaseAddress, (uint)pattern.Length);

            var block = new byte[pattern.Length];
            for(var i = 0; i < processMemoryDump.Length; i++)
            {
                Buffer.BlockCopy(processMemoryDump, i, block, 0, pattern.Length);

                if (Compare(block, pattern, mask))
                    return module.BaseAddress + i;
            }

            return IntPtr.Zero;
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
    }

    public static class Native
    {

        private const string KERNEL32 = "kernel32.dll";

        /// <summary>
        /// Opens an existing local process object.
        /// </summary>
        /// <param name="dwDesiredAccess">The access to the process object.</param>
        /// <param name="bInheritHandle">Determines wether processes created by this process will inherit the handle.</param>
        /// <param name="processId">The identifier of the local process to be opened.</param>
        /// <returns>Returns an open handle on success or NULL on error.</returns>
        [DllImport(KERNEL32, SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, uint processId);

        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="hObject">A valid handle to an open object.</param>
        /// <returns>Returns whether the function succeeded.</returns>
        [DllImport(KERNEL32, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport(KERNEL32, SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

        [DllImport(KERNEL32, SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

        [DllImport(KERNEL32, SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesRead);

        [DllImport(KERNEL32, SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out, MarshalAs(UnmanagedType.AsAny)] object lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesRead);
        
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

    }
}