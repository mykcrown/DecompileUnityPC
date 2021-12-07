using System;
using network;

namespace P2P
{
	// Token: 0x02000824 RID: 2084
	public class P2PConfirmScreenChangedMsg : NetMsgFast, IP2PMessage
	{
		// Token: 0x0600339A RID: 13210 RVA: 0x000F5594 File Offset: 0x000F3994
		public P2PConfirmScreenChangedMsg()
		{
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x000F559C File Offset: 0x000F399C
		public P2PConfirmScreenChangedMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8U;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x000F55C9 File Offset: 0x000F39C9
		public override uint MsgID()
		{
			return 26U;
		}

		// Token: 0x0600339D RID: 13213 RVA: 0x000F55CD File Offset: 0x000F39CD
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToClient;
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x000F55D0 File Offset: 0x000F39D0
		public override void SerializeMsg()
		{
			base.Pack(this.userID, 64U);
			base.Pack(this.screenID, 32U);
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x000F55EE File Offset: 0x000F39EE
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.userID, 64U);
			base.Unpack(ref this.screenID, 32U);
		}

		// Token: 0x04002406 RID: 9222
		public ulong userID;

		// Token: 0x04002407 RID: 9223
		public int screenID;
	}
}
