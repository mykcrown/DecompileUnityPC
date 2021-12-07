using System;
using network;

namespace Auth
{
	// Token: 0x02000780 RID: 1920
	public class AccountCreateAckMsg : NetMsgBase
	{
		// Token: 0x06002F72 RID: 12146 RVA: 0x000EDC09 File Offset: 0x000EC009
		public AccountCreateAckMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x000EDC34 File Offset: 0x000EC034
		public override uint MsgID()
		{
			return 6U;
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x000EDC37 File Offset: 0x000EC037
		public override void SerializeMsg()
		{
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x000EDC3C File Offset: 0x000EC03C
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.success);
			uint num = 0U;
			base.Unpack(ref num);
			this.nameResult = (AccountCreateAckMsg.ENameResult)num;
			base.Unpack(ref num);
			this.emailResult = (AccountCreateAckMsg.EEmailResult)num;
		}

		// Token: 0x04002137 RID: 8503
		public bool success;

		// Token: 0x04002138 RID: 8504
		public AccountCreateAckMsg.ENameResult nameResult;

		// Token: 0x04002139 RID: 8505
		public AccountCreateAckMsg.EEmailResult emailResult;

		// Token: 0x02000781 RID: 1921
		public enum ENameResult
		{
			// Token: 0x0400213B RID: 8507
			Invalid,
			// Token: 0x0400213C RID: 8508
			Okay,
			// Token: 0x0400213D RID: 8509
			NameShort
		}

		// Token: 0x02000782 RID: 1922
		public enum EEmailResult
		{
			// Token: 0x0400213F RID: 8511
			Invalid,
			// Token: 0x04002140 RID: 8512
			Okay,
			// Token: 0x04002141 RID: 8513
			Bad,
			// Token: 0x04002142 RID: 8514
			AlreadyInUse
		}
	}
}
