using System;
using network;

namespace P2P
{
	// Token: 0x02000821 RID: 2081
	public class P2PSendWinnerMsg : NetMsgBase, IP2PMessage
	{
		// Token: 0x06003388 RID: 13192 RVA: 0x000F543A File Offset: 0x000F383A
		public P2PSendWinnerMsg()
		{
		}

		// Token: 0x06003389 RID: 13193 RVA: 0x000F5442 File Offset: 0x000F3842
		public P2PSendWinnerMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x0600338A RID: 13194 RVA: 0x000F546D File Offset: 0x000F386D
		public override uint MsgID()
		{
			return 16U;
		}

		// Token: 0x0600338B RID: 13195 RVA: 0x000F5471 File Offset: 0x000F3871
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToHost;
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x000F5474 File Offset: 0x000F3874
		public override void SerializeMsg()
		{
			base.Pack(this.reportedWinningTeams);
		}

		// Token: 0x0600338D RID: 13197 RVA: 0x000F5482 File Offset: 0x000F3882
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.reportedWinningTeams);
		}

		// Token: 0x04002400 RID: 9216
		public byte reportedWinningTeams;
	}
}
