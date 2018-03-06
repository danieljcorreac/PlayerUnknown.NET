namespace PlayerUnknown.Helpers
{
    using System.Collections;

    public static class PubgReaderExt
    {
        /// <summary>
        /// Gets a bit from a byte at a specified offset.
        /// </summary>
        /// <param name="Byte">The byte.</param>
        /// <param name="Offset">The offset.</param>
        public static bool GetBit(this byte Byte, int Offset)
        {
            BitArray Bits = new BitArray(new byte[]
                {
                    Byte
                }
            );

            return Bits.Get(Offset);
        }
    }
}
