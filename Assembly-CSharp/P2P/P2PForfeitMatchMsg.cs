using System;
using network;

namespace P2P
{
	// Token: 0x02000820 RID: 2080
	public class P2PForfeitMatchMsg : NetMsgBase, IP2PMessage
	{
		// Token: 0x06003382 RID: 13186 RVA: 0x000F53E4 File Offset: 0x000F37E4
		public P2PForfeitMatchMsg()
		{
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x000F53EC File Offset: 0x000F37EC
		public P2PForfeitMatchMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x000F5417 File Offset: 0x000F3817
		public override uint MsgID()
		{
			return 15U;
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x000F541B File Offset: 0x000F381B
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToHost;
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x000F541E File Offset: 0x000F381E
		public override void SerializeMsg()
		{
			base.Pack(this.senderSteamID);
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x000F542C File Offset: 0x000F382C
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.senderSteamID);
		}

		// Token: 0x040023FF RID: 9215
		public ulong senderSteamID;
	}
}
