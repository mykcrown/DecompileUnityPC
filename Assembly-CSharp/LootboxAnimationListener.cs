using System;
using UnityEngine;

// Token: 0x02000298 RID: 664
public class LootboxAnimationListener : MonoBehaviour
{
	// Token: 0x06000E06 RID: 3590 RVA: 0x000588FB File Offset: 0x00056CFB
	public void OnOpenFinished()
	{
		this.director.OnOpenFinished();
	}

	// Token: 0x0400083F RID: 2111
	public ClickToOpenLoot director;
}
