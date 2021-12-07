using System;

// Token: 0x02000ADF RID: 2783
public class DespawnCharacterCommand : GameEvent
{
	// Token: 0x060050C4 RID: 20676 RVA: 0x00150797 File Offset: 0x0014EB97
	public DespawnCharacterCommand(PlayerNum player, TeamNum team)
	{
		this.player = player;
		this.team = team;
	}

	// Token: 0x04003418 RID: 13336
	public PlayerNum player;

	// Token: 0x04003419 RID: 13337
	public TeamNum team;
}
