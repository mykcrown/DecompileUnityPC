using System;

// Token: 0x02000AE7 RID: 2791
public class AttemptFriendlyAssistCommand : GameEvent
{
	// Token: 0x060050CC RID: 20684 RVA: 0x00150804 File Offset: 0x0014EC04
	public AttemptFriendlyAssistCommand(PlayerNum player, TeamNum team)
	{
		this.playerNum = player;
		this.team = team;
	}

	// Token: 0x060050CD RID: 20685 RVA: 0x0015081A File Offset: 0x0014EC1A
	public AttemptFriendlyAssistCommand(PlayerNum player, TeamNum team, bool debugMode)
	{
		this.playerNum = player;
		this.team = team;
		this.debugMode = debugMode;
	}

	// Token: 0x0400341E RID: 13342
	public PlayerNum playerNum;

	// Token: 0x0400341F RID: 13343
	public TeamNum team;

	// Token: 0x04003420 RID: 13344
	public bool debugMode;
}
