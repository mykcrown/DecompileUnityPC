using System;
using Steamworks;

namespace Auth
{
	// Token: 0x02000777 RID: 1911
	public class LoginError
	{
		// Token: 0x06002F59 RID: 12121 RVA: 0x000ED39F File Offset: 0x000EB79F
		public LoginError()
		{
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x000ED3BC File Offset: 0x000EB7BC
		public LoginError(bool noConnection)
		{
			this.noConnection = noConnection;
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x000ED3E0 File Offset: 0x000EB7E0
		public LoginError(EResult ticketError)
		{
			this.steamTicketError = ticketError;
		}

		// Token: 0x06002F5C RID: 12124 RVA: 0x000ED404 File Offset: 0x000EB804
		public LoginError(LoginWavedashAckMsg.EResult wavedashError)
		{
			this.wavedashLoginError = wavedashError;
		}

		// Token: 0x06002F5D RID: 12125 RVA: 0x000ED428 File Offset: 0x000EB828
		public LoginError(LoginSteamAckMsg.EResult steamError)
		{
			this.steamLoginError = steamError;
		}

		// Token: 0x0400210E RID: 8462
		public bool noConnection;

		// Token: 0x0400210F RID: 8463
		public EResult steamTicketError = EResult.k_EResultOK;

		// Token: 0x04002110 RID: 8464
		public LoginWavedashAckMsg.EResult wavedashLoginError = LoginWavedashAckMsg.EResult.Accepted;

		// Token: 0x04002111 RID: 8465
		public LoginSteamAckMsg.EResult steamLoginError = LoginSteamAckMsg.EResult.Accepted;
	}
}
