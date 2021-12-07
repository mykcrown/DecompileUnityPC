using System;
using System.Collections.Generic;

// Token: 0x02000A75 RID: 2677
public class UIScreenAdapter : IUIAdapter
{
	// Token: 0x17001277 RID: 4727
	// (get) Token: 0x06004E0C RID: 19980 RVA: 0x00148CE8 File Offset: 0x001470E8
	// (set) Token: 0x06004E0D RID: 19981 RVA: 0x00148CF0 File Offset: 0x001470F0
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x17001278 RID: 4728
	// (get) Token: 0x06004E0E RID: 19982 RVA: 0x00148CF9 File Offset: 0x001470F9
	// (set) Token: 0x06004E0F RID: 19983 RVA: 0x00148D01 File Offset: 0x00147101
	[Inject]
	public IUILoaderBindings loadBindings { get; set; }

	// Token: 0x17001279 RID: 4729
	// (get) Token: 0x06004E10 RID: 19984 RVA: 0x00148D0A File Offset: 0x0014710A
	// (set) Token: 0x06004E11 RID: 19985 RVA: 0x00148D12 File Offset: 0x00147112
	[Inject]
	public ILoadDataSequence dataLoader { get; set; }

	// Token: 0x1700127A RID: 4730
	// (get) Token: 0x06004E12 RID: 19986 RVA: 0x00148D1B File Offset: 0x0014711B
	// (set) Token: 0x06004E13 RID: 19987 RVA: 0x00148D23 File Offset: 0x00147123
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x1700127B RID: 4731
	// (get) Token: 0x06004E14 RID: 19988 RVA: 0x00148D2C File Offset: 0x0014712C
	// (set) Token: 0x06004E15 RID: 19989 RVA: 0x00148D34 File Offset: 0x00147134
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x1700127C RID: 4732
	// (get) Token: 0x06004E16 RID: 19990 RVA: 0x00148D3D File Offset: 0x0014713D
	// (set) Token: 0x06004E17 RID: 19991 RVA: 0x00148D45 File Offset: 0x00147145
	[Inject]
	public IInputBlocker inputBlocker { get; set; }

	// Token: 0x1700127D RID: 4733
	// (get) Token: 0x06004E18 RID: 19992 RVA: 0x00148D4E File Offset: 0x0014714E
	// (set) Token: 0x06004E19 RID: 19993 RVA: 0x00148D56 File Offset: 0x00147156
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x1700127E RID: 4734
	// (get) Token: 0x06004E1A RID: 19994 RVA: 0x00148D5F File Offset: 0x0014715F
	// (set) Token: 0x06004E1B RID: 19995 RVA: 0x00148D67 File Offset: 0x00147167
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x1700127F RID: 4735
	// (get) Token: 0x06004E1C RID: 19996 RVA: 0x00148D70 File Offset: 0x00147170
	// (set) Token: 0x06004E1D RID: 19997 RVA: 0x00148D78 File Offset: 0x00147178
	[Inject]
	public PlayerInputLocator playerInput { get; set; }

	// Token: 0x17001280 RID: 4736
	// (get) Token: 0x06004E1E RID: 19998 RVA: 0x00148D81 File Offset: 0x00147181
	// (set) Token: 0x06004E1F RID: 19999 RVA: 0x00148D89 File Offset: 0x00147189
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17001281 RID: 4737
	// (get) Token: 0x06004E20 RID: 20000 RVA: 0x00148D92 File Offset: 0x00147192
	// (set) Token: 0x06004E21 RID: 20001 RVA: 0x00148D9A File Offset: 0x0014719A
	[Inject]
	public IScreenPermissions screenPermissions { get; set; }

	// Token: 0x17001282 RID: 4738
	// (get) Token: 0x06004E22 RID: 20002 RVA: 0x00148DA3 File Offset: 0x001471A3
	// (set) Token: 0x06004E23 RID: 20003 RVA: 0x00148DAB File Offset: 0x001471AB
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17001283 RID: 4739
	// (get) Token: 0x06004E24 RID: 20004 RVA: 0x00148DB4 File Offset: 0x001471B4
	// (set) Token: 0x06004E25 RID: 20005 RVA: 0x00148DBC File Offset: 0x001471BC
	[Inject]
	public ICustomLobbyController lobbyController { get; set; }

