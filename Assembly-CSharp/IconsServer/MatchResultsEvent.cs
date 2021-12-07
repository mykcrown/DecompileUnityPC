using System;

namespace IconsServer
{
	// Token: 0x020007E8 RID: 2024
	public class MatchResultsEvent : ServerEvent
	{
		// Token: 0x06003201 RID: 12801 RVA: 0x000F2629 File Offset: 0x000F0A29
		public MatchResultsEvent(Guid matchId, byte winningTeamMask)
		{
			this.matchId = matchId;
			this.winningTeamMask = winningTeamMask;
		}

		// Token: 0x04002337 RID: 9015
		public Guid matchId;

		// Token: 0x04002338 RID: 9016
		public byte winningTeamMask;
	}
}
