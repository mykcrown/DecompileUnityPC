using System;

// Token: 0x02000AFB RID: 2811
public class ShareLifeCommand : GameEvent
{
	// Token: 0x060050E4 RID: 20708 RVA: 0x001509A3 File Offset: 0x0014EDA3
	public ShareLifeCommand(PlayerNum player, TeamNum team)
	{
		this.player = player;
		this.team = team;
	}

	// Token: 0x0400343F RID: 13375
	public PlayerNum player;

	// Token: 0x04003440 RID: 13376
	public TeamNum team;
}
