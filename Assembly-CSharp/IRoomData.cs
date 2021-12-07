using System;

// Token: 0x02000807 RID: 2055
public interface IRoomData
{
	// Token: 0x17000C55 RID: 3157
	// (get) Token: 0x060032B7 RID: 12983
	int playerCount { get; }

	// Token: 0x17000C56 RID: 3158
	// (get) Token: 0x060032B8 RID: 12984
	byte maxPlayers { get; }

	// Token: 0x17000C57 RID: 3159
	// (get) Token: 0x060032B9 RID: 12985
	bool visible { get; }

	// Token: 0x17000C58 RID: 3160
	// (get) Token: 0x060032BA RID: 12986
	string name { get; }

	// Token: 0x17000C59 RID: 3161
	// (get) Token: 0x060032BB RID: 12987
	int masterClientId { get; }
}
