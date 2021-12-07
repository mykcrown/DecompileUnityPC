using System;
using network;

namespace Auth
{
	// Token: 0x02000779 RID: 1913
	public class LoginSteamMsg : NetMsgBase
	{
		// Token: 0x06002F5E RID: 12126 RVA: 0x000EDA64 File Offset: 0x000EBE64
		public LoginSteamMsg(ulong version, uint appId, byte[] ticket, uint ticketSize, string language)
		{
			this.version = version;
			this.appId = appId;
			if (ticketSize > 0U && ticket != null)
			{
				this.ticket = new byte[ticketSize];
				Buffer.BlockCopy(ticket, 0, this.ticket, 0, (int)ticketSize);
			}
			this.language = language;
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x000EDAC4 File Offset: 0x000EBEC4
		public override uint MsgID()
		{
			return 1U;
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x000EDAC7 File Offset: 0x000EBEC7
		public override void SerializeMsg()
		{
			base.Pack(this.version);
			base.Pack(this.appId);
			base.PackByteArray(this.ticket);
			base.Pack(this.language);
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x000EDAF9 File Offset: 0x000EBEF9
		public override void DeserializeMsg()
		{
		}

		// Token: 0x0400211D RID: 8477
		private ulong version;

		// Token: 0x0400211E RID: 8478
		private uint appId;

		// Token: 0x0400211F RID: 8479
		private byte[] ticket;

		// Token: 0x04002120 RID: 8480
		private string language = string.Empty;
	}
}
