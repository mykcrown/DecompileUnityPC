using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

// Token: 0x02000974 RID: 2420
public class OptionToggle : ClientBehavior
{
	// Token: 0x060040EF RID: 16623 RVA: 0x00124A66 File Offset: 0x00122E66
	public bool Value()
	{
		return this.currentValue;
	}

	// Token: 0x060040F0 RID: 16624 RVA: 0x00124A70 File Offset: 0x00122E70
	public void SetToggle(bool isOn)
	{
		if (this.currentValue == isOn && this.hasIntialized)
		{
			return;
		}
		this.currentValue = isOn;
		float duration;
		if (this.hasIntialized)
		{
			if (isOn)
			{
				base.audioManager.PlayMenuSound(SoundKey.generic_toggleOn, 0f);
			}
			else
			{
				base.audioManager.PlayMenuSound(SoundKey.generic_toggleOff, 0f);
			}
			duration = 0.1f;
		}
		else
		{
			duration = 0f;
		}
		this.hasIntialized = true;
		this.killSwitchTween();
		Vector3 zero;
		if (isOn)
		{
			zero = Vector3.zero;
		}
		else
		{
			zero = new Vector3(-this.SwitchToggle.rect.width, 0f, 0f);
		}
		this._switchTween = DOTween.To(() => this.SwitchToggle.localPosition, delegate(Vector3 x)
		{
			this.SwitchToggle.localPosition = x;
		}, zero, duration).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.killSwitchTween));
	}

	// Token: 0x060040F1 RID: 16625 RVA: 0x00124B65 File Offset: 0x00122F65
	private void killSwitchTween()
	{
		TweenUtil.Destroy(ref this._switchTween);
	}

	// Token: 0x04002BCD RID: 11213
	public MenuItemButton Button;

	// Token: 0x04002BCE RID: 11214
	public RectTransform SwitchToggle;

	// Token: 0x04002BCF RID: 11215
	public TextMeshProUGUI Title;

	// Token: 0x04002BD0 RID: 11216
	private Tweener _switchTween;

	// Token: 0x04002BD1 RID: 11217
	private bool currentValue;

	// Token: 0x04002BD2 RID: 11218
	private bool hasIntialized;
}
