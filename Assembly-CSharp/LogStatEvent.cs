using System;

// Token: 0x02000AD9 RID: 2777
public class LogStatEvent : GameEvent
{
	// Token: 0x060050BE RID: 20670 RVA: 0x001506D3 File Offset: 0x0014EAD3
	public LogStatEvent(StatType stat, int value, PointsValueType operation, PlayerNum player, TeamNum team)
	{
		this.stat = stat;
		this.value = value;
		this.player = player;
		this.team = team;
		this.operation = operation;
	}

	// Token: 0x04003405 RID: 13317
	public StatType stat;

	// Token: 0x04003406 RID: 13318
	public int value;

	// Token: 0x04003407 RID: 13319
	public PlayerNum player;

	// Token: 0x04003408 RID: 13320
	public TeamNum team;

	// Token: 0x04003409 RID: 13321
	public PointsValueType operation;
}
