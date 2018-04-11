namespace PlayerUnknown.Native
{
    using System.Runtime.InteropServices;

    using NetCoreEx.Geometry;

    using WinApi.User32;

    public static class Mouse
    {
        [DllImport("user32.dll", EntryPoint = "mouse_event")]
        public static extern void mouse_event(int Flags, int X, int Y, int Buttons, int Extras);

        /// <summary>
        /// Gets the position of the mouse cursor.
        /// </summary>
        public static Point GetPosition()
        {
            if (User32Methods.GetCursorPos(out Point Point))
            {
                return Point;
            }

            return new Point(-1, -1);
        }

        /// <summary>
        /// Sets the position of the mouse cursor,
        /// using the specified <see cref="Point"/>.
        /// </summary>
        /// <param name="Position">The new position.</param>
        /// <param name="SimulateEvent">If set to true, simulates a mouse_event.</param>
        public static void SetPosition(Point Position)
        {
            Mouse.mouse_event((int) MouseInputFlags.MOUSEEVENTF_MOVE, 3, 3, 0, 0);
        }
        
        /// <summary>
        /// Moves the position of the mouse cursor,
        /// using the specified <see cref="Point"/>.
        /// </summary>
        /// <param name="NewPosition">The new position.</param>
        /// <param name="SimulateEvent">If set to true, simulates a mouse_event.</param>
        public static void MovePosition(int DiffX, int DiffY)
        {
            Mouse.mouse_event((int) MouseInputFlags.MOUSEEVENTF_MOVE, DiffX, DiffY, 0, 0);
        }

        /// <summary>
        /// Clicks this instance.
        /// </summary>
        public static void Click()
        {
            Mouse.mouse_event((int) (MouseInputFlags.MOUSEEVENTF_LEFTDOWN), 0, 0, 0, 0);
        }
    }
}
