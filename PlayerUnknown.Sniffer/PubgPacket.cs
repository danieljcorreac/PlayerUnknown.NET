namespace PlayerUnknown.Sniffer
{
    public class PubgPacket
    {
        /// <summary>
        /// Gets or sets the buffer.
        /// </summary>
        public byte[] Buffer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public int Length
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgPacket"/> class.
        /// </summary>
        /// <param name="Buffer">The buffer.</param>
        public PubgPacket(byte[] Buffer)
        {
            this.Buffer = Buffer;
            this.Length = Buffer.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgPacket"/> class.
        /// </summary>
        /// <param name="Buffer">The buffer.</param>
        /// <param name="Length">The length.</param>
        public PubgPacket(byte[] Buffer, int Length)
        {
            this.Buffer = Buffer;
            this.Length = Length;
        }

        /// <summary>
        /// Creates a new instance of <see cref="PubgPacket"/> using the specified buffer.
        /// </summary>
        /// <param name="Buffer">The buffer.</param>
        public static PubgPacket FromBuffer(byte[] Buffer)
        {
            return new PubgPacket(Buffer);
        }
    }
}
