// Decompile from assembly: Assembly-CSharp.dll

using MatchMaking;
using network;
using System;

namespace P2P
{
	public class SP2PMatchBasicPlayerDesc : SBasicMatchPlayerDesc
	{
		public ulong steamID;

		public override void Pack(NetMsgBase msg)
		{
			base.Pack(msg);
			msg.Pack(this.steamID);
		}

		public override void Unpack(NetMsgBase msg)
		{
			base.Unpack(msg);
			msg.Unpack(ref this.steamID);
		}
	}
}
