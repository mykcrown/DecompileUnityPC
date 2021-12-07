// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class OnStartJoinLobbyCommand : GameEvent
{
	public string LobbyName;

	public string LobbyPassword;

	public OnStartJoinLobbyCommand(string LobbyName, string LobbyPassword)
	{
		UnityEngine.Debug.LogFormat("Steam Lobby Join Request: [{0}] [{1}]", new object[]
		{
			LobbyName,
			LobbyPassword
		});
		this.LobbyName = LobbyName;
		this.LobbyPassword = LobbyPassword;
	}
}
