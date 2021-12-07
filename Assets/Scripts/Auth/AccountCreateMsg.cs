// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace Auth
{
	public class AccountCreateMsg : NetMsgBase
	{
		public string userName;

		public string email;

		public override uint MsgID()
		{
			return 5u;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.userName);
			base.Pack(this.email);
		}

		public override void DeserializeMsg()
		{
		}
	}
}
