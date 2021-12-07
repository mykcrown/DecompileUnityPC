// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace network
{
	public abstract class NetMsgFast : INetMsg
	{
		protected uint m_size;

		protected int m_offset;

		private int m_initializedBufferIndex = -1;

		protected byte[] m_msgbuffer = new byte[256];

		private byte[] m_resultsBuffer1 = new byte[1];

		private byte[] m_resultsBuffer2 = new byte[2];

		private byte[] m_resultsBuffer4 = new byte[4];

		private byte[] m_resultsBuffer8 = new byte[8];

		private byte[] m_resultsBuffer64 = new byte[64];

		private ASCIIEncoding encoder = new ASCIIEncoding();

		public uint MsgSize
		{
			get
			{
				return (uint)Mathf.Ceil(this.m_size / 8f);
			}
		}

		public byte[] MsgBuffer
		{
			get
			{
				return this.m_msgbuffer;
			}
		}

		public abstract void SerializeMsg();

		public abstract void DeserializeMsg();

		public void PackString64(string value)
		{
			byte b = (byte)value.Length;
			if (b > 64)
			{
				b = 64;
			}
			this.Pack(b, 7u);
			if (b > 0)
			{
				byte[] bytes = this.encoder.GetBytes(value);
				this.packBits(bytes, (uint)(b * 8));
			}
		}

		public void UnpackString64(ref string value)
		{
			byte b = 0;
			this.Unpack(ref b, 7u);
			if (b > 0)
			{
				this.unpackToResultBuffer((uint)(b * 8), this.m_resultsBuffer64);
				char[] chars = this.encoder.GetChars(this.m_resultsBuffer64, 0, (int)b);
				value = new string(chars);
			}
		}

		public void Pack(ulong value, uint bitCount)
		{
			this.packBits(BitConverter.GetBytes(value), bitCount);
		}

		public void Unpack(ref ulong value, uint bitCount)
		{
			this.unpackToResultBuffer(bitCount, this.m_resultsBuffer8);
			value = BitConverter.ToUInt64(this.m_resultsBuffer8, 0);
		}

		public void Pack(long value, uint bitCount)
		{
			this.packBits(BitConverter.GetBytes(value), bitCount);
		}

		public void Unpack(ref long value, uint bitCount)
		{
			this.unpackToResultBuffer(bitCount, this.m_resultsBuffer8);
			value = BitConverter.ToInt64(this.m_resultsBuffer8, 0);
		}

		public void Pack(ushort value, uint bitCount)
		{
			this.packBits(BitConverter.GetBytes(value), bitCount);
		}

		public void Unpack(ref ushort value, uint bitCount)
		{
			this.unpackToResultBuffer(bitCount, this.m_resultsBuffer2);
			value = BitConverter.ToUInt16(this.m_resultsBuffer2, 0);
		}

		public void Pack(short value, uint bitCount)
		{
			this.packBits(BitConverter.GetBytes(value), bitCount);
		}

		public void Unpack(ref short value, uint bitCount)
		{
			this.unpackToResultBuffer(bitCount, this.m_resultsBuffer2);
			value = BitConverter.ToInt16(this.m_resultsBuffer2, 0);
		}

		public void Pack(byte value, uint bitCount)
		{
			this.packBits(BitConverter.GetBytes((short)value), bitCount);
		}

		public void Unpack(ref byte value, uint bitCount)
		{
			this.unpackToResultBuffer(bitCount, this.m_resultsBuffer1);
			value = this.m_resultsBuffer1[0];
		}

		public void Pack(bool value)
		{
			this.packBits(BitConverter.GetBytes(value), 1u);
		}

		public void Unpack(ref bool value)
		{
			this.unpackToResultBuffer(1u, this.m_resultsBuffer1);
			value = Convert.ToBoolean(this.m_resultsBuffer1[0]);
		}

		public void Pack(uint value, uint bitCount)
		{
			this.packBits(BitConverter.GetBytes(value), bitCount);
		}

		public void Unpack(ref uint value, uint bitCount)
		{
			this.unpackToResultBuffer(bitCount, this.m_resultsBuffer4);
			value = BitConverter.ToUInt32(this.m_resultsBuffer4, 0);
		}

		public void Pack(int value, uint bitCount)
		{
			this.packBits(BitConverter.GetBytes(value), bitCount);
		}

		public void Unpack(ref int value, uint bitCount)
		{
			this.unpackToResultBuffer(bitCount, this.m_resultsBuffer4);
			value = BitConverter.ToInt32(this.m_resultsBuffer4, 0);
		}

		public void ResetBuffer()
		{
			this.m_size = 0u;
			this.m_offset = 0;
			this.m_initializedBufferIndex = -1;
		}

		private void unpackToResultBuffer(uint bitCount, byte[] resultBuffer)
		{
			for (int i = 0; i < resultBuffer.Length; i++)
			{
				resultBuffer[i] = 0;
			}
			int num = this.m_offset / 8;
			int num2 = this.m_offset % 8;
			int j = (int)bitCount;
			int num3 = 0;
			while (j > 0)
			{
				int num4 = 8;
				if (num4 > j)
				{
					num4 = j;
				}
				byte b = this.m_msgbuffer[num];
				num++;
				resultBuffer[num3] = (byte)(b >> num2);
				int num5 = num4 - (8 - num2);
				if (num5 > 0)
				{
					byte b2 = this.m_msgbuffer[num];
					byte b3 = (byte)(b2 << 8 - num2);
					resultBuffer[num3] |= b3;
				}
				if (j <= 8)
				{
					byte b4 = (byte)(255 >> 8 - j);
					resultBuffer[num3] = (b4 & resultBuffer[num3]);
				}
				j -= num4;
				num3++;
			}
			this.m_offset += (int)bitCount;
		}

		private void packBits(byte[] bytes, uint bitCount)
		{
			if (bytes == null || bytes.Length == 0)
			{
				return;
			}
			int num = this.m_offset % 8;
			int i = (int)bitCount;
			int num2 = this.m_offset / 8;
			if (this.m_initializedBufferIndex < num2)
			{
				this.m_msgbuffer[num2] = 0;
				this.m_initializedBufferIndex = num2;
			}
			int num3 = 0;
			while (i > 0)
			{
				int num4 = 8;
				if (num4 > i)
				{
					num4 = i;
				}
				byte b = bytes[num3];
				byte b2 = (byte)(b << num);
				this.m_msgbuffer[num2] = (this.m_msgbuffer[num2] | b2);
				num2++;
				if (this.m_initializedBufferIndex < num2)
				{
					this.m_msgbuffer[num2] = 0;
					this.m_initializedBufferIndex = num2;
				}
				int num5 = num4 - (8 - num);
				if (num5 > 0)
				{
					b2 = (byte)(b >> 8 - num);
					this.m_msgbuffer[num2] = (this.m_msgbuffer[num2] | b2);
					if (i <= 8)
					{
						byte b3 = (byte)(255 >> 8 - num5);
						this.m_msgbuffer[num2] = (b3 & this.m_msgbuffer[num2]);
					}
				}
				else if (num5 < 0)
				{
					byte b4 = (byte)(255 >> -num5);
					this.m_msgbuffer[num2 - 1] = (this.m_msgbuffer[num2 - 1] & b4);
				}
				i -= num4;
				num3++;
			}
			this.m_offset += (int)bitCount;
			this.m_size += bitCount;
		}

		public abstract uint MsgID();

		public virtual bool Serialize()
		{
			byte value = (byte)this.MsgID();
			this.Pack(value, 8u);
			this.SerializeMsg();
			return true;
		}

		public static string displayBits(byte[] arr, int len = -1)
		{
			int num;
			if (len == -1)
			{
				num = arr.Length;
			}
			else
			{
				num = Math.Min(len, arr.Length);
			}
			string text = string.Empty;
			for (int i = num - 1; i >= 0; i--)
			{
				text = text + NetMsgFast.displayBits(arr[i]) + " ";
			}
			return text;
		}

		public static string displayBits(byte value)
		{
			string text = string.Empty;
			for (int i = 0; i < 8; i++)
			{
				text = (int)(value & 1) + text;
				value = (byte)(value >> 1);
			}
			return text;
		}
	}
}
