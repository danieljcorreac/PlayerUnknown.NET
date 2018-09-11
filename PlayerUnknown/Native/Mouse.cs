namespace PlayerUnknown.Native
{
    using PlayerUnknown.Native.Enums;
    using PlayerUnknown.Native.Structures;

    public static class Mouse
    {
        /// <summary>
        /// Gets the position of the mouse cursor.
        /// </summary>
        public static Point GetPosition()
        {
            if (Win32.GetCursorPos(out Point Point))
            {
                return Point;
            }

            return new Point()
            {
                X = -1,
                Y = -1
            };
        }

        /// <summary>
        /// Sets the position of the mouse cursor,
        /// using the specified <see cref="Point"/>.
        /// </summary>
        /// <param name="Position">The new position.</param>
        /// <param name="SimulateEvent">If set to true, simulates a mouse_event.</param>
        public static void SetPosition(Point Position)
        {
            Win32.mouse_event((int) MouseInputFlags.Move, 3, 3, 0, 0);
        }
        
        /// <summary>
        /// Moves the position of the mouse cursor,
        /// using the specified <see cref="Point"/>.
        /// </summary>
        /// <param name="NewPosition">The new position.</param>
        /// <param name="SimulateEvent">If set to true, simulates a mouse_event.</param>
        public static void MovePosition(int DiffX, int DiffY)
        {
            Win32.mouse_event((int) MouseInputFlags.Move, DiffX, DiffY, 0, 0);
        }

        /// <summary>
        /// Clicks this instance.
        /// </summary>
        public static void Click()
        {
            Win32.mouse_event((int) MouseInputFlags.LeftDown, 0, 0, 0, 0);
        }
    }
}
