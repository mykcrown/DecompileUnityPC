using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006BC RID: 1724
public class BackgroundLoader : IBackgroundLoader
{
	// Token: 0x17000AB2 RID: 2738
	// (get) Token: 0x06002B5E RID: 11102 RVA: 0x000E2CC8 File Offset: 0x000E10C8
	// (set) Token: 0x06002B5F RID: 11103 RVA: 0x000E2CD0 File Offset: 0x000E10D0
	[Inject]
	public IDialogController dialogController { private get; set; }

	// Token: 0x17000AB3 RID: 2739
	// (get) Token: 0x06002B60 RID: 11104 RVA: 0x000E2CD9 File Offset: 0x000E10D9
	// (set) Token: 0x06002B61 RID: 11105 RVA: 0x000E2CE1 File Offset: 0x000E10E1
	[Inject]
	public DeveloperConfig devConfig { private get; set; }

	// Token: 0x17000AB4 RID: 2740
	// (get) Token: 0x06002B62 RID: 11106 RVA: 0x000E2CEA File Offset: 0x000E10EA
	// (set) Token: 0x06002B63 RID: 11107 RVA: 0x000E2CF2 File Offset: 0x000E10F2
	[Inject]
	public ConfigData config { private get; set; }

	// Token: 0x17000AB5 RID: 2741
	// (get) Token: 0x06002B64 RID: 11108 RVA: 0x000E2CFB File Offset: 0x000E10FB
	// (set) Token: 0x06002B65 RID: 11109 RVA: 0x000E2D03 File Offset: 0x000E1103
	[Inject]
	public IGameClient gameClient { private get; set; }

	// Token: 0x17000AB6 RID: 2742
	// (get) Token: 0x06002B66 RID: 11110 RVA: 0x000E2D0C File Offset: 0x000E110C
	// (set) Token: 0x06002B67 RID: 11111 RVA: 0x000E2D14 File Offset: 0x000E1114
	[Inject]
	public IBakedAnimationDataManager bakedAnimationDataManager { get; set; }

	// Token: 0x17000AB7 RID: 2743
	// (get) Token: 0x06002B68 RID: 11112 RVA: 0x000E2D1D File Offset: 0x000E111D
	// (set) Token: 0x06002B69 RID: 11113 RVA: 0x000E2D25 File Offset: 0x000E1125
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

	// Token: 0x17000AB8 RID: 2744
	// (get) Token: 0x06002B6A RID: 11114 RVA: 0x000E2D34 File Offset: 0x000E1134
	private bool allAsyncLoadsComplete
	{
		get
		{
			return this.BakedAnimationDataLoaded;
		}
	}

	// Token: 0x06002B6B RID: 11115 RVA: 0x000E2D3C File Offset: 0x000E113C
	public void LoadBakedAnimations(MonoBehaviour host)
	{
		bool flag = !this.BakedAnimationDataLoaded && (!Debug.isDebugBuild || (this.devConfig.useLocalBakedAnimations && SystemBoot.mode == SystemBoot.Mode.Standard)) && !this.config.IsAutoStart;
		if (flag)
		{
			BakedAnimationDataLoader.LoadBakedData(host, this.bakedAnimationDataManager, delegate
			{
				GC.Collect();
				this.BakedAnimationDataLoaded = true;
			}, delegate(string error)
			{
				this.dialogController.ShowOneButtonDialog("Error", error, "OK", WindowTransition.NONE, false, default(AudioData));
			});
		}
		else
		{
			this.BakedAnimationDataLoaded = true;
		}
	}

	// Token: 0x06002B6C RID: 11116 RVA: 0x000E2DC0 File Offset: 0x000E11C0
	public void OnApplicationQuit()
	{
		BakedAnimationDataLoader.OnApplicationQuit(this.bakedAnimationDataManager);
	}

	// Token: 0x06002B6D RID: 11117 RVA: 0x000E2DCD File Offset: 0x000E11CD
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

	// Token: 0x06002B6E RID: 11118 RVA: 0x000E2DF4 File Offset: 0x000E11F4
	private void checkForAsyncLoadsComplete()
	{
		if (this.allAsyncLoadsComplete)
		{
			Action[] array = this.asyncLoadCallbacks.ToArray();
			this.asyncLoadCallbacks.Clear();
			foreach (Action action in array)
			{
				action();
			}
		}
	}

	// Token: 0x04001F03 RID: 7939
	private bool _bakedAnimationDataLoaded;

	// Token: 0x04001F04 RID: 7940
	private List<Action> asyncLoadCallbacks = new List<Action>();
}
