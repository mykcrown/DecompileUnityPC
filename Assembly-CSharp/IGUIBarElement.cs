using System;
using UnityEngine;

// Token: 0x020008C1 RID: 2241
public interface IGUIBarElement : ITickable
{
	// Token: 0x06003880 RID: 14464
	void setPosition(float x, float y);

	// Token: 0x17000DA8 RID: 3496
	// (get) Token: 0x06003881 RID: 14465
	bool Visible { get; }

	// Token: 0x17000DA9 RID: 3497
	// (get) Token: 0x06003882 RID: 14466
	Transform transform { get; }
}
