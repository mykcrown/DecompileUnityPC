// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace Auth
{
	public class LoginWavedashMsg : NetMsgBase
	{
		private ulong version;

		private string emailAddress;

		private string password;

		public LoginWavedashMsg(ulong version, string emailAddress, string password)
		{
			this.version = version;
			this.emailAddress = emailAddress;
			this.password = password;
		}

		public override uint MsgID()
		{
			return 3u;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.version);
			base.Pack(this.emailAddress);
			base.Pack(this.password);
		}

		public override void DeserializeMsg()
		{
		}
	}
}
