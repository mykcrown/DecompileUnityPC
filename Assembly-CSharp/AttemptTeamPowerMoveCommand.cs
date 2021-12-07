using System;

// Token: 0x02000AE9 RID: 2793
public class AttemptTeamPowerMoveCommand : GameEvent
{
	// Token: 0x060050CF RID: 20687 RVA: 0x0015085C File Offset: 0x0014EC5C
	public AttemptTeamPowerMoveCommand(PlayerNum playerNum, TeamNum team, ParticleData spawnParticle, int cooldownFrames, int invulnerableFrames, CreateArticleAction[] assistArticles)
	{
		this.playerNum = playerNum;
		this.team = team;
		this.spawnParticle = spawnParticle;
		this.cooldownFrames = cooldownFrames;
		this.immunityFrames = invulnerableFrames;
		this.assistArticles = assistArticles;
	}

	// Token: 0x04003425 RID: 13349
	public PlayerNum playerNum;

	// Token: 0x04003426 RID: 13350
	public TeamNum team;

	// Token: 0x04003427 RID: 13351
	public ParticleData spawnParticle;

	// Token: 0x04003428 RID: 13352
	public int cooldownFrames;

	// Token: 0x04003429 RID: 13353
	public int immunityFrames;

	// Token: 0x0400342A RID: 13354
	public CreateArticleAction[] assistArticles;
}
