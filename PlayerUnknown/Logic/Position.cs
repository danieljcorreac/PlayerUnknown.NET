namespace PlayerUnknown.Logic
{
    public class Position
    {
        /// <summary>
        /// Gets the X position.
        /// </summary>
        public float X
        {
            get;
        }

        /// <summary>
        /// Gets the Y position.
        /// </summary>
        public float Y
        {
            get;
        }

        /// <summary>
        /// Gets the Z position.
        /// </summary>
        public float Z
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> class.
        /// </summary>
        /// <param name="X">The x.</param>
        /// <param name="Y">The y.</param>
        /// <param name="Z">The z.</param>
        public Position(float X, float Y, float Z = 0)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Position"/>
        /// with the specified parameters.
        /// </summary>
        /// <param name="X">The x.</param>
        /// <param name="Y">The y.</param>
        /// <param name="Z">The z.</param>
        public static Position New(float X, float Y, float Z = 0)
        {
            return new Position(X, Y, Z);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return "[ X : " + X + ", Y : " + Y + ", Z : " + Z + "]";
        }
    }
}