	// Token: 0x17001284 RID: 4740
	// (get) Token: 0x06004E26 RID: 20006 RVA: 0x00148DC5 File Offset: 0x001471C5
	// (set) Token: 0x06004E27 RID: 20007 RVA: 0x00148DCD File Offset: 0x001471CD
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x17001285 RID: 4741
	// (get) Token: 0x06004E28 RID: 20008 RVA: 0x00148DD6 File Offset: 0x001471D6
	// (set) Token: 0x06004E29 RID: 20009 RVA: 0x00148DDE File Offset: 0x001471DE
	[Inject]
	public IPreviousCrashDetector previousCrashDetector { get; set; }

	// Token: 0x17001286 RID: 4742
	// (get) Token: 0x06004E2A RID: 20010 RVA: 0x00148DE7 File Offset: 0x001471E7
	// (set) Token: 0x06004E2B RID: 20011 RVA: 0x00148DEF File Offset: 0x001471EF
	[Inject]
	public ISoundFileManager soundFileManager { get; set; }

	// Token: 0x17001287 RID: 4743
	// (get) Token: 0x06004E2C RID: 20012 RVA: 0x00148DF8 File Offset: 0x001471F8
	// (set) Token: 0x06004E2D RID: 20013 RVA: 0x00148E00 File Offset: 0x00147200
	[Inject]
	public ISceneController sceneController { get; set; }

	// Token: 0x17001288 RID: 4744
	// (get) Token: 0x06004E2E RID: 20014 RVA: 0x00148E09 File Offset: 0x00147209
	public ScreenType CurrentScreen
	{
		get
		{
			return this.currentScreenType;
		}
	}

	// Token: 0x17001289 RID: 4745
	// (get) Token: 0x06004E2F RID: 20015 RVA: 0x00148E11 File Offset: 0x00147211
	public ScreenType PreviousScreen
	{
		get
		{
			return this.previousScreenType;
		}
	}

	// Token: 0x1700128A RID: 4746
	// (get) Token: 0x06004E30 RID: 20016 RVA: 0x00148E19 File Offset: 0x00147219
	public ScreenType LoadingScreen
	{
		get
		{
			return this.loadingScreenType;
		}
	}

	// Token: 0x06004E31 RID: 20017 RVA: 0x00148E24 File Offset: 0x00147224
	public void Initialize(IEvents events, IScreenDisplay display)
	{
		this.display = display;
		this.events = events;
		this.signalBus.AddListener(UIManager.SCREEN_CLOSED, new Action(this.syncSceneDisplay));
		events.Subscribe(typeof(LoadScreenCommand), new Events.EventHandler(this.onLoadScreenCommand));
		events.Subscribe(typeof(DisconnectUpdate), new Events.EventHandler(this.onDisconnect));
	}

	// Token: 0x06004E32 RID: 20018 RVA: 0x00148E94 File Offset: 0x00147294
	private void onDisconnect(GameEvent message)
	{
		DisconnectUpdate disconnectUpdate = message as DisconnectUpdate;
		if (this.currentScreenType == ScreenType.BattleGUI)
		{
			if (this.gameController.currentGame != null && !this.gameController.currentGame.EndedGame)
			{
				this.gameController.currentGame.ForfeitGame(ScreenType.MainMenu);
			}
		}
		else
		{
			this.events.Broadcast(new LeaveRoomCommand());
			this.attemptLoadScreen((!this.lobbyController.IsInLobby) ? ScreenType.MainMenu : ScreenType.CustomLobbyScreen, this.payload, ScreenUpdateType.Previous);
		}
		if (this.dialog == null)
		{
			this.netErrorCode = disconnectUpdate.errorCode;
			this.dialog = this.dialogController.ShowOneButtonDialog(this.localization.GetText("dialog.OnlineGame.Error.title"), this.localization.GetText(NetErrorCodeUtil.GetErrorCodeString(disconnectUpdate.errorCode)), this.localization.GetText("dialog.exit"), WindowTransition.STANDARD_FADE, false, default(AudioData));
			this.dialog.CloseCallback = delegate()
			{
				if (this.dialog != null)
				{
					this.dialog.Close();
				}
				this.dialog = null;
				this.netErrorCode = NetErrorCode.None;
			};
		}
		else if (NetErrorCodeUtil.ShouldReplaceError(this.netErrorCode, disconnectUpdate.errorCode))
		{
			this.netErrorCode = disconnectUpdate.errorCode;
			this.dialog.SetBody(this.localization.GetText(NetErrorCodeUtil.GetErrorCodeString(disconnectUpdate.errorCode)));
		}
	}

