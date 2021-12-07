using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

// Token: 0x0200093D RID: 2365
public class GamewideOverlayTransitionManager
{
	// Token: 0x06003E7C RID: 15996 RVA: 0x0011CB0B File Offset: 0x0011AF0B
	public void ShowInTransition(BaseGamewideOverlay target, WindowTransition transition)
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

	// Token: 0x06003E7D RID: 15997 RVA: 0x0011CB30 File Offset: 0x0011AF30
	public void ShowOutTransition(BaseGamewideOverlay target, WindowTransition transition, Action callback)
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

	// Token: 0x06003E7E RID: 15998 RVA: 0x0011CB5C File Offset: 0x0011AF5C
	private void standardFadeIn(BaseGamewideOverlay target)
	{
		target.KillTween();
		target.Alpha = 0f;
		target.TransitionTween = DOTween.To(() => target.Alpha, delegate(float valueIn)
		{
			target.Alpha = valueIn;
		}, 1f, 0.07f).SetEase(Ease.Linear).OnComplete(new TweenCallback(target.KillTween));
	}

	// Token: 0x06003E7F RID: 15999 RVA: 0x0011CBE0 File Offset: 0x0011AFE0
	private void standardFadeOut(BaseGamewideOverlay target, Action callback)
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
