namespace PlayerUnknown.Sniffer
{
    using System;
    using System.Linq;

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
            if (Buffer == null)
            {
                throw new ArgumentNullException(nameof(Buffer), "Buffer was null at FromBuffer(Buffer).");
            }

            return new PubgPacket(Buffer);
        }

        /// <summary>
        /// Creates a new instance of <see cref="PubgPacket"/> using the specified hexa.
        /// </summary>
        /// <param name="Hexa">The hexa.</param>
        public static PubgPacket FromHexa(string Hexa)
        {
            Hexa = Hexa.Replace("-", "");

            if (string.IsNullOrWhiteSpace(Hexa))
            {
                throw new ArgumentNullException(nameof(Hexa), "Hexa was null or empty at FromHexa(Hexa).");
            }

            return FromBuffer(Enumerable.Range(0, Hexa.Length).Where(T => T % 2 == 0).Select(T => Convert.ToByte(Hexa.Substring(T, 2), 16)).ToArray());
        }
    }
}
