using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000931 RID: 2353
public class ButtonAnimator
{
	// Token: 0x06003DDB RID: 15835 RVA: 0x0011B464 File Offset: 0x00119864
	public ButtonAnimator(IAnimatableButton button)
	{
		this.button = button;
	}

	// Token: 0x17000EBE RID: 3774
	// (get) Token: 0x06003DDC RID: 15836 RVA: 0x0011B473 File Offset: 0x00119873
	// (set) Token: 0x06003DDD RID: 15837 RVA: 0x0011B47B File Offset: 0x0011987B
	public ButtonAnimator.VisualDisableType DisableType { get; set; }

	// Token: 0x17000EBF RID: 3775
	// (get) Token: 0x06003DDE RID: 15838 RVA: 0x0011B484 File Offset: 0x00119884
	// (set) Token: 0x06003DDF RID: 15839 RVA: 0x0011B48C File Offset: 0x0011988C
	public float DisableDuration { get; set; }

	// Token: 0x06003DE0 RID: 15840 RVA: 0x0011B498 File Offset: 0x00119898
	public void PlayDisable()
	{
		this.killButtonDisableTween();
		this._disableTween = DOTween.To(new DOGetter<float>(this.get_visualDisableValue), delegate(float x)
		{
			this.visualDisableValue = x;
		}, 1f, this.DisableDuration).SetEase(Ease.OutSine).OnComplete(delegate
		{
			this.killButtonDisableTween();
		});
	}

	// Token: 0x06003DE1 RID: 15841 RVA: 0x0011B4F0 File Offset: 0x001198F0
	public void PlayEnable()
	{
		this.killButtonDisableTween();
		this._disableTween = DOTween.To(new DOGetter<float>(this.get_visualDisableValue), delegate(float x)
		{
			this.visualDisableValue = x;
		}, 0f, this.DisableDuration).SetEase(Ease.OutSine).OnComplete(delegate
		{
			this.killButtonDisableTween();
		});
	}

	// Token: 0x06003DE2 RID: 15842 RVA: 0x0011B548 File Offset: 0x00119948
	public void OnDestroy()
	{
		this.killButtonDisableTween();
	}

	// Token: 0x06003DE3 RID: 15843 RVA: 0x0011B550 File Offset: 0x00119950
	private void killButtonDisableTween()
	{
		TweenUtil.Destroy(ref this._disableTween);
	}

	// Token: 0x17000EC0 RID: 3776
	// (get) Token: 0x06003DE4 RID: 15844 RVA: 0x0011B55D File Offset: 0x0011995D
	// (set) Token: 0x06003DE5 RID: 15845 RVA: 0x0011B568 File Offset: 0x00119968
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
						foreach (Image image in this.button.AdditionalImagesGet)
						{
							image.color = color;
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
						foreach (Image image2 in this.button.AdditionalImagesGet)
						{
							image2.color = new Color(num2, num2, num2);
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

	// Token: 0x04002A0C RID: 10764
	private IAnimatableButton button;

	// Token: 0x04002A0D RID: 10765
	private float _normalizedButtonDisableValue;

	// Token: 0x04002A0E RID: 10766
	private Tweener _disableTween;

	// Token: 0x04002A0F RID: 10767
	private const float GREY_AMOUNT = 0.5f;

	// Token: 0x02000932 RID: 2354
	public enum VisualDisableType
	{
		// Token: 0x04002A11 RID: 10769
		Fade,
		// Token: 0x04002A12 RID: 10770
		Grey,
		// Token: 0x04002A13 RID: 10771
		DisappearCanvas,
		// Token: 0x04002A14 RID: 10772
		None
	}
}
