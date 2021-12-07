using System;

// Token: 0x02000AEA RID: 2794
public class RequestTagInCommand : GameEvent
{
	// Token: 0x060050D0 RID: 20688 RVA: 0x00150891 File Offset: 0x0014EC91
	public RequestTagInCommand(PlayerNum player, TeamNum team)
	{
		this.playerNum = player;
		this.team = team;
	}

	// Token: 0x0400342B RID: 13355
	public PlayerNum playerNum;

	// Token: 0x0400342C RID: 13356
	public TeamNum team;
}
