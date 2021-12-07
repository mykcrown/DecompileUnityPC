// Decompile from assembly: Assembly-CSharp.dll

using System;

public class AttemptTeamDynamicMoveCommand : GameEvent
{
	public PlayerNum playerNum;

	public TeamNum team;

	public ParticleData spawnParticle;

	public int cooldownFrames;

	public AttemptTeamDynamicMoveCommand(PlayerNum playerNum, TeamNum team, ParticleData spawnParticle, int cooldownFrames)
	{
		this.playerNum = playerNum;
		this.team = team;
		this.spawnParticle = spawnParticle;
		this.cooldownFrames = cooldownFrames;
	}
}
