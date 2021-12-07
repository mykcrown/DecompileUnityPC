// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine.Serialization;

[Serializable]
public class KnockbackConfig
{
	public Fixed currentDamageScale = (Fixed)10.0;

	public Fixed impactDamageScale = (Fixed)20.0;

	public Fixed weightMultiplier = (Fixed)200.0;

	public Fixed weightOffset = (Fixed)100.0;

	public Fixed baseWeight = (Fixed)100.0;

	public Fixed knockbackMultiplier = (Fixed)1.25;

	public Fixed knockbackAdd = (Fixed)20.0;

	public Fixed crouchCancelMultiplier = (Fixed)0.800000011920929;

	[FormerlySerializedAs("knockbackAirFriction")]
	public Fixed knockbackAirDrag = (Fixed)0.005;

	public Fixed knockbackToSpeedConversion = (Fixed)0.00336;

	public bool knockbackSuppressGravity;

	public Fixed knockbackSuppressGravityThreshold = (Fixed)20.0;

	public bool knockbackSuppressGravityDebug;

	public Fixed knockbackGravity = 0;

	public Fixed maxKnockbackValue = (Fixed)1000.0;

	public bool cancelKnockbackAtZeroVelocity = true;

	public bool ignoreFrictionInJumpSquat;

	public Fixed smokeTrailMulti = (Fixed)0.5;

	public bool useBalloonKnockback = true;

	public bool debugBalloonKnockback;

	public int balloonKnockbackMaxIterator = 4;

	public Fixed balloonKnockbackThreshold = 100;

	public Fixed balloonKnockbackRequiredMomentum = 50;

	public Fixed shieldKnockbackMultiplier = (Fixed)1.2000000476837158;

	public Fixed shieldKnockbackAdd = (Fixed)10.0;

	[FormerlySerializedAs("knockbackBounceReduction")]
	public Fixed knockbackGroundBounceReduction = (Fixed)0.11999999731779099;

	public Fixed knockbackNonGroundBounceReduction = (Fixed)0.11999999731779099;

	public Fixed knockbackLiftOffThreshold = (Fixed)0.0;

	public Fixed knockbackBounceThreshold = (Fixed)22.0;

	public Fixed bounceGroundHitstunReduction = 0;

	public Fixed bounceNongroundHitstunReduction = 0;

	public Fixed hitlagMultiplier = (Fixed)0.33300000429153442;

	public int hitlagAdd = 4;

	public int hitlagMin = 3;

	public Fixed hitstunMultiplier = (Fixed)0.47999998927116394;

	public Fixed hitlagShieldMulti = (Fixed)0.5;

	public bool hitLagDebug;

	public bool enableHitVibration;

	public int maxHitLagFrames = 300;

	public Fixed clankHitlagMultiplier = (Fixed)1.0;

	public int clankHitlagAdd = 3;

	public int clankHitlagMin = 8;

	public bool clankLagOnAttackAnimations = true;

	public bool clankUseRecoilAnimation = true;

	public Fixed clankKnockback = (Fixed)1.0;

	public Fixed clankKnockbackMin = (Fixed)1.0;

	public Fixed clankKnockbackYForce = -(Fixed)0.5;

	public bool clankDebug;

	public Fixed clankDebugDamage = 0;

	public float hitlagVibration = 1f;

	public float hitlagVibrationAttacker = 0.1f;

	public float hitlagVibrationTaperSpeed = 0.12f;

	public float hitlagVibrationRandomizer = 0.75f;

	public Fixed tumbleKnockbackThreshold = 16;

	public int minimumDownedFrames = 10;

	public int maxDownedFrames = 60;

	public int tumbleWiggleOutInputCount = 5;

	public Fixed spinKnockbackThreshold = 35;

	public int minTumbleUpAngle = 70;

	public int maxTumbleUpAngle = 110;

	public int forcedGetUpHitstunFrames = 10;

	public Fixed shieldStunMultiplier = (Fixed)0.30000001192092896;

	public int shieldStunAdd = 1;

	public int techInputThresholdFrames = 20;

	public int techCooldownFrames = 50;

	public bool enableSdiTeching = true;

	public bool allowShoveEnemyMoves;

	public Fixed ignoreShoveThreshold = 6;

	public Fixed shoveSpeed = (Fixed)0.94875;

	public bool globalCapBrakePrivotSpeed = true;

	public HitData throwHit;

	public void Rescale(Fixed rescale)
	{
		this.knockbackLiftOffThreshold *= rescale;
		this.knockbackBounceThreshold *= rescale;
		this.ignoreShoveThreshold *= rescale;
		this.shoveSpeed *= rescale;
	}
}
