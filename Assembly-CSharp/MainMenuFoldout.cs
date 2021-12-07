using System;
using System.Collections.Generic;
using MatchMaking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009AA RID: 2474
public class MainMenuFoldout : BaseWindow
{
	// Token: 0x1700100D RID: 4109
	// (get) Token: 0x060043E8 RID: 17384 RVA: 0x0012BE75 File Offset: 0x0012A275
	// (set) Token: 0x060043E9 RID: 17385 RVA: 0x0012BE7D File Offset: 0x0012A27D
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x1700100E RID: 4110
	// (get) Token: 0x060043EA RID: 17386 RVA: 0x0012BE86 File Offset: 0x0012A286
	// (set) Token: 0x060043EB RID: 17387 RVA: 0x0012BE8E File Offset: 0x0012A28E
	[Inject]
	public OptionsProfileAPI optionsProfileAPI { get; set; }

	// Token: 0x1700100F RID: 4111
	// (get) Token: 0x060043EC RID: 17388 RVA: 0x0012BE97 File Offset: 0x0012A297
	// (set) Token: 0x060043ED RID: 17389 RVA: 0x0012BE9F File Offset: 0x0012A29F
	[Inject]
	public DeveloperConfig devConfig { get; set; }

	// Token: 0x17001010 RID: 4112
	// (get) Token: 0x060043EE RID: 17390 RVA: 0x0012BEA8 File Offset: 0x0012A2A8
	// (set) Token: 0x060043EF RID: 17391 RVA: 0x0012BEB0 File Offset: 0x0012A2B0
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x17001011 RID: 4113
	// (get) Token: 0x060043F0 RID: 17392 RVA: 0x0012BEB9 File Offset: 0x0012A2B9
	// (set) Token: 0x060043F1 RID: 17393 RVA: 0x0012BEC1 File Offset: 0x0012A2C1
	[Inject]
	public IServerConnectionManager serverManager { get; set; }

	// Token: 0x17001012 RID: 4114
	// (get) Token: 0x060043F2 RID: 17394 RVA: 0x0012BECA File Offset: 0x0012A2CA
	// (set) Token: 0x060043F3 RID: 17395 RVA: 0x0012BED2 File Offset: 0x0012A2D2
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x17001013 RID: 4115
	// (get) Token: 0x060043F4 RID: 17396 RVA: 0x0012BEDB File Offset: 0x0012A2DB
	// (set) Token: 0x060043F5 RID: 17397 RVA: 0x0012BEE3 File Offset: 0x0012A2E3
	[Inject]
	public IOfflineModeDetector offlineMode { get; set; }

	// Token: 0x17001014 RID: 4116
	// (get) Token: 0x060043F6 RID: 17398 RVA: 0x0012BEEC File Offset: 0x0012A2EC
	// (set) Token: 0x060043F7 RID: 17399 RVA: 0x0012BEF4 File Offset: 0x0012A2F4
	[Inject]
	public IRichPresence richPresence { get; set; }

	// Token: 0x17001015 RID: 4117
	// (get) Token: 0x060043F8 RID: 17400 RVA: 0x0012BEFD File Offset: 0x0012A2FD
	// (set) Token: 0x060043F9 RID: 17401 RVA: 0x0012BF05 File Offset: 0x0012A305
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17001016 RID: 4118
	// (get) Token: 0x060043FA RID: 17402 RVA: 0x0012BF0E File Offset: 0x0012A30E
	// (set) Token: 0x060043FB RID: 17403 RVA: 0x0012BF16 File Offset: 0x0012A316
	[Inject]
	public P2PServerMgr p2pServerMgr { get; set; }

	// Token: 0x060043FC RID: 17404 RVA: 0x0012BF1F File Offset: 0x0012A31F
	[PostConstruct]
	public void Init()
	{
		base.UseOverrideOpenSound = true;
		base.OverrideOpenSound = AudioData.Empty;
		base.UseOverrideCloseSound = true;
		base.OverrideCloseSound = AudioData.Empty;
	}

