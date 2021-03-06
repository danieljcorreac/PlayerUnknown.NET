﻿namespace PlayerUnknown.Native.Enums
{
    /// <summary>
    ///     Window states list.
    /// </summary>
    public enum WindowStates
    {
        /// <summary>Hides the window and activates another window.</summary>
        Hide = 0,

        /// <summary>
        ///     Activates and displays a window. If the window is minimized or maximized, the system restores it to its original
        ///     size and position.
        ///     An application should specify this flag when displaying the window for the first time.
        /// </summary>
        ShowNormal = 1,

        /// <summary>
        ///     Activates the window and displays it as a minimized window.
        /// </summary>
        ShowMinimized = 2,

        /// <summary>
        ///     Activates the window and displays it as a maximized window.
        /// </summary>
        ShowMaximized = 3,

        /// <summary>
        ///     Maximizes the specified window.
        /// </summary>
        Maximize = 3,

        /// <summary>
        ///     Displays a window in its most recent size and position.
        ///     This value is similar to <see cref="ShowNormal" />, except the window is not actived.
        /// </summary>
        ShowNormalNoActivate = 4,

        /// <summary>
        ///     Activates the window and displays it in its current size and position.
        /// </summary>
        Show = 5,

        /// <summary>
        ///     Minimizes the specified window and activates the next top-level window in the Z order.
        /// </summary>
        Minimize = 6,

        /// <summary>
        ///     Displays the window as a minimized window. This value is similar to <see cref="ShowMinimized" />, except the window
        ///     is not activated.
        /// </summary>
        ShowMinNoActive = 7,

        /// <summary>
        ///     Displays the window in its current size and position. This value is similar to <see cref="Show" />, except the
        ///     window is not activated.
        /// </summary>
        ShowNoActivate = 8,

        /// <summary>
        ///     Activates and displays the window. If the window is minimized or maximized, the system restores it to its original
        ///     size and position.
        ///     An application should specify this flag when restoring a minimized window.
        /// </summary>
        Restore = 9,

        /// <summary>
        ///     Sets the show state based on the <see cref="WindowStates" /> value specified in the STARTUPINFO structure passed to
        ///     the CreateProcess
        ///     function by the program that started the application.
        /// </summary>
        ShowDefault = 10,

        /// <summary>
        ///     Windows 2000/XP: Minimizes a window, even if the thread that owns the window is hung.
        ///     This flag should only be used when minimizing windows from a different thread.
        /// </summary>
        ForceMinimized = 11
    }
}