using System;
using network;

namespace P2P
{
	// Token: 0x02000823 RID: 2083
	public class P2PChangePlayerConfirmedMsg : NetMsgBase, IP2PMessage
	{
		// Token: 0x06003394 RID: 13204 RVA: 0x000F550E File Offset: 0x000F390E
		public P2PChangePlayerConfirmedMsg()
		{
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x000F5516 File Offset: 0x000F3916
		public P2PChangePlayerConfirmedMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x000F5541 File Offset: 0x000F3941
		public override uint MsgID()
		{
			return 25U;
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x000F5545 File Offset: 0x000F3945
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToClient;
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x000F5548 File Offset: 0x000F3948
		public override void SerializeMsg()
		{
			base.Pack(this.userID);
			base.Pack(this.isSpectating);
			base.Pack(this.team);
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x000F556E File Offset: 0x000F396E
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.userID);
			base.Unpack(ref this.isSpectating);
			base.Unpack(ref this.team);
		}

		// Token: 0x04002403 RID: 9219
		public ulong userID;

		// Token: 0x04002404 RID: 9220
		public bool isSpectating;

		// Token: 0x04002405 RID: 9221
		public byte team;
	}
}
