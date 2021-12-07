using System;
using UnityEngine;

// Token: 0x02000985 RID: 2437
public interface ISettingsTabsModel
{
	// Token: 0x06004209 RID: 16905
	void MapPrefab(SettingsTab id, GameObject prefab);

	// Token: 0x0600420A RID: 16906
	void Reset();

	// Token: 0x17000F92 RID: 3986
	// (get) Token: 0x0600420B RID: 16907
	// (set) Token: 0x0600420C RID: 16908
	SettingsTab Current { get; set; }

	// Token: 0x17000F93 RID: 3987
	// (get) Token: 0x0600420D RID: 16909
	TabDefinition[] Tabs { get; }

	// Token: 0x0600420E RID: 16910
	void Shift(int amount);
}
