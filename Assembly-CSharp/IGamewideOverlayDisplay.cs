using System;
using UnityEngine;

// Token: 0x0200066A RID: 1642
public interface IGamewideOverlayDisplay
{
	// Token: 0x060028AA RID: 10410
	T AddGamewideOverlay<T>(GameObject prefab, WindowTransition transition) where T : BaseGamewideOverlay;

	// Token: 0x060028AB RID: 10411
	void Remove(BaseGamewideOverlay overlay);

	// Token: 0x060028AC RID: 10412
	int GetGamewideOverlayCount();
}
