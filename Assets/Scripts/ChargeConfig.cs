// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class ChargeConfig : ICloneable
{
	public int maxChargeFrames = 60;

	public int scalingStartFrame;

	public int scalingEndFrame = 60;

	public bool flashWhileCharging = true;

	public bool canMoveWhileCharging;

	public Fixed maxChargeDamageMultiplier = (Fixed)1.5;

	public Fixed maxChargeForceMultiplier = (Fixed)1.0;

	public Fixed maxChargeProjectileScale = (Fixed)1.0;

	public Fixed maxChargeProjectileSpeedMultiplier = (Fixed)1.0;

	public Fixed maxChargeProjectileHitLagMultiplier = (Fixed)1.0;

	public Fixed maxChargeProjectileLifetimeMultiplier = (Fixed)1.0;

	public int maxChargeAngleAdjustment;

	public Fixed GetScaledPercent(Fixed chargePercent)
	{
		Fixed @fixed = chargePercent * this.maxChargeFrames;
		@fixed = FixedMath.Clamp(@fixed, this.scalingStartFrame, this.scalingEndFrame);
		return (@fixed - this.scalingStartFrame) / Math.Max(this.scalingEndFrame - this.scalingStartFrame, 1);
	}

	public Fixed GetScaledValue(Fixed value, Fixed chargePercent)
	{
		return 1 + (value - 1) * this.GetScaledPercent(chargePercent);
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
