// Decompile from assembly: Assembly-CSharp.dll

using network;
using P2P;
using System;

namespace BattleServer
{
	public class HashCodeMsg : NetMsgFast, IBufferable, IP2PMessage, IUDPMessage
	{
		public ushort frame;

		public short hashCode;

		public byte senderId;

		public HashCodeMsg()
		{
		}

		public HashCodeMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8u;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 2u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToHost;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.frame, 16u);
			base.Pack(this.hashCode, 16u);
			base.Pack(this.senderId, 4u);
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.frame, 16u);
			base.Unpack(ref this.hashCode, 16u);
			base.Unpack(ref this.senderId, 4u);
		}

		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize];
		}

		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			base.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}
	}
}
