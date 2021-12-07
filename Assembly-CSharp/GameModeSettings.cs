using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004A6 RID: 1190
[Serializable]
public class GameModeSettings
{
	// Token: 0x04001397 RID: 5015
	public const int MAX_NUMBER_OF_WINNERS = 3;

	// Token: 0x04001398 RID: 5016
	public bool usesLives;

	// Token: 0x04001399 RID: 5017
	public bool usesTime;

	// Token: 0x0400139A RID: 5018
	public bool usesAssists;

	// Token: 0x0400139B RID: 5019
	public int defaultAssistsCount = 2;

	// Token: 0x0400139C RID: 5020
	public bool usesTeams;

	// Token: 0x0400139D RID: 5021
	public TeamMode teamMode = TeamMode.FreeTeams;

	// Token: 0x0400139E RID: 5022
	public bool requireEvenTeams;

	// Token: 0x0400139F RID: 5023
	public bool defaultTeamAttackOn = true;

	// Token: 0x040013A0 RID: 5024
	public bool allowLifeSharing = true;

	// Token: 0x040013A1 RID: 5025
	public int defaultLivesCount = 4;

	// Token: 0x040013A2 RID: 5026
	public int defaultTimeMinutes = 8;

	// Token: 0x040013A3 RID: 5027
	public bool announceTeamFinalStock;

	// Token: 0x040013A4 RID: 5028
	public bool usesPlayerBarUI = true;

	// Token: 0x040013A5 RID: 5029
	public bool usesTeamBarUI;

	// Token: 0x040013A6 RID: 5030
	public bool usesFloatyNamesUI = true;

	// Token: 0x040013A7 RID: 5031
	public bool usesOffScreenArrows = true;

	// Token: 0x040013A8 RID: 5032
	public bool usesCrewBattleBottomBar;

	// Token: 0x040013A9 RID: 5033
	public bool sortVictorsByTeam;

	// Token: 0x040013AA RID: 5034
	public bool disablePausing;

	// Token: 0x040013AB RID: 5035
	public int maxPlayerCount = 8;

	// Token: 0x040013AC RID: 5036
	public int minPlayerCount = 2;

	// Token: 0x040013AD RID: 5037
	public bool hasFixedLivesCount;

	// Token: 0x040013AE RID: 5038
	public int fixedLivesCount = 1;

	// Token: 0x040013AF RID: 5039
	public bool allowPlayerGust = true;

	// Token: 0x040013B0 RID: 5040
	public float playerGustRadiusMultiplier = 1f;

	// Token: 0x040013B1 RID: 5041
	public string characterSelectionAnnouncement = string.Empty;

	// Token: 0x040013B2 RID: 5042
	public List<GameObject> customPlayerGUIPrefabs;
}
