// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BackgroundLoader : IBackgroundLoader
{
	private bool _bakedAnimationDataLoaded;

	private List<Action> asyncLoadCallbacks = new List<Action>();

	[Inject]
	public IDialogController dialogController
	{
		private get;
		set;
	}

	[Inject]
	public DeveloperConfig devConfig
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
	public IGameClient gameClient
	{
		private get;
		set;
	}

	[Inject]
	public IBakedAnimationDataManager bakedAnimationDataManager
	{
		get;
		set;
	}

	public bool BakedAnimationDataLoaded
	{
		get
		{
			return this._bakedAnimationDataLoaded;
		}
		private set
		{
			this._bakedAnimationDataLoaded = value;
			this.checkForAsyncLoadsComplete();
		}
	}

	private bool allAsyncLoadsComplete
	{
		get
		{
			return this.BakedAnimationDataLoaded;
		}
	}

	public void LoadBakedAnimations(MonoBehaviour host)
	{
		bool flag = !this.BakedAnimationDataLoaded && (!Debug.isDebugBuild || (this.devConfig.useLocalBakedAnimations && SystemBoot.mode == SystemBoot.Mode.Standard)) && !this.config.IsAutoStart;
		if (flag)
		{
			BakedAnimationDataLoader.LoadBakedData(host, this.bakedAnimationDataManager, new Action(this._LoadBakedAnimations_m__0), new Action<string>(this._LoadBakedAnimations_m__1));
		}
		else
		{
			this.BakedAnimationDataLoaded = true;
		}
	}

	public void OnApplicationQuit()
	{
		BakedAnimationDataLoader.OnApplicationQuit(this.bakedAnimationDataManager);
	}

	public void WaitForSetup(Action callback)
	{
		if (this.allAsyncLoadsComplete)
		{
			callback();
		}
		else
		{
			this.asyncLoadCallbacks.Add(callback);
		}
	}

	private void checkForAsyncLoadsComplete()
	{
		if (this.allAsyncLoadsComplete)
		{
			Action[] array = this.asyncLoadCallbacks.ToArray();
			this.asyncLoadCallbacks.Clear();
			Action[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Action action = array2[i];
				action();
			}
		}
	}

	private void _LoadBakedAnimations_m__0()
	{
		GC.Collect();
		this.BakedAnimationDataLoaded = true;
	}

	private void _LoadBakedAnimations_m__1(string error)
	{
		this.dialogController.ShowOneButtonDialog("Error", error, "OK", WindowTransition.NONE, false, default(AudioData));
	}
}
