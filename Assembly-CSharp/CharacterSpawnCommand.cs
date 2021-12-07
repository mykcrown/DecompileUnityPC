using System;

// Token: 0x02000ADE RID: 2782
public class CharacterSpawnCommand : GameEvent
{
	// Token: 0x060050C3 RID: 20675 RVA: 0x0015075B File Offset: 0x0014EB5B
	public CharacterSpawnCommand(PlayerNum player, TeamNum team, PlayerEngagementState spawnType, SpawnPointBase spawnPoint, int temporarySpawnDurationFrames = -1, int startingDamagePercent = 0)
	{
		this.spawnPoint = spawnPoint;
		this.player = player;
		this.team = team;
		this.spawnType = spawnType;
		this.temporarySpawnDurationFrames = temporarySpawnDurationFrames;
		this.startingDamagePercent = startingDamagePercent;
	}

	// Token: 0x04003412 RID: 13330
	public PlayerNum player;

	// Token: 0x04003413 RID: 13331
	public TeamNum team;

	// Token: 0x04003414 RID: 13332
	public PlayerEngagementState spawnType;

	// Token: 0x04003415 RID: 13333
	public int temporarySpawnDurationFrames = -1;

	// Token: 0x04003416 RID: 13334
	public int startingDamagePercent;

	// Token: 0x04003417 RID: 13335
	public SpawnPointBase spawnPoint;
}
