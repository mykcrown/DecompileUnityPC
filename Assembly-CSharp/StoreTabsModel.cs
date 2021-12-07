using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A32 RID: 2610
public class StoreTabsModel : IStoreTabsModel
{
	// Token: 0x17001221 RID: 4641
	// (get) Token: 0x06004C69 RID: 19561 RVA: 0x0014479F File Offset: 0x00142B9F
	// (set) Token: 0x06004C6A RID: 19562 RVA: 0x001447A7 File Offset: 0x00142BA7
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17001222 RID: 4642
	// (get) Token: 0x06004C6B RID: 19563 RVA: 0x001447B0 File Offset: 0x00142BB0
	// (set) Token: 0x06004C6C RID: 19564 RVA: 0x001447B8 File Offset: 0x00142BB8
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x06004C6D RID: 19565 RVA: 0x001447C1 File Offset: 0x00142BC1
	[PostConstruct]
	public void Init()
	{
		this.rebuildStoreTabList();
	}

	// Token: 0x06004C6E RID: 19566 RVA: 0x001447CC File Offset: 0x00142BCC
	private void rebuildStoreTabList()
	{
		List<TabDefinition> list = new List<TabDefinition>();
		if (this.gameDataManager.GameData.IsFeatureEnabled(FeatureID.Store))
		{
			if (this.gameDataManager.GameData.IsFeatureEnabled(FeatureID.PortalPacks) && this.gameDataManager.GameData.IsFeatureEnabled(FeatureID.LootBoxPurchase))
			{
				list.Add(new TabDefinition(StoreTab.LOOT_BOXES, null));
			}
			list.Add(new TabDefinition(StoreTab.CHARACTERS, null));
			list.Add(new TabDefinition(StoreTab.COLLECTIBLES, null));
		}
		for (int i = 0; i < list.Count; i++)
		{
			list[i].ordinal = i;
		}
		this.tabs = list.ToArray();
		this.index = new Dictionary<StoreTab, TabDefinition>();
		foreach (TabDefinition tabDefinition in this.tabs)
		{
			this.index[(StoreTab)tabDefinition.id] = tabDefinition;
		}
		if (!this.isCurrentValid())
		{
			this.current = (StoreTab)this.tabs[0].id;
		}
	}

	// Token: 0x06004C6F RID: 19567 RVA: 0x001448DC File Offset: 0x00142CDC
	private bool isCurrentValid()
	{
		foreach (TabDefinition tabDefinition in this.tabs)
		{
			if (tabDefinition.id == (int)this.current)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x17001223 RID: 4643
	// (get) Token: 0x06004C70 RID: 19568 RVA: 0x0014491C File Offset: 0x00142D1C
	public bool ShowTabs
	{
		get
		{
			return this.tabs.Length > 1;
		}
	}

	// Token: 0x06004C71 RID: 19569 RVA: 0x00144929 File Offset: 0x00142D29
	private StoreTab getDefaultTab()
	{
		return (StoreTab)this.tabs[0].id;
	}

	// Token: 0x06004C72 RID: 19570 RVA: 0x00144938 File Offset: 0x00142D38
	public void Reset()
	{
		this.Current = this.getDefaultTab();
	}

	// Token: 0x17001224 RID: 4644
	// (get) Token: 0x06004C73 RID: 19571 RVA: 0x00144946 File Offset: 0x00142D46
	// (set) Token: 0x06004C74 RID: 19572 RVA: 0x0014494E File Offset: 0x00142D4E
	public StoreTab Current
	{
		get
		{
			return this.current;
		}
		set
		{
			if (this.current != value)
			{
				this.current = value;
				this.signalBus.Dispatch("StoreTabSelectionModel.STORE_TAB_UPDATED");
			}
		}
	}

	// Token: 0x17001225 RID: 4645
	// (get) Token: 0x06004C75 RID: 19573 RVA: 0x00144973 File Offset: 0x00142D73
	public TabDefinition[] Tabs
	{
		get
		{
			return this.tabs;
		}
	}

	// Token: 0x06004C76 RID: 19574 RVA: 0x0014497B File Offset: 0x00142D7B
	public void MapPrefab(StoreTab id, GameObject prefab)
	{
		if (this.index.ContainsKey(id))
		{
			this.index[id].prefab = prefab;
		}
	}

	// Token: 0x06004C77 RID: 19575 RVA: 0x001449A0 File Offset: 0x00142DA0
	public void Shift(int amount)
	{
		int num = this.index[this.current].ordinal + amount;
		int num2 = this.tabs.Length;
		if (num >= num2)
		{
			num -= num2;
		}
		if (num < 0)
		{
			num += num2;
		}
		TabDefinition tabDefinition = this.tabs[num];
		this.Current = (StoreTab)tabDefinition.id;
	}

	// Token: 0x0400321A RID: 12826
	public const string STORE_TAB_UPDATED = "StoreTabSelectionModel.STORE_TAB_UPDATED";

	// Token: 0x0400321D RID: 12829
	private TabDefinition[] tabs;

	// Token: 0x0400321E RID: 12830
	private Dictionary<StoreTab, TabDefinition> index;

	// Token: 0x0400321F RID: 12831
	private StoreTab current;
}
