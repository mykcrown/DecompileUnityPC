using System;

// Token: 0x02000ADD RID: 2781
public class ComboKillEvent : GameEvent
{
	// Token: 0x060050C2 RID: 20674 RVA: 0x00150736 File Offset: 0x0014EB36
	public ComboKillEvent(PlayerNum killedPlayer, TeamNum killedTeam, PlayerNum comboerPlayer, TeamNum comboerTeam)
	{
		this.killedPlayer = killedPlayer;
		this.killedTeam = killedTeam;
		this.comboerPlayer = comboerPlayer;
		this.comboerTeam = comboerTeam;
	}

	// Token: 0x0400340E RID: 13326
	public PlayerNum killedPlayer;

	// Token: 0x0400340F RID: 13327
	public TeamNum killedTeam;

	// Token: 0x04003410 RID: 13328
	public PlayerNum comboerPlayer;

	// Token: 0x04003411 RID: 13329
	public TeamNum comboerTeam;
}
