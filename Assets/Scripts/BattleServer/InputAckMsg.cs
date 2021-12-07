// Decompile from assembly: Assembly-CSharp.dll

using network;
using P2P;
using System;

namespace BattleServer
{
	public class InputAckMsg : NetMsgFast, IQueueDuplicateHandler, IBufferable, IP2PMessage, IUDPMessage, IMessageToSpecificUser
	{
		public ulong _targetUserID;

		public ushort latestAckedFrame;

		public byte playerID;

		public ulong TargetUserID
		{
			get
			{
				return this._targetUserID;
			}
		}

		public InputAckMsg()
		{
		}

		public InputAckMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public void CopyFrom(InputAckMsg copyMessage)
		{
			this._targetUserID = copyMessage._targetUserID;
			this.latestAckedFrame = copyMessage.latestAckedFrame;
			this.playerID = copyMessage.playerID;
		}

		public override uint MsgID()
		{
			return 3u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.playerID, 4u);
			base.Pack(this.latestAckedFrame, 16u);
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.playerID, 4u);
			base.Unpack(ref this.latestAckedFrame, 16u);
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

		public bool HandledAsDuplicate(INetMsg messageInQueue)
		{
			(messageInQueue as InputAckMsg).CopyFrom(this);
			return true;
		}
	}
}
