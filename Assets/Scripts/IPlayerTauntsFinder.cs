// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IPlayerTauntsFinder
{
	UserTaunts GetForPlayer(PlayerNum player);

	List<MoveData> GetPlayerEmoteMoveData(PlayerNum playerNum, CharacterID characterID);

	List<HologramData> GetPlayerHologramData(PlayerNum playerNum, CharacterID characterID);
}
