// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace Auth
{
	public class LoginSteamMsg : NetMsgBase
	{
		private ulong version;

		private uint appId;

		private byte[] ticket;

		private string language = string.Empty;

		public LoginSteamMsg(ulong version, uint appId, byte[] ticket, uint ticketSize, string language)
		{
			this.version = version;
			this.appId = appId;
			if (ticketSize > 0u && ticket != null)
			{
				this.ticket = new byte[ticketSize];
				Buffer.BlockCopy(ticket, 0, this.ticket, 0, (int)ticketSize);
			}
			this.language = language;
		}

		public override uint MsgID()
		{
			return 1u;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.version);
			base.Pack(this.appId);
			base.PackByteArray(this.ticket);
			base.Pack(this.language);
		}

		public override void DeserializeMsg()
		{
		}
	}
}
