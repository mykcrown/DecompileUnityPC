// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace MatchMaking
{
	public class SBasicMatchPlayerDesc
	{
		public string name;

		public ulong userID;

		public bool isSpectator;

		public byte team;

		public virtual void Pack(NetMsgBase msg)
		{
			msg.Pack(this.name);
			msg.Pack(this.userID);
			msg.Pack(this.isSpectator);
			msg.Pack(this.team);
		}

		public virtual void Unpack(NetMsgBase msg)
		{
			msg.Unpack(ref this.name);
			msg.Unpack(ref this.userID);
			msg.Unpack(ref this.isSpectator);
			msg.Unpack(ref this.team);
		}
	}
}
