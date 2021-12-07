using System;
using network;
using P2P;

namespace BattleServer
{
	// Token: 0x0200078B RID: 1931
	public class InputAckMsg : NetMsgFast, IQueueDuplicateHandler, IBufferable, IP2PMessage, IUDPMessage, IMessageToSpecificUser
	{
		// Token: 0x06002F9D RID: 12189 RVA: 0x000EE71F File Offset: 0x000ECB1F
		public InputAckMsg()
		{
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x000EE727 File Offset: 0x000ECB27
		public InputAckMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x000EE752 File Offset: 0x000ECB52
		public void CopyFrom(InputAckMsg copyMessage)
		{
			this._targetUserID = copyMessage._targetUserID;
			this.latestAckedFrame = copyMessage.latestAckedFrame;
			this.playerID = copyMessage.playerID;
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x000EE778 File Offset: 0x000ECB78
		public override uint MsgID()
		{
			return 3U;
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x000EE77B File Offset: 0x000ECB7B
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06002FA2 RID: 12194 RVA: 0x000EE77E File Offset: 0x000ECB7E
		public ulong TargetUserID
		{
			get
			{
				return this._targetUserID;
			}
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x000EE786 File Offset: 0x000ECB86
		public override void SerializeMsg()
		{
			base.Pack(this.playerID, 4U);
			base.Pack(this.latestAckedFrame, 16U);
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x000EE7A3 File Offset: 0x000ECBA3
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.playerID, 4U);
			base.Unpack(ref this.latestAckedFrame, 16U);
		}

		// Token: 0x06002FA5 RID: 12197 RVA: 0x000EE7C0 File Offset: 0x000ECBC0
		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize];
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x000EE7CF File Offset: 0x000ECBCF
		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			base.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x000EE7EC File Offset: 0x000ECBEC
		public bool HandledAsDuplicate(INetMsg messageInQueue)
		{
			(messageInQueue as InputAckMsg).CopyFrom(this);
			return true;
		}

		// Token: 0x0400216A RID: 8554
		public ulong _targetUserID;

		// Token: 0x0400216B RID: 8555
		public ushort latestAckedFrame;

		// Token: 0x0400216C RID: 8556
		public byte playerID;
	}
}
