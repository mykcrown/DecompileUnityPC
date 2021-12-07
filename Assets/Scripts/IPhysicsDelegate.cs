// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface IPhysicsDelegate
{
	ICharacterPhysicsData Data
	{
		get;
	}

	CharacterPhysicsData DefaultData
	{
		get;
	}

	Fixed GetDirectionHeldAmount
	{
		get;
	}

	IPlayerState State
	{
		get;
	}

	HorizontalDirection Facing
	{
		get;
	}

	MoveData CurrentMove
	{
		get;
	}

	MoveController ActiveMove
	{
		get;
	}

	IBodyOwner Body
	{
		get;
	}

	bool IsUnderContinuousForce
	{
		get;
	}

	ICombatController Combat
	{
		get;
	}

	IGrabController GrabController
	{
		get;
	}

	bool IsRotationRolled
	{
		get;
	}

	bool IsDirectionHeld(HorizontalDirection direction);

	Fixed GetHorizontalAcceleration(bool grounded);

	Fixed CalculateMaxHorizontalSpeed();

	void OnLand(ref Vector3F previousVelocity);

	void OnFall();

	void OnJump();

	void OnGroundBounce();

	TechType AvailableTech(CollisionData collision);

	void PerformTech(TechType techType, CollisionData collision);

	bool ShouldFallThroughPlatforms();

	bool IsPlatformLastDropped(IPhysicsCollider platformCollider);

	bool ShouldBounce();

	bool ShouldMaintainVelocityOnCollision();

	bool IgnorePhysicsCollisions();
}
