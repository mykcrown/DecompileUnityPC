using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x02000499 RID: 1177
public interface ICrewAssistSpawnHelper
{
	// Token: 0x060019E1 RID: 6625
	void BeginContext(PlayerController controller, Dictionary<TeamNum, List<PlayerReference>> teamPlayers);

	// Token: 0x060019E2 RID: 6626
	bool CannotProceed();

	// Token: 0x060019E3 RID: 6627
	void PrepareForAssist();

	// Token: 0x060019E4 RID: 6628
	bool PrepareForPowerMove();

	// Token: 0x1700055B RID: 1371
	// (get) Token: 0x060019E5 RID: 6629
	Vector3F SpawnPoint { get; }

	// Token: 0x1700055C RID: 1372
	// (get) Token: 0x060019E6 RID: 6630
	Vector3F TargetPoint { get; }

	// Token: 0x1700055D RID: 1373
	// (get) Token: 0x060019E7 RID: 6631
	HorizontalDirection Facing { get; }

	// Token: 0x1700055E RID: 1374
	// (get) Token: 0x060019E8 RID: 6632
	AssistAttackComponent MoveComponent { get; }

	// Token: 0x1700055F RID: 1375
	// (get) Token: 0x060019E9 RID: 6633
	MoveData Move { get; }

	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x060019EA RID: 6634
	PlayerController Controller { get; }

	// Token: 0x17000561 RID: 1377
	// (get) Token: 0x060019EB RID: 6635
	PlayerReference Teammate { get; }

	// Token: 0x17000562 RID: 1378
	// (get) Token: 0x060019EC RID: 6636
	PlayerReference Opponent { get; }

	// Token: 0x060019ED RID: 6637
	PlayerReference GetTeammate();

	// Token: 0x060019EE RID: 6638
	PlayerReference GetOpponent();
}
