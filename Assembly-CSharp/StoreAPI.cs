using System;
using UnityEngine;

// Token: 0x02000A26 RID: 2598
public class StoreAPI : IStoreAPI, IDataDependency
{
	// Token: 0x17001200 RID: 4608
	// (get) Token: 0x06004BB8 RID: 19384 RVA: 0x00142BDD File Offset: 0x00140FDD
	// (set) Token: 0x06004BB9 RID: 19385 RVA: 0x00142BE5 File Offset: 0x00140FE5
	[Inject]
	public UIPreload3DAssets preload3dAssets { get; set; }

	// Token: 0x17001201 RID: 4609
	// (get) Token: 0x06004BBA RID: 19386 RVA: 0x00142BEE File Offset: 0x00140FEE
	// (set) Token: 0x06004BBB RID: 19387 RVA: 0x00142BF6 File Offset: 0x00140FF6
	[Inject]
	public IStoreTabsModel storeTabsModel { get; set; }

	// Token: 0x17001202 RID: 4610
	// (get) Token: 0x06004BBC RID: 19388 RVA: 0x00142BFF File Offset: 0x00140FFF
	// (set) Token: 0x06004BBD RID: 19389 RVA: 0x00142C07 File Offset: 0x00141007
	[Inject]
	public IUserInventory userInventory { get; set; }

	// Token: 0x17001203 RID: 4611
	// (get) Token: 0x06004BBE RID: 19390 RVA: 0x00142C10 File Offset: 0x00141010
	// (set) Token: 0x06004BBF RID: 19391 RVA: 0x00142C18 File Offset: 0x00141018
	[Inject]
	public GameDataManager gameData { get; set; }

	// Token: 0x17001204 RID: 4612
	// (get) Token: 0x06004BC0 RID: 19392 RVA: 0x00142C21 File Offset: 0x00141021
	// (set) Token: 0x06004BC1 RID: 19393 RVA: 0x00142C29 File Offset: 0x00141029
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17001205 RID: 4613
	// (get) Token: 0x06004BC2 RID: 19394 RVA: 0x00142C32 File Offset: 0x00141032
	// (set) Token: 0x06004BC3 RID: 19395 RVA: 0x00142C3A File Offset: 0x0014103A
	public int Port { get; set; }

	// Token: 0x06004BC4 RID: 19396 RVA: 0x00142C43 File Offset: 0x00141043
	public void OnScreenOpened()
	{
		this.userInventory.MarkAsNotNewAny(false);
	}

	// Token: 0x06004BC5 RID: 19397 RVA: 0x00142C54 File Offset: 0x00141054
	public void Load(Action<DataLoadResult> callback)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (this.gameData.ConfigData.storeSettings.shaderVariants != null)
		{
			this.gameData.ConfigData.storeSettings.shaderVariants.WarmUp();
		}
		Debug.Log("Warmup time " + (Time.realtimeSinceStartup - realtimeSinceStartup));
		this.preload3dAssets.PreloadForScene(delegate
		{
			DataLoadResult dataLoadResult = new DataLoadResult();
			dataLoadResult.status = DataLoadStatus.SUCCESS;
			callback(dataLoadResult);
		});
	}

	// Token: 0x17001206 RID: 4614
	// (get) Token: 0x06004BC6 RID: 19398 RVA: 0x00142CE0 File Offset: 0x001410E0
	// (set) Token: 0x06004BC7 RID: 19399 RVA: 0x00142CE8 File Offset: 0x001410E8
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

	// Token: 0x17001207 RID: 4615
	// (get) Token: 0x06004BC8 RID: 19400 RVA: 0x00142D53 File Offset: 0x00141153
	public string PortDisplay
	{
		get
		{
			return string.Empty + (this.Port - 99);
		}
	}

	// Token: 0x040031B8 RID: 12728
	public static string UPDATE = "StoreAPI.UPDATE";

	// Token: 0x040031BE RID: 12734
	private StoreMode mode;
}
