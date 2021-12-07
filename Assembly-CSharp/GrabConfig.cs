using System;
using FixedPoint;

// Token: 0x020003CB RID: 971
[Serializable]
public class GrabConfig
{
	// Token: 0x04000E47 RID: 3655
	public int baseDurationFrames = 60;

	// Token: 0x04000E48 RID: 3656
	public Fixed dmgScaling = (Fixed)1.600000023841858;

	// Token: 0x04000E49 RID: 3657
	public int attackBonusFrames = 10;

	// Token: 0x04000E4A RID: 3658
	public int buttonMashEscapeFrames = 7;

	// Token: 0x04000E4B RID: 3659
	public Fixed grabEscapeSpeed = (Fixed)10.0;

	// Token: 0x04000E4C RID: 3660
	public Vector2F airGrabEscapeVelocity = new Vector2F(5, 12);

	// Token: 0x04000E4D RID: 3661
	public int chainGrabPreventionFrames = 60;

	// Token: 0x04000E4E RID: 3662
	public bool useRegrabDelay;

	// Token: 0x04000E4F RID: 3663
	public int regrabDelayFrames;
}
