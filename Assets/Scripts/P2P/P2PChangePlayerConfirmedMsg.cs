// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace P2P
{
	public class P2PChangePlayerConfirmedMsg : NetMsgBase, IP2PMessage
	{
		public ulong userID;

		public bool isSpectating;

		public byte team;

		public P2PChangePlayerConfirmedMsg()
		{
		}

		public P2PChangePlayerConfirmedMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 25u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToClient;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.userID);
			base.Pack(this.isSpectating);
			base.Pack(this.team);
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.userID);
			base.Unpack(ref this.isSpectating);
			base.Unpack(ref this.team);
		}
	}
}
