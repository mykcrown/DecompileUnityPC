using System;
using System.Collections.Generic;

// Token: 0x02000AE5 RID: 2789
public class MatchResultMessage : GameEvent
{
	// Token: 0x060050CA RID: 20682 RVA: 0x001507D8 File Offset: 0x0014EBD8
	public MatchResultMessage(List<TeamNum> winningTeams)
	{
		this.winningTeams = winningTeams;
	}

	// Token: 0x0400341A RID: 13338
	public List<TeamNum> winningTeams;
}
