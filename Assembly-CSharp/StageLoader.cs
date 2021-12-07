using System;
using System.Collections;
using System.Collections.Generic;
using network;
using UnityEngine;

// Token: 0x02000492 RID: 1170
public class StageLoader : MonoBehaviour
{
	// Token: 0x1700053B RID: 1339
	// (get) Token: 0x06001969 RID: 6505 RVA: 0x00084978 File Offset: 0x00082D78
	// (set) Token: 0x0600196A RID: 6506 RVA: 0x00084980 File Offset: 0x00082D80
	[Inject]
	public UIPreload3DAssets preload3dAssets { get; set; }

	// Token: 0x1700053C RID: 1340
	// (get) Token: 0x0600196B RID: 6507 RVA: 0x00084989 File Offset: 0x00082D89
	// (set) Token: 0x0600196C RID: 6508 RVA: 0x00084991 File Offset: 0x00082D91
	[Inject]
	public ISkinDataManager skinDataManager { get; set; }

	// Token: 0x1700053D RID: 1341
	// (get) Token: 0x0600196D RID: 6509 RVA: 0x0008499A File Offset: 0x00082D9A
	// (set) Token: 0x0600196E RID: 6510 RVA: 0x000849A2 File Offset: 0x00082DA2
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x1700053E RID: 1342
	// (get) Token: 0x0600196F RID: 6511 RVA: 0x000849AB File Offset: 0x00082DAB
	// (set) Token: 0x06001970 RID: 6512 RVA: 0x000849B3 File Offset: 0x00082DB3
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x1700053F RID: 1343
	// (get) Token: 0x06001971 RID: 6513 RVA: 0x000849BC File Offset: 0x00082DBC
	// (set) Token: 0x06001972 RID: 6514 RVA: 0x000849C4 File Offset: 0x00082DC4
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x06001973 RID: 6515 RVA: 0x000849CD File Offset: 0x00082DCD
	// (set) Token: 0x06001974 RID: 6516 RVA: 0x000849D5 File Offset: 0x00082DD5
	[Inject]
	public IUIAdapter uiAdapter { get; set; }

	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x06001975 RID: 6517 RVA: 0x000849DE File Offset: 0x00082DDE
	// (set) Token: 0x06001976 RID: 6518 RVA: 0x000849E6 File Offset: 0x00082DE6
	[Inject]
	public ISceneController sceneController { private get; set; }

	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x06001977 RID: 6519 RVA: 0x000849EF File Offset: 0x00082DEF
	// (set) Token: 0x06001978 RID: 6520 RVA: 0x000849F7 File Offset: 0x00082DF7
	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel { get; set; }

	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x06001979 RID: 6521 RVA: 0x00084A00 File Offset: 0x00082E00
	// (set) Token: 0x0600197A RID: 6522 RVA: 0x00084A08 File Offset: 0x00082E08
	[Inject]
	public IVideoSettingsUtility videoSettingsUtility { get; set; }

	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x0600197B RID: 6523 RVA: 0x00084A11 File Offset: 0x00082E11
	// (set) Token: 0x0600197C RID: 6524 RVA: 0x00084A19 File Offset: 0x00082E19
	[Inject]
	public ICursorManager cursorManager { get; set; }

	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x0600197D RID: 6525 RVA: 0x00084A22 File Offset: 0x00082E22
	// (set) Token: 0x0600197E RID: 6526 RVA: 0x00084A2A File Offset: 0x00082E2A
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x0600197F RID: 6527 RVA: 0x00084A33 File Offset: 0x00082E33
	// (set) Token: 0x06001980 RID: 6528 RVA: 0x00084A3B File Offset: 0x00082E3B
	[Inject]
	public IMatchDeepLogging deepLogging { get; set; }

	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x06001981 RID: 6529 RVA: 0x00084A44 File Offset: 0x00082E44
	// (set) Token: 0x06001982 RID: 6530 RVA: 0x00084A4C File Offset: 0x00082E4C
	[Inject]
	public IBackgroundLoader backgroundLoader { get; set; }

