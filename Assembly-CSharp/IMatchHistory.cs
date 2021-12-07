using System;

// Token: 0x02000496 RID: 1174
public interface IMatchHistory
{
	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x060019B0 RID: 6576
	VictoryScreenPayload LastVictoryPayload { get; }

	// Token: 0x060019B1 RID: 6577
	CharacterID GetFirstLocalCharacterID(VictoryScreenPayload victoryPayload);
}
