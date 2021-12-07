using System;

// Token: 0x020007D4 RID: 2004
public class LobbyPlayerData
{
	// Token: 0x060031D0 RID: 12752 RVA: 0x000F2267 File Offset: 0x000F0667
	public LobbyPlayerData(string name, ulong userID, ScreenType currentScreen)
	{
		this.name = name;
		this.userID = userID;
		this.currentScreen = currentScreen;
	}

	// Token: 0x040022CB RID: 8907
	public string name;

	// Token: 0x040022CC RID: 8908
	public ulong userID;

	// Token: 0x040022CD RID: 8909
	public bool isSpectator;

	// Token: 0x040022CE RID: 8910
	public int team;

	// Token: 0x040022CF RID: 8911
	public ScreenType currentScreen;
}
