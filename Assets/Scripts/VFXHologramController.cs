// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class VFXHologramController : MonoBehaviour
{
	public List<ParticleSystemRenderer> applyTextureToRenderers = new List<ParticleSystemRenderer>();

	private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

	private void Awake()
	{
		this.particleSystems.AddRange(base.GetComponentsInChildren<ParticleSystem>());
	}

	public void SetHologramData(Texture2D hologramTexture)
	{
		if (hologramTexture == null)
		{
			return;
		}
		foreach (ParticleSystemRenderer current in this.applyTextureToRenderers)
		{
			if (current.material.HasProperty("_MainTex"))
			{
				current.material.mainTexture = hologramTexture;
			}
			else if (current.material.HasProperty("_Albedo"))
			{
				current.material.SetTexture("_Albedo", hologramTexture);
			}
		}
	}

	public void Replay()
	{
		foreach (ParticleSystem current in this.particleSystems)
		{
			current.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		this.Play();
	}

	public void Play()
	{
		foreach (ParticleSystem current in this.particleSystems)
		{
			current.main.simulationSpeed = 1f;
			current.Play(false);
		}
	}

	public void Pause()
	{
		foreach (ParticleSystem current in this.particleSystems)
		{
			current.main.simulationSpeed = 0f;
		}
	}

	public void Stop()
	{
		foreach (ParticleSystem current in this.particleSystems)
		{
			current.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
	}
}
