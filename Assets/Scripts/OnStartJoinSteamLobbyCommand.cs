// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class OnStartJoinSteamLobbyCommand : GameEvent
{
	public ulong SteamLobbyID;

	public OnStartJoinSteamLobbyCommand(ulong SteamLobbyID)
	{
		UnityEngine.Debug.LogFormat("Steam Lobby Join Request: [{0}]", new object[]
		{
			SteamLobbyID
		});
		this.SteamLobbyID = SteamLobbyID;
	}
}
