using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009D2 RID: 2514
public class StageSelectPreview : MonoBehaviour
{
	// Token: 0x170010DF RID: 4319
	// (get) Token: 0x06004659 RID: 18009 RVA: 0x00132EC8 File Offset: 0x001312C8
	// (set) Token: 0x0600465A RID: 18010 RVA: 0x00132ED0 File Offset: 0x001312D0
	public bool InUse
	{
		get
		{
			return this._inUse;
		}
		set
		{
			this._inUse = value;
		}
	}

	// Token: 0x170010E0 RID: 4320
	// (get) Token: 0x0600465B RID: 18011 RVA: 0x00132ED9 File Offset: 0x001312D9
	// (set) Token: 0x0600465C RID: 18012 RVA: 0x00132EE1 File Offset: 0x001312E1
	public float Alpha
	{
		get
		{
			return this._alpha;
		}
		set
		{
			this._alpha = value;
			this.SelectedStagePortrait.CrossFadeAlpha(this._alpha, 0f, true);
		}
	}

	// Token: 0x0600465D RID: 18013 RVA: 0x00132F01 File Offset: 0x00131301
	public void TweenAlpha(float alpha, float time)
	{
		this.TweenAlpha(alpha, time, null);
	}

	// Token: 0x0600465E RID: 18014 RVA: 0x00132F0C File Offset: 0x0013130C
	public void TweenAlpha(float alpha, float time, Action callback)
	{
		this.KillTween();
		this._tween = DOTween.To(new DOGetter<float>(this.get_Alpha), delegate(float x)
		{
			this.Alpha = x;
		}, alpha, time).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.KillTween();
			if (callback != null)
			{
				callback();
			}
		});
	}

	// Token: 0x0600465F RID: 18015 RVA: 0x00132F6F File Offset: 0x0013136F
	public void KillTween()
	{
		if (this._tween != null)
		{
			if (this._tween.IsPlaying())
			{
				this._tween.Kill(false);
			}
			this._tween = null;
		}
	}

	// Token: 0x06004660 RID: 18016 RVA: 0x00132F9F File Offset: 0x0013139F
	public void Load(StageData stage)
	{
		this.SelectedStagePortrait.sprite = stage.largePortrait;
	}

	// Token: 0x04002E9E RID: 11934
	public Image SelectedStagePortrait;

	// Token: 0x04002E9F RID: 11935
	private bool _inUse;

	// Token: 0x04002EA0 RID: 11936
	private float _alpha;

	// Token: 0x04002EA1 RID: 11937
	private Tweener _tween;
}
