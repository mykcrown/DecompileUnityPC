// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IRoomData
{
	int playerCount
	{
		get;
	}

	byte maxPlayers
	{
		get;
	}

	bool visible
	{
		get;
	}

	string name
	{
		get;
	}

	int masterClientId
	{
		get;
	}
}
