namespace PlayerUnknown.Native.Structures
{
    using System.Runtime.InteropServices;

    /// <summary>
    ///     The <see cref="Point" /> structure defines the x and y coordinates of a point.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        /// <summary>
        ///     The x-coordinate of the point.
        /// </summary>
        public int X;

        /// <summary>
        ///     The y-coordinate of the point.
        /// </summary>
        public int Y;

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"X = {this.X} Y = {this.Y}";
        }
    }
}
