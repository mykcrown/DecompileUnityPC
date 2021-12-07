using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x020008A8 RID: 2216
public class BackButtonMenuMode : MonoBehaviour
{
	// Token: 0x06003780 RID: 14208 RVA: 0x00102E30 File Offset: 0x00101230
	public void SetMouseMode(bool isMouseMode)
	{
		if (this.isMouseMode == isMouseMode)
		{
			return;
		}
		this.killSwapTween();
		float endValue = (!isMouseMode) ? 0f : 1f;
		this._backButtonSwapTween = DOTween.To(new DOGetter<float>(this.GetMouseAlpha), new DOSetter<float>(this.SetMouseAlpha), endValue, 0.1f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.killSwapTween));
		this.isMouseMode = isMouseMode;
	}

	// Token: 0x06003781 RID: 14209 RVA: 0x00102EAD File Offset: 0x001012AD
	private void SetMouseAlpha(float alpha)
	{
		this.MouseModeGroup.alpha = alpha;
		this.ControllerModeGroup.alpha = 1f - alpha;
	}

	// Token: 0x06003782 RID: 14210 RVA: 0x00102ECD File Offset: 0x001012CD
	private float GetMouseAlpha()
	{
		return this.MouseModeGroup.alpha;
	}

	// Token: 0x06003783 RID: 14211 RVA: 0x00102EDA File Offset: 0x001012DA
	private void killSwapTween()
	{
		TweenUtil.Destroy(ref this._backButtonSwapTween);
	}

	// Token: 0x040025AD RID: 9645
	public CanvasGroup MouseModeGroup;

	// Token: 0x040025AE RID: 9646
	public CanvasGroup ControllerModeGroup;

	// Token: 0x040025AF RID: 9647
	public WavedashUIButton BackButton;

	// Token: 0x040025B0 RID: 9648
	private bool isMouseMode = true;

	// Token: 0x040025B1 RID: 9649
	private Tweener _backButtonSwapTween;
}
