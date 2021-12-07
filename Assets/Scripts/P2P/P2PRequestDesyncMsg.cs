// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace P2P
{
	public class P2PRequestDesyncMsg : NetMsgBase, IP2PMessage
	{
		public ushort desyncFrame;

		public P2PRequestDesyncMsg()
		{
		}

		public P2PRequestDesyncMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 24u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToClient;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.desyncFrame);
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.desyncFrame);
		}
	}
}
