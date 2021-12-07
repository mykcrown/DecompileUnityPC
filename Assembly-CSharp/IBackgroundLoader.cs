using System;
using UnityEngine;

// Token: 0x020006BD RID: 1725
public interface IBackgroundLoader
{
	// Token: 0x06002B71 RID: 11121
	void LoadBakedAnimations(MonoBehaviour host);

	// Token: 0x06002B72 RID: 11122
	void WaitForSetup(Action callback);

	// Token: 0x06002B73 RID: 11123
	void OnApplicationQuit();

	// Token: 0x17000AB9 RID: 2745
	// (get) Token: 0x06002B74 RID: 11124
	bool BakedAnimationDataLoaded { get; }
}
