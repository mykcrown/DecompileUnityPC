using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

// Token: 0x02000952 RID: 2386
public class FlashableText : MonoBehaviour
{
	// Token: 0x06003F5E RID: 16222 RVA: 0x00120413 File Offset: 0x0011E813
	private void Awake()
	{
		this.init();
	}

	// Token: 0x06003F5F RID: 16223 RVA: 0x0012041C File Offset: 0x0011E81C
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

	// Token: 0x06003F60 RID: 16224 RVA: 0x001204D0 File Offset: 0x0011E8D0
	public void Flash()
	{
		this.init();
		this.killErrorTextFlashTween();
		this.ErrorTextFlash = 1f;
		this.flashTween = DOTween.To(new DOGetter<float>(this.get_ErrorTextFlash), delegate(float x)
		{
			this.ErrorTextFlash = x;
		}, 0f, 0.2f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.killErrorTextFlashTween));
	}

	// Token: 0x06003F61 RID: 16225 RVA: 0x00120538 File Offset: 0x0011E938
	private void killErrorTextFlashTween()
	{
		TweenUtil.Destroy(ref this.flashTween);
	}

	// Token: 0x17000F04 RID: 3844
	// (get) Token: 0x06003F62 RID: 16226 RVA: 0x00120545 File Offset: 0x0011E945
	// (set) Token: 0x06003F63 RID: 16227 RVA: 0x00120550 File Offset: 0x0011E950
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

	// Token: 0x04002AFD RID: 11005
	private float flashValue;

	// Token: 0x04002AFE RID: 11006
	private Color baseColor;

	// Token: 0x04002AFF RID: 11007
	private Color flashColor;

	// Token: 0x04002B00 RID: 11008
	private Tweener flashTween;

	// Token: 0x04002B01 RID: 11009
	private bool isInit;

	// Token: 0x04002B02 RID: 11010
	private TextMeshProUGUI textField;
}
