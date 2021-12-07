using System;
using network;
using P2P;

namespace BattleServer
{
	// Token: 0x0200078E RID: 1934
	public class DisconnectMsg : NetMsgFast, IP2PMessage, IBufferable, IUDPMessage, IMessageToSpecificUser
	{
		// Token: 0x06002FB9 RID: 12217 RVA: 0x000EE97B File Offset: 0x000ECD7B
		public DisconnectMsg()
		{
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x000EE983 File Offset: 0x000ECD83
		public DisconnectMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8U;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x000EE9B0 File Offset: 0x000ECDB0
		public override uint MsgID()
		{
			return 11U;
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x000EE9B4 File Offset: 0x000ECDB4
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06002FBD RID: 12221 RVA: 0x000EE9B7 File Offset: 0x000ECDB7
		public ulong TargetUserID
		{
			get
			{
				return this._targetUserID;
			}
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x000EE9BF File Offset: 0x000ECDBF
		public override void SerializeMsg()
		{
			base.Pack(this.playerID, 4U);
			base.Pack(this.frame, 16U);
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x000EE9DC File Offset: 0x000ECDDC
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.playerID, 4U);
			base.Unpack(ref this.frame, 16U);
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x000EE9F9 File Offset: 0x000ECDF9
		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize];
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x000EEA08 File Offset: 0x000ECE08
		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			base.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}

		// Token: 0x04002174 RID: 8564
		public ulong _targetUserID;

		// Token: 0x04002175 RID: 8565
		public int playerID;

		// Token: 0x04002176 RID: 8566
		public int frame;
	}
}
