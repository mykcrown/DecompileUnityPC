// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuScreen : GameScreen
{
	private sealed class _addPromos_c__AnonStorey0
	{
		internal PromoButton button;

		internal MainMenuScreen _this;

		internal void __m__0()
		{
			this._this.onPromoButtonClicked(this.button);
		}
	}

	private sealed class _closeSubMenu_c__AnonStorey1
	{
		internal Transform target;

		internal Vector3 __m__0()
		{
			return this.target.position;
		}

		internal void __m__1(Vector3 x)
		{
			this.target.position = x;
		}
	}

	private sealed class _openSubMenu_c__AnonStorey2
	{
		internal Transform target;

		internal Vector3 __m__0()
		{
			return this.target.position;
		}

		internal void __m__1(Vector3 x)
		{
			this.target.position = x;
		}
	}

	private sealed class _resetOptions_c__AnonStorey3
	{
		internal Action callback;

		internal MainMenuScreen _this;

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

	public TextMeshProUGUI VersionText;

	private MenuItemButton playButton;

	private MenuItemButton trainingButton;

	private MenuItemButton galleryButton;

	private MenuItemButton replayButton;

	private MenuItemButton settingsButton;

	private MenuItemButton creditsButton;

	private MenuItemButton quitButton;

	public GameObject MenuButtonPrefab;

	public GameObject MenuButtonSmallPrefab;

	public VerticalLayoutGroup MainMenuList;

	public GameObject SubMenuContainer;

	public GameObject SubMenuShroud;

	public GameObject SubMenuClosedAnchor;

	public GameObject SubMenuOpenAnchor;

	public GameObject SelectorImage;

	public float SelectorImageLargeHeight = 90f;

	public float SelectorImageSmallHeight = 75f;

	public float SelectorImageMouseReduce = 0.8f;

	private MenuItemList mainMenuController;

	private MenuItemList promoMenuController;

	public GameObject PromoContainer;

	public PromoButton PromoSlot1;

	public PromoButton PromoSlot2;

	public Sprite TestPromoImage1;

	public Sprite TestPromoImage2;

	public Sprite FoundersPackPromoImage;

	public GameObject Splash;

	public GameObject InputInstructionsPrefab;

	public Transform InputInstructionsAnchor;

	public GameObject SubmenuPrefab;

	private MainMenuFoldout subMenu;

	private bool subMenuOpen;

	private Tweener _subMenuTween;

	private bool tweenInComplete;

	private MainMenuScene scene;

	private List<PromoButton> promoButtonList = new List<PromoButton>();

	[Inject]
	public IServerConnectionManager serverManager
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
	public IUserInputManager userInputManager
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
	public IReplaySystem replaySystem
	{
		get;
		set;
	}

	[Inject]
	public DebugKeys debugKeys
	{
		get;
		set;
	}

	[Inject]
	public IStoreAPI storeAPI
	{
		get;
		set;
	}

	[Inject]
	public IStoreTabsModel storeTabsModel
	{
		get;
		set;
	}

	[Inject]
	public IUserInventory userInventory
	{
		get;
		set;
	}

	[Inject]
	public IUserLootboxesModel userLootboxesModel
	{
		get;
		set;
	}

	[Inject]
	public IMainMenuAPI api
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
	public IAutoJoin autoJoin
	{
		get;
		set;
	}

	[Inject]
	public IOfflineModeDetector offlineMode
	{
		private get;
		set;
	}

	[Inject]
	public OptionsProfileAPI optionsProfileAPI
	{
		get;
		set;
	}

	protected override bool SupportsAutoJoin
	{
		get
		{
			return true;
		}
	}

	public override bool WaitForDrawCallback
	{
		get
		{
			return true;
		}
	}

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
				UnityEngine.Debug.LogError("Cannot create join dialog");
			}
		}
	}

	private void characterHighlight()
	{
		this.scene.SetCharacter(this.api.characterHighlight);
	}

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

	protected override FirstSelectionInfo getInitialSelection()
	{
		return new FirstSelectionInfo(this.mainMenuController, this.playButton);
	}

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
		this.mainMenuTweenInItem(this.PromoContainer, 1, 0, 0.25f, delay, new Action(this._tweenIn_m__0));
	}

	private void onTweenInComplete()
	{
		base.unlockInput();
		List<Func<BaseWindow>> createWindows = new List<Func<BaseWindow>>();
		GenericWindowFlow instance = base.injector.GetInstance<GenericWindowFlow>();
		GenericWindowFlow expr_19 = instance;
		expr_19.AllClosedCallback = (Action)Delegate.Combine(expr_19.AllClosedCallback, new Action(this.initInputAfterTween));
		instance.StartFlow(createWindows);
	}

	private void initInputAfterTween()
	{
		this.tweenInComplete = true;
		if (!this.autoJoin.AutoJoinIfSet() && !base.uiManager.CurrentInputModule.IsMouseMode)
		{
			this.handleMenuSelectionOnButtonMode();
		}
	}

	private void mainMenuTweenInItem(GameObject target, int directionX, int directionY, float time, float delay = 0f, Action callback = null)
	{
		LayoutElement component = target.GetComponent<LayoutElement>();
		if (component != null)
		{
			component.ignoreLayout = true;
		}
		base.tweenInItem(target, directionX, directionY, time, delay, callback);
	}

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
			MainMenuScreen._addPromos_c__AnonStorey0 _addPromos_c__AnonStorey = new MainMenuScreen._addPromos_c__AnonStorey0();
			_addPromos_c__AnonStorey._this = this;
			_addPromos_c__AnonStorey.button = this.promoButtonList[i];
			this.promoMenuController.AddButton(this.promoButtonList[i], new Action(_addPromos_c__AnonStorey.__m__0));
		}
		this.promoMenuController.LandingPoint = this.promoButtonList[0];
		this.mainMenuController.AddEdgeNavigation(MoveDirection.Right, this.promoMenuController);
		MenuItemList expr_114 = this.promoMenuController;
		expr_114.OnSelected = (Action<MenuItemButton, BaseEventData>)Delegate.Combine(expr_114.OnSelected, new Action<MenuItemButton, BaseEventData>(this.onPromoButtonSelected));
		this.promoMenuController.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.promoMenuController.DisableGridWrap();
		this.promoMenuController.AddEdgeNavigation(MoveDirection.Left, this.mainMenuController);
		this.promoMenuController.Initialize();
	}

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

	private void onPromoButtonSelected(MenuItemButton thePromoButton, BaseEventData eventData)
	{
		thePromoButton.transform.SetAsLastSibling();
	}

	private void addToMainList(MenuItemButton button, LayoutGroup layoutGroup, Action callback)
	{
		button.transform.SetParent(layoutGroup.transform, false);
		this.mainMenuController.AddButton(button, callback);
	}

	private void addToMainList(MenuItemButton button, LayoutGroup layoutGroup, Action<InputEventData> callback)
	{
		button.transform.SetParent(layoutGroup.transform, false);
		this.mainMenuController.AddButton(button, callback);
	}

	protected override void onScreenSizeUpdate()
	{
		this.instantFinishSubmenuTweens();
	}

	public override void OnCancelPressed()
	{
		this.closeSubMenu();
	}

	protected override bool isAnythingFocused()
	{
		return base.isAnyMenuItemSelected() || this.hasOtherWindow() || this.promoMenuController.CurrentSelection != null;
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

	public void OnSubmenuCloseButton()
	{
		this.closeSubMenu();
	}

	private void closeSubMenu()
	{
		MainMenuScreen._closeSubMenu_c__AnonStorey1 _closeSubMenu_c__AnonStorey = new MainMenuScreen._closeSubMenu_c__AnonStorey1();
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
		_closeSubMenu_c__AnonStorey.target = this.subMenu.transform;
		this._subMenuTween = DOTween.To(new DOGetter<Vector3>(_closeSubMenu_c__AnonStorey.__m__0), new DOSetter<Vector3>(_closeSubMenu_c__AnonStorey.__m__1), this.SubMenuClosedAnchor.transform.position, 0.25f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.subMenuTweenComplete));
		base.audioManager.PlayMenuSound(SoundKey.mainMenu_mainMenuFoldoutClose, 0f);
	}

	private void openSubMenu()
	{
		MainMenuScreen._openSubMenu_c__AnonStorey2 _openSubMenu_c__AnonStorey = new MainMenuScreen._openSubMenu_c__AnonStorey2();
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
		_openSubMenu_c__AnonStorey.target = this.subMenu.transform;
		this._subMenuTween = DOTween.To(new DOGetter<Vector3>(_openSubMenu_c__AnonStorey.__m__0), new DOSetter<Vector3>(_openSubMenu_c__AnonStorey.__m__1), this.SubMenuOpenAnchor.transform.position, 0.25f).SetEase(Ease.InSine).OnComplete(new TweenCallback(this.subMenuTweenComplete));
		base.audioManager.PlayMenuSound(SoundKey.mainMenu_mainMenuFoldoutOpen, 0f);
	}

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

	private void subMenuTweenComplete()
	{
		this._subMenuTween = null;
	}

	private void play()
	{
		this.openSubMenu();
	}

	private void train()
	{
		this.optionsProfileAPI.SetDefaultGameMode(GameMode.Training);
		this.goToBattle(GameMode.Training);
	}

	private void goToBattle(GameMode mode)
	{
		this.optionsProfileAPI.SetDefaultGameMode(mode);
		this.resetOptions(new Action(this._goToBattle_m__1));
	}

	private void resetOptions(Action callback)
	{
		MainMenuScreen._resetOptions_c__AnonStorey3 _resetOptions_c__AnonStorey = new MainMenuScreen._resetOptions_c__AnonStorey3();
		_resetOptions_c__AnonStorey.callback = callback;
		_resetOptions_c__AnonStorey._this = this;
		this.optionsProfileAPI.DeleteDefaultSettings(new Action<SaveOptionsProfileResult>(_resetOptions_c__AnonStorey.__m__0));
	}

	private void replays()
	{
		this.enterNewGame.StartReplay(this.replaySystem);
	}

	public override void GoToNextScreen()
	{
		this.richPresence.SetPresence("InCharacterSelect", null, null, null);
		base.events.Broadcast(new LoadScreenCommand(ScreenType.CharacterSelect, null, ScreenUpdateType.Next));
	}

	private void quit()
	{
		Application.Quit();
	}

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

	private void feedback(InputEventData data)
	{
		base.dialogController.ShowFeedbackDialog();
	}

	private void store()
	{
		this.storeAPI.Mode = StoreMode.NORMAL;
		base.events.Broadcast(new LoadScreenCommand(ScreenType.StoreScreen, null, ScreenUpdateType.Next));
	}

	private void credits(InputEventData data)
	{
		base.events.Broadcast(new LoadScreenCommand(ScreenType.CreditsScreen, null, ScreenUpdateType.Next));
	}

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

	public void LoadUserScreen()
	{
		base.events.Broadcast(new LoadScreenCommand(ScreenType.UserProfileScreen, null, ScreenUpdateType.Next));
	}

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

	private void _tweenIn_m__0()
	{
		this.onTweenInComplete();
	}

	private void _goToBattle_m__1()
	{
		this.enterNewGame.InitPayload(GameStartType.FreePlay, null);
		this.richPresence.SetPresence("InCharacterSelect", null, null, null);
		base.events.Broadcast(new LoadScreenCommand(ScreenType.CharacterSelect, null, ScreenUpdateType.Next));
	}
}
