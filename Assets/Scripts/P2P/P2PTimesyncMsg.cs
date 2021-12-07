// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using network;
using System;

namespace P2P
{
	public class P2PTimesyncMsg : NetMsgBase, IP2PMessage, IMessageToSpecificUser
	{
		public ulong _targetUserID;

		public long timeOffset;

		public long sendTimeLocalMs;

		public ulong TargetUserID
		{
			get
			{
				return this._targetUserID;
			}
		}

		public P2PTimesyncMsg()
		{
		}

		public P2PTimesyncMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 9u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.timeOffset);
			base.Pack(this.sendTimeLocalMs);
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.timeOffset);
			base.Unpack(ref this.sendTimeLocalMs);
		}
	}
}
