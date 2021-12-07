using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200091D RID: 2333
public class CustomLobbyScreen : GameScreen
{
	// Token: 0x17000E73 RID: 3699
	// (get) Token: 0x06003C93 RID: 15507 RVA: 0x00118F62 File Offset: 0x00117362
	// (set) Token: 0x06003C94 RID: 15508 RVA: 0x00118F6A File Offset: 0x0011736A
	[Inject]
	public ICustomLobbyScreenAPI api { get; set; }

	// Token: 0x17000E74 RID: 3700
	// (get) Token: 0x06003C95 RID: 15509 RVA: 0x00118F73 File Offset: 0x00117373
	// (set) Token: 0x06003C96 RID: 15510 RVA: 0x00118F7B File Offset: 0x0011737B
	[Inject]
	public IAccountAPI accountAPI { get; set; }

	// Token: 0x17000E75 RID: 3701
	// (get) Token: 0x06003C97 RID: 15511 RVA: 0x00118F84 File Offset: 0x00117384
	// (set) Token: 0x06003C98 RID: 15512 RVA: 0x00118F8C File Offset: 0x0011738C
	[Inject]
	public LobbyNameValidator lobbyNameValidator { get; set; }

	// Token: 0x17000E76 RID: 3702
	// (get) Token: 0x06003C99 RID: 15513 RVA: 0x00118F95 File Offset: 0x00117395
	// (set) Token: 0x06003C9A RID: 15514 RVA: 0x00118F9D File Offset: 0x0011739D
	[Inject]
	public LobbyPasswordValidator lobbyPasswordValidator { get; set; }

	// Token: 0x17000E77 RID: 3703
	// (get) Token: 0x06003C9B RID: 15515 RVA: 0x00118FA6 File Offset: 0x001173A6
	// (set) Token: 0x06003C9C RID: 15516 RVA: 0x00118FAE File Offset: 0x001173AE
	[Inject]
	public IStageDataHelper stageDataHelper { get; set; }

	// Token: 0x17000E78 RID: 3704
	// (get) Token: 0x06003C9D RID: 15517 RVA: 0x00118FB7 File Offset: 0x001173B7
	// (set) Token: 0x06003C9E RID: 15518 RVA: 0x00118FBF File Offset: 0x001173BF
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x17000E79 RID: 3705
	// (get) Token: 0x06003C9F RID: 15519 RVA: 0x00118FC8 File Offset: 0x001173C8
	// (set) Token: 0x06003CA0 RID: 15520 RVA: 0x00118FD0 File Offset: 0x001173D0
	[Inject]
	public SteamManager steamManager { get; set; }

	// Token: 0x17000E7A RID: 3706
	// (get) Token: 0x06003CA1 RID: 15521 RVA: 0x00118FD9 File Offset: 0x001173D9
	// (set) Token: 0x06003CA2 RID: 15522 RVA: 0x00118FE1 File Offset: 0x001173E1
	[Inject]
	public IAutoJoin autoJoin { get; set; }

	// Token: 0x17000E7B RID: 3707
	// (get) Token: 0x06003CA3 RID: 15523 RVA: 0x00118FEA File Offset: 0x001173EA
	// (set) Token: 0x06003CA4 RID: 15524 RVA: 0x00118FF2 File Offset: 0x001173F2
	[Inject]
	public P2PServerMgr p2pServerMgr { get; set; }

	// Token: 0x17000E7C RID: 3708
	// (get) Token: 0x06003CA5 RID: 15525 RVA: 0x00118FFB File Offset: 0x001173FB
	// (set) Token: 0x06003CA6 RID: 15526 RVA: 0x00119003 File Offset: 0x00117403
	[Inject]
	public IMainThreadTimer timer { private get; set; }

