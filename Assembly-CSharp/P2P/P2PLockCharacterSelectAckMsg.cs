using System;
using network;

namespace P2P
{
	// Token: 0x0200081C RID: 2076
	public class P2PLockCharacterSelectAckMsg : NetMsgBase, IP2PMessage
	{
		// Token: 0x0600336C RID: 13164 RVA: 0x000F4FDE File Offset: 0x000F33DE
		public P2PLockCharacterSelectAckMsg()
		{
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x000F4FE6 File Offset: 0x000F33E6
		public P2PLockCharacterSelectAckMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x000F5011 File Offset: 0x000F3411
		public override uint MsgID()
		{
			return 20U;
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x000F5015 File Offset: 0x000F3415
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToClient;
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x000F5018 File Offset: 0x000F3418
		public override void SerializeMsg()
		{
			base.Pack(this.steamID);
			base.Pack(this.accepted);
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x000F5032 File Offset: 0x000F3432
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.steamID);
			base.Unpack(ref this.accepted);
		}

		// Token: 0x040023F1 RID: 9201
		public ulong steamID;

		// Token: 0x040023F2 RID: 9202
		public bool accepted;
	}
}
