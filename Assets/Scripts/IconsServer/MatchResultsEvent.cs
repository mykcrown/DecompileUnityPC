// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace IconsServer
{
	public class MatchResultsEvent : ServerEvent
	{
		public Guid matchId;

		public byte winningTeamMask;

		public MatchResultsEvent(Guid matchId, byte winningTeamMask)
		{
			this.matchId = matchId;
			this.winningTeamMask = winningTeamMask;
		}
	}
}
