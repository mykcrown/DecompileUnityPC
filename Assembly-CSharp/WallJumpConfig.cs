using System;
using FixedPoint;

// Token: 0x020003DF RID: 991
[Serializable]
public class WallJumpConfig
{
	// Token: 0x04000F1B RID: 3867
	public Fixed wallFlushDistance = (Fixed)0.10000000149011612;

	// Token: 0x04000F1C RID: 3868
	public int wallPressFrameWindow = 5;

	// Token: 0x04000F1D RID: 3869
	public int ledgeReleaseLockoutFrames = 5;

	// Token: 0x04000F1E RID: 3870
	public int ledgeGrabCooldown = 50;
}
