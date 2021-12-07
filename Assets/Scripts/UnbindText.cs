// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class UnbindText : MonoBehaviour
{
	public TextMeshProUGUI Text;

	private Tweener _flashTween;

	private float alphaDuration;

	private float showDuration;

	private float textAlpha
	{
		get
		{
			return this.Text.color.a;
		}
		set
		{
			Color color = this.Text.color;
			color.a = value;
			this.Text.color = color;
		}
	}

	public void Awake()
	{
		this.textAlpha = 0f;
	}

	public void FlashText(string text, float delay, float alphaDuration = 1f, float showDuration = 1f)
	{
		this.killFlashTween();
		this.alphaDuration = alphaDuration;
		this.showDuration = showDuration;
		this.Text.text = text;
		this.textAlpha = 0f;
		this._flashTween = DOTween.To(new DOGetter<float>(this.get_textAlpha), new DOSetter<float>(this._FlashText_m__0), 1f, this.alphaDuration).SetEase(Ease.OutQuart).SetDelay(delay).OnComplete(new TweenCallback(this.tweenOut));
	}

	private void tweenOut()
	{
		this.killFlashTween();
		this._flashTween = DOTween.To(new DOGetter<float>(this.get_textAlpha), new DOSetter<float>(this._tweenOut_m__1), 0f, this.alphaDuration).SetEase(Ease.OutQuart).SetDelay(this.showDuration).OnComplete(new TweenCallback(this.killFlashTween));
	}

	private void killFlashTween()
	{
		TweenUtil.Destroy(ref this._flashTween);
	}

	private void _FlashText_m__0(float x)
	{
		this.textAlpha = x;
	}

	private void _tweenOut_m__1(float x)
	{
		this.textAlpha = x;
	}
}
