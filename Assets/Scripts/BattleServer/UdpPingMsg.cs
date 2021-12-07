// Decompile from assembly: Assembly-CSharp.dll

using network;
using P2P;
using System;

namespace BattleServer
{
	public class UdpPingMsg : NetMsgFast, IBufferable, IP2PMessage, IUDPMessage, IMessageToSpecificUser
	{
		public ulong _targetUserID;

		public ulong senderId;

		public ulong TargetUserID
		{
			get
			{
				return this._targetUserID;
			}
		}

		public UdpPingMsg()
		{
		}

		public UdpPingMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8u;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 7u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.senderId, 64u);
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.senderId, 64u);
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
