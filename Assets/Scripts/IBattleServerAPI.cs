// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using MatchMaking;
using P2P;
using System;
using System.Collections.Generic;

public interface IBattleServerAPI
{
	bool IsConnected
	{
		get;
	}

	bool IsOnlineMatchReady
	{
		get;
	}

	bool IsSinglePlayerNetworkGame
	{
		get;
	}

	int ServerTimestepDelta
	{
		get;
	}

	ulong ServerPing
	{
		get;
	}

	Guid MatchID
	{
		get;
	}

	int NumTeams
	{
		get;
	}

	int CurrentMatchIndex
	{
		get;
	}

	int NumMatches
	{
		get;
	}

	List<StageID> Stages
	{
		get;
	}

	List<PlayerNum> LocalPlayerNumIDs
	{
		get;
	}

	PlayerNum GetPrimaryLocalPlayer
	{
		get;
	}

	int PlayerCount
	{
		get;
	}

	int NetworkBytesSent
	{
		get;
	}

	int NetworkBytesReceived
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

	float SelectionTime
	{
		get;
	}

	bool ReceivedMatchResults
	{
		get;
	}

	Dictionary<TeamNum, List<SBasicMatchPlayerDesc>> TeamPlayers
	{
		get;
	}

	Action<SP2PMatchBasicPlayerDesc[]> OnMatchReady
	{
		get;
		set;
	}

	Action<bool> OnLeftRoom
	{
		get;
		set;
	}

	void Initialize();

	void OnDestroy();

	void QueueUnreliableMessage(BatchEvent msg);

	void Listen<T>(ServerEventHandler handler) where T : ServerEvent;

	void Listen(Type type, ServerEventHandler handler);

	void Unsubscribe<T>(ServerEventHandler handler) where T : ServerEvent;

	void Unsubscribe(Type type, ServerEventHandler handler);

	PlayerNum GetPlayerNum(int index);

	void StageLoaded();

	void SendWinner(List<TeamNum> winningTeams);

	void ClearNetUsage();

	void LeaveRoom(bool clientExpectsSetToEnd);

	void ResetRoom();

	void LockInSelection(ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom);

	bool IsLocalPlayer(PlayerNum playerNum);

	bool ResultsForMatch(int MatchIndex);
}
