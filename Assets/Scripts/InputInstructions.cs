// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;

public class InputInstructions : MonoBehaviour
{
	public CanvasGroup ControllerInstructions;

	public CanvasGroup KeyboardInstructions;

	public CanvasGroup MouseInstuctions;

	public WavedashUIButton MouseInstuctionsButton;

	private ControlMode currentMode = ControlMode.MouseMode;

	private ControlMode lastMode = ControlMode.MouseMode;

	private Tweener _backButtonSwapTween;

	private float alpha;

	private void Awake()
	{
		if (this.ControllerInstructions != null)
		{
			this.ControllerInstructions.gameObject.SetActive(true);
		}
		if (this.KeyboardInstructions != null)
		{
			this.KeyboardInstructions.gameObject.SetActive(true);
		}
		if (this.MouseInstuctions != null)
		{
			this.MouseInstuctions.gameObject.SetActive(true);
		}
		this.setAlpha(1f);
	}

	public void SetControlMode(ControlMode mode)
	{
		if (this.currentMode == mode)
		{
			return;
		}
		this.lastMode = this.currentMode;
		this.currentMode = mode;
		this.killSwapTween();
		this.alpha = 0f;
		this._backButtonSwapTween = DOTween.To(new DOGetter<float>(this.getAlpha), new DOSetter<float>(this.setAlpha), 1f, 0.2f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.killSwapTween));
	}

	private void setAlpha(float alpha)
	{
		if (this.ControllerInstructions != null)
		{
			this.ControllerInstructions.alpha = 0f;
		}
		if (this.KeyboardInstructions != null)
		{
			this.KeyboardInstructions.alpha = 0f;
		}
		if (this.MouseInstuctions != null)
		{
			this.MouseInstuctions.alpha = 0f;
		}
		float num = 1f - alpha;
		ControlMode controlMode = this.lastMode;
		if (controlMode != ControlMode.ControllerMode)
		{
			if (controlMode != ControlMode.KeyboardMode)
			{
				if (controlMode == ControlMode.MouseMode)
				{
					if (this.MouseInstuctions != null)
					{
						this.MouseInstuctions.alpha = num;
					}
				}
			}
			else if (this.KeyboardInstructions != null)
			{
				this.KeyboardInstructions.alpha = num;
			}
		}
		else if (this.ControllerInstructions != null)
		{
			this.ControllerInstructions.alpha = num;
		}
		ControlMode controlMode2 = this.currentMode;
		if (controlMode2 != ControlMode.ControllerMode)
		{
			if (controlMode2 != ControlMode.KeyboardMode)
			{
				if (controlMode2 == ControlMode.MouseMode)
				{
					if (this.MouseInstuctions != null)
					{
						this.MouseInstuctions.alpha = alpha;
					}
				}
			}
			else if (this.KeyboardInstructions != null)
			{
				this.KeyboardInstructions.alpha = alpha;
			}
		}
		else if (this.ControllerInstructions != null)
		{
			this.ControllerInstructions.alpha = alpha;
		}
		this.alpha = alpha;
	}

	private float getAlpha()
	{
		return this.alpha;
	}

	private void killSwapTween()
	{
		TweenUtil.Destroy(ref this._backButtonSwapTween);
	}
}
