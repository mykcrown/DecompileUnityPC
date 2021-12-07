using System;

// Token: 0x020004A4 RID: 1188
public interface IBenchedPlayerSpawner
{
	// Token: 0x06001A4A RID: 6730
	int GetAssistsUsed(PlayerNum player);

	// Token: 0x06001A4B RID: 6731
	int GetAssistsRemaining(PlayerNum player);

	// Token: 0x06001A4C RID: 6732
	bool IsRespawning(PlayerNum player);

	// Token: 0x06001A4D RID: 6733
	PlayerNum GetPrimaryPlayer(TeamNum team);

	// Token: 0x06001A4E RID: 6734
	PlayerReference GetPlayerRef(PlayerNum playerNum);

	// Token: 0x06001A4F RID: 6735
	int GetRespawnDelayFrames(PlayerNum player);

	// Token: 0x06001A50 RID: 6736
	PlayerEngagementState GetSpawnType(PlayerNum player);

	// Token: 0x06001A51 RID: 6737
	bool CanTagIn(PlayerNum player);

	// Token: 0x06001A52 RID: 6738
	bool CanAssist(PlayerReference playerRef);

	// Token: 0x06001A53 RID: 6739
	bool DisplayTagInOptionInHUD();
}