	// Token: 0x17000548 RID: 1352
	// (get) Token: 0x06001983 RID: 6531 RVA: 0x00084A55 File Offset: 0x00082E55
	// (set) Token: 0x06001984 RID: 6532 RVA: 0x00084A5D File Offset: 0x00082E5D
	[Inject]
	public ISoundFileManager soundFileManager { get; set; }

	// Token: 0x17000549 RID: 1353
	// (get) Token: 0x06001985 RID: 6533 RVA: 0x00084A66 File Offset: 0x00082E66
	// (set) Token: 0x06001986 RID: 6534 RVA: 0x00084A6E File Offset: 0x00082E6E
	[Inject]
	public StageMusicMap stageMusicMap { get; set; }

	// Token: 0x1700054A RID: 1354
	// (get) Token: 0x06001987 RID: 6535 RVA: 0x00084A77 File Offset: 0x00082E77
	// (set) Token: 0x06001988 RID: 6536 RVA: 0x00084A7F File Offset: 0x00082E7F
	[Inject]
	public ICharacterLists characterLists { get; set; }

	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x06001989 RID: 6537 RVA: 0x00084A88 File Offset: 0x00082E88
	// (set) Token: 0x0600198A RID: 6538 RVA: 0x00084A90 File Offset: 0x00082E90
	[Inject]
	public ICharacterDataHelper characterDataHelper { get; set; }

	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x0600198B RID: 6539 RVA: 0x00084A99 File Offset: 0x00082E99
	// (set) Token: 0x0600198C RID: 6540 RVA: 0x00084AA1 File Offset: 0x00082EA1
	[Inject]
	public IUDPWarming udpWarming { private get; set; }

	// Token: 0x1700054D RID: 1357
	// (get) Token: 0x0600198D RID: 6541 RVA: 0x00084AAA File Offset: 0x00082EAA
	// (set) Token: 0x0600198E RID: 6542 RVA: 0x00084AB2 File Offset: 0x00082EB2
	[Inject]
	public IServerConnectionManager serverManager { get; set; }

	// Token: 0x1700054E RID: 1358
	// (get) Token: 0x0600198F RID: 6543 RVA: 0x00084ABB File Offset: 0x00082EBB
	private int totalWeight
	{
		get
		{
			return this.stageLoadWeight + this.preloadAssetsWeight;
		}
	}

	// Token: 0x06001990 RID: 6544 RVA: 0x00084ACC File Offset: 0x00082ECC
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

	// Token: 0x06001991 RID: 6545 RVA: 0x00084B84 File Offset: 0x00082F84
	private void clearUnusedSkins()
	{
		List<string> list = new List<string>();
		IEnumerator enumerator = ((IEnumerable)this.payload.players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)obj;
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
		foreach (SkinDefinition skinDefinition2 in this.preload3dAssets.GetDefaultSkins())
		{
			if (!list.Contains(skinDefinition2.uniqueKey))
			{
				list.Add(skinDefinition2.uniqueKey);
			}
		}
		this.skinDataManager.ClearUnused(list);
	}

	// Token: 0x06001992 RID: 6546 RVA: 0x00084CC8 File Offset: 0x000830C8
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

	// Token: 0x06001993 RID: 6547 RVA: 0x00084CF4 File Offset: 0x000830F4
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

	// Token: 0x06001994 RID: 6548 RVA: 0x00084DAD File Offset: 0x000831AD
	private void loadScene(string sceneName)
	{
		this.stageLoadBegun = true;
		this.stageLoadComplete = false;
		this.sceneController.LoadBattleScene(sceneName, delegate
		{
			this.stageLoadComplete = true;
			if (!this.isTerminated)
			{
				this.onStageLoadComplete();
			}
		});
	}

	// Token: 0x06001995 RID: 6549 RVA: 0x00084DD5 File Offset: 0x000831D5
	private bool isComplete()
	{
		return this.startedPreloadingAssets && this.gameController.preloader.IsComplete && this.backgroundLoader.BakedAnimationDataLoaded && this.udpWarming.IsAllConnectionsReady;
	}

