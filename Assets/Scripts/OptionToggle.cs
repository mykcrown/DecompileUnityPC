// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class OptionToggle : ClientBehavior
{
	public MenuItemButton Button;

	public RectTransform SwitchToggle;

	public TextMeshProUGUI Title;

	private Tweener _switchTween;

	private bool currentValue;

	private bool hasIntialized;

	public bool Value()
	{
		return this.currentValue;
	}

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
		this._switchTween = DOTween.To(new DOGetter<Vector3>(this._SetToggle_m__0), new DOSetter<Vector3>(this._SetToggle_m__1), zero, duration).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.killSwitchTween));
	}

	private void killSwitchTween()
	{
		TweenUtil.Destroy(ref this._switchTween);
	}

	private Vector3 _SetToggle_m__0()
	{
		return this.SwitchToggle.localPosition;
	}

	private void _SetToggle_m__1(Vector3 x)
	{
		this.SwitchToggle.localPosition = x;
	}
}
