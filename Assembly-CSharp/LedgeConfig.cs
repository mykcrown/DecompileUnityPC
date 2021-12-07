using System;
using FixedPoint;

// Token: 0x020003D9 RID: 985
[Serializable]
public class LedgeConfig
{
	// Token: 0x04000EB5 RID: 3765
	public int invincibilityFrames = 35;

	// Token: 0x04000EB6 RID: 3766
	public int ledgeCooldownFrames = 20;

	// Token: 0x04000EB7 RID: 3767
	public Fixed invincibilityDecay = (Fixed)0.2;

	// Token: 0x04000EB8 RID: 3768
	public int minInvincibilityCliffFrames = 5;

	// Token: 0x04000EB9 RID: 3769
	public int maxEdgeHoldFrames = 300;

	// Token: 0x04000EBA RID: 3770
	public Fixed multiGrabOffset = 1;

	// Token: 0x04000EBB RID: 3771
	public int multigrabTranslateFrames = 5;

	// Token: 0x04000EBC RID: 3772
	public bool secondPlayerNoIntangible = true;

	// Token: 0x04000EBD RID: 3773
	public int secondPlayerLedgeLag = 30;
}
