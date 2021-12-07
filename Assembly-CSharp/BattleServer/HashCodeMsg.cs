using System;
using network;
using P2P;

namespace BattleServer
{
	// Token: 0x0200078C RID: 1932
	public class HashCodeMsg : NetMsgFast, IBufferable, IP2PMessage, IUDPMessage
	{
		// Token: 0x06002FA8 RID: 12200 RVA: 0x000EE7FB File Offset: 0x000ECBFB
		public HashCodeMsg()
		{
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x000EE803 File Offset: 0x000ECC03
		public HashCodeMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8U;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06002FAA RID: 12202 RVA: 0x000EE830 File Offset: 0x000ECC30
		public override uint MsgID()
		{
			return 2U;
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x000EE833 File Offset: 0x000ECC33
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToHost;
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x000EE836 File Offset: 0x000ECC36
		public override void SerializeMsg()
		{
			base.Pack(this.frame, 16U);
			base.Pack(this.hashCode, 16U);
			base.Pack(this.senderId, 4U);
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x000EE861 File Offset: 0x000ECC61
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.frame, 16U);
			base.Unpack(ref this.hashCode, 16U);
			base.Unpack(ref this.senderId, 4U);
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x000EE88C File Offset: 0x000ECC8C
		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize];
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x000EE89B File Offset: 0x000ECC9B
		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			base.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}

		// Token: 0x0400216D RID: 8557
		public ushort frame;

		// Token: 0x0400216E RID: 8558
		public short hashCode;

		// Token: 0x0400216F RID: 8559
		public byte senderId;
	}
}
