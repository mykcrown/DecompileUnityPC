using System;
using network;

namespace Auth
{
	// Token: 0x02000784 RID: 1924
	public class RequestPortalAckMsg : NetMsgBase
	{
		// Token: 0x06002F7A RID: 12154 RVA: 0x000EDC90 File Offset: 0x000EC090
		public RequestPortalAckMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x000EDCDE File Offset: 0x000EC0DE
		public override uint MsgID()
		{
			return 8U;
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x000EDCE1 File Offset: 0x000EC0E1
		public override void SerializeMsg()
		{
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x000EDCE4 File Offset: 0x000EC0E4
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.accepted);
			base.Unpack(ref this.sessionId);
			this.gatewayAddr.Unpack(this);
			base.UnpackByteArray(ref this.token);
			base.Unpack(ref this.username);
			base.Unpack(ref this.accountId);
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06002F7E RID: 12158 RVA: 0x000EDD39 File Offset: 0x000EC139
		public bool Accepted
		{
			get
			{
				return this.accepted;
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06002F7F RID: 12159 RVA: 0x000EDD41 File Offset: 0x000EC141
		public ulong SessionId
		{
			get
			{
				return this.sessionId;
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06002F80 RID: 12160 RVA: 0x000EDD49 File Offset: 0x000EC149
		public NetAddress GatewayAddr
		{
			get
			{
				return this.gatewayAddr;
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06002F81 RID: 12161 RVA: 0x000EDD51 File Offset: 0x000EC151
		public string Username
		{
			get
			{
				return this.username;
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06002F82 RID: 12162 RVA: 0x000EDD59 File Offset: 0x000EC159
		public ulong AccountId
		{
			get
			{
				return this.accountId;
			}
		}

		// Token: 0x04002144 RID: 8516
		private bool accepted;

		// Token: 0x04002145 RID: 8517
		private ulong sessionId;

		// Token: 0x04002146 RID: 8518
		private NetAddress gatewayAddr = new NetAddress();

		// Token: 0x04002147 RID: 8519
		private string username;

		// Token: 0x04002148 RID: 8520
		private ulong accountId;

		// Token: 0x04002149 RID: 8521
		public byte[] token = new byte[16];
	}
}
