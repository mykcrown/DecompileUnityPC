using System;
using FixedPoint;

// Token: 0x020003D6 RID: 982
[Serializable]
public class ChargeConfig : ICloneable
{
	// Token: 0x0600154D RID: 5453 RVA: 0x00075994 File Offset: 0x00073D94
	public Fixed GetScaledPercent(Fixed chargePercent)
	{
		Fixed @fixed = chargePercent * this.maxChargeFrames;
		@fixed = FixedMath.Clamp(@fixed, this.scalingStartFrame, this.scalingEndFrame);
		return (@fixed - this.scalingStartFrame) / Math.Max(this.scalingEndFrame - this.scalingStartFrame, 1);
	}

	// Token: 0x0600154E RID: 5454 RVA: 0x000759EF File Offset: 0x00073DEF
	public Fixed GetScaledValue(Fixed value, Fixed chargePercent)
	{
		return 1 + (value - 1) * this.GetScaledPercent(chargePercent);
	}

	// Token: 0x0600154F RID: 5455 RVA: 0x00075A0A File Offset: 0x00073E0A
	public object Clone()
	{
		return base.MemberwiseClone();
	}

	// Token: 0x04000E95 RID: 3733
	public int maxChargeFrames = 60;

	// Token: 0x04000E96 RID: 3734
	public int scalingStartFrame;

	// Token: 0x04000E97 RID: 3735
	public int scalingEndFrame = 60;

	// Token: 0x04000E98 RID: 3736
	public bool flashWhileCharging = true;

	// Token: 0x04000E99 RID: 3737
	public bool canMoveWhileCharging;

	// Token: 0x04000E9A RID: 3738
	public Fixed maxChargeDamageMultiplier = (Fixed)1.5;

	// Token: 0x04000E9B RID: 3739
	public Fixed maxChargeForceMultiplier = (Fixed)1.0;

	// Token: 0x04000E9C RID: 3740
	public Fixed maxChargeProjectileScale = (Fixed)1.0;

	// Token: 0x04000E9D RID: 3741
	public Fixed maxChargeProjectileSpeedMultiplier = (Fixed)1.0;

	// Token: 0x04000E9E RID: 3742
	public Fixed maxChargeProjectileHitLagMultiplier = (Fixed)1.0;

	// Token: 0x04000E9F RID: 3743
	public Fixed maxChargeProjectileLifetimeMultiplier = (Fixed)1.0;

	// Token: 0x04000EA0 RID: 3744
	public int maxChargeAngleAdjustment;
}
