// Decompile from assembly: Assembly-CSharp.dll

using System;

public class CharacterSpawnCommand : GameEvent
{
	public PlayerNum player;

	public TeamNum team;

	public PlayerEngagementState spawnType;

	public int temporarySpawnDurationFrames = -1;

	public int startingDamagePercent;

	public SpawnPointBase spawnPoint;

	public CharacterSpawnCommand(PlayerNum player, TeamNum team, PlayerEngagementState spawnType, SpawnPointBase spawnPoint, int temporarySpawnDurationFrames = -1, int startingDamagePercent = 0)
	{
		this.spawnPoint = spawnPoint;
		this.player = player;
		this.team = team;
		this.spawnType = spawnType;
		this.temporarySpawnDurationFrames = temporarySpawnDurationFrames;
		this.startingDamagePercent = startingDamagePercent;
	}
}
