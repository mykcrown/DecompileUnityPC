// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class SettingsTabsModel : ISettingsTabsModel
{
	public const string SETTINGS_TAB_UPDATED = "SettingsTabSelectionModel.SETTINGS_TAB_UPDATED";

	private TabDefinition[] tabs;

	private Dictionary<SettingsTab, TabDefinition> index;

	private SettingsTab current = SettingsTab.CONTROLS;

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	public SettingsTab Current
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
				this.signalBus.Dispatch("SettingsTabSelectionModel.SETTINGS_TAB_UPDATED");
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

	public SettingsTabsModel()
	{
		List<TabDefinition> list = new List<TabDefinition>();
		list.Add(new TabDefinition(SettingsTab.CONTROLS, null));
		list.Add(new TabDefinition(SettingsTab.VIDEO, null));
		list.Add(new TabDefinition(SettingsTab.AUDIO, null));
		for (int i = 0; i < list.Count; i++)
		{
			list[i].ordinal = i;
		}
		this.tabs = list.ToArray();
		this.index = new Dictionary<SettingsTab, TabDefinition>();
		TabDefinition[] array = this.tabs;
		for (int j = 0; j < array.Length; j++)
		{
			TabDefinition tabDefinition = array[j];
			this.index[(SettingsTab)tabDefinition.id] = tabDefinition;
		}
	}

	private SettingsTab getDefaultTab()
	{
		return SettingsTab.CONTROLS;
	}

	public void Reset()
	{
		this.Current = this.getDefaultTab();
	}

	public void MapPrefab(SettingsTab id, GameObject prefab)
	{
		this.index[id].prefab = prefab;
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
		this.Current = (SettingsTab)tabDefinition.id;
	}
}
