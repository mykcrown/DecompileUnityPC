using System;
using network;

namespace P2P
{
	// Token: 0x0200082C RID: 2092
	public class P2PTimesyncResponseMsg : NetMsgBase, IP2PMessage
	{
		// Token: 0x060033CF RID: 13263 RVA: 0x000F5926 File Offset: 0x000F3D26
		public P2PTimesyncResponseMsg()
		{
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x000F592E File Offset: 0x000F3D2E
		public P2PTimesyncResponseMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x000F5959 File Offset: 0x000F3D59
		public override uint MsgID()
		{
			return 10U;
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x000F595D File Offset: 0x000F3D5D
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x000F5960 File Offset: 0x000F3D60
		public override void SerializeMsg()
		{
			base.Pack(this.senderSteamID);
			base.Pack(this.localTimeMs);
		}

		// Token: 0x060033D4 RID: 13268 RVA: 0x000F597A File Offset: 0x000F3D7A
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.senderSteamID);
			base.Unpack(ref this.localTimeMs);
		}

		// Token: 0x04002413 RID: 9235
		public long localTimeMs;

		// Token: 0x04002414 RID: 9236
		public ulong senderSteamID;
	}
}
