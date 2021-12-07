// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IMatchHistory
{
	VictoryScreenPayload LastVictoryPayload
	{
		get;
	}

	CharacterID GetFirstLocalCharacterID(VictoryScreenPayload victoryPayload);
}