	// Token: 0x06004E33 RID: 20019 RVA: 0x00148FF7 File Offset: 0x001473F7
	public bool AtOrGoingTo(ScreenType screen)
	{
		return this.loadingScreenType == screen || this.currentScreenType == screen;
	}

	// Token: 0x06004E34 RID: 20020 RVA: 0x00149014 File Offset: 0x00147414
	private void onLoadScreenCommand(GameEvent message)
	{
		LoadScreenCommand loadScreenCommand = message as LoadScreenCommand;
		this.attemptLoadScreen(loadScreenCommand.type, loadScreenCommand.payload, loadScreenCommand.updateType);
	}

	// Token: 0x06004E35 RID: 20021 RVA: 0x00149040 File Offset: 0x00147440
	private void attemptLoadScreen(ScreenType type, Payload payload, ScreenUpdateType updateType)
	{
		type = this.screenPermissions.GetRedirect(type);
		if (this.currentScreen != null)
		{
			this.playTransitionSound(type, updateType);
		}
		this.timer.CancelTimeout(new Action(this.loadScreenAfterMusicEnd));
		this.pendingRequest = null;
		if (type == ScreenType.StoreScreen)
		{
			this.pendingRequest = new UIScreenAdapter.Request();
			this.pendingRequest.type = type;
			this.pendingRequest.payload = payload;
			float num = 0.35f;
			this.timer.SetTimeout((int)(num * 1000f), new Action(this.loadScreenAfterMusicEnd));
			this.audioManager.StopMusic(null, num);
		}
		else
		{
			this.goToScreen(type, payload);
		}
	}

	// Token: 0x06004E36 RID: 20022 RVA: 0x001490FC File Offset: 0x001474FC
	private void loadScreenAfterMusicEnd()
	{
		if (this.pendingRequest != null)
		{
			this.goToScreen(this.pendingRequest.type, this.pendingRequest.payload);
		}
	}

