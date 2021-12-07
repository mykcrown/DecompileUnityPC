// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace Auth
{
	public class RequestPortalAckMsg : NetMsgBase
	{
		private bool accepted;

		private ulong sessionId;

		private NetAddress gatewayAddr = new NetAddress();

		private string username;

		private ulong accountId;

		public byte[] token = new byte[16];

		public bool Accepted
		{
			get
			{
				return this.accepted;
			}
		}

		public ulong SessionId
		{
			get
			{
				return this.sessionId;
			}
		}

		public NetAddress GatewayAddr
		{
			get
			{
				return this.gatewayAddr;
			}
		}

		public string Username
		{
			get
			{
				return this.username;
			}
		}

		public ulong AccountId
		{
			get
			{
				return this.accountId;
			}
		}

		public RequestPortalAckMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 8u;
		}

		public override void SerializeMsg()
		{
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.accepted);
			base.Unpack(ref this.sessionId);
			this.gatewayAddr.Unpack(this);
			base.UnpackByteArray(ref this.token);
			base.Unpack(ref this.username);
			base.Unpack(ref this.accountId);
		}
	}
}
