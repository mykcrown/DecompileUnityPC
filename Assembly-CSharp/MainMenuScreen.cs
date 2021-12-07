using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020009AC RID: 2476
public class MainMenuScreen : GameScreen
{
	// Token: 0x1700101D RID: 4125
	// (get) Token: 0x0600442E RID: 17454 RVA: 0x0012C738 File Offset: 0x0012AB38
	// (set) Token: 0x0600442F RID: 17455 RVA: 0x0012C740 File Offset: 0x0012AB40
	[Inject]
	public IServerConnectionManager serverManager { get; set; }

	// Token: 0x1700101E RID: 4126
	// (get) Token: 0x06004430 RID: 17456 RVA: 0x0012C749 File Offset: 0x0012AB49
	// (set) Token: 0x06004431 RID: 17457 RVA: 0x0012C751 File Offset: 0x0012AB51
	[Inject]
	public DeveloperConfig devConfig { get; set; }

	// Token: 0x1700101F RID: 4127
	// (get) Token: 0x06004432 RID: 17458 RVA: 0x0012C75A File Offset: 0x0012AB5A
	// (set) Token: 0x06004433 RID: 17459 RVA: 0x0012C762 File Offset: 0x0012AB62
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x17001020 RID: 4128
	// (get) Token: 0x06004434 RID: 17460 RVA: 0x0012C76B File Offset: 0x0012AB6B
	// (set) Token: 0x06004435 RID: 17461 RVA: 0x0012C773 File Offset: 0x0012AB73
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x17001021 RID: 4129
	// (get) Token: 0x06004436 RID: 17462 RVA: 0x0012C77C File Offset: 0x0012AB7C
	// (set) Token: 0x06004437 RID: 17463 RVA: 0x0012C784 File Offset: 0x0012AB84
	[Inject]
	public IReplaySystem replaySystem { get; set; }

	// Token: 0x17001022 RID: 4130
	// (get) Token: 0x06004438 RID: 17464 RVA: 0x0012C78D File Offset: 0x0012AB8D
	// (set) Token: 0x06004439 RID: 17465 RVA: 0x0012C795 File Offset: 0x0012AB95
	[Inject]
	public DebugKeys debugKeys { get; set; }

	// Token: 0x17001023 RID: 4131
	// (get) Token: 0x0600443A RID: 17466 RVA: 0x0012C79E File Offset: 0x0012AB9E
	// (set) Token: 0x0600443B RID: 17467 RVA: 0x0012C7A6 File Offset: 0x0012ABA6
	[Inject]
	public IStoreAPI storeAPI { get; set; }

	// Token: 0x17001024 RID: 4132
	// (get) Token: 0x0600443C RID: 17468 RVA: 0x0012C7AF File Offset: 0x0012ABAF
	// (set) Token: 0x0600443D RID: 17469 RVA: 0x0012C7B7 File Offset: 0x0012ABB7
	[Inject]
	public IStoreTabsModel storeTabsModel { get; set; }

	// Token: 0x17001025 RID: 4133
	// (get) Token: 0x0600443E RID: 17470 RVA: 0x0012C7C0 File Offset: 0x0012ABC0
	// (set) Token: 0x0600443F RID: 17471 RVA: 0x0012C7C8 File Offset: 0x0012ABC8
	[Inject]
	public IUserInventory userInventory { get; set; }

	// Token: 0x17001026 RID: 4134
	// (get) Token: 0x06004440 RID: 17472 RVA: 0x0012C7D1 File Offset: 0x0012ABD1
	// (set) Token: 0x06004441 RID: 17473 RVA: 0x0012C7D9 File Offset: 0x0012ABD9
	[Inject]
	public IUserLootboxesModel userLootboxesModel { get; set; }

	// Token: 0x17001027 RID: 4135
	// (get) Token: 0x06004442 RID: 17474 RVA: 0x0012C7E2 File Offset: 0x0012ABE2
	// (set) Token: 0x06004443 RID: 17475 RVA: 0x0012C7EA File Offset: 0x0012ABEA
	[Inject]
	public IMainMenuAPI api { get; set; }

