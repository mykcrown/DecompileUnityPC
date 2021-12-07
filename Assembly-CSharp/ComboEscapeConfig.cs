using System;
using FixedPoint;

// Token: 0x020003E7 RID: 999
[Serializable]
public class ComboEscapeConfig
{
	// Token: 0x06001584 RID: 5508 RVA: 0x0007693C File Offset: 0x00074D3C
	public void Rescale(Fixed rescale)
	{
		this.escapeDistance *= rescale;
		this.autoEscapeDistance *= rescale;
	}

	// Token: 0x04000F54 RID: 3924
	public int maxRotationAngle = 20;

	// Token: 0x04000F55 RID: 3925
	public Fixed escapeDistance = (Fixed)0.0989999994635582;

	// Token: 0x04000F56 RID: 3926
	public Fixed autoEscapeDistance = (Fixed)0.009900000877678394;

	// Token: 0x04000F57 RID: 3927
	public int maxEscapeInputs = 1;

	// Token: 0x04000F58 RID: 3928
	public int minAngleDifference = 45;

	// Token: 0x04000F59 RID: 3929
	public bool allowLandingDuringHitlag = true;

	// Token: 0x04000F5A RID: 3930
	public Fixed shieldEscapeMultiplier = (Fixed)0.5;

	// Token: 0x04000F5B RID: 3931
	public bool scaling;

	// Token: 0x04000F5C RID: 3932
	public Fixed scalingFloor = 20;

	// Token: 0x04000F5D RID: 3933
	public Fixed scalingCeiling = 80;

	// Token: 0x04000F5E RID: 3934
	public Fixed scalingMin = 8;

	// Token: 0x04000F5F RID: 3935
	public Fixed scalingMax = 24;

	// Token: 0x04000F60 RID: 3936
	public bool debugScaling;
}
