// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using network;
using System;

namespace P2P
{
	public class P2PMatchResultsMsg : NetMsgBase, IP2PMessage
	{
		public byte winningTeamBitMask;

		public EMatchResult reason;

		public P2PMatchResultsMsg()
		{
		}

		public P2PMatchResultsMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 23u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToClient;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.winningTeamBitMask);
			base.Pack((uint)this.reason);
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.winningTeamBitMask);
			uint num = 4u;
			base.Unpack(ref num);
			this.reason = (EMatchResult)num;
		}
	}
}
