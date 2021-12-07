// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class PhysicsMotionContext
{
	public Vector3F initialVelocity;

	public Vector3F initialMovementVelocity;

	public Vector3F initialKnockbackVelocity;

	public Vector3F travelDelta;

	public Fixed maxTravelDist;

	public Fixed distanceTraveled;

	public bool completedMovement;
}
