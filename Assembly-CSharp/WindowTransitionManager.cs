using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

// Token: 0x02000961 RID: 2401
public class WindowTransitionManager
{
	// Token: 0x0600401D RID: 16413 RVA: 0x00120F54 File Offset: 0x0011F354
	public void ShowInTransition(BaseWindow target, WindowTransition transition)
	{
		if (transition != WindowTransition.STANDARD_FADE)
		{
			target.KillTween();
		}
		else
		{
			this.standardFadeIn(target);
		}
	}

	// Token: 0x0600401E RID: 16414 RVA: 0x00120F79 File Offset: 0x0011F379
	public void ShowOutTransition(BaseWindow target, WindowTransition transition, Action callback)
	{
		if (transition != WindowTransition.STANDARD_FADE)
		{
			target.KillTween();
			callback();
		}
		else
		{
			this.standardFadeOut(target, callback);
		}
	}

	// Token: 0x0600401F RID: 16415 RVA: 0x00120FA8 File Offset: 0x0011F3A8
	private void standardFadeIn(BaseWindow target)
	{
		target.KillTween();
		target.Alpha = 0f;
		target.TransitionTween = DOTween.To(() => target.Alpha, delegate(float valueIn)
		{
			target.Alpha = valueIn;
		}, 1f, 0.07f).SetEase(Ease.Linear).OnComplete(new TweenCallback(target.KillTween));
	}

	// Token: 0x06004020 RID: 16416 RVA: 0x0012102C File Offset: 0x0011F42C
	private void standardFadeOut(BaseWindow target, Action callback)
	{
		target.KillTween();
		target.TransitionTween = DOTween.To(() => target.Alpha, delegate(float valueIn)
		{
			target.Alpha = valueIn;
		}, 0f, 0.05f).SetEase(Ease.Linear).OnComplete(delegate
		{
			target.KillTween();
			if (callback != null)
			{
				callback();
			}
		});
	}
}
