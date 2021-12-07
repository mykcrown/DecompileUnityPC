// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;

public class GamewideOverlayTransitionManager
{
	private sealed class _standardFadeIn_c__AnonStorey0
	{
		internal BaseGamewideOverlay target;

		internal float __m__0()
		{
			return this.target.Alpha;
		}

		internal void __m__1(float valueIn)
		{
			this.target.Alpha = valueIn;
		}
	}

	private sealed class _standardFadeOut_c__AnonStorey1
	{
		internal BaseGamewideOverlay target;

		internal Action callback;

		internal float __m__0()
		{
			return this.target.Alpha;
		}

		internal void __m__1(float valueIn)
		{
			this.target.Alpha = valueIn;
		}

		internal void __m__2()
		{
			this.target.KillTween();
			if (this.callback != null)
			{
				this.callback();
			}
		}
	}

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

	private void standardFadeIn(BaseGamewideOverlay target)
	{
		GamewideOverlayTransitionManager._standardFadeIn_c__AnonStorey0 _standardFadeIn_c__AnonStorey = new GamewideOverlayTransitionManager._standardFadeIn_c__AnonStorey0();
		_standardFadeIn_c__AnonStorey.target = target;
		_standardFadeIn_c__AnonStorey.target.KillTween();
		_standardFadeIn_c__AnonStorey.target.Alpha = 0f;
		_standardFadeIn_c__AnonStorey.target.TransitionTween = DOTween.To(new DOGetter<float>(_standardFadeIn_c__AnonStorey.__m__0), new DOSetter<float>(_standardFadeIn_c__AnonStorey.__m__1), 1f, 0.07f).SetEase(Ease.Linear).OnComplete(new TweenCallback(_standardFadeIn_c__AnonStorey.target.KillTween));
	}

	private void standardFadeOut(BaseGamewideOverlay target, Action callback)
	{
		GamewideOverlayTransitionManager._standardFadeOut_c__AnonStorey1 _standardFadeOut_c__AnonStorey = new GamewideOverlayTransitionManager._standardFadeOut_c__AnonStorey1();
		_standardFadeOut_c__AnonStorey.target = target;
		_standardFadeOut_c__AnonStorey.callback = callback;
		_standardFadeOut_c__AnonStorey.target.KillTween();
		_standardFadeOut_c__AnonStorey.target.TransitionTween = DOTween.To(new DOGetter<float>(_standardFadeOut_c__AnonStorey.__m__0), new DOSetter<float>(_standardFadeOut_c__AnonStorey.__m__1), 0f, 0.05f).SetEase(Ease.Linear).OnComplete(new TweenCallback(_standardFadeOut_c__AnonStorey.__m__2));
	}
}
