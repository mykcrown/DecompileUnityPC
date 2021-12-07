using System;
using DG.Tweening;

// Token: 0x02000A98 RID: 2712
public static class TweenUtil
{
	// Token: 0x06004F92 RID: 20370 RVA: 0x0014CAE4 File Offset: 0x0014AEE4
	public static void Destroy(ref Tweener target)
	{
		if (target != null && target.IsPlaying())
		{
			target.Kill(false);
		}
		target = null;
	}
}
