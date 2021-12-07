using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using DevConsole;
using strange.extensions.injector.api;
using UnityEngine;

// Token: 0x0200046A RID: 1130
public class GameLoader
{
	// Token: 0x060017FB RID: 6139 RVA: 0x0007F6D8 File Offset: 0x0007DAD8
	public GameLoader(SystemBoot boot, Transform devUIContainer)
	{
		GC.Collect();
		GC.Collect();
		ProfilingUtil.ReportMemory();
		this.devUIContainer = devUIContainer;
		this.boot = boot;
		this.STARTUP();
	}

	// Token: 0x060017FC RID: 6140 RVA: 0x0007F724 File Offset: 0x0007DB24
	private void STARTUP()
	{
		this.bootSteps.Add(new Action<Action>(this.initialize));
		this.bootSteps.Add(new Action<Action>(this.systemConfigure));
		this.bootSteps.Add(new Action<Action>(this.systemStartup));
		this.bootSteps.Add(new Action<Action>(this.preload));
		this.recursiveStartup();
	}

	// Token: 0x060017FD RID: 6141 RVA: 0x0007F794 File Offset: 0x0007DB94
	private void recursiveStartup()
	{
		if (this.bootSteps.Count == 0)
		{
			this.onStartupComplete();
		}
		else
		{
			Action<Action> action = this.bootSteps[0];
			this.bootSteps.RemoveAt(0);
			action(delegate
			{
				this.recursiveStartup();
			});
		}
	}

	// Token: 0x060017FE RID: 6142 RVA: 0x0007F7E7 File Offset: 0x0007DBE7
	private void initialize(Action callback)
	{
		WTime.Startup();
		this.splashScreenDisplayTimer.Reset();
		this.splashScreenDisplayTimer.Start();
		callback();
	}

	// Token: 0x060017FF RID: 6143 RVA: 0x0007F80A File Offset: 0x0007DC0A
	private void systemConfigure(Action callback)
	{
		new GameLoader.SystemConfigure(this, callback);
	}

	// Token: 0x06001800 RID: 6144 RVA: 0x0007F814 File Offset: 0x0007DC14
	private void systemStartup(Action callback)
	{
		new GameLoader.SystemStartup(this, callback);
	}

	// Token: 0x06001801 RID: 6145 RVA: 0x0007F81E File Offset: 0x0007DC1E
	private void preload(Action callback)
	{
		new GameLoader.Preload(this, this.injector, callback);
	}

	// Token: 0x06001802 RID: 6146 RVA: 0x0007F830 File Offset: 0x0007DC30
	private void onStartupComplete()
	{
		GC.Collect();
		GC.Collect();
		if (this.startupCanvas != null)
		{
			UnityEngine.Object.DestroyImmediate(this.startupCanvas.gameObject);
		}
		if (this.boot != null)
		{
			this.boot.gameObject.SetActive(false);
		}
		this.splashScreenDisplayTimer.Stop();
		UnityEngine.Debug.LogFormat("[GameLoader TOTAL] FINAL LOAD TIME: {0}s", new object[]
		{
			this.splashScreenDisplayTimer.Elapsed.TotalSeconds
		});
		GC.Collect();
		GC.Collect();
		SystemBoot.OnStartupComplete();
		GC.Collect();
		GC.Collect();
		ProfilingUtil.ReportMemory();
		this.client.StartupComplete();
	}

	// Token: 0x06001803 RID: 6147 RVA: 0x0007F8E9 File Offset: 0x0007DCE9
	public static GameEnvironmentData LoadGameEnvironmentData()
	{
		return Resources.Load<GameEnvironmentData>(GameLoader.gameEnvironmentDataPath);
	}

	// Token: 0x04001252 RID: 4690
	private static string gameEnvironmentDataPath = "Config/GameEnvironmentData";

	// Token: 0x04001253 RID: 4691
	private GameObject clientPrefab;

	// Token: 0x04001254 RID: 4692
	private StartupCanvas startupCanvas;

	// Token: 0x04001255 RID: 4693
	private Transform devUIContainer;

