// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using network;
using System;

namespace P2P
{
	public class P2PPingMsg : NetMsgBase, IBufferable, IP2PMessage
	{
		public P2PPingMsg()
		{
		}

		public P2PPingMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 5u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		public override void SerializeMsg()
		{
		}

		public override void DeserializeMsg()
		{
		}

		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize];
			this.m_reusable = true;
		}

		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			this.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}
	}
}