	// Token: 0x17001028 RID: 4136
	// (get) Token: 0x06004444 RID: 17476 RVA: 0x0012C7F3 File Offset: 0x0012ABF3
	// (set) Token: 0x06004445 RID: 17477 RVA: 0x0012C7FB File Offset: 0x0012ABFB
	[Inject]
	public IRichPresence richPresence { get; set; }

	// Token: 0x17001029 RID: 4137
	// (get) Token: 0x06004446 RID: 17478 RVA: 0x0012C804 File Offset: 0x0012AC04
	// (set) Token: 0x06004447 RID: 17479 RVA: 0x0012C80C File Offset: 0x0012AC0C
	[Inject]
	public IAutoJoin autoJoin { get; set; }

	// Token: 0x1700102A RID: 4138
	// (get) Token: 0x06004448 RID: 17480 RVA: 0x0012C815 File Offset: 0x0012AC15
	// (set) Token: 0x06004449 RID: 17481 RVA: 0x0012C81D File Offset: 0x0012AC1D
	[Inject]
	public IOfflineModeDetector offlineMode { private get; set; }

	// Token: 0x1700102B RID: 4139
	// (get) Token: 0x0600444A RID: 17482 RVA: 0x0012C826 File Offset: 0x0012AC26
	// (set) Token: 0x0600444B RID: 17483 RVA: 0x0012C82E File Offset: 0x0012AC2E
	[Inject]
	public OptionsProfileAPI optionsProfileAPI { get; set; }

	// Token: 0x1700102C RID: 4140
	// (get) Token: 0x0600444C RID: 17484 RVA: 0x0012C837 File Offset: 0x0012AC37
	protected override bool SupportsAutoJoin
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600444D RID: 17485 RVA: 0x0012C83C File Offset: 0x0012AC3C
	public override void Start()
	{
		base.Start();
		this.scene = base.uiAdapter.GetUIScene<MainMenuScene>();
		if (this.scene.isStageScene())
		{
			this.Splash.SetActive(false);
			this.characterHighlight();
		}
		this.userInputManager.ResetPlayerMapping();
		this.mainMenuController = base.createMenuController();
		bool key = Input.GetKey(KeyCode.LeftControl);
		string versionString = BuildConfigUtil.GetVersionString(base.config, this.devConfig.onlineMode, key);
		this.VersionText.text = versionString;
		this.replaySystem.Mode = ((!base.config.replaySettings.replayButtonEnabled) ? ReplayMode.Disabled : ReplayMode.Record);
		this.subMenu = base.windowDisplay.Add<MainMenuFoldout>(this.SubmenuPrefab, WindowTransition.STANDARD_FADE, true, true, false, default(AudioData));
		this.subMenu.RequestCloseSubmenu = new Action(this.closeSubMenu);
		this.playButton = UnityEngine.Object.Instantiate<GameObject>(this.MenuButtonPrefab).GetComponent<MenuItemButton>();
		this.galleryButton = UnityEngine.Object.Instantiate<GameObject>(this.MenuButtonPrefab).GetComponent<MenuItemButton>();
		this.replayButton = UnityEngine.Object.Instantiate<GameObject>(this.MenuButtonPrefab).GetComponent<MenuItemButton>();
		this.settingsButton = UnityEngine.Object.Instantiate<GameObject>(this.MenuButtonSmallPrefab).GetComponent<MenuItemButton>();
		this.quitButton = UnityEngine.Object.Instantiate<GameObject>(this.MenuButtonSmallPrefab).GetComponent<MenuItemButton>();
		this.creditsButton = UnityEngine.Object.Instantiate<GameObject>(this.MenuButtonSmallPrefab).GetComponent<MenuItemButton>();
		this.trainingButton = UnityEngine.Object.Instantiate<GameObject>(this.MenuButtonSmallPrefab).GetComponent<MenuItemButton>();
		this.trainingButton.SetText(base.localization.GetText("ui.main.train"));
		this.playButton.name = "Play_Button";
		this.playButton.SetText(base.localization.GetText("ui.main.play"));
		this.replayButton.SetText(base.localization.GetText("ui.main.replays"));
		this.settingsButton.SetText(base.localization.GetText("ui.main.settings"));
		this.quitButton.SetText(base.localization.GetText("ui.main.quit"));
		this.creditsButton.SetText(base.localization.GetText("ui.main.credits"));
		if (this.galleryButton)
		{
			this.galleryButton.SetText(base.localization.GetText("ui.main.gallery"));
		}
		this.addToMainList(this.playButton, this.MainMenuList, new Action(this.play));
		if (this.galleryButton)
		{
			this.galleryButton.transform.SetParent(this.MainMenuList.transform, false);
			this.mainMenuController.AddButton(this.galleryButton, new Action<InputEventData>(this.gallery));
		}
		this.addToMainList(this.replayButton, this.MainMenuList, new Action(this.replays));
		this.addToMainList(this.trainingButton, this.MainMenuList, new Action(this.train));
		this.addToMainList(this.settingsButton, this.MainMenuList, new Action<InputEventData>(this.settings));
		this.addToMainList(this.creditsButton, this.MainMenuList, new Action<InputEventData>(this.credits));
		this.addToMainList(this.quitButton, this.MainMenuList, new Action(this.quit));
		this.mainMenuController.LandingPoint = this.playButton;
		this.mainMenuController.SetSelectorImage(this.SelectorImage);
		this.addPromos();
		this.mainMenuController.OnSelectorImageChanged = new Action<MenuItemButton>(this.onSelectorImageChanged);
		this.mainMenuController.Initialize();
		this.updateButtons();
		this.MainMenuList.Redraw();
		this.MainMenuList.enabled = false;
		base.onDrawComplete();
		base.addInputInstuctionsForMenuScreen(this.InputInstructionsAnchor, this.InputInstructionsPrefab);
		base.lockInput();
		this.tweenIn();
		this.subMenu.transform.position = this.SubMenuClosedAnchor.transform.position;
		base.audioManager.PlayMusic(this.api.GetCurrentMusic());
	}

