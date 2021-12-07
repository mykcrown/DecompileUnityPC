using System;
using UnityEngine;

// Token: 0x02000B92 RID: 2962
public class VFXZhurongFadeoutCurve : VFXBehavior
{
	// Token: 0x06005565 RID: 21861 RVA: 0x001B5863 File Offset: 0x001B3C63
	public override void OnVFXInit()
	{
		this.renderers = base.GetComponentsInChildren<Renderer>();
	}

	// Token: 0x06005566 RID: 21862 RVA: 0x001B5871 File Offset: 0x001B3C71
	public override void OnVFXStart()
	{
		this.time = 0f;
	}

	// Token: 0x06005567 RID: 21863 RVA: 0x001B5880 File Offset: 0x001B3C80
	private void SetAlphaForMaterials(Renderer renderer, float alpha)
	{
		this.mats = renderer.sharedMaterials;
		foreach (Material material in this.mats)
		{
			if (material)
			{
				material.SetFloat("_alpha", alpha);
			}
			else
			{
				Debug.LogError("Null material reference on Zhurong skin. Fix the prefab!");
			}
		}
	}

	// Token: 0x06005568 RID: 21864 RVA: 0x001B58E0 File Offset: 0x001B3CE0
	private void SetAlphaForRenderers(float alpha)
	{
		foreach (Renderer renderer in this.renderers)
		{
			this.SetAlphaForMaterials(renderer, alpha);
		}
	}

	// Token: 0x06005569 RID: 21865 RVA: 0x001B5914 File Offset: 0x001B3D14
	private void Update()
	{
		this.time += WTime.deltaTime;
		if (this.time < this.pauseBegin)
		{
			float num = this.InitialFadeoutCurve.Evaluate(this.time / this.pauseBegin);
			float alphaForRenderers = 1f - (1f - this.pauseAlpha) * num;
			this.SetAlphaForRenderers(alphaForRenderers);
		}
		else if (this.time > this.pauseEnd)
		{
			float num2 = this.FinalFadeoutCurve.Evaluate((this.time - this.pauseEnd) / this.finalFadeoutTime);
			float alphaForRenderers2 = this.pauseAlpha - this.pauseAlpha * num2;
			this.SetAlphaForRenderers(alphaForRenderers2);
		}
	}

	// Token: 0x04003651 RID: 13905
	private Renderer[] renderers;

	// Token: 0x04003652 RID: 13906
	public AnimationCurve InitialFadeoutCurve;

	// Token: 0x04003653 RID: 13907
	public AnimationCurve FinalFadeoutCurve;

	// Token: 0x04003654 RID: 13908
	public float pauseAlpha = 0.7f;

	// Token: 0x04003655 RID: 13909
	public float pauseBegin = 1f;

	// Token: 0x04003656 RID: 13910
	public float pauseEnd = 5f;

	// Token: 0x04003657 RID: 13911
	public float finalFadeoutTime = 1f;

	// Token: 0x04003658 RID: 13912
	private float time;

	// Token: 0x04003659 RID: 13913
	private Material[] mats;
}
