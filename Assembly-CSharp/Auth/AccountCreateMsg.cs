using System;
using network;

namespace Auth
{
	// Token: 0x0200077F RID: 1919
	public class AccountCreateMsg : NetMsgBase
	{
		// Token: 0x06002F6F RID: 12143 RVA: 0x000EDBEA File Offset: 0x000EBFEA
		public override uint MsgID()
		{
			return 5U;
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x000EDBED File Offset: 0x000EBFED
		public override void SerializeMsg()
		{
			base.Pack(this.userName);
			base.Pack(this.email);
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x000EDC07 File Offset: 0x000EC007
		public override void DeserializeMsg()
		{
		}

		// Token: 0x04002135 RID: 8501
		public string userName;

		// Token: 0x04002136 RID: 8502
		public string email;
	}
}
