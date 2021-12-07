using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x02000977 RID: 2423
public class TapArrowElement : MonoBehaviour
{
	// Token: 0x06004103 RID: 16643 RVA: 0x00124F9D File Offset: 0x0012339D
	public void Init()
	{
		this.Alpha = 0f;
	}

	// Token: 0x06004104 RID: 16644 RVA: 0x00124FAC File Offset: 0x001233AC
	public void Play(float alphaDuration)
	{
		this.Alpha = 1f;
		this.tween = DOTween.To(new DOGetter<float>(this.get_Alpha), delegate(float x)
		{
			this.Alpha = x;
		}, 0f, alphaDuration).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.killTween));
	}

	// Token: 0x06004105 RID: 16645 RVA: 0x00125004 File Offset: 0x00123404
	public bool IsActive()
	{
		return this.tween != null && this.tween.IsActive();
	}

	// Token: 0x06004106 RID: 16646 RVA: 0x0012501F File Offset: 0x0012341F
	private void killTween()
	{
		TweenUtil.Destroy(ref this.tween);
	}

	// Token: 0x17000F52 RID: 3922
	// (get) Token: 0x06004107 RID: 16647 RVA: 0x0012502C File Offset: 0x0012342C
	// (set) Token: 0x06004108 RID: 16648 RVA: 0x00125034 File Offset: 0x00123434
	public float Alpha
	{
		get
		{
			return this._alpha;
		}
		set
		{
			this._alpha = value;
			this.CanvasGroup.alpha = this._alpha;
		}
	}

	// Token: 0x04002BEB RID: 11243
	public CanvasGroup CanvasGroup;

	// Token: 0x04002BEC RID: 11244
	private Tweener tween;

	// Token: 0x04002BED RID: 11245
	private float _alpha;
}