	// Token: 0x06004E37 RID: 20023 RVA: 0x00149128 File Offset: 0x00147528
	private void playTransitionSound(ScreenType type, ScreenUpdateType updateType)
	{
		AudioData sound = AudioData.Empty;
		if (updateType == ScreenUpdateType.Previous)
		{
			sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.generic_screenBackTransition);
		}
		else if (updateType == ScreenUpdateType.Next)
		{
			switch (type)
			{
			case ScreenType.SelectStage:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.stageSelect_stageSelectScreenOpen);
				goto IL_125;
			case ScreenType.LoadingBattle:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.loadingBattle_loadingBattleScreenOpen);
				goto IL_125;
			case ScreenType.CharacterSelect:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.characterSelect_characterSelectScreenOpen);
				goto IL_125;
			case ScreenType.SettingsScreen:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.settings_settingsScreenOpen);
				goto IL_125;
			case ScreenType.VictoryGUI:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.victoryScreen_victoryScreenOpen);
				goto IL_125;
			case ScreenType.CustomLobbyScreen:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.customLobby_customLobbyScreenOpen);
				goto IL_125;
			case ScreenType.StoreScreen:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.store_storeScreenOpen);
				goto IL_125;
			case ScreenType.OnlineBlindPick:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.characterSelect_onlineBlindPickScreenOpen);
				goto IL_125;
			case ScreenType.PlayerProgression:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.playerProgression_playerProgressionScreenOpen);
				goto IL_125;
			}
			sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.generic_screenForwardTransition);
		}
		IL_125:
		this.audioManager.PlayMenuSound(sound, 0f);
	}

	// Token: 0x06004E38 RID: 20024 RVA: 0x0014926C File Offset: 0x0014766C
	public void PreloadScreen(ScreenType type)
	{
		if (this.currentScreenType == type || this.loadingScreenType == type)
		{
			return;
		}
		GameScreen screenByType = this.display.GetScreenByType(type);
		this.createScreen(screenByType, type, null);
		this.display.ShowScreen(screenByType, null, null, true);
	}

	// Token: 0x06004E39 RID: 20025 RVA: 0x001492B7 File Offset: 0x001476B7
	public void ShowPreloaded()
	{
		this.events.Broadcast(new AllPlayersReadyMessage());
	}

	// Token: 0x06004E3A RID: 20026 RVA: 0x001492C9 File Offset: 0x001476C9
	public void OnGameSynced()
	{
		this.display.ClearPreviousScreen();
	}

	// Token: 0x06004E3B RID: 20027 RVA: 0x001492D8 File Offset: 0x001476D8
	private void goToScreen(ScreenType type, Payload payload)
	{
		if (this.currentScreenType == type || this.loadingScreenType == type)
		{
			return;
		}
		GameScreen prefab = this.display.GetScreenByType(type);
		InputBlock block = this.inputBlocker.Request();
		this.loadingScreenType = type;
		this.incrementLoadingAttemptId();
		int thisAttemptId = this.loadingAttemptId;
		foreach (UILoadBindings.SoundBundleDef soundBundleDef in this.loadBindings.GetSoundBindings(prefab.GetType()))
		{
			this.soundFileManager.PreloadBundle(soundBundleDef.key, soundBundleDef.preloadIndividualSounds);
		}
		this.dataLoader.Load(this.loadBindings.GetBindings(prefab.GetType()), delegate(LoadSequenceResults data)
		{
			this.inputBlocker.Release(block);
			if (this.loadingAttemptId == thisAttemptId)
			{
				this.incrementLoadingAttemptId();
				this.onDataLoadComplete(prefab, data, type, payload);
			}
		});
	}

	// Token: 0x06004E3C RID: 20028 RVA: 0x001493DC File Offset: 0x001477DC
	private void incrementLoadingAttemptId()
	{
		this.loadingAttemptId++;
		if (this.loadingAttemptId > 2147483647)
		{
			this.loadingAttemptId = 0;
		}
	}

	// Token: 0x06004E3D RID: 20029 RVA: 0x00149404 File Offset: 0x00147804
	private void onDataLoadComplete(GameScreen prefab, LoadSequenceResults data, ScreenType type, Payload payload)
	{
		bool ignoreDataDependencies = this.gameDataManager.ConfigData.uiConfig.ignoreDataDependencies;
		if (data.status != DataLoadStatus.SUCCESS && !ignoreDataDependencies)
		{
			this.loadingScreenType = ScreenType.None;
			this.dialogController.ShowOneButtonDialog("TEMP MESSAGE", this.localization.GetText("ui.screen.loadFailed"), "OK", WindowTransition.STANDARD_FADE, false, default(AudioData));
		}
		else
		{
			this.incrementLoadingAttemptId();
			int thisAttemptId = this.loadingAttemptId;
			InputBlock block = this.inputBlocker.Request();
			this.sceneController.PreloadUIScene(type, delegate
			{
				this.inputBlocker.Release(block);
				if (this.loadingAttemptId == thisAttemptId)
				{
					this.loadingScreenType = ScreenType.None;
					this.incrementLoadingAttemptId();
					this.postLoadCreateScreen(prefab, data, type, payload);
				}
			});
		}
	}

	// Token: 0x06004E3E RID: 20030 RVA: 0x001494F4 File Offset: 0x001478F4
	private void postLoadCreateScreen(GameScreen prefab, LoadSequenceResults data, ScreenType type, Payload payload)
	{
		this.createScreen(prefab, type, payload);
		this.playerInput.Input.ConsumeButtonPresses();
		Payload payload2 = null;
		if (payload != null)
		{
			payload2 = (payload.Clone() as Payload);
		}
		this.display.ShowScreen(prefab, payload2, data, false);
	}

	// Token: 0x06004E3F RID: 20031 RVA: 0x00149540 File Offset: 0x00147940
	private void createScreen(GameScreen prefab, ScreenType type, Payload payload)
	{
		foreach (Type messageType in this.controllerSubscribedTypes)
		{
			this.events.Unsubscribe(messageType, new Events.EventHandler(this.onLocalEvent));
		}
		this.controllerSubscribedTypes.Clear();
		if (this.controller != null)
		{
			this.controller.Unregister();
		}
		this.previousScreenType = this.currentScreenType;
		this.currentScreenType = type;
		if (this.gameController.currentGame == null)
		{
			this.previousCrashDetector.UpdateStatus(PreviousCrashDetector.GameStatus.Menu);
		}
		if (this.previousScreenType == ScreenType.None)
		{
			this.syncSceneDisplay();
		}
		this.controller = this.injector.GetInstance<UIScreenController>(type);
		if (this.controller != null)
		{
			this.controller.Register(this);
			this.controller.payload = payload;
			this.controller.Initialize();
		}
		this.payload = payload;
		this.signalBus.Dispatch(UIScreenAdapter.SCREEN_CHANGED);
	}

	// Token: 0x06004E40 RID: 20032 RVA: 0x00149674 File Offset: 0x00147A74
	private void syncSceneDisplay()
	{
		this.sceneController.ActivateUIScene(this.currentScreenType);
	}

	// Token: 0x06004E41 RID: 20033 RVA: 0x00149688 File Offset: 0x00147A88
	public void OnScreenCreated(GameScreen screen)
	{
		this.currentScreen = screen;
		Payload payload = null;
		if (this.payload != null)
		{
			payload = (this.payload.Clone() as Payload);
		}
		this.currentScreen.LoadPayload(payload);
	}

	// Token: 0x06004E42 RID: 20034 RVA: 0x001496C6 File Offset: 0x00147AC6
	public void Subscribe(Type type)
	{
		this.controllerSubscribedTypes.Add(type);
		this.events.Subscribe(type, new Events.EventHandler(this.onLocalEvent));
	}

	// Token: 0x06004E43 RID: 20035 RVA: 0x001496EC File Offset: 0x00147AEC
	public void SendUpdate(GameEvent message)
	{
		this.handleUIUpdate(message);
	}

	// Token: 0x06004E44 RID: 20036 RVA: 0x001496F8 File Offset: 0x00147AF8
	private void handleUIUpdate(GameEvent update)
	{
		if (update is UIPayloadUpdate && this.currentScreen != null)
		{
			Payload payload = null;
			if (this.payload != null)
			{
				payload = (this.payload.Clone() as Payload);
			}
			this.currentScreen.UpdatePayload(payload);
		}
	}

	// Token: 0x06004E45 RID: 20037 RVA: 0x0014974C File Offset: 0x00147B4C
	protected void onLocalEvent(GameEvent message)
	{
		Type type = message.GetType();
		this.controller.HandleRequest(message);
	}

	// Token: 0x06004E46 RID: 20038 RVA: 0x0014976C File Offset: 0x00147B6C
	public T GetUIScene<T>() where T : UIScene
	{
		return this.sceneController.GetUIScene<T>();
	}

	// Token: 0x0400330B RID: 13067
	public static string SCREEN_CHANGED = "UIScreenAdapter.SCREEN_CHANGED";

	// Token: 0x0400331D RID: 13085
	protected IEvents events;

	// Token: 0x0400331E RID: 13086
	private IScreenDisplay display;

	// Token: 0x0400331F RID: 13087
	private ScreenType currentScreenType = ScreenType.None;

	// Token: 0x04003320 RID: 13088
	private ScreenType previousScreenType = ScreenType.None;

	// Token: 0x04003321 RID: 13089
	private GameScreen currentScreen;

	// Token: 0x04003322 RID: 13090
	private int loadingAttemptId;

	// Token: 0x04003323 RID: 13091
	private ScreenType loadingScreenType = ScreenType.None;

	// Token: 0x04003324 RID: 13092
	private Payload payload;

	// Token: 0x04003325 RID: 13093
	protected UIScreenController controller;

	// Token: 0x04003326 RID: 13094
	private List<Type> controllerSubscribedTypes = new List<Type>();

	// Token: 0x04003327 RID: 13095
	private GenericDialog dialog;

	// Token: 0x04003328 RID: 13096
	private NetErrorCode netErrorCode;

	// Token: 0x04003329 RID: 13097
	private UIScreenAdapter.Request pendingRequest;

	// Token: 0x02000A76 RID: 2678
	private class Request
	{
		// Token: 0x0400332A RID: 13098
		public ScreenType type;

		// Token: 0x0400332B RID: 13099
		public Payload payload;
	}
}
