using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000579 RID: 1401
public interface IAnimatorDataOwner
{
	// Token: 0x170006D4 RID: 1748
	// (get) Token: 0x06001F71 RID: 8049
	string CurrentAnimationName { get; }

	// Token: 0x170006D5 RID: 1749
	// (get) Token: 0x06001F72 RID: 8050
	int CurrentFrame { get; }

	// Token: 0x06001F73 RID: 8051
	void SetHiddenProps(List<Transform> hiddenProps);
}
