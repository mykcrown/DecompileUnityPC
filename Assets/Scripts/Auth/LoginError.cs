// Decompile from assembly: Assembly-CSharp.dll

using Steamworks;
using System;

namespace Auth
{
	public class LoginError
	{
		public bool noConnection;

		public EResult steamTicketError = EResult.k_EResultOK;

		public LoginWavedashAckMsg.EResult wavedashLoginError = LoginWavedashAckMsg.EResult.Accepted;

		public LoginSteamAckMsg.EResult steamLoginError = LoginSteamAckMsg.EResult.Accepted;

		public LoginError()
		{
		}

		public LoginError(bool noConnection)
		{
			this.noConnection = noConnection;
		}

		public LoginError(EResult ticketError)
		{
			this.steamTicketError = ticketError;
		}

		public LoginError(LoginWavedashAckMsg.EResult wavedashError)
		{
			this.wavedashLoginError = wavedashError;
		}

		public LoginError(LoginSteamAckMsg.EResult steamError)
		{
			this.steamLoginError = steamError;
		}
	}
}
