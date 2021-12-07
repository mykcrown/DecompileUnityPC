// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace network
{
	public interface INetMsg
	{
		uint MsgSize
		{
			get;
		}

		byte[] MsgBuffer
		{
			get;
		}

		uint MsgID();

		bool Serialize();
	}
}
