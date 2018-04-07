namespace PlayerUnknown.Native
{
    using System;
    using System.Runtime.InteropServices;

    public static class Win32
    {
        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr Handle, out Rectangle Rectangle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr Handle, ref WindowPlacement Placement);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr Handle, IntPtr Address, byte[] Buffer, int Size, int BytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr Handle, IntPtr Address, byte[] Buffer, int Size, int BytesWritten);

        [DllImport("user32.dll")]
        public static extern int SetWindowText(IntPtr Handle, string Title);

        /// <summary>
        /// Gets the window placement using the specified handle.
        /// </summary>
        /// <param name="Handle">The handle.</param>
        public static WindowPlacement GetWindowPlacement(IntPtr Handle)
        {
            WindowPlacement Placement = new WindowPlacement();

            if (Win32.GetWindowPlacement(Handle, ref Placement))
            {
                return Placement;
            }

            return Placement;
        }

        /// <summary>
        /// Gets the window rectangle using the specified handle.
        /// </summary>
        /// <param name="Handle">The handle.</param>
        public static Rectangle GetWindowRectangle(IntPtr Handle)
        {
            if (Win32.GetWindowRect(Handle, out Rectangle Rectangle) == 0)
            {
                return Rectangle;
            }

            return Rectangle;
        }
    }
}
