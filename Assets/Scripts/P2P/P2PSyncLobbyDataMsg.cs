// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace P2P
{
	public class P2PSyncLobbyDataMsg : NetMsgFast, IP2PMessage
	{
		public bool isInMatch;

		public P2PSyncLobbyDataMsg()
		{
		}

		public P2PSyncLobbyDataMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8u;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 27u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToClient;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.isInMatch);
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.isInMatch);
		}
	}
}
