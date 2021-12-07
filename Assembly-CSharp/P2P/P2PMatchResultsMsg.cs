using System;
using BattleServer;
using network;

namespace P2P
{
	// Token: 0x02000822 RID: 2082
	public class P2PMatchResultsMsg : NetMsgBase, IP2PMessage
	{
		// Token: 0x0600338E RID: 13198 RVA: 0x000F5490 File Offset: 0x000F3890
		public P2PMatchResultsMsg()
		{
		}

		// Token: 0x0600338F RID: 13199 RVA: 0x000F5498 File Offset: 0x000F3898
		public P2PMatchResultsMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x000F54C3 File Offset: 0x000F38C3
		public override uint MsgID()
		{
			return 23U;
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x000F54C7 File Offset: 0x000F38C7
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToClient;
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x000F54CA File Offset: 0x000F38CA
		public override void SerializeMsg()
		{
			base.Pack(this.winningTeamBitMask);
			base.Pack((uint)this.reason);
		}

		// Token: 0x06003393 RID: 13203 RVA: 0x000F54E4 File Offset: 0x000F38E4
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.winningTeamBitMask);
			uint num = 4U;
			base.Unpack(ref num);
			this.reason = (EMatchResult)num;
		}

		// Token: 0x04002401 RID: 9217
		public byte winningTeamBitMask;

		// Token: 0x04002402 RID: 9218
		public EMatchResult reason;
	}
}
