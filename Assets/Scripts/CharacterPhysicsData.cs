// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine.Serialization;

[Serializable]
public class CharacterPhysicsData
{
	[FormerlySerializedAs("groundAcceleration")]
	public float walkAcceleration = 44.4f;

	public float slowWalkMaxSpeed = 1.67f;

	public float mediumWalkMaxSpeed = 3.33f;

	public float fastWalkMaxSpeed = 6.66f;

	public float friction = 9f;

	public float highSpeedFriction = 50f;

	public float airMaxSpeed = 6f;

	public float airAcceleration = 20f;

	public float airFriction = 13f;

	public float gravity = 17f;

	public float maxFallSpeed = 17f;

	public float runPivotAcceleration = 10f;

	public float dashStartSpeed = 8f;

	public float dashMaxSpeed = 13f;

	public float dashAcceleration = 10f;

	public Fixed dashJumpNeutralSpeedMulti = (Fixed)0.5;

	public float groundToAirMaxSpeed = 7f;

	public float runMaxSpeed = 11f;

	public float jumpHorizontalAccel = 2.52f;

	public int jumpHorizontalAccelMaxFrames;

	public float jumpVerticalAccel = 10.2f;

	public int jumpVerticalAccelMaxFrames;

	public bool debugMoveSpeed;

	public float helplessAirSpeedMultiplier = 0.9f;

	public float helplessAirAccelerationMultiplier = 0.9f;

	[FormerlySerializedAs("jumpForce")]
	public float jumpSpeed = 20f;

	[FormerlySerializedAs("secondaryJumpForce")]
	public float secondaryJumpSpeed = 20f;

	[FormerlySerializedAs("shortJumpForce")]
	public float shortJumpSpeed = 13f;

	[FormerlySerializedAs("multiJumps")]
	public int jumpCount = 2;

	public float weight = 175f;

	public float fastFallSpeed = 23f;

	[FormerlySerializedAs("shieldBreakForce")]
	public float shieldBreakSpeed = 33f;

	public bool ignorePlatforms;

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
}