	// Token: 0x0600444E RID: 17486 RVA: 0x0012CC60 File Offset: 0x0012B060
	protected override void onAutoJoinRequest()
	{
		if (this.tweenInComplete)
		{
			this.openSubMenu();
			if (this.subMenu.TryJoinSteamCustomLobby(this.autoJoin.LobbySteamID))
			{
				this.autoJoin.Clear();
			}
			else
			{
				Debug.LogError("Cannot create join dialog");
			}
		}
	}

	// Token: 0x0600444F RID: 17487 RVA: 0x0012CCB3 File Offset: 0x0012B0B3
	private void characterHighlight()
	{
		this.scene.SetCharacter(this.api.characterHighlight);
	}

	// Token: 0x06004450 RID: 17488 RVA: 0x0012CCCC File Offset: 0x0012B0CC
	private void onSelectorImageChanged(MenuItemButton button)
	{
		float num = this.SelectorImageLargeHeight;
		if ((base.uiManager.CurrentInputModule as UIInputModule).CurrentMode == ControlMode.MouseMode)
		{
			num *= this.SelectorImageMouseReduce;
		}
		Vector2 sizeDelta = this.SelectorImage.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.y = num;
		this.SelectorImage.GetComponent<RectTransform>().sizeDelta = sizeDelta;
	}

	// Token: 0x06004451 RID: 17489 RVA: 0x0012CD2E File Offset: 0x0012B12E
	protected override FirstSelectionInfo getInitialSelection()
	{
		return new FirstSelectionInfo(this.mainMenuController, this.playButton);
	}

