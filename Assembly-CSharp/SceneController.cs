using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200029D RID: 669
public class SceneController : ISceneController, IStartupLoader, ITickable
{
	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06000E21 RID: 3617 RVA: 0x00058B20 File Offset: 0x00056F20
	// (set) Token: 0x06000E22 RID: 3618 RVA: 0x00058B28 File Offset: 0x00056F28
	[Inject]
	public ISignalBus signalBus { private get; set; }

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06000E23 RID: 3619 RVA: 0x00058B31 File Offset: 0x00056F31
	// (set) Token: 0x06000E24 RID: 3620 RVA: 0x00058B39 File Offset: 0x00056F39
	[Inject]
	public ConfigData config { private get; set; }

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06000E25 RID: 3621 RVA: 0x00058B42 File Offset: 0x00056F42
	// (set) Token: 0x06000E26 RID: 3622 RVA: 0x00058B4A File Offset: 0x00056F4A
	[Inject]
	public ISceneLists sceneLists { private get; set; }

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06000E27 RID: 3623 RVA: 0x00058B53 File Offset: 0x00056F53
	// (set) Token: 0x06000E28 RID: 3624 RVA: 0x00058B5B File Offset: 0x00056F5B
	[Inject]
	public IUserVideoSettingsModel userVideoSettings { private get; set; }

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06000E29 RID: 3625 RVA: 0x00058B64 File Offset: 0x00056F64
	// (set) Token: 0x06000E2A RID: 3626 RVA: 0x00058B6C File Offset: 0x00056F6C
	[Inject]
	public IDependencyInjection injector { private get; set; }

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06000E2B RID: 3627 RVA: 0x00058B75 File Offset: 0x00056F75
	// (set) Token: 0x06000E2C RID: 3628 RVA: 0x00058B7D File Offset: 0x00056F7D
	[Inject]
	public AudioManager audioManager { private get; set; }

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x06000E2D RID: 3629 RVA: 0x00058B86 File Offset: 0x00056F86
	// (set) Token: 0x06000E2E RID: 3630 RVA: 0x00058B8E File Offset: 0x00056F8E
	[Inject]
	public GameController gameController { private get; set; }

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06000E2F RID: 3631 RVA: 0x00058B97 File Offset: 0x00056F97
	// (set) Token: 0x06000E30 RID: 3632 RVA: 0x00058B9F File Offset: 0x00056F9F
	[Inject]
	public IMainThreadTimer timer { private get; set; }

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06000E31 RID: 3633 RVA: 0x00058BA8 File Offset: 0x00056FA8
	// (set) Token: 0x06000E32 RID: 3634 RVA: 0x00058BB0 File Offset: 0x00056FB0
	[Inject]
	public IUIAdapter uiAdapter { private get; set; }

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06000E33 RID: 3635 RVA: 0x00058BB9 File Offset: 0x00056FB9
	// (set) Token: 0x06000E34 RID: 3636 RVA: 0x00058BC1 File Offset: 0x00056FC1
	[Inject]
	public IMainMenuAPI mainMenuAPI { private get; set; }

