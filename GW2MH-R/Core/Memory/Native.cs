using System;
using System.Runtime.InteropServices;

namespace GW2MH.Core.Memory
{
    internal static class Native
    {
        
        private const string KERNEL32 = "kernel32.dll";
        private const string USER32 = "user32.dll";

        #region "KERNEL32"
        [DllImport(KERNEL32, SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, uint processId);

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

        [DllImport(KERNEL32, SetLastError = true)]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, uint nSize, MemoryProtectionFlags flNewProtect, out MemoryProtectionFlags flOldProtect);

        [DllImport(KERNEL32, SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint nSize, AllocationTypeFlags flAllocationType, MemoryProtectionFlags flProtect);

        [DllImport(KERNEL32, SetLastError = true)]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint nSize, uint dwFreeType);
        #endregion

        #region "USER32"
        [DllImport(USER32, SetLastError = true)]
        public static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        [DllImport(USER32, SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, UInt64 wParam, Int64 lParam);
        #endregion

        #region "Flags"
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

        [Flags]
        public enum MemoryProtectionFlags : uint
        {
            PAGE_EXECUTE = 0x10,
            PAGE_EXECUTE_READ = 0x20,
            PAGE_EXECUTE_READ_WRITE = 0x40,
            PAGE_EXECUTE_WRITE_COPY = 0x80,
            PAGE_NOACCESS = 0x02,
            PAGE_READ_WRITE = 0x04,
            PAGE_WRITECOPY = 0x08
        }

        [Flags]
        public enum AllocationTypeFlags : uint
        {
            MEM_COMMIT = 0x00001000,
            MEM_RESERVE = 0x00002000,
            MEM_RESET = 0x00080000,
            MEM_RESET_UNDO = 0x1000000
        }
#endregion

    }
}