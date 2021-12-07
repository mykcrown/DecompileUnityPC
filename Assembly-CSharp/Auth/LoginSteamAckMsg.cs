using System;
using network;

namespace Auth
{
	// Token: 0x0200077A RID: 1914
	public class LoginSteamAckMsg : NetMsgBase
	{
		// Token: 0x06002F62 RID: 12130 RVA: 0x000EDAFB File Offset: 0x000EBEFB
		public LoginSteamAckMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x000EDB26 File Offset: 0x000EBF26
		public override uint MsgID()
		{
			return 2U;
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x000EDB29 File Offset: 0x000EBF29
		public override void SerializeMsg()
		{
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x000EDB2C File Offset: 0x000EBF2C
		public override void DeserializeMsg()
		{
			uint num = 0U;
			base.Unpack(ref num);
			this.result = (LoginSteamAckMsg.EResult)num;
		}

		// Token: 0x04002121 RID: 8481
		public LoginSteamAckMsg.EResult result;

		// Token: 0x0200077B RID: 1915
		public enum EResult
		{
			// Token: 0x04002123 RID: 8483
			Invalid,
			// Token: 0x04002124 RID: 8484
			TicketFailedDecrypt,
			// Token: 0x04002125 RID: 8485
			AppIdMismatch,
			// Token: 0x04002126 RID: 8486
			TicketTimedOut,
			// Token: 0x04002127 RID: 8487
			DbQueryFailed,
			// Token: 0x04002128 RID: 8488
			Accepted,
			// Token: 0x04002129 RID: 8489
			AccountDetailsNeeded,
			// Token: 0x0400212A RID: 8490
			ClosedToPublic,
			// Token: 0x0400212B RID: 8491
			BadGwVersion
		}
	}
}
