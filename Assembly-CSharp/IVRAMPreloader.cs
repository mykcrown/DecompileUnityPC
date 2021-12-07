using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006C8 RID: 1736
public interface IVRAMPreloader
{
	// Token: 0x06002B88 RID: 11144
	void DebugPreload(Camera myCamera, List<GameObject> preloadObjects, Action callback);

	// Token: 0x06002B89 RID: 11145
	void Preload(Camera myCamera, List<GameObject> preloadObjects, Action callback);
}
