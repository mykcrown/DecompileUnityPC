using System;
using network;

namespace P2P
{
	// Token: 0x02000826 RID: 2086
	public class P2PInformScreenChangedMsg : NetMsgFast, IP2PMessage
	{
		// Token: 0x060033A6 RID: 13222 RVA: 0x000F5692 File Offset: 0x000F3A92
		public P2PInformScreenChangedMsg()
		{
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x000F569A File Offset: 0x000F3A9A
		public P2PInformScreenChangedMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8U;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x000F56C7 File Offset: 0x000F3AC7
		public override uint MsgID()
		{
			return 18U;
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x000F56CB File Offset: 0x000F3ACB
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToHost;
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x000F56CE File Offset: 0x000F3ACE
		public override void SerializeMsg()
		{
			base.Pack(this.userID, 64U);
			base.Pack(this.screenID, 32U);
		}

		// Token: 0x060033AB RID: 13227 RVA: 0x000F56EC File Offset: 0x000F3AEC
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.userID, 64U);
			base.Unpack(ref this.screenID, 32U);
		}

		// Token: 0x0400240B RID: 9227
		public ulong userID;

		// Token: 0x0400240C RID: 9228
		public int screenID;
	}
}
