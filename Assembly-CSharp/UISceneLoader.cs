using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000A71 RID: 2673
public class UISceneLoader
{
	// Token: 0x17001263 RID: 4707
	// (get) Token: 0x06004DCD RID: 19917 RVA: 0x001480AB File Offset: 0x001464AB
	// (set) Token: 0x06004DCE RID: 19918 RVA: 0x001480B3 File Offset: 0x001464B3
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x17001264 RID: 4708
	// (get) Token: 0x06004DCF RID: 19919 RVA: 0x001480BC File Offset: 0x001464BC
	// (set) Token: 0x06004DD0 RID: 19920 RVA: 0x001480C4 File Offset: 0x001464C4
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17001265 RID: 4709
	// (get) Token: 0x06004DD1 RID: 19921 RVA: 0x001480CD File Offset: 0x001464CD
	// (set) Token: 0x06004DD2 RID: 19922 RVA: 0x001480D5 File Offset: 0x001464D5
	public UIScene SceneContainer { get; private set; }

	// Token: 0x17001266 RID: 4710
	// (get) Token: 0x06004DD3 RID: 19923 RVA: 0x001480DE File Offset: 0x001464DE
	// (set) Token: 0x06004DD4 RID: 19924 RVA: 0x001480E6 File Offset: 0x001464E6
	public string SceneName { get; private set; }

	// Token: 0x06004DD5 RID: 19925 RVA: 0x001480EF File Offset: 0x001464EF
	public void Init(string sceneName)
	{
		this.SceneName = sceneName;
		this.proxyMono = SceneLoaderMono.CreateNew("UISceneLoader - " + sceneName);
	}

	// Token: 0x17001267 RID: 4711
	// (get) Token: 0x06004DD6 RID: 19926 RVA: 0x0014810E File Offset: 0x0014650E
	// (set) Token: 0x06004DD7 RID: 19927 RVA: 0x00148138 File Offset: 0x00146538
	public bool IsLoaded
	{
		get
		{
			if (this.isLoaded && this.SceneContainer == null)
			{
				this.setUnloaded();
			}
			return this.isLoaded;
		}
		private set
		{
			this.isLoaded = value;
		}
	}

	// Token: 0x06004DD8 RID: 19928 RVA: 0x00148144 File Offset: 0x00146544
	public void LoadSync(Action callback)
	{
		if (this.IsLoaded)
		{
			callback();
		}
		else
		{
			this.proxyMono.LoadSceneSync(this.SceneName, LoadSceneMode.Additive);
			this.timer.NextFrame(delegate
			{
				this.onSceneLoaded();
				callback();
			});
		}
	}

	// Token: 0x06004DD9 RID: 19929 RVA: 0x001481AC File Offset: 0x001465AC
	public void Load(Action callback = null)
	{
		if (this.IsLoaded)
		{
			if (callback != null)
			{
				callback();
			}
		}
		else
		{
			if (callback != null)
			{
				this.callbacks.Add(callback);
			}
			if (!this.loadInProgress)
			{
				this.doLoad();
			}
		}
	}

	// Token: 0x06004DDA RID: 19930 RVA: 0x001481F8 File Offset: 0x001465F8
	public void Unload()
	{
		if (this.loadInProgress)
		{
			this.callbacks.Add(new Action(this.doUnload));
		}
		else if (this.IsLoaded)
		{
			this.doUnload();
		}
	}

	// Token: 0x06004DDB RID: 19931 RVA: 0x00148234 File Offset: 0x00146634
	private void doUnload()
	{
		if (!this.unloadInProgress)
		{
			this.unloadInProgress = true;
			this.IsLoaded = false;
			this.loadInProgress = false;
			this.SceneContainer = null;
			this.rootHierarchy = null;
			this.proxyMono.UnloadScene(this.SceneName, delegate
			{
				this.unloadInProgress = false;
			});
		}
	}

	// Token: 0x06004DDC RID: 19932 RVA: 0x0014828C File Offset: 0x0014668C
	private void setUnloaded()
	{
		this.isLoaded = false;
		this.loadInProgress = false;
		this.SceneContainer = null;
		this.rootHierarchy = null;
		this.unloadInProgress = false;
	}

	// Token: 0x06004DDD RID: 19933 RVA: 0x001482B1 File Offset: 0x001466B1
	private void doLoad()
	{
		this.loadInProgress = true;
		this.proxyMono.LoadScene(this.SceneName, LoadSceneMode.Additive, new Action(this.onSceneLoaded));
	}

	// Token: 0x06004DDE RID: 19934 RVA: 0x001482D8 File Offset: 0x001466D8
	private void onSceneLoaded()
	{
		this.findSceneContainer();
		this.validateHierarchy();
		this.SceneContainer.gameObject.SetActive(false);
		this.IsLoaded = true;
		this.loadInProgress = false;
		this.flushCallbacks();
	}

	// Token: 0x06004DDF RID: 19935 RVA: 0x0014830C File Offset: 0x0014670C
	private void validateHierarchy()
	{
		if (this.rootHierarchy.Length > 1)
		{
			throw new UnityException("!!! UI SCENE ERROR !!! - scene " + this.SceneName + ", UIScenes can only have 1 object in the root heirarchy.");
		}
		if (this.SceneContainer == null)
		{
			throw new UnityException("!!! UI SCENE ERROR !!! - scene " + this.SceneName + " must have a UIScene object as its only root object.");
		}
	}

	// Token: 0x06004DE0 RID: 19936 RVA: 0x00148370 File Offset: 0x00146770
	private void findSceneContainer()
	{
		this.rootHierarchy = SceneManager.GetSceneByName(this.SceneName).GetRootGameObjects();
		foreach (GameObject gameObject in this.rootHierarchy)
		{
			this.SceneContainer = gameObject.GetComponent<UIScene>();
			if (this.SceneContainer != null)
			{
				this.injector.Inject(this.SceneContainer);
			}
		}
	}

	// Token: 0x06004DE1 RID: 19937 RVA: 0x001483E4 File Offset: 0x001467E4
	private void flushCallbacks()
	{
		Action[] array = this.callbacks.ToArray();
		this.callbacks.Clear();
		foreach (Action action in array)
		{
			action();
		}
	}

	// Token: 0x040032F0 RID: 13040
	private SceneLoaderMono proxyMono;

	// Token: 0x040032F1 RID: 13041
	private List<Action> callbacks = new List<Action>();

	// Token: 0x040032F2 RID: 13042
	private bool loadInProgress;

	// Token: 0x040032F3 RID: 13043
	private bool unloadInProgress;

	// Token: 0x040032F4 RID: 13044
	private GameObject[] rootHierarchy;

	// Token: 0x040032F5 RID: 13045
	private bool isLoaded;
}
