// Decompile from assembly: Assembly-CSharp.dll

using RollbackDebug;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.impl;
using strange.extensions.injector.api;
using System;
using UnityEngine;

public class MasterContext : MVCSContext
{
	public class InitContext
	{
		public ConfigData config;

		public GameEnvironmentData environment;

		public DeveloperConfig devConfig;

		public IInjectionBinder injectionBinder;

		public IOfflineModeDetector offlineModeDetector;

		public InitContext(ConfigData config, GameEnvironmentData environment, DeveloperConfig devConfig)
		{
			this.config = config;
			this.environment = environment;
			this.devConfig = devConfig;
		}
	}

	private IDependencyInjection diWrapper;

	private MasterContext.InitContext initContext;

	public MasterContext(MonoBehaviour view, MasterContext.InitContext initContext) : base(view)
	{
		this.initContext = initContext;
		this.configureBindingMap();
	}

	protected override void addCoreComponents()
	{
		base.addCoreComponents();
		base.injectionBinder.Unbind<ICommandBinder>();
		base.injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
	}

	protected override void postBindings()
	{
	}

	public override void Launch()
	{
	}

	private void configureBindingMap()
	{
		this.diWrapper = new DependencyInjection(base.injectionBinder);
		this.initContext.injectionBinder = base.injectionBinder;
		base.injectionBinder.Bind<IDependencyInjection>().ToValue(this.diWrapper);
		base.injectionBinder.Bind<IAutoQualityDetection>().To<AutoQualityDetection>().ToSingleton();
		base.injectionBinder.Bind<IInjector>().ToValue(base.injectionBinder.injector);
		base.injectionBinder.Bind<ISignalBus>().To<SignalBus>().ToSingleton();
		base.injectionBinder.Bind<global::ILogger>().To<BroadcastingLogger>().ToSingleton();
		base.injectionBinder.Bind<EventLogger>().To<EventLogger>().ToSingleton();
		base.injectionBinder.Bind<ConsoleLogger>().To<ConsoleLogger>();
		base.injectionBinder.Bind<ISceneController>().To<SceneController>().ToSingleton();
		base.injectionBinder.Bind<ISceneLists>().To<SceneLists>().ToSingleton();
		base.injectionBinder.Bind<ISoundFileManager>().To<SoundFileManager>().ToSingleton();
		base.injectionBinder.Bind<AudioManager>().ToSingleton();
		base.injectionBinder.Bind<IAudioHandler>().To<AudioHandler>().ToSingleton();
		base.injectionBinder.Bind<IAudioPlayer>().To<AudioPlayer>().ToSingleton();
		base.injectionBinder.Bind<UserAudioSettings>().ToSingleton();
		base.injectionBinder.Bind<GameDataManager>().ToSingleton();
		base.injectionBinder.Bind<IEffectHelper>().To<EffectHelper>().ToSingleton();
		base.injectionBinder.Bind<IGameVFX>().To<GameVFX>();
		base.injectionBinder.Bind<DynamicObjectContainer>().To<DynamicObjectContainer>();
		base.injectionBinder.Bind<IBakedAnimationDataManager>().To<BakedAnimationDataManager>().ToSingleton();
		base.injectionBinder.Bind<ISaveFileData>().To<SaveFileData>().ToSingleton();
		base.injectionBinder.Bind<ILoadDataSequence>().To<LoadDataSequence>().ToSingleton();
		base.injectionBinder.Bind<ILocalization>().To<Localization>().ToSingleton();
		base.injectionBinder.Bind<IHyperlinkHandler>().To<HyperlinkHandler>().ToSingleton();
		base.injectionBinder.Bind<IBackgroundLoader>().To<BackgroundLoader>().ToSingleton();
		base.injectionBinder.Bind<IVRAMPreloader>().To<VRAMPreloader>().ToSingleton();
		base.injectionBinder.Bind<GameData>().To<GameData>();
		base.injectionBinder.Bind<StageMusicMap>().ToSingleton();
		base.injectionBinder.Bind<ICharacterDataLoader>().To<CharacterDataLoader>().ToSingleton();
		base.injectionBinder.Bind<ICharacterMenusDataLoader>().To<CharacterMenusDataLoader>().ToSingleton();
		base.injectionBinder.Bind<ISkinDataManager>().To<SkinDataManager>().ToSingleton();
		base.injectionBinder.Bind<IUserManager>().To<UserManager>().ToSingleton();
		base.injectionBinder.Bind<IUserInputManager>().To<UserInputManager>().ToSingleton();
		base.injectionBinder.Bind<PlayerInputLocator>().To<PlayerInputLocator>().ToSingleton();
		base.injectionBinder.Bind<IKeyBindingManager>().To<KeyBindingManager>().ToSingleton();
		base.injectionBinder.Bind<IEquipmentModel>().To<EquipmentModel>().ToSingleton();
		base.injectionBinder.Bind<IEquipMethodMap>().To<EquipMethodMap>().ToSingleton();
		base.injectionBinder.Bind<IItemLoader>().To<ItemLoader>().ToSingleton();
		base.injectionBinder.Bind<IAutoJoin>().To<AutoJoin>().ToSingleton();
		base.injectionBinder.Bind<IUserInventory>().To<UserInventoryModel>().ToSingleton();
		base.injectionBinder.Bind<IUserLootboxesModel>().To<UserLootboxesModel>().ToSingleton();
		base.injectionBinder.Bind<IUserTauntsModel>().To<UserTauntsModel>().ToSingleton();
		base.injectionBinder.Bind<IUserCharacterEquippedModel>().To<UserCharacterEquippedModel>().ToSingleton();
		base.injectionBinder.Bind<IUserGlobalEquippedModel>().To<UserGlobalEquippedModel>().ToSingleton();
		base.injectionBinder.Bind<IUserCharacterUnlockModel>().To<UserCharacterUnlockModel>().ToSingleton();
		base.injectionBinder.Bind<IUserProAccountUnlockedModel>().To<UserProAccountUnlockedModel>().ToSingleton();
		base.injectionBinder.Bind<IUnlockCharacterFlow>().To<UnlockCharacterFlow>().ToSingleton();
		base.injectionBinder.Bind<IDetailedUnlockCharacterFlow>().To<DetailedUnlockCharacterFlow>().ToSingleton();
		base.injectionBinder.Bind<IBuyLootboxFlow>().To<BuyLootboxFlow>().ToSingleton();
		base.injectionBinder.Bind<ICreate3DItemDisplay>().To<Create3DItemDisplay>().ToSingleton();
		base.injectionBinder.Bind<IPreviewAnimationHelper>().To<PreviewAnimationHelper>().ToSingleton();
		base.injectionBinder.Bind<IUserCurrencyModel>().To<UserCurrencyModel>().ToSingleton();
		base.injectionBinder.Bind<ILootBoxesModel>().To<LootBoxesModel>().ToSingleton();
		base.injectionBinder.Bind<IRespawnPlatformLocator>().To<RespawnPlatformLocator>().ToSingleton();
		base.injectionBinder.Bind<IDebugLatencyManager>().To<DebugLatencyManager>().ToSingleton();
		base.injectionBinder.Bind<IMatchDeepLogging>().To<MatchDeepLogging>().ToSingleton();
		base.injectionBinder.Bind<SaveReplay>().To<SaveReplay>().ToSingleton();
		base.injectionBinder.Bind<IReplaySystem>().To<ReplaySystem>().ToSingleton();
		base.injectionBinder.Bind<IRollbackDebugLayer>().To<RollbackDebugLayer>().ToSingleton();
		base.injectionBinder.Bind<IRollbackStatus>().To<RollbackStatusLocator>();
		base.injectionBinder.Bind<PhysicsSimulator>().To<PhysicsSimulator>();
		base.injectionBinder.Bind<IPhysicsCalculator>().To<PhysicsCalculator>().ToSingleton();
		base.injectionBinder.Bind<CharacterSelectCalculator>().To<CharacterSelectCalculator>().ToSingleton();
		base.injectionBinder.Bind<SelectRandomCharacters>().To<SelectRandomCharacters>().ToSingleton();
		base.injectionBinder.Bind<ICharacterLists>().To<CharacterLists>().ToSingleton();
		base.injectionBinder.Bind<ICharacterDataHelper>().To<CharacterDataHelper>().ToSingleton();
		base.injectionBinder.Bind<IVictoryPoseHelper>().To<VictoryPoseHelper>().ToSingleton();
		base.injectionBinder.Bind<IStageDataHelper>().To<StageDataHelper>().ToSingleton();
		base.injectionBinder.Bind<IWeaponTrailHelper>().To<WeaponTrailHelper>().ToSingleton();
		base.injectionBinder.Bind<IInputSettingsSaveManager>().To<InputSettingsSaveManager>().ToSingleton();
		base.injectionBinder.Bind<IOptionProfileSaver>().To<OptionProfileSaverLocalFiles>().ToSingleton();
		base.injectionBinder.Bind<IOptionsProfileManager>().To<OptionsProfileManager>().ToSingleton();
		base.injectionBinder.Bind<OptionsProfileAPI>().ToSingleton();
		base.injectionBinder.Bind<IPurchaseResponseHandler>().To<PurchaseResponseHandler>().ToSingleton();
		base.injectionBinder.Bind<IStaticDataSource>().To<StaticDataSource>().ToSingleton();
		base.injectionBinder.Bind<IPreviousCrashDetector>().To<PreviousCrashDetector>().ToSingleton();
		base.injectionBinder.Bind<IStartupArgs>().To<StartupArgs>().ToSingleton();
		base.injectionBinder.Bind<IExitGame>().To<ExitGame>().ToSingleton();
		base.injectionBinder.Bind<MusicPlayer>().To<MusicPlayer>();
		base.injectionBinder.Bind<IAudioVolumeConfig>().To<GameVolumeConfig>().ToSingleton();
		base.injectionBinder.Bind<CharacterInputProcessor>().To<CharacterInputProcessor>();
		base.injectionBinder.Bind<ObjectPoolManager>().To<ObjectPoolManager>();
		base.injectionBinder.Bind<GameObjectPool>().To<GameObjectPool>();
		base.injectionBinder.Bind<IUserVideoSettingsModel>().To<UserVideoSettingsModel>().ToSingleton();
		base.injectionBinder.Bind<IUserGameplaySettingsModel>().To<UserGameplaySettingsModel>().ToSingleton();
		base.injectionBinder.Bind<IVideoSettingsUtility>().To<VideoSettingsUtility>().ToSingleton();
		base.injectionBinder.Bind<IEnterNewGame>().To<EnterNewGameController>().ToSingleton();
		base.injectionBinder.Bind<IPostBattleFlow>().To<PostBattleFlow>().ToSingleton();
		base.injectionBinder.Bind<ICustomLobbyEventNotifier>().To<CustomLobbyEventNotifier>().ToSingleton();
		base.injectionBinder.Bind<IMatchHistory>().To<MatchHistoryModel>().ToSingleton();
		base.injectionBinder.Bind<ICharacterSelectSharedFunctions>().To<CharacterSelectSharedFunctions>();
		if (this.initContext.config.rollbackDebugSettings.enableRollbackDebug)
		{
			base.injectionBinder.Bind<IRollbackLayerDebugger>().To<RollbackLayerDebugger>().ToSingleton();
		}
		else
		{
			base.injectionBinder.Bind<IRollbackLayerDebugger>().To<DummyRollbackLayerDebugger>().ToSingleton();
		}
		new ContextConfigUI(this.initContext);
		new ContextConfigGameplay(this.initContext);
		new ContextConfigOnline(this.initContext);
		new GameObject("ProxyMonoObjects").AddComponent<ProxyMono>();
		base.injectionBinder.Bind<ITimeStatTrackerManager>().To<TimeStatTrackerManager>().ToSingleton();
		base.injectionBinder.Bind<IRollbackStatePooling>().To<RollbackStatePooling>().ToSingleton();
		base.injectionBinder.Bind<IRollbackStateCodeValidator>().To<RollbackStateCodeValidator>().ToSingleton();
		base.injectionBinder.Bind<DebugKeys>().ToSingleton();
		base.injectionBinder.Bind<IMainThreadTimer>().To<UnityThreadTimer>().ToSingleton();
		base.injectionBinder.Bind<IEvents>().To<Events>().ToSingleton();
		base.injectionBinder.Bind<ScreenshotModel>().ToSingleton();
		base.injectionBinder.Bind<IStoreAPI>().To<StoreAPI>().ToSingleton();
		base.injectionBinder.Bind<IPerformanceTracker>().To<PerformanceTracker>().ToSingleton();
		base.injectionBinder.Bind<IApplicationFramerateManager>().To<ApplicationFramerateManager>().ToSingleton();
		base.injectionBinder.Bind<IExceptionParser>().To<ExceptionParser>().ToSingleton();
	}
}
