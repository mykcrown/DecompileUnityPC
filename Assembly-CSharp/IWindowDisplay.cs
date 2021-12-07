using System;
using UnityEngine;

// Token: 0x02000669 RID: 1641
public interface IWindowDisplay
{
	// Token: 0x060028A7 RID: 10407
	T Add<T>(GameObject prefab, WindowTransition transition, bool pushToBack = false, bool closeOnScreenChange = false, bool useOverrideOpenSound = false, AudioData overrideOpenSound = default(AudioData)) where T : BaseWindow;

	// Token: 0x060028A8 RID: 10408
	void Remove(BaseWindow window);

	// Token: 0x060028A9 RID: 10409
	int GetWindowCount();
}
