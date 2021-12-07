using System;
using System.Collections;
using System.Text;
using DG.Tweening;
using IconsServer;
using RollbackDebug;
using strange.extensions.context.impl;
using UnityEngine;

// Token: 0x02000466 RID: 1126
public class GameClient : ContextView, ITickable, ICoroutineStarter, IGameClient
{
	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x0600176C RID: 5996 RVA: 0x0007E265 File Offset: 0x0007C665
	// (set) Token: 0x0600176D RID: 5997 RVA: 0x0007E26D File Offset: 0x0007C66D
	[Inject]
	public IDependencyInjection injector { private get; set; }

	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x0600176E RID: 5998 RVA: 0x0007E276 File Offset: 0x0007C676
	// (set) Token: 0x0600176F RID: 5999 RVA: 0x0007E27E File Offset: 0x0007C67E
	[Inject]
	public ISignalBus signalBus { private get; set; }

	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x06001770 RID: 6000 RVA: 0x0007E287 File Offset: 0x0007C687
	// (set) Token: 0x06001771 RID: 6001 RVA: 0x0007E28F File Offset: 0x0007C68F
	[Inject]
	public GameController gameController { private get; set; }

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x06001772 RID: 6002 RVA: 0x0007E298 File Offset: 0x0007C698
	// (set) Token: 0x06001773 RID: 6003 RVA: 0x0007E2A0 File Offset: 0x0007C6A0
	[Inject]
	public OptionsProfileAPI optionsProfileAPI { private get; set; }

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x06001774 RID: 6004 RVA: 0x0007E2A9 File Offset: 0x0007C6A9
	// (set) Token: 0x06001775 RID: 6005 RVA: 0x0007E2B1 File Offset: 0x0007C6B1
	[Inject]
	public IEvents Events { private get; set; }

	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x06001776 RID: 6006 RVA: 0x0007E2BA File Offset: 0x0007C6BA
	// (set) Token: 0x06001777 RID: 6007 RVA: 0x0007E2C2 File Offset: 0x0007C6C2
	[Inject]
	public UIManager UI { get; set; }

	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x06001778 RID: 6008 RVA: 0x0007E2CB File Offset: 0x0007C6CB
	// (set) Token: 0x06001779 RID: 6009 RVA: 0x0007E2D3 File Offset: 0x0007C6D3
	[Inject]
	public ISceneController sceneController { private get; set; }

	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x0600177A RID: 6010 RVA: 0x0007E2DC File Offset: 0x0007C6DC
	// (set) Token: 0x0600177B RID: 6011 RVA: 0x0007E2E4 File Offset: 0x0007C6E4
	[Inject]
	public UserAudioSettings userAudioSettings { private get; set; }

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x0600177C RID: 6012 RVA: 0x0007E2ED File Offset: 0x0007C6ED
	// (set) Token: 0x0600177D RID: 6013 RVA: 0x0007E2F5 File Offset: 0x0007C6F5
	[Inject]
	public ILocalization localization { private get; set; }

	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x0600177E RID: 6014 RVA: 0x0007E2FE File Offset: 0x0007C6FE
	// (set) Token: 0x0600177F RID: 6015 RVA: 0x0007E306 File Offset: 0x0007C706
	[Inject]
	public IDialogController dialogController { private get; set; }

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x06001780 RID: 6016 RVA: 0x0007E30F File Offset: 0x0007C70F
	// (set) Token: 0x06001781 RID: 6017 RVA: 0x0007E317 File Offset: 0x0007C717
	[Inject]
	public IGamewideOverlayController gamewideOverlayController { private get; set; }

	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x06001782 RID: 6018 RVA: 0x0007E320 File Offset: 0x0007C720
	// (set) Token: 0x06001783 RID: 6019 RVA: 0x0007E328 File Offset: 0x0007C728
	[Inject]
	public DeveloperConfig devConfig { private get; set; }

	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x06001784 RID: 6020 RVA: 0x0007E331 File Offset: 0x0007C731
	// (set) Token: 0x06001785 RID: 6021 RVA: 0x0007E339 File Offset: 0x0007C739
	[Inject]
	public ScreenshotModel screenshotModel { private get; set; }

	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x06001786 RID: 6022 RVA: 0x0007E342 File Offset: 0x0007C742
	// (set) Token: 0x06001787 RID: 6023 RVA: 0x0007E34A File Offset: 0x0007C74A
	[Inject]
	public NetGraphVisualizer netGraphVisualizer { private get; set; }

	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x06001788 RID: 6024 RVA: 0x0007E353 File Offset: 0x0007C753
	// (set) Token: 0x06001789 RID: 6025 RVA: 0x0007E35B File Offset: 0x0007C75B
	[Inject]
	public IServerConnectionManager serverManager { private get; set; }

	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x0600178A RID: 6026 RVA: 0x0007E364 File Offset: 0x0007C764
	// (set) Token: 0x0600178B RID: 6027 RVA: 0x0007E36C File Offset: 0x0007C76C
	[Inject]
	public IIconsServerAPI iconsServerAPI { private get; set; }

	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x0600178C RID: 6028 RVA: 0x0007E375 File Offset: 0x0007C775
	// (set) Token: 0x0600178D RID: 6029 RVA: 0x0007E37D File Offset: 0x0007C77D
	[Inject]
	public IOfflineModeDetector offlineMode { private get; set; }

	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x0600178E RID: 6030 RVA: 0x0007E386 File Offset: 0x0007C786
	// (set) Token: 0x0600178F RID: 6031 RVA: 0x0007E38E File Offset: 0x0007C78E
	[Inject]
	public ConnectionController connectionController { private get; set; }

	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x06001790 RID: 6032 RVA: 0x0007E397 File Offset: 0x0007C797
	// (set) Token: 0x06001791 RID: 6033 RVA: 0x0007E39F File Offset: 0x0007C79F
	[Inject]
	public ITimeStatTrackerManager timeStatTrackerManager { private get; set; }

