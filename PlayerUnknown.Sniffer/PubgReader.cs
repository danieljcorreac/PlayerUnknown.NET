namespace PlayerUnknown.Sniffer
{
    using System;
    using System.Text;

    public class PubgReader : IDisposable
    {
        protected byte[] Buffer;

        /// <summary>
        /// Gets the length of the stream.
        /// </summary>
        public int Length
        {
            get
            {
                return this.Buffer.Length;
            }
        }

        /// <summary>
        /// Gets or sets the offset of the stream.
        /// </summary>
        public int Offset
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the count of bytes left.
        /// </summary>
        public int BytesLeft
        {
            get
            {
                return this.Buffer.Length - this.Offset;
            }
        }

        /// <summary>
        /// Gets a value indicating if the stream end has been reached.
        /// </summary>
        public bool IsAtEnd
        {
            get
            {
                return this.Buffer.Length <= this.Offset;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="PubgReader"/> is disposed.
        /// </summary>
        public bool Disposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgReader"/> class.
        /// </summary>
        /// <param name="Size">The size.</param>
        public PubgReader(int Size)
        {
            this.Buffer = new byte[Size];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubgReader"/> class.
        /// </summary>
        /// <param name="Buffer">The buffer.</param>
        public PubgReader(byte[] Buffer)
        {
            this.Buffer = Buffer;
        }

        /// <summary>
        /// Reads a element of buffer.
        /// </summary>
        private byte Read()
        {
            return this.Buffer[this.Offset++];
        }

        /// <summary>
        /// Reads a short value.
        /// </summary>
        public short ReadShort()
        {
            return (short) (this.Read() << 8 | this.Read());
        }

        /// <summary>
        /// Reads a ushort value.
        /// </summary>
        public ushort ReadUShort()
        {
            return (ushort) (this.Read() << 8 | this.Read());
        }

        /// <summary>
        /// Reads a int value.
        /// </summary>
        public int ReadInt()
        {
            return this.Read() << 24 | this.Read() << 16 | this.Read() << 8 | this.Read();
        }

        /// <summary>
        /// Reads a int value.
        /// </summary>
        public int ReadInt24()
        {
            return this.Read() << 16 | this.Read() << 8 | this.Read();
        }

        /// <summary>
        /// Reads a uint value.
        /// </summary>
        public uint ReadUInt()
        {
            return (uint) (this.Read() << 24 | this.Read() << 16 | this.Read() << 8 | this.Read());
        }

        /// <summary>
        /// Reads a uint value.
        /// </summary>
        public uint ReadUInt24()
        {
            return (uint) (this.Read() << 16 | this.Read() << 8 | this.Read());
        }

        /// <summary>
        /// Reads a long value.
        /// </summary>
        public long ReadLong()
        {
            return (long) (this.Read() << 56 | this.Read() << 48 | this.Read() << 40 | this.Read() << 32 | this.Read() << 24 | this.Read() << 16 | this.Read() << 8 | this.Read());
        }

        /// <summary>
        /// Reads a ulong value.
        /// </summary>
        public ulong ReadULong()
        {
            return (ulong) (this.Read() << 56 | this.Read() << 48 | this.Read() << 40 | this.Read() << 32 | this.Read() << 24 | this.Read() << 16 | this.Read() << 8 | this.Read());
        }

        /// <summary>
        /// Reads a byte array using the specified length.
        /// </summary>
        /// <param name="Length">The length.</param>
        public byte[] ReadBytes(int Length)
        {
            if (Length < 0)
            {
                if (Length != -1)
                {
                    throw new Exception("Length is invalid at ReadBytes(" + Length + ").");
                }

                return null;
            }

            return this.ReadRange(Length);
        }

        /// <summary>
        /// Reads a string value.
        /// </summary>
        public string ReadString()
        {
            int Length = this.ReadInt();

            if (Length < 0)
            {
                if (Length != -1)
                {
                    throw new Exception("Length is invalid at ReadBytes(" + Length + ").");
                }

                return null;
            }

            return Encoding.UTF8.GetString(this.ReadRange(Length));
        }

        /// <summary>
        /// Reads a range of bytes from the buffer.
        /// </summary>
        private byte[] ReadRange(int Length)
        {
            if (Length > this.BytesLeft)
            {
                throw new Exception("ReadRange(" + Length + "), this.BytesLeft < " + Length + ".");
            }

            byte[] Bytes = new byte[Length];
            Array.Copy(this.Buffer, this.Offset, Bytes, 0, Length);
            this.Offset += Length;

            return Bytes;
        }

        /// <summary>
        /// Skips some bytes to reach a certain offset.
        /// </summary>
        /// <param name="Length">The length.</param>
        public void SkipBytes(int Length)
        {
            this.Offset += Length;

            if (this.Offset >= this.Length)
            {
                this.Offset = this.Length;
            }
        }

        /// <summary>
        /// Exécute les tâches définies par l'application associées à
        /// la libération ou à la redéfinition des ressources non managées.
        /// </summary>
        public void Dispose()
        {
            if (this.Disposed)
            {
                return;
            }

            this.Buffer     = null;
            this.Offset     = 0;
            this.Disposed   = true;
        }
    }
}
