using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x02000966 RID: 2406
public class InputInstructions : MonoBehaviour
{
	// Token: 0x06004026 RID: 16422 RVA: 0x00121120 File Offset: 0x0011F520
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

	// Token: 0x06004027 RID: 16423 RVA: 0x001211A0 File Offset: 0x0011F5A0
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

	// Token: 0x06004028 RID: 16424 RVA: 0x00121224 File Offset: 0x0011F624
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

	// Token: 0x06004029 RID: 16425 RVA: 0x001213AF File Offset: 0x0011F7AF
	private float getAlpha()
	{
		return this.alpha;
	}

	// Token: 0x0600402A RID: 16426 RVA: 0x001213B7 File Offset: 0x0011F7B7
	private void killSwapTween()
	{
		TweenUtil.Destroy(ref this._backButtonSwapTween);
	}

	// Token: 0x04002B4A RID: 11082
	public CanvasGroup ControllerInstructions;

	// Token: 0x04002B4B RID: 11083
	public CanvasGroup KeyboardInstructions;

	// Token: 0x04002B4C RID: 11084
	public CanvasGroup MouseInstuctions;

	// Token: 0x04002B4D RID: 11085
	public WavedashUIButton MouseInstuctionsButton;

	// Token: 0x04002B4E RID: 11086
	private ControlMode currentMode = ControlMode.MouseMode;

	// Token: 0x04002B4F RID: 11087
	private ControlMode lastMode = ControlMode.MouseMode;

	// Token: 0x04002B50 RID: 11088
	private Tweener _backButtonSwapTween;

	// Token: 0x04002B51 RID: 11089
	private float alpha;
}
