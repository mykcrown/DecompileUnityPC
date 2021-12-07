// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ForceData : ICloneable
{
	public enum LegalForceState
	{
		All,
		AirOnly,
		GroundOnly
	}

	[Serializable]
	public class MoveUseStaling
	{
		public int numUses;

		public Vector2F multiplier;
	}

	[FormerlySerializedAs("castingFrame")]
	public int frame;

	[FormerlySerializedAs("resetPreviousVertical")]
	public bool resetYVelocity;

	[FormerlySerializedAs("resetPreviousHorizontal")]
	public bool resetXVelocity;

	public bool repeatCast;

	public int endFrame;

	public MoveForceType forceType = MoveForceType.Normal;

	public ForceData.LegalForceState legalState;

	public int maxRotationAngleAmount = 20;

	public int neutralRotationAngle = 90;

	public int minInput360Angle;

	public int maxInput360Angle;

	public bool hasDefaultInputDirection;

	public int defaultInputDirection;

	public float inputDirectionForce;

	public bool setFacingDirection = true;

	public Vector2F stickInfluence;

	public Vector2 force;

	public ForceData.MoveUseStaling[] forceStaling = new ForceData.MoveUseStaling[0];

	public bool isFrictionForce;

	public bool isContinuous;

	public bool chargeScalesForce;

	public bool applyOnChargeScaledDurationComplete;

	public bool chargeScalesDuration;

	public int minChargedDuration;

	public int maxChargedDuration;

	public string note = string.Empty;

	public bool casted
	{
		get;
		set;
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
