using System;
using UnityEngine;

// Token: 0x02000B37 RID: 2871
public static class ParticleSystemExtensions
{
	// Token: 0x0600533F RID: 21311 RVA: 0x001AEB28 File Offset: 0x001ACF28
	public static void Reset(this ParticleSystem particle)
	{
		ParticleSystem.EmissionModule emission = particle.emission;
		emission.enabled = false;
		particle.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
		particle.Simulate(0f, false, true);
		emission.enabled = true;
	}
}
