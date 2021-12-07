// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using IconsServer;
using RollbackDebug;
using strange.extensions.context.impl;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class GameClient : ContextView, ITickable, ICoroutineStarter, IGameClient
{
	private sealed class _endGame_c__AnonStorey1
	{
		internal VictoryScreenPayload payload;

		internal GameClient _this;
	}

	private sealed class _endGame_c__AnonStorey0
	{
		internal bool isNetworkGame;

		internal Guid matchID;

		internal GameClient._endGame_c__AnonStorey1 __f__ref_1;

		internal void __m__0()
		{
			string text = this.__f__ref_1._this.config.replaySettings.replayName;
			if (this.isNetworkGame)
			{
				text = string.Format("{0}-{1}-{2}", text, this.matchID, this.__f__ref_1._this.iconsServerAPI.Username);
			}
			this.__f__ref_1._this.replaySystem.OnGameFinished(this.__f__ref_1._this.frame, this.__f__ref_1.payload, text);
			this.__f__ref_1._this.Events.Broadcast(new TransitionToVictoryPoseCommand(this.__f__ref_1.payload));
		}
	}

	public static Vector2 NATIVE_RESOLUTION = new Vector2(1920f, 1080f);

	private GameObject toolContainer;

	private bool isStarted;

	private static GameClient instance;

	private static global::ILogger Logger;

	[Inject]
	public IDependencyInjection injector
	{
		private get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		private get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		private get;
		set;
	}

	[Inject]
	public OptionsProfileAPI optionsProfileAPI
	{
		private get;
		set;
	}

	[Inject]
	public IEvents Events
	{
		private get;
		set;
	}

	[Inject]
	public UIManager UI
	{
		get;
		set;
	}

	[Inject]
	public ISceneController sceneController
	{
		private get;
		set;
	}

	[Inject]
	public UserAudioSettings userAudioSettings
	{
		private get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		private get;
		set;
	}

	[Inject]
	public IDialogController dialogController
	{
		private get;
		set;
	}

	[Inject]
	public IGamewideOverlayController gamewideOverlayController
	{
		private get;
		set;
	}

	[Inject]
	public DeveloperConfig devConfig
	{
		private get;
		set;
	}

	[Inject]
	public ScreenshotModel screenshotModel
	{
		private get;
		set;
	}

	[Inject]
	public NetGraphVisualizer netGraphVisualizer
	{
		private get;
		set;
	}

	[Inject]
	public IServerConnectionManager serverManager
	{
		private get;
		set;
	}

	[Inject]
	public IIconsServerAPI iconsServerAPI
	{
		private get;
		set;
	}

	[Inject]
	public IOfflineModeDetector offlineMode
	{
		private get;
		set;
	}

	[Inject]
	public ConnectionController connectionController
	{
		private get;
		set;
	}

	[Inject]
	public ITimeStatTrackerManager timeStatTrackerManager
	{
		private get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServerAPI
	{
		private get;
		set;
	}

	[Inject]
	public PlayerInputLocator playerInput
	{
		private get;
		set;
	}

	[Inject]
	public IPerformanceTracker performanceTracker
	{
		private get;
		set;
	}

	[Inject]
	public IEnterNewGame enterNewGame
	{
		private get;
		set;
	}

	[Inject]
	public AnnouncementHelper announcements
	{
		private get;
		set;
	}

	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel
	{
		private get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		private get;
		set;
	}

	[Inject]
	public GameEnvironmentData environmentData
	{
		private get;
		set;
	}

	[Inject]
	public IReplaySystem replaySystem
	{
		private get;
		set;
	}

	[Inject]
	public IGetEquippedItemsFromServer getEquippedItems
	{
		get;
		set;
	}

	[Inject]
	public IOnlineSubsystem onlineSubsystem
	{
		get;
		set;
	}

	[Inject]
	public ICustomLobbyController customLobby
	{
		get;
		set;
	}

	public GameDataManager GameData
	{
		get;
		private set;
	}

	public AudioManager Audio
	{
		get;
		private set;
	}

	public UserAudioSettings UserAudioSettings
	{
		get;
		private set;
	}

	public PlayerInputManager Input
	{
		get;
		private set;
	}

	public FPSCounter DisplayFPSCounter
	{
		get;
		private set;
	}

	public FPSCounter GameTickFPSCounter
	{
		get;
		private set;
	}

	public IUserInputManager UserInputManager
	{
		get;
		private set;
	}

	public UserFacingPerformanceDisplay PerformanceDisplay
	{
		get;
		private set;
	}

	private global::ILogger logger
	{
		get;
		set;
	}

	private GameManager gameManager
	{
		get
		{
			return (this.gameController != null) ? this.gameController.currentGame : null;
		}
	}

	private int frame
	{
		get
		{
			return (!(this.gameManager != null)) ? 0 : this.gameManager.Frame;
		}
	}

	public static bool IsCreated
	{
		get;
		private set;
	}

	public static bool IsRollingBack
	{
		get
		{
			return !(GameClient.instance == null) && !(GameClient.instance.gameManager == null) && GameClient.instance.gameManager.IsRollingBack;
		}
	}

	public static bool IsCurrentFrame
	{
		get
		{
			return GameClient.instance == null || GameClient.instance.gameManager == null || GameClient.instance.gameManager.FrameController == null || (GameClient.instance.gameManager.FrameController.IsCurrentFrame && !GameClient.instance.gameManager.IsRollingBack);
		}
	}

	private bool useGameplayTick
	{
		get
		{
			return this.gameController.MatchIsRunning;
		}
	}

	private void Awake()
	{
		if (GameClient.instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			GameClient.instance = this;
			GameClient.IsCreated = true;
		}
	}

	public void Init()
	{
		Screen.sleepTimeout = -1;
		TimeUtil.Init();
		StringUtil.Init();
		this.logger = this.injector.GetInstance<global::ILogger>();
		BroadcastingLogger broadcastingLogger = this.logger as BroadcastingLogger;
		broadcastingLogger.AttachLogger(new UnityLogger());
		this.logger.LogLevel = LogLevel.Debug;
		GameClient.Logger = this.logger;
		this.GameData = this.injector.GetInstance<GameDataManager>();
		Application.logMessageReceived += new Application.LogCallback(this.handleUnityLog);
		DOTween.Init(new bool?(true), new bool?(true), new LogBehaviour?(LogBehaviour.ErrorsOnly));
		this.injector.GetInstance<IBackgroundLoader>().LoadBakedAnimations(this);
		this.injector.GetInstance<IRollbackStatePooling>().Init();
		this.startupListeningClasses();
		this.dialogController.Init(this);
		this.gamewideOverlayController.Init(this);
		if (BuildConfig.environmentType == BuildEnvironment.Live || BuildConfig.environmentType == BuildEnvironment.ReleaseCandidate)
		{
			this.dialogController.ShowOneButtonDialog("DEBUG ERROR", "This is a Release build that is incorrectly Debug!", "OK", WindowTransition.STANDARD_FADE, false, default(AudioData));
		}
		this.userVideoSettingsModel.Init();
		this.injector.GetInstance<IUserGameplaySettingsModel>().Init();
		this.localization.SetRegion(LocalizationRegion.en_US);
		foreach (LocalizationData current in this.config.uiLocalizationData.localizationData)
		{
			this.localization.AddLocalizationData(current);
		}
		this.injector.GetInstance<IUIComponentLocator>().Load();
		this.toolContainer = new GameObject("Tools");
		this.toolContainer.transform.SetParent(base.transform);
		broadcastingLogger.AttachLogger(this.injector.GetInstance<ConsoleLogger>());
		this.injector.GetInstance<IConnectionStatusHandler>().Startup();
		this.injector.GetInstance<EventLogger>().Init();
		this.Events.Subscribe(typeof(SetupGameCommand), new Events.EventHandler(this.startGame));
		this.signalBus.GetSignal<EndGameSignal>().AddListener(new Action<VictoryScreenPayload>(this.endGame));
		this.Events.Subscribe(typeof(CreateGameManagerCommand), new Events.EventHandler(this.createGameManager));
		this.Audio = this.injector.GetInstance<AudioManager>();
		this.Audio.Init(base.transform);
		this.Input = new GameObject("GameInput").AddComponent<PlayerInputManager>();
		this.Input.Init(this.config);
		this.Input.transform.SetParent(base.transform);
		this.playerInput.Set(this.Input);
		this.UserInputManager = this.injector.GetInstance<IUserInputManager>();
		this.UserInputManager.Init(this.config);
		this.UserInputManager.BindWithoutDeviceToAvailableUser(this.Input.GetFirstPortWithNoUser());
		this.UI.Init(this.config.uiConfig, this);
		this.sceneController.Init();
		this.Input.transform.SetParent(this.toolContainer.transform, false);
		this.DisplayFPSCounter = new GameObject("FPS").AddComponent<FPSCounter>();
		this.DisplayFPSCounter.transform.SetParent(this.toolContainer.transform, false);
		this.DisplayFPSCounter.RecordOnUpdate = true;
		this.GameTickFPSCounter = new GameObject("GameTickFPS").AddComponent<FPSCounter>();
		this.GameTickFPSCounter.transform.SetParent(this.toolContainer.transform, false);
		this.GameTickFPSCounter.RecordOnUpdate = false;
		this.PerformanceDisplay = this.gamewideOverlayController.ShowOverlay<UserFacingPerformanceDisplay>(WindowTransition.STANDARD_FADE);
		this.PerformanceDisplay.DisplayFPS = this.DisplayFPSCounter;
		this.PerformanceDisplay.GameTickFPS = this.GameTickFPSCounter;
		this.gamewideOverlayController.ShowOverlay<UserFacingSystemClock>(WindowTransition.STANDARD_FADE);
		if (this.GameData.IsFeatureEnabled(FeatureID.Watermark))
		{
			this.gamewideOverlayController.ShowOverlay<FutureWatermarkDisplay>(WindowTransition.STANDARD_FADE);
		}
		if (this.GameData.IsFeatureEnabled(FeatureID.PlayerCard))
		{
			this.gamewideOverlayController.ShowOverlay<PlayerCard>(WindowTransition.STANDARD_FADE);
		}
		this.replaySystem.Init(this.config.replaySettings);
		this.injector.GetInstance<ISoundFileManager>().PreloadBundle(SoundBundleKey.generic, true);
		this.injector.GetInstance<IStartupArgs>().DebugLogArgs();
		this.announcements.Init();
		if (this.config.fps > 0)
		{
			Time.timeScale = this.config.gameSpeed;
		}
	}

	public void InitDebug()
	{
		this.injector.GetInstance<IRollbackStateCodeValidator>().Validate();
		this.injector.GetInstance<DeveloperUtilityConsoleCommands>().Init();
		this.injector.GetInstance<DebugKeys>().Init();
		this.netGraphVisualizer.Init();
		this.timeStatTrackerManager.InitDebug();
		RollbackDebugValidation.instance.Initialize(this.devConfig.enableRollbackDebugValidation);
		RollbackDebugValidation.instance.StartupValidation();
		if (this.config.replaySettings.testReplayEquality)
		{
			string empty = string.Empty;
			if (!this.replaySystem.TestReplayEquality(this.config.replaySettings.replayName, this.config.replaySettings.testReplayFilePath, ref empty))
			{
				UnityEngine.Debug.LogError(string.Concat(new string[]
				{
					"ERROR: Replays ",
					this.config.replaySettings.replayName,
					" and ",
					this.config.replaySettings.testReplayFilePath,
					" are not equal:\n",
					empty
				}));
			}
			else
			{
				UnityEngine.Debug.LogWarning(string.Concat(new string[]
				{
					" Replays ",
					this.config.replaySettings.replayName,
					" and ",
					this.config.replaySettings.testReplayFilePath,
					" are equal"
				}));
			}
		}
		if (!this.environmentData.toggles.GetToggle(FeatureID.LocalStore))
		{
			StringBuilder error = new StringBuilder();
			if (!StaticDataValidator.ValidateLocalStoreDb(error))
			{
			}
		}
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.HitBoxes, false);
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.HurtBoxes, false);
		DebugDraw.Instance.Events = this.Events;
		this.injector.GetInstance<IDevConsole>().RunAutoExec();
		ServerUtilityConsoleCommands serverUtilityConsoleCommands = new ServerUtilityConsoleCommands();
		this.injector.Inject(serverUtilityConsoleCommands);
		serverUtilityConsoleCommands.Init();
	}

	private void startupListeningClasses()
	{
		this.injector.GetInstance<DebugKeys>();
		this.injector.GetInstance<IEnterNewGame>();
		this.injector.GetInstance<EnterCustomLobbyController>();
		this.injector.GetInstance<IUserCurrencyReceiver>();
		this.injector.GetInstance<IApplicationFramerateManager>();
	}

	public void StartupComplete()
	{
		this.isStarted = true;
		SystemBoot.Mode mode = SystemBoot.mode;
		this.iconsServerAPI.Startup(10);
		this.onlineSubsystem.Startup();
		this.battleServerAPI.Initialize();
		this.customLobby.Initialize();
		if (mode == SystemBoot.Mode.StagePreview)
		{
			this.enterNewGame.StartPreviewGame(false);
		}
		else if (mode == SystemBoot.Mode.VictoryPosePreview)
		{
			this.enterNewGame.StartPreviewGame(true);
			this.Events.Broadcast(new LoadScreenCommand(ScreenType.BattleGUI, null, ScreenUpdateType.Next));
			this.gameManager.EndPreviewGame();
		}
		else if (mode != SystemBoot.Mode.NoGame)
		{
			if (this.config.IsAutoStart)
			{
				this.enterNewGame.StartTestGame();
			}
			else if (this.config.uiuxSettings.DemoModeEnabled)
			{
				this.UserInputManager.ResetPlayerMapping();
				this.optionsProfileAPI.SetDefaultGameMode(GameMode.FreeForAll);
				this.optionsProfileAPI.DeleteDefaultSettings(new Action<SaveOptionsProfileResult>(this._StartupComplete_m__0));
			}
			else if (!this.offlineMode.IsOfflineMode())
			{
				this.Events.Broadcast(new LoadScreenCommand(ScreenType.LoginScreen, null, ScreenUpdateType.Next));
			}
			else
			{
				this.Events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, null, ScreenUpdateType.Next));
			}
		}
	}

	private void OnApplicationQuit()
	{
		if (this.injector != null)
		{
			this.injector.GetInstance<IBackgroundLoader>().OnApplicationQuit();
		}
		if (this.serverManager.IsConnectedToNexus)
		{
			float num = Time.realtimeSinceStartup + 1f;
			while (Time.realtimeSinceStartup < num)
			{
			}
		}
		this.connectionController.OnDestroy();
		this.iconsServerAPI.Shutdown();
		Application.logMessageReceived -= new Application.LogCallback(this.handleUnityLog);
	}

	private TeamNum GetTeamNumFromTest(TestGameTeamNum testTeamNum, int index)
	{
		TeamNum result = TeamNum.Team1;
		if (testTeamNum == TestGameTeamNum.Auto)
		{
			result = ((index % 2 != 0) ? TeamNum.Team2 : TeamNum.Team1);
		}
		else if (testTeamNum == TestGameTeamNum.Team2)
		{
			result = TeamNum.Team2;
		}
		return result;
	}

	private void Update()
	{
		if (this.isStarted)
		{
			this.tickOncePerRenderFrame();
		}
	}

	protected void FixedUpdate()
	{
		if (this.isStarted)
		{
			this.TickFrame();
		}
	}

	private void tickOncePerRenderFrame()
	{
		if (this.gameManager == null)
		{
			this.serverManager.ReceiveAllMessages();
		}
		this.netGraphVisualizer.ReportFPS(this.GameTickFPSCounter.FPS);
		this.userVideoSettingsModel.Update();
		if (!this.useGameplayTick)
		{
			this.tickBaseSystems();
		}
		if (this.gameManager == null)
		{
			this.serverManager.SendAllMessages();
		}
	}

	public void TickFrame()
	{
		if (this.useGameplayTick)
		{
			this.tickBaseSystems();
		}
		if (this.gameManager == null)
		{
			if (this.announcements != null)
			{
				this.announcements.TickFrame();
			}
			this.Audio.TickFrame();
		}
		this.performanceTracker.RecordFPS(this.DisplayFPSCounter.FPS, this.GameTickFPSCounter.FPS);
		this.performanceTracker.TickFrame();
		this.timeStatTrackerManager.TickFrame();
		this.netGraphVisualizer.TickFrame();
	}

	private void tickBaseSystems()
	{
		if (this.Input != null)
		{
			this.Input.TickFrame();
		}
		if (this.UI != null)
		{
			this.UI.TickFrame();
		}
		if (this.sceneController != null)
		{
			this.sceneController.TickFrame();
		}
	}

	private void createGameManager(GameEvent message)
	{
		this.gameController.SetCurrentGame(this.injector.CreateComponentWithGameObject<GameManager>("GameManager"));
		this.gameManager.Initialize(this);
		this.gameController.preloader = new GameAssetPreloader();
		this.injector.Inject(this.gameController.preloader);
	}

	private void startGame(GameEvent message)
	{
		this.gameController.EndPreload();
		SetupGameCommand setupGameCommand = message as SetupGameCommand;
		GameLoadPayload payload = setupGameCommand.payload;
		this.replaySystem.LoadSettings(this.config.replaySettings);
		if (this.replaySystem.IsReplaying && this.replaySystem.IsDirty)
		{
			this.replaySystem.LoadFromFile(this.config.replaySettings.replayName);
		}
		this.replaySystem.OnGameStarted(ref payload);
		this.gameManager.StartGame(payload);
	}

	public void PreCompleteMatch()
	{
	}

	private void endGame(VictoryScreenPayload payload)
	{
		GameClient._endGame_c__AnonStorey1 _endGame_c__AnonStorey = new GameClient._endGame_c__AnonStorey1();
		_endGame_c__AnonStorey.payload = payload;
		_endGame_c__AnonStorey._this = this;
		if (this.gameManager != null && !this.screenshotModel.InProgress)
		{
			GameClient._endGame_c__AnonStorey0 _endGame_c__AnonStorey2 = new GameClient._endGame_c__AnonStorey0();
			_endGame_c__AnonStorey2.__f__ref_1 = _endGame_c__AnonStorey;
			_endGame_c__AnonStorey2.isNetworkGame = this.battleServerAPI.IsConnected;
			_endGame_c__AnonStorey2.matchID = this.battleServerAPI.MatchID;
			if (_endGame_c__AnonStorey.payload.wasForfeited && _endGame_c__AnonStorey.payload.gamePayload.isOnlineGame)
			{
				this.Events.Broadcast(new LeaveRoomCommand());
			}
			this.screenshotModel.SaveScreenshot(new Action(_endGame_c__AnonStorey2.__m__0));
		}
	}

	public static void Log(LogLevel logLevel, params object[] parameters)
	{
		GameClient.Logger.Log(logLevel, parameters);
	}

	private void handleUnityLog(string logString, string stackTrace, LogType logType)
	{
		BroadcastingLogger broadcastingLogger = GameClient.Logger as BroadcastingLogger;
		broadcastingLogger.LogUnityLog(logString, stackTrace, logType);
	}

	private void _StartupComplete_m__0(SaveOptionsProfileResult result)
	{
		this.Events.Broadcast(new LoadScreenCommand(ScreenType.CharacterSelect, null, ScreenUpdateType.Next));
	}

	Coroutine ICoroutineStarter.StartCoroutine(IEnumerator routine)
	{
		return base.StartCoroutine(routine);
	}
}