	// Token: 0x06001996 RID: 6550 RVA: 0x00084E18 File Offset: 0x00083218
	private void checkCallbacksOnLoad()
	{
		if (this.callbackOnLoad.Count > 0 && !this.isStageLoading())
		{
			List<Action> list = new List<Action>(this.callbackOnLoad);
			this.callbackOnLoad.Clear();
			foreach (Action action in list)
			{
				action();
			}
		}
	}

	// Token: 0x06001997 RID: 6551 RVA: 0x00084EA4 File Offset: 0x000832A4
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

	// Token: 0x06001998 RID: 6552 RVA: 0x00084F36 File Offset: 0x00083336
	private void unloadUnusedResources(Action callback)
	{
		this.unloadResourcesCallback = callback;
		this.unloadResourcesAsync = Resources.UnloadUnusedAssets();
	}

	// Token: 0x06001999 RID: 6553 RVA: 0x00084F4A File Offset: 0x0008334A
	private bool isStageLoading()
	{
		return (this.stageLoadBegun && !this.stageLoadComplete) || (this.unloadResourcesAsync != null && this.unloadResourcesAsync.progress < 1f);
	}

	// Token: 0x0600199A RID: 6554 RVA: 0x00084F87 File Offset: 0x00083387
	private void onStageLoadComplete()
	{
		this.unloadUnusedResources(delegate
		{
			this.startPreloadingAssets();
		});
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x00084F9B File Offset: 0x0008339B
	private void startPreloadingAssets()
	{
		this.startedPreloadingAssets = true;
		this.gameController.preloader.Preload(this.payload);
	}

	// Token: 0x0600199C RID: 6556 RVA: 0x00084FBC File Offset: 0x000833BC
	private void syncCameraEffects()
	{
		foreach (Camera camera in Camera.allCameras)
		{
			this.videoSettingsUtility.ApplyToCamera(camera);
			this.fixUnityMouseEventAllocationBug(camera);
		}
	}

	// Token: 0x0600199D RID: 6557 RVA: 0x00084FFC File Offset: 0x000833FC
	private void fixUnityMouseEventAllocationBug(Camera camera)
	{
		camera.eventMask = 0;
		GUILayer component = camera.gameObject.GetComponent<GUILayer>();
		if (component == null)
		{
			camera.gameObject.AddComponent<GUILayer>();
		}
	}

	// Token: 0x0600199E RID: 6558 RVA: 0x00085034 File Offset: 0x00083434
	private void startDeepLogging()
	{
		this.deepLogging.BeginMatch(this.gameController.battleServerAPI.MatchID.ToString(), "todo");
	}

	// Token: 0x0600199F RID: 6559 RVA: 0x00085070 File Offset: 0x00083470
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

	// Token: 0x04001320 RID: 4896
	public static string GAME_BEGINNING = "StageLoader.GAME_BEGINNING";

	// Token: 0x04001334 RID: 4916
	private AsyncOperation unloadResourcesAsync;

	// Token: 0x04001335 RID: 4917
	private Action unloadResourcesCallback;

	// Token: 0x04001336 RID: 4918
	private IEvents events;

	// Token: 0x04001337 RID: 4919
	public StageData stage;

	// Token: 0x04001338 RID: 4920
	private GameLoadPayload payload;

	// Token: 0x04001339 RID: 4921
	private int stageLoadWeight = 70;

	// Token: 0x0400133A RID: 4922
	private int preloadAssetsWeight = 30;

	// Token: 0x0400133B RID: 4923
	private bool stageLoadBegun;

	// Token: 0x0400133C RID: 4924
	private bool stageLoadComplete;

	// Token: 0x0400133D RID: 4925
	private bool startedPreloadingAssets;

	// Token: 0x0400133E RID: 4926
	private bool gameStarted;

	// Token: 0x0400133F RID: 4927
	private bool isTerminated;

	// Token: 0x04001340 RID: 4928
	private List<Action> callbackOnLoad = new List<Action>();
}
