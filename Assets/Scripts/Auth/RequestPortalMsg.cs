// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace Auth
{
	public class RequestPortalMsg : NetMsgBase
	{
		public ulong staticChecksum;

		public override uint MsgID()
		{
			return 7u;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.staticChecksum);
		}

		public override void DeserializeMsg()
		{
		}
	}
}
