// Decompile from assembly: Assembly-CSharp.dll

using network;
using P2P;
using System;

namespace BattleServer
{
	public class DisconnectAckMsg : NetMsgFast, IP2PMessage, IBufferable, IUDPMessage, IMessageToSpecificUser
	{
		public ulong _targetUserID;

		public int senderPlayerID;

		public int quittingPlayerID;

		public ulong TargetUserID
		{
			get
			{
				return this._targetUserID;
			}
		}

		public DisconnectAckMsg()
		{
		}

		public DisconnectAckMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8u;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 12u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.senderPlayerID, 4u);
			base.Pack(this.quittingPlayerID, 4u);
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.senderPlayerID, 4u);
			base.Unpack(ref this.quittingPlayerID, 4u);
		}

		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize];
		}

		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			base.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}
	}
}
