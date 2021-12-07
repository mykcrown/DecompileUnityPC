// Decompile from assembly: Assembly-CSharp.dll

using MatchMaking;
using System;
using System.Collections.Generic;

public interface IBattleServerStagingAPI
{
	Guid MatchID
	{
		get;
	}

	float SelectionEndTime
	{
		get;
	}

	float PurchaseEndTime
	{
		get;
	}

	int NumMatches
	{
		get;
	}

	int CurrentMatchIndex
	{
		get;
	}

	List<StageID> Stages
	{
		get;
	}

	List<PlayerNum> LocalPlayerNumIds
	{
		get;
	}

	int NumLives
	{
		get;
	}

	int AssistCount
	{
		get;
	}

	int MatchTime
	{
		get;
	}

	Dictionary<TeamNum, List<SBasicMatchPlayerDesc>> TeamPlayers
	{
		get;
	}

	GameMode MatchGameMode
	{
		get;
	}

	GameRules MatchRules
	{
		get;
	}

	bool TeamAttack
	{
		get;
	}

	List<MatchPlayerDetailsData> PlayerDetails
	{
		get;
	}

	Action<bool, PlayerNum> OnLockInSelection
	{
		get;
		set;
	}

	Action OnMatchDetailsComplete
	{
		get;
		set;
	}

	bool ResultsForMatch(int MatchIndex);

	void ResetSelectionTimer();

	void OnRoomJoined();

	void OnRoomDestroyed();

	void LockInSelection(CharacterID characterID, int characterIndex, int skinID, bool isRandom);
}
