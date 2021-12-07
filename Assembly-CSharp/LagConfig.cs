using System;
using FixedPoint;

// Token: 0x020003CC RID: 972
[Serializable]
public class LagConfig
{
	// Token: 0x06001541 RID: 5441 RVA: 0x000756EF File Offset: 0x00073AEF
	public void Rescale(Fixed rescale)
	{
		this.heavyLandSpeedThreshold *= rescale;
	}

	// Token: 0x04000E50 RID: 3664
	public int lightLandLagFrames = 1;

	// Token: 0x04000E51 RID: 3665
	public int heavyLandLagFrames = 4;

	// Token: 0x04000E52 RID: 3666
	public Fixed heavyLandSpeedThreshold = (Fixed)20.0;

	// Token: 0x04000E53 RID: 3667
	public int platformDropLagFrames = 3;

	// Token: 0x04000E54 RID: 3668
	public int fallThroughPlatformDurationFrames = 3;

	// Token: 0x04000E55 RID: 3669
	public int platformFallPreventFastfall = 3;

	// Token: 0x04000E56 RID: 3670
	public int dashPivotFrames = 2;

	// Token: 0x04000E57 RID: 3671
	public int collisionDiamondAirDelayFrames = 8;

	// Token: 0x04000E58 RID: 3672
	public int runShieldDelayFrames = 4;
}
