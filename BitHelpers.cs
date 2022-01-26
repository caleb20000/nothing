using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ChaCha20Tests")]
namespace ChaCha20
{
    internal static class BitHelpers
    {
        /// <summary>
        /// Rotate to the left the bits of a 32-bit integer by shift bits.
        /// Overflowing bits are wrapped to the right.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        public static uint RotateLeft(uint n, int shift)
        {
            return (n << shift) | (n >> (32 - shift));
        }

        public static byte[] IntToLittleEndianBytes(uint[] integers)
        {
            byte[] bytes = new byte[integers.Length * 4];
            for (int i = 0; i < integers.Length; i++)
            {
                bytes[0 + i * 4] = (byte)(integers[i] & 0xFF);
                bytes[1 + i * 4] = (byte)((integers[i] >> 8) & 0xFF);
                bytes[2 + i * 4] = (byte)((integers[i] >> 16) & 0xFF);
                bytes[3 + i * 4] = (byte)((integers[i] >> 24) & 0xFF);
            }
            return bytes;
        }
        public static byte[] LongToLittleEndianBytes(ulong[] longs)
        {
            byte[] bytes = new byte[longs.Length * 8];
            for (int i = 0; i < longs.Length; i++)
            {
                bytes[0 + i * 8] = (byte)(longs[i] & 0xFF);
                bytes[1 + i * 8] = (byte)((longs[i] >> 8) & 0xFF);
                bytes[2 + i * 8] = (byte)((longs[i] >> 16) & 0xFF);
                bytes[3 + i * 8] = (byte)((longs[i] >> 24) & 0xFF);
                bytes[4 + i * 8] = (byte)((longs[i] >> 32) & 0xFF);
                bytes[5 + i * 8] = (byte)((longs[i] >> 40) & 0xFF);
                bytes[6 + i * 8] = (byte)((longs[i] >> 48) & 0xFF);
                bytes[7 + i * 8] = (byte)((longs[i] >> 56) & 0xFF);
            }
            return bytes;
        }

        public static uint[] LittleEndianBytesToIntegers(byte[] bytes)
        {
            uint[] integers = new uint[bytes.Length / 4];
            for (int i = 0; i < integers.Length; i++)
            {
                integers[i] = (uint)bytes[i * 4 + 0];
                integers[i] |= (uint)bytes[i * 4 + 1] << 8;
                integers[i] |= (uint)bytes[i * 4 + 2] << 16;
                integers[i] |= (uint)bytes[i * 4 + 3] << 24;
            }
            return integers;
        }

        public static byte[] ByteArrayXor(byte[] left, byte[] right)
        {
            int length = Math.Min(left.Length, right.Length);
            byte[] output = new byte[length];

            for (int i = 0; i < length; i++)
            {
                output[i] = (byte)(left[i] ^ right[i]);
            }
            return output;
        }

        /// <summary>
        /// Return a padding of 0 bytes to align the provided length to a multiple of 16.
        /// The function does not perform the concatenation of the byte arrays itself.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] Pad16Bytes(int length)
        {
            byte[] padding;
            if (length % 16 > 0)
                padding = new byte[16 - (length % 16)];
            else
                padding = new byte[0];
            return padding;
        }

        public static bool BytesAreEqualConstantTime(byte[] left, byte[] right)
        {
            int length = Math.Min(left.Length, right.Length);
            int accumulator = (left.Length - right.Length);
            for (int i = 0; i < length; i++)
            {
                accumulator |= (left[i] ^ right[i]);
            }
            return (accumulator == 0);
        }
    }
}