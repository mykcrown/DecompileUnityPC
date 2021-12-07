using System;
using FixedPoint;
using UnityEngine;

// Token: 0x020005DE RID: 1502
public interface ICombatController : IRollbackStateOwner, ITickable
{
	// Token: 0x06002227 RID: 8743
	void Setup(IPlayerDelegate player, ConfigData config, IEvents events, IModeOwner modeOwner, PhysicsSimulator simulator, GameObject displayObject);

	// Token: 0x06002228 RID: 8744
	void BeginStun(int frames, StunType stunType, bool cannotTech, bool isSpike, Fixed knockbackForce, Vector2F direction, HitContext hitContext, HitData hitData);

	// Token: 0x06002229 RID: 8745
	void BeginHitLag(int hitLagFrames, IHitOwner owner, HitData hitData, bool isFlourish);

	// Token: 0x0600222A RID: 8746
	void BeginClankLag(int hitLagFrames, HitData hitData);

	// Token: 0x0600222B RID: 8747
	void BeginHitVibration(int frames, float amplitude, float xFactor, float yFactor);

	// Token: 0x0600222C RID: 8748
	void ReceiveDamage(Fixed damage);

	// Token: 0x0600222D RID: 8749
	void OnShieldHitImpact(HitData hitData, IHitOwner other);

	// Token: 0x0600222E RID: 8750
	void OnHitImpact(HitData hitData, IHitOwner other, ImpactType impactType, ref HitContext hitContext);

	// Token: 0x0600222F RID: 8751
	void OnKnockbackBounce(CollisionData collision);

	// Token: 0x06002230 RID: 8752
	ActionState GetGroundStunAction();

	// Token: 0x1700079E RID: 1950
	// (get) Token: 0x06002231 RID: 8753
	bool IsGroundHitStunned { get; }

	// Token: 0x06002232 RID: 8754
	ActionState GetAirStunAction();

	// Token: 0x1700079F RID: 1951
	// (get) Token: 0x06002233 RID: 8755
	bool IsAirHitStunned { get; }

	// Token: 0x170007A0 RID: 1952
	// (get) Token: 0x06002234 RID: 8756
	bool IsMeteorStunned { get; }
}
