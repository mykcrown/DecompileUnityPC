using System;
using System.Collections.Generic;
using Steamworks;

// Token: 0x020007D3 RID: 2003
public interface ICustomLobbyController
{
	// Token: 0x17000C1D RID: 3101
	// (get) Token: 0x060031B4 RID: 12724
	bool IsInLobby { get; }

	// Token: 0x17000C1E RID: 3102
	// (get) Token: 0x060031B5 RID: 12725
	string LobbyName { get; }

	// Token: 0x17000C1F RID: 3103
	// (get) Token: 0x060031B6 RID: 12726
	string LobbyPassword { get; }

	// Token: 0x17000C20 RID: 3104
	// (get) Token: 0x060031B7 RID: 12727
	StageID StageID { get; }

	// Token: 0x17000C21 RID: 3105
	// (get) Token: 0x060031B8 RID: 12728
	LobbyGameMode ModeID { get; }

	// Token: 0x17000C22 RID: 3106
	// (get) Token: 0x060031B9 RID: 12729
	int NumLobbyPlayers { get; }

	// Token: 0x17000C23 RID: 3107
	// (get) Token: 0x060031BA RID: 12730
	bool IsLobbyLeader { get; }

	// Token: 0x17000C24 RID: 3108
	// (get) Token: 0x060031BB RID: 12731
	ulong HostUserId { get; }

	// Token: 0x17000C25 RID: 3109
	// (get) Token: 0x060031BC RID: 12732
	LobbyEvent LastEvent { get; }

	// Token: 0x17000C26 RID: 3110
	// (get) Token: 0x060031BD RID: 12733
	Dictionary<ulong, LobbyPlayerData> Players { get; }

	// Token: 0x060031BE RID: 12734
	bool IsValidPlayerConfiguration();

	// Token: 0x060031BF RID: 12735
	bool IsAllPlayersReadyForCSS();

	// Token: 0x17000C27 RID: 3111
	// (get) Token: 0x060031C0 RID: 12736
	ulong MyUserID { get; }

	// Token: 0x17000C28 RID: 3112
	// (get) Token: 0x060031C1 RID: 12737
	bool IsTeams { get; }

	// Token: 0x060031C2 RID: 12738
	void Initialize();

	// Token: 0x060031C3 RID: 12739
	void Create(string lobbyName, string lobbyPass, ELobbyType lobbyType);

	// Token: 0x060031C4 RID: 12740
	void SetStage(StageID stageID);

	// Token: 0x060031C5 RID: 12741
	void SetMode(LobbyGameMode modeID);

	// Token: 0x060031C6 RID: 12742
	void ChangePlayer(ulong userID, bool isSpectating, int team);

	// Token: 0x060031C7 RID: 12743
	void HostChangePlayer(ulong userID, bool isSpectating, int team);

	// Token: 0x060031C8 RID: 12744
	void HostReceivedScreenChanged(ulong userID, int screenID);

	// Token: 0x060031C9 RID: 12745
	void ReceivedPlayerChange(ulong userID, bool isSpectating, int team);

	// Token: 0x060031CA RID: 12746
	void ReceivedScreenChanged(ulong userID, int screenID);

	// Token: 0x060031CB RID: 12747
	void ReceivedIsLobbyInMatch(bool isInMatch);

	// Token: 0x17000C29 RID: 3113
	// (get) Token: 0x060031CC RID: 12748
	bool IsLobbyInMatch { get; }

	// Token: 0x060031CD RID: 12749
	void Leave();

	// Token: 0x060031CE RID: 12750
	void StartMatch();

	// Token: 0x17000C2A RID: 3114
	// (get) Token: 0x060031CF RID: 12751
	bool IsSpectator { get; }
}
