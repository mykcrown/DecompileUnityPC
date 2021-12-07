// Decompile from assembly: Assembly-CSharp.dll

using DevConsole;
using strange.extensions.injector.api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameLoader
{
	private class SystemConfigure
	{
		private sealed class _loadGameClientFile_c__AnonStorey0
		{
			internal Action callback;

			internal GameLoader.SystemConfigure _this;

			internal void __m__0(GameObject clientPrefab)
			{
				this._this.gameLoader.clientPrefab = clientPrefab;
				this.callback();
			}
		}

		private sealed class _loadConfigFile_c__AnonStorey1
		{
			internal Action callback;

			internal GameLoader.SystemConfigure _this;

			internal void __m__0(ConfigData configData)
			{
				this._this.gameLoader.config = configData;
				this.callback();
			}
		}

		private sealed class _loadEnvFile_c__AnonStorey2
		{
			internal Action callback;

			internal GameLoader.SystemConfigure _this;

			internal void __m__0(GameEnvironmentData environmentData)
			{
				this._this.gameLoader.environmentData = environmentData;
				this.callback();
			}
		}

		private MasterContext masterContext;

		private SystemBoot boot;

		private GameLoader gameLoader;

		private GameLoader.TimeTracker timeTracker;

		private static Action __f__mg_cache0;

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
			GameLoader.TimeTracker arg_E9_0 = this.timeTracker;
			string arg_E9_1 = "Load DLLS";
			if (GameLoader.SystemConfigure.__f__mg_cache0 == null)
			{
				GameLoader.SystemConfigure.__f__mg_cache0 = new Action(DllLoader.LoadNativeDLLs);
			}
			arg_E9_0.Add(arg_E9_1, GameLoader.SystemConfigure.__f__mg_cache0);
			this.timeTracker.Add("Clear Dev UI", new Action(this.clearDevUI));
			this.timeTracker.Add("Configure Master Context", new Action(this.configureDIContext));
			this.timeTracker.Add("Create DevConsole", new Action(this.loadDevConsole));
			this.timeTracker.Add("Create GameClient", new Action(this.loadGameClient));
			this.timeTracker.Execute(callback);
		}

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

		private void loadStartupCanvas()
		{
			StartupCanvas original = Resources.Load<StartupCanvas>("Prefabs/StartupCanvas");
			this.gameLoader.startupCanvas = UnityEngine.Object.Instantiate<StartupCanvas>(original);
		}

		private void loadGameClientFile(Action callback)
		{
			GameLoader.SystemConfigure._loadGameClientFile_c__AnonStorey0 _loadGameClientFile_c__AnonStorey = new GameLoader.SystemConfigure._loadGameClientFile_c__AnonStorey0();
			_loadGameClientFile_c__AnonStorey.callback = callback;
			_loadGameClientFile_c__AnonStorey._this = this;
			this.gameLoader.asyncResourceLoader.Load<GameObject>("Prefabs/GameClient", new Action<GameObject>(_loadGameClientFile_c__AnonStorey.__m__0));
		}

		private void loadConfigFile(Action callback)
		{
			GameLoader.SystemConfigure._loadConfigFile_c__AnonStorey1 _loadConfigFile_c__AnonStorey = new GameLoader.SystemConfigure._loadConfigFile_c__AnonStorey1();
			_loadConfigFile_c__AnonStorey.callback = callback;
			_loadConfigFile_c__AnonStorey._this = this;
			this.gameLoader.asyncResourceLoader.Load<ConfigData>("Config/Config", new Action<ConfigData>(_loadConfigFile_c__AnonStorey.__m__0));
		}

		private void loadDevConfig()
		{
			this.gameLoader.devConfig = new DeveloperConfigLoader().config;
		}

		private void loadEnvFile(Action callback)
		{
			GameLoader.SystemConfigure._loadEnvFile_c__AnonStorey2 _loadEnvFile_c__AnonStorey = new GameLoader.SystemConfigure._loadEnvFile_c__AnonStorey2();
			_loadEnvFile_c__AnonStorey.callback = callback;
			_loadEnvFile_c__AnonStorey._this = this;
			this.gameLoader.asyncResourceLoader.Load<GameEnvironmentData>(GameLoader.gameEnvironmentDataPath, new Action<GameEnvironmentData>(_loadEnvFile_c__AnonStorey.__m__0));
		}

		private void clearDevUI()
		{
			if (this.gameLoader.devUIContainer != null)
			{
				UnityEngine.Object.DestroyImmediate(this.gameLoader.devUIContainer.gameObject);
			}
		}

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

		private void loadDevConsole()
		{
			GameObject original = (GameObject)Resources.Load("GUI/Developer/WavedashConsole");
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			this.gameLoader.devConsole = gameObject.GetComponent<WavedashConsole>();
			this.gameLoader.injector.BindToValue<IDevConsole>(this.gameLoader.devConsole);
		}

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
	}

	private class SystemStartup
	{
		private GameLoader gameLoader;

		private GameLoader.TimeTracker timeTracker;

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

		private void loadBakedAnimations()
		{
			BakedAnimationDataLoader.LoadResourcesSync(this.gameLoader.environmentData);
		}

		private void loadGameDataManager()
		{
			this.gameLoader.injector.GetInstance<GameDataManager>().Initialize(this.gameLoader.config, this.gameLoader.environmentData);
		}

		private void injectDevConsole()
		{
			this.gameLoader.injector.Inject(this.gameLoader.devConsole);
		}

		private void injectGameClient()
		{
			this.gameLoader.injector.Inject(this.gameLoader.client);
		}

		private void startGameClient()
		{
			this.gameLoader.client.Init();
		}

		private void startDebugTools()
		{
			this.gameLoader.client.InitDebug();
		}
	}

	private class Preload
	{
		private struct StartupLoadItem
		{
			public string name;

			public IStartupLoader obj;

			public StartupLoadItem(string name, IStartupLoader obj)
			{
				this.name = name;
				this.obj = obj;
			}
		}

		private List<GameLoader.Preload.StartupLoadItem> list = new List<GameLoader.Preload.StartupLoadItem>();

		private Action callback;

		private IDependencyInjection injector;

		private GameLoader.TimeTracker timeTracker;

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

		private void addLoad<T>(string name) where T : IStartupLoader
		{
			this.list.Add(new GameLoader.Preload.StartupLoadItem(name, this.injector.GetInstance<T>()));
		}

		private void recursiveStartupLoad()
		{
			foreach (GameLoader.Preload.StartupLoadItem current in this.list)
			{
				this.timeTracker.Add(current.name, new Action<Action>(current.obj.StartupLoad));
			}
			this.timeTracker.Execute(this.callback);
		}
	}

	private class TimeTracker
	{
		private class CoroutineHelperMonoBehaviour : MonoBehaviour
		{
		}

		private struct LoadStep
		{
			public string name;

			public Action fn;

			public Action<Action> asyncFn;

			public LoadStep(string name, Action fn, Action<Action> asyncFn)
			{
				this.name = name;
				this.fn = fn;
				this.asyncFn = asyncFn;
			}
		}

		private sealed class _Execute_c__AnonStorey2
		{
			internal Action callback;

			internal GameLoader.TimeTracker _this;
		}

		private sealed class _Execute_c__AnonStorey1
		{
			internal GameLoader.TimeTracker.CoroutineHelperMonoBehaviour behaviour;

			internal GameLoader.TimeTracker._Execute_c__AnonStorey2 __f__ref_2;

			internal void __m__0()
			{
				this.__f__ref_2._this.activeExecution = null;
				UnityEngine.Object.Destroy(this.behaviour.gameObject);
				this.__f__ref_2.callback();
			}
		}

		private sealed class _performLoadSteps_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			private sealed class _performLoadSteps_c__AnonStorey3
			{
				internal ReferenceValue<bool> stepComplete;

				internal void __m__0()
				{
					this.stepComplete.Value = true;
				}
			}

			internal float _totalSecondsBetweenSteps___0;

			internal GameLoader.TimeTracker.LoadStep _currentStep___1;

			internal Action callback;

			internal GameLoader.TimeTracker _this;

			internal object _current;

			internal bool _disposing;

			internal int _PC;

			private GameLoader.TimeTracker._performLoadSteps_c__Iterator0._performLoadSteps_c__AnonStorey3 _locvar0;

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

			public _performLoadSteps_c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this._PC;
				this._PC = -1;
				switch (num)
				{
				case 0u:
					this._totalSecondsBetweenSteps___0 = 0f;
					goto IL_168;
				case 1u:
					break;
				case 2u:
					goto IL_168;
				default:
					return false;
				}
				IL_107:
				if (!this._locvar0.stepComplete.Value)
				{
					this._totalSecondsBetweenSteps___0 = 0f;
					this._current = null;
					if (!this._disposing)
					{
						this._PC = 1;
					}
					return true;
				}
				IL_11C:
				this._this.stop(this._currentStep___1.name);
				if (this._totalSecondsBetweenSteps___0 > 0.05f)
				{
					this._totalSecondsBetweenSteps___0 = 0f;
					this._current = null;
					if (!this._disposing)
					{
						this._PC = 2;
					}
					return true;
				}
				IL_168:
				if (this._this.loadStepList.Count <= 0)
				{
					this.callback();
					this._PC = -1;
				}
				else
				{
					this._currentStep___1 = this._this.loadStepList[0];
					this._this.loadStepList.RemoveAt(0);
					this._totalSecondsBetweenSteps___0 += Time.deltaTime;
					this._this.start();
					if (this._currentStep___1.fn != null)
					{
						this._currentStep___1.fn();
						goto IL_11C;
					}
					this._locvar0 = new GameLoader.TimeTracker._performLoadSteps_c__Iterator0._performLoadSteps_c__AnonStorey3();
					this._locvar0.stepComplete = new ReferenceValue<bool>(false);
					this._currentStep___1.asyncFn(new Action(this._locvar0.__m__0));
					goto IL_107;
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

		private Stopwatch featureTimer = new Stopwatch();

		private Stopwatch globalTimer;

		private List<GameLoader.TimeTracker.LoadStep> loadStepList = new List<GameLoader.TimeTracker.LoadStep>();

		private Coroutine activeExecution;

		public TimeTracker(Stopwatch globalTimer)
		{
			this.globalTimer = globalTimer;
		}

		public void Add(string name, Action fn)
		{
			this.loadStepList.Add(new GameLoader.TimeTracker.LoadStep(name, fn, null));
		}

		public void Add(string name, Action<Action> fn)
		{
			this.loadStepList.Add(new GameLoader.TimeTracker.LoadStep(name, null, fn));
		}

		public void Execute(Action callback)
		{
			GameLoader.TimeTracker._Execute_c__AnonStorey2 _Execute_c__AnonStorey = new GameLoader.TimeTracker._Execute_c__AnonStorey2();
			_Execute_c__AnonStorey.callback = callback;
			_Execute_c__AnonStorey._this = this;
			if (this.activeExecution == null)
			{
				GameLoader.TimeTracker._Execute_c__AnonStorey1 _Execute_c__AnonStorey2 = new GameLoader.TimeTracker._Execute_c__AnonStorey1();
				_Execute_c__AnonStorey2.__f__ref_2 = _Execute_c__AnonStorey;
				_Execute_c__AnonStorey2.behaviour = new GameObject("TimeTracker.Execute").AddComponent<GameLoader.TimeTracker.CoroutineHelperMonoBehaviour>();
				UnityEngine.Object.DontDestroyOnLoad(_Execute_c__AnonStorey2.behaviour);
				this.activeExecution = _Execute_c__AnonStorey2.behaviour.StartCoroutine(this.performLoadSteps(new Action(_Execute_c__AnonStorey2.__m__0)));
			}
		}

		private IEnumerator performLoadSteps(Action callback)
		{
			GameLoader.TimeTracker._performLoadSteps_c__Iterator0 _performLoadSteps_c__Iterator = new GameLoader.TimeTracker._performLoadSteps_c__Iterator0();
			_performLoadSteps_c__Iterator.callback = callback;
			_performLoadSteps_c__Iterator._this = this;
			return _performLoadSteps_c__Iterator;
		}

		private void start()
		{
			this.featureTimer.Reset();
			this.featureTimer.Start();
		}

		private void stop(string message)
		{
			this.featureTimer.Stop();
			if (this.featureTimer.Elapsed.TotalSeconds > 0.0099999997764825821)
			{
				UnityEngine.Debug.LogFormat("[GameLoader] {0}: {1}ms --- total: {2}s", new object[]
				{
					message,
					this.featureTimer.Elapsed.TotalSeconds * 1000.0,
					this.globalTimer.Elapsed.TotalSeconds
				});
			}
		}
	}

	private static string gameEnvironmentDataPath = "Config/GameEnvironmentData";

	private GameObject clientPrefab;

	private StartupCanvas startupCanvas;

	private Transform devUIContainer;

	private ConfigData config;

	private GameEnvironmentData environmentData;

	private DeveloperConfig devConfig;

	private SystemBoot boot;

	private AsyncResourceLoader asyncResourceLoader;

	public Stopwatch splashScreenDisplayTimer = new Stopwatch();

	private GameClient client;

	private IDevConsole devConsole;

	private IDependencyInjection injector;

	private List<Action<Action>> bootSteps = new List<Action<Action>>();

	public GameLoader(SystemBoot boot, Transform devUIContainer)
	{
		GC.Collect();
		GC.Collect();
		ProfilingUtil.ReportMemory();
		this.devUIContainer = devUIContainer;
		this.boot = boot;
		this.STARTUP();
	}

	private void STARTUP()
	{
		this.bootSteps.Add(new Action<Action>(this.initialize));
		this.bootSteps.Add(new Action<Action>(this.systemConfigure));
		this.bootSteps.Add(new Action<Action>(this.systemStartup));
		this.bootSteps.Add(new Action<Action>(this.preload));
		this.recursiveStartup();
	}

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
			action(new Action(this._recursiveStartup_m__0));
		}
	}

	private void initialize(Action callback)
	{
		WTime.Startup();
		this.splashScreenDisplayTimer.Reset();
		this.splashScreenDisplayTimer.Start();
		callback();
	}

	private void systemConfigure(Action callback)
	{
		new GameLoader.SystemConfigure(this, callback);
	}

	private void systemStartup(Action callback)
	{
		new GameLoader.SystemStartup(this, callback);
	}

	private void preload(Action callback)
	{
		new GameLoader.Preload(this, this.injector, callback);
	}

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

	public static GameEnvironmentData LoadGameEnvironmentData()
	{
		return Resources.Load<GameEnvironmentData>(GameLoader.gameEnvironmentDataPath);
	}

	private void _recursiveStartup_m__0()
	{
		this.recursiveStartup();
	}
}
