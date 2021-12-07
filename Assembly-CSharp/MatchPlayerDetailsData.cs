using System;
using System.Collections.Generic;

// Token: 0x020007A4 RID: 1956
public struct MatchPlayerDetailsData
{
	// Token: 0x040021DF RID: 8671
	public string playerName;

	// Token: 0x040021E0 RID: 8672
	public ulong userID;

	// Token: 0x040021E1 RID: 8673
	public bool isSpectator;

	// Token: 0x040021E2 RID: 8674
	public PlayerNum playerNum;

	// Token: 0x040021E3 RID: 8675
	public CharacterID characterID;

	// Token: 0x040021E4 RID: 8676
	public int skinID;

	// Token: 0x040021E5 RID: 8677
	public int characterIndex;

	// Token: 0x040021E6 RID: 8678
	public List<EquipmentID> equippedToCharacter;

	// Token: 0x040021E7 RID: 8679
	public List<EquipmentID> equippedToPlayer;
}
