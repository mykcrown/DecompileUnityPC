using System;
using network;
using P2P;

namespace BattleServer
{
	// Token: 0x0200078F RID: 1935
	public class DisconnectAckMsg : NetMsgFast, IP2PMessage, IBufferable, IUDPMessage, IMessageToSpecificUser
	{
		// Token: 0x06002FC2 RID: 12226 RVA: 0x000EEA25 File Offset: 0x000ECE25
		public DisconnectAckMsg()
		{
		}

		// Token: 0x06002FC3 RID: 12227 RVA: 0x000EEA2D File Offset: 0x000ECE2D
		public DisconnectAckMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8U;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x000EEA5A File Offset: 0x000ECE5A
		public override uint MsgID()
		{
			return 12U;
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x000EEA5E File Offset: 0x000ECE5E
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06002FC6 RID: 12230 RVA: 0x000EEA61 File Offset: 0x000ECE61
		public ulong TargetUserID
		{
			get
			{
				return this._targetUserID;
			}
		}

		// Token: 0x06002FC7 RID: 12231 RVA: 0x000EEA69 File Offset: 0x000ECE69
		public override void SerializeMsg()
		{
			base.Pack(this.senderPlayerID, 4U);
			base.Pack(this.quittingPlayerID, 4U);
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x000EEA85 File Offset: 0x000ECE85
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.senderPlayerID, 4U);
			base.Unpack(ref this.quittingPlayerID, 4U);
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x000EEAA1 File Offset: 0x000ECEA1
		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize];
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x000EEAB0 File Offset: 0x000ECEB0
		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			base.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}

		// Token: 0x04002177 RID: 8567
		public ulong _targetUserID;

		// Token: 0x04002178 RID: 8568
		public int senderPlayerID;

		// Token: 0x04002179 RID: 8569
		public int quittingPlayerID;
	}
}
