// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace P2P
{
	public class P2PTimesyncResponseMsg : NetMsgBase, IP2PMessage
	{
		public long localTimeMs;

		public ulong senderSteamID;

		public P2PTimesyncResponseMsg()
		{
		}

		public P2PTimesyncResponseMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 10u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.senderSteamID);
			base.Pack(this.localTimeMs);
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.senderSteamID);
			base.Unpack(ref this.localTimeMs);
		}
	}
}
