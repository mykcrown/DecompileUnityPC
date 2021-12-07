// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface IPhysicsCalculator
{
	bool IsCheckGrounded(PhysicsContext context);

	bool IgnoreCollisions(PhysicsContext context);

	bool IsCheckPlatformForGrounded(PhysicsContext context, Vector2F deltaPos);

	bool IsPlatformCollision(PhysicsContext context, Vector2F deltaPos);

	bool IsPlatformUndersideCollision(PhysicsContext context, Vector2F deltaPos);

	bool IsGroundCollision(PhysicsContext context, Vector2F deltaPos);

	Fixed GetGravity(PhysicsContext context);

	Vector2F GetAirFriction(PhysicsContext context);

	Fixed GetGroundFriction(PhysicsContext context, ref Vector3F newMovementVelocity);

	Fixed GetMaxDownwardVelocity(PhysicsContext context);

	Fixed GetMaxHorizontalAirVelocity(PhysicsContext context);
}