	// Token: 0x06004452 RID: 17490 RVA: 0x0012CD44 File Offset: 0x0012B144
	private void tweenIn()
	{
		MenuItemButton[] buttons = this.mainMenuController.GetButtons();
		float delay = 0f;
		for (int i = 0; i < buttons.Length; i++)
		{
			MenuItemButton menuItemButton = buttons[i];
			delay = 0.025f * (float)i;
			this.mainMenuTweenInItem(menuItemButton.gameObject, -1, 0, 0.25f, delay, null);
		}
		this.mainMenuTweenInItem(this.SelectorImage.gameObject, -1, 0, 0.25f, 0f, null);
		this.mainMenuTweenInItem(this.PromoContainer, 1, 0, 0.25f, delay, delegate
		{
			this.onTweenInComplete();
		});
	}

	// Token: 0x06004453 RID: 17491 RVA: 0x0012CDD8 File Offset: 0x0012B1D8
	private void onTweenInComplete()
	{
		base.unlockInput();
		List<Func<BaseWindow>> createWindows = new List<Func<BaseWindow>>();
		GenericWindowFlow instance = base.injector.GetInstance<GenericWindowFlow>();
		GenericWindowFlow genericWindowFlow = instance;
		genericWindowFlow.AllClosedCallback = (Action)Delegate.Combine(genericWindowFlow.AllClosedCallback, new Action(this.initInputAfterTween));
		instance.StartFlow(createWindows);
	}

	// Token: 0x06004454 RID: 17492 RVA: 0x0012CE28 File Offset: 0x0012B228
	private void initInputAfterTween()
	{
		this.tweenInComplete = true;
		if (!this.autoJoin.AutoJoinIfSet() && !base.uiManager.CurrentInputModule.IsMouseMode)
		{
			this.handleMenuSelectionOnButtonMode();
		}
	}

	// Token: 0x06004455 RID: 17493 RVA: 0x0012CE6C File Offset: 0x0012B26C
	private void mainMenuTweenInItem(GameObject target, int directionX, int directionY, float time, float delay = 0f, Action callback = null)
	{
		LayoutElement component = target.GetComponent<LayoutElement>();
		if (component != null)
		{
			component.ignoreLayout = true;
		}
		base.tweenInItem(target, directionX, directionY, time, delay, callback);
	}

	// Token: 0x06004456 RID: 17494 RVA: 0x0012CEA4 File Offset: 0x0012B2A4
	private void addPromos()
	{
		this.promoMenuController = base.createMenuController();
		PromoButton promoSlot = this.PromoSlot1;
		if (promoSlot != null)
		{
			promoSlot.SetText("Test Promo");
			promoSlot.SetImage(this.TestPromoImage1);
		}
		PromoButton promoSlot2 = this.PromoSlot2;
		promoSlot2.SetText("Test Promotion 2");
		if (base.gameDataManager.GameData.IsFeatureEnabled(FeatureID.FoundersPack))
		{
			promoSlot2.SetImage(this.FoundersPackPromoImage);
		}
		else
		{
			promoSlot2.SetImage(this.TestPromoImage2);
		}
		this.promoButtonList.Add(promoSlot2);
		for (int i = 0; i < this.promoButtonList.Count; i++)
		{
			PromoButton button = this.promoButtonList[i];
			this.promoMenuController.AddButton(this.promoButtonList[i], delegate()
			{
				this.onPromoButtonClicked(button);
			});
		}
		this.promoMenuController.LandingPoint = this.promoButtonList[0];
		this.mainMenuController.AddEdgeNavigation(MoveDirection.Right, this.promoMenuController);
		MenuItemList menuItemList = this.promoMenuController;
		menuItemList.OnSelected = (Action<MenuItemButton, BaseEventData>)Delegate.Combine(menuItemList.OnSelected, new Action<MenuItemButton, BaseEventData>(this.onPromoButtonSelected));
		this.promoMenuController.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.promoMenuController.DisableGridWrap();
		this.promoMenuController.AddEdgeNavigation(MoveDirection.Left, this.mainMenuController);
		this.promoMenuController.Initialize();
	}

