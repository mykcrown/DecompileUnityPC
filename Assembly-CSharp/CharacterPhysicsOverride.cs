using System;

// Token: 0x02000591 RID: 1425
[Serializable]
public class CharacterPhysicsOverride
{
	// Token: 0x0400199B RID: 6555
	public static readonly CharacterPhysicsOverride NoOverride = new CharacterPhysicsOverride();

	// Token: 0x0400199C RID: 6556
	public PhysicsFixedOverride slowWalkMaxSpeed = default(PhysicsFixedOverride);

	// Token: 0x0400199D RID: 6557
	public PhysicsFixedOverride mediumWalkMaxSpeed = default(PhysicsFixedOverride);

	// Token: 0x0400199E RID: 6558
	public PhysicsFixedOverride fastWalkMaxSpeed = default(PhysicsFixedOverride);

	// Token: 0x0400199F RID: 6559
	public PhysicsFixedOverride runMaxSpeed = default(PhysicsFixedOverride);

	// Token: 0x040019A0 RID: 6560
	public PhysicsFixedOverride groundToAirMaxSpeed = default(PhysicsFixedOverride);

	// Token: 0x040019A1 RID: 6561
	public PhysicsFixedOverride walkAcceleration = default(PhysicsFixedOverride);

	// Token: 0x040019A2 RID: 6562
	public PhysicsFixedOverride runPivotAcceleration = default(PhysicsFixedOverride);

	// Token: 0x040019A3 RID: 6563
	public PhysicsFixedOverride friction = default(PhysicsFixedOverride);

	// Token: 0x040019A4 RID: 6564
	public PhysicsFixedOverride highSpeedFriction = default(PhysicsFixedOverride);

	// Token: 0x040019A5 RID: 6565
	public PhysicsFixedOverride dashStartSpeed = default(PhysicsFixedOverride);

	// Token: 0x040019A6 RID: 6566
	public PhysicsFixedOverride dashMaxSpeed = default(PhysicsFixedOverride);

	// Token: 0x040019A7 RID: 6567
	public PhysicsFixedOverride dashAcceleration = default(PhysicsFixedOverride);

	// Token: 0x040019A8 RID: 6568
	public PhysicsFixedOverride airMaxSpeed = default(PhysicsFixedOverride);

	// Token: 0x040019A9 RID: 6569
	public PhysicsFixedOverride airAcceleration = default(PhysicsFixedOverride);

	// Token: 0x040019AA RID: 6570
	public PhysicsFixedOverride airFriction = default(PhysicsFixedOverride);

	// Token: 0x040019AB RID: 6571
	public PhysicsFixedOverride gravity = default(PhysicsFixedOverride);

	// Token: 0x040019AC RID: 6572
	public PhysicsFixedOverride maxFallSpeed = default(PhysicsFixedOverride);

	// Token: 0x040019AD RID: 6573
	public PhysicsFixedOverride helplessAirSpeedMultiplier = default(PhysicsFixedOverride);

	// Token: 0x040019AE RID: 6574
	public PhysicsFixedOverride helplessAirAccelerationMultiplier = default(PhysicsFixedOverride);

	// Token: 0x040019AF RID: 6575
	public PhysicsFixedOverride jumpSpeed = default(PhysicsFixedOverride);

	// Token: 0x040019B0 RID: 6576
	public PhysicsFixedOverride secondaryJumpSpeed = default(PhysicsFixedOverride);

	// Token: 0x040019B1 RID: 6577
	public PhysicsFixedOverride shortJumpSpeed = default(PhysicsFixedOverride);

	// Token: 0x040019B2 RID: 6578
	public PhysicsIntOverride jumpCount = default(PhysicsIntOverride);

	// Token: 0x040019B3 RID: 6579
	public PhysicsFixedOverride weight = default(PhysicsFixedOverride);

	// Token: 0x040019B4 RID: 6580
	public PhysicsFixedOverride fastFallSpeed = default(PhysicsFixedOverride);

	// Token: 0x040019B5 RID: 6581
	public PhysicsFixedOverride shieldBreakSpeed = default(PhysicsFixedOverride);

	// Token: 0x040019B6 RID: 6582
	public PhysicsBoolOverride ignorePlatforms = default(PhysicsBoolOverride);
}
