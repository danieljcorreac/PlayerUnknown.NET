namespace PlayerUnknown.Native
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    using PlayerUnknown.Native.Enums;

    public static class Win32
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
            OpenPermissions     AccessPermissions,
            bool                InheritHandle,
            int                 ProcessId
        );

        /// <summary>
        /// Opens the specified process.
        /// </summary>
        /// <param name="Process">The process.</param>
        /// <param name="Flags">The flags.</param>
        public static IntPtr OpenProcess(Process Process, OpenPermissions Flags = OpenPermissions.All)
        {
            return OpenProcess(Flags, false, Process.Id);
        }
    }
}
