using System;
using System.Collections.Generic;

// Token: 0x0200060E RID: 1550
public interface IPlayerTauntsFinder
{
	// Token: 0x06002638 RID: 9784
	UserTaunts GetForPlayer(PlayerNum player);

	// Token: 0x06002639 RID: 9785
	List<MoveData> GetPlayerEmoteMoveData(PlayerNum playerNum, CharacterID characterID);

	// Token: 0x0600263A RID: 9786
	List<HologramData> GetPlayerHologramData(PlayerNum playerNum, CharacterID characterID);
}
