// Decompile from assembly: Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;

public interface ICustomLobbyController
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

	int NumLobbyPlayers
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

	ulong MyUserID
	{
		get;
	}

	bool IsTeams
	{
		get;
	}

	bool IsLobbyInMatch
	{
		get;
	}

	bool IsSpectator
	{
		get;
	}

	bool IsValidPlayerConfiguration();

	bool IsAllPlayersReadyForCSS();

	void Initialize();

	void Create(string lobbyName, string lobbyPass, ELobbyType lobbyType);

	void SetStage(StageID stageID);

	void SetMode(LobbyGameMode modeID);

	void ChangePlayer(ulong userID, bool isSpectating, int team);

	void HostChangePlayer(ulong userID, bool isSpectating, int team);

	void HostReceivedScreenChanged(ulong userID, int screenID);

	void ReceivedPlayerChange(ulong userID, bool isSpectating, int team);

	void ReceivedScreenChanged(ulong userID, int screenID);

	void ReceivedIsLobbyInMatch(bool isInMatch);

	void Leave();

	void StartMatch();
}
