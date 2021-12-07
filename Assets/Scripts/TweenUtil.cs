// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using System;

public static class TweenUtil
{
	public static void Destroy(ref Tweener target)
	{
		if (target != null && target.IsPlaying())
		{
			target.Kill(false);
		}
		target = null;
	}
}
