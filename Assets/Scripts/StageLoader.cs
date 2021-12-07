// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StageLoader : MonoBehaviour
{
	public static string GAME_BEGINNING = "StageLoader.GAME_BEGINNING";

	private AsyncOperation unloadResourcesAsync;

	private Action unloadResourcesCallback;

	private IEvents events;

	public StageData stage;

	private GameLoadPayload payload;

	private int stageLoadWeight = 70;

	private int preloadAssetsWeight = 30;

	private bool stageLoadBegun;

	private bool stageLoadComplete;

	private bool startedPreloadingAssets;

	private bool gameStarted;

	private bool isTerminated;

	private List<Action> callbackOnLoad = new List<Action>();

	[Inject]
	public UIPreload3DAssets preload3dAssets
	{
		get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
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
	public AudioManager audioManager
	{
		get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public IUIAdapter uiAdapter
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
	public IUserVideoSettingsModel userVideoSettingsModel
	{
		get;
		set;
	}

	[Inject]
	public IVideoSettingsUtility videoSettingsUtility
	{
		get;
		set;
	}

	[Inject]
	public ICursorManager cursorManager
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
	public IMatchDeepLogging deepLogging
	{
		get;
		set;
	}

	[Inject]
	public IBackgroundLoader backgroundLoader
	{
		get;
		set;
	}

	[Inject]
	public ISoundFileManager soundFileManager
	{
		get;
		set;
	}

	[Inject]
	public StageMusicMap stageMusicMap
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
	public ICharacterDataHelper characterDataHelper
	{
		get;
		set;
	}

	[Inject]
	public IUDPWarming udpWarming
	{
		private get;
		set;
	}

	[Inject]
	public IServerConnectionManager serverManager
	{
		get;
		set;
	}

	private int totalWeight
	{
		get
		{
			return this.stageLoadWeight + this.preloadAssetsWeight;
		}
	}

	public void Initialize(IEvents events, GameLoadPayload gameLoadPayload)
	{
		this.events = events;
		this.payload = gameLoadPayload;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.preload3dAssets.Reset();
		StageData dataByID = this.gameDataManager.StageData.GetDataByID(this.payload.stage);
		events.Broadcast(typeof(CreateGameManagerCommand));
		if (dataByID.stageType == StageType.Random)
		{
			this.stage = this.gameDataManager.StageData.GetDataByID(this.payload.stagePayloadData.lastRandomStage);
		}
		else
		{
			this.stage = dataByID;
		}
		this.audioManager.StopMusic(new Action(this.onMusicStopped), -1f);
	}

	private void clearUnusedSkins()
	{
		List<string> list = new List<string>();
		IEnumerator enumerator = ((IEnumerable)this.payload.players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)enumerator.Current;
				CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(playerSelectionInfo.characterID);
				if (characterDefinition != null)
				{
					SkinDefinition skinDefinition = this.characterDataHelper.GetSkinDefinition(playerSelectionInfo.characterID, playerSelectionInfo.skinKey);
					if (skinDefinition != null && !list.Contains(skinDefinition.uniqueKey))
					{
						list.Add(skinDefinition.uniqueKey);
					}
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
		foreach (SkinDefinition current in this.preload3dAssets.GetDefaultSkins())
		{
			if (!list.Contains(current.uniqueKey))
			{
				list.Add(current.uniqueKey);
			}
		}
		this.skinDataManager.ClearUnused(list);
	}

	public void TerminateGame(Action callback)
	{
		this.isTerminated = true;
		if (!this.isStageLoading())
		{
			callback();
		}
		else
		{
			this.callbackOnLoad.Add(callback);
		}
	}

	private void onMusicStopped()
	{
		this.clearUnusedSkins();
		this.soundFileManager.PreloadBundle(SoundBundleKey.inGame, true);
		this.soundFileManager.PreloadSound(this.stageMusicMap.GetSoundKey(this.stage.stageID));
		this.udpWarming.Reset();
		if (this.gameController.battleServerAPI.IsConnected)
		{
			this.udpWarming.BeginUdpWarm();
		}
		if (this.userVideoSettingsModel.StageQuality == ThreeTierQualityLevel.Low && !string.IsNullOrEmpty(this.stage.lowDetailSceneName))
		{
			this.loadScene(this.stage.lowDetailSceneName);
		}
		else
		{
			this.loadScene(this.stage.sceneName);
		}
	}

	private void loadScene(string sceneName)
	{
		this.stageLoadBegun = true;
		this.stageLoadComplete = false;
		this.sceneController.LoadBattleScene(sceneName, new Action(this._loadScene_m__0));
	}

	private bool isComplete()
	{
		return this.startedPreloadingAssets && this.gameController.preloader.IsComplete && this.backgroundLoader.BakedAnimationDataLoaded && this.udpWarming.IsAllConnectionsReady;
	}

	private void checkCallbacksOnLoad()
	{
		if (this.callbackOnLoad.Count > 0 && !this.isStageLoading())
		{
			List<Action> list = new List<Action>(this.callbackOnLoad);
			this.callbackOnLoad.Clear();
			foreach (Action current in list)
			{
				current();
			}
		}
	}

	private void Update()
	{
		if (this.gameStarted)
		{
			return;
		}
		this.checkCallbacksOnLoad();
		if (!this.isTerminated)
		{
			this.serverManager.ReceiveAllMessages();
			this.udpWarming.Tick();
			this.serverManager.SendAllMessages();
			if (this.isComplete())
			{
				this.onComplete();
			}
			if (this.unloadResourcesAsync != null && this.unloadResourcesAsync.isDone)
			{
				this.unloadResourcesAsync = null;
				Action action = this.unloadResourcesCallback;
				this.unloadResourcesCallback = null;
				action();
			}
		}
	}

	private void unloadUnusedResources(Action callback)
	{
		this.unloadResourcesCallback = callback;
		this.unloadResourcesAsync = Resources.UnloadUnusedAssets();
	}

	private bool isStageLoading()
	{
		return (this.stageLoadBegun && !this.stageLoadComplete) || (this.unloadResourcesAsync != null && this.unloadResourcesAsync.progress < 1f);
	}

	private void onStageLoadComplete()
	{
		this.unloadUnusedResources(new Action(this._onStageLoadComplete_m__1));
	}

	private void startPreloadingAssets()
	{
		this.startedPreloadingAssets = true;
		this.gameController.preloader.Preload(this.payload);
	}

	private void syncCameraEffects()
	{
		Camera[] allCameras = Camera.allCameras;
		for (int i = 0; i < allCameras.Length; i++)
		{
			Camera camera = allCameras[i];
			this.videoSettingsUtility.ApplyToCamera(camera);
			this.fixUnityMouseEventAllocationBug(camera);
		}
	}

	private void fixUnityMouseEventAllocationBug(Camera camera)
	{
		camera.eventMask = 0;
		GUILayer component = camera.gameObject.GetComponent<GUILayer>();
		if (component == null)
		{
			camera.gameObject.AddComponent<GUILayer>();
		}
	}

	private void startDeepLogging()
	{
		this.deepLogging.BeginMatch(this.gameController.battleServerAPI.MatchID.ToString(), "todo");
	}

	private void onComplete()
	{
		ProfilingUtil.BeginTimer();
		this.syncCameraEffects();
		this.startDeepLogging();
		this.gameStarted = true;
		this.events.Broadcast(new SetupGameCommand(this.payload, -1f));
		this.uiAdapter.PreloadScreen(ScreenType.BattleGUI);
		this.cursorManager.SetDisplay(false);
		this.signalBus.Dispatch(StageLoader.GAME_BEGINNING);
		UnityEngine.Object.Destroy(base.gameObject);
		GC.Collect();
		this.gameController.ClientReadyToStartGame();
		ProfilingUtil.EndTimer("Starting game");
	}

	private void _loadScene_m__0()
	{
		this.stageLoadComplete = true;
		if (!this.isTerminated)
		{
			this.onStageLoadComplete();
		}
	}

	private void _onStageLoadComplete_m__1()
	{
		this.startPreloadingAssets();
	}
}
