// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : ISceneController, IStartupLoader, ITickable
{
	private sealed class _onVideoSettingsUpdated_c__AnonStorey0
	{
		internal string sceneName;

		internal SceneController _this;

		internal void __m__0()
		{
			this._this.ActivateUIScene(this.sceneName);
			this._this.unloadUIScenes(new Action(this.__m__2), this.sceneName);
		}

		internal void __m__1()
		{
			this._this.preloadScenes();
		}

		internal void __m__2()
		{
			this._this.preloadScenes();
		}
	}

	private sealed class _recursiveLoadScenesSync_c__AnonStorey1
	{
		internal List<string> list;

		internal int i;

		internal Action callback;

		internal SceneController _this;

		internal void __m__0()
		{
			this._this.recursiveLoadScenesSync(this.list, this.i + 1, this.callback);
		}
	}

	private sealed class _LoadBattleScene_c__AnonStorey2
	{
		internal string sceneName;

		internal Action callback;

		internal SceneController _this;

		internal void __m__0()
		{
			this._this.battleSceneLoader.gameObject.SetActive(true);
			this._this.battleSceneLoader.LoadScene(this.sceneName, LoadSceneMode.Additive, new Action(this.__m__1));
		}

		internal void __m__1()
		{
			this._this.battleSceneName = this.sceneName;
			Scene sceneByName = SceneManager.GetSceneByName(this.sceneName);
			SceneManager.SetActiveScene(sceneByName);
			this._this.switchCamerasForScene();
			this._this.battleSceneLoader.gameObject.SetActive(false);
			this.callback();
		}
	}

	private sealed class _LoadVictoryPoseScene_c__AnonStorey3
	{
		internal Action callback;

		internal SceneController _this;

		internal void __m__0()
		{
			this._this.switchCamerasForScene();
			this._this.battleSceneLoader.LoadScene(this._this.battleSceneName, LoadSceneMode.Additive, new Action(this.__m__1));
		}

		internal void __m__1()
		{
			Scene sceneByName = SceneManager.GetSceneByName(this._this.battleSceneName);
			SceneManager.SetActiveScene(sceneByName);
			this._this.switchCamerasForScene();
			this.callback();
		}
	}

	private sealed class _ExitBattle_c__AnonStorey4
	{
		internal Action callback;

		internal SceneController _this;

		internal void __m__0()
		{
			this._this.switchCamerasForScene();
			this._this.preloadScenes();
			this.callback();
		}
	}

	private static HashSet<string> setActiveForScene = new HashSet<string>
	{
		"MainMenuScene-Stage",
		"MainMenuScene-Cryo",
		"MainMenuScene-Shrine",
		"MainMenuScene-CombatLab"
	};

	private Dictionary<string, UISceneLoader> sceneLoaders = new Dictionary<string, UISceneLoader>();

	private List<UISceneLoader> pendingUnloads = new List<UISceneLoader>();

	private Action pendingUnloadsCallback;

	private const string mainSceneName = "Main";

	private Camera mainSceneCamera;

	private string battleSceneName;

	private ThreeTierQualityLevel lastStageQuality;

	private SceneLoaderMono battleSceneLoader;

	[Inject]
	public ISignalBus signalBus
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
	public ISceneLists sceneLists
	{
		private get;
		set;
	}

	[Inject]
	public IUserVideoSettingsModel userVideoSettings
	{
		private get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
	{
		private get;
		set;
	}

	[Inject]
	public AudioManager audioManager
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
	public IMainThreadTimer timer
	{
		private get;
		set;
	}

	[Inject]
	public IUIAdapter uiAdapter
	{
		private get;
		set;
	}

	[Inject]
	public IMainMenuAPI mainMenuAPI
	{
		private get;
		set;
	}

	public void Init()
	{
		this.init();
	}

	private void init()
	{
		this.mainSceneCamera = this.findMainSceneCamera();
		this.lastStageQuality = this.userVideoSettings.StageQuality;
		this.battleSceneLoader = SceneLoaderMono.CreateNew("StageSceneLoader");
		UnityEngine.Object.DontDestroyOnLoad(this.battleSceneLoader);
		this.battleSceneLoader.gameObject.SetActive(false);
		this.signalBus.AddListener(UserVideoSettingsModel.UPDATED, new Action(this.onVideoSettingsUpdated));
	}

	private Camera findMainSceneCamera()
	{
		GameObject[] rootGameObjects = SceneManager.GetSceneByName("Main").GetRootGameObjects();
		GameObject[] array = rootGameObjects;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = array[i];
			Camera component = gameObject.GetComponent<Camera>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	public void StartupLoad(Action callback)
	{
		if (this.isPreloadOnStartup())
		{
			this.recursiveLoadScenesSync(this.sceneLists.GetUIPreloadSceneList(), 0, callback);
		}
		else
		{
			callback();
		}
	}

	private bool isPreloadOnStartup()
	{
		SystemBoot.Mode mode = SystemBoot.mode;
		return !this.config.IsAutoStart && mode != SystemBoot.Mode.StagePreview && mode != SystemBoot.Mode.VictoryPosePreview;
	}

	private void onVideoSettingsUpdated()
	{
		SceneController._onVideoSettingsUpdated_c__AnonStorey0 _onVideoSettingsUpdated_c__AnonStorey = new SceneController._onVideoSettingsUpdated_c__AnonStorey0();
		_onVideoSettingsUpdated_c__AnonStorey._this = this;
		if (this.lastStageQuality == this.userVideoSettings.StageQuality)
		{
			return;
		}
		this.lastStageQuality = this.userVideoSettings.StageQuality;
		_onVideoSettingsUpdated_c__AnonStorey.sceneName = this.sceneLists.GetSceneForUIScreen(this.uiAdapter.CurrentScreen);
		if (!string.IsNullOrEmpty(_onVideoSettingsUpdated_c__AnonStorey.sceneName))
		{
			this.preloadScene(_onVideoSettingsUpdated_c__AnonStorey.sceneName, true, new Action(_onVideoSettingsUpdated_c__AnonStorey.__m__0));
		}
		else
		{
			this.unloadUIScenes(new Action(_onVideoSettingsUpdated_c__AnonStorey.__m__1), null);
		}
	}

	private void preloadScenes()
	{
		List<string> uIPreloadSceneList = this.sceneLists.GetUIPreloadSceneList();
		foreach (string current in uIPreloadSceneList)
		{
			this.preloadScene(current, true, null);
		}
	}

	private void recursiveLoadScenesSync(List<string> list, int i, Action callback)
	{
		SceneController._recursiveLoadScenesSync_c__AnonStorey1 _recursiveLoadScenesSync_c__AnonStorey = new SceneController._recursiveLoadScenesSync_c__AnonStorey1();
		_recursiveLoadScenesSync_c__AnonStorey.list = list;
		_recursiveLoadScenesSync_c__AnonStorey.i = i;
		_recursiveLoadScenesSync_c__AnonStorey.callback = callback;
		_recursiveLoadScenesSync_c__AnonStorey._this = this;
		if (_recursiveLoadScenesSync_c__AnonStorey.i >= _recursiveLoadScenesSync_c__AnonStorey.list.Count)
		{
			_recursiveLoadScenesSync_c__AnonStorey.callback();
		}
		else
		{
			string name = _recursiveLoadScenesSync_c__AnonStorey.list[_recursiveLoadScenesSync_c__AnonStorey.i];
			this.preloadScene(name, false, new Action(_recursiveLoadScenesSync_c__AnonStorey.__m__0));
		}
	}

	public void TickFrame()
	{
		if (this.pendingUnloadsCallback != null)
		{
			bool flag = true;
			foreach (UISceneLoader current in this.pendingUnloads)
			{
				if (current.IsLoaded)
				{
					flag = false;
				}
			}
			if (flag)
			{
				this.pendingUnloads.Clear();
				Action action = this.pendingUnloadsCallback;
				this.pendingUnloadsCallback = null;
				action();
			}
		}
	}

	public void ActivateUIScene(ScreenType type)
	{
		string sceneForUIScreen = this.sceneLists.GetSceneForUIScreen(type);
		this.ActivateUIScene(sceneForUIScreen);
	}

	public void ActivateUIScene(string sceneName)
	{
		this.activateScene(sceneName);
		this.closeOtherScenes(sceneName);
		this.switchCamerasForScene();
	}

	private void switchCamerasForScene()
	{
		this.mainSceneCamera.gameObject.SetActive(false);
		if (Camera.allCameras.Length == 0)
		{
			this.mainSceneCamera.gameObject.SetActive(true);
		}
	}

	private void closeOtherScenes(string name)
	{
		foreach (UISceneLoader current in this.sceneLoaders.Values)
		{
			if (current.IsLoaded && current.SceneContainer != null && current.SceneName != name)
			{
				current.SceneContainer.gameObject.SetActive(false);
			}
		}
	}

	private void activateScene(string name)
	{
		foreach (UISceneLoader current in this.sceneLoaders.Values)
		{
			if (current.IsLoaded && current.SceneContainer != null && current.SceneName == name)
			{
				current.SceneContainer.gameObject.SetActive(true);
			}
		}
		if (this.gameController.currentGame == null)
		{
			if (SceneController.setActiveForScene.Contains(name))
			{
				SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
			}
			else
			{
				Scene sceneByName = SceneManager.GetSceneByName("Main");
				SceneManager.SetActiveScene(sceneByName);
			}
		}
	}

	public void PreloadUIScene(ScreenType type, Action callback)
	{
		this.preloadScene(this.sceneLists.GetSceneForUIScreen(type), true, callback);
	}

	public void PreloadScene(string name, bool async, Action callback = null)
	{
		this.preloadScene(name, async, callback);
	}

	public void LoadBattleScene(string sceneName, Action callback)
	{
		SceneController._LoadBattleScene_c__AnonStorey2 _LoadBattleScene_c__AnonStorey = new SceneController._LoadBattleScene_c__AnonStorey2();
		_LoadBattleScene_c__AnonStorey.sceneName = sceneName;
		_LoadBattleScene_c__AnonStorey.callback = callback;
		_LoadBattleScene_c__AnonStorey._this = this;
		this.unloadUIScenes(new Action(_LoadBattleScene_c__AnonStorey.__m__0), null);
	}

	public void LoadVictoryPoseScene(Action callback)
	{
		SceneController._LoadVictoryPoseScene_c__AnonStorey3 _LoadVictoryPoseScene_c__AnonStorey = new SceneController._LoadVictoryPoseScene_c__AnonStorey3();
		_LoadVictoryPoseScene_c__AnonStorey.callback = callback;
		_LoadVictoryPoseScene_c__AnonStorey._this = this;
		this.disableAudioInBattleScene();
		this.audioManager.UIAudioListener.gameObject.SetActive(true);
		this.battleSceneLoader.UnloadScene(this.battleSceneName, new Action(_LoadVictoryPoseScene_c__AnonStorey.__m__0));
	}

	public void ExitBattle(Action callback)
	{
		SceneController._ExitBattle_c__AnonStorey4 _ExitBattle_c__AnonStorey = new SceneController._ExitBattle_c__AnonStorey4();
		_ExitBattle_c__AnonStorey.callback = callback;
		_ExitBattle_c__AnonStorey._this = this;
		this.mainMenuAPI.RandomizeCharacter();
		this.disableAudioInBattleScene();
		this.audioManager.UIAudioListener.gameObject.SetActive(true);
		this.battleSceneLoader.UnloadScene(this.battleSceneName, new Action(_ExitBattle_c__AnonStorey.__m__0));
	}

	private void disableAudioInBattleScene()
	{
		Scene sceneByName = SceneManager.GetSceneByName(this.battleSceneName);
		if (sceneByName.isLoaded)
		{
			GameObject[] rootGameObjects = sceneByName.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				GameObject gameObject = rootGameObjects[i];
				AudioListener componentInChildren = gameObject.GetComponentInChildren<AudioListener>();
				if (componentInChildren != null)
				{
					componentInChildren.gameObject.SetActive(false);
				}
			}
		}
	}

	private void preloadScene(string name, bool async, Action callback = null)
	{
		if (name == null)
		{
			if (callback != null)
			{
				callback();
			}
		}
		else
		{
			if (!this.sceneLoaders.ContainsKey(name))
			{
				this.sceneLoaders[name] = this.injector.GetInstance<UISceneLoader>();
				this.sceneLoaders[name].Init(name);
			}
			if (async)
			{
				this.sceneLoaders[name].Load(callback);
			}
			else
			{
				this.sceneLoaders[name].LoadSync(callback);
			}
		}
	}

	private void unloadUIScenes(Action callback, string excludeScene = null)
	{
		if (this.pendingUnloadsCallback != null)
		{
			throw new UnityException("Duplicate call not supported");
		}
		this.pendingUnloadsCallback = callback;
		this.pendingUnloads.Clear();
		foreach (KeyValuePair<string, UISceneLoader> current in this.sceneLoaders)
		{
			if (current.Key != "Main" && current.Key != excludeScene)
			{
				this.pendingUnloads.Add(this.sceneLoaders[current.Key]);
			}
		}
		foreach (UISceneLoader current2 in this.pendingUnloads)
		{
			current2.Unload();
		}
	}

	private void unloadScene(string name)
	{
		if (this.sceneLoaders.ContainsKey(name))
		{
			this.sceneLoaders[name].Unload();
		}
	}

	public T GetUIScene<T>() where T : UIScene
	{
		foreach (UISceneLoader current in this.sceneLoaders.Values)
		{
			if (current.IsLoaded && current.SceneContainer.GetType() == typeof(T))
			{
				return (T)((object)current.SceneContainer);
			}
		}
		return (T)((object)null);
	}
}
