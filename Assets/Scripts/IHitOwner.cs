// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

public interface IHitOwner
{
	int HitOwnerID
	{
		get;
		set;
	}

	List<Hit> Hits
	{
		get;
	}

	List<HurtBoxState> HurtBoxes
	{
		get;
	}

	List<HitBoxState> ShieldBoxes
	{
		get;
	}

	MoveController ActiveMove
	{
		get;
	}

	HitOwnerType Type
	{
		get;
	}

	Vector3F Position
	{
		get;
	}

	Vector3F Center
	{
		get;
	}

	HorizontalDirection Facing
	{
		get;
	}

	TeamNum Team
	{
		get;
	}

	PlayerNum PlayerNum
	{
		get;
	}

	Fixed DamageMultiplier
	{
		get;
	}

	Fixed StaleDamageMultiplier
	{
		get;
	}

	Fixed HitLagMultiplier
	{
		get;
	}

	bool IsProjectile
	{
		get;
	}

	PlayerPhysicsController Physics
	{
		get;
	}

	bool IsInvincible
	{
		get;
	}

	bool IsAllyAssist
	{
		get;
	}

	bool AssistAbsorbsHits
	{
		get;
	}

	bool AllowClanking(HitData hitData, IHitOwner other);

	bool ForceCollisionChecks(CollisionCheckType type, HitData hitData);

	bool IsImmune(HitData hitData, IHitOwner enemy);

	bool IsHitActive(Hit hit, IHitOwner other, bool excludePhantomHitbox);

	HorizontalDirection CalculateVictimFacing(bool hitWasReversed);

	bool HandleComponentHitInteraction(Hit otherHit, IHitOwner other, CollisionCheckType checkType, HitContext hitContext);

	bool ShouldCancelClankedMove(Hit myHit, Hit otherHit, IHitOwner other);

	void BeginHitLag(int hitLagFrames, IHitOwner owner, HitData hitData);

	bool ResistsHit(HitData hitData, IHitOwner hitInstigator, Vector3F hitPosition);

	bool ArmorResistsHit(HitData hitData, IHitOwner hitInstigator, Vector3F hitPosition);

	bool CanReflect(HitData hitData);

	bool ShouldReflect(IHitOwner other, ref Vector3F collisionPoint, CollisionCheckType type, Hit myHit = null);

	bool OnHitSuccess(Hit hit, IHitOwner other, ImpactType impactType, ref Vector3F hitPosition, ref Vector3F hitVelocity, HitContext hitContext);

	void OnDamageTaken(Fixed damage, ImpactType impactType);

	void ReceiveHit(HitData hitData, IHitOwner other, ImpactType impactType, HitContext hitContext);

	void OnHitBoxCollision(Hit myHit, IHitOwner other, Hit otherHit, ref Vector3F hitPostion, bool cancelMine, bool makeClank);
}
