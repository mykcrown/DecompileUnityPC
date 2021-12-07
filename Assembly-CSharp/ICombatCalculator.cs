using System;
using FixedPoint;

// Token: 0x020003C3 RID: 963
public interface ICombatCalculator
{
	// Token: 0x06001502 RID: 5378
	float CalculateHitVibration(HitData hitData, Fixed damage, bool isAttacker);

	// Token: 0x06001503 RID: 5379
	int CalculateHitLagFrames(HitData hitData, IHitOwner other, Fixed damage, bool wasBlocked);

	// Token: 0x06001504 RID: 5380
	int CalculateHitLagForClank(HitData hitData, Fixed damage);

	// Token: 0x06001505 RID: 5381
	int CalculateHitLagForSpikeBounce(Fixed knockbackMagnitude);

	// Token: 0x06001506 RID: 5382
	int CalculateHitStunFrames(HitData hitData, Fixed knockbackForce);

	// Token: 0x06001507 RID: 5383
	int CalculateKnockbackIterator(Fixed knockbackForce);

	// Token: 0x06001508 RID: 5384
	int CalculateSmokeTrailFrames(HitData hitData, Fixed knockbackForce);

	// Token: 0x06001509 RID: 5385
	int CalculateShieldStunFrames(HitData hitData, IHitOwner owner);

	// Token: 0x0600150A RID: 5386
	int CalculateShieldBreakFrames(Fixed currentDamage);

	// Token: 0x0600150B RID: 5387
	Fixed CalculateModifiedDamage(HitData hitData, IHitOwner owner);

	// Token: 0x0600150C RID: 5388
	Fixed CalculateModifiedDamageUnstaled(HitData hitData, IHitOwner owner);

	// Token: 0x0600150D RID: 5389
	bool CheckReverseHit(HitData hitData, IHitOwner attacker, IHitOwner defender, out int forceDirection);

	// Token: 0x0600150E RID: 5390
	Fixed CalculateKnockback(HitData hitData, IHitOwner hitOwner, IPlayerDelegate hitReceiver, Fixed hitReceiverTotalDamage, Fixed damage, Vector3F hitPosition, out Vector2F knockbackVelocity);

	// Token: 0x0600150F RID: 5391
	bool IsLethalHit(HitData hitData, IHitOwner attacker, IHitOwner defender, Vector3F impactPosition, Fixed multiplyVector);
}
