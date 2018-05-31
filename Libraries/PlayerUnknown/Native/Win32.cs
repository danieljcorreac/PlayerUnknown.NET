namespace PlayerUnknown.Native
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    using PlayerUnknown.Native.Enums;

    using Process.NET.Native.Types;

    public static class Win32
    {
        public const int SeLoadDriverPrivilege = 10;
        public const int SeDebugPrivilege      = 20;

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("User32.dll")]
        public static extern int SetForegroundWindow(
            IntPtr              Handle
        );

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PrintWindow(
            IntPtr              Handle,
            IntPtr              DC,
            uint                Flags
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
            OpenPermissions     AccessPermissions,
            bool                InheritHandle,
            int                 ProcessId
        );

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReadProcessMemory(
            int                 Process,
            long                BaseAddress,
            byte[]              Buffer,
            long                Size,
            ref long            NumberOfBytesRead
        );

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(
            IntPtr              Handle,
            out Rectangle       Rectangle
        );

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(
            IntPtr              Handle,
            ref WindowPlacement Placement
        );

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowText(
            IntPtr              Handle,
            string              Title
        );

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(
            string              ClassName,
            string              WindowName
        );

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(
            out Point           Point
        );

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void mouse_event(
            int                 Flags,
            int                 X,
            int                 Y,
            int                 Buttons,
            int                 Extras
        );


        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(
            IntPtr              Handle
        );

        [DllImport("User32.dll")]
        public static extern void ReleaseDC(
            IntPtr              Handle,
            IntPtr              HandleDc
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

        public static uint CtlCode(uint DeviceType, uint Function, uint Method, uint Access)
        {
            return (((DeviceType) << 16) | ((Access) << 14) | ((Function) << 2) | (Method));
        } 
    }
}