	// Token: 0x04001256 RID: 4694
	private ConfigData config;

	// Token: 0x04001257 RID: 4695
	private GameEnvironmentData environmentData;

	// Token: 0x04001258 RID: 4696
	private DeveloperConfig devConfig;

	// Token: 0x04001259 RID: 4697
	private SystemBoot boot;

	// Token: 0x0400125A RID: 4698
	private AsyncResourceLoader asyncResourceLoader;

	// Token: 0x0400125B RID: 4699
	public Stopwatch splashScreenDisplayTimer = new Stopwatch();

	// Token: 0x0400125C RID: 4700
	private GameClient client;

	// Token: 0x0400125D RID: 4701
	private IDevConsole devConsole;

	// Token: 0x0400125E RID: 4702
	private IDependencyInjection injector;

	// Token: 0x0400125F RID: 4703
	private List<Action<Action>> bootSteps = new List<Action<Action>>();

	// Token: 0x0200046B RID: 1131
	private class SystemConfigure
	{
		// Token: 0x06001806 RID: 6150 RVA: 0x0007F90C File Offset: 0x0007DD0C
		public SystemConfigure(GameLoader gameLoader, Action callback)
		{
			this.gameLoader = gameLoader;
			this.boot = gameLoader.boot;
			this.timeTracker = new GameLoader.TimeTracker(gameLoader.splashScreenDisplayTimer);
			gameLoader.asyncResourceLoader = new AsyncResourceLoader();
			this.timeTracker.Add("Load Startup Canvas", new Action(this.loadStartupCanvas));
			this.timeTracker.Add("Load Game Client File", new Action<Action>(this.loadGameClientFile));
			this.timeTracker.Add("Load Config File", new Action<Action>(this.loadConfigFile));
			this.timeTracker.Add("Load Env File", new Action<Action>(this.loadEnvFile));
			this.timeTracker.Add("Load Dev Config", new Action(this.loadDevConfig));
			GameLoader.TimeTracker timeTracker = this.timeTracker;
			string name = "Load DLLS";
			if (GameLoader.SystemConfigure.f__mg_cache0 == null)
			{
				GameLoader.SystemConfigure.f__mg_cache0 = new Action(DllLoader.LoadNativeDLLs);
			}
			timeTracker.Add(name, GameLoader.SystemConfigure.f__mg_cache0);
			this.timeTracker.Add("Clear Dev UI", new Action(this.clearDevUI));
			this.timeTracker.Add("Configure Master Context", new Action(this.configureDIContext));
			this.timeTracker.Add("Create DevConsole", new Action(this.loadDevConsole));
			this.timeTracker.Add("Create GameClient", new Action(this.loadGameClient));
			this.timeTracker.Execute(callback);
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x0007FA84 File Offset: 0x0007DE84
		private void checkUnityVersion()
		{
			bool flag = false;
			if (Application.unityVersion == "2018.2.21f1")
			{
				flag = true;
			}
			if (!flag)
			{
				throw new Exception("Invalid Unity version: must be using 2018.1.9f2 (currently on " + Application.unityVersion + ")");
			}
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x0007FACC File Offset: 0x0007DECC
		private void loadStartupCanvas()
		{
			StartupCanvas original = Resources.Load<StartupCanvas>("Prefabs/StartupCanvas");
			this.gameLoader.startupCanvas = UnityEngine.Object.Instantiate<StartupCanvas>(original);
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x0007FAF8 File Offset: 0x0007DEF8
		private void loadGameClientFile(Action callback)
		{
			this.gameLoader.asyncResourceLoader.Load<GameObject>("Prefabs/GameClient", delegate(GameObject clientPrefab)
			{
				this.gameLoader.clientPrefab = clientPrefab;
				callback();
			});
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x0007FB3C File Offset: 0x0007DF3C
		private void loadConfigFile(Action callback)
		{
			this.gameLoader.asyncResourceLoader.Load<ConfigData>("Config/Config", delegate(ConfigData configData)
			{
				this.gameLoader.config = configData;
				callback();
			});
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x0007FB7E File Offset: 0x0007DF7E
		private void loadDevConfig()
		{
			this.gameLoader.devConfig = new DeveloperConfigLoader().config;
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x0007FB98 File Offset: 0x0007DF98
		private void loadEnvFile(Action callback)
		{
			this.gameLoader.asyncResourceLoader.Load<GameEnvironmentData>(GameLoader.gameEnvironmentDataPath, delegate(GameEnvironmentData environmentData)
			{
				this.gameLoader.environmentData = environmentData;
				callback();
			});
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x0007FBDA File Offset: 0x0007DFDA
		private void clearDevUI()
		{
			if (this.gameLoader.devUIContainer != null)
			{
				UnityEngine.Object.DestroyImmediate(this.gameLoader.devUIContainer.gameObject);
			}
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x0007FC08 File Offset: 0x0007E008
		private void configureDIContext()
		{
			MasterContext.InitContext initContext = new MasterContext.InitContext(this.gameLoader.config, this.gameLoader.environmentData, this.gameLoader.devConfig);
			initContext.offlineModeDetector = new OfflineModeDetector(initContext);
			this.masterContext = new MasterContext(this.boot, initContext);
			ICrossContextInjectionBinder injectionBinder = this.masterContext.injectionBinder;
			injectionBinder.Bind<ConfigData>().ToValue(this.gameLoader.config);
			injectionBinder.Bind<GameEnvironmentData>().ToValue(this.gameLoader.environmentData);
			injectionBinder.Bind<DeveloperConfig>().ToValue(this.gameLoader.devConfig);
			injectionBinder.Bind<IOfflineModeDetector>().ToValue(initContext.offlineModeDetector);
			injectionBinder.Bind<IResourceLoader>().ToValue(this.gameLoader.asyncResourceLoader);
			this.gameLoader.injector = this.masterContext.injectionBinder.GetInstance<IDependencyInjection>();
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x0007FCF0 File Offset: 0x0007E0F0
		private void loadDevConsole()
		{
			GameObject original = (GameObject)Resources.Load("GUI/Developer/WavedashConsole");
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			this.gameLoader.devConsole = gameObject.GetComponent<WavedashConsole>();
			this.gameLoader.injector.BindToValue<IDevConsole>(this.gameLoader.devConsole);
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x0007FD40 File Offset: 0x0007E140
		private void loadGameClient()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.gameLoader.clientPrefab);
			this.gameLoader.client = gameObject.GetComponent<GameClient>();
			this.gameLoader.client.gameObject.name = "GameClient";
			if (this.gameLoader.client == null)
			{
				throw new UnityException("[GameLoader] Unable to find GameClient component on client prefab.");
			}
			this.gameLoader.injector.BindToValue<IGameClient>(this.gameLoader.client);
		}

		// Token: 0x04001260 RID: 4704
		private MasterContext masterContext;

		// Token: 0x04001261 RID: 4705
		private SystemBoot boot;

		// Token: 0x04001262 RID: 4706
		private GameLoader gameLoader;

		// Token: 0x04001263 RID: 4707
		private GameLoader.TimeTracker timeTracker;

		// Token: 0x04001264 RID: 4708
		[CompilerGenerated]
		private static Action f__mg_cache0;
	}

	// Token: 0x0200046C RID: 1132
	private class SystemStartup
	{
		// Token: 0x06001811 RID: 6161 RVA: 0x0007FE38 File Offset: 0x0007E238
		public SystemStartup(GameLoader gameLoader, Action callback)
		{
			this.gameLoader = gameLoader;
			this.timeTracker = new GameLoader.TimeTracker(gameLoader.splashScreenDisplayTimer);
			this.timeTracker.Add("BakedAnimationDataLoader", new Action(this.loadBakedAnimations));
			this.timeTracker.Add("Start GameDataManager", new Action(this.loadGameDataManager));
			this.timeTracker.Add("Inject Dev Console", new Action(this.injectDevConsole));
			this.timeTracker.Add("Inject GameClient", new Action(this.injectGameClient));
			this.timeTracker.Add("Start GameClient", new Action(this.startGameClient));
			this.timeTracker.Add("Start Debug Tools", new Action(this.startDebugTools));
			this.timeTracker.Execute(callback);
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x0007FF17 File Offset: 0x0007E317
		private void loadBakedAnimations()
		{
			BakedAnimationDataLoader.LoadResourcesSync(this.gameLoader.environmentData);
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x0007FF29 File Offset: 0x0007E329
		private void loadGameDataManager()
		{
			this.gameLoader.injector.GetInstance<GameDataManager>().Initialize(this.gameLoader.config, this.gameLoader.environmentData);
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x0007FF56 File Offset: 0x0007E356
		private void injectDevConsole()
		{
			this.gameLoader.injector.Inject(this.gameLoader.devConsole);
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x0007FF73 File Offset: 0x0007E373
		private void injectGameClient()
		{
			this.gameLoader.injector.Inject(this.gameLoader.client);
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x0007FF90 File Offset: 0x0007E390
		private void startGameClient()
		{
			this.gameLoader.client.Init();
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x0007FFA2 File Offset: 0x0007E3A2
		private void startDebugTools()
		{
			this.gameLoader.client.InitDebug();
		}

		// Token: 0x04001265 RID: 4709
		private GameLoader gameLoader;

		// Token: 0x04001266 RID: 4710
		private GameLoader.TimeTracker timeTracker;
	}

	// Token: 0x0200046D RID: 1133
	private class Preload
	{
		// Token: 0x06001818 RID: 6168 RVA: 0x0007FFB4 File Offset: 0x0007E3B4
		public Preload(GameLoader gameLoader, IDependencyInjection injector, Action callback)
		{
			this.callback = callback;
			this.injector = injector;
			this.timeTracker = new GameLoader.TimeTracker(gameLoader.splashScreenDisplayTimer);
			this.addLoad<IUserCharacterEquippedModel>("Character Equipment");
			this.addLoad<IUserTauntsModel>("Taunt Equipment");
			this.addLoad<IUserGlobalEquippedModel>("Global Equipment");
			this.addLoad<IMainMenuAPI>("MainMenuCharacter");
			this.addLoad<UIPreload3DAssets>("UI Characters");
			this.addLoad<ISceneController>("UI Scenes");
			this.addLoad<IUICSSSceneCharacterManager>("Character Select");
			this.recursiveStartupLoad();
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x00080044 File Offset: 0x0007E444
		private void addLoad<T>(string name) where T : IStartupLoader
		{
			this.list.Add(new GameLoader.Preload.StartupLoadItem(name, this.injector.GetInstance<T>()));
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x00080068 File Offset: 0x0007E468
		private void recursiveStartupLoad()
		{
			foreach (GameLoader.Preload.StartupLoadItem startupLoadItem in this.list)
			{
				this.timeTracker.Add(startupLoadItem.name, new Action<Action>(startupLoadItem.obj.StartupLoad));
			}
			this.timeTracker.Execute(this.callback);
		}

		// Token: 0x04001267 RID: 4711
		private List<GameLoader.Preload.StartupLoadItem> list = new List<GameLoader.Preload.StartupLoadItem>();

		// Token: 0x04001268 RID: 4712
		private Action callback;

		// Token: 0x04001269 RID: 4713
		private IDependencyInjection injector;

		// Token: 0x0400126A RID: 4714
		private GameLoader.TimeTracker timeTracker;

		// Token: 0x0200046E RID: 1134
		private struct StartupLoadItem
		{
			// Token: 0x0600181B RID: 6171 RVA: 0x000800F4 File Offset: 0x0007E4F4
			public StartupLoadItem(string name, IStartupLoader obj)
			{
				this.name = name;
				this.obj = obj;
			}

			// Token: 0x0400126B RID: 4715
			public string name;

			// Token: 0x0400126C RID: 4716
			public IStartupLoader obj;
		}
	}

	// Token: 0x0200046F RID: 1135
	private class TimeTracker
	{
		// Token: 0x0600181C RID: 6172 RVA: 0x00080104 File Offset: 0x0007E504
		public TimeTracker(Stopwatch globalTimer)
		{
			this.globalTimer = globalTimer;
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x00080129 File Offset: 0x0007E529
		public void Add(string name, Action fn)
		{
			this.loadStepList.Add(new GameLoader.TimeTracker.LoadStep(name, fn, null));
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x0008013E File Offset: 0x0007E53E
		public void Add(string name, Action<Action> fn)
		{
			this.loadStepList.Add(new GameLoader.TimeTracker.LoadStep(name, null, fn));
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x00080154 File Offset: 0x0007E554
		public void Execute(Action callback)
		{
			if (this.activeExecution == null)
			{
				GameLoader.TimeTracker.CoroutineHelperMonoBehaviour behaviour = new GameObject("TimeTracker.Execute").AddComponent<GameLoader.TimeTracker.CoroutineHelperMonoBehaviour>();
				UnityEngine.Object.DontDestroyOnLoad(behaviour);
				this.activeExecution = behaviour.StartCoroutine(this.performLoadSteps(delegate
				{
					this.activeExecution = null;
					UnityEngine.Object.Destroy(behaviour.gameObject);
					callback();
				}));
			}
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x000801D0 File Offset: 0x0007E5D0
		private IEnumerator performLoadSteps(Action callback)
		{
			float totalSecondsBetweenSteps = 0f;
			while (this.loadStepList.Count > 0)
			{
				GameLoader.TimeTracker.LoadStep currentStep = this.loadStepList[0];
				this.loadStepList.RemoveAt(0);
				totalSecondsBetweenSteps += Time.deltaTime;
				this.start();
				if (currentStep.fn != null)
				{
					currentStep.fn();
				}
				else
				{
					ReferenceValue<bool> stepComplete = new ReferenceValue<bool>(false);
					currentStep.asyncFn(delegate
					{
						stepComplete.Value = true;
					});
					while (!stepComplete.Value)
					{
						totalSecondsBetweenSteps = 0f;
						yield return null;
					}
				}
				this.stop(currentStep.name);
				if (totalSecondsBetweenSteps > 0.05f)
				{
					totalSecondsBetweenSteps = 0f;
					yield return null;
				}
			}
			callback();
			yield break;
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x000801F2 File Offset: 0x0007E5F2
		private void start()
		{
			this.featureTimer.Reset();
			this.featureTimer.Start();
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x0008020C File Offset: 0x0007E60C
		private void stop(string message)
		{
			this.featureTimer.Stop();
			if (this.featureTimer.Elapsed.TotalSeconds > 0.009999999776482582)
			{
				UnityEngine.Debug.LogFormat("[GameLoader] {0}: {1}ms --- total: {2}s", new object[]
				{
					message,
					this.featureTimer.Elapsed.TotalSeconds * 1000.0,
					this.globalTimer.Elapsed.TotalSeconds
				});
			}
		}

		// Token: 0x0400126D RID: 4717
		private Stopwatch featureTimer = new Stopwatch();

		// Token: 0x0400126E RID: 4718
		private Stopwatch globalTimer;

		// Token: 0x0400126F RID: 4719
		private List<GameLoader.TimeTracker.LoadStep> loadStepList = new List<GameLoader.TimeTracker.LoadStep>();

		// Token: 0x04001270 RID: 4720
		private Coroutine activeExecution;

		// Token: 0x02000470 RID: 1136
		private class CoroutineHelperMonoBehaviour : MonoBehaviour
		{
		}

		// Token: 0x02000471 RID: 1137
		private struct LoadStep
		{
			// Token: 0x06001824 RID: 6180 RVA: 0x000802A1 File Offset: 0x0007E6A1
			public LoadStep(string name, Action fn, Action<Action> asyncFn)
			{
				this.name = name;
				this.fn = fn;
				this.asyncFn = asyncFn;
			}

			// Token: 0x04001271 RID: 4721
			public string name;

			// Token: 0x04001272 RID: 4722
			public Action fn;

			// Token: 0x04001273 RID: 4723
			public Action<Action> asyncFn;
		}
	}
}
