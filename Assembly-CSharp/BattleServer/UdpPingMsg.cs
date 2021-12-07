using System;
using network;
using P2P;

namespace BattleServer
{
	// Token: 0x02000790 RID: 1936
	public class UdpPingMsg : NetMsgFast, IBufferable, IP2PMessage, IUDPMessage, IMessageToSpecificUser
	{
		// Token: 0x06002FCB RID: 12235 RVA: 0x000EEACD File Offset: 0x000ECECD
		public UdpPingMsg()
		{
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x000EEAD5 File Offset: 0x000ECED5
		public UdpPingMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8U;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x000EEB02 File Offset: 0x000ECF02
		public override uint MsgID()
		{
			return 7U;
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x000EEB05 File Offset: 0x000ECF05
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06002FCF RID: 12239 RVA: 0x000EEB08 File Offset: 0x000ECF08
		public ulong TargetUserID
		{
			get
			{
				return this._targetUserID;
			}
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x000EEB10 File Offset: 0x000ECF10
		public override void SerializeMsg()
		{
			base.Pack(this.senderId, 64U);
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x000EEB20 File Offset: 0x000ECF20
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.senderId, 64U);
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x000EEB30 File Offset: 0x000ECF30
		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize];
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x000EEB3F File Offset: 0x000ECF3F
		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			base.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}

		// Token: 0x0400217A RID: 8570
		public ulong _targetUserID;

		// Token: 0x0400217B RID: 8571
		public ulong senderId;
	}
}
