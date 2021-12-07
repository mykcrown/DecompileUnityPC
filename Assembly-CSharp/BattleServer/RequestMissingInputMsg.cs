using System;
using network;
using P2P;

namespace BattleServer
{
	// Token: 0x0200078D RID: 1933
	public class RequestMissingInputMsg : NetMsgFast, IBufferable, IP2PMessage, IUDPMessage, IMessageToSpecificUser
	{
		// Token: 0x06002FB0 RID: 12208 RVA: 0x000EE8B8 File Offset: 0x000ECCB8
		public RequestMissingInputMsg()
		{
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x000EE8C0 File Offset: 0x000ECCC0
		public RequestMissingInputMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8U;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x000EE8ED File Offset: 0x000ECCED
		public override uint MsgID()
		{
			return 4U;
		}

		// Token: 0x06002FB3 RID: 12211 RVA: 0x000EE8F0 File Offset: 0x000ECCF0
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x06002FB4 RID: 12212 RVA: 0x000EE8F3 File Offset: 0x000ECCF3
		public ulong TargetUserID
		{
			get
			{
				return this._targetUserID;
			}
		}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x000EE8FB File Offset: 0x000ECCFB
		public override void SerializeMsg()
		{
			base.Pack(this.fromPlayer, 4U);
			base.Pack(this.forPlayer, 4U);
			base.Pack(this.startFrame, 16U);
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x000EE925 File Offset: 0x000ECD25
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.fromPlayer, 4U);
			base.Unpack(ref this.forPlayer, 4U);
			base.Unpack(ref this.startFrame, 16U);
		}

		// Token: 0x06002FB7 RID: 12215 RVA: 0x000EE94F File Offset: 0x000ECD4F
		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize];
		}

		// Token: 0x06002FB8 RID: 12216 RVA: 0x000EE95E File Offset: 0x000ECD5E
		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			base.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}

		// Token: 0x04002170 RID: 8560
		public ulong _targetUserID;

		// Token: 0x04002171 RID: 8561
		public int fromPlayer;

		// Token: 0x04002172 RID: 8562
		public int forPlayer;

		// Token: 0x04002173 RID: 8563
		public int startFrame;
	}
}
