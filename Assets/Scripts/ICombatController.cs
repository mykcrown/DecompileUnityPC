// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public interface ICombatController : IRollbackStateOwner, ITickable
{
	bool IsGroundHitStunned
	{
		get;
	}

	bool IsAirHitStunned
	{
		get;
	}

	bool IsMeteorStunned
	{
		get;
	}

	void Setup(IPlayerDelegate player, ConfigData config, IEvents events, IModeOwner modeOwner, PhysicsSimulator simulator, GameObject displayObject);

	void BeginStun(int frames, StunType stunType, bool cannotTech, bool isSpike, Fixed knockbackForce, Vector2F direction, HitContext hitContext, HitData hitData);

	void BeginHitLag(int hitLagFrames, IHitOwner owner, HitData hitData, bool isFlourish);

	void BeginClankLag(int hitLagFrames, HitData hitData);

	void BeginHitVibration(int frames, float amplitude, float xFactor, float yFactor);

	void ReceiveDamage(Fixed damage);

	void OnShieldHitImpact(HitData hitData, IHitOwner other);

	void OnHitImpact(HitData hitData, IHitOwner other, ImpactType impactType, ref HitContext hitContext);

	void OnKnockbackBounce(CollisionData collision);

	ActionState GetGroundStunAction();

	ActionState GetAirStunAction();
}
