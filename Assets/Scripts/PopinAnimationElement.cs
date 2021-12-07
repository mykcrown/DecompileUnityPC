// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PopinAnimationElement : MonoBehaviour
{
	private static float ANIMATION_DURATION = 0.25f;

	private RectTransform rectTransform;

	private LayoutElement layoutElement;

	private CanvasGroup canvasGroup;

	private bool isVisible;

	private bool hasBeenIntialized;

	private float initialHeight;

	private Tweener _tween;

	private float _visibleValue;

	private float visibleValue
	{
		get
		{
			return this._visibleValue;
		}
		set
		{
			if (this.layoutElement != null)
			{
				this.layoutElement.preferredHeight = this.initialHeight * value;
			}
			this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.initialHeight * value);
			this.canvasGroup.alpha = value * value * value;
			this._visibleValue = value;
		}
	}

	private void Awake()
	{
		this.rectTransform = base.GetComponent<RectTransform>();
		this.layoutElement = base.GetComponent<LayoutElement>();
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.initialHeight = this.rectTransform.rect.height;
	}

	public void SetVisible(bool isVisible)
	{
		if (isVisible == this.isVisible && this.hasBeenIntialized)
		{
			return;
		}
		this.isVisible = isVisible;
		this.killTween();
		float num = (float)((!isVisible) ? 0 : 1);
		if (this.hasBeenIntialized)
		{
			this._tween = DOTween.To(new DOGetter<float>(this.get_visibleValue), new DOSetter<float>(this._SetVisible_m__0), num, PopinAnimationElement.ANIMATION_DURATION).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this._SetVisible_m__1));
		}
		else
		{
			this.visibleValue = num;
		}
		this.hasBeenIntialized = true;
	}

	private void killTween()
	{
		TweenUtil.Destroy(ref this._tween);
	}

	private void _SetVisible_m__0(float x)
	{
		this.visibleValue = x;
	}

	private void _SetVisible_m__1()
	{
		this.killTween();
	}
}
