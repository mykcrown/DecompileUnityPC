using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x02000B91 RID: 2961
public class VFXUnboxingPortal : MonoBehaviour
{
	// Token: 0x170013A5 RID: 5029
	// (get) Token: 0x0600555B RID: 21851 RVA: 0x001B55AE File Offset: 0x001B39AE
	// (set) Token: 0x0600555C RID: 21852 RVA: 0x001B55B6 File Offset: 0x001B39B6
	public float ScaleTweenTime { get; set; }

	// Token: 0x0600555D RID: 21853 RVA: 0x001B55C0 File Offset: 0x001B39C0
	public List<GameObject> GetColorObjects()
	{
		return new List<GameObject>
		{
			this.Border1,
			this.Border2,
			this.Border1B,
			this.Border2B,
			this.Border1_Mouseover,
			this.Border2_Mouseover
		};
	}

	// Token: 0x0600555E RID: 21854 RVA: 0x001B561C File Offset: 0x001B3A1C
	public void SetMouseover(bool value)
	{
		if (value != this._isMouseover)
		{
			this._isMouseover = value;
			Vector3 one;
			if (value)
			{
				this.NormalMode.GetComponent<Animator>().enabled = false;
				this.Border1B.GetComponent<ParticleSystem>().main.playOnAwake = false;
				this.Border2B.GetComponent<ParticleSystem>().main.playOnAwake = false;
				one = new Vector3(1.1f, 1.1f, 1.1f);
				this.NormalMode.SetActive(false);
				this.MouseoverMode.SetActive(true);
			}
			else
			{
				one = Vector3.one;
				this.NormalMode.SetActive(true);
				this.MouseoverMode.SetActive(false);
			}
			this.killScaleTween();
			this.scaleTween = DOTween.To(() => base.transform.localScale, delegate(Vector3 valueIn)
			{
				base.transform.localScale = valueIn;
			}, one, this.ScaleTweenTime).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killScaleTween));
		}
	}

	// Token: 0x0600555F RID: 21855 RVA: 0x001B571D File Offset: 0x001B3B1D
	private void killScaleTween()
	{
		TweenUtil.Destroy(ref this.scaleTween);
	}

	// Token: 0x170013A6 RID: 5030
	// (get) Token: 0x06005560 RID: 21856 RVA: 0x001B572A File Offset: 0x001B3B2A
	// (set) Token: 0x06005561 RID: 21857 RVA: 0x001B5734 File Offset: 0x001B3B34
	public Color color
	{
		get
		{
			return this._color;
		}
		set
		{
			if (this._color != value)
			{
				this._color = value;
				foreach (GameObject gameObject in this.GetColorObjects())
				{
					ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
					component.main.startColor = this._color;
					component.GetComponent<Renderer>().material.SetColor("_TintColor", this._color);
				}
				this.NormalMode.GetComponent<Renderer>().material.color = this._color;
				this.MouseoverMode.GetComponent<Renderer>().material.color = this._color;
			}
		}
	}

	// Token: 0x04003645 RID: 13893
	public GameObject Border1;

	// Token: 0x04003646 RID: 13894
	public GameObject Border1B;

	// Token: 0x04003647 RID: 13895
	public GameObject Border2;

	// Token: 0x04003648 RID: 13896
	public GameObject Border2B;

	// Token: 0x04003649 RID: 13897
	public GameObject Border1_Mouseover;

	// Token: 0x0400364A RID: 13898
	public GameObject Border2_Mouseover;

	// Token: 0x0400364B RID: 13899
	public GameObject NormalMode;

	// Token: 0x0400364C RID: 13900
	public GameObject MouseoverMode;

	// Token: 0x0400364E RID: 13902
	private Color _color;

	// Token: 0x0400364F RID: 13903
	private bool _isMouseover;

	// Token: 0x04003650 RID: 13904
	private Tweener scaleTween;
}
