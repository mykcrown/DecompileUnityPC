using System;
using System.Collections.Generic;
using MatchMaking;

// Token: 0x020007A6 RID: 1958
public interface IBattleServerStagingAPI
{
	// Token: 0x17000BE1 RID: 3041
	// (get) Token: 0x060030D6 RID: 12502
	Guid MatchID { get; }

	// Token: 0x17000BE2 RID: 3042
	// (get) Token: 0x060030D7 RID: 12503
	float SelectionEndTime { get; }

	// Token: 0x17000BE3 RID: 3043
	// (get) Token: 0x060030D8 RID: 12504
	float PurchaseEndTime { get; }

	// Token: 0x17000BE4 RID: 3044
	// (get) Token: 0x060030D9 RID: 12505
	int NumMatches { get; }

	// Token: 0x17000BE5 RID: 3045
	// (get) Token: 0x060030DA RID: 12506
	int CurrentMatchIndex { get; }

	// Token: 0x17000BE6 RID: 3046
	// (get) Token: 0x060030DB RID: 12507
	List<StageID> Stages { get; }

	// Token: 0x17000BE7 RID: 3047
	// (get) Token: 0x060030DC RID: 12508
	List<PlayerNum> LocalPlayerNumIds { get; }

	// Token: 0x17000BE8 RID: 3048
	// (get) Token: 0x060030DD RID: 12509
	int NumLives { get; }

	// Token: 0x17000BE9 RID: 3049
	// (get) Token: 0x060030DE RID: 12510
	int AssistCount { get; }

	// Token: 0x17000BEA RID: 3050
	// (get) Token: 0x060030DF RID: 12511
	int MatchTime { get; }

	// Token: 0x17000BEB RID: 3051
	// (get) Token: 0x060030E0 RID: 12512
	Dictionary<TeamNum, List<SBasicMatchPlayerDesc>> TeamPlayers { get; }

	// Token: 0x17000BEC RID: 3052
	// (get) Token: 0x060030E1 RID: 12513
	GameMode MatchGameMode { get; }

	// Token: 0x17000BED RID: 3053
	// (get) Token: 0x060030E2 RID: 12514
	GameRules MatchRules { get; }

	// Token: 0x17000BEE RID: 3054
	// (get) Token: 0x060030E3 RID: 12515
	bool TeamAttack { get; }

	// Token: 0x17000BEF RID: 3055
	// (get) Token: 0x060030E4 RID: 12516
	List<MatchPlayerDetailsData> PlayerDetails { get; }

	// Token: 0x060030E5 RID: 12517
	bool ResultsForMatch(int MatchIndex);

	// Token: 0x060030E6 RID: 12518
	void ResetSelectionTimer();

	// Token: 0x060030E7 RID: 12519
	void OnRoomJoined();

	// Token: 0x060030E8 RID: 12520
	void OnRoomDestroyed();

	// Token: 0x060030E9 RID: 12521
	void LockInSelection(CharacterID characterID, int characterIndex, int skinID, bool isRandom);

	// Token: 0x17000BF0 RID: 3056
	// (get) Token: 0x060030EA RID: 12522
	// (set) Token: 0x060030EB RID: 12523
	Action<bool, PlayerNum> OnLockInSelection { get; set; }

	// Token: 0x17000BF1 RID: 3057
	// (get) Token: 0x060030EC RID: 12524
	// (set) Token: 0x060030ED RID: 12525
	Action OnMatchDetailsComplete { get; set; }
}
