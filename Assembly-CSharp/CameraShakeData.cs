using System;

// Token: 0x0200037D RID: 893
[Serializable]
public class CameraShakeData
{
	// Token: 0x04000C89 RID: 3209
	public int minFrames;

	// Token: 0x04000C8A RID: 3210
	public int extraFrames;

	// Token: 0x04000C8B RID: 3211
	public float amplitude = 3f;

	// Token: 0x04000C8C RID: 3212
	public float wavelength = 1f;

	// Token: 0x04000C8D RID: 3213
	public float shakeRandomizer = 0.75f;

	// Token: 0x04000C8E RID: 3214
	public float lateralMotion = 0.1f;

	// Token: 0x04000C8F RID: 3215
	public bool scale;

	// Token: 0x04000C90 RID: 3216
	public bool softenScale;

	// Token: 0x04000C91 RID: 3217
	public bool useOverrideAngle;

	// Token: 0x04000C92 RID: 3218
	public float overrideAngle;

	// Token: 0x04000C93 RID: 3219
	public float maxAmplitude;
}