	// Token: 0x17000E7D RID: 3709
	// (get) Token: 0x06003CA7 RID: 15527 RVA: 0x0011900C File Offset: 0x0011740C
	protected override bool SupportsAutoJoin
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003CA8 RID: 15528 RVA: 0x00119010 File Offset: 0x00117410
	protected override void postStartAndPayload()
	{
		base.postStartAndPayload();
		base.addInputInstuctionsForMenuScreen(this.InputInstructionsAnchor, this.InputInstructionsPrefab);
		base.addBackButtonForMenuScreen(this.BackButtonAnchor, this.BackButtonPrefab);
		this.dropdownList = base.injector.GetInstance<MenuItemList>();
		this.buttonList = base.injector.GetInstance<MenuItemList>();
		this.playerMenuItemList = base.injector.GetInstance<MenuItemList>();
		base.injector.Inject(this.StageDropdown);
		this.dropdownList.AddButton(this.StageDropdown.Button, new Action(this.openStageDropDown));
		DropdownElement stageDropdown = this.StageDropdown;
		stageDropdown.ValueSelected = (Action<int>)Delegate.Combine(stageDropdown.ValueSelected, new Action<int>(this.stageSelected));
		this.StageDropdown.Initialize(this.getStageNames(), this.DropdownContainer, null);
		this.StageDropdown.Button.DisableType = ButtonAnimator.VisualDisableType.Grey;
		DropdownElement stageDropdown2 = this.StageDropdown;
		stageDropdown2.OnDropdownClosed = (Action)Delegate.Combine(stageDropdown2.OnDropdownClosed, new Action(delegate()
		{
			this.onDropdownClosed(this.StageDropdown);
		}));
		base.injector.Inject(this.ModeDropdown);
		this.dropdownList.AddButton(this.ModeDropdown.Button, new Action(this.openModeDropdown));
		DropdownElement modeDropdown = this.ModeDropdown;
		modeDropdown.ValueSelected = (Action<int>)Delegate.Combine(modeDropdown.ValueSelected, new Action<int>(this.modeSelected));
		this.ModeDropdown.Initialize(this.getModeNames(), this.DropdownContainer, null);
		this.ModeDropdown.Button.DisableType = ButtonAnimator.VisualDisableType.Grey;
		DropdownElement modeDropdown2 = this.ModeDropdown;
		modeDropdown2.OnDropdownClosed = (Action)Delegate.Combine(modeDropdown2.OnDropdownClosed, new Action(delegate()
		{
			this.onDropdownClosed(this.ModeDropdown);
		}));
		this.buttonList.AddButton(this.InviteButton, new Action(this.inviteClicked));
		this.buttonList.AddButton(this.EnterMatchButton, new Action(this.enterClicked));
		this.EnterMatchButton.DisableType = ButtonAnimator.VisualDisableType.Grey;
		this.EnterMatchButton.DisableDuration = 0.2f;
		this.dropdownList.LandingPoint = this.StageDropdown.Button;
		this.buttonList.LandingPoint = this.buttonList.GetButtons()[1];
		this.dropdownList.AddEdgeNavigation(MoveDirection.Down, this.buttonList);
		this.buttonList.AddEdgeNavigation(MoveDirection.Up, this.dropdownList);
		this.dropdownList.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.dropdownList.Initialize();
		this.buttonList.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.buttonList.Initialize();
		this.buttonList.AddEdgeNavigation(MoveDirection.Left, this.playerMenuItemList);
		this.playerMenuItemList.AddEdgeNavigation(MoveDirection.Right, this.buttonList);
		this.playerMenuItemList.SetNavigationType(MenuItemList.NavigationType.GridVerticalFill, 0);
		this.buttonList.SetButtonEnabled(this.InviteButton, this.steamManager.CurrentSteamLobbyId != 0UL);
		for (int i = 0; i < base.gameDataManager.ConfigData.maxPlayers; i++)
		{
			LobbyMemberDisplay item = UnityEngine.Object.Instantiate<LobbyMemberDisplay>(this.LobbyMemberPrefab, this.List1Anchor.transform, false);
			this.lobbyList1Displays.Add(item);
			LobbyMemberDisplay item2 = UnityEngine.Object.Instantiate<LobbyMemberDisplay>(this.LobbyMemberPrefab, this.List2Anchor.transform, false);
			this.lobbyList2Displays.Add(item2);
			LobbyMemberDisplay item3 = UnityEngine.Object.Instantiate<LobbyMemberDisplay>(this.LobbyMemberPrefab, this.List3Anchor.transform, false);
			this.lobbyList3Displays.Add(item3);
		}
		this.api.Initialize();
		base.listen(CustomLobbyController.EVENT, new Action(this.onEvent));
		base.listen(CustomLobbyScreenAPI.UPDATED, new Action(this.onUpdate));
		base.listen("STEAM_LOBBY_ID_UPDATED", new Action(this.onSteamLobbyIdUpdated));
		this.onUpdate();
		base.lockInput();
		base.tweenInItem(this.TweenInRight, 1, 0, 0.25f, 0f, null);
		base.tweenInItem(this.TweenInLeft, -1, 0, 0.25f, 0f, delegate
		{
			base.unlockInput();
		});
		this.p2pServerMgr.OnUpdateCustomLobby();
		if (base.userAudioSettings.UseAltMenuMusic())
		{
			base.audioManager.StopMusic(null, 0.5f);
		}
	}

