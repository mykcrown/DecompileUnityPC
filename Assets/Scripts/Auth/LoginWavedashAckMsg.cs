// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace Auth
{
	public class LoginWavedashAckMsg : NetMsgBase
	{
		public enum EResult
		{
			Invalid,
			DbQueryFailed,
			Accepted,
			AccountDetailsNeeded
		}

		public LoginWavedashAckMsg.EResult result;

		public LoginWavedashAckMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 4u;
		}

		public override void SerializeMsg()
		{
		}

		public override void DeserializeMsg()
		{
			uint num = 0u;
			base.Unpack(ref num);
			this.result = (LoginWavedashAckMsg.EResult)num;
		}
	}
}