	// Token: 0x170004A5 RID: 1189
	// (get) Token: 0x06001792 RID: 6034 RVA: 0x0007E3A8 File Offset: 0x0007C7A8
	// (set) Token: 0x06001793 RID: 6035 RVA: 0x0007E3B0 File Offset: 0x0007C7B0
	[Inject]
	public IBattleServerAPI battleServerAPI { private get; set; }

	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x06001794 RID: 6036 RVA: 0x0007E3B9 File Offset: 0x0007C7B9
	// (set) Token: 0x06001795 RID: 6037 RVA: 0x0007E3C1 File Offset: 0x0007C7C1
	[Inject]
	public PlayerInputLocator playerInput { private get; set; }

	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x06001796 RID: 6038 RVA: 0x0007E3CA File Offset: 0x0007C7CA
	// (set) Token: 0x06001797 RID: 6039 RVA: 0x0007E3D2 File Offset: 0x0007C7D2
	[Inject]
	public IPerformanceTracker performanceTracker { private get; set; }

	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x06001798 RID: 6040 RVA: 0x0007E3DB File Offset: 0x0007C7DB
	// (set) Token: 0x06001799 RID: 6041 RVA: 0x0007E3E3 File Offset: 0x0007C7E3
	[Inject]
	public IEnterNewGame enterNewGame { private get; set; }

	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x0600179A RID: 6042 RVA: 0x0007E3EC File Offset: 0x0007C7EC
	// (set) Token: 0x0600179B RID: 6043 RVA: 0x0007E3F4 File Offset: 0x0007C7F4
	[Inject]
	public AnnouncementHelper announcements { private get; set; }

	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x0600179C RID: 6044 RVA: 0x0007E3FD File Offset: 0x0007C7FD
	// (set) Token: 0x0600179D RID: 6045 RVA: 0x0007E405 File Offset: 0x0007C805
	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel { private get; set; }

