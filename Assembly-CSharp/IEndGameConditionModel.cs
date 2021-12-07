using System;
using System.Collections.Generic;

// Token: 0x02000671 RID: 1649
public interface IEndGameConditionModel
{
	// Token: 0x170009F5 RID: 2549
	// (get) Token: 0x060028BB RID: 10427
	// (set) Token: 0x060028BC RID: 10428
	bool IsFinished { get; set; }

	// Token: 0x170009F6 RID: 2550
	// (get) Token: 0x060028BD RID: 10429
	List<PlayerNum> Victors { get; }

	// Token: 0x170009F7 RID: 2551
	// (get) Token: 0x060028BE RID: 10430
	List<TeamNum> WinningTeams { get; }
}
