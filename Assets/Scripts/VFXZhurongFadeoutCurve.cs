// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class VFXZhurongFadeoutCurve : VFXBehavior
{
	private Renderer[] renderers;

	public AnimationCurve InitialFadeoutCurve;

	public AnimationCurve FinalFadeoutCurve;

	public float pauseAlpha = 0.7f;

	public float pauseBegin = 1f;

	public float pauseEnd = 5f;

	public float finalFadeoutTime = 1f;

	private float time;

	private Material[] mats;

	public override void OnVFXInit()
	{
		this.renderers = base.GetComponentsInChildren<Renderer>();
	}

	public override void OnVFXStart()
	{
		this.time = 0f;
	}

	private void SetAlphaForMaterials(Renderer renderer, float alpha)
	{
		this.mats = renderer.sharedMaterials;
		Material[] array = this.mats;
		for (int i = 0; i < array.Length; i++)
		{
			Material material = array[i];
			if (material)
			{
				material.SetFloat("_alpha", alpha);
			}
			else
			{
				UnityEngine.Debug.LogError("Null material reference on Zhurong skin. Fix the prefab!");
			}
		}
	}

	private void SetAlphaForRenderers(float alpha)
	{
		Renderer[] array = this.renderers;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer renderer = array[i];
			this.SetAlphaForMaterials(renderer, alpha);
		}
	}

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
}
