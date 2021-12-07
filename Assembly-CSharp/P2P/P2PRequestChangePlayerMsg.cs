using System;
using network;

namespace P2P
{
	// Token: 0x02000825 RID: 2085
	public class P2PRequestChangePlayerMsg : NetMsgBase, IP2PMessage
	{
		// Token: 0x060033A0 RID: 13216 RVA: 0x000F560C File Offset: 0x000F3A0C
		public P2PRequestChangePlayerMsg()
		{
		}

		// Token: 0x060033A1 RID: 13217 RVA: 0x000F5614 File Offset: 0x000F3A14
		public P2PRequestChangePlayerMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x060033A2 RID: 13218 RVA: 0x000F563F File Offset: 0x000F3A3F
		public override uint MsgID()
		{
			return 17U;
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x000F5643 File Offset: 0x000F3A43
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToHost;
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x000F5646 File Offset: 0x000F3A46
		public override void SerializeMsg()
		{
			base.Pack(this.userID);
			base.Pack(this.isSpectating);
			base.Pack(this.team);
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x000F566C File Offset: 0x000F3A6C
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.userID);
			base.Unpack(ref this.isSpectating);
			base.Unpack(ref this.team);
		}

		// Token: 0x04002408 RID: 9224
		public ulong userID;

		// Token: 0x04002409 RID: 9225
		public bool isSpectating;

		// Token: 0x0400240A RID: 9226
		public byte team;
	}
}
