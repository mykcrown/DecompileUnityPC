// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IAutoJoin
{
	string LobbyName
	{
		get;
	}

	string LobbyPassword
	{
		get;
	}

	ulong LobbySteamID
	{
		get;
	}

	void Clear();

	bool AutoJoinIfSet();
}
