using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000925 RID: 2341
public class PopinAnimationElement : MonoBehaviour
{
	// Token: 0x06003D14 RID: 15636 RVA: 0x0011A7F8 File Offset: 0x00118BF8
	private void Awake()
	{
		this.rectTransform = base.GetComponent<RectTransform>();
		this.layoutElement = base.GetComponent<LayoutElement>();
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.initialHeight = this.rectTransform.rect.height;
	}

	// Token: 0x06003D15 RID: 15637 RVA: 0x0011A844 File Offset: 0x00118C44
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
			this._tween = DOTween.To(new DOGetter<float>(this.get_visibleValue), delegate(float x)
			{
				this.visibleValue = x;
			}, num, PopinAnimationElement.ANIMATION_DURATION).SetEase(Ease.OutSine).OnComplete(delegate
			{
				this.killTween();
			});
		}
		else
		{
			this.visibleValue = num;
		}
		this.hasBeenIntialized = true;
	}

	// Token: 0x06003D16 RID: 15638 RVA: 0x0011A8E3 File Offset: 0x00118CE3
	private void killTween()
	{
		TweenUtil.Destroy(ref this._tween);
	}

	// Token: 0x17000E9D RID: 3741
	// (get) Token: 0x06003D17 RID: 15639 RVA: 0x0011A8F0 File Offset: 0x00118CF0
	// (set) Token: 0x06003D18 RID: 15640 RVA: 0x0011A8F8 File Offset: 0x00118CF8
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

	// Token: 0x040029AF RID: 10671
	private static float ANIMATION_DURATION = 0.25f;

	// Token: 0x040029B0 RID: 10672
	private RectTransform rectTransform;

	// Token: 0x040029B1 RID: 10673
	private LayoutElement layoutElement;

	// Token: 0x040029B2 RID: 10674
	private CanvasGroup canvasGroup;

	// Token: 0x040029B3 RID: 10675
	private bool isVisible;

	// Token: 0x040029B4 RID: 10676
	private bool hasBeenIntialized;

	// Token: 0x040029B5 RID: 10677
	private float initialHeight;

	// Token: 0x040029B6 RID: 10678
	private Tweener _tween;

	// Token: 0x040029B7 RID: 10679
	private float _visibleValue;
}
