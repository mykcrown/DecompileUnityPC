// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface ICombatCalculator
{
	float CalculateHitVibration(HitData hitData, Fixed damage, bool isAttacker);

	int CalculateHitLagFrames(HitData hitData, IHitOwner other, Fixed damage, bool wasBlocked);

	int CalculateHitLagForClank(HitData hitData, Fixed damage);

	int CalculateHitLagForSpikeBounce(Fixed knockbackMagnitude);

	int CalculateHitStunFrames(HitData hitData, Fixed knockbackForce);

	int CalculateKnockbackIterator(Fixed knockbackForce);

	int CalculateSmokeTrailFrames(HitData hitData, Fixed knockbackForce);

	int CalculateShieldStunFrames(HitData hitData, IHitOwner owner);

	int CalculateShieldBreakFrames(Fixed currentDamage);

	Fixed CalculateModifiedDamage(HitData hitData, IHitOwner owner);

	Fixed CalculateModifiedDamageUnstaled(HitData hitData, IHitOwner owner);

	bool CheckReverseHit(HitData hitData, IHitOwner attacker, IHitOwner defender, out int forceDirection);

	Fixed CalculateKnockback(HitData hitData, IHitOwner hitOwner, IPlayerDelegate hitReceiver, Fixed hitReceiverTotalDamage, Fixed damage, Vector3F hitPosition, out Vector2F knockbackVelocity);

	bool IsLethalHit(HitData hitData, IHitOwner attacker, IHitOwner defender, Vector3F impactPosition, Fixed multiplyVector);
}