	// Token: 0x06000E35 RID: 3637 RVA: 0x00058BCA File Offset: 0x00056FCA
	public void Init()
	{
		this.init();
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x00058BD4 File Offset: 0x00056FD4
	private void init()
	{
		this.mainSceneCamera = this.findMainSceneCamera();
		this.lastStageQuality = this.userVideoSettings.StageQuality;
		this.battleSceneLoader = SceneLoaderMono.CreateNew("StageSceneLoader");
		UnityEngine.Object.DontDestroyOnLoad(this.battleSceneLoader);
		this.battleSceneLoader.gameObject.SetActive(false);
		this.signalBus.AddListener(UserVideoSettingsModel.UPDATED, new Action(this.onVideoSettingsUpdated));
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x00058C48 File Offset: 0x00057048
	private Camera findMainSceneCamera()
	{
		GameObject[] rootGameObjects = SceneManager.GetSceneByName("Main").GetRootGameObjects();
		foreach (GameObject gameObject in rootGameObjects)
		{
			Camera component = gameObject.GetComponent<Camera>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x00058CA0 File Offset: 0x000570A0
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

	// Token: 0x06000E39 RID: 3641 RVA: 0x00058CCC File Offset: 0x000570CC
	private bool isPreloadOnStartup()
	{
		SystemBoot.Mode mode = SystemBoot.mode;
		return !this.config.IsAutoStart && mode != SystemBoot.Mode.StagePreview && mode != SystemBoot.Mode.VictoryPosePreview;
	}

	// Token: 0x06000E3A RID: 3642 RVA: 0x00058D04 File Offset: 0x00057104
	private void onVideoSettingsUpdated()
	{
		if (this.lastStageQuality == this.userVideoSettings.StageQuality)
		{
			return;
		}
		this.lastStageQuality = this.userVideoSettings.StageQuality;
		string sceneName = this.sceneLists.GetSceneForUIScreen(this.uiAdapter.CurrentScreen);
		if (!string.IsNullOrEmpty(sceneName))
		{
			this.preloadScene(sceneName, true, delegate
			{
				this.ActivateUIScene(sceneName);
				this.unloadUIScenes(delegate
				{
					this.preloadScenes();
				}, sceneName);
			});
		}
		else
		{
			this.unloadUIScenes(delegate
			{
				this.preloadScenes();
			}, null);
		}
	}

	// Token: 0x06000E3B RID: 3643 RVA: 0x00058DA4 File Offset: 0x000571A4
	private void preloadScenes()
	{
		List<string> uipreloadSceneList = this.sceneLists.GetUIPreloadSceneList();
		foreach (string name in uipreloadSceneList)
		{
			this.preloadScene(name, true, null);
		}
	}

	// Token: 0x06000E3C RID: 3644 RVA: 0x00058E0C File Offset: 0x0005720C
	private void recursiveLoadScenesSync(List<string> list, int i, Action callback)
	{
		if (i >= list.Count)
		{
			callback();
		}
		else
		{
			string name = list[i];
			this.preloadScene(name, false, delegate
			{
				this.recursiveLoadScenesSync(list, i + 1, callback);
			});
		}
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x00058E88 File Offset: 0x00057288
	public void TickFrame()
	{
		if (this.pendingUnloadsCallback != null)
		{
			bool flag = true;
			foreach (UISceneLoader uisceneLoader in this.pendingUnloads)
			{
				if (uisceneLoader.IsLoaded)
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

	// Token: 0x06000E3E RID: 3646 RVA: 0x00058F1C File Offset: 0x0005731C
	public void ActivateUIScene(ScreenType type)
	{
		string sceneForUIScreen = this.sceneLists.GetSceneForUIScreen(type);
		this.ActivateUIScene(sceneForUIScreen);
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x00058F3D File Offset: 0x0005733D
	public void ActivateUIScene(string sceneName)
	{
		this.activateScene(sceneName);
		this.closeOtherScenes(sceneName);
		this.switchCamerasForScene();
	}

	// Token: 0x06000E40 RID: 3648 RVA: 0x00058F53 File Offset: 0x00057353
	private void switchCamerasForScene()
	{
		this.mainSceneCamera.gameObject.SetActive(false);
		if (Camera.allCameras.Length == 0)
		{
			this.mainSceneCamera.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000E41 RID: 3649 RVA: 0x00058F84 File Offset: 0x00057384
	private void closeOtherScenes(string name)
	{
		foreach (UISceneLoader uisceneLoader in this.sceneLoaders.Values)
		{
			if (uisceneLoader.IsLoaded && uisceneLoader.SceneContainer != null && uisceneLoader.SceneName != name)
			{
				uisceneLoader.SceneContainer.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000E42 RID: 3650 RVA: 0x0005901C File Offset: 0x0005741C
	private void activateScene(string name)
	{
		foreach (UISceneLoader uisceneLoader in this.sceneLoaders.Values)
		{
			if (uisceneLoader.IsLoaded && uisceneLoader.SceneContainer != null && uisceneLoader.SceneName == name)
			{
				uisceneLoader.SceneContainer.gameObject.SetActive(true);
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

	// Token: 0x06000E43 RID: 3651 RVA: 0x00059100 File Offset: 0x00057500
	public void PreloadUIScene(ScreenType type, Action callback)
	{
		this.preloadScene(this.sceneLists.GetSceneForUIScreen(type), true, callback);
	}

	// Token: 0x06000E44 RID: 3652 RVA: 0x00059116 File Offset: 0x00057516
	public void PreloadScene(string name, bool async, Action callback = null)
	{
		this.preloadScene(name, async, callback);
	}

	// Token: 0x06000E45 RID: 3653 RVA: 0x00059124 File Offset: 0x00057524
	public void LoadBattleScene(string sceneName, Action callback)
	{
		this.unloadUIScenes(delegate
		{
			this.battleSceneLoader.gameObject.SetActive(true);
			this.battleSceneLoader.LoadScene(sceneName, LoadSceneMode.Additive, delegate
			{
				this.battleSceneName = sceneName;
				Scene sceneByName = SceneManager.GetSceneByName(sceneName);
				SceneManager.SetActiveScene(sceneByName);
				this.switchCamerasForScene();
				this.battleSceneLoader.gameObject.SetActive(false);
				callback();
			});
		}, null);
	}

	// Token: 0x06000E46 RID: 3654 RVA: 0x00059160 File Offset: 0x00057560
	public void LoadVictoryPoseScene(Action callback)
	{
		this.disableAudioInBattleScene();
		this.audioManager.UIAudioListener.gameObject.SetActive(true);
		this.battleSceneLoader.UnloadScene(this.battleSceneName, delegate
		{
			this.switchCamerasForScene();
			this.battleSceneLoader.LoadScene(this.battleSceneName, LoadSceneMode.Additive, delegate
			{
				Scene sceneByName = SceneManager.GetSceneByName(this.battleSceneName);
				SceneManager.SetActiveScene(sceneByName);
				this.switchCamerasForScene();
				callback();
			});
		});
	}

	// Token: 0x06000E47 RID: 3655 RVA: 0x000591BC File Offset: 0x000575BC
	public void ExitBattle(Action callback)
	{
		this.mainMenuAPI.RandomizeCharacter();
		this.disableAudioInBattleScene();
		this.audioManager.UIAudioListener.gameObject.SetActive(true);
		this.battleSceneLoader.UnloadScene(this.battleSceneName, delegate
		{
			this.switchCamerasForScene();
			this.preloadScenes();
			callback();
		});
	}

	// Token: 0x06000E48 RID: 3656 RVA: 0x00059224 File Offset: 0x00057624
	private void disableAudioInBattleScene()
	{
		Scene sceneByName = SceneManager.GetSceneByName(this.battleSceneName);
		if (sceneByName.isLoaded)
		{
			foreach (GameObject gameObject in sceneByName.GetRootGameObjects())
			{
				AudioListener componentInChildren = gameObject.GetComponentInChildren<AudioListener>();
				if (componentInChildren != null)
				{
					componentInChildren.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06000E49 RID: 3657 RVA: 0x0005928C File Offset: 0x0005768C
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

	// Token: 0x06000E4A RID: 3658 RVA: 0x0005931C File Offset: 0x0005771C
	private void unloadUIScenes(Action callback, string excludeScene = null)
	{
		if (this.pendingUnloadsCallback != null)
		{
			throw new UnityException("Duplicate call not supported");
		}
		this.pendingUnloadsCallback = callback;
		this.pendingUnloads.Clear();
		foreach (KeyValuePair<string, UISceneLoader> keyValuePair in this.sceneLoaders)
		{
			if (keyValuePair.Key != "Main" && keyValuePair.Key != excludeScene)
			{
				this.pendingUnloads.Add(this.sceneLoaders[keyValuePair.Key]);
			}
		}
		foreach (UISceneLoader uisceneLoader in this.pendingUnloads)
		{
			uisceneLoader.Unload();
		}
	}

	// Token: 0x06000E4B RID: 3659 RVA: 0x00059428 File Offset: 0x00057828
	private void unloadScene(string name)
	{
		if (this.sceneLoaders.ContainsKey(name))
		{
			this.sceneLoaders[name].Unload();
		}
	}

	// Token: 0x06000E4C RID: 3660 RVA: 0x0005944C File Offset: 0x0005784C
	public T GetUIScene<T>() where T : UIScene
	{
		foreach (UISceneLoader uisceneLoader in this.sceneLoaders.Values)
		{
			if (uisceneLoader.IsLoaded && uisceneLoader.SceneContainer.GetType() == typeof(T))
			{
				return (T)((object)uisceneLoader.SceneContainer);
			}
		}
		return (T)((object)null);
	}

	// Token: 0x04000848 RID: 2120
	private static HashSet<string> setActiveForScene = new HashSet<string>
	{
		"MainMenuScene-Stage",
		"MainMenuScene-Cryo",
		"MainMenuScene-Shrine",
		"MainMenuScene-CombatLab"
	};

	// Token: 0x04000853 RID: 2131
	private Dictionary<string, UISceneLoader> sceneLoaders = new Dictionary<string, UISceneLoader>();

	// Token: 0x04000854 RID: 2132
	private List<UISceneLoader> pendingUnloads = new List<UISceneLoader>();

	// Token: 0x04000855 RID: 2133
	private Action pendingUnloadsCallback;

	// Token: 0x04000856 RID: 2134
	private const string mainSceneName = "Main";

	// Token: 0x04000857 RID: 2135
	private Camera mainSceneCamera;

	// Token: 0x04000858 RID: 2136
	private string battleSceneName;

	// Token: 0x04000859 RID: 2137
	private ThreeTierQualityLevel lastStageQuality;

	// Token: 0x0400085A RID: 2138
	private SceneLoaderMono battleSceneLoader;
}
