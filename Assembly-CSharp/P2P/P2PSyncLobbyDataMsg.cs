using System;
using network;

namespace P2P
{
	// Token: 0x02000827 RID: 2087
	public class P2PSyncLobbyDataMsg : NetMsgFast, IP2PMessage
	{
		// Token: 0x060033AC RID: 13228 RVA: 0x000F570A File Offset: 0x000F3B0A
		public P2PSyncLobbyDataMsg()
		{
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x000F5712 File Offset: 0x000F3B12
		public P2PSyncLobbyDataMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8U;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x000F573F File Offset: 0x000F3B3F
		public override uint MsgID()
		{
			return 27U;
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x000F5743 File Offset: 0x000F3B43
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToClient;
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x000F5746 File Offset: 0x000F3B46
		public override void SerializeMsg()
		{
			base.Pack(this.isInMatch);
		}

		// Token: 0x060033B1 RID: 13233 RVA: 0x000F5754 File Offset: 0x000F3B54
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.isInMatch);
		}

		// Token: 0x0400240D RID: 9229
		public bool isInMatch;
	}
}
