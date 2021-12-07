using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000984 RID: 2436
public class SettingsTabsModel : ISettingsTabsModel
{
	// Token: 0x060041FF RID: 16895 RVA: 0x0012707C File Offset: 0x0012547C
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
		foreach (TabDefinition tabDefinition in this.tabs)
		{
			this.index[(SettingsTab)tabDefinition.id] = tabDefinition;
		}
	}

	// Token: 0x17000F8F RID: 3983
	// (get) Token: 0x06004200 RID: 16896 RVA: 0x00127134 File Offset: 0x00125534
	// (set) Token: 0x06004201 RID: 16897 RVA: 0x0012713C File Offset: 0x0012553C
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x06004202 RID: 16898 RVA: 0x00127145 File Offset: 0x00125545
	private SettingsTab getDefaultTab()
	{
		return SettingsTab.CONTROLS;
	}

	// Token: 0x06004203 RID: 16899 RVA: 0x00127148 File Offset: 0x00125548
	public void Reset()
	{
		this.Current = this.getDefaultTab();
	}

	// Token: 0x17000F90 RID: 3984
	// (get) Token: 0x06004204 RID: 16900 RVA: 0x00127156 File Offset: 0x00125556
	// (set) Token: 0x06004205 RID: 16901 RVA: 0x0012715E File Offset: 0x0012555E
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

	// Token: 0x17000F91 RID: 3985
	// (get) Token: 0x06004206 RID: 16902 RVA: 0x00127183 File Offset: 0x00125583
	public TabDefinition[] Tabs
	{
		get
		{
			return this.tabs;
		}
	}

	// Token: 0x06004207 RID: 16903 RVA: 0x0012718B File Offset: 0x0012558B
	public void MapPrefab(SettingsTab id, GameObject prefab)
	{
		this.index[id].prefab = prefab;
	}

	// Token: 0x06004208 RID: 16904 RVA: 0x001271A0 File Offset: 0x001255A0
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

	// Token: 0x04002C45 RID: 11333
	public const string SETTINGS_TAB_UPDATED = "SettingsTabSelectionModel.SETTINGS_TAB_UPDATED";

	// Token: 0x04002C47 RID: 11335
	private TabDefinition[] tabs;

	// Token: 0x04002C48 RID: 11336
	private Dictionary<SettingsTab, TabDefinition> index;

	// Token: 0x04002C49 RID: 11337
	private SettingsTab current = SettingsTab.CONTROLS;
}
