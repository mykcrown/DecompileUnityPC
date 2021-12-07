using System;
using System.Text;

namespace network
{
	// Token: 0x020007C1 RID: 1985
	public abstract class NetMsgBase : INetMsg
	{
		// Token: 0x06003110 RID: 12560
		public abstract uint MsgID();

		// Token: 0x06003111 RID: 12561 RVA: 0x000ED460 File Offset: 0x000EB860
		public virtual string MsgName()
		{
			return base.GetType().Name;
		}

		// Token: 0x06003112 RID: 12562
		public abstract void SerializeMsg();

		// Token: 0x06003113 RID: 12563
		public abstract void DeserializeMsg();

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06003114 RID: 12564 RVA: 0x000ED46D File Offset: 0x000EB86D
		public byte[] MsgBuffer
		{
			get
			{
				return this.m_msgbuffer;
			}
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06003115 RID: 12565 RVA: 0x000ED475 File Offset: 0x000EB875
		public uint MsgSize
		{
			get
			{
				return this.m_msgSize;
			}
		}

		// Token: 0x06003116 RID: 12566 RVA: 0x000ED47D File Offset: 0x000EB87D
		public virtual void ResetBuffer()
		{
			this.m_msgSize = 0U;
			this.m_msgOffset = 0;
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x000ED490 File Offset: 0x000EB890
		public void PackFixedBytes(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
			{
				return;
			}
			if (this.m_gettingSize)
			{
				this.m_msgSize += (uint)bytes.Length;
				return;
			}
			Buffer.BlockCopy(bytes, 0, this.m_msgbuffer, this.m_msgOffset, bytes.Length);
			this.m_msgOffset += bytes.Length;
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x000ED4EE File Offset: 0x000EB8EE
		public void Pack(bool val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		// Token: 0x06003119 RID: 12569 RVA: 0x000ED4FC File Offset: 0x000EB8FC
		public void Pack(byte val)
		{
			this.PackFixedBytes(new byte[]
			{
				val
			});
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x000ED50E File Offset: 0x000EB90E
		public void Pack(ushort val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x000ED51C File Offset: 0x000EB91C
		public void Pack(short val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x000ED52A File Offset: 0x000EB92A
		public void Pack(uint val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x000ED538 File Offset: 0x000EB938
		public void Pack(int val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x000ED546 File Offset: 0x000EB946
		public void Pack(long val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x000ED554 File Offset: 0x000EB954
		public void Pack(ulong val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x000ED562 File Offset: 0x000EB962
		public void Pack(float val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x000ED570 File Offset: 0x000EB970
		public void Pack(double val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x000ED580 File Offset: 0x000EB980
		public void Pack(string val)
		{
			ASCIIEncoding asciiencoding = new ASCIIEncoding();
			if (this.m_gettingSize)
			{
				this.m_msgSize += 4U;
				this.m_msgSize += (uint)asciiencoding.GetByteCount(val);
				return;
			}
			uint length = (uint)val.Length;
			this.Pack(length);
			if (length > 0U)
			{
				byte[] bytes = asciiencoding.GetBytes(val);
				Buffer.BlockCopy(bytes, 0, this.m_msgbuffer, this.m_msgOffset, bytes.Length);
				this.m_msgOffset += bytes.Length;
			}
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x000ED604 File Offset: 0x000EBA04
		public void Pack(Guid val)
		{
			this.PackFixedBytes(val.ToByteArray());
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x000ED614 File Offset: 0x000EBA14
		public void PackByteArrayBuffered(byte[] byteArray, uint byteSize)
		{
			if (this.m_gettingSize)
			{
				this.m_msgSize += 4U;
				if (byteArray != null)
				{
					this.m_msgSize += byteSize;
				}
				return;
			}
			uint num = 0U;
			if (byteArray != null)
			{
				num = byteSize;
			}
			this.Pack(num);
			if (num != 0U)
			{
				Buffer.BlockCopy(byteArray, 0, this.m_msgbuffer, this.m_msgOffset, (int)num);
				this.m_msgOffset += (int)num;
			}
		}

		// Token: 0x06003125 RID: 12581 RVA: 0x000ED688 File Offset: 0x000EBA88
		public void PackByteArray(byte[] byteArray)
		{
			if (this.m_gettingSize)
			{
				this.m_msgSize += 4U;
				if (byteArray != null)
				{
					this.m_msgSize += (uint)byteArray.Length;
				}
				return;
			}
			uint num = 0U;
			if (byteArray != null)
			{
				num = (uint)byteArray.Length;
			}
			this.Pack(num);
			if (num != 0U)
			{
				Buffer.BlockCopy(byteArray, 0, this.m_msgbuffer, this.m_msgOffset, (int)num);
				this.m_msgOffset += (int)num;
			}
		}

		// Token: 0x06003126 RID: 12582 RVA: 0x000ED700 File Offset: 0x000EBB00
		public void Unpack(ref bool val)
		{
			val = BitConverter.ToBoolean(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset++;
		}

		// Token: 0x06003127 RID: 12583 RVA: 0x000ED724 File Offset: 0x000EBB24
		public void Unpack(ref byte val)
		{
			byte[] array = new byte[1];
			Buffer.BlockCopy(this.m_msgbuffer, this.m_msgOffset, array, 0, 1);
			val = array[0];
			this.m_msgOffset++;
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x000ED75F File Offset: 0x000EBB5F
		public void Unpack(ref ushort val)
		{
			val = BitConverter.ToUInt16(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset += 2;
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x000ED782 File Offset: 0x000EBB82
		public void Unpack(ref short val)
		{
			val = BitConverter.ToInt16(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset += 2;
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x000ED7A5 File Offset: 0x000EBBA5
		public void Unpack(ref uint val)
		{
			val = BitConverter.ToUInt32(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset += 4;
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x000ED7C8 File Offset: 0x000EBBC8
		public void Unpack(ref int val)
		{
			val = BitConverter.ToInt32(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset += 4;
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x000ED7EB File Offset: 0x000EBBEB
		public void Unpack(ref ulong val)
		{
			val = BitConverter.ToUInt64(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset += 8;
		}

		// Token: 0x0600312D RID: 12589 RVA: 0x000ED80E File Offset: 0x000EBC0E
		public void Unpack(ref long val)
		{
			val = BitConverter.ToInt64(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset += 8;
		}

		// Token: 0x0600312E RID: 12590 RVA: 0x000ED831 File Offset: 0x000EBC31
		public void Unpack(ref float val)
		{
			val = BitConverter.ToSingle(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset += 4;
		}

		// Token: 0x0600312F RID: 12591 RVA: 0x000ED854 File Offset: 0x000EBC54
		public void Unpack(ref double val)
		{
			val = BitConverter.ToDouble(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset += 8;
		}

		// Token: 0x06003130 RID: 12592 RVA: 0x000ED878 File Offset: 0x000EBC78
		public void Unpack(ref string val)
		{
			uint num = 0U;
			this.Unpack(ref num);
			ASCIIEncoding asciiencoding = new ASCIIEncoding();
			char[] array = new char[num];
			asciiencoding.GetChars(this.m_msgbuffer, this.m_msgOffset, (int)num, array, 0);
			this.m_msgOffset += (int)num;
			val = new string(array);
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x000ED8CC File Offset: 0x000EBCCC
		public void Unpack(ref Guid val)
		{
			int num = val.ToByteArray().Length;
			byte[] array = new byte[num];
			Buffer.BlockCopy(this.m_msgbuffer, this.m_msgOffset, array, 0, num);
			this.m_msgOffset += num;
			val = new Guid(array);
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x000ED914 File Offset: 0x000EBD14
		public void UnpackByteArray(ref byte[] byteArray)
		{
			uint num = 0U;
			this.Unpack(ref num);
			if (num != 0U)
			{
				byteArray = new byte[num];
				Buffer.BlockCopy(this.m_msgbuffer, this.m_msgOffset, byteArray, 0, (int)num);
				this.m_msgOffset += (int)num;
			}
		}

		// Token: 0x06003133 RID: 12595 RVA: 0x000ED960 File Offset: 0x000EBD60
		public void UnpackByteArrayBuffered(ref byte[] byteArray)
		{
			uint num = 0U;
			this.Unpack(ref num);
			if (num != 0U)
			{
				uint num2 = (uint)(this.m_msgOffset + (int)num);
				if ((ulong)num2 <= (ulong)((long)byteArray.Length) && (ulong)num2 <= (ulong)((long)this.m_msgbuffer.Length))
				{
					Buffer.BlockCopy(this.m_msgbuffer, this.m_msgOffset, byteArray, 0, (int)num);
					this.m_msgOffset += (int)num;
				}
			}
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x000ED9C8 File Offset: 0x000EBDC8
		public bool Serialize()
		{
			if (this.m_reusable != (this.m_msgbuffer.Length != 0))
			{
				return false;
			}
			this.m_gettingSize = true;
			this.SerializeMsg();
			this.m_gettingSize = false;
			byte value = (byte)this.MsgID();
			uint num = 1U;
			this.m_msgSize += num;
			byte[] bytes = BitConverter.GetBytes((short)value);
			if (!this.m_reusable)
			{
				this.m_msgbuffer = new byte[this.m_msgSize];
			}
			Buffer.BlockCopy(bytes, 0, this.m_msgbuffer, this.m_msgOffset, (int)num);
			this.m_msgOffset = (int)num;
			this.SerializeMsg();
			return true;
		}

		// Token: 0x0400228B RID: 8843
		public static readonly int skInvalidMsgId;

		// Token: 0x0400228C RID: 8844
		protected byte[] m_msgbuffer = new byte[0];

		// Token: 0x0400228D RID: 8845
		protected uint m_msgSize;

		// Token: 0x0400228E RID: 8846
		protected int m_msgOffset;

		// Token: 0x0400228F RID: 8847
		protected bool m_reusable;

		// Token: 0x04002290 RID: 8848
		private bool m_gettingSize;
	}
}
