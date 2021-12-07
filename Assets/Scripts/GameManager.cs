// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using IconsServer;
using RollbackDebug;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IRollbackStateOwner, IRollbackGameClient, IFrameOwner, IPlayerLookup, IGame, IModeOwner, IStageTriggerDependency, IRollbackClient, IEndableClient
{
	private sealed class _endGame_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float seconds;

		internal bool notifiedBattleServer;

		internal List<TeamNum> winningTeams;

		internal VictoryScreenPayload _victoryPayload___0;

		internal List<PlayerNum> winners;

		internal GameManager _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _endGame_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForSeconds(this.seconds);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (this._this.battleServerAPI.IsConnected && !this.notifiedBattleServer)
				{
					this._this.battleServerAPI.SendWinner(this.winningTeams);
				}
				this._victoryPayload___0 = new VictoryScreenPayload();
				this._victoryPayload___0.stats = this._this.statsTracker.PlayerStats;
				this._this.GetEndGameCharacterIndicies(this._victoryPayload___0.endGameCharacterIndicies);
				this._victoryPayload___0.victors = this.winners;
				this._victoryPayload___0.winningTeams = this.winningTeams;
				this._victoryPayload___0.gamePayload = this._this.gameConfig;
				this._victoryPayload___0.wasForfeited = this._this.gameManagerState.gameWasForfeitted;
				this._victoryPayload___0.nextScreen = ScreenType.VictoryGUI;
				this._this.EndGame(this._victoryPayload___0);
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private int gameStartFrame;

	public StageData stageData;

	private int gameStartInputFrame;

	private int allowGameplayInputsFrame;

	private Dictionary<int, int> rollbackCounts = new Dictionary<int, int>();

	private IGrabManager grabManager;

	private GameObject inactiveObjects;

	private GameLoadPayload gameConfig;

	private GameManagerState gameManagerState = new GameManagerState();

	private StatsTracker statsTracker;

	public List<PlayerController> CharacterControllers = new List<PlayerController>();

	public List<ICameraInfluencer> CameraInfluencers = new List<ICameraInfluencer>();

	private List<PlayerReference> playerReferences = new List<PlayerReference>();

	private Dictionary<PlayerNum, PlayerReference> playerReferenceMap = new Dictionary<PlayerNum, PlayerReference>(default(PlayerNumComparer));

	private Dictionary<PlayerNum, bool> localPlayers = new Dictionary<PlayerNum, bool>(default(PlayerNumComparer));

	private PlayerSpawner playerSpawner;

	private List<RollbackInput> localInputsBuffer = new List<RollbackInput>();

	private GenericResetObjectPool<RollbackInput> rollbackInputPool;

	private RollbackInput[] inputsThisFrameBuffer;

	private static GenericResetObjectPool<RollbackInput>.NewCallback __f__am_cache0;

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	[Inject]
	public AudioManager audioManager
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
	public IGameTauntsSetup gameTauntsSetup
	{
		get;
		set;
	}

	[Inject]
	public NetGraphVisualizer netGraph
	{
		get;
		set;
	}

	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		get;
		set;
	}

	[Inject]
	public IDevConsole devConsole
	{
		get;
		set;
	}

	[Inject]
	public IRollbackDebugLayer rollbackDebug
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
	public IBattleServerAPI battleServerAPI
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
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
	public IPerformanceTracker performanceTracker
	{
		get;
		set;
	}

	[Inject]
	public HitCollisionManager Hits
	{
		get;
		set;
	}

	[Inject]
	public AnnouncementHelper announcements
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
	public IIconsServerAPI iconsServerAPI
	{
		get;
		set;
	}

	[Inject]
	public IServerConnectionManager serverConnectionManager
	{
		get;
		set;
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel
	{
		get;
		set;
	}

	[Inject]
	public IMatchDeepLogging deepLogging
	{
		get;
		set;
	}

	[Inject]
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	[Inject]
	public ICharacterLists characterLists
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

	public int Frame
	{
		get
		{
			return (!(this.FrameController == null)) ? this.FrameController.Frame : 0;
		}
	}

	public int GameStartInputFrame
	{
		get
		{
			return this.gameStartInputFrame;
		}
		set
		{
			this.gameStartInputFrame = value;
		}
	}

	public int AllowGameplayInputsFrame
	{
		get
		{
			return this.allowGameplayInputsFrame;
		}
	}

	public string MatchID
	{
		get;
		private set;
	}

	public StageSceneData Stage
	{
		get;
		private set;
	}

	public PhysicsSimulator Physics
	{
		get;
		private set;
	}

	public PhysicsWorld PhysicsWorld
	{
		get;
		private set;
	}

	public FrameController FrameController
	{
		get;
		private set;
	}

	public GameLog Log
	{
		get;
		private set;
	}

	public GameObject GameContainer
	{
		get;
		private set;
	}

	public DynamicObjectContainer DynamicObjects
	{
		get;
		private set;
	}

	public PointAudio PointAudio
	{
		get;
		private set;
	}

	public bool IsRunning
	{
		get;
		private set;
	}

	public bool IsPaused
	{
		get
		{
			return this.PausedPort != null;
		}
	}

	public PlayerNum PausedPlayer
	{
		get
		{
			return this.userInputManager.GetPlayerNum(this.PausedPort);
		}
	}

	public PlayerInputPort PausedPort
	{
		get;
		private set;
	}

	public int PlayerCount
	{
		get;
		private set;
	}

	public BattleSettings BattleSettings
	{
		get;
		private set;
	}

	public bool IsRollingBack
	{
		get;
		set;
	}

	public CameraController Camera
	{
		get;
		private set;
	}

	public UIManager UI
	{
		get
		{
			return this.Client.UI;
		}
	}

	public AudioManager Audio
	{
		get
		{
			return this.Client.Audio;
		}
	}

	public GameClient Client
	{
		get;
		private set;
	}

	public CapsulePool CapsulePool
	{
		get;
		private set;
	}

	public GameLoadPayload GameConfig
	{
		get
		{
			return this.gameConfig;
		}
	}

	public GameDataManager GameData
	{
		get
		{
			return this.Client.GameData;
		}
	}

	public GameMode Mode
	{
		get
		{
			return this.BattleSettings.mode;
		}
	}

	public GameModeData ModeData
	{
		get;
		private set;
	}

	public IGameMode CurrentGameMode
	{
		get;
		private set;
	}

	public GameModeSettings GameModeSettings
	{
		get
		{
			return this.ModeData.settings;
		}
	}

	public bool StartedGame
	{
		get
		{
			return this.gameManagerState.gameStarted;
		}
	}

	public bool EndedGame
	{
		get;
		private set;
	}

	public bool IsTrainingMode
	{
		get
		{
			return this.Mode == GameMode.Training;
		}
	}

	public bool IsNetworkGame
	{
		get
		{
			return this.battleServerAPI.IsConnected;
		}
	}

	public PlayerSpawner PlayerSpawner
	{
		get
		{
			return this.playerSpawner;
		}
	}

	public ComboManager ComboManager
	{
		get;
		private set;
	}

	public ObjectPoolManager ObjectPools
	{
		get;
		private set;
	}

	public bool IsPauseEnabled
	{
		get
		{
			return this.BattleSettings.IsPauseEnabled && (this.ModeData == null || !this.ModeData.settings.disablePausing);
		}
	}

	public List<PlayerReference> PlayerReferences
	{
		get
		{
			return this.playerReferences;
		}
	}

	public bool IsGameComplete
	{
		get
		{
			return this.CurrentGameMode != null && this.CurrentGameMode.IsGameComplete;
		}
	}

	public bool LoadState(RollbackStateContainer container)
	{
		if (container == null || container.Count == 0)
		{
			UnityEngine.Debug.LogError("Attempted to load state with no content!");
			return false;
		}
		int frame = this.Frame;
		container.ReadState<GameManagerState>(ref this.gameManagerState);
		this.FrameController.LoadState(container);
		this.Stage.LoadState(container);
		this.playerSpawner.LoadState(container);
		this.Hits.LoadState(container);
		this.statsTracker.LoadState(container);
		this.ComboManager.LoadState(container);
		this.announcements.LoadState(container);
		int num = frame - this.Frame;
		if (!this.rollbackCounts.ContainsKey(num))
		{
			this.rollbackCounts.Add(num, 1);
		}
		else
		{
			Dictionary<int, int> dictionary;
			int key;
			(dictionary = this.rollbackCounts)[key = num] = dictionary[key] + 1;
		}
		if (this.UI.DebugTextEnabled)
		{
			this.UI.AddDebugTextEvent(string.Concat(new object[]
			{
				"Rolled back ",
				frame - this.Frame,
				" frames (",
				frame,
				"->",
				this.Frame,
				")"
			}));
		}
		for (int i = 0; i < this.CharacterControllers.Count; i++)
		{
			PlayerController playerController = this.CharacterControllers[i];
			playerController.LoadState(container);
		}
		for (int j = 0; j < this.playerReferences.Count; j++)
		{
			PlayerReference playerReference = this.playerReferences[j];
			playerReference.LoadState(container);
		}
		this.CurrentGameMode.LoadState(container);
		this.DynamicObjects.LoadState(container);
		this.Camera.LoadState(container);
		return true;
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		container.Clear();
		container.WriteState(this.rollbackStatePooling.Clone<GameManagerState>(this.gameManagerState));
		this.FrameController.ExportState(ref container);
		this.Stage.ExportState(ref container);
		this.playerSpawner.ExportState(ref container);
		this.Hits.ExportState(ref container);
		this.statsTracker.ExportState(ref container);
		this.ComboManager.ExportState(ref container);
		this.announcements.ExportState(ref container);
		for (int i = 0; i < this.CharacterControllers.Count; i++)
		{
			PlayerController playerController = this.CharacterControllers[i];
			playerController.ExportState(ref container);
		}
		for (int j = 0; j < this.playerReferences.Count; j++)
		{
			PlayerReference playerReference = this.playerReferences[j];
			playerReference.ExportState(ref container);
		}
		this.CurrentGameMode.ExportState(ref container);
		this.DynamicObjects.ExportState(ref container);
		this.Camera.ExportState(ref container);
		return true;
	}

	private void Awake()
	{
		base.tag = Tags.GameManager;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.onSceneLoaded);
	}

	public void Initialize(GameClient client)
	{
		this.Client = client;
		this.gameManagerState.gameVersion = BuildConfigUtil.GetCompareVersion(this.config);
		this.GameContainer = new GameObject("Game");
		this.GameContainer.transform.SetParent(base.transform);
		this.DynamicObjects = new GameObject("DynamicObjects").AddComponent<DynamicObjectContainer>();
		this.injector.Inject(this.DynamicObjects);
		this.DynamicObjects.transform.SetParent(this.GameContainer.transform, false);
		this.inactiveObjects = new GameObject("InactiveObjects");
		UnityEngine.Object.DontDestroyOnLoad(this.inactiveObjects);
		this.inactiveObjects.SetActive(false);
		this.PointAudio = new GameObject("PointAudio").AddComponent<PointAudio>();
		this.injector.Inject(this.PointAudio);
		this.PointAudio.transform.SetParent(this.GameContainer.transform, false);
		this.PointAudio.Init();
		this.ObjectPools = this.injector.GetInstance<ObjectPoolManager>();
		this.ObjectPools.Init(this.DynamicObjects.transform, this.inactiveObjects.transform);
		this.Camera = this.GameContainer.AddComponent<CameraController>();
		this.injector.Inject(this.Camera);
		this.Physics = this.injector.GetInstance<PhysicsSimulator>();
		this.PhysicsWorld = new PhysicsWorld(this.devConsole);
		this.events.Subscribe(typeof(GameEndEvent), new Events.EventHandler(this.onGameEnd));
		this.events.Subscribe(typeof(GameStartEvent), new Events.EventHandler(this.onGameStart));
		this.events.Subscribe(typeof(DebugDesyncEvent), new Events.EventHandler(this.onDebugDesync));
		this.grabManager = new GrabManager(this);
		this.PausedPort = null;
		Fixed one = (!this.devConfig.disableCountdown) ? this.config.uiuxSettings.countdownIntervalSeconds : Fixed.Zero;
		this.gameStartFrame = (int)((this.config.uiuxSettings.countdownAmt + 1) * one * WTime.fps);
		this.allowGameplayInputsFrame = this.gameStartFrame - this.config.inputConfig.inputBufferFrames;
		this.Log = new GameLog(this.events);
		if (this.config.uiuxSettings.emotiveStartup)
		{
			this.GameStartInputFrame = this.config.uiuxSettings.inputFramesBuffer - this.config.inputConfig.inputBufferFrames;
		}
		else
		{
			this.GameStartInputFrame = this.allowGameplayInputsFrame;
		}
		this.localInputsBuffer.Clear();
	}

	private void onSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.Stage = UnityEngine.Object.FindObjectOfType<StageSceneData>();
	}

	public void StartGame(GameLoadPayload payload)
	{
		this.updatePlayerTeams(payload);
		this.updatePlayerPorts(payload);
		this.stageData = this.gameDataManager.StageData.GetDataByID(payload.stage);
		if (this.stageData != null && this.stageData.stageType == StageType.Random)
		{
			this.stageData = this.gameDataManager.StageData.GetDataByID(payload.stagePayloadData.lastRandomStage);
		}
		if (this.IsNetworkGame)
		{
			this.MatchID = this.battleServerAPI.MatchID.ToString();
		}
		else
		{
			this.MatchID = string.Format("{0}:{1}", (!this.serverConnectionManager.IsConnectedToNexus) ? Environment.UserName : this.iconsServerAPI.Username, Guid.NewGuid());
		}
		string empty = string.Empty;
		if (!this.validateGameLoadPayload(payload, ref empty))
		{
			UnityEngine.Debug.LogError("Invalid game load payload:\n" + empty);
			return;
		}
		this.gameConfig = payload;
		this.gameConfig.isOnlineGame = this.battleServerAPI.IsConnected;
		this.BattleSettings = payload.battleConfig;
		this.ModeData = this.GameData.GameModeData.GetDataByType(this.BattleSettings.mode);
		this.FrameController = this.injector.CreateComponentWithGameObject<FrameController>(null);
		this.statsTracker = new StatsTracker();
		this.injector.Inject(this.statsTracker);
		this.statsTracker.Init(this.gameConfig.players);
		if (this.Stage == null)
		{
			UnityEngine.Debug.LogWarning("GameManager failed to find stage during the scene load. Attempting again.");
			this.Stage = UnityEngine.Object.FindObjectOfType<StageSceneData>();
		}
		this.injector.Inject(this.Stage);
		this.Stage.Startup();
		IEnumerator enumerator = ((IEnumerable)payload.players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)enumerator.Current;
				if (playerSelectionInfo.type == PlayerType.Human)
				{
					this.events.Broadcast(new SetPlayerTypeRequest(playerSelectionInfo.playerNum, PlayerType.Human, true));
				}
				else if (playerSelectionInfo.type == PlayerType.CPU)
				{
					this.events.Broadcast(new SetPlayerTypeRequest(playerSelectionInfo.playerNum, PlayerType.CPU, true));
				}
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
		for (int i = 0; i < payload.players.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo2 = payload.players[i];
			if (playerSelectionInfo2.type != PlayerType.None)
			{
				this.createPlayerReference(playerSelectionInfo2);
			}
		}
		this.CurrentGameMode = this.injector.GetInstance<IGameMode>(this.BattleSettings.mode);
		if (this.CurrentGameMode != null)
		{
			this.CurrentGameMode.Init(this.ModeData, this.config, this.BattleSettings, this.events, this.playerReferences, this);
			this.playerSpawner = this.CurrentGameMode.CreateSpawner(this, this.playerReferenceMap);
			if (this.playerSpawner == null)
			{
				this.playerSpawner = this.injector.GetInstance<PlayerSpawner>();
				this.playerSpawner.Init(this.events, this.Stage, this.ModeData, this.config.respawnConfig, this.playerReferenceMap, this.playerReferences);
			}
			this.playerSpawner.PerformInitialSpawn(payload, this, new Action<PlayerReference, SpawnPointBase>(this.spawnPlayerReference));
			for (int j = 0; j < this.CharacterControllers.Count; j++)
			{
				this.CharacterControllers[j].LoadSharedAnimations(this.playerReferences);
			}
			this.inputsThisFrameBuffer = new RollbackInput[this.CharacterControllers.Count];
			int arg_413_0 = 0;
			if (GameManager.__f__am_cache0 == null)
			{
				GameManager.__f__am_cache0 = new GenericResetObjectPool<RollbackInput>.NewCallback(GameManager._StartGame_m__0);
			}
			this.rollbackInputPool = new GenericResetObjectPool<RollbackInput>(arg_413_0, GameManager.__f__am_cache0, null);
			this.applyQualitySettings();
			this.Camera.Init();
			this.PlayerCount = this.playerReferences.Count;
			this.IsRunning = true;
			this.ComboManager = this.injector.GetInstance<ComboManager>();
			this.ComboManager.Setup(this.playerReferences);
			List<RollbackPlayerData> list = new List<RollbackPlayerData>();
			foreach (PlayerReference current in this.playerReferences)
			{
				int intFromPlayerNum = PlayerUtil.GetIntFromPlayerNum(current.PlayerNum, false);
				bool flag = this.battleServerAPI.IsLocalPlayer(current.PlayerNum);
				if (flag)
				{
					this.localPlayers[current.PlayerNum] = true;
				}
				RollbackPlayerData rollbackPlayerData = new RollbackPlayerData();
				rollbackPlayerData.playerID = intFromPlayerNum;
				rollbackPlayerData.userID = current.PlayerInfo.userID;
				rollbackPlayerData.isSpectator = current.PlayerInfo.isSpectator;
				rollbackPlayerData.isLocal = flag;
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Add player ",
					current.PlayerInfo.curProfile.profileName,
					" ",
					rollbackPlayerData.playerID,
					" ",
					rollbackPlayerData.userID
				}));
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Local: ",
					flag,
					", Spectator: ",
					rollbackPlayerData.isSpectator
				}));
				list.Add(rollbackPlayerData);
			}
			RollbackSettings rollbackSettings = new RollbackSettings(this.PlayerCount, list, this.replaySystem, this.rollbackInputPool, this.config.networkSettings);
			rollbackSettings.UpdatePerMatchNetworkSettings(this.battleServerAPI);
			IRollbackLayerDebugger instance = this.injector.GetInstance<IRollbackLayerDebugger>();
			this.FrameController.Init(rollbackSettings, this.config.rollbackDebugSettings, instance, this.replaySystem.IsReplaying);
			if (this.config.DebugConfig.beginDebugPaused)
			{
				this.FrameController.SetControlMode(FrameControlMode.Manual);
			}
			this.gameTauntsSetup.Execute(this);
			this.events.Broadcast(new GameInitEvent(this));
			return;
		}
		throw new Exception("GameMode not found for type " + this.BattleSettings.mode + ". Please make sure it is bound in ContextConfigGameplay.cs");
	}

	public PlayerReference getAllyReferenceWithValidController(PlayerReference player)
	{
		if (this.playerReferenceMap[player.PlayerNum].InputController == null)
		{
			foreach (KeyValuePair<PlayerNum, PlayerReference> current in this.playerReferenceMap)
			{
				if (current.Key != player.PlayerNum && current.Value.Team == player.Team && current.Value.InputController != null)
				{
					return current.Value;
				}
			}
		}
		return null;
	}

	public PlayerReference getAllyReferenceWithInvalidController(PlayerReference player)
	{
		if (this.playerReferenceMap[player.PlayerNum].InputController != null)
		{
			foreach (KeyValuePair<PlayerNum, PlayerReference> current in this.playerReferenceMap)
			{
				if (current.Key != player.PlayerNum && current.Value.Team == player.Team && current.Value.InputController == null)
				{
					return current.Value;
				}
			}
		}
		return null;
	}

	private void applyQualitySettings()
	{
		if (this.userVideoSettingsModel.MaterialQuality == ThreeTierQualityLevel.Low)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			List<Renderer> list = new List<Renderer>();
			foreach (PlayerController current in this.CharacterControllers)
			{
				if (current)
				{
					list.AddRange(current.GetComponentsInChildren<Renderer>(true));
				}
			}
			SceneUtil.AssignCheapMaterialToRenderers(list, new Material(Shader.Find("WaveDash/SimpleCharacter")), null);
			SceneUtil.AssignCheapMaterialToTransforms(new List<Transform>
			{
				this.ObjectPools.ParentTransform,
				this.ObjectPools.InactiveTransform
			});
			stopwatch.Stop();
			UnityEngine.Debug.LogFormat("Assign Cheap Materials: {0}s", new object[]
			{
				stopwatch.Elapsed.TotalSeconds
			});
		}
		foreach (PlayerController current2 in this.CharacterControllers)
		{
			if (current2)
			{
				current2.InitMaterials();
			}
		}
	}

	private void updatePlayerPorts(GameLoadPayload gamePayload)
	{
		if (!this.battleServerAPI.IsOnlineMatchReady || !this.battleServerAPI.IsSinglePlayerNetworkGame)
		{
			for (int i = 0; i < gamePayload.players.Length; i++)
			{
				PlayerSelectionInfo playerSelectionInfo = gamePayload.players[i];
				if (playerSelectionInfo.type == PlayerType.Human && this.battleServerAPI.IsLocalPlayer(playerSelectionInfo.playerNum))
				{
					PlayerInputPort playerInputPort = this.userInputManager.GetPortWithPlayerNum(playerSelectionInfo.playerNum);
					if (playerInputPort == null)
					{
						playerInputPort = this.userInputManager.GetFirstPortWithNoPlayer();
						if (this.userInputManager.AssignPlayerNum(playerInputPort.Id, playerSelectionInfo.playerNum))
						{
							this.signalBus.GetSignal<PlayerAssignedSignal>().Dispatch(playerSelectionInfo.playerNum);
							this.Client.Input.AssignFirstAvailableDevice(playerInputPort, DevicePreference.Any);
						}
					}
				}
			}
		}
	}

	private void updatePlayerTeams(GameLoadPayload gamePayload)
	{
		GameMode mode = gamePayload.battleConfig.mode;
		GameModeData dataByType = this.GameData.GameModeData.GetDataByType(mode);
		if (!dataByType.settings.usesTeams)
		{
			for (int i = 0; i < gamePayload.players.Length; i++)
			{
				PlayerSelectionInfo playerSelectionInfo = gamePayload.players[i];
				playerSelectionInfo.SetTeam(mode, PlayerUtil.GetTeamNumFromInt(i, true));
			}
		}
	}

	private bool validateGameLoadPayload(GameLoadPayload payload, ref string error)
	{
		bool result = true;
		GameMode mode = payload.battleConfig.mode;
		GameModeData dataByType = this.GameData.GameModeData.GetDataByType(mode);
		if (!dataByType.settings.usesTeams)
		{
			HashSet<TeamNum> hashSet = new HashSet<TeamNum>(default(TeamNumComparer));
			for (int i = 0; i < payload.players.Length; i++)
			{
				PlayerSelectionInfo playerSelectionInfo = payload.players[i];
				if (playerSelectionInfo.type != PlayerType.None && !playerSelectionInfo.isSpectator)
				{
					if (hashSet.Contains(playerSelectionInfo.GetTeam(mode)))
					{
						result = false;
						string text = error;
						error = string.Concat(new object[]
						{
							text,
							"Duplicate team in non-team mode: ",
							playerSelectionInfo.GetTeam(mode),
							"\n"
						});
						break;
					}
					hashSet.Add(playerSelectionInfo.GetTeam(mode));
				}
			}
		}
		return result;
	}

	protected PlayerReference createPlayerReference(PlayerSelectionInfo info)
	{
		PlayerReference instance = this.injector.GetInstance<PlayerReference>();
		instance.Init(info);
		this.playerReferences.Add(instance);
		this.playerReferenceMap[instance.PlayerNum] = instance;
		return instance;
	}

	protected void spawnPlayerReference(PlayerReference reference, SpawnPointBase point)
	{
		GameObject gameObject = this.injector.CreateGameObjectWithComponent<PlayerController>("Player" + this.playerReferences.Count);
		PlayerController component = gameObject.GetComponent<PlayerController>();
		gameObject.transform.SetParent(this.GameContainer.transform, false);
		this.CharacterControllers.Add(component);
		this.CameraInfluencers.Add(component);
		reference.AddController(component);
		component.Init(reference.PlayerInfo, reference, this.ModeData, point);
	}

	public PlayerReference GetPlayerReference(PlayerNum player)
	{
		if (!this.playerReferenceMap.ContainsKey(player))
		{
			UnityEngine.Debug.LogError("Failed to find reference for " + player);
			return null;
		}
		return this.playerReferenceMap[player];
	}

	public void OnDestroy()
	{
		if (this.config != null)
		{
			Time.timeScale = this.config.gameSpeed;
		}
		this.IsRunning = false;
		if (this.GameContainer != null)
		{
			UnityEngine.Object.Destroy(this.GameContainer);
			this.GameContainer = null;
		}
		if (this.DynamicObjects != null)
		{
			UnityEngine.Object.Destroy(this.DynamicObjects.gameObject);
			this.DynamicObjects = null;
		}
		if (this.PointAudio != null)
		{
			UnityEngine.Object.Destroy(this.PointAudio.gameObject);
			this.PointAudio = null;
		}
		if (this.inactiveObjects != null)
		{
			UnityEngine.Object.DestroyImmediate(this.inactiveObjects);
			this.inactiveObjects = null;
		}
		if (this.CurrentGameMode != null)
		{
			this.CurrentGameMode.Destroy();
			this.CurrentGameMode = null;
		}
		if (this.statsTracker != null)
		{
			this.statsTracker.Destroy();
			this.statsTracker = null;
		}
		if (this.playerSpawner != null)
		{
			this.playerSpawner.Destroy();
		}
		if (this.ComboManager != null)
		{
			this.ComboManager.Destroy();
		}
		this.events.Unsubscribe(typeof(GameEndEvent), new Events.EventHandler(this.onGameEnd));
		this.events.Unsubscribe(typeof(GameStartEvent), new Events.EventHandler(this.onGameStart));
		this.events.Unsubscribe(typeof(DebugDesyncEvent), new Events.EventHandler(this.onDebugDesync));
		for (int i = 0; i < this.playerReferences.Count; i++)
		{
			this.playerReferences[i].Destroy();
		}
		this.Hits.OnDestroy();
		SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.onSceneLoaded);
		if (this.FrameController != null)
		{
			this.FrameController.OnGameManagerDestroyed();
		}
	}

	public void TogglePaused(PlayerInputPort port)
	{
		if (!this.replaySystem.IsReplaying && (!this.StartedGame || !this.IsPauseEnabled))
		{
			return;
		}
		if (this.IsPaused)
		{
			if (port != this.PausedPort)
			{
				return;
			}
			this.PausedPort = null;
			this.Client.UI.LockingPort = null;
		}
		else
		{
			this.PausedPort = port;
			this.Client.UI.LockingPort = port;
		}
		this.Client.UI.SetPauseMode(this.IsPaused);
		this.Audio.PlayMenuSound((!this.IsPaused) ? SoundKey.inGame_unpause : SoundKey.inGame_pause, 0f);
		this.events.Broadcast(new GamePausedEvent(this.IsPaused, this.PausedPlayer));
		this.Audio.UpdateVolume();
		this.events.Broadcast(new PauseSoundCommand(SoundType.SFX, this.IsPaused));
	}

	private void onGameEnd(GameEvent message)
	{
		GameEndEvent gameEndEvent = message as GameEndEvent;
		if (!this.EndedGame)
		{
			this.EndedGame = true;
			this.Audio.PlayMenuSound(SoundKey.inGame_endGame, 0f);
			bool notifiedBattleServer = false;
			if (this.battleServerAPI.IsConnected)
			{
				this.Client.PreCompleteMatch();
				int num = (this.BattleSettings.mode != GameMode.CrewBattle) ? this.BattleSettings.durationSeconds : this.BattleSettings.crewsBattle_durationSeconds;
				num *= (int)WTime.fps;
				if (this.Frame - this.gameStartFrame >= num - 5 * this.config.uiuxSettings.endGameDelayFrames)
				{
					notifiedBattleServer = true;
					this.battleServerAPI.SendWinner(gameEndEvent.winningTeams);
				}
			}
			base.StartCoroutine(this.endGame((float)this.config.uiuxSettings.endGameDelayFrames / WTime.fps, gameEndEvent.winners, gameEndEvent.winningTeams, notifiedBattleServer));
		}
	}

	private void onGameStart(GameEvent message)
	{
		this.gameManagerState.gameStarted = true;
	}

	private void onDebugDesync(GameEvent message)
	{
		this.gameManagerState.gameDebugDesync = true;
	}

	private IEnumerator endGame(float seconds, List<PlayerNum> winners, List<TeamNum> winningTeams, bool notifiedBattleServer)
	{
		GameManager._endGame_c__Iterator0 _endGame_c__Iterator = new GameManager._endGame_c__Iterator0();
		_endGame_c__Iterator.seconds = seconds;
		_endGame_c__Iterator.notifiedBattleServer = notifiedBattleServer;
		_endGame_c__Iterator.winningTeams = winningTeams;
		_endGame_c__Iterator.winners = winners;
		_endGame_c__Iterator._this = this;
		return _endGame_c__Iterator;
	}

	public void EndGame()
	{
		this.setupEndGame((!this.IsTrainingMode && !this.replaySystem.IsReplaying) ? ScreenType.VictoryGUI : ScreenType.MainMenu, true);
	}

	private void setupEndGame(ScreenType nextScreen, bool forceOnlineMatchEnd)
	{
		if (!this.EndedGame)
		{
			this.EndedGame = true;
			this.Client.UI.LockingPort = null;
			VictoryScreenPayload victoryScreenPayload = new VictoryScreenPayload();
			victoryScreenPayload.stats = this.statsTracker.PlayerStats;
			this.GetEndGameCharacterIndicies(victoryScreenPayload.endGameCharacterIndicies);
			victoryScreenPayload.wasForfeited = forceOnlineMatchEnd;
			victoryScreenPayload.wasExited = !forceOnlineMatchEnd;
			victoryScreenPayload.gamePayload = this.gameConfig;
			victoryScreenPayload.nextScreen = nextScreen;
			victoryScreenPayload.winningTeams.Add(TeamNum.None);
			if (this.battleServerAPI.IsConnected && forceOnlineMatchEnd)
			{
				this.Client.PreCompleteMatch();
				this.battleServerAPI.LeaveRoom(true);
			}
			this.EndGame(victoryScreenPayload);
		}
	}

	public void EndPreviewGame()
	{
		if (this.battleServerAPI.IsConnected)
		{
			throw new Exception("Unhandled winner");
		}
		List<PlayerNum> list = new List<PlayerNum>();
		List<TeamNum> list2 = new List<TeamNum>();
		TeamNum teamNum = TeamNum.None;
		for (int i = 0; i < this.playerReferences.Count; i++)
		{
			PlayerReference playerReference = this.playerReferences[i];
			if (teamNum == TeamNum.None)
			{
				teamNum = playerReference.PlayerInfo.team;
				list2.Add(teamNum);
			}
			if (teamNum == playerReference.PlayerInfo.team)
			{
				list.Add(playerReference.PlayerNum);
			}
		}
		VictoryScreenPayload victoryScreenPayload = new VictoryScreenPayload();
		victoryScreenPayload.stats = this.statsTracker.PlayerStats;
		victoryScreenPayload.victors = list;
		this.GetEndGameCharacterIndicies(victoryScreenPayload.endGameCharacterIndicies);
		victoryScreenPayload.winningTeams = list2;
		victoryScreenPayload.gamePayload = this.gameConfig;
		victoryScreenPayload.wasForfeited = false;
		victoryScreenPayload.nextScreen = ScreenType.VictoryGUI;
		this.EndGame(victoryScreenPayload);
	}

	private void GetEndGameCharacterIndicies(List<int> output)
	{
		output.Clear();
		for (int i = 0; i < this.statsTracker.PlayerStats.Count; i++)
		{
			int item = 0;
			PlayerStats playerStats = this.statsTracker.PlayerStats[i];
			if (playerStats.playerInfo.type != PlayerType.None && PlayerUtil.IsValidPlayer(playerStats.playerInfo.playerNum) && !playerStats.playerInfo.isSpectator)
			{
				CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(playerStats.playerInfo.characterID);
				PlayerReference playerReference = this.GetPlayerReference(playerStats.playerInfo.playerNum);
				item = this.characterDataHelper.GetIndexOfLinkedCharacterData(characterDefinition, playerReference.Controller.CharacterData.characterDefinition);
			}
			output.Add(item);
		}
	}

	public void ForfeitGame(ScreenType nextScreen)
	{
		this.setupEndGame(nextScreen, true);
	}

	public void ExitGame(ScreenType nextScreen, int activePlayerCount)
	{
		this.setupEndGame(nextScreen, activePlayerCount == 0);
	}

	public void EndGame(VictoryScreenPayload victoryPayload)
	{
		this.Audio.StopMusic(null, -1f);
		this.Audio.PlayMenuSound(SoundKey.inGame_exitGame, 0f);
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.HitBoxes, false);
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.HurtBoxes, false);
		this.debugKeys.SetFrameAdvanceOn(false);
		this.FrameController.OnEndGame();
		this.deepLogging.EndMatch();
		this.signalBus.GetSignal<EndGameSignal>().Dispatch(victoryPayload);
	}

	private void syncInputs()
	{
		this.FrameController.SyncInputForFrame(ref this.inputsThisFrameBuffer);
		if (this.inputsThisFrameBuffer[0].values == null)
		{
			UnityEngine.Debug.LogError("Something seems wrong... we didn't get any input for p1 this frame");
		}
		for (int i = 0; i < this.inputsThisFrameBuffer.Length; i++)
		{
			PlayerReference playerReference = this.playerReferences[i];
			PlayerController controller = playerReference.Controller;
			if (!(controller == null) && !(controller.InputController == null))
			{
				controller.InputController.LoadInputValues(this.inputsThisFrameBuffer[i].values);
			}
		}
	}

	public void TickInput(int frame, bool isSkippedFrame)
	{
		if (this.config == null)
		{
			return;
		}
		if (this.IsPaused && this.FrameController.IsLocal)
		{
			return;
		}
		for (int i = 0; i < this.playerReferences.Count; i++)
		{
			PlayerController controller = this.playerReferences[i].Controller;
			if (!(controller == null))
			{
				if (this.localPlayers.ContainsKey(controller.PlayerNum))
				{
					RollbackInput rollbackInput = this.rollbackInputPool.New();
					rollbackInput.playerID = PlayerUtil.GetIntFromPlayerNum(controller.PlayerNum, false);
					if (frame + this.FrameController.rollbackStatus.InputDelayFrames >= this.GameStartInputFrame)
					{
						if (isSkippedFrame)
						{
							this.FrameController.FillSkippedLocalInputFrame(frame, rollbackInput);
						}
						else
						{
							bool tauntsOnly = false;
							if (this.config.uiuxSettings.emotiveStartup && frame + this.FrameController.rollbackStatus.InputDelayFrames < this.AllowGameplayInputsFrame)
							{
								tauntsOnly = true;
							}
							InputValuesSnapshot values = rollbackInput.values;
							if (controller.InputController != null)
							{
								controller.InputController.ReadPlayerInputValues(ref values, tauntsOnly);
							}
						}
					}
					this.localInputsBuffer.Add(rollbackInput);
				}
			}
		}
		this.FrameController.SaveLocalInputs(frame, this.localInputsBuffer, !isSkippedFrame);
		foreach (RollbackInput current in this.localInputsBuffer)
		{
			this.rollbackInputPool.Store(current);
		}
		this.localInputsBuffer.Clear();
	}

	public void TickUpdate()
	{
		if (this.CurrentGameMode != null)
		{
			this.CurrentGameMode.TickUpdate();
		}
	}

	public void TickFrame()
	{
		this.Audio.TickFrame();
		this.PhysicsWorld.ClearFrameDebuggingInfo();
		if (this.config == null)
		{
			return;
		}
		if (!this.StartedGame && this.Frame > this.gameStartFrame)
		{
			this.gameManagerState.gameStarted = true;
			this.events.Broadcast(new GameStartEvent());
		}
		this.Client.GameTickFPSCounter.TickFrame();
		if (this.IsPaused && this.FrameController.IsLocal)
		{
			return;
		}
		this.syncInputs();
		this.playerSpawner.TickFrame();
		for (int i = 0; i < this.playerReferences.Count; i++)
		{
			this.playerReferences[i].TickFrame();
		}
		for (int j = 0; j < this.CharacterControllers.Count; j++)
		{
			PlayerController playerController = this.CharacterControllers[j];
			playerController.TickFrame();
		}
		this.Hits.TickFrame();
		this.grabManager.TickFrame();
		this.Camera.TickFrame();
		this.ComboManager.TickFrame();
		this.Stage.TickFrame();
		foreach (PlayerController current in this.CharacterControllers)
		{
			current.PlayDelayedParticles();
		}
		if (this.DynamicObjects != null)
		{
			this.DynamicObjects.TickFrame();
		}
		if (this.PointAudio != null)
		{
			this.PointAudio.TickTimeDelta(WTime.frameTime);
		}
		this.ObjectPools.TickFrame();
		if (this.CurrentGameMode != null)
		{
			this.CurrentGameMode.TickFrame();
		}
		this.announcements.TickFrame();
		for (int k = 0; k < this.playerReferences.Count; k++)
		{
			if (this.playerReferences[k] != null && !this.playerReferences[k].IsSpectating)
			{
				PlayerController controller = this.playerReferences[k].Controller;
				controller.UpdateDebugText();
			}
		}
		this.FrameController.OnFrameAdvanced();
		this.replaySystem.Tick(this.Frame, this);
	}

	public void Update()
	{
		if (this.config == null)
		{
			return;
		}
		this.generateGlobalDebugText();
	}

	public PlayerController GetPlayerController(PlayerNum playerNum)
	{
		for (int i = 0; i < this.CharacterControllers.Count; i++)
		{
			PlayerController playerController = this.CharacterControllers[i];
			if (playerController.PlayerNum == playerNum && playerController.IsActive)
			{
				return playerController;
			}
		}
		return null;
	}

	public List<PlayerController> GetPlayers()
	{
		return this.CharacterControllers;
	}

	void IRollbackClient.ReportWaiting(double waitingDurationMs)
	{
		this.performanceTracker.RecordWaiting(waitingDurationMs);
	}

	void IRollbackClient.ReportHealth(NetworkHealthReport health)
	{
		this.UI.ReportRollbackHealth(health);
		this.netGraph.ReportHealth(health);
		this.Client.PerformanceDisplay.ReportNetHealth(health);
		this.performanceTracker.RecordPing((float)health.calculatedLatencyMs, this.battleServerAPI.ServerPing);
		this.performanceTracker.RecordSkippedFrames(health.skippedFrames);
	}

	void IRollbackClient.ReportErrors(List<string> errors)
	{
		for (int i = 0; i < errors.Count; i++)
		{
			this.UI.AddDebugTextEvent(errors[i]);
		}
	}

	void IRollbackClient.Halt()
	{
		UnityEngine.Debug.LogError("Halting simulation");
		this.FrameController.SetControlMode(FrameControlMode.Manual);
	}

	public void DestroyCharacter(PlayerController character)
	{
		this.Hits.Unregister(character);
		this.CharacterControllers.Remove(character);
	}

	private void generateGlobalDebugText()
	{
		string text = string.Empty;
		if (this.UI.DebugTextEnabled)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"Display FPS: ",
				this.Client.DisplayFPSCounter.FPS,
				" Game Tick FPS: ",
				this.Client.GameTickFPSCounter.FPS,
				" \n"
			});
			if (this.FrameController != null)
			{
				text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"Frame: ",
					this.FrameController.Frame,
					(!this.IsRollingBack) ? string.Empty : "(rb)",
					"\n"
				});
			}
			string debugString = DebugDraw.Instance.getDebugString();
			if (debugString.Length > 0)
			{
				text = text + "Debug Draw: " + debugString + "\n";
			}
			text = text + WTime.currentTimeMs.ToString() + "\n";
		}
		this.UI.GlobalDebugText.text = text;
	}

	private void OnDrawGizmos()
	{
		this.PhysicsWorld.DrawDebugInfo();
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Grid))
		{
			DebugDraw.Instance.DrawGridGizmos();
		}
	}

	private static RollbackInput _StartGame_m__0()
	{
		return new RollbackInput();
	}
}
