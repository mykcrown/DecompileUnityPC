// Decompile from assembly: Assembly-CSharp.dll

using MatchMaking;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuFoldout : BaseWindow
{
	private sealed class _resetOptions_c__AnonStorey0
	{
		internal Action callback;

		internal MainMenuFoldout _this;

		internal void __m__0(SaveOptionsProfileResult result)
		{
			if (result != SaveOptionsProfileResult.SUCCESS)
			{
				this._this.dialogController.ShowOneButtonDialog("Placeholder error", "There was an error", "Continue", WindowTransition.STANDARD_FADE, false, default(AudioData));
			}
			else
			{
				this.callback();
			}
		}
	}

	public GameObject SubMenuButtonPrefab;

	public VerticalLayoutGroup LocalPlayList;

	public VerticalLayoutGroup CasualOnlineList;

	public VerticalLayoutGroup CustomLobbiesList;

	public TextMeshProUGUI PlayLocal;

	public TextMeshProUGUI PlayOnline;

	public TextMeshProUGUI CustomLobbies;

	private MenuItemList subMenuController;

	private MenuItemButton button_playLocal;

	private MenuItemButton button_trainingMode;

	private MenuItemButton button_onlineCasual1v1;

	private MenuItemButton button_onlineCasualFFA;

	private MenuItemButton button_createCustomLobby;

	private MenuItemButton button_joinCustomLobby;

	private List<MenuItemButton> willDisableButton = new List<MenuItemButton>();

	public Action RequestCloseSubmenu;

	private CreateLobbyDialog createLobbyDialog;

	private JoinLobbyDialog joinLobbyDialog;

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public OptionsProfileAPI optionsProfileAPI
	{
		get;
		set;
	}

	[Inject]
	public DeveloperConfig devConfig
	{
		get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServerAPI
	{
		get;
		set;
	}

	[Inject]
	public IServerConnectionManager serverManager
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
	public IOfflineModeDetector offlineMode
	{
		get;
		set;
	}

	[Inject]
	public IRichPresence richPresence
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
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

	[PostConstruct]
	public void Init()
	{
		base.UseOverrideOpenSound = true;
		base.OverrideOpenSound = AudioData.Empty;
		base.UseOverrideCloseSound = true;
		base.OverrideCloseSound = AudioData.Empty;
	}

	private void Start()
	{
		this.subMenuController = base.injector.GetInstance<MenuItemList>();
		this.button_playLocal = UnityEngine.Object.Instantiate<GameObject>(this.SubMenuButtonPrefab).GetComponent<MenuItemButton>();
		this.button_createCustomLobby = UnityEngine.Object.Instantiate<GameObject>(this.SubMenuButtonPrefab).GetComponent<MenuItemButton>();
		this.button_joinCustomLobby = UnityEngine.Object.Instantiate<GameObject>(this.SubMenuButtonPrefab).GetComponent<MenuItemButton>();
		this.willDisableButton.Add(this.button_createCustomLobby);
		this.willDisableButton.Add(this.button_joinCustomLobby);
		this.willDisableButton.Add(this.button_playLocal);
		foreach (MenuItemButton current in this.willDisableButton)
		{
			current.DisableType = ButtonAnimator.VisualDisableType.Grey;
		}
		this.button_playLocal.SetText(base.localization.GetText("ui.main.submenu.play_local"));
		this.updateQueueText();
		this.button_createCustomLobby.SetText(base.localization.GetText("ui.main.submenu.custom_lobby_create"));
		this.button_joinCustomLobby.SetText(base.localization.GetText("ui.main.submenu.custom_lobby_join"));
		this.CustomLobbies.gameObject.SetActive(true);
		this.addToSubList(this.button_createCustomLobby, this.CustomLobbiesList, new Action(this.createCustomLobby));
		this.addToSubList(this.button_joinCustomLobby, this.CustomLobbiesList, new Action(this.joinCustomLobby));
		this.addToSubList(this.button_playLocal, this.LocalPlayList, new Action(this.playLocal));
		this.subMenuController.Initialize();
		base.signalBus.AddListener(BattleServerController.UPDATE_STATE, new Action(this.onUpdate));
		this.events.Subscribe(typeof(UpdateRequestMatchStatus), new Events.EventHandler(this.onUpdateRequestMatchStatus));
		this.onUpdate();
	}

	private void playLocal()
	{
		this.goToBattle(GameMode.FreeForAll);
	}

	private void playTrainingMode()
	{
		this.optionsProfileAPI.SetDefaultGameMode(GameMode.Training);
		this.goToBattle(GameMode.Training);
	}

	private void playOnlineCasual1v1()
	{
		this.QueueForMatch(EQueueTypes.Quick);
	}

	private void playOnlineCasualFFA()
	{
		this.QueueForMatch(EQueueTypes.FFA);
	}

	private void QueueForMatch(EQueueTypes matchType)
	{
		if (!this.canPlay(matchType))
		{
			return;
		}
	}

	private bool canPlay(EQueueTypes matchType)
	{
		if (matchType == EQueueTypes.FFA || matchType == EQueueTypes.Quick)
		{
			if (!this.serverManager.IsConnectedToNexus)
			{
				base.dialogController.ShowOneButtonDialog(base.localization.GetText("ui.main.submenu.quickmatch.error.NotConnected.title"), base.localization.GetText("ui.main.submenu.quickmatch.error.NotConnected.body"), base.localization.GetText("ui.main.submenu.quickmatch.error.NotConnected.button"), WindowTransition.STANDARD_FADE, false, default(AudioData));
				return false;
			}
			if (Debug.isDebugBuild && !this.devConfig.useLocalBakedAnimations)
			{
				base.dialogController.ShowOneButtonDialog(base.localization.GetText("ui.main.submenu.quickmatch.error.RawAnimations.title"), base.localization.GetText("ui.main.submenu.quickmatch.error.RawAnimations.body"), base.localization.GetText("ui.main.submenu.quickmatch.error.RawAnimations.button"), WindowTransition.STANDARD_FADE, false, default(AudioData));
				return false;
			}
		}
		return true;
	}

	private bool canStartCustomLobby(bool isCreate)
	{
		return true;
	}

	private void createCustomLobby()
	{
		if (this.canStartCustomLobby(true))
		{
			this.createLobbyDialog = base.dialogController.ShowCreateLobbyDialog(true);
			this.createLobbyDialog.CloseCallback = new Action(this._createCustomLobby_m__0);
		}
	}

	public bool TryJoinSteamCustomLobby(ulong steamLobbyID)
	{
		if (this.canStartCustomLobby(true))
		{
			this.joinLobbyDialog = base.dialogController.ShowJoinLobbyDialog();
			this.joinLobbyDialog.JoinCustomLobby(steamLobbyID);
			this.joinLobbyDialog.CloseCallback = new Action(this._TryJoinSteamCustomLobby_m__1);
			return true;
		}
		return false;
	}

	public bool TryJoinCustomLobby(string lobbyName, string lobbyPassword)
	{
		if (this.canStartCustomLobby(true))
		{
			this.p2pServerMgr.UpdateLobbyists(false);
			return true;
		}
		return false;
	}

	private void joinCustomLobby()
	{
		if (this.canStartCustomLobby(true))
		{
			this.joinLobbyDialog = base.dialogController.ShowJoinLobbyDialog();
			this.joinLobbyDialog.CloseCallback = new Action(this._joinCustomLobby_m__2);
		}
	}

	public void OnButtonMode()
	{
		if (this.hasOtherWindow())
		{
			return;
		}
		if (this.subMenuController.CurrentSelection == null)
		{
			this.subMenuController.AutoSelect(this.button_playLocal);
		}
	}

	private bool hasOtherWindow()
	{
		BaseWindow[] activeWindows = base.uiManager.GetActiveWindows();
		for (int i = 0; i < activeWindows.Length; i++)
		{
			BaseWindow baseWindow = activeWindows[i];
			if (!(baseWindow is MainMenuFoldout))
			{
				return true;
			}
		}
		return false;
	}

	private void goToBattle(GameMode mode)
	{
		this.optionsProfileAPI.SetDefaultGameMode(mode);
		this.resetOptions(new Action(this._goToBattle_m__3));
	}

	private void resetOptions(Action callback)
	{
		MainMenuFoldout._resetOptions_c__AnonStorey0 _resetOptions_c__AnonStorey = new MainMenuFoldout._resetOptions_c__AnonStorey0();
		_resetOptions_c__AnonStorey.callback = callback;
		_resetOptions_c__AnonStorey._this = this;
		this.optionsProfileAPI.DeleteDefaultSettings(new Action<SaveOptionsProfileResult>(_resetOptions_c__AnonStorey.__m__0));
	}

	private void addToSubList(MenuItemButton button, LayoutGroup layoutGroup, Action callback)
	{
		button.transform.SetParent(layoutGroup.transform, false);
		button.DisableDuration = 0.075f;
		button.DisableType = ButtonAnimator.VisualDisableType.Grey;
		this.subMenuController.AddButton(button, callback);
	}

	private void onUpdateRequestMatchStatus(GameEvent message)
	{
		this.onUpdate();
	}

	private void onUpdate()
	{
		this.updateQueueText();
		this.updateDisabledButtons();
	}

	private void updateQueueText()
	{
	}

	public void InternalCloseSubmenu()
	{
		this.RequestCloseSubmenu();
	}

	public void Activate()
	{
		this.subMenuController.AutoSelect(this.button_playLocal);
	}

	public void Deactivate()
	{
		this.subMenuController.Disable();
	}

	public override void OnCancelPressed()
	{
		this.InternalCloseSubmenu();
	}

	private void updateQueueButton(EQueueTypes queueType, MenuItemButton button)
	{
	}

	private void updateDisabledButtons()
	{
	}

	protected override void OnDestroy()
	{
		if (this.events != null)
		{
			this.events.Unsubscribe(typeof(UpdateRequestMatchStatus), new Events.EventHandler(this.onUpdateRequestMatchStatus));
		}
		base.signalBus.RemoveListener(BattleServerController.UPDATE_STATE, new Action(this.onUpdate));
		if (this.subMenuController != null)
		{
			this.subMenuController.OnDestroy();
		}
		base.OnDestroy();
	}

	private void _createCustomLobby_m__0()
	{
		this.createLobbyDialog = null;
	}

	private void _TryJoinSteamCustomLobby_m__1()
	{
		this.joinLobbyDialog = null;
	}

	private void _joinCustomLobby_m__2()
	{
		this.joinLobbyDialog = null;
	}

	private void _goToBattle_m__3()
	{
		this.enterNewGame.InitPayload(GameStartType.FreePlay, null);
		this.richPresence.SetPresence("InCharacterSelect", null, null, null);
		this.events.Broadcast(new LoadScreenCommand(ScreenType.CharacterSelect, null, ScreenUpdateType.Next));
	}
}