	// Token: 0x170004AB RID: 1195
	// (get) Token: 0x0600179E RID: 6046 RVA: 0x0007E40E File Offset: 0x0007C80E
	// (set) Token: 0x0600179F RID: 6047 RVA: 0x0007E416 File Offset: 0x0007C816
	[Inject]
	public ConfigData config { private get; set; }

	// Token: 0x170004AC RID: 1196
	// (get) Token: 0x060017A0 RID: 6048 RVA: 0x0007E41F File Offset: 0x0007C81F
	// (set) Token: 0x060017A1 RID: 6049 RVA: 0x0007E427 File Offset: 0x0007C827
	[Inject]
	public GameEnvironmentData environmentData { private get; set; }

	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x060017A2 RID: 6050 RVA: 0x0007E430 File Offset: 0x0007C830
	// (set) Token: 0x060017A3 RID: 6051 RVA: 0x0007E438 File Offset: 0x0007C838
	[Inject]
	public IReplaySystem replaySystem { private get; set; }

	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x060017A4 RID: 6052 RVA: 0x0007E441 File Offset: 0x0007C841
	// (set) Token: 0x060017A5 RID: 6053 RVA: 0x0007E449 File Offset: 0x0007C849
	[Inject]
	public IGetEquippedItemsFromServer getEquippedItems { get; set; }

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x060017A6 RID: 6054 RVA: 0x0007E452 File Offset: 0x0007C852
	// (set) Token: 0x060017A7 RID: 6055 RVA: 0x0007E45A File Offset: 0x0007C85A
	[Inject]
	public IOnlineSubsystem onlineSubsystem { get; set; }

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x060017A8 RID: 6056 RVA: 0x0007E463 File Offset: 0x0007C863
	// (set) Token: 0x060017A9 RID: 6057 RVA: 0x0007E46B File Offset: 0x0007C86B
	[Inject]
	public ICustomLobbyController customLobby { get; set; }

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x060017AA RID: 6058 RVA: 0x0007E474 File Offset: 0x0007C874
	// (set) Token: 0x060017AB RID: 6059 RVA: 0x0007E47C File Offset: 0x0007C87C
	public GameDataManager GameData { get; private set; }

	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x060017AC RID: 6060 RVA: 0x0007E485 File Offset: 0x0007C885
	// (set) Token: 0x060017AD RID: 6061 RVA: 0x0007E48D File Offset: 0x0007C88D
	public AudioManager Audio { get; private set; }

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x060017AE RID: 6062 RVA: 0x0007E496 File Offset: 0x0007C896
	// (set) Token: 0x060017AF RID: 6063 RVA: 0x0007E49E File Offset: 0x0007C89E
	public UserAudioSettings UserAudioSettings { get; private set; }

	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x060017B0 RID: 6064 RVA: 0x0007E4A7 File Offset: 0x0007C8A7
	// (set) Token: 0x060017B1 RID: 6065 RVA: 0x0007E4AF File Offset: 0x0007C8AF
	public PlayerInputManager Input { get; private set; }

	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x060017B2 RID: 6066 RVA: 0x0007E4B8 File Offset: 0x0007C8B8
	// (set) Token: 0x060017B3 RID: 6067 RVA: 0x0007E4C0 File Offset: 0x0007C8C0
	public FPSCounter DisplayFPSCounter { get; private set; }

	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x060017B4 RID: 6068 RVA: 0x0007E4C9 File Offset: 0x0007C8C9
	// (set) Token: 0x060017B5 RID: 6069 RVA: 0x0007E4D1 File Offset: 0x0007C8D1
	public FPSCounter GameTickFPSCounter { get; private set; }

	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x060017B6 RID: 6070 RVA: 0x0007E4DA File Offset: 0x0007C8DA
	// (set) Token: 0x060017B7 RID: 6071 RVA: 0x0007E4E2 File Offset: 0x0007C8E2
	public IUserInputManager UserInputManager { get; private set; }

	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x060017B8 RID: 6072 RVA: 0x0007E4EB File Offset: 0x0007C8EB
	// (set) Token: 0x060017B9 RID: 6073 RVA: 0x0007E4F3 File Offset: 0x0007C8F3
	public UserFacingPerformanceDisplay PerformanceDisplay { get; private set; }

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x060017BA RID: 6074 RVA: 0x0007E4FC File Offset: 0x0007C8FC
	// (set) Token: 0x060017BB RID: 6075 RVA: 0x0007E504 File Offset: 0x0007C904
	private global::ILogger logger { get; set; }

	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x060017BC RID: 6076 RVA: 0x0007E50D File Offset: 0x0007C90D
	private GameManager gameManager
	{
		get
		{
			return (this.gameController != null) ? this.gameController.currentGame : null;
		}
	}

	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x060017BD RID: 6077 RVA: 0x0007E52B File Offset: 0x0007C92B
	private int frame
	{
		get
		{
			return (!(this.gameManager != null)) ? 0 : this.gameManager.Frame;
		}
	}

	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x060017BE RID: 6078 RVA: 0x0007E54F File Offset: 0x0007C94F
	// (set) Token: 0x060017BF RID: 6079 RVA: 0x0007E556 File Offset: 0x0007C956
	public static bool IsCreated { get; private set; }

	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x060017C0 RID: 6080 RVA: 0x0007E55E File Offset: 0x0007C95E
	public static bool IsRollingBack
	{
		get
		{
			return !(GameClient.instance == null) && !(GameClient.instance.gameManager == null) && GameClient.instance.gameManager.IsRollingBack;
		}
	}

