// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public static class ParticleSystemExtensions
{
	public static void Reset(this ParticleSystem particle)
	{
		ParticleSystem.EmissionModule emission = particle.emission;
		emission.enabled = false;
		particle.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
		particle.Simulate(0f, false, true);
		emission.enabled = true;
	}
}
