// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace P2P
{
	public class P2PForfeitMatchMsg : NetMsgBase, IP2PMessage
	{
		public ulong senderSteamID;

		public P2PForfeitMatchMsg()
		{
		}

		public P2PForfeitMatchMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 15u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToHost;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.senderSteamID);
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.senderSteamID);
		}
	}
}