	// Token: 0x170004BE RID: 1214
	// (get) Token: 0x060017C1 RID: 6081 RVA: 0x0007E59C File Offset: 0x0007C99C
	public static bool IsCurrentFrame
	{
		get
		{
			return GameClient.instance == null || GameClient.instance.gameManager == null || GameClient.instance.gameManager.FrameController == null || (GameClient.instance.gameManager.FrameController.IsCurrentFrame && !GameClient.instance.gameManager.IsRollingBack);
		}
	}

	// Token: 0x060017C2 RID: 6082 RVA: 0x0007E61C File Offset: 0x0007CA1C
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

	// Token: 0x060017C3 RID: 6083 RVA: 0x0007E658 File Offset: 0x0007CA58
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
		Application.logMessageReceived += this.handleUnityLog;
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
		foreach (LocalizationData regionData in this.config.uiLocalizationData.localizationData)
		{
			this.localization.AddLocalizationData(regionData);
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

	// Token: 0x060017C4 RID: 6084 RVA: 0x0007EAF8 File Offset: 0x0007CEF8
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
				Debug.LogError(string.Concat(new string[]
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
				Debug.LogWarning(string.Concat(new string[]
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

	// Token: 0x060017C5 RID: 6085 RVA: 0x0007ECD6 File Offset: 0x0007D0D6
	private void startupListeningClasses()
	{
		this.injector.GetInstance<DebugKeys>();
		this.injector.GetInstance<IEnterNewGame>();
		this.injector.GetInstance<EnterCustomLobbyController>();
		this.injector.GetInstance<IUserCurrencyReceiver>();
		this.injector.GetInstance<IApplicationFramerateManager>();
	}

	// Token: 0x060017C6 RID: 6086 RVA: 0x0007ED14 File Offset: 0x0007D114
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
				this.optionsProfileAPI.DeleteDefaultSettings(delegate(SaveOptionsProfileResult result)
				{
					this.Events.Broadcast(new LoadScreenCommand(ScreenType.CharacterSelect, null, ScreenUpdateType.Next));
				});
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

	// Token: 0x060017C7 RID: 6087 RVA: 0x0007EE5C File Offset: 0x0007D25C
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
		Application.logMessageReceived -= this.handleUnityLog;
	}

	// Token: 0x060017C8 RID: 6088 RVA: 0x0007EED8 File Offset: 0x0007D2D8
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

	// Token: 0x060017C9 RID: 6089 RVA: 0x0007EF0C File Offset: 0x0007D30C
	private void Update()
	{
		if (this.isStarted)
		{
			this.tickOncePerRenderFrame();
		}
	}

	// Token: 0x060017CA RID: 6090 RVA: 0x0007EF1F File Offset: 0x0007D31F
	protected void FixedUpdate()
	{
		if (this.isStarted)
		{
			this.TickFrame();
		}
	}

	// Token: 0x060017CB RID: 6091 RVA: 0x0007EF34 File Offset: 0x0007D334
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

	// Token: 0x060017CC RID: 6092 RVA: 0x0007EFAC File Offset: 0x0007D3AC
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

	// Token: 0x170004BF RID: 1215
	// (get) Token: 0x060017CD RID: 6093 RVA: 0x0007F03E File Offset: 0x0007D43E
	private bool useGameplayTick
	{
		get
		{
			return this.gameController.MatchIsRunning;
		}
	}

	// Token: 0x060017CE RID: 6094 RVA: 0x0007F04C File Offset: 0x0007D44C
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

	// Token: 0x060017CF RID: 6095 RVA: 0x0007F0A4 File Offset: 0x0007D4A4
	private void createGameManager(GameEvent message)
	{
		this.gameController.SetCurrentGame(this.injector.CreateComponentWithGameObject<GameManager>("GameManager"));
		this.gameManager.Initialize(this);
		this.gameController.preloader = new GameAssetPreloader();
		this.injector.Inject(this.gameController.preloader);
	}

	// Token: 0x060017D0 RID: 6096 RVA: 0x0007F100 File Offset: 0x0007D500
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

	// Token: 0x060017D1 RID: 6097 RVA: 0x0007F191 File Offset: 0x0007D591
	public void PreCompleteMatch()
	{
	}

	// Token: 0x060017D2 RID: 6098 RVA: 0x0007F194 File Offset: 0x0007D594
	private void endGame(VictoryScreenPayload payload)
	{
		if (this.gameManager != null && !this.screenshotModel.InProgress)
		{
			bool isNetworkGame = this.battleServerAPI.IsConnected;
			Guid matchID = this.battleServerAPI.MatchID;
			if (payload.wasForfeited && payload.gamePayload.isOnlineGame)
			{
				this.Events.Broadcast(new LeaveRoomCommand());
			}
			this.screenshotModel.SaveScreenshot(delegate
			{
				string text = this.config.replaySettings.replayName;
				if (isNetworkGame)
				{
					text = string.Format("{0}-{1}-{2}", text, matchID, this.iconsServerAPI.Username);
				}
				this.replaySystem.OnGameFinished(this.frame, payload, text);
				this.Events.Broadcast(new TransitionToVictoryPoseCommand(payload));
			});
		}
	}

	// Token: 0x060017D3 RID: 6099 RVA: 0x0007F251 File Offset: 0x0007D651
	public static void Log(LogLevel logLevel, params object[] parameters)
	{
		GameClient.Logger.Log(logLevel, parameters);
	}

	// Token: 0x060017D4 RID: 6100 RVA: 0x0007F260 File Offset: 0x0007D660
	private void handleUnityLog(string logString, string stackTrace, LogType logType)
	{
		BroadcastingLogger broadcastingLogger = GameClient.Logger as BroadcastingLogger;
		broadcastingLogger.LogUnityLog(logString, stackTrace, logType);
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x0007F2AC File Offset: 0x0007D6AC
	Coroutine ICoroutineStarter.StartCoroutine(IEnumerator routine)
	{
		return base.StartCoroutine(routine);
	}

	// Token: 0x04001217 RID: 4631
	public static Vector2 NATIVE_RESOLUTION = new Vector2(1920f, 1080f);

	// Token: 0x04001240 RID: 4672
	private GameObject toolContainer;

	// Token: 0x04001241 RID: 4673
	private bool isStarted;

	// Token: 0x04001242 RID: 4674
	private static GameClient instance;

	// Token: 0x04001243 RID: 4675
	private static global::ILogger Logger;
}
