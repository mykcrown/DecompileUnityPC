using System;
using network;

namespace P2P
{
	// Token: 0x0200082A RID: 2090
	public class P2PRequestDesyncMsg : NetMsgBase, IP2PMessage
	{
		// Token: 0x060033C2 RID: 13250 RVA: 0x000F585A File Offset: 0x000F3C5A
		public P2PRequestDesyncMsg()
		{
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x000F5862 File Offset: 0x000F3C62
		public P2PRequestDesyncMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x000F588D File Offset: 0x000F3C8D
		public override uint MsgID()
		{
			return 24U;
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x000F5891 File Offset: 0x000F3C91
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToClient;
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x000F5894 File Offset: 0x000F3C94
		public override void SerializeMsg()
		{
			base.Pack(this.desyncFrame);
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x000F58A2 File Offset: 0x000F3CA2
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.desyncFrame);
		}

		// Token: 0x0400240F RID: 9231
		public ushort desyncFrame;
	}
}
