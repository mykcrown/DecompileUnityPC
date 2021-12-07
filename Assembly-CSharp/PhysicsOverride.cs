using System;
using FixedPoint;
using UnityEngine.Serialization;

// Token: 0x02000512 RID: 1298
[Serializable]
public class PhysicsOverride : CloneableObject, ICloneable
{
	// Token: 0x040016A1 RID: 5793
	[FormerlySerializedAs("activationFrame")]
	public int frame;

	// Token: 0x040016A2 RID: 5794
	public bool restoreDefaults;

	// Token: 0x040016A3 RID: 5795
	public bool applyOnChargeScaledDurationComplete;

	// Token: 0x040016A4 RID: 5796
	public float gravityMultiplier = 1f;

	// Token: 0x040016A5 RID: 5797
	[FormerlySerializedAs("frictionMultiplier")]
	public float horizontalFrictionMultiplier = 1f;

	// Token: 0x040016A6 RID: 5798
	public bool overrideFriction;

	// Token: 0x040016A7 RID: 5799
	public float horizontalFrictionOverride;

	// Token: 0x040016A8 RID: 5800
	public float airSpeedMultiplier = 1f;

	// Token: 0x040016A9 RID: 5801
	public float dragValue;

	// Token: 0x040016AA RID: 5802
	public Fixed velocityMultiplier = 1;

	// Token: 0x040016AB RID: 5803
	public bool ignoreMaxFallSpeed;

	// Token: 0x040016AC RID: 5804
	public bool ignorePlatforms;

	// Token: 0x040016AD RID: 5805
	public bool chargeScalesDuration;

	// Token: 0x040016AE RID: 5806
	public int minChargedDuration;

	// Token: 0x040016AF RID: 5807
	public int maxChargedDuration;

	// Token: 0x040016B0 RID: 5808
	public Fixed steerMomentumMaxAnglePerFrame = 0;

	// Token: 0x040016B1 RID: 5809
	public Fixed steerMomentumMinOverallAngle = -1;

	// Token: 0x040016B2 RID: 5810
	public Fixed steerMomentumMaxOverallAngle = -1;

	// Token: 0x040016B3 RID: 5811
	public bool steerMomentumFaceVelocity = true;

	// Token: 0x040016B4 RID: 5812
	public Fixed groundCheckUp = (Fixed)0.10000000149011612;

	// Token: 0x040016B5 RID: 5813
	public Fixed groundCheckDown = (Fixed)0.10000000149011612;

	// Token: 0x040016B6 RID: 5814
	public string note = string.Empty;

	// Token: 0x040016B7 RID: 5815
	public bool overrideMaxFallSpeed;

	// Token: 0x040016B8 RID: 5816
	public Fixed maxFallSpeed = 0;

	// Token: 0x040016B9 RID: 5817
	public bool ignoreOverrideWhenFastFalling;

	// Token: 0x040016BA RID: 5818
	public bool preventGroundedness;
}
