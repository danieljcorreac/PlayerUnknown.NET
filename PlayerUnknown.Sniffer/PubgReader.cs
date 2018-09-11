namespace PlayerUnknown.Sniffer
{
    using System;
    using System.Text;

    public class PubgReader
    {
        public readonly StringBuilder BinString;
        
        /// <summary>
        /// Gets the current position of the reader.
        /// </summary>
        public int Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the total number of bits.
        /// </summary>
        public int Length
        {
            get;
        }

        /// <summary>
        /// Gets the remaining number of bits.
        /// </summary>
        public int Remain
        {
            get
            {
                return this.Length - this.Position;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has no bits to read anymore.
        /// </summary>
        public bool IsEnd
        {
            get
            {
                return this.Remain == 0;
            }
        }

        /// <summary>
        /// create a PubgReader
        /// </summary>
        /// <param name="Data">data to read</param>
        public PubgReader(byte[] Data)
        {
            this.Length    = Data.Length * 8;
            this.BinString = new StringBuilder(this.Length);

            for (int i = 0; i < Data.Length; i++)
            {
                this.BinString.Append(PubgReader.ByteToBinString(Data[i]));
            }

            this.Position = 0;
        }

        /// <summary>
        /// Reads 8 bits at the specified offset, and returns a byte.
        /// </summary>
        /// <param name="Offset">The offset.</param>
        public byte ReadByte(int Offset)
        {
            var bin = this.BinString.ToString(Offset, 8);
            return Convert.ToByte(bin, 2);
        }

        /// <summary>
        /// Reads 8 bits, increase the position, and returns a byte.
        /// </summary>
        public byte ReadByte()
        {
            var Result      = this.ReadByte(this.Position);
            this.Position  += 8;

            return Result;
        }

        /// <summary>
        /// Reads x bits at the specified offset, and returns an integer.
        /// </summary>
        /// <param name="Offset">The offset.</param>
        /// <param name="BitLength">The length.</param>
        public int ReadInt(int Offset, int BitLength)
        {
            var bin = this.BinString.ToString(Offset, BitLength);
            return Convert.ToInt32(bin, 2);
        }

        /// <summary>
        /// Reads x bits and returns an integer.
        /// </summary>
        /// <param name="BitLength">The length.</param>
        public int ReadInt(int BitLength)
        {
            var Result      = this.ReadInt(this.Position, BitLength);
            this.Position  += BitLength;

            return Result;
        }

        /// <summary>
        /// Reads 1 bit at the specified offset, and returns a boolean.
        /// </summary>
        /// <param name="Offset">offset</param>
        public bool ReadBool(int Offset)
        {
            var result = this.ReadInt(Offset, 1);
            return result != 0;
        }

        /// <summary>
        /// Reads 1 bit and returns a boolean.
        /// </summary>
        public bool ReadBool()
        {
            var Result      = this.ReadBool(this.Position);
            this.Position  += 1;

            return Result;
        }

        /// <summary>
        /// read {bitLength} binary string from offset
        /// </summary>
        /// <param name="Offset">offset</param>
        /// <param name="BitLength">length of binary string</param>
        /// <returns></returns>
        public string ReadBinString(int Offset, int BitLength)
        {
            return this.BinString.ToString(Offset, BitLength);
        }

        /// <summary>
        /// read {bitLength} binary string from Position, and move position with {bitLength}
        /// </summary>
        /// <param name="BitLength">length of binary string</param>
        public string ReadBinString(int BitLength)
        {
            var result = this.ReadBinString(this.Position, BitLength);
            this.Position += BitLength;
            return result;
        }

        /// <summary>
        /// read {bitLength} to char from offset
        /// </summary>
        /// <param name="Offset">offset</param>
        /// <param name="BitLength">number of bit</param>
        /// <returns></returns>
        public char ReadChar(int Offset, int BitLength)
        {
            var b = this.ReadInt(Offset, BitLength);
            return Convert.ToChar(b);
        }

        /// <summary>
        /// read {bitLength} to char from Position, and move Position with {bitLength}
        /// </summary>
        /// <param name="BitLength">number of bit</param>
        /// <returns></returns>
        public char ReadChar(int BitLength)
        {
            var result = this.ReadChar(this.Position, BitLength);
            this.Position += BitLength;
            return result;
        }

        /// <summary>
        /// convert byte to 8 bit binary string
        /// </summary>
        /// <param name="B">byte value</param>
        /// <returns>8 bit binary string</returns>
        public static char[] ByteToBinString(byte B)
        {
            var Result    = new char[8];

            for (int i = 0; i < 8; i++)
            {
                var Temp  = B & 128;
                Result[i] = Temp == 0 ? '0' : '1';
                B         = (byte)(B << 1);
            }

            return Result;
        }
    }
}