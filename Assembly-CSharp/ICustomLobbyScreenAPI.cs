using System;
using System.Collections.Generic;
using Steamworks;

// Token: 0x0200091F RID: 2335
public interface ICustomLobbyScreenAPI
{
	// Token: 0x17000E8B RID: 3723
	// (get) Token: 0x06003CE3 RID: 15587
	bool IsInLobby { get; }

	// Token: 0x17000E8C RID: 3724
	// (get) Token: 0x06003CE4 RID: 15588
	string LobbyName { get; }

	// Token: 0x17000E8D RID: 3725
	// (get) Token: 0x06003CE5 RID: 15589
	string LobbyPassword { get; }

	// Token: 0x17000E8E RID: 3726
	// (get) Token: 0x06003CE6 RID: 15590
	StageID StageID { get; }

	// Token: 0x17000E8F RID: 3727
	// (get) Token: 0x06003CE7 RID: 15591
	LobbyGameMode ModeID { get; }

	// Token: 0x17000E90 RID: 3728
	// (get) Token: 0x06003CE8 RID: 15592
	bool IsTeams { get; }

	// Token: 0x17000E91 RID: 3729
	// (get) Token: 0x06003CE9 RID: 15593
	bool IsLobbyLeader { get; }

	// Token: 0x17000E92 RID: 3730
	// (get) Token: 0x06003CEA RID: 15594
	ulong HostUserId { get; }

	// Token: 0x17000E93 RID: 3731
	// (get) Token: 0x06003CEB RID: 15595
	LobbyEvent LastEvent { get; }

	// Token: 0x17000E94 RID: 3732
	// (get) Token: 0x06003CEC RID: 15596
	Dictionary<ulong, LobbyPlayerData> Players { get; }

	// Token: 0x06003CED RID: 15597
	bool IsValidPlayerConfiguration();

	// Token: 0x06003CEE RID: 15598
	bool IsAllPlayersReadyForCSS();

	// Token: 0x06003CEF RID: 15599
	void Initialize();

	// Token: 0x06003CF0 RID: 15600
	void OnDestroy();

	// Token: 0x06003CF1 RID: 15601
	void Create(string lobbyName, string lobbyPass, ELobbyType lobbyType);

	// Token: 0x06003CF2 RID: 15602
	void SetStage(StageID stageID);

	// Token: 0x06003CF3 RID: 15603
	void SetMode(LobbyGameMode modeID);

	// Token: 0x06003CF4 RID: 15604
	void Leave();

	// Token: 0x06003CF5 RID: 15605
	void StartMatch();

	// Token: 0x17000E95 RID: 3733
	// (get) Token: 0x06003CF6 RID: 15606
	bool IsLobbyInMatch { get; }

	// Token: 0x06003CF7 RID: 15607
	void ChangePlayer(ulong userID, bool isSpectating, int team);
}
