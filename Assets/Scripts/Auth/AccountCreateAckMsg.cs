// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace Auth
{
	public class AccountCreateAckMsg : NetMsgBase
	{
		public enum ENameResult
		{
			Invalid,
			Okay,
			NameShort
		}

		public enum EEmailResult
		{
			Invalid,
			Okay,
			Bad,
			AlreadyInUse
		}

		public bool success;

		public AccountCreateAckMsg.ENameResult nameResult;

		public AccountCreateAckMsg.EEmailResult emailResult;

		public AccountCreateAckMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 6u;
		}

		public override void SerializeMsg()
		{
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.success);
			uint num = 0u;
			base.Unpack(ref num);
			this.nameResult = (AccountCreateAckMsg.ENameResult)num;
			base.Unpack(ref num);
			this.emailResult = (AccountCreateAckMsg.EEmailResult)num;
		}
	}
}
