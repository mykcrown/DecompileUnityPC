using System;
using System.Collections.Generic;

// Token: 0x02000AD2 RID: 2770
public class TogglePlayerVisibilityCommand : GameEvent
{
	// Token: 0x060050B3 RID: 20659 RVA: 0x0015062C File Offset: 0x0014EA2C
	public TogglePlayerVisibilityCommand(List<PlayerNum> players, bool visibility)
	{
		this.players = players;
		this.visibility = visibility;
	}

	// Token: 0x040033FA RID: 13306
	public List<PlayerNum> players;

	// Token: 0x040033FB RID: 13307
	public bool visibility;
}
