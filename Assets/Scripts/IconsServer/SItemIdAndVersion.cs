// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace IconsServer
{
	public class SItemIdAndVersion
	{
		public ulong itemId = 18446744073709551615uL;

		public ulong version;

		public void Pack(NetMsgBase msg)
		{
			msg.Pack((uint)this.itemId);
			msg.Pack((ushort)this.version);
		}

		public void Unpack(NetMsgBase msg)
		{
			uint num = 0u;
			msg.Unpack(ref num);
			this.itemId = (ulong)num;
			ushort num2 = 0;
			msg.Unpack(ref num2);
			this.version = (ulong)num2;
		}
	}
}
