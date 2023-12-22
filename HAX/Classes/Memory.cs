using HAX.Classes;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HAX
{
    internal static class Memory
    {
        public static IntPtr ProcessHandle;
        public static Process Process;

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hwnd, out Rect lpRect);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        public static T ReadMemory<T>(IntPtr Address) where T : struct
        {
            int m_iBytesRead = 0;
            int ByteSize = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[ByteSize];
            ReadProcessMemory(ProcessHandle, Address, buffer, buffer.Length, ref m_iBytesRead);

            return ByteArrayToStructure<T>(buffer);
        }

        public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }

        public static IntPtr GetClientAddress(Process process)
        {
            IntPtr client = IntPtr.Zero;
            foreach (ProcessModule module in process.Modules)
            {
                if (module.ModuleName == "client.dll")
                    client = module.BaseAddress;
            }

            return client;
        }

        public static IntPtr GetEntityList()
        {
            return Memory.ReadMemory<IntPtr>(Main.ClientAddress + Offsets.dwEntityList);
        }
    }
}
