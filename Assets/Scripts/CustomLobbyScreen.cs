// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomLobbyScreen : GameScreen
{
	private sealed class _addPlayer_c__AnonStorey0
	{
		internal int i;

		internal CustomLobbyScreen _this;

		internal void __m__0()
		{
			this._this.arrowLeft(this._this.playerBuffer[this.i]);
		}

		internal void __m__1()
		{
			this._this.arrowRight(this._this.playerBuffer[this.i]);
		}
	}

	public GameObject TweenInLeft;

	public GameObject TweenInRight;

	public GameObject BackButtonPrefab;

	public Transform BackButtonAnchor;

	public GameObject InputInstructionsPrefab;

	public Transform InputInstructionsAnchor;

	private MenuItemList dropdownList;

	private MenuItemList buttonList;

	private MenuItemList playerMenuItemList;

	public TextMeshProUGUI LobbyName;

	public TextMeshProUGUI LobbyPlayerCounts;

	public TextMeshProUGUI Title;

	public DropdownElement StageDropdown;

	public DropdownElement ModeDropdown;

	public Transform List1Anchor;

	public Transform List2Anchor;

	public Transform List3Anchor;

	public LobbyMemberDisplay LobbyMemberPrefab;

	public TextMeshProUGUI List1Title;

	public TextMeshProUGUI List2Title;

	public TextMeshProUGUI List3Title;

	public MenuItemButton EnterMatchButton;

	public MenuItemButton InviteButton;

	public Transform DropdownContainer;

	private List<LobbyMemberDisplay> lobbyList1Displays = new List<LobbyMemberDisplay>();

	private List<LobbyMemberDisplay> lobbyList2Displays = new List<LobbyMemberDisplay>();

	private List<LobbyMemberDisplay> lobbyList3Displays = new List<LobbyMemberDisplay>();

	private List<LobbyPlayerData> playerBuffer = new List<LobbyPlayerData>();

	private double animateFightTextTime;

	private int animateFightTextCycle;

	[Inject]
	public ICustomLobbyScreenAPI api
	{
		get;
		set;
	}

	[Inject]
	public IAccountAPI accountAPI
	{
		get;
		set;
	}

	[Inject]
	public LobbyNameValidator lobbyNameValidator
	{
		get;
		set;
	}

	[Inject]
	public LobbyPasswordValidator lobbyPasswordValidator
	{
		get;
		set;
	}

	[Inject]
	public IStageDataHelper stageDataHelper
	{
		get;
		set;
	}

	[Inject]
	public IEnterNewGame enterNewGame
	{
		get;
		set;
	}

	[Inject]
	public SteamManager steamManager
	{
		get;
		set;
	}

	[Inject]
	public IAutoJoin autoJoin
	{
		get;
		set;
	}

	[Inject]
	public P2PServerMgr p2pServerMgr
	{
		get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		private get;
		set;
	}

	protected override bool SupportsAutoJoin
	{
		get
		{
			return true;
		}
	}

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
		DropdownElement expr_98 = this.StageDropdown;
		expr_98.ValueSelected = (Action<int>)Delegate.Combine(expr_98.ValueSelected, new Action<int>(this.stageSelected));
		this.StageDropdown.Initialize(this.getStageNames(), this.DropdownContainer, null);
		this.StageDropdown.Button.DisableType = ButtonAnimator.VisualDisableType.Grey;
		DropdownElement expr_E8 = this.StageDropdown;
		expr_E8.OnDropdownClosed = (Action)Delegate.Combine(expr_E8.OnDropdownClosed, new Action(this._postStartAndPayload_m__0));
		base.injector.Inject(this.ModeDropdown);
		this.dropdownList.AddButton(this.ModeDropdown.Button, new Action(this.openModeDropdown));
		DropdownElement expr_142 = this.ModeDropdown;
		expr_142.ValueSelected = (Action<int>)Delegate.Combine(expr_142.ValueSelected, new Action<int>(this.modeSelected));
		this.ModeDropdown.Initialize(this.getModeNames(), this.DropdownContainer, null);
		this.ModeDropdown.Button.DisableType = ButtonAnimator.VisualDisableType.Grey;
		DropdownElement expr_192 = this.ModeDropdown;
		expr_192.OnDropdownClosed = (Action)Delegate.Combine(expr_192.OnDropdownClosed, new Action(this._postStartAndPayload_m__1));
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
		this.buttonList.SetButtonEnabled(this.InviteButton, this.steamManager.CurrentSteamLobbyId != 0uL);
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
		base.tweenInItem(this.TweenInLeft, -1, 0, 0.25f, 0f, new Action(this._postStartAndPayload_m__2));
		this.p2pServerMgr.OnUpdateCustomLobby();
		if (base.userAudioSettings.UseAltMenuMusic())
		{
			base.audioManager.StopMusic(null, 0.5f);
		}
	}

	public void onSteamLobbyIdUpdated()
	{
		this.buttonList.SetButtonEnabled(this.InviteButton, this.steamManager.CurrentSteamLobbyId != 0uL);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		this.api.OnDestroy();
	}

	protected override void onAutoJoinRequest()
	{
		base.dialogController.ShowOneButtonDialog(base.localization.GetText("dialog.AutoJoinFail.title"), base.localization.GetText("dialog.AutoJoinFail.InLobby.body"), base.localization.GetText("dialog.continue"), WindowTransition.STANDARD_FADE, false, default(AudioData));
		this.autoJoin.Clear();
	}

	private void returnToPreviousScreen()
	{
		this.api.Leave();
		base.events.Broadcast(new PreviousScreenRequest());
	}

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
		foreach (KeyValuePair<ulong, LobbyPlayerData> current in this.api.Players)
		{
			LobbyPlayerData value2 = current.Value;
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

	private void addPlayer(ref int list1DisplayCount, ref int list2DisplayCount, ref int list3DisplayCount, int i)
	{
		CustomLobbyScreen._addPlayer_c__AnonStorey0 _addPlayer_c__AnonStorey = new CustomLobbyScreen._addPlayer_c__AnonStorey0();
		_addPlayer_c__AnonStorey.i = i;
		_addPlayer_c__AnonStorey._this = this;
		bool flag = this.steamManager.MySteamID().m_SteamID == this.playerBuffer[_addPlayer_c__AnonStorey.i].userID || this.api.IsLobbyLeader;
		bool flag2 = this.steamManager.MySteamID().m_SteamID == this.playerBuffer[_addPlayer_c__AnonStorey.i].userID || this.api.IsLobbyLeader;
		bool flag3 = this.playerBuffer[_addPlayer_c__AnonStorey.i].currentScreen == ScreenType.CustomLobbyScreen;
		LobbyMemberDisplay lobbyMemberDisplay;
		if (!this.api.IsTeams)
		{
			if (!this.playerBuffer[_addPlayer_c__AnonStorey.i].isSpectator)
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
		else if (this.playerBuffer[_addPlayer_c__AnonStorey.i].isSpectator)
		{
			lobbyMemberDisplay = this.lobbyList3Displays[list3DisplayCount];
			list3DisplayCount++;
			flag = false;
		}
		else if (this.playerBuffer[_addPlayer_c__AnonStorey.i].team == 0)
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
		bool isLeader = this.api.HostUserId == this.playerBuffer[_addPlayer_c__AnonStorey.i].userID;
		lobbyMemberDisplay.Setup(this.playerBuffer[_addPlayer_c__AnonStorey.i].name, isLeader);
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
			this.playerMenuItemList.AddButton(lobbyMemberDisplay.arrowLeft, new Action(_addPlayer_c__AnonStorey.__m__0));
		}
		if (flag)
		{
			this.playerMenuItemList.AddButton(lobbyMemberDisplay.arrowRight, new Action(_addPlayer_c__AnonStorey.__m__1));
		}
	}

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

	private void onEvent()
	{
		if (this.api.LastEvent == LobbyEvent.StartSystemError)
		{
			base.unlockInput();
		}
	}

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

	private void openStageDropDown()
	{
		this.dropdownList.Lock();
		this.buttonList.Lock();
		this.playerMenuItemList.Lock();
		this.StageDropdown.Open();
		this.StageDropdown.AutoSelectElement();
	}

	private void openModeDropdown()
	{
		this.dropdownList.Lock();
		this.buttonList.Lock();
		this.playerMenuItemList.Lock();
		this.ModeDropdown.Open();
		this.ModeDropdown.AutoSelectElement();
	}

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

	private void modeSelected(int value)
	{
		this.ModeDropdown.Close();
		this.api.SetMode((LobbyGameMode)value);
	}

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

	private void inviteClicked()
	{
		this.steamManager.InviteToLobby(this.api.LobbyName, this.api.LobbyPassword);
	}

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

	public override void GoToPreviousScreen()
	{
		this.buttonList.RemoveSelection();
		GenericDialog genericDialog = base.dialogController.ShowTwoButtonDialog(base.localization.GetText("ui.customLobby.leaveDialog.title"), base.localization.GetText("ui.customLobby.leaveDialog.body"), base.localization.GetText("dialog.leave"), base.localization.GetText("dialog.cancel"));
		GenericDialog expr_58 = genericDialog;
		expr_58.ConfirmCallback = (Action)Delegate.Combine(expr_58.ConfirmCallback, new Action(this._GoToPreviousScreen_m__3));
	}

	public override void OnStartPressed(IPlayerCursor cursor)
	{
		if (this.api.IsValidPlayerConfiguration())
		{
			this.startMatch();
		}
	}

	private void startMatch()
	{
		base.lockInput();
		this.api.StartMatch();
	}

	private void onDropdownClosed(DropdownElement dropdown)
	{
		this.dropdownList.Unlock();
		this.buttonList.Unlock();
		this.dropdownList.AutoSelect(dropdown.Button);
	}

	private string[] getModeNames()
	{
		List<string> list = new List<string>();
		IEnumerator enumerator = Enum.GetValues(typeof(LobbyGameMode)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				list.Add(base.localization.GetText("lobbyGameMode." + current));
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

	private void _postStartAndPayload_m__0()
	{
		this.onDropdownClosed(this.StageDropdown);
	}

	private void _postStartAndPayload_m__1()
	{
		this.onDropdownClosed(this.ModeDropdown);
	}

	private void _postStartAndPayload_m__2()
	{
		base.unlockInput();
	}

	private void _GoToPreviousScreen_m__3()
	{
		this.returnToPreviousScreen();
	}
}
