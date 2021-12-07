using System;
using network;
using P2P;

namespace BattleServer
{
	// Token: 0x02000788 RID: 1928
	public class RelayMsg : NetMsgBase, IBufferable, IP2PMessage, IP2PAdjustBroadcastMode
	{
		// Token: 0x06002F85 RID: 12165 RVA: 0x000EDD7F File Offset: 0x000EC17F
		public RelayMsg()
		{
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x000EDD87 File Offset: 0x000EC187
		public RelayMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x000EDDB2 File Offset: 0x000EC1B2
		public override uint MsgID()
		{
			return 0U;
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x000EDDB5 File Offset: 0x000EC1B5
		public BroadcastType GetBroadcastMode()
		{
			return this.broadcastMode;
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x000EDDBD File Offset: 0x000EC1BD
		public void SetBroadcastMode(BroadcastType broadcastMode)
		{
			this.broadcastMode = broadcastMode;
		}

		// Token: 0x06002F8A RID: 12170 RVA: 0x000EDDC6 File Offset: 0x000EC1C6
		public override void SerializeMsg()
		{
			base.PackByteArray(this.bytes);
			base.Pack((int)this.broadcastMode);
		}

		// Token: 0x06002F8B RID: 12171 RVA: 0x000EDDE0 File Offset: 0x000EC1E0
		public override void DeserializeMsg()
		{
			base.UnpackByteArray(ref this.bytes);
			uint num = 3U;
			base.Unpack(ref num);
			this.broadcastMode = (BroadcastType)num;
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x000EDE0A File Offset: 0x000EC20A
		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize];
			this.bytes = new byte[bufferSize];
			this.m_reusable = true;
		}

		// Token: 0x06002F8D RID: 12173 RVA: 0x000EDE2D File Offset: 0x000EC22D
		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			this.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}

		// Token: 0x0400215D RID: 8541
		public byte[] bytes;

		// Token: 0x0400215E RID: 8542
		public uint byteSize;

		// Token: 0x0400215F RID: 8543
		private BroadcastType broadcastMode;
	}
}
