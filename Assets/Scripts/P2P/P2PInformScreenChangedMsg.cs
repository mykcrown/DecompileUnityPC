// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace P2P
{
	public class P2PInformScreenChangedMsg : NetMsgFast, IP2PMessage
	{
		public ulong userID;

		public int screenID;

		public P2PInformScreenChangedMsg()
		{
		}

		public P2PInformScreenChangedMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8u;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 18u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToHost;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.userID, 64u);
			base.Pack(this.screenID, 32u);
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.userID, 64u);
			base.Unpack(ref this.screenID, 32u);
		}
	}
}
