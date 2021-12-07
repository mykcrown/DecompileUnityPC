using System;
using network;

namespace Auth
{
	// Token: 0x0200077C RID: 1916
	public class LoginWavedashMsg : NetMsgBase
	{
		// Token: 0x06002F66 RID: 12134 RVA: 0x000EDB4A File Offset: 0x000EBF4A
		public LoginWavedashMsg(ulong version, string emailAddress, string password)
		{
			this.version = version;
			this.emailAddress = emailAddress;
			this.password = password;
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x000EDB67 File Offset: 0x000EBF67
		public override uint MsgID()
		{
			return 3U;
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x000EDB6A File Offset: 0x000EBF6A
		public override void SerializeMsg()
		{
			base.Pack(this.version);
			base.Pack(this.emailAddress);
			base.Pack(this.password);
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x000EDB90 File Offset: 0x000EBF90
		public override void DeserializeMsg()
		{
		}

		// Token: 0x0400212C RID: 8492
		private ulong version;

		// Token: 0x0400212D RID: 8493
		private string emailAddress;

		// Token: 0x0400212E RID: 8494
		private string password;
	}
}
