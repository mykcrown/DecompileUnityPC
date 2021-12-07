using System;

// Token: 0x02000AE8 RID: 2792
public class AttemptTeamDynamicMoveCommand : GameEvent
{
	// Token: 0x060050CE RID: 20686 RVA: 0x00150837 File Offset: 0x0014EC37
	public AttemptTeamDynamicMoveCommand(PlayerNum playerNum, TeamNum team, ParticleData spawnParticle, int cooldownFrames)
	{
		this.playerNum = playerNum;
		this.team = team;
		this.spawnParticle = spawnParticle;
		this.cooldownFrames = cooldownFrames;
	}

	// Token: 0x04003421 RID: 13345
	public PlayerNum playerNum;

	// Token: 0x04003422 RID: 13346
	public TeamNum team;

	// Token: 0x04003423 RID: 13347
	public ParticleData spawnParticle;

	// Token: 0x04003424 RID: 13348
	public int cooldownFrames;
}
