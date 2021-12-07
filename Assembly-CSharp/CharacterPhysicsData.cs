using System;
using FixedPoint;
using UnityEngine.Serialization;

// Token: 0x0200058F RID: 1423
[Serializable]
public class CharacterPhysicsData
{
	// Token: 0x0600200E RID: 8206 RVA: 0x000A2978 File Offset: 0x000A0D78
	public void Rescale(float rescale)
	{
		this.walkAcceleration *= rescale;
		this.slowWalkMaxSpeed *= rescale;
		this.mediumWalkMaxSpeed *= rescale;
		this.dashStartSpeed *= rescale;
		this.dashMaxSpeed *= rescale;
		this.dashAcceleration *= rescale * rescale;
		this.dashJumpNeutralSpeedMulti *= rescale;
		this.runMaxSpeed *= rescale;
		this.jumpSpeed *= rescale;
		this.secondaryJumpSpeed *= rescale;
		this.shortJumpSpeed *= rescale;
		this.fastFallSpeed *= rescale;
		this.shieldBreakSpeed *= rescale;
		this.friction *= rescale;
		this.highSpeedFriction *= rescale;
		this.airMaxSpeed *= rescale;
		this.airAcceleration *= rescale * rescale;
		this.airFriction *= rescale;
		this.gravity *= rescale;
		this.maxFallSpeed *= rescale;
		this.fastWalkMaxSpeed *= rescale;
		this.runPivotAcceleration *= rescale * rescale;
	}

	// Token: 0x0400197A RID: 6522
	[FormerlySerializedAs("groundAcceleration")]
	public float walkAcceleration = 44.4f;

	// Token: 0x0400197B RID: 6523
	public float slowWalkMaxSpeed = 1.67f;

	// Token: 0x0400197C RID: 6524
	public float mediumWalkMaxSpeed = 3.33f;

	// Token: 0x0400197D RID: 6525
	public float fastWalkMaxSpeed = 6.66f;

	// Token: 0x0400197E RID: 6526
	public float friction = 9f;

	// Token: 0x0400197F RID: 6527
	public float highSpeedFriction = 50f;

	// Token: 0x04001980 RID: 6528
	public float airMaxSpeed = 6f;

	// Token: 0x04001981 RID: 6529
	public float airAcceleration = 20f;

	// Token: 0x04001982 RID: 6530
	public float airFriction = 13f;

	// Token: 0x04001983 RID: 6531
	public float gravity = 17f;

	// Token: 0x04001984 RID: 6532
	public float maxFallSpeed = 17f;

	// Token: 0x04001985 RID: 6533
	public float runPivotAcceleration = 10f;

	// Token: 0x04001986 RID: 6534
	public float dashStartSpeed = 8f;

	// Token: 0x04001987 RID: 6535
	public float dashMaxSpeed = 13f;

	// Token: 0x04001988 RID: 6536
	public float dashAcceleration = 10f;

	// Token: 0x04001989 RID: 6537
	public Fixed dashJumpNeutralSpeedMulti = (Fixed)0.5;

	// Token: 0x0400198A RID: 6538
	public float groundToAirMaxSpeed = 7f;

	// Token: 0x0400198B RID: 6539
	public float runMaxSpeed = 11f;

	// Token: 0x0400198C RID: 6540
	public float jumpHorizontalAccel = 2.52f;

	// Token: 0x0400198D RID: 6541
	public int jumpHorizontalAccelMaxFrames;

	// Token: 0x0400198E RID: 6542
	public float jumpVerticalAccel = 10.2f;

	// Token: 0x0400198F RID: 6543
	public int jumpVerticalAccelMaxFrames;

	// Token: 0x04001990 RID: 6544
	public bool debugMoveSpeed;

	// Token: 0x04001991 RID: 6545
	public float helplessAirSpeedMultiplier = 0.9f;

	// Token: 0x04001992 RID: 6546
	public float helplessAirAccelerationMultiplier = 0.9f;

	// Token: 0x04001993 RID: 6547
	[FormerlySerializedAs("jumpForce")]
	public float jumpSpeed = 20f;

	// Token: 0x04001994 RID: 6548
	[FormerlySerializedAs("secondaryJumpForce")]
	public float secondaryJumpSpeed = 20f;

	// Token: 0x04001995 RID: 6549
	[FormerlySerializedAs("shortJumpForce")]
	public float shortJumpSpeed = 13f;

	// Token: 0x04001996 RID: 6550
	[FormerlySerializedAs("multiJumps")]
	public int jumpCount = 2;

	// Token: 0x04001997 RID: 6551
	public float weight = 175f;

	// Token: 0x04001998 RID: 6552
	public float fastFallSpeed = 23f;

	// Token: 0x04001999 RID: 6553
	[FormerlySerializedAs("shieldBreakForce")]
	public float shieldBreakSpeed = 33f;

	// Token: 0x0400199A RID: 6554
	public bool ignorePlatforms;
}
