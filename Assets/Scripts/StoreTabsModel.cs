// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class StoreTabsModel : IStoreTabsModel
{
	public const string STORE_TAB_UPDATED = "StoreTabSelectionModel.STORE_TAB_UPDATED";

	private TabDefinition[] tabs;

	private Dictionary<StoreTab, TabDefinition> index;

	private StoreTab current;

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	public bool ShowTabs
	{
		get
		{
			return this.tabs.Length > 1;
		}
	}

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

	public TabDefinition[] Tabs
	{
		get
		{
			return this.tabs;
		}
	}

	[PostConstruct]
	public void Init()
	{
		this.rebuildStoreTabList();
	}

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
		TabDefinition[] array = this.tabs;
		for (int j = 0; j < array.Length; j++)
		{
			TabDefinition tabDefinition = array[j];
			this.index[(StoreTab)tabDefinition.id] = tabDefinition;
		}
		if (!this.isCurrentValid())
		{
			this.current = (StoreTab)this.tabs[0].id;
		}
	}

	private bool isCurrentValid()
	{
		TabDefinition[] array = this.tabs;
		for (int i = 0; i < array.Length; i++)
		{
			TabDefinition tabDefinition = array[i];
			if (tabDefinition.id == (int)this.current)
			{
				return true;
			}
		}
		return false;
	}

	private StoreTab getDefaultTab()
	{
		return (StoreTab)this.tabs[0].id;
	}

	public void Reset()
	{
		this.Current = this.getDefaultTab();
	}

	public void MapPrefab(StoreTab id, GameObject prefab)
	{
		if (this.index.ContainsKey(id))
		{
			this.index[id].prefab = prefab;
		}
	}

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
}