	// Token: 0x06004457 RID: 17495 RVA: 0x0012D01C File Offset: 0x0012B41C
	private void onPromoButtonClicked(PromoButton button)
	{
		if (button == this.PromoSlot1)
		{
			Application.OpenURL("https://icons.gg/news");
		}
		else if (button == this.PromoSlot2)
		{
			if (base.gameDataManager.GameData.IsFeatureEnabled(FeatureID.FoundersPack))
			{
				this.storeAPI.Mode = StoreMode.NORMAL;
				this.storeTabsModel.Current = StoreTab.FEATURED;
				base.events.Broadcast(new LoadScreenCommand(ScreenType.StoreScreen, null, ScreenUpdateType.Next));
			}
			else
			{
				Application.OpenURL("https://discord.gg/vE8DvVG");
			}
		}
	}

	// Token: 0x06004458 RID: 17496 RVA: 0x0012D0AB File Offset: 0x0012B4AB
	private void onPromoButtonSelected(MenuItemButton thePromoButton, BaseEventData eventData)
	{
		thePromoButton.transform.SetAsLastSibling();
	}

	// Token: 0x06004459 RID: 17497 RVA: 0x0012D0B8 File Offset: 0x0012B4B8
	private void addToMainList(MenuItemButton button, LayoutGroup layoutGroup, Action callback)
	{
		button.transform.SetParent(layoutGroup.transform, false);
		this.mainMenuController.AddButton(button, callback);
	}

	// Token: 0x0600445A RID: 17498 RVA: 0x0012D0D9 File Offset: 0x0012B4D9
	private void addToMainList(MenuItemButton button, LayoutGroup layoutGroup, Action<InputEventData> callback)
	{
		button.transform.SetParent(layoutGroup.transform, false);
		this.mainMenuController.AddButton(button, callback);
	}

	// Token: 0x0600445B RID: 17499 RVA: 0x0012D0FA File Offset: 0x0012B4FA
	protected override void onScreenSizeUpdate()
	{
		this.instantFinishSubmenuTweens();
	}

	// Token: 0x0600445C RID: 17500 RVA: 0x0012D102 File Offset: 0x0012B502
	public override void OnCancelPressed()
	{
		this.closeSubMenu();
	}

	// Token: 0x0600445D RID: 17501 RVA: 0x0012D10A File Offset: 0x0012B50A
	protected override bool isAnythingFocused()
	{
		return base.isAnyMenuItemSelected() || this.hasOtherWindow() || this.promoMenuController.CurrentSelection != null;
	}

	// Token: 0x0600445E RID: 17502 RVA: 0x0012D140 File Offset: 0x0012B540
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

	// Token: 0x0600445F RID: 17503 RVA: 0x0012D180 File Offset: 0x0012B580
	protected override void handleMenuSelectionOnButtonMode()
	{
		if (!this.tweenInComplete)
		{
			return;
		}
		if (this.subMenuOpen)
		{
			this.subMenu.OnButtonMode();
		}
		else if (!this.isAnythingFocused())
		{
			this.FirstSelected = this.playButton.InteractableButton.gameObject;
			this.mainMenuController.AutoSelect(this.playButton);
		}
	}

	// Token: 0x06004460 RID: 17504 RVA: 0x0012D1E8 File Offset: 0x0012B5E8
	private void updateButtons()
	{
		bool flag = false;
		if (this.debugKeys.EnableReleaseReplays || base.gameDataManager.GameData.IsFeatureEnabled(FeatureID.Replays))
		{
			flag = this.replaySystem.HasFile(base.config.replaySettings.replayName);
		}
		this.mainMenuController.SetButtonEnabled(this.replayButton, flag);
		this.replayButton.gameObject.SetActive(flag);
		bool isEnabled = base.gameDataManager.GameData.IsFeatureEnabled(FeatureID.Store);
		if (this.galleryButton)
		{
			this.mainMenuController.SetButtonEnabled(this.galleryButton, isEnabled);
		}
	}

	// Token: 0x06004461 RID: 17505 RVA: 0x0012D290 File Offset: 0x0012B690
	public void OnSubmenuCloseButton()
	{
		this.closeSubMenu();
	}

