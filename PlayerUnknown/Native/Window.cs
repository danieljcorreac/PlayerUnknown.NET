namespace PlayerUnknown.Native
{
    using System;

    using NetCoreEx.Geometry;

    using WinApi.User32;

    public static class Window
    {
        /// <summary>
        /// Gets the window placement using the specified handle.
        /// </summary>
        /// <param name="Handle">The handle.</param>
        public static WindowPlacement GetWindowPlacement(IntPtr Handle)
        {
            if (User32Methods.GetWindowPlacement(Handle, out WindowPlacement Placement))
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
            if (User32Methods.GetWindowRect(Handle, out Rectangle Rectangle))
            {
                return Rectangle;
            }

            return Rectangle;
        }

        /// <summary>
        /// Finds the specified window.
        /// </summary>
        /// <param name="ClassName">Name of the class.</param>
        /// <param name="WindowsName">Name of the windows.</param>
        public static IntPtr FindWindow(string ClassName, string WindowsName = null)
        {
            return User32Methods.FindWindow(ClassName, WindowsName);
        }

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <param name="Handle">The handle.</param>
        /// <param name="Title">The title.</param>
        public static bool SetTitle(IntPtr Handle, string Title)
        {
            return User32Methods.SetWindowText(Handle, Title);
        }
    }
}