	// Token: 0x06003CA9 RID: 15529 RVA: 0x0011944E File Offset: 0x0011784E
	public void onSteamLobbyIdUpdated()
	{
		this.buttonList.SetButtonEnabled(this.InviteButton, this.steamManager.CurrentSteamLobbyId != 0UL);
	}

	// Token: 0x06003CAA RID: 15530 RVA: 0x00119473 File Offset: 0x00117873
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.api.OnDestroy();
	}

	// Token: 0x06003CAB RID: 15531 RVA: 0x00119488 File Offset: 0x00117888
	protected override void onAutoJoinRequest()
	{
		base.dialogController.ShowOneButtonDialog(base.localization.GetText("dialog.AutoJoinFail.title"), base.localization.GetText("dialog.AutoJoinFail.InLobby.body"), base.localization.GetText("dialog.continue"), WindowTransition.STANDARD_FADE, false, default(AudioData));
		this.autoJoin.Clear();
	}

	// Token: 0x06003CAC RID: 15532 RVA: 0x001194E7 File Offset: 0x001178E7
	private void returnToPreviousScreen()
	{
		this.api.Leave();
		base.events.Broadcast(new PreviousScreenRequest());
	}

	// Token: 0x06003CAD RID: 15533 RVA: 0x00119504 File Offset: 0x00117904
	private void onUpdate()
	{
		if (!this.api.IsInLobby)
		{
			this.returnToPreviousScreen();
			return;
		}
		this.LobbyName.text = this.api.LobbyName;
		if (this.api.IsLobbyInMatch)
		{
			this.animateFightText();
		}
		else
		{
			this.timer.CancelTimeout(new Action(this.animateFightText));
			this.animateFightTextTime = 0.0;
			this.Title.text = base.localization.GetText("ui.customLobby.title");
		}
		List<StageID> possibleOnlineStageChoices = this.stageDataHelper.GetPossibleOnlineStageChoices();
		if (this.api.StageID == StageID.Random)
		{
			this.StageDropdown.SetValue(0);
		}
		else
		{
			int value = possibleOnlineStageChoices.IndexOf(this.api.StageID) + 1;
			this.StageDropdown.SetValue(value);
		}
		int modeID = (int)this.api.ModeID;
		this.ModeDropdown.SetValue(modeID);
		this.dropdownList.SetButtonEnabled(this.StageDropdown.Button, this.api.IsLobbyLeader);
		this.dropdownList.SetButtonEnabled(this.ModeDropdown.Button, this.api.IsLobbyLeader);
		this.playerBuffer.Clear();
		foreach (KeyValuePair<ulong, LobbyPlayerData> keyValuePair in this.api.Players)
		{
			LobbyPlayerData value2 = keyValuePair.Value;
			if (value2 != null && value2.name != null)
			{
				this.playerBuffer.Add(value2);
			}
		}
		this.LobbyPlayerCounts.text = base.localization.GetText("ui.customLobby.playerCount", this.playerBuffer.Count.ToString(), base.config.lobbySettings.maxPlayerNum.ToString());
		this.playerMenuItemList.ClearButtons();
		if (this.api.IsTeams)
		{
			this.List1Title.text = base.localization.GetText("lobby.team0");
			this.List2Title.text = base.localization.GetText("lobby.team1");
			this.List3Title.text = base.localization.GetText("lobby.spectators");
			this.List1Title.color = WColor.UIRed;
			this.List2Title.color = WColor.UIBlue;
			this.List3Title.color = WColor.UIGrey;
			this.List3Title.gameObject.SetActive(true);
		}
		else
		{
			this.List1Title.text = base.localization.GetText("lobby.players");
			this.List2Title.text = base.localization.GetText("lobby.spectators");
			this.List1Title.color = Color.white;
			this.List2Title.color = WColor.UIGrey;
			this.List3Title.gameObject.SetActive(false);
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < this.playerBuffer.Count; i++)
		{
			this.addPlayer(ref num, ref num2, ref num3, i);
		}
		this.playerMenuItemList.Initialize();
		for (int j = num; j < this.lobbyList1Displays.Count; j++)
		{
			LobbyMemberDisplay lobbyMemberDisplay = this.lobbyList1Displays[j];
			lobbyMemberDisplay.Setup(null, false);
		}
		for (int k = num2; k < this.lobbyList2Displays.Count; k++)
		{
			LobbyMemberDisplay lobbyMemberDisplay2 = this.lobbyList2Displays[k];
			lobbyMemberDisplay2.Setup(null, false);
		}
		for (int l = num3; l < this.lobbyList3Displays.Count; l++)
		{
			LobbyMemberDisplay lobbyMemberDisplay3 = this.lobbyList3Displays[l];
			lobbyMemberDisplay3.Setup(null, false);
		}
		this.EnterMatchButton.gameObject.SetActive(this.api.IsLobbyLeader);
		bool flag = this.api.IsValidPlayerConfiguration();
		this.buttonList.SetButtonEnabled(this.EnterMatchButton, flag);
		if (this.buttonList.CurrentSelection == this.EnterMatchButton && !flag)
		{
			this.buttonList.Disable();
		}
	}

	// Token: 0x06003CAE RID: 15534 RVA: 0x00119984 File Offset: 0x00117D84
	private void animateFightText()
	{
		double num = 800.0;
		double num2 = WTime.precisionTimeSinceStartup - this.animateFightTextTime;
		if (num2 >= num)
		{
			this.animateFightTextCycle++;
			if (this.animateFightTextCycle > 3)
			{
				this.animateFightTextCycle = 0;
			}
		}
		string text = string.Empty;
		for (int i = 0; i < this.animateFightTextCycle; i++)
		{
			text += ".";
		}
		this.Title.text = base.localization.GetText("ui.customLobby.title.inProgress") + text;
		this.timer.CancelTimeout(new Action(this.animateFightText));
		this.timer.SetOrReplaceTimeout((int)(num + 33.0), new Action(this.animateFightText));
	}

	// Token: 0x06003CAF RID: 15535 RVA: 0x00119A54 File Offset: 0x00117E54
	private void addPlayer(ref int list1DisplayCount, ref int list2DisplayCount, ref int list3DisplayCount, int i)
	{
		bool flag = this.steamManager.MySteamID().m_SteamID == this.playerBuffer[i].userID || this.api.IsLobbyLeader;
		bool flag2 = this.steamManager.MySteamID().m_SteamID == this.playerBuffer[i].userID || this.api.IsLobbyLeader;
		bool flag3 = this.playerBuffer[i].currentScreen == ScreenType.CustomLobbyScreen;
		LobbyMemberDisplay lobbyMemberDisplay;
		if (!this.api.IsTeams)
		{
			if (!this.playerBuffer[i].isSpectator)
			{
				lobbyMemberDisplay = this.lobbyList1Displays[list1DisplayCount];
				list1DisplayCount++;
				flag2 = false;
			}
			else
			{
				lobbyMemberDisplay = this.lobbyList2Displays[list2DisplayCount];
				list2DisplayCount++;
				flag = false;
			}
		}
		else if (this.playerBuffer[i].isSpectator)
		{
			lobbyMemberDisplay = this.lobbyList3Displays[list3DisplayCount];
			list3DisplayCount++;
			flag = false;
		}
		else if (this.playerBuffer[i].team == 0)
		{
			lobbyMemberDisplay = this.lobbyList1Displays[list1DisplayCount];
			list1DisplayCount++;
			flag2 = false;
		}
		else
		{
			lobbyMemberDisplay = this.lobbyList2Displays[list2DisplayCount];
			list2DisplayCount++;
		}
		bool isLeader = this.api.HostUserId == this.playerBuffer[i].userID;
		lobbyMemberDisplay.Setup(this.playerBuffer[i].name, isLeader);
		lobbyMemberDisplay.arrowLeft.gameObject.SetActive(flag2);
		lobbyMemberDisplay.arrowRight.gameObject.SetActive(flag);
		lobbyMemberDisplay.SetFade(!flag3);
		if (this.playerMenuItemList.CurrentSelection == lobbyMemberDisplay.arrowLeft && !flag2)
		{
			this.playerMenuItemList.Disable();
		}
		if (this.playerMenuItemList.CurrentSelection == lobbyMemberDisplay.arrowRight && !flag)
		{
			this.playerMenuItemList.Disable();
		}
		if (flag2)
		{
			this.playerMenuItemList.AddButton(lobbyMemberDisplay.arrowLeft, delegate()
			{
				this.arrowLeft(this.playerBuffer[i]);
			});
		}
		if (flag)
		{
			this.playerMenuItemList.AddButton(lobbyMemberDisplay.arrowRight, delegate()
			{
				this.arrowRight(this.playerBuffer[i]);
			});
		}
	}

	// Token: 0x06003CB0 RID: 15536 RVA: 0x00119D04 File Offset: 0x00118104
	private void arrowLeft(LobbyPlayerData playerData)
	{
		if (this.api.IsTeams)
		{
			if (playerData.isSpectator)
			{
				this.api.ChangePlayer(playerData.userID, false, 1);
			}
			else
			{
				this.api.ChangePlayer(playerData.userID, false, 0);
			}
		}
		else
		{
			this.api.ChangePlayer(playerData.userID, false, 0);
		}
	}

	// Token: 0x06003CB1 RID: 15537 RVA: 0x00119D70 File Offset: 0x00118170
	private void arrowRight(LobbyPlayerData playerData)
	{
		if (this.api.IsTeams)
		{
			if (playerData.team == 0)
			{
				this.api.ChangePlayer(playerData.userID, false, 1);
			}
			else
			{
				this.api.ChangePlayer(playerData.userID, true, 0);
			}
		}
		else
		{
			this.api.ChangePlayer(playerData.userID, true, 0);
		}
	}

	// Token: 0x06003CB2 RID: 15538 RVA: 0x00119DDB File Offset: 0x001181DB
	private void onEvent()
	{
		if (this.api.LastEvent == LobbyEvent.StartSystemError)
		{
			base.unlockInput();
		}
	}

	// Token: 0x06003CB3 RID: 15539 RVA: 0x00119DF8 File Offset: 0x001181F8
	protected override void onMouseModeUpdate()
	{
		if (this.buttonList == null)
		{
			return;
		}
		base.onMouseModeUpdate();
		if (!(base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode && base.windowDisplay.GetWindowCount() == 0)
		{
			if (this.StageDropdown.IsOpen())
			{
				this.StageDropdown.AutoSelectElement();
			}
			else if (this.ModeDropdown.IsOpen())
			{
				this.ModeDropdown.AutoSelectElement();
			}
			else if (this.buttonList.CurrentSelection == null && this.dropdownList.CurrentSelection == null && this.api.IsLobbyLeader)
			{
				if (this.api.IsValidPlayerConfiguration())
				{
					this.buttonList.AutoSelect(this.EnterMatchButton);
				}
				else
				{
					this.buttonList.AutoSelect(this.ModeDropdown.Button);
				}
			}
		}
	}

	// Token: 0x06003CB4 RID: 15540 RVA: 0x00119EF9 File Offset: 0x001182F9
	private void openStageDropDown()
	{
		this.dropdownList.Lock();
		this.buttonList.Lock();
		this.playerMenuItemList.Lock();
		this.StageDropdown.Open();
		this.StageDropdown.AutoSelectElement();
	}

	// Token: 0x06003CB5 RID: 15541 RVA: 0x00119F32 File Offset: 0x00118332
	private void openModeDropdown()
	{
		this.dropdownList.Lock();
		this.buttonList.Lock();
		this.playerMenuItemList.Lock();
		this.ModeDropdown.Open();
		this.ModeDropdown.AutoSelectElement();
	}

	// Token: 0x06003CB6 RID: 15542 RVA: 0x00119F6C File Offset: 0x0011836C
	private void stageSelected(int value)
	{
		this.StageDropdown.Close();
		List<StageID> possibleOnlineStageChoices = this.stageDataHelper.GetPossibleOnlineStageChoices();
		if (value == 0)
		{
			this.api.SetStage(StageID.Random);
		}
		else
		{
			StageID stage = possibleOnlineStageChoices[value - 1];
			this.api.SetStage(stage);
		}
	}

	// Token: 0x06003CB7 RID: 15543 RVA: 0x00119FBD File Offset: 0x001183BD
	private void modeSelected(int value)
	{
		this.ModeDropdown.Close();
		this.api.SetMode((LobbyGameMode)value);
	}

	// Token: 0x06003CB8 RID: 15544 RVA: 0x00119FD8 File Offset: 0x001183D8
	private void enterClicked()
	{
		if (!this.api.IsAllPlayersReadyForCSS())
		{
			base.dialogController.ShowOneButtonDialog(base.localization.GetText("dialog.LobbyWaiting.title"), base.localization.GetText("dialog.LobbyWaiting.body"), base.localization.GetText("dialog.continue"), WindowTransition.STANDARD_FADE, false, default(AudioData));
		}
		else
		{
			this.startMatch();
		}
	}

	// Token: 0x06003CB9 RID: 15545 RVA: 0x0011A047 File Offset: 0x00118447
	private void inviteClicked()
	{
		this.steamManager.InviteToLobby(this.api.LobbyName, this.api.LobbyPassword);
	}

	// Token: 0x06003CBA RID: 15546 RVA: 0x0011A06C File Offset: 0x0011846C
	public override void OnCancelPressed()
	{
		if (this.StageDropdown.IsOpen())
		{
			this.StageDropdown.Close();
		}
		else if (this.ModeDropdown.IsOpen())
		{
			this.ModeDropdown.Close();
		}
		else
		{
			this.GoToPreviousScreen();
		}
	}

	// Token: 0x06003CBB RID: 15547 RVA: 0x0011A0C0 File Offset: 0x001184C0
	public override void GoToPreviousScreen()
	{
		this.buttonList.RemoveSelection();
		GenericDialog genericDialog = base.dialogController.ShowTwoButtonDialog(base.localization.GetText("ui.customLobby.leaveDialog.title"), base.localization.GetText("ui.customLobby.leaveDialog.body"), base.localization.GetText("dialog.leave"), base.localization.GetText("dialog.cancel"));
		GenericDialog genericDialog2 = genericDialog;
		genericDialog2.ConfirmCallback = (Action)Delegate.Combine(genericDialog2.ConfirmCallback, new Action(delegate()
		{
			this.returnToPreviousScreen();
		}));
	}

	// Token: 0x06003CBC RID: 15548 RVA: 0x0011A146 File Offset: 0x00118546
	public override void OnStartPressed(IPlayerCursor cursor)
	{
		if (this.api.IsValidPlayerConfiguration())
		{
			this.startMatch();
		}
	}

	// Token: 0x06003CBD RID: 15549 RVA: 0x0011A15E File Offset: 0x0011855E
	private void startMatch()
	{
		base.lockInput();
		this.api.StartMatch();
	}

	// Token: 0x06003CBE RID: 15550 RVA: 0x0011A171 File Offset: 0x00118571
	private void onDropdownClosed(DropdownElement dropdown)
	{
		this.dropdownList.Unlock();
		this.buttonList.Unlock();
		this.dropdownList.AutoSelect(dropdown.Button);
	}

	// Token: 0x06003CBF RID: 15551 RVA: 0x0011A19C File Offset: 0x0011859C
	private string[] getModeNames()
	{
		List<string> list = new List<string>();
		IEnumerator enumerator = Enum.GetValues(typeof(LobbyGameMode)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object arg = enumerator.Current;
				list.Add(base.localization.GetText("lobbyGameMode." + arg));
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return list.ToArray();
	}

	// Token: 0x06003CC0 RID: 15552 RVA: 0x0011A228 File Offset: 0x00118628
	private string[] getStageNames()
	{
		List<StageID> possibleOnlineStageChoices = this.stageDataHelper.GetPossibleOnlineStageChoices();
		string[] array = new string[possibleOnlineStageChoices.Count + 1];
		array[0] = base.localization.GetText("gameData.stageSelect.random");
		for (int i = 0; i < possibleOnlineStageChoices.Count; i++)
		{
			StageData dataByID = base.gameDataManager.StageData.GetDataByID(possibleOnlineStageChoices[i]);
			string text = base.localization.GetText("gameData.stageSelect." + dataByID.stageName);
			array[i + 1] = text;
		}
		return array;
	}

	// Token: 0x0400297E RID: 10622
	public GameObject TweenInLeft;

	// Token: 0x0400297F RID: 10623
	public GameObject TweenInRight;

	// Token: 0x04002980 RID: 10624
	public GameObject BackButtonPrefab;

	// Token: 0x04002981 RID: 10625
	public Transform BackButtonAnchor;

	// Token: 0x04002982 RID: 10626
	public GameObject InputInstructionsPrefab;

	// Token: 0x04002983 RID: 10627
	public Transform InputInstructionsAnchor;

	// Token: 0x04002984 RID: 10628
	private MenuItemList dropdownList;

	// Token: 0x04002985 RID: 10629
	private MenuItemList buttonList;

	// Token: 0x04002986 RID: 10630
	private MenuItemList playerMenuItemList;

	// Token: 0x04002987 RID: 10631
	public TextMeshProUGUI LobbyName;

	// Token: 0x04002988 RID: 10632
	public TextMeshProUGUI LobbyPlayerCounts;

	// Token: 0x04002989 RID: 10633
	public TextMeshProUGUI Title;

	// Token: 0x0400298A RID: 10634
	public DropdownElement StageDropdown;

	// Token: 0x0400298B RID: 10635
	public DropdownElement ModeDropdown;

	// Token: 0x0400298C RID: 10636
	public Transform List1Anchor;

	// Token: 0x0400298D RID: 10637
	public Transform List2Anchor;

	// Token: 0x0400298E RID: 10638
	public Transform List3Anchor;

	// Token: 0x0400298F RID: 10639
	public LobbyMemberDisplay LobbyMemberPrefab;

	// Token: 0x04002990 RID: 10640
	public TextMeshProUGUI List1Title;

	// Token: 0x04002991 RID: 10641
	public TextMeshProUGUI List2Title;

	// Token: 0x04002992 RID: 10642
	public TextMeshProUGUI List3Title;

	// Token: 0x04002993 RID: 10643
	public MenuItemButton EnterMatchButton;

	// Token: 0x04002994 RID: 10644
	public MenuItemButton InviteButton;

	// Token: 0x04002995 RID: 10645
	public Transform DropdownContainer;

	// Token: 0x04002996 RID: 10646
	private List<LobbyMemberDisplay> lobbyList1Displays = new List<LobbyMemberDisplay>();

	// Token: 0x04002997 RID: 10647
	private List<LobbyMemberDisplay> lobbyList2Displays = new List<LobbyMemberDisplay>();

	// Token: 0x04002998 RID: 10648
	private List<LobbyMemberDisplay> lobbyList3Displays = new List<LobbyMemberDisplay>();

	// Token: 0x04002999 RID: 10649
	private List<LobbyPlayerData> playerBuffer = new List<LobbyPlayerData>();

	// Token: 0x0400299A RID: 10650
	private double animateFightTextTime;

	// Token: 0x0400299B RID: 10651
	private int animateFightTextCycle;
}
