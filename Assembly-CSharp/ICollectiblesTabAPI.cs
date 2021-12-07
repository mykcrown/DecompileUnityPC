using System;

// Token: 0x020009F2 RID: 2546
public interface ICollectiblesTabAPI
{
	// Token: 0x17001164 RID: 4452
	// (get) Token: 0x060048BD RID: 18621
	bool SkipAnimation { get; }

	// Token: 0x060048BE RID: 18622
	void SetState(CollectiblesTabState state, bool skipAnimation = false);

	// Token: 0x060048BF RID: 18623
	CollectiblesTabState GetState();
}
