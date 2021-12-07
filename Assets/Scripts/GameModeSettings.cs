// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameModeSettings
{
	public const int MAX_NUMBER_OF_WINNERS = 3;

	public bool usesLives;

	public bool usesTime;

	public bool usesAssists;

	public int defaultAssistsCount = 2;

	public bool usesTeams;

	public TeamMode teamMode = TeamMode.FreeTeams;

	public bool requireEvenTeams;

	public bool defaultTeamAttackOn = true;

	public bool allowLifeSharing = true;

	public int defaultLivesCount = 4;

	public int defaultTimeMinutes = 8;

	public bool announceTeamFinalStock;

	public bool usesPlayerBarUI = true;

	public bool usesTeamBarUI;

	public bool usesFloatyNamesUI = true;

	public bool usesOffScreenArrows = true;

	public bool usesCrewBattleBottomBar;

	public bool sortVictorsByTeam;

	public bool disablePausing;

	public int maxPlayerCount = 8;

	public int minPlayerCount = 2;

	public bool hasFixedLivesCount;

	public int fixedLivesCount = 1;

	public bool allowPlayerGust = true;

	public float playerGustRadiusMultiplier = 1f;

	public string characterSelectionAnnouncement = string.Empty;

	public List<GameObject> customPlayerGUIPrefabs;
}
