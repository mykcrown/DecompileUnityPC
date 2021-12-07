// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Text;

namespace network
{
	public abstract class NetMsgBase : INetMsg
	{
		public static readonly int skInvalidMsgId;

		protected byte[] m_msgbuffer = new byte[0];

		protected uint m_msgSize;

		protected int m_msgOffset;

		protected bool m_reusable;

		private bool m_gettingSize;

		public byte[] MsgBuffer
		{
			get
			{
				return this.m_msgbuffer;
			}
		}

		public uint MsgSize
		{
			get
			{
				return this.m_msgSize;
			}
		}

		public abstract uint MsgID();

		public virtual string MsgName()
		{
			return base.GetType().Name;
		}

		public abstract void SerializeMsg();

		public abstract void DeserializeMsg();

		public virtual void ResetBuffer()
		{
			this.m_msgSize = 0u;
			this.m_msgOffset = 0;
		}

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

		public void Pack(bool val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		public void Pack(byte val)
		{
			this.PackFixedBytes(new byte[]
			{
				val
			});
		}

		public void Pack(ushort val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		public void Pack(short val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		public void Pack(uint val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		public void Pack(int val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		public void Pack(long val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		public void Pack(ulong val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		public void Pack(float val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		public void Pack(double val)
		{
			this.PackFixedBytes(BitConverter.GetBytes(val));
		}

		public void Pack(string val)
		{
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			if (this.m_gettingSize)
			{
				this.m_msgSize += 4u;
				this.m_msgSize += (uint)aSCIIEncoding.GetByteCount(val);
				return;
			}
			uint length = (uint)val.Length;
			this.Pack(length);
			if (length > 0u)
			{
				byte[] bytes = aSCIIEncoding.GetBytes(val);
				Buffer.BlockCopy(bytes, 0, this.m_msgbuffer, this.m_msgOffset, bytes.Length);
				this.m_msgOffset += bytes.Length;
			}
		}

		public void Pack(Guid val)
		{
			this.PackFixedBytes(val.ToByteArray());
		}

		public void PackByteArrayBuffered(byte[] byteArray, uint byteSize)
		{
			if (this.m_gettingSize)
			{
				this.m_msgSize += 4u;
				if (byteArray != null)
				{
					this.m_msgSize += byteSize;
				}
				return;
			}
			uint num = 0u;
			if (byteArray != null)
			{
				num = byteSize;
			}
			this.Pack(num);
			if (num != 0u)
			{
				Buffer.BlockCopy(byteArray, 0, this.m_msgbuffer, this.m_msgOffset, (int)num);
				this.m_msgOffset += (int)num;
			}
		}

		public void PackByteArray(byte[] byteArray)
		{
			if (this.m_gettingSize)
			{
				this.m_msgSize += 4u;
				if (byteArray != null)
				{
					this.m_msgSize += (uint)byteArray.Length;
				}
				return;
			}
			uint num = 0u;
			if (byteArray != null)
			{
				num = (uint)byteArray.Length;
			}
			this.Pack(num);
			if (num != 0u)
			{
				Buffer.BlockCopy(byteArray, 0, this.m_msgbuffer, this.m_msgOffset, (int)num);
				this.m_msgOffset += (int)num;
			}
		}

		public void Unpack(ref bool val)
		{
			val = BitConverter.ToBoolean(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset++;
		}

		public void Unpack(ref byte val)
		{
			byte[] array = new byte[1];
			Buffer.BlockCopy(this.m_msgbuffer, this.m_msgOffset, array, 0, 1);
			val = array[0];
			this.m_msgOffset++;
		}

		public void Unpack(ref ushort val)
		{
			val = BitConverter.ToUInt16(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset += 2;
		}

		public void Unpack(ref short val)
		{
			val = BitConverter.ToInt16(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset += 2;
		}

		public void Unpack(ref uint val)
		{
			val = BitConverter.ToUInt32(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset += 4;
		}

		public void Unpack(ref int val)
		{
			val = BitConverter.ToInt32(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset += 4;
		}

		public void Unpack(ref ulong val)
		{
			val = BitConverter.ToUInt64(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset += 8;
		}

		public void Unpack(ref long val)
		{
			val = BitConverter.ToInt64(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset += 8;
		}

		public void Unpack(ref float val)
		{
			val = BitConverter.ToSingle(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset += 4;
		}

		public void Unpack(ref double val)
		{
			val = BitConverter.ToDouble(this.m_msgbuffer, this.m_msgOffset);
			this.m_msgOffset += 8;
		}

		public void Unpack(ref string val)
		{
			uint num = 0u;
			this.Unpack(ref num);
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			char[] array = new char[num];
			aSCIIEncoding.GetChars(this.m_msgbuffer, this.m_msgOffset, (int)num, array, 0);
			this.m_msgOffset += (int)num;
			val = new string(array);
		}

		public void Unpack(ref Guid val)
		{
			int num = val.ToByteArray().Length;
			byte[] array = new byte[num];
			Buffer.BlockCopy(this.m_msgbuffer, this.m_msgOffset, array, 0, num);
			this.m_msgOffset += num;
			val = new Guid(array);
		}

		public void UnpackByteArray(ref byte[] byteArray)
		{
			uint num = 0u;
			this.Unpack(ref num);
			if (num != 0u)
			{
				byteArray = new byte[num];
				Buffer.BlockCopy(this.m_msgbuffer, this.m_msgOffset, byteArray, 0, (int)num);
				this.m_msgOffset += (int)num;
			}
		}

		public void UnpackByteArrayBuffered(ref byte[] byteArray)
		{
			uint num = 0u;
			this.Unpack(ref num);
			if (num != 0u)
			{
				uint num2 = (uint)(this.m_msgOffset + (int)num);
				if ((ulong)num2 <= (ulong)((long)byteArray.Length) && (ulong)num2 <= (ulong)((long)this.m_msgbuffer.Length))
				{
					Buffer.BlockCopy(this.m_msgbuffer, this.m_msgOffset, byteArray, 0, (int)num);
					this.m_msgOffset += (int)num;
				}
			}
		}

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
			uint num = 1u;
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
	}
}
