// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace P2P
{
	public class P2PSendWinnerMsg : NetMsgBase, IP2PMessage
	{
		public byte reportedWinningTeams;

		public P2PSendWinnerMsg()
		{
		}

		public P2PSendWinnerMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 16u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToHost;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.reportedWinningTeams);
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.reportedWinningTeams);
		}
	}
}
