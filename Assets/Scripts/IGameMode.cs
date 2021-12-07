// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IGameMode : ITickable, IRollbackStateOwner
{
	List<EndGameCondition> EndGameConditions
	{
		get;
	}

	float CurrentSeconds
	{
		get;
	}

	bool ShouldDisplayTimer
	{
		get;
	}

	bool IsGameComplete
	{
		get;
	}

	void Init(GameModeData modeData, ConfigData config, BattleSettings settings, IEvents events, List<PlayerReference> playerReferences, IFrameOwner frameOwner);

	void Destroy();

	void TickUpdate();

	PlayerSpawner CreateSpawner(GameManager manager, Dictionary<PlayerNum, PlayerReference> references);
}
