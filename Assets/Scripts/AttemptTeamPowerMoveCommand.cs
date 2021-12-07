// Decompile from assembly: Assembly-CSharp.dll

using System;

public class AttemptTeamPowerMoveCommand : GameEvent
{
	public PlayerNum playerNum;

	public TeamNum team;

	public ParticleData spawnParticle;

	public int cooldownFrames;

	public int immunityFrames;

	public CreateArticleAction[] assistArticles;

	public AttemptTeamPowerMoveCommand(PlayerNum playerNum, TeamNum team, ParticleData spawnParticle, int cooldownFrames, int invulnerableFrames, CreateArticleAction[] assistArticles)
	{
		this.playerNum = playerNum;
		this.team = team;
		this.spawnParticle = spawnParticle;
		this.cooldownFrames = cooldownFrames;
		this.immunityFrames = invulnerableFrames;
		this.assistArticles = assistArticles;
	}
}
