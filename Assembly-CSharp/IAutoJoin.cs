using System;

// Token: 0x020006D5 RID: 1749
public interface IAutoJoin
{
	// Token: 0x06002BEA RID: 11242
	void Clear();

	// Token: 0x06002BEB RID: 11243
	bool AutoJoinIfSet();

	// Token: 0x17000AC6 RID: 2758
	// (get) Token: 0x06002BEC RID: 11244
	string LobbyName { get; }

	// Token: 0x17000AC7 RID: 2759
	// (get) Token: 0x06002BED RID: 11245
	string LobbyPassword { get; }

	// Token: 0x17000AC8 RID: 2760
	// (get) Token: 0x06002BEE RID: 11246
	ulong LobbySteamID { get; }
}
