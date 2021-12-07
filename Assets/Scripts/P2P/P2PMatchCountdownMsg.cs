// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace P2P
{
	public class P2PMatchCountdownMsg : NetMsgBase, IP2PMessage
	{
		public uint countDownSeconds;

		public ulong serverStartTimeMs;

		public P2PMatchCountdownMsg()
		{
		}

		public P2PMatchCountdownMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 22u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToClient;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.countDownSeconds);
			base.Pack(this.serverStartTimeMs);
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.countDownSeconds);
			base.Unpack(ref this.serverStartTimeMs);
		}
	}
}
