// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISceneLoader
{
	private sealed class _LoadSync_c__AnonStorey0
	{
		internal Action callback;

		internal UISceneLoader _this;

		internal void __m__0()
		{
			this._this.onSceneLoaded();
			this.callback();
		}
	}

	private SceneLoaderMono proxyMono;

	private List<Action> callbacks = new List<Action>();

	private bool loadInProgress;

	private bool unloadInProgress;

	private GameObject[] rootHierarchy;

	private bool isLoaded;

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	public UIScene SceneContainer
	{
		get;
		private set;
	}

	public string SceneName
	{
		get;
		private set;
	}

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

	public void Init(string sceneName)
	{
		this.SceneName = sceneName;
		this.proxyMono = SceneLoaderMono.CreateNew("UISceneLoader - " + sceneName);
	}

	public void LoadSync(Action callback)
	{
		UISceneLoader._LoadSync_c__AnonStorey0 _LoadSync_c__AnonStorey = new UISceneLoader._LoadSync_c__AnonStorey0();
		_LoadSync_c__AnonStorey.callback = callback;
		_LoadSync_c__AnonStorey._this = this;
		if (this.IsLoaded)
		{
			_LoadSync_c__AnonStorey.callback();
		}
		else
		{
			this.proxyMono.LoadSceneSync(this.SceneName, LoadSceneMode.Additive);
			this.timer.NextFrame(new Action(_LoadSync_c__AnonStorey.__m__0));
		}
	}

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

	private void doUnload()
	{
		if (!this.unloadInProgress)
		{
			this.unloadInProgress = true;
			this.IsLoaded = false;
			this.loadInProgress = false;
			this.SceneContainer = null;
			this.rootHierarchy = null;
			this.proxyMono.UnloadScene(this.SceneName, new Action(this._doUnload_m__0));
		}
	}

	private void setUnloaded()
	{
		this.isLoaded = false;
		this.loadInProgress = false;
		this.SceneContainer = null;
		this.rootHierarchy = null;
		this.unloadInProgress = false;
	}

	private void doLoad()
	{
		this.loadInProgress = true;
		this.proxyMono.LoadScene(this.SceneName, LoadSceneMode.Additive, new Action(this.onSceneLoaded));
	}

	private void onSceneLoaded()
	{
		this.findSceneContainer();
		this.validateHierarchy();
		this.SceneContainer.gameObject.SetActive(false);
		this.IsLoaded = true;
		this.loadInProgress = false;
		this.flushCallbacks();
	}

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

	private void findSceneContainer()
	{
		this.rootHierarchy = SceneManager.GetSceneByName(this.SceneName).GetRootGameObjects();
		GameObject[] array = this.rootHierarchy;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = array[i];
			this.SceneContainer = gameObject.GetComponent<UIScene>();
			if (this.SceneContainer != null)
			{
				this.injector.Inject(this.SceneContainer);
			}
		}
	}

	private void flushCallbacks()
	{
		Action[] array = this.callbacks.ToArray();
		this.callbacks.Clear();
		Action[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Action action = array2[i];
			action();
		}
	}

	private void _doUnload_m__0()
	{
		this.unloadInProgress = false;
	}
}
