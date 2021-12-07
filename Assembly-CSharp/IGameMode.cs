using System;
using System.Collections.Generic;

// Token: 0x020004AB RID: 1195
public interface IGameMode : ITickable, IRollbackStateOwner
{
	// Token: 0x06001A68 RID: 6760
	void Init(GameModeData modeData, ConfigData config, BattleSettings settings, IEvents events, List<PlayerReference> playerReferences, IFrameOwner frameOwner);

	// Token: 0x17000577 RID: 1399
	// (get) Token: 0x06001A69 RID: 6761
	List<EndGameCondition> EndGameConditions { get; }

	// Token: 0x06001A6A RID: 6762
	void Destroy();

	// Token: 0x17000578 RID: 1400
	// (get) Token: 0x06001A6B RID: 6763
	float CurrentSeconds { get; }

	// Token: 0x17000579 RID: 1401
	// (get) Token: 0x06001A6C RID: 6764
	bool ShouldDisplayTimer { get; }

	// Token: 0x06001A6D RID: 6765
	void TickUpdate();

	// Token: 0x1700057A RID: 1402
	// (get) Token: 0x06001A6E RID: 6766
	bool IsGameComplete { get; }

	// Token: 0x06001A6F RID: 6767
	PlayerSpawner CreateSpawner(GameManager manager, Dictionary<PlayerNum, PlayerReference> references);
}
