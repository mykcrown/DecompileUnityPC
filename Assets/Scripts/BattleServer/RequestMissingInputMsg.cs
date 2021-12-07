// Decompile from assembly: Assembly-CSharp.dll

using network;
using P2P;
using System;

namespace BattleServer
{
	public class RequestMissingInputMsg : NetMsgFast, IBufferable, IP2PMessage, IUDPMessage, IMessageToSpecificUser
	{
		public ulong _targetUserID;

		public int fromPlayer;

		public int forPlayer;

		public int startFrame;

		public ulong TargetUserID
		{
			get
			{
				return this._targetUserID;
			}
		}

		public RequestMissingInputMsg()
		{
		}

		public RequestMissingInputMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8u;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 4u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.fromPlayer, 4u);
			base.Pack(this.forPlayer, 4u);
			base.Pack(this.startFrame, 16u);
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.fromPlayer, 4u);
			base.Unpack(ref this.forPlayer, 4u);
			base.Unpack(ref this.startFrame, 16u);
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
