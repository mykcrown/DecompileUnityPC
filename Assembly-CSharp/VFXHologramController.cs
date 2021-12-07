using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B88 RID: 2952
public class VFXHologramController : MonoBehaviour
{
	// Token: 0x06005538 RID: 21816 RVA: 0x001B4CA6 File Offset: 0x001B30A6
	private void Awake()
	{
		this.particleSystems.AddRange(base.GetComponentsInChildren<ParticleSystem>());
	}

	// Token: 0x06005539 RID: 21817 RVA: 0x001B4CBC File Offset: 0x001B30BC
	public void SetHologramData(Texture2D hologramTexture)
	{
		if (hologramTexture == null)
		{
			return;
		}
		foreach (ParticleSystemRenderer particleSystemRenderer in this.applyTextureToRenderers)
		{
			if (particleSystemRenderer.material.HasProperty("_MainTex"))
			{
				particleSystemRenderer.material.mainTexture = hologramTexture;
			}
			else if (particleSystemRenderer.material.HasProperty("_Albedo"))
			{
				particleSystemRenderer.material.SetTexture("_Albedo", hologramTexture);
			}
		}
	}

	// Token: 0x0600553A RID: 21818 RVA: 0x001B4D6C File Offset: 0x001B316C
	public void Replay()
	{
		foreach (ParticleSystem particleSystem in this.particleSystems)
		{
			particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		this.Play();
	}

	// Token: 0x0600553B RID: 21819 RVA: 0x001B4DD0 File Offset: 0x001B31D0
	public void Play()
	{
		foreach (ParticleSystem particleSystem in this.particleSystems)
		{
			particleSystem.main.simulationSpeed = 1f;
			particleSystem.Play(false);
		}
	}

	// Token: 0x0600553C RID: 21820 RVA: 0x001B4E40 File Offset: 0x001B3240
	public void Pause()
	{
		foreach (ParticleSystem particleSystem in this.particleSystems)
		{
			particleSystem.main.simulationSpeed = 0f;
		}
	}

	// Token: 0x0600553D RID: 21821 RVA: 0x001B4EA8 File Offset: 0x001B32A8
	public void Stop()
	{
		foreach (ParticleSystem particleSystem in this.particleSystems)
		{
			particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
	}

	// Token: 0x04003626 RID: 13862
	public List<ParticleSystemRenderer> applyTextureToRenderers = new List<ParticleSystemRenderer>();

	// Token: 0x04003627 RID: 13863
	private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
}
