using System;
using BattleServer;
using network;

namespace P2P
{
	// Token: 0x02000828 RID: 2088
	public class P2PPingMsg : NetMsgBase, IBufferable, IP2PMessage
	{
		// Token: 0x060033B2 RID: 13234 RVA: 0x000F5762 File Offset: 0x000F3B62
		public P2PPingMsg()
		{
		}

		// Token: 0x060033B3 RID: 13235 RVA: 0x000F576A File Offset: 0x000F3B6A
		public P2PPingMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x000F5795 File Offset: 0x000F3B95
		public override uint MsgID()
		{
			return 5U;
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x000F5798 File Offset: 0x000F3B98
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x000F579B File Offset: 0x000F3B9B
		public override void SerializeMsg()
		{
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x000F579D File Offset: 0x000F3B9D
		public override void DeserializeMsg()
		{
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x000F579F File Offset: 0x000F3B9F
		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize];
			this.m_reusable = true;
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x000F57B5 File Offset: 0x000F3BB5
		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			this.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}
	}
}
