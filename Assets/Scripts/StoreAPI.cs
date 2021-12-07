// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StoreAPI : IStoreAPI, IDataDependency
{
	private sealed class _Load_c__AnonStorey0
	{
		internal Action<DataLoadResult> callback;

		internal void __m__0()
		{
			DataLoadResult dataLoadResult = new DataLoadResult();
			dataLoadResult.status = DataLoadStatus.SUCCESS;
			this.callback(dataLoadResult);
		}
	}

	public static string UPDATE = "StoreAPI.UPDATE";

	private StoreMode mode;

	[Inject]
	public UIPreload3DAssets preload3dAssets
	{
		get;
		set;
	}

	[Inject]
	public IStoreTabsModel storeTabsModel
	{
		get;
		set;
	}

	[Inject]
	public IUserInventory userInventory
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameData
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

	public int Port
	{
		get;
		set;
	}

	public StoreMode Mode
	{
		get
		{
			return this.mode;
		}
		set
		{
			if (this.mode != value)
			{
				this.mode = value;
				if (this.mode == StoreMode.UNBOXING)
				{
					if (this.gameData.IsFeatureEnabled(FeatureID.LootBoxPurchase))
					{
						this.storeTabsModel.Current = StoreTab.LOOT_BOXES;
					}
					else
					{
						this.storeTabsModel.Current = StoreTab.FEATURED;
					}
				}
				this.signalBus.Dispatch(StoreAPI.UPDATE);
			}
		}
	}

	public string PortDisplay
	{
		get
		{
			return string.Empty + (this.Port - 99);
		}
	}

	public void OnScreenOpened()
	{
		this.userInventory.MarkAsNotNewAny(false);
	}

	public void Load(Action<DataLoadResult> callback)
	{
		StoreAPI._Load_c__AnonStorey0 _Load_c__AnonStorey = new StoreAPI._Load_c__AnonStorey0();
		_Load_c__AnonStorey.callback = callback;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (this.gameData.ConfigData.storeSettings.shaderVariants != null)
		{
			this.gameData.ConfigData.storeSettings.shaderVariants.WarmUp();
		}
		UnityEngine.Debug.Log("Warmup time " + (Time.realtimeSinceStartup - realtimeSinceStartup));
		this.preload3dAssets.PreloadForScene(new Action(_Load_c__AnonStorey.__m__0));
	}
}
