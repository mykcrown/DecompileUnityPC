using System;
using BattleServer;
using network;

namespace P2P
{
	// Token: 0x02000829 RID: 2089
	public class P2PPongMsg : NetMsgBase, IBufferable, IP2PMessage
	{
		// Token: 0x060033BA RID: 13242 RVA: 0x000F57D2 File Offset: 0x000F3BD2
		public P2PPongMsg()
		{
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x000F57DA File Offset: 0x000F3BDA
		public P2PPongMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x060033BC RID: 13244 RVA: 0x000F5805 File Offset: 0x000F3C05
		public override uint MsgID()
		{
			return 6U;
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x000F5808 File Offset: 0x000F3C08
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x000F580B File Offset: 0x000F3C0B
		public override void SerializeMsg()
		{
			base.Pack(this.senderSteamID);
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x000F5819 File Offset: 0x000F3C19
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.senderSteamID);
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x000F5827 File Offset: 0x000F3C27
		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize];
			this.m_reusable = true;
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x000F583D File Offset: 0x000F3C3D
		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			this.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}

		// Token: 0x0400240E RID: 9230
		public ulong senderSteamID;
	}
}
