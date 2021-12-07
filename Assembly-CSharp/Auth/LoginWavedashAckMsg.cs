using System;
using network;

namespace Auth
{
	// Token: 0x0200077D RID: 1917
	public class LoginWavedashAckMsg : NetMsgBase
	{
		// Token: 0x06002F6A RID: 12138 RVA: 0x000EDB92 File Offset: 0x000EBF92
		public LoginWavedashAckMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x000EDBBD File Offset: 0x000EBFBD
		public override uint MsgID()
		{
			return 4U;
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x000EDBC0 File Offset: 0x000EBFC0
		public override void SerializeMsg()
		{
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x000EDBC4 File Offset: 0x000EBFC4
		public override void DeserializeMsg()
		{
			uint num = 0U;
			base.Unpack(ref num);
			this.result = (LoginWavedashAckMsg.EResult)num;
		}

		// Token: 0x0400212F RID: 8495
		public LoginWavedashAckMsg.EResult result;

		// Token: 0x0200077E RID: 1918
		public enum EResult
		{
			// Token: 0x04002131 RID: 8497
			Invalid,
			// Token: 0x04002132 RID: 8498
			DbQueryFailed,
			// Token: 0x04002133 RID: 8499
			Accepted,
			// Token: 0x04002134 RID: 8500
			AccountDetailsNeeded
		}
	}
}
