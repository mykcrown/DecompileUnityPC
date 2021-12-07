using System;

// Token: 0x02000A41 RID: 2625
public class UIConnectionStatusHandler : IConnectionStatusHandler
{
	// Token: 0x17001233 RID: 4659
	// (get) Token: 0x06004CD5 RID: 19669 RVA: 0x001454C0 File Offset: 0x001438C0
	// (set) Token: 0x06004CD6 RID: 19670 RVA: 0x001454C8 File Offset: 0x001438C8
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17001234 RID: 4660
	// (get) Token: 0x06004CD7 RID: 19671 RVA: 0x001454D1 File Offset: 0x001438D1
	// (set) Token: 0x06004CD8 RID: 19672 RVA: 0x001454D9 File Offset: 0x001438D9
	[Inject]
	public IServerConnectionManager connectionManager { get; set; }

	// Token: 0x17001235 RID: 4661
	// (get) Token: 0x06004CD9 RID: 19673 RVA: 0x001454E2 File Offset: 0x001438E2
	// (set) Token: 0x06004CDA RID: 19674 RVA: 0x001454EA File Offset: 0x001438EA
	[Inject]
	public IUIAdapter uiAdapter { get; set; }

	// Token: 0x17001236 RID: 4662
	// (get) Token: 0x06004CDB RID: 19675 RVA: 0x001454F3 File Offset: 0x001438F3
	// (set) Token: 0x06004CDC RID: 19676 RVA: 0x001454FB File Offset: 0x001438FB
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17001237 RID: 4663
	// (get) Token: 0x06004CDD RID: 19677 RVA: 0x00145504 File Offset: 0x00143904
	// (set) Token: 0x06004CDE RID: 19678 RVA: 0x0014550C File Offset: 0x0014390C
	[Inject]
	public IOfflineModeDetector offlineMode { get; set; }

	// Token: 0x17001238 RID: 4664
	// (get) Token: 0x06004CDF RID: 19679 RVA: 0x00145515 File Offset: 0x00143915
	// (set) Token: 0x06004CE0 RID: 19680 RVA: 0x0014551D File Offset: 0x0014391D
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x17001239 RID: 4665
	// (get) Token: 0x06004CE1 RID: 19681 RVA: 0x00145526 File Offset: 0x00143926
	// (set) Token: 0x06004CE2 RID: 19682 RVA: 0x0014552E File Offset: 0x0014392E
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x1700123A RID: 4666
	// (get) Token: 0x06004CE3 RID: 19683 RVA: 0x00145537 File Offset: 0x00143937
	// (set) Token: 0x06004CE4 RID: 19684 RVA: 0x0014553F File Offset: 0x0014393F
	[Inject]
	public ICustomLobbyController lobbyController { get; set; }

	// Token: 0x1700123B RID: 4667
	// (get) Token: 0x06004CE5 RID: 19685 RVA: 0x00145548 File Offset: 0x00143948
	// (set) Token: 0x06004CE6 RID: 19686 RVA: 0x00145550 File Offset: 0x00143950
	[Inject]
	public IExitGame terminateGame { get; set; }

	// Token: 0x1700123C RID: 4668
	// (get) Token: 0x06004CE7 RID: 19687 RVA: 0x00145559 File Offset: 0x00143959
	// (set) Token: 0x06004CE8 RID: 19688 RVA: 0x00145561 File Offset: 0x00143961
	[Inject]
	public IRichPresence richPresence { get; set; }

	// Token: 0x06004CE9 RID: 19689 RVA: 0x0014556C File Offset: 0x0014396C
	public void Startup()
	{
		this.signalBus.AddListener(ServerConnectionManager.UPDATED, new Action(this.onServerStatusUpdate));
		IBattleServerAPI battleServerAPI = this.battleServerAPI;
		battleServerAPI.OnLeftRoom = (Action<bool>)Delegate.Combine(battleServerAPI.OnLeftRoom, new Action<bool>(this.onLeftRoom));
	}

	// Token: 0x06004CEA RID: 19690 RVA: 0x001455BC File Offset: 0x001439BC
	private void onLeftRoom(bool setIncomplete)
	{
		this.userInputManager.ResetPlayerMapping();
		if (setIncomplete)
		{
			this.exitBattle();
			if (this.lobbyController.IsInLobby)
			{
				this.richPresence.SetPresence("InCustomLobby", null, null, null);
				this.events.Broadcast(new LoadScreenCommand(ScreenType.CustomLobbyScreen, null, ScreenUpdateType.Next));
			}
			else if (!this.uiAdapter.AtOrGoingTo(ScreenType.MainMenu) && !this.uiAdapter.AtOrGoingTo(ScreenType.LoginScreen))
			{
				this.richPresence.SetPresence(null, null, null, null);
				this.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, null, ScreenUpdateType.Previous));
			}
		}
		this.onServerStatusUpdate();
	}

	// Token: 0x06004CEB RID: 19691 RVA: 0x00145668 File Offset: 0x00143A68
	private void onServerStatusUpdate()
	{
		if (this.offlineMode.IsOfflineMode())
		{
			return;
		}
		if ((!this.connectionManager.IsConnectedToAuth || !this.connectionManager.IsCoreConnectionReady) && this.uiAdapter.CurrentScreen != ScreenType.None && this.uiAdapter.CurrentScreen != ScreenType.LoginScreen)
		{
			this.exitBattle();
			this.richPresence.SetPresence(null, null, null, null);
			this.events.Broadcast(new LoadScreenCommand(ScreenType.LoginScreen, null, ScreenUpdateType.Next));
		}
	}

	// Token: 0x06004CEC RID: 19692 RVA: 0x001456F3 File Offset: 0x00143AF3
	private void exitBattle()
	{
		if (this.isInBattle())
		{
			this.terminateGame.InstantTerminate();
		}
	}

	// Token: 0x06004CED RID: 19693 RVA: 0x0014570B File Offset: 0x00143B0B
	private bool isInBattle()
	{
		return this.uiAdapter.CurrentScreen == ScreenType.BattleGUI || this.uiAdapter.CurrentScreen == ScreenType.LoadingBattle;
	}
}
