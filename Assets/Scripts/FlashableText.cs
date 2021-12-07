// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class FlashableText : MonoBehaviour
{
	private float flashValue;

	private Color baseColor;

	private Color flashColor;

	private Tweener flashTween;

	private bool isInit;

	private TextMeshProUGUI textField;

	public float ErrorTextFlash
	{
		get
		{
			return this.flashValue;
		}
		set
		{
			this.flashValue = value;
			this.flashColor = this.textField.color;
			this.flashColor.r = this.baseColor.r + (1f - this.baseColor.r) * this.flashValue;
			this.flashColor.b = this.baseColor.b + (1f - this.baseColor.b) * this.flashValue;
			this.flashColor.g = this.baseColor.g + (1f - this.baseColor.g) * this.flashValue;
			this.textField.color = this.flashColor;
		}
	}

	private void Awake()
	{
		this.init();
	}

	private void init()
	{
		if (!this.isInit)
		{
			this.isInit = true;
			this.textField = base.GetComponent<TextMeshProUGUI>();
			this.baseColor = default(Color);
			this.baseColor.a = this.textField.color.a;
			this.baseColor.r = this.textField.color.r;
			this.baseColor.b = this.textField.color.b;
			this.baseColor.g = this.textField.color.g;
		}
	}

	public void Flash()
	{
		this.init();
		this.killErrorTextFlashTween();
		this.ErrorTextFlash = 1f;
		this.flashTween = DOTween.To(new DOGetter<float>(this.get_ErrorTextFlash), new DOSetter<float>(this._Flash_m__0), 0f, 0.2f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.killErrorTextFlashTween));
	}

	private void killErrorTextFlashTween()
	{
		TweenUtil.Destroy(ref this.flashTween);
	}

	private void _Flash_m__0(float x)
	{
		this.ErrorTextFlash = x;
	}
}
