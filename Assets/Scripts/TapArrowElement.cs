// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TapArrowElement : MonoBehaviour
{
	public CanvasGroup CanvasGroup;

	private Tweener tween;

	private float _alpha;

	public float Alpha
	{
		get
		{
			return this._alpha;
		}
		set
		{
			this._alpha = value;
			this.CanvasGroup.alpha = this._alpha;
		}
	}

	public void Init()
	{
		this.Alpha = 0f;
	}

	public void Play(float alphaDuration)
	{
		this.Alpha = 1f;
		this.tween = DOTween.To(new DOGetter<float>(this.get_Alpha), new DOSetter<float>(this._Play_m__0), 0f, alphaDuration).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.killTween));
	}

	public bool IsActive()
	{
		return this.tween != null && this.tween.IsActive();
	}

	private void killTween()
	{
		TweenUtil.Destroy(ref this.tween);
	}

	private void _Play_m__0(float x)
	{
		this.Alpha = x;
	}
}