	// Token: 0x060043FD RID: 17405 RVA: 0x0012BF48 File Offset: 0x0012A348
	private void Start()
	{
		this.subMenuController = base.injector.GetInstance<MenuItemList>();
		this.button_playLocal = UnityEngine.Object.Instantiate<GameObject>(this.SubMenuButtonPrefab).GetComponent<MenuItemButton>();
		this.button_createCustomLobby = UnityEngine.Object.Instantiate<GameObject>(this.SubMenuButtonPrefab).GetComponent<MenuItemButton>();
		this.button_joinCustomLobby = UnityEngine.Object.Instantiate<GameObject>(this.SubMenuButtonPrefab).GetComponent<MenuItemButton>();
		this.willDisableButton.Add(this.button_createCustomLobby);
		this.willDisableButton.Add(this.button_joinCustomLobby);
		this.willDisableButton.Add(this.button_playLocal);
		foreach (MenuItemButton menuItemButton in this.willDisableButton)
		{
			menuItemButton.DisableType = ButtonAnimator.VisualDisableType.Grey;
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

	// Token: 0x060043FE RID: 17406 RVA: 0x0012C13C File Offset: 0x0012A53C
	private void playLocal()
	{
		this.goToBattle(GameMode.FreeForAll);
	}

	// Token: 0x060043FF RID: 17407 RVA: 0x0012C145 File Offset: 0x0012A545
	private void playTrainingMode()
	{
		this.optionsProfileAPI.SetDefaultGameMode(GameMode.Training);
		this.goToBattle(GameMode.Training);
	}

	// Token: 0x06004400 RID: 17408 RVA: 0x0012C15A File Offset: 0x0012A55A
	private void playOnlineCasual1v1()
	{
		this.QueueForMatch(EQueueTypes.Quick);
	}

	// Token: 0x06004401 RID: 17409 RVA: 0x0012C163 File Offset: 0x0012A563
	private void playOnlineCasualFFA()
	{
		this.QueueForMatch(EQueueTypes.FFA);
	}

	// Token: 0x06004402 RID: 17410 RVA: 0x0012C16C File Offset: 0x0012A56C
	private void QueueForMatch(EQueueTypes matchType)
	{
		if (!this.canPlay(matchType))
		{
			return;
		}
	}

	// Token: 0x06004403 RID: 17411 RVA: 0x0012C17C File Offset: 0x0012A57C
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

	// Token: 0x06004404 RID: 17412 RVA: 0x0012C25D File Offset: 0x0012A65D
	private bool canStartCustomLobby(bool isCreate)
	{
		return true;
	}

	// Token: 0x06004405 RID: 17413 RVA: 0x0012C260 File Offset: 0x0012A660
	private void createCustomLobby()
	{
		if (this.canStartCustomLobby(true))
		{
			this.createLobbyDialog = base.dialogController.ShowCreateLobbyDialog(true);
			this.createLobbyDialog.CloseCallback = delegate()
			{
				this.createLobbyDialog = null;
			};
		}
	}

	// Token: 0x06004406 RID: 17414 RVA: 0x0012C298 File Offset: 0x0012A698
	public bool TryJoinSteamCustomLobby(ulong steamLobbyID)
	{
		if (this.canStartCustomLobby(true))
		{
			this.joinLobbyDialog = base.dialogController.ShowJoinLobbyDialog();
			this.joinLobbyDialog.JoinCustomLobby(steamLobbyID);
			this.joinLobbyDialog.CloseCallback = delegate()
			{
				this.joinLobbyDialog = null;
			};
			return true;
		}
		return false;
	}

	// Token: 0x06004407 RID: 17415 RVA: 0x0012C2E8 File Offset: 0x0012A6E8
	public bool TryJoinCustomLobby(string lobbyName, string lobbyPassword)
	{
		if (this.canStartCustomLobby(true))
		{
			this.p2pServerMgr.UpdateLobbyists(false);
			return true;
		}
		return false;
	}

	// Token: 0x06004408 RID: 17416 RVA: 0x0012C305 File Offset: 0x0012A705
	private void joinCustomLobby()
	{
		if (this.canStartCustomLobby(true))
		{
			this.joinLobbyDialog = base.dialogController.ShowJoinLobbyDialog();
			this.joinLobbyDialog.CloseCallback = delegate()
			{
				this.joinLobbyDialog = null;
			};
		}
	}

	// Token: 0x06004409 RID: 17417 RVA: 0x0012C33B File Offset: 0x0012A73B
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

	// Token: 0x0600440A RID: 17418 RVA: 0x0012C370 File Offset: 0x0012A770
	private bool hasOtherWindow()
	{
		foreach (BaseWindow baseWindow in base.uiManager.GetActiveWindows())
		{
			if (!(baseWindow is MainMenuFoldout))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600440B RID: 17419 RVA: 0x0012C3AF File Offset: 0x0012A7AF
	private void goToBattle(GameMode mode)
	{
		this.optionsProfileAPI.SetDefaultGameMode(mode);
		this.resetOptions(delegate
		{
			this.enterNewGame.InitPayload(GameStartType.FreePlay, null);
			this.richPresence.SetPresence("InCharacterSelect", null, null, null);
			this.events.Broadcast(new LoadScreenCommand(ScreenType.CharacterSelect, null, ScreenUpdateType.Next));
		});
	}

	// Token: 0x0600440C RID: 17420 RVA: 0x0012C3D0 File Offset: 0x0012A7D0
	private void resetOptions(Action callback)
	{
		this.optionsProfileAPI.DeleteDefaultSettings(delegate(SaveOptionsProfileResult result)
		{
			if (result != SaveOptionsProfileResult.SUCCESS)
			{
				this.dialogController.ShowOneButtonDialog("Placeholder error", "There was an error", "Continue", WindowTransition.STANDARD_FADE, false, default(AudioData));
			}
			else
			{
				callback();
			}
		});
	}

	// Token: 0x0600440D RID: 17421 RVA: 0x0012C408 File Offset: 0x0012A808
	private void addToSubList(MenuItemButton button, LayoutGroup layoutGroup, Action callback)
	{
		button.transform.SetParent(layoutGroup.transform, false);
		button.DisableDuration = 0.075f;
		button.DisableType = ButtonAnimator.VisualDisableType.Grey;
		this.subMenuController.AddButton(button, callback);
	}

	// Token: 0x0600440E RID: 17422 RVA: 0x0012C43B File Offset: 0x0012A83B
	private void onUpdateRequestMatchStatus(GameEvent message)
	{
		this.onUpdate();
	}

	// Token: 0x0600440F RID: 17423 RVA: 0x0012C443 File Offset: 0x0012A843
	private void onUpdate()
	{
		this.updateQueueText();
		this.updateDisabledButtons();
	}

	// Token: 0x06004410 RID: 17424 RVA: 0x0012C451 File Offset: 0x0012A851
	private void updateQueueText()
	{
	}

	// Token: 0x06004411 RID: 17425 RVA: 0x0012C453 File Offset: 0x0012A853
	public void InternalCloseSubmenu()
	{
		this.RequestCloseSubmenu();
	}

	// Token: 0x06004412 RID: 17426 RVA: 0x0012C460 File Offset: 0x0012A860
	public void Activate()
	{
		this.subMenuController.AutoSelect(this.button_playLocal);
	}

	// Token: 0x06004413 RID: 17427 RVA: 0x0012C473 File Offset: 0x0012A873
	public void Deactivate()
	{
		this.subMenuController.Disable();
	}

	// Token: 0x06004414 RID: 17428 RVA: 0x0012C480 File Offset: 0x0012A880
	public override void OnCancelPressed()
	{
		this.InternalCloseSubmenu();
	}

	// Token: 0x06004415 RID: 17429 RVA: 0x0012C488 File Offset: 0x0012A888
	private void updateQueueButton(EQueueTypes queueType, MenuItemButton button)
	{
	}

	// Token: 0x06004416 RID: 17430 RVA: 0x0012C48A File Offset: 0x0012A88A
	private void updateDisabledButtons()
	{
	}

	// Token: 0x06004417 RID: 17431 RVA: 0x0012C48C File Offset: 0x0012A88C
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

	// Token: 0x04002D3A RID: 11578
	public GameObject SubMenuButtonPrefab;

	// Token: 0x04002D3B RID: 11579
	public VerticalLayoutGroup LocalPlayList;

	// Token: 0x04002D3C RID: 11580
	public VerticalLayoutGroup CasualOnlineList;

	// Token: 0x04002D3D RID: 11581
	public VerticalLayoutGroup CustomLobbiesList;

	// Token: 0x04002D3E RID: 11582
	public TextMeshProUGUI PlayLocal;

	// Token: 0x04002D3F RID: 11583
	public TextMeshProUGUI PlayOnline;

	// Token: 0x04002D40 RID: 11584
	public TextMeshProUGUI CustomLobbies;

	// Token: 0x04002D41 RID: 11585
	private MenuItemList subMenuController;

	// Token: 0x04002D42 RID: 11586
	private MenuItemButton button_playLocal;

	// Token: 0x04002D43 RID: 11587
	private MenuItemButton button_trainingMode;

	// Token: 0x04002D44 RID: 11588
	private MenuItemButton button_onlineCasual1v1;

	// Token: 0x04002D45 RID: 11589
	private MenuItemButton button_onlineCasualFFA;

	// Token: 0x04002D46 RID: 11590
	private MenuItemButton button_createCustomLobby;

	// Token: 0x04002D47 RID: 11591
	private MenuItemButton button_joinCustomLobby;

	// Token: 0x04002D48 RID: 11592
	private List<MenuItemButton> willDisableButton = new List<MenuItemButton>();

	// Token: 0x04002D49 RID: 11593
	public Action RequestCloseSubmenu;

	// Token: 0x04002D4A RID: 11594
	private CreateLobbyDialog createLobbyDialog;

	// Token: 0x04002D4B RID: 11595
	private JoinLobbyDialog joinLobbyDialog;
}
