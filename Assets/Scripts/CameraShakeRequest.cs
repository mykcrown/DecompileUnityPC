// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public struct CameraShakeRequest
{
	private CameraShakeData source;

	public float framesUntilReduction;

	public float amplitude;

	public float wavelength;

	public float xFactor;

	public float yFactor;

	public float lateralXFactor;

	public float lateralYFactor;

	public float shakeRandomizer;

	public float lateralMotion;

	public int minFrames;

	public int extraFrames;

	public CameraShakeRequest(CameraShakeData source)
	{
		this.source = source;
		this.framesUntilReduction = 0f;
		this.xFactor = 0f;
		this.yFactor = 1f;
		this.lateralXFactor = 0f;
		this.lateralYFactor = 1f;
		this.shakeRandomizer = 0.75f;
		this.amplitude = source.amplitude;
		this.wavelength = source.wavelength;
		this.minFrames = source.minFrames;
		this.extraFrames = source.extraFrames;
		this.shakeRandomizer = source.shakeRandomizer;
		this.lateralMotion = source.lateralMotion;
	}

	public void useMulti(float value)
	{
		if (this.source.scale)
		{
			if (this.source.softenScale)
			{
				value = Mathf.Sqrt(value);
			}
			this.amplitude *= value;
			if (this.source.maxAmplitude != 0f)
			{
				this.amplitude = Mathf.Min(this.amplitude, this.source.maxAmplitude);
			}
		}
	}

	public void useAngle(float angle, bool mirror = false)
	{
		if (this.source.useOverrideAngle)
		{
			angle = this.source.overrideAngle;
		}
		this.xFactor = Mathf.Cos(angle * 0.0174532924f);
		this.yFactor = Mathf.Sin(angle * 0.0174532924f);
		if (!this.source.useOverrideAngle && mirror)
		{
			this.xFactor *= -1f;
		}
		this.lateralXFactor = -this.yFactor;
		this.lateralYFactor = this.xFactor;
	}
}
