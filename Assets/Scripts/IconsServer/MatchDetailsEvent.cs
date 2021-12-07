// Decompile from assembly: Assembly-CSharp.dll

using P2P;
using System;

namespace IconsServer
{
	public class MatchDetailsEvent : ServerEvent
	{
		public Guid matchId;

		public P2PMatchDetailsMsg.SPlayerDesc[] players;

		public MatchDetailsEvent(Guid matchId, P2PMatchDetailsMsg.SPlayerDesc[] players)
		{
			this.matchId = matchId;
			this.players = players;
		}
	}
}
