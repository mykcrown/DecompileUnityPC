using System;
using UnityEngine;

// Token: 0x02000380 RID: 896
public struct CameraShakeRequest
{
	// Token: 0x06001316 RID: 4886 RVA: 0x0006EFC4 File Offset: 0x0006D3C4
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

	// Token: 0x06001317 RID: 4887 RVA: 0x0006F064 File Offset: 0x0006D464
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

	// Token: 0x06001318 RID: 4888 RVA: 0x0006F0D8 File Offset: 0x0006D4D8
	public void useAngle(float angle, bool mirror = false)
	{
		if (this.source.useOverrideAngle)
		{
			angle = this.source.overrideAngle;
		}
		this.xFactor = Mathf.Cos(angle * 0.017453292f);
		this.yFactor = Mathf.Sin(angle * 0.017453292f);
		if (!this.source.useOverrideAngle && mirror)
		{
			this.xFactor *= -1f;
		}
		this.lateralXFactor = -this.yFactor;
		this.lateralYFactor = this.xFactor;
	}

	// Token: 0x04000C9D RID: 3229
	private CameraShakeData source;

	// Token: 0x04000C9E RID: 3230
	public float framesUntilReduction;

	// Token: 0x04000C9F RID: 3231
	public float amplitude;

	// Token: 0x04000CA0 RID: 3232
	public float wavelength;

	// Token: 0x04000CA1 RID: 3233
	public float xFactor;

	// Token: 0x04000CA2 RID: 3234
	public float yFactor;

	// Token: 0x04000CA3 RID: 3235
	public float lateralXFactor;

	// Token: 0x04000CA4 RID: 3236
	public float lateralYFactor;

	// Token: 0x04000CA5 RID: 3237
	public float shakeRandomizer;

	// Token: 0x04000CA6 RID: 3238
	public float lateralMotion;

	// Token: 0x04000CA7 RID: 3239
	public int minFrames;

	// Token: 0x04000CA8 RID: 3240
	public int extraFrames;
}
