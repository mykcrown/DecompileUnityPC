using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200057A RID: 1402
public class DummyAnimationDataOwner : IAnimatorDataOwner
{
	// Token: 0x06001F74 RID: 8052 RVA: 0x000A0C41 File Offset: 0x0009F041
	public DummyAnimationDataOwner(string animationName, int currentFrame)
	{
		this.CurrentAnimationName = animationName;
		this.CurrentFrame = currentFrame;
	}

	// Token: 0x170006D6 RID: 1750
	// (get) Token: 0x06001F75 RID: 8053 RVA: 0x000A0C57 File Offset: 0x0009F057
	// (set) Token: 0x06001F76 RID: 8054 RVA: 0x000A0C5F File Offset: 0x0009F05F
	public string CurrentAnimationName { get; private set; }

	// Token: 0x170006D7 RID: 1751
	// (get) Token: 0x06001F77 RID: 8055 RVA: 0x000A0C68 File Offset: 0x0009F068
	// (set) Token: 0x06001F78 RID: 8056 RVA: 0x000A0C70 File Offset: 0x0009F070
	public int CurrentFrame { get; private set; }

	// Token: 0x06001F79 RID: 8057 RVA: 0x000A0C79 File Offset: 0x0009F079
	public void SetHiddenProps(List<Transform> hiddenProps)
	{
	}
}
