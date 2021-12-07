using System;
using network;
using P2P;

namespace BattleServer
{
	// Token: 0x02000791 RID: 1937
	public class UdpPongMsg : NetMsgFast, IBufferable, IP2PMessage, IUDPMessage, IMessageToSpecificUser
	{
		// Token: 0x06002FD4 RID: 12244 RVA: 0x000EEB5C File Offset: 0x000ECF5C
		public UdpPongMsg()
		{
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x000EEB64 File Offset: 0x000ECF64
		public UdpPongMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8U;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x000EEB91 File Offset: 0x000ECF91
		public override uint MsgID()
		{
			return 8U;
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x000EEB94 File Offset: 0x000ECF94
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06002FD8 RID: 12248 RVA: 0x000EEB97 File Offset: 0x000ECF97
		public ulong TargetUserID
		{
			get
			{
				return this._targetUserID;
			}
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x000EEB9F File Offset: 0x000ECF9F
		public override void SerializeMsg()
		{
			base.Pack(this.senderId, 64U);
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x000EEBAF File Offset: 0x000ECFAF
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.senderId, 64U);
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x000EEBBF File Offset: 0x000ECFBF
		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize];
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x000EEBCE File Offset: 0x000ECFCE
		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			base.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}

		// Token: 0x0400217C RID: 8572
		public ulong _targetUserID;

		// Token: 0x0400217D RID: 8573
		public ulong senderId;
	}
}
