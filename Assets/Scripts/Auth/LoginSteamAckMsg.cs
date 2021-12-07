// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace Auth
{
	public class LoginSteamAckMsg : NetMsgBase
	{
		public enum EResult
		{
			Invalid,
			TicketFailedDecrypt,
			AppIdMismatch,
			TicketTimedOut,
			DbQueryFailed,
			Accepted,
			AccountDetailsNeeded,
			ClosedToPublic,
			BadGwVersion
		}

		public LoginSteamAckMsg.EResult result;

		public LoginSteamAckMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 2u;
		}

		public override void SerializeMsg()
		{
		}

		public override void DeserializeMsg()
		{
			uint num = 0u;
			base.Unpack(ref num);
			this.result = (LoginSteamAckMsg.EResult)num;
		}
	}
}
