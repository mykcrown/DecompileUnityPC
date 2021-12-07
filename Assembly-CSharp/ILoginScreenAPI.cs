using System;

// Token: 0x02000996 RID: 2454
public interface ILoginScreenAPI
{
	// Token: 0x060042EA RID: 17130
	void OnScreenShown();

	// Token: 0x060042EB RID: 17131
	void OnEnterGame();

	// Token: 0x060042EC RID: 17132
	void RetryConnection();

	// Token: 0x17000FCF RID: 4047
	// (get) Token: 0x060042ED RID: 17133
	// (set) Token: 0x060042EE RID: 17134
	bool RememberMe { get; set; }

	// Token: 0x17000FD0 RID: 4048
	// (get) Token: 0x060042EF RID: 17135
	// (set) Token: 0x060042F0 RID: 17136
	bool TermsAccepted { get; set; }

	// Token: 0x17000FD1 RID: 4049
	// (get) Token: 0x060042F1 RID: 17137
	bool HasAccount { get; }

	// Token: 0x17000FD2 RID: 4050
	// (get) Token: 0x060042F2 RID: 17138
	bool IsConnected { get; }

	// Token: 0x17000FD3 RID: 4051
	// (get) Token: 0x060042F3 RID: 17139
	bool IsDataSyncError { get; }

	// Token: 0x17000FD4 RID: 4052
	// (get) Token: 0x060042F4 RID: 17140
	bool IsConnectingInProgress { get; }

	// Token: 0x17000FD5 RID: 4053
	// (get) Token: 0x060042F5 RID: 17141
	bool IsAccountCreateInProgress { get; }

	// Token: 0x17000FD6 RID: 4054
	// (get) Token: 0x060042F6 RID: 17142
	bool CanEnterGame { get; }

	// Token: 0x17000FD7 RID: 4055
	// (get) Token: 0x060042F7 RID: 17143
	// (set) Token: 0x060042F8 RID: 17144
	bool ShowingAccountCreateComplete { get; set; }

	// Token: 0x17000FD8 RID: 4056
	// (get) Token: 0x060042F9 RID: 17145
	// (set) Token: 0x060042FA RID: 17146
	bool ShowingAccountCreateError { get; set; }

	// Token: 0x17000FD9 RID: 4057
	// (get) Token: 0x060042FB RID: 17147
	bool IsShowingAccountCreateResult { get; }
}
