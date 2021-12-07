// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using network;
using System;

namespace P2P
{
	public class P2PPongMsg : NetMsgBase, IBufferable, IP2PMessage
	{
		public ulong senderSteamID;

		public P2PPongMsg()
		{
		}

		public P2PPongMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 6u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.senderSteamID);
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.senderSteamID);
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
