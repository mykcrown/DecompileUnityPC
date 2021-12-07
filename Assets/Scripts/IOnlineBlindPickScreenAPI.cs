// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IOnlineBlindPickScreenAPI
{
	ulong MyUserID
	{
		get;
	}

	bool IsTeams
	{
		get;
	}

	bool CanLockInSelection
	{
		get;
	}

	bool CanStartGame
	{
		get;
	}

	bool LockedIn
	{
		get;
	}

	bool IsSpectator
	{
		get;
	}

	void OnScreenShown();

	void OnScreenDestroyed();

	void LeaveRoom();

	void LockInSelection();

	Dictionary<ulong, LobbyPlayerData> GetLobbyPlayers();
}
