// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IBenchedPlayerSpawner
{
	int GetAssistsUsed(PlayerNum player);

	int GetAssistsRemaining(PlayerNum player);

	bool IsRespawning(PlayerNum player);

	PlayerNum GetPrimaryPlayer(TeamNum team);

	PlayerReference GetPlayerRef(PlayerNum playerNum);

	int GetRespawnDelayFrames(PlayerNum player);

	PlayerEngagementState GetSpawnType(PlayerNum player);

	bool CanTagIn(PlayerNum player);

	bool CanAssist(PlayerReference playerRef);

	bool DisplayTagInOptionInHUD();
}
