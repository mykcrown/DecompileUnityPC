// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectPreview : MonoBehaviour
{
	private sealed class _TweenAlpha_c__AnonStorey0
	{
		internal Action callback;

		internal StageSelectPreview _this;

		internal void __m__0(float x)
		{
			this._this.Alpha = x;
		}

		internal void __m__1()
		{
			this._this.KillTween();
			if (this.callback != null)
			{
				this.callback();
			}
		}
	}

	public Image SelectedStagePortrait;

	private bool _inUse;

	private float _alpha;

	private Tweener _tween;

	public bool InUse
	{
		get
		{
			return this._inUse;
		}
		set
		{
			this._inUse = value;
		}
	}

	public float Alpha
	{
		get
		{
			return this._alpha;
		}
		set
		{
			this._alpha = value;
			this.SelectedStagePortrait.CrossFadeAlpha(this._alpha, 0f, true);
		}
	}

	public void TweenAlpha(float alpha, float time)
	{
		this.TweenAlpha(alpha, time, null);
	}

	public void TweenAlpha(float alpha, float time, Action callback)
	{
		StageSelectPreview._TweenAlpha_c__AnonStorey0 _TweenAlpha_c__AnonStorey = new StageSelectPreview._TweenAlpha_c__AnonStorey0();
		_TweenAlpha_c__AnonStorey.callback = callback;
		_TweenAlpha_c__AnonStorey._this = this;
		this.KillTween();
		this._tween = DOTween.To(new DOGetter<float>(this.get_Alpha), new DOSetter<float>(_TweenAlpha_c__AnonStorey.__m__0), alpha, time).SetEase(Ease.Linear).OnComplete(new TweenCallback(_TweenAlpha_c__AnonStorey.__m__1));
	}

	public void KillTween()
	{
		if (this._tween != null)
		{
			if (this._tween.IsPlaying())
			{
				this._tween.Kill(false);
			}
			this._tween = null;
		}
	}

	public void Load(StageData stage)
	{
		this.SelectedStagePortrait.sprite = stage.largePortrait;
	}
}
