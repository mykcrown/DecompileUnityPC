using System;
using network;

namespace Auth
{
	// Token: 0x02000783 RID: 1923
	public class RequestPortalMsg : NetMsgBase
	{
		// Token: 0x06002F77 RID: 12151 RVA: 0x000EDC7D File Offset: 0x000EC07D
		public override uint MsgID()
		{
			return 7U;
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x000EDC80 File Offset: 0x000EC080
		public override void SerializeMsg()
		{
			base.Pack(this.staticChecksum);
		}

		// Token: 0x06002F79 RID: 12153 RVA: 0x000EDC8E File Offset: 0x000EC08E
		public override void DeserializeMsg()
		{
		}

		// Token: 0x04002143 RID: 8515
		public ulong staticChecksum;
	}
}