	// Token: 0x06004462 RID: 17506 RVA: 0x0012D298 File Offset: 0x0012B698
	private void closeSubMenu()
	{
		if (!this.subMenuOpen)
		{
			return;
		}
		if (this.SubMenuShroud == null)
		{
			return;
		}
		this.subMenuOpen = false;
		this.subMenu.Deactivate();
		this.mainMenuController.Unlock();
		this.SubMenuShroud.SetActive(false);
		this.FirstSelected = this.playButton.InteractableButton.gameObject;
		this.mainMenuController.AutoSelect(this.playButton);
		if (this._subMenuTween != null && this._subMenuTween.IsPlaying())
		{
			this._subMenuTween.Kill(false);
		}
		Transform target = this.subMenu.transform;
		this._subMenuTween = DOTween.To(() => target.position, delegate(Vector3 x)
		{
			target.position = x;
		}, this.SubMenuClosedAnchor.transform.position, 0.25f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.subMenuTweenComplete));
		base.audioManager.PlayMenuSound(SoundKey.mainMenu_mainMenuFoldoutClose, 0f);
	}

	// Token: 0x06004463 RID: 17507 RVA: 0x0012D3B4 File Offset: 0x0012B7B4
	private void openSubMenu()
	{
		if (this.subMenuOpen)
		{
			return;
		}
		if (this.SubMenuShroud == null)
		{
			return;
		}
		this.subMenuOpen = true;
		this.mainMenuController.Lock();
		this.SubMenuShroud.SetActive(true);
		this.subMenu.Activate();
		this.SubMenuContainer.SetActive(true);
		if (this._subMenuTween != null && this._subMenuTween.IsPlaying())
		{
			this._subMenuTween.Kill(false);
		}
		Transform target = this.subMenu.transform;
		this._subMenuTween = DOTween.To(() => target.position, delegate(Vector3 x)
		{
			target.position = x;
		}, this.SubMenuOpenAnchor.transform.position, 0.25f).SetEase(Ease.InSine).OnComplete(new TweenCallback(this.subMenuTweenComplete));
		base.audioManager.PlayMenuSound(SoundKey.mainMenu_mainMenuFoldoutOpen, 0f);
	}

	// Token: 0x06004464 RID: 17508 RVA: 0x0012D4B4 File Offset: 0x0012B8B4
	private void instantFinishSubmenuTweens()
	{
		if (this._subMenuTween != null && this._subMenuTween.IsPlaying())
		{
			this._subMenuTween.Kill(false);
		}
		if (this.subMenuOpen)
		{
			this.subMenu.transform.position = this.SubMenuOpenAnchor.transform.position;
		}
		else
		{
			this.subMenu.transform.position = this.SubMenuClosedAnchor.transform.position;
		}
	}

	// Token: 0x06004465 RID: 17509 RVA: 0x0012D538 File Offset: 0x0012B938
	private void subMenuTweenComplete()
	{
		this._subMenuTween = null;
	}

	// Token: 0x06004466 RID: 17510 RVA: 0x0012D541 File Offset: 0x0012B941
	private void play()
	{
		this.openSubMenu();
	}

	// Token: 0x06004467 RID: 17511 RVA: 0x0012D549 File Offset: 0x0012B949
	private void train()
	{
		this.optionsProfileAPI.SetDefaultGameMode(GameMode.Training);
		this.goToBattle(GameMode.Training);
	}

	// Token: 0x06004468 RID: 17512 RVA: 0x0012D55E File Offset: 0x0012B95E
	private void goToBattle(GameMode mode)
	{
		this.optionsProfileAPI.SetDefaultGameMode(mode);
		this.resetOptions(delegate
		{
			this.enterNewGame.InitPayload(GameStartType.FreePlay, null);
			this.richPresence.SetPresence("InCharacterSelect", null, null, null);
			base.events.Broadcast(new LoadScreenCommand(ScreenType.CharacterSelect, null, ScreenUpdateType.Next));
		});
	}

	// Token: 0x06004469 RID: 17513 RVA: 0x0012D580 File Offset: 0x0012B980
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

	// Token: 0x0600446A RID: 17514 RVA: 0x0012D5B8 File Offset: 0x0012B9B8
	private void replays()
	{
		this.enterNewGame.StartReplay(this.replaySystem);
	}

	// Token: 0x0600446B RID: 17515 RVA: 0x0012D5CC File Offset: 0x0012B9CC
	public override void GoToNextScreen()
	{
		this.richPresence.SetPresence("InCharacterSelect", null, null, null);
		base.events.Broadcast(new LoadScreenCommand(ScreenType.CharacterSelect, null, ScreenUpdateType.Next));
	}

	// Token: 0x0600446C RID: 17516 RVA: 0x0012D5F4 File Offset: 0x0012B9F4
	private void quit()
	{
		Application.Quit();
	}

	// Token: 0x0600446D RID: 17517 RVA: 0x0012D5FC File Offset: 0x0012B9FC
	private void settings(InputEventData data)
	{
		int portId = 0;
		if (data.port != null)
		{
			portId = data.port.Id;
		}
		InputSettingsPayload inputSettingsPayload = new InputSettingsPayload();
		inputSettingsPayload.portId = portId;
		base.events.Broadcast(new LoadScreenCommand(ScreenType.SettingsScreen, inputSettingsPayload, ScreenUpdateType.Next));
	}

	// Token: 0x0600446E RID: 17518 RVA: 0x0012D64A File Offset: 0x0012BA4A
	private void feedback(InputEventData data)
	{
		base.dialogController.ShowFeedbackDialog();
	}

	// Token: 0x0600446F RID: 17519 RVA: 0x0012D658 File Offset: 0x0012BA58
	private void store()
	{
		this.storeAPI.Mode = StoreMode.NORMAL;
		base.events.Broadcast(new LoadScreenCommand(ScreenType.StoreScreen, null, ScreenUpdateType.Next));
	}

	// Token: 0x06004470 RID: 17520 RVA: 0x0012D67A File Offset: 0x0012BA7A
	private void credits(InputEventData data)
	{
		base.events.Broadcast(new LoadScreenCommand(ScreenType.CreditsScreen, null, ScreenUpdateType.Next));
	}

	// Token: 0x06004471 RID: 17521 RVA: 0x0012D690 File Offset: 0x0012BA90
	private void lootBoxes()
	{
		base.audioManager.StopMusic(null, 0.5f);
		this.storeAPI.Mode = StoreMode.UNBOXING;
		if (base.gameDataManager.IsFeatureEnabled(FeatureID.LootBoxPurchase))
		{
			this.storeTabsModel.Current = StoreTab.LOOT_BOXES;
		}
		base.events.Broadcast(new LoadScreenCommand(ScreenType.StoreScreen, null, ScreenUpdateType.Next));
	}

	// Token: 0x06004472 RID: 17522 RVA: 0x0012D6EC File Offset: 0x0012BAEC
	private void gallery(InputEventData inputEventData)
	{
		int port = 100;
		if (inputEventData.port != null)
		{
			port = inputEventData.port.Id;
		}
		base.audioManager.StopMusic(null, 0.5f);
		this.storeAPI.Port = port;
		this.storeAPI.Mode = StoreMode.NORMAL;
		this.storeTabsModel.Current = StoreTab.CHARACTERS;
		base.events.Broadcast(new LoadScreenCommand(ScreenType.StoreScreen, null, ScreenUpdateType.Next));
	}

	// Token: 0x06004473 RID: 17523 RVA: 0x0012D764 File Offset: 0x0012BB64
	public void LoadUserScreen()
	{
		base.events.Broadcast(new LoadScreenCommand(ScreenType.UserProfileScreen, null, ScreenUpdateType.Next));
	}

	// Token: 0x06004474 RID: 17524 RVA: 0x0012D77A File Offset: 0x0012BB7A
	public override void OnDestroy()
	{
		if (this.mainMenuController != null)
		{
			this.mainMenuController.OnDestroy();
		}
		if (this.promoMenuController != null)
		{
			this.promoMenuController.OnDestroy();
		}
		this.scene.Exit();
		base.OnDestroy();
	}

	// Token: 0x1700102D RID: 4141
	// (get) Token: 0x06004475 RID: 17525 RVA: 0x0012D7B9 File Offset: 0x0012BBB9
	public override bool WaitForDrawCallback
	{
		get
		{
			return true;
		}
	}

	// Token: 0x04002D65 RID: 11621
	public TextMeshProUGUI VersionText;

	// Token: 0x04002D66 RID: 11622
	private MenuItemButton playButton;

	// Token: 0x04002D67 RID: 11623
	private MenuItemButton trainingButton;

	// Token: 0x04002D68 RID: 11624
	private MenuItemButton galleryButton;

	// Token: 0x04002D69 RID: 11625
	private MenuItemButton replayButton;

	// Token: 0x04002D6A RID: 11626
	private MenuItemButton settingsButton;

	// Token: 0x04002D6B RID: 11627
	private MenuItemButton creditsButton;

	// Token: 0x04002D6C RID: 11628
	private MenuItemButton quitButton;

	// Token: 0x04002D6D RID: 11629
	public GameObject MenuButtonPrefab;

	// Token: 0x04002D6E RID: 11630
	public GameObject MenuButtonSmallPrefab;

	// Token: 0x04002D6F RID: 11631
	public VerticalLayoutGroup MainMenuList;

	// Token: 0x04002D70 RID: 11632
	public GameObject SubMenuContainer;

	// Token: 0x04002D71 RID: 11633
	public GameObject SubMenuShroud;

	// Token: 0x04002D72 RID: 11634
	public GameObject SubMenuClosedAnchor;

	// Token: 0x04002D73 RID: 11635
	public GameObject SubMenuOpenAnchor;

	// Token: 0x04002D74 RID: 11636
	public GameObject SelectorImage;

	// Token: 0x04002D75 RID: 11637
	public float SelectorImageLargeHeight = 90f;

	// Token: 0x04002D76 RID: 11638
	public float SelectorImageSmallHeight = 75f;

	// Token: 0x04002D77 RID: 11639
	public float SelectorImageMouseReduce = 0.8f;

	// Token: 0x04002D78 RID: 11640
	private MenuItemList mainMenuController;

	// Token: 0x04002D79 RID: 11641
	private MenuItemList promoMenuController;

	// Token: 0x04002D7A RID: 11642
	public GameObject PromoContainer;

	// Token: 0x04002D7B RID: 11643
	public PromoButton PromoSlot1;

	// Token: 0x04002D7C RID: 11644
	public PromoButton PromoSlot2;

	// Token: 0x04002D7D RID: 11645
	public Sprite TestPromoImage1;

	// Token: 0x04002D7E RID: 11646
	public Sprite TestPromoImage2;

	// Token: 0x04002D7F RID: 11647
	public Sprite FoundersPackPromoImage;

	// Token: 0x04002D80 RID: 11648
	public GameObject Splash;

	// Token: 0x04002D81 RID: 11649
	public GameObject InputInstructionsPrefab;

	// Token: 0x04002D82 RID: 11650
	public Transform InputInstructionsAnchor;

	// Token: 0x04002D83 RID: 11651
	public GameObject SubmenuPrefab;

	// Token: 0x04002D84 RID: 11652
	private MainMenuFoldout subMenu;

	// Token: 0x04002D85 RID: 11653
	private bool subMenuOpen;

	// Token: 0x04002D86 RID: 11654
	private Tweener _subMenuTween;

	// Token: 0x04002D87 RID: 11655
	private bool tweenInComplete;

	// Token: 0x04002D88 RID: 11656
	private MainMenuScene scene;

	// Token: 0x04002D89 RID: 11657
	private List<PromoButton> promoButtonList = new List<PromoButton>();
}
