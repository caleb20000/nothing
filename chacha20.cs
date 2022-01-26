

using System;
using System.Text;

namespace ChaCha20Cipher
{
	public sealed class ChaCha20Cipher : IDisposable
	{

		private uint[] state;
		private bool isDisposed;
		public ChaCha20Cipher(byte[] key, byte[] nonce, uint counter)
		{
			this.state = new uint[16];
			this.isDisposed = false;

			KeySetup(key);
			IvSetup(nonce, counter);
		}

		/// <summary>
		/// The ChaCha20 state (aka "context"). Read-Only.
		/// </summary>
		public uint[] State
		{
			get
			{
				return this.state;
			}
		}


		private void KeySetup(byte[] key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("Key is null");
			}
			if (key.Length != 32)
			{
				
			}

	
			byte[] sigma = Encoding.ASCII.GetBytes("expand 32-byte k");
			byte[] tau = Encoding.ASCII.GetBytes("expand 16-byte k");

			state[4] = Util.U8To32Little(key, 0);
			state[5] = Util.U8To32Little(key, 4);
			state[6] = Util.U8To32Little(key, 8);
			state[7] = Util.U8To32Little(key, 12);

			byte[] constants = (key.Length == 32) ? sigma : tau;
			int keyIndex = key.Length - 16;

			state[8] = Util.U8To32Little(key, keyIndex + 0);
			state[9] = Util.U8To32Little(key, keyIndex + 4);
			state[10] = Util.U8To32Little(key, keyIndex + 8);
			state[11] = Util.U8To32Little(key, keyIndex + 12);

			state[0] = Util.U8To32Little(constants, 0);
			state[1] = Util.U8To32Little(constants, 4);
			state[2] = Util.U8To32Little(constants, 8);
			state[3] = Util.U8To32Little(constants, 12);
		}

		private void IvSetup(byte[] nonce, uint counter)
		{
			if (nonce == null)
			{
				// There has already been some state set up. Clear it before exiting.
				Dispose();
				
			}
			if (nonce.Length != 12)
			{
				// There has already been some state set up. Clear it before exiting.
				Dispose();
				
			}

			state[12] = counter;
			state[13] = Util.U8To32Little(nonce, 0);
			state[14] = Util.U8To32Little(nonce, 4);
			state[15] = Util.U8To32Little(nonce, 8);
		}


		public void EncryptBytes(byte[] output, byte[] input, int numBytes)
		{
			if (isDisposed)
			{
				
			}
			if (numBytes < 0 || numBytes > input.Length)
			{
				
			}

			uint[] x = new uint[16];    // Working buffer
			byte[] tmp = new byte[64];  // Temporary buffer
			int outputOffset = 0;
			int inputOffset = 0;

			while (numBytes > 0)
			{
				for (int i = 16; i-- > 0;)
				{
					x[i] = this.state[i];
				}

				for (int i = 20; i > 0; i -= 2)
				{
					QuarterRound(x, 0, 4, 8, 12);
					QuarterRound(x, 1, 5, 9, 13);
					QuarterRound(x, 2, 6, 10, 14);
					QuarterRound(x, 3, 7, 11, 15);
					QuarterRound(x, 0, 5, 10, 15);
					QuarterRound(x, 1, 6, 11, 12);
					QuarterRound(x, 2, 7, 8, 13);
					QuarterRound(x, 3, 4, 9, 14);
				}

				for (int i = 16; i-- > 0;)
				{
					Util.ToBytes(tmp, Util.Add(x[i], this.state[i]), 4 * i);
				}

				this.state[12] = Util.AddOne(state[12]);
				if (this.state[12] <= 0)
				{
					/* Stopping at 2^70 bytes per nonce is the user's responsibility */
					this.state[13] = Util.AddOne(state[13]);
				}

				if (numBytes <= 64)
				{
					for (int i = numBytes; i-- > 0;)
					{
						output[i + outputOffset] = (byte)(input[i + inputOffset] ^ tmp[i]);
					}

					return;
				}

				for (int i = 64; i-- > 0;)
				{
					output[i + outputOffset] = (byte)(input[i + inputOffset] ^ tmp[i]);
				}

				numBytes -= 64;
				outputOffset += 64;
				inputOffset += 64;
			}
		}

		public static void QuarterRound(uint[] x, uint a, uint b, uint c, uint d)
		{
			if (x == null)
			{
			
			}
			if (x.Length != 16)
			{
				throw new ArgumentException();
			}

			x[a] = Util.Add(x[a], x[b]); x[d] = Util.Rotate(Util.XOr(x[d], x[a]), 16);
			x[c] = Util.Add(x[c], x[d]); x[b] = Util.Rotate(Util.XOr(x[b], x[c]), 12);
			x[a] = Util.Add(x[a], x[b]); x[d] = Util.Rotate(Util.XOr(x[d], x[a]), 8);
			x[c] = Util.Add(x[c], x[d]); x[b] = Util.Rotate(Util.XOr(x[b], x[c]), 7);
		}


		public static void ChaCha20BlockFunction(byte[] output, uint[] input)
		{
			if (input == null || output == null)
			{
				throw new ArgumentNullException();
			}
			if (input.Length != 16 || output.Length != 64)
			{
				throw new ArgumentException();
			}

			uint[] x = new uint[16];  // Working buffer

			for (int i = 16; i-- > 0;)
			{
				x[i] = input[i];
			}

			for (int i = 20; i > 0; i -= 2)
			{
				QuarterRound(x, 0, 4, 8, 12);
				QuarterRound(x, 1, 5, 9, 13);
				QuarterRound(x, 2, 6, 10, 14);
				QuarterRound(x, 3, 7, 11, 15);

				QuarterRound(x, 0, 5, 10, 15);
				QuarterRound(x, 1, 6, 11, 12);
				QuarterRound(x, 2, 7, 8, 13);
				QuarterRound(x, 3, 4, 9, 14);
			}

			for (int i = 16; i-- > 0;)
			{
				Util.ToBytes(output, Util.Add(x[i], input[i]), 4 * i);
			}
		}

		#region Destructor and Disposer

		~ChaCha20Cipher()
		{
			Dispose(false);
		}

		
		public void Dispose()
		{
			Dispose(true);
			
			GC.SuppressFinalize(this);
		}

		
		private void Dispose(bool disposing)
		{
			if (!isDisposed)
			{
				if (disposing)
				{
					/* Cleanup managed objects by calling their Dispose() methods */
				}

				/* Cleanup any unmanaged objects here */
				if (state != null)
				{
					Array.Clear(state, 0, state.Length);
				}

				state = null;
			}

			isDisposed = true;
		}

		#endregion
	}
}
