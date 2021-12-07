// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class GeneratedEffect
{
	private Effect effectData;

	private GameObject effectVisual;

	private ParticleData particleInfo;

	public Effect EffectController
	{
		get;
		private set;
	}

	public GameObject EffectObject
	{
		get;
		private set;
	}

	public ParticleData ParticleInfo
	{
		get;
		private set;
	}

	public GeneratedEffect(Effect effectData, GameObject effectVisual, ParticleData particleInfo)
	{
		this.EffectController = effectData;
		this.EffectObject = effectVisual;
		this.ParticleInfo = particleInfo;
	}

	public void Expire()
	{
		if (this.EffectController != null)
		{
			this.EffectController.Destroy();
			this.EffectController = null;
		}
		this.EffectObject = null;
		this.ParticleInfo = null;
	}
}
