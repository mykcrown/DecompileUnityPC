// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace P2P
{
	public class P2PLockCharacterSelectAckMsg : NetMsgBase, IP2PMessage
	{
		public ulong steamID;

		public bool accepted;

		public P2PLockCharacterSelectAckMsg()
		{
		}

		public P2PLockCharacterSelectAckMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 20u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToClient;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.steamID);
			base.Pack(this.accepted);
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.steamID);
			base.Unpack(ref this.accepted);
		}
	}
}
