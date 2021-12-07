// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;

public class BackButtonMenuMode : MonoBehaviour
{
	public CanvasGroup MouseModeGroup;

	public CanvasGroup ControllerModeGroup;

	public WavedashUIButton BackButton;

	private bool isMouseMode = true;

	private Tweener _backButtonSwapTween;

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

	private void SetMouseAlpha(float alpha)
	{
		this.MouseModeGroup.alpha = alpha;
		this.ControllerModeGroup.alpha = 1f - alpha;
	}

	private float GetMouseAlpha()
	{
		return this.MouseModeGroup.alpha;
	}

	private void killSwapTween()
	{
		TweenUtil.Destroy(ref this._backButtonSwapTween);
	}
}
