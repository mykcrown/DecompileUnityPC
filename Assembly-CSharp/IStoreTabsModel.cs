using System;
using UnityEngine;

// Token: 0x02000A33 RID: 2611
public interface IStoreTabsModel
{
	// Token: 0x06004C78 RID: 19576
	void MapPrefab(StoreTab id, GameObject prefab);

	// Token: 0x17001226 RID: 4646
	// (get) Token: 0x06004C79 RID: 19577
	bool ShowTabs { get; }

	// Token: 0x06004C7A RID: 19578
	void Reset();

	// Token: 0x17001227 RID: 4647
	// (get) Token: 0x06004C7B RID: 19579
	// (set) Token: 0x06004C7C RID: 19580
	StoreTab Current { get; set; }

	// Token: 0x17001228 RID: 4648
	// (get) Token: 0x06004C7D RID: 19581
	TabDefinition[] Tabs { get; }

	// Token: 0x06004C7E RID: 19582
	void Shift(int amount);
}
