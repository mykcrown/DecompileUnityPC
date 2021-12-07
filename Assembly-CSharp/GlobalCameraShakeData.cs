using System;

// Token: 0x020003EA RID: 1002
[Serializable]
public class GlobalCameraShakeData
{
	// Token: 0x04000F6E RID: 3950
	public HitCameraShakeMethod hitShakeMethod;

	// Token: 0x04000F6F RID: 3951
	public int hitShakeDamageMin;

	// Token: 0x04000F70 RID: 3952
	public int hitShakeDamageMax;

	// Token: 0x04000F71 RID: 3953
	public int hitShakeDamageThreshold;

	// Token: 0x04000F72 RID: 3954
	public int hitShakeKnockbackThreshold;

	// Token: 0x04000F73 RID: 3955
	public int hitShakeKnockbackMin = -1;

	// Token: 0x04000F74 RID: 3956
	public int hitShakeKnockbackMax;

	// Token: 0x04000F75 RID: 3957
	public bool debug;

	// Token: 0x04000F76 RID: 3958
	public CameraShakeData deathShake;

	// Token: 0x04000F77 RID: 3959
	public CameraShakeData hitShake;

	// Token: 0x04000F78 RID: 3960
	public CameraShakeData downedShake;
}
