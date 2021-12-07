using System;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace network
{
	// Token: 0x020007C3 RID: 1987
	public abstract class NetMsgFast : INetMsg
	{
		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x0600313B RID: 12603 RVA: 0x000EDEBE File Offset: 0x000EC2BE
		public uint MsgSize
		{
			[CompilerGenerated]
			get
			{
				return (uint)Mathf.Ceil(this.m_size / 8f);
			}
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x0600313C RID: 12604 RVA: 0x000EDED4 File Offset: 0x000EC2D4
		public byte[] MsgBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.m_msgbuffer;
			}
		}

		// Token: 0x0600313D RID: 12605
		public abstract void SerializeMsg();

		// Token: 0x0600313E RID: 12606
		public abstract void DeserializeMsg();

		// Token: 0x0600313F RID: 12607 RVA: 0x000EDEDC File Offset: 0x000EC2DC
		public void PackString64(string value)
		{
			byte b = (byte)value.Length;
			if (b > 64)
			{
				b = 64;
			}
			this.Pack(b, 7U);
			if (b > 0)
			{
				byte[] bytes = this.encoder.GetBytes(value);
				this.packBits(bytes, (uint)(b * 8));
			}
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x000EDF24 File Offset: 0x000EC324
		public void UnpackString64(ref string value)
		{
			byte b = 0;
			this.Unpack(ref b, 7U);
			if (b > 0)
			{
				this.unpackToResultBuffer((uint)(b * 8), this.m_resultsBuffer64);
				char[] chars = this.encoder.GetChars(this.m_resultsBuffer64, 0, (int)b);
				value = new string(chars);
			}
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x000EDF6E File Offset: 0x000EC36E
		public void Pack(ulong value, uint bitCount)
		{
			this.packBits(BitConverter.GetBytes(value), bitCount);
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x000EDF7D File Offset: 0x000EC37D
		public void Unpack(ref ulong value, uint bitCount)
		{
			this.unpackToResultBuffer(bitCount, this.m_resultsBuffer8);
			value = BitConverter.ToUInt64(this.m_resultsBuffer8, 0);
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x000EDF9A File Offset: 0x000EC39A
		public void Pack(long value, uint bitCount)
		{
			this.packBits(BitConverter.GetBytes(value), bitCount);
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x000EDFA9 File Offset: 0x000EC3A9
		public void Unpack(ref long value, uint bitCount)
		{
			this.unpackToResultBuffer(bitCount, this.m_resultsBuffer8);
			value = BitConverter.ToInt64(this.m_resultsBuffer8, 0);
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x000EDFC6 File Offset: 0x000EC3C6
		public void Pack(ushort value, uint bitCount)
		{
			this.packBits(BitConverter.GetBytes(value), bitCount);
		}

		// Token: 0x06003146 RID: 12614 RVA: 0x000EDFD5 File Offset: 0x000EC3D5
		public void Unpack(ref ushort value, uint bitCount)
		{
			this.unpackToResultBuffer(bitCount, this.m_resultsBuffer2);
			value = BitConverter.ToUInt16(this.m_resultsBuffer2, 0);
		}

		// Token: 0x06003147 RID: 12615 RVA: 0x000EDFF2 File Offset: 0x000EC3F2
		public void Pack(short value, uint bitCount)
		{
			this.packBits(BitConverter.GetBytes(value), bitCount);
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x000EE001 File Offset: 0x000EC401
		public void Unpack(ref short value, uint bitCount)
		{
			this.unpackToResultBuffer(bitCount, this.m_resultsBuffer2);
			value = BitConverter.ToInt16(this.m_resultsBuffer2, 0);
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x000EE01E File Offset: 0x000EC41E
		public void Pack(byte value, uint bitCount)
		{
			this.packBits(BitConverter.GetBytes((short)value), bitCount);
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x000EE02D File Offset: 0x000EC42D
		public void Unpack(ref byte value, uint bitCount)
		{
			this.unpackToResultBuffer(bitCount, this.m_resultsBuffer1);
			value = this.m_resultsBuffer1[0];
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x000EE046 File Offset: 0x000EC446
		public void Pack(bool value)
		{
			this.packBits(BitConverter.GetBytes(value), 1U);
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x000EE055 File Offset: 0x000EC455
		public void Unpack(ref bool value)
		{
			this.unpackToResultBuffer(1U, this.m_resultsBuffer1);
			value = Convert.ToBoolean(this.m_resultsBuffer1[0]);
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x000EE073 File Offset: 0x000EC473
		public void Pack(uint value, uint bitCount)
		{
			this.packBits(BitConverter.GetBytes(value), bitCount);
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x000EE082 File Offset: 0x000EC482
		public void Unpack(ref uint value, uint bitCount)
		{
			this.unpackToResultBuffer(bitCount, this.m_resultsBuffer4);
			value = BitConverter.ToUInt32(this.m_resultsBuffer4, 0);
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x000EE09F File Offset: 0x000EC49F
		public void Pack(int value, uint bitCount)
		{
			this.packBits(BitConverter.GetBytes(value), bitCount);
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x000EE0AE File Offset: 0x000EC4AE
		public void Unpack(ref int value, uint bitCount)
		{
			this.unpackToResultBuffer(bitCount, this.m_resultsBuffer4);
			value = BitConverter.ToInt32(this.m_resultsBuffer4, 0);
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x000EE0CB File Offset: 0x000EC4CB
		public void ResetBuffer()
		{
			this.m_size = 0U;
			this.m_offset = 0;
			this.m_initializedBufferIndex = -1;
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x000EE0E4 File Offset: 0x000EC4E4
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

		// Token: 0x06003153 RID: 12627 RVA: 0x000EE1C4 File Offset: 0x000EC5C4
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

		// Token: 0x06003154 RID: 12628
		public abstract uint MsgID();

		// Token: 0x06003155 RID: 12629 RVA: 0x000EE324 File Offset: 0x000EC724
		public virtual bool Serialize()
		{
			byte value = (byte)this.MsgID();
			this.Pack(value, 8U);
			this.SerializeMsg();
			return true;
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x000EE348 File Offset: 0x000EC748
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

		// Token: 0x06003157 RID: 12631 RVA: 0x000EE3A0 File Offset: 0x000EC7A0
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

		// Token: 0x04002291 RID: 8849
		protected uint m_size;

		// Token: 0x04002292 RID: 8850
		protected int m_offset;

		// Token: 0x04002293 RID: 8851
		private int m_initializedBufferIndex = -1;

		// Token: 0x04002294 RID: 8852
		protected byte[] m_msgbuffer = new byte[256];

		// Token: 0x04002295 RID: 8853
		private byte[] m_resultsBuffer1 = new byte[1];

		// Token: 0x04002296 RID: 8854
		private byte[] m_resultsBuffer2 = new byte[2];

		// Token: 0x04002297 RID: 8855
		private byte[] m_resultsBuffer4 = new byte[4];

		// Token: 0x04002298 RID: 8856
		private byte[] m_resultsBuffer8 = new byte[8];

		// Token: 0x04002299 RID: 8857
		private byte[] m_resultsBuffer64 = new byte[64];

		// Token: 0x0400229A RID: 8858
		private ASCIIEncoding encoder = new ASCIIEncoding();
	}
}
