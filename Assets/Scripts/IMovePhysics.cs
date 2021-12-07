// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface IMovePhysics
{
	Vector3F Velocity
	{
		get;
	}

	Vector3F GroundedNormal
	{
		get;
	}

	Vector3F Center
	{
		get;
	}

	Vector3F MovementVelocity
	{
		get;
	}

	Fixed SteerMomentumMaxAnglePerFrame
	{
		get;
	}

	Fixed SteerMomentumMinOverallAngle
	{
		get;
	}

	Fixed SteerMomentumMaxOverallAngle
	{
		get;
	}

	bool SteerMomentumFaceVelocity
	{
		get;
	}

	bool PreventGroundedness
	{
		get;
	}

	void StopMovement(bool stopX, bool stopY, VelocityType velocityType);

	void AddVelocity(Vector2F velocity, int directionMultiplier, VelocityType velocityType);

	void SetOverride(PhysicsOverride physicsOverride);

	void ForceTranslate(Vector3F delta, bool checkFeet, bool detectCliffs = true);
}
