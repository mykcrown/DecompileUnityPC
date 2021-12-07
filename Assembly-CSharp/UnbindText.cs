using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

// Token: 0x02000979 RID: 2425
public class UnbindText : MonoBehaviour
{
	// Token: 0x17000F53 RID: 3923
	// (get) Token: 0x0600410C RID: 16652 RVA: 0x00125068 File Offset: 0x00123468
	// (set) Token: 0x0600410D RID: 16653 RVA: 0x00125088 File Offset: 0x00123488
	private float textAlpha
	{
		get
		{
			return this.Text.color.a;
		}
		set
		{
			Color color = this.Text.color;
			color.a = value;
			this.Text.color = color;
		}
	}

	// Token: 0x0600410E RID: 16654 RVA: 0x001250B5 File Offset: 0x001234B5
	public void Awake()
	{
		this.textAlpha = 0f;
	}

	// Token: 0x0600410F RID: 16655 RVA: 0x001250C4 File Offset: 0x001234C4
	public void FlashText(string text, float delay, float alphaDuration = 1f, float showDuration = 1f)
	{
		this.killFlashTween();
		this.alphaDuration = alphaDuration;
		this.showDuration = showDuration;
		this.Text.text = text;
		this.textAlpha = 0f;
		this._flashTween = DOTween.To(new DOGetter<float>(this.get_textAlpha), delegate(float x)
		{
			this.textAlpha = x;
		}, 1f, this.alphaDuration).SetEase(Ease.OutQuart).SetDelay(delay).OnComplete(new TweenCallback(this.tweenOut));
	}

	// Token: 0x06004110 RID: 16656 RVA: 0x0012514C File Offset: 0x0012354C
	private void tweenOut()
	{
		this.killFlashTween();
		this._flashTween = DOTween.To(new DOGetter<float>(this.get_textAlpha), delegate(float x)
		{
			this.textAlpha = x;
		}, 0f, this.alphaDuration).SetEase(Ease.OutQuart).SetDelay(this.showDuration).OnComplete(new TweenCallback(this.killFlashTween));
	}

	// Token: 0x06004111 RID: 16657 RVA: 0x001251B0 File Offset: 0x001235B0
	private void killFlashTween()
	{
		TweenUtil.Destroy(ref this._flashTween);
	}

	// Token: 0x04002BEF RID: 11247
	public TextMeshProUGUI Text;

	// Token: 0x04002BF0 RID: 11248
	private Tweener _flashTween;

	// Token: 0x04002BF1 RID: 11249
	private float alphaDuration;

	// Token: 0x04002BF2 RID: 11250
	private float showDuration;
}
