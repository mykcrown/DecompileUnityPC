// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimator
{
	public enum VisualDisableType
	{
		Fade,
		Grey,
		DisappearCanvas,
		None
	}

	private IAnimatableButton button;

	private float _normalizedButtonDisableValue;

	private Tweener _disableTween;

	private const float GREY_AMOUNT = 0.5f;

	public ButtonAnimator.VisualDisableType DisableType
	{
		get;
		set;
	}

	public float DisableDuration
	{
		get;
		set;
	}

	private float visualDisableValue
	{
		get
		{
			return this._normalizedButtonDisableValue;
		}
		set
		{
			this._normalizedButtonDisableValue = value;
			float num = 1f - this._normalizedButtonDisableValue;
			switch (this.DisableType)
			{
			case ButtonAnimator.VisualDisableType.Fade:
				if (this.button.ButtonBackgroundGet != null)
				{
					Color color = this.button.ButtonBackgroundGet.color;
					color.a = num;
					this.button.ButtonBackgroundGet.color = color;
					if (this.button.AdditionalImagesGet != null)
					{
						foreach (Image current in this.button.AdditionalImagesGet)
						{
							current.color = color;
						}
					}
				}
				if (this.button.TextFieldGet != null)
				{
					this.button.TextFieldGet.alpha = num;
				}
				break;
			case ButtonAnimator.VisualDisableType.Grey:
			{
				float num2 = 0.5f + num * 0.5f;
				if (this.button.ButtonBackgroundGet != null)
				{
					this.button.ButtonBackgroundGet.color = new Color(num2, num2, num2);
					if (this.button.AdditionalImagesGet != null)
					{
						foreach (Image current2 in this.button.AdditionalImagesGet)
						{
							current2.color = new Color(num2, num2, num2);
						}
					}
				}
				if (this.button.TextFieldGet != null)
				{
					this.button.TextFieldGet.alpha = num2;
				}
				break;
			}
			case ButtonAnimator.VisualDisableType.DisappearCanvas:
				this.button.FadeCanvasGet.alpha = num;
				break;
			}
		}
	}

	public ButtonAnimator(IAnimatableButton button)
	{
		this.button = button;
	}

	public void PlayDisable()
	{
		this.killButtonDisableTween();
		this._disableTween = DOTween.To(new DOGetter<float>(this.get_visualDisableValue), new DOSetter<float>(this._PlayDisable_m__0), 1f, this.DisableDuration).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this._PlayDisable_m__1));
	}

	public void PlayEnable()
	{
		this.killButtonDisableTween();
		this._disableTween = DOTween.To(new DOGetter<float>(this.get_visualDisableValue), new DOSetter<float>(this._PlayEnable_m__2), 0f, this.DisableDuration).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this._PlayEnable_m__3));
	}

	public void OnDestroy()
	{
		this.killButtonDisableTween();
	}

	private void killButtonDisableTween()
	{
		TweenUtil.Destroy(ref this._disableTween);
	}

	private void _PlayDisable_m__0(float x)
	{
		this.visualDisableValue = x;
	}

	private void _PlayDisable_m__1()
	{
		this.killButtonDisableTween();
	}

	private void _PlayEnable_m__2(float x)
	{
		this.visualDisableValue = x;
	}

	private void _PlayEnable_m__3()
	{
		this.killButtonDisableTween();
	}
}
