// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine.Serialization;

[Serializable]
public class PhysicsOverride : CloneableObject, ICloneable
{
	[FormerlySerializedAs("activationFrame")]
	public int frame;

	public bool restoreDefaults;

	public bool applyOnChargeScaledDurationComplete;

	public float gravityMultiplier = 1f;

	[FormerlySerializedAs("frictionMultiplier")]
	public float horizontalFrictionMultiplier = 1f;

	public bool overrideFriction;

	public float horizontalFrictionOverride;

	public float airSpeedMultiplier = 1f;

	public float dragValue;

	public Fixed velocityMultiplier = 1;

	public bool ignoreMaxFallSpeed;

	public bool ignorePlatforms;

	public bool chargeScalesDuration;

	public int minChargedDuration;

	public int maxChargedDuration;

	public Fixed steerMomentumMaxAnglePerFrame = 0;

	public Fixed steerMomentumMinOverallAngle = -1;

	public Fixed steerMomentumMaxOverallAngle = -1;

	public bool steerMomentumFaceVelocity = true;

	public Fixed groundCheckUp = (Fixed)0.10000000149011612;

	public Fixed groundCheckDown = (Fixed)0.10000000149011612;

	public string note = string.Empty;

	public bool overrideMaxFallSpeed;

	public Fixed maxFallSpeed = 0;

	public bool ignoreOverrideWhenFastFalling;

	public bool preventGroundedness;
}
