using System;

// Token: 0x02000AE6 RID: 2790
public class PlayerEngagementStateChangedEvent : GameEvent
{
	// Token: 0x060050CB RID: 20683 RVA: 0x001507E7 File Offset: 0x0014EBE7
	public PlayerEngagementStateChangedEvent(PlayerNum player, TeamNum team, PlayerEngagementState engagement)
	{
		this.engagement = engagement;
		this.playerNum = player;
		this.team = team;
	}

	// Token: 0x0400341B RID: 13339
	public PlayerEngagementState engagement;

	// Token: 0x0400341C RID: 13340
	public PlayerNum playerNum;

	// Token: 0x0400341D RID: 13341
	public TeamNum team;
}
