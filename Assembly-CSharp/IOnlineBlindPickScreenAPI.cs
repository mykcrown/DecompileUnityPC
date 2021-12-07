using System;
using System.Collections.Generic;

// Token: 0x020009B7 RID: 2487
public interface IOnlineBlindPickScreenAPI
{
	// Token: 0x06004531 RID: 17713
	void OnScreenShown();

	// Token: 0x06004532 RID: 17714
	void OnScreenDestroyed();

	// Token: 0x06004533 RID: 17715
	void LeaveRoom();

	// Token: 0x17001061 RID: 4193
	// (get) Token: 0x06004534 RID: 17716
	ulong MyUserID { get; }

	// Token: 0x17001062 RID: 4194
	// (get) Token: 0x06004535 RID: 17717
	bool IsTeams { get; }

	// Token: 0x06004536 RID: 17718
	void LockInSelection();

	// Token: 0x17001063 RID: 4195
	// (get) Token: 0x06004537 RID: 17719
	bool CanLockInSelection { get; }

	// Token: 0x17001064 RID: 4196
	// (get) Token: 0x06004538 RID: 17720
	bool CanStartGame { get; }

	// Token: 0x17001065 RID: 4197
	// (get) Token: 0x06004539 RID: 17721
	bool LockedIn { get; }

	// Token: 0x17001066 RID: 4198
	// (get) Token: 0x0600453A RID: 17722
	bool IsSpectator { get; }

	// Token: 0x0600453B RID: 17723
	Dictionary<ulong, LobbyPlayerData> GetLobbyPlayers();
}
