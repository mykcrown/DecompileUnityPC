using System;
using System.Collections.Generic;

// Token: 0x02000AEE RID: 2798
public class GameEndEvent : GameEvent
{
	// Token: 0x060050D6 RID: 20694 RVA: 0x001508E5 File Offset: 0x0014ECE5
	public GameEndEvent(List<PlayerNum> winners, List<TeamNum> winningTeams)
	{
		this.winners = winners;
		this.winningTeams = winningTeams;
	}

	// Token: 0x04003430 RID: 13360
	public List<PlayerNum> winners;

	// Token: 0x04003431 RID: 13361
	public List<TeamNum> winningTeams;
}
