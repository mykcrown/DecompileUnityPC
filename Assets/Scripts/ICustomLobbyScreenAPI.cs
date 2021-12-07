// Decompile from assembly: Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;

public interface ICustomLobbyScreenAPI
{
	bool IsInLobby
	{
		get;
	}

	string LobbyName
	{
		get;
	}

	string LobbyPassword
	{
		get;
	}

	StageID StageID
	{
		get;
	}

	LobbyGameMode ModeID
	{
		get;
	}

	bool IsTeams
	{
		get;
	}

	bool IsLobbyLeader
	{
		get;
	}

	ulong HostUserId
	{
		get;
	}

	LobbyEvent LastEvent
	{
		get;
	}

	Dictionary<ulong, LobbyPlayerData> Players
	{
		get;
	}

	bool IsLobbyInMatch
	{
		get;
	}

	bool IsValidPlayerConfiguration();

	bool IsAllPlayersReadyForCSS();

	void Initialize();

	void OnDestroy();

	void Create(string lobbyName, string lobbyPass, ELobbyType lobbyType);

	void SetStage(StageID stageID);

	void SetMode(LobbyGameMode modeID);

	void Leave();

	void StartMatch();

	void ChangePlayer(ulong userID, bool isSpectating, int team);
}
