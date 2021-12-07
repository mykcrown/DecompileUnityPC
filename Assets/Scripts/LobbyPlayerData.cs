// Decompile from assembly: Assembly-CSharp.dll

using System;

public class LobbyPlayerData
{
	public string name;

	public ulong userID;

	public bool isSpectator;

	public int team;

	public ScreenType currentScreen;

	public LobbyPlayerData(string name, ulong userID, ScreenType currentScreen)
	{
		this.name = name;
		this.userID = userID;
		this.currentScreen = currentScreen;
	}
}
