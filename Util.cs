
using System;

namespace ChaCha20Cipher
{
	public class Util
	{
		/// <summary>
		/// n-bit left rotation operation (towards the high bits) for 32-bit
		/// integers.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="c"></param>
		/// <returns>The result of (v LEFTSHIFT c)</returns>
		public static uint Rotate(uint v, int c)
		{
			unchecked
			{
				return (v << c) | (v >> (32 - c));
			}
		}

		public static uint XOr(uint v, uint w)
		{
			return unchecked(v ^ w);
		}

		public static uint Add(uint v, uint w)
		{
			return unchecked(v + w);
		}

	
		public static uint AddOne(uint v)
		{
			return unchecked(v + 1);
		}

	
		public static uint U8To32Little(byte[] p, int inputOffset)
		{
			unchecked
			{
				return ((uint)p[inputOffset]
					| ((uint)p[inputOffset + 1] << 8)
					| ((uint)p[inputOffset + 2] << 16)
					| ((uint)p[inputOffset + 3] << 24));
			}
		}

	
		public static void ToBytes(byte[] output, uint input, int outputOffset)
		{
			if (outputOffset < 0)
			{
				
			}

			unchecked
			{
				output[outputOffset] = (byte)input;
				output[outputOffset + 1] = (byte)(input >> 8);
				output[outputOffset + 2] = (byte)(input >> 16);
				output[outputOffset + 3] = (byte)(input >> 24);
			}
		}
	}
}
