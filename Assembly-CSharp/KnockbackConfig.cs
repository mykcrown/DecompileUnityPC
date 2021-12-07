using System;
using FixedPoint;
using UnityEngine.Serialization;

// Token: 0x020003DE RID: 990
[Serializable]
public class KnockbackConfig
{
	// Token: 0x06001559 RID: 5465 RVA: 0x00076038 File Offset: 0x00074438
	public void Rescale(Fixed rescale)
	{
		this.knockbackLiftOffThreshold *= rescale;
		this.knockbackBounceThreshold *= rescale;
		this.ignoreShoveThreshold *= rescale;
		this.shoveSpeed *= rescale;
	}

	// Token: 0x04000ED4 RID: 3796
	public Fixed currentDamageScale = (Fixed)10.0;

	// Token: 0x04000ED5 RID: 3797
	public Fixed impactDamageScale = (Fixed)20.0;

	// Token: 0x04000ED6 RID: 3798
	public Fixed weightMultiplier = (Fixed)200.0;

	// Token: 0x04000ED7 RID: 3799
	public Fixed weightOffset = (Fixed)100.0;

	// Token: 0x04000ED8 RID: 3800
	public Fixed baseWeight = (Fixed)100.0;

	// Token: 0x04000ED9 RID: 3801
	public Fixed knockbackMultiplier = (Fixed)1.25;

	// Token: 0x04000EDA RID: 3802
	public Fixed knockbackAdd = (Fixed)20.0;

	// Token: 0x04000EDB RID: 3803
	public Fixed crouchCancelMultiplier = (Fixed)0.800000011920929;

	// Token: 0x04000EDC RID: 3804
	[FormerlySerializedAs("knockbackAirFriction")]
	public Fixed knockbackAirDrag = (Fixed)0.005;

	// Token: 0x04000EDD RID: 3805
	public Fixed knockbackToSpeedConversion = (Fixed)0.00336;

	// Token: 0x04000EDE RID: 3806
	public bool knockbackSuppressGravity;

	// Token: 0x04000EDF RID: 3807
	public Fixed knockbackSuppressGravityThreshold = (Fixed)20.0;

	// Token: 0x04000EE0 RID: 3808
	public bool knockbackSuppressGravityDebug;

	// Token: 0x04000EE1 RID: 3809
	public Fixed knockbackGravity = 0;

	// Token: 0x04000EE2 RID: 3810
	public Fixed maxKnockbackValue = (Fixed)1000.0;

	// Token: 0x04000EE3 RID: 3811
	public bool cancelKnockbackAtZeroVelocity = true;

	// Token: 0x04000EE4 RID: 3812
	public bool ignoreFrictionInJumpSquat;

	// Token: 0x04000EE5 RID: 3813
	public Fixed smokeTrailMulti = (Fixed)0.5;

	// Token: 0x04000EE6 RID: 3814
	public bool useBalloonKnockback = true;

	// Token: 0x04000EE7 RID: 3815
	public bool debugBalloonKnockback;

	// Token: 0x04000EE8 RID: 3816
	public int balloonKnockbackMaxIterator = 4;

	// Token: 0x04000EE9 RID: 3817
	public Fixed balloonKnockbackThreshold = 100;

	// Token: 0x04000EEA RID: 3818
	public Fixed balloonKnockbackRequiredMomentum = 50;

	// Token: 0x04000EEB RID: 3819
	public Fixed shieldKnockbackMultiplier = (Fixed)1.2000000476837158;

	// Token: 0x04000EEC RID: 3820
	public Fixed shieldKnockbackAdd = (Fixed)10.0;

	// Token: 0x04000EED RID: 3821
	[FormerlySerializedAs("knockbackBounceReduction")]
	public Fixed knockbackGroundBounceReduction = (Fixed)0.11999999731779099;

	// Token: 0x04000EEE RID: 3822
	public Fixed knockbackNonGroundBounceReduction = (Fixed)0.11999999731779099;

	// Token: 0x04000EEF RID: 3823
	public Fixed knockbackLiftOffThreshold = (Fixed)0.0;

	// Token: 0x04000EF0 RID: 3824
	public Fixed knockbackBounceThreshold = (Fixed)22.0;

	// Token: 0x04000EF1 RID: 3825
	public Fixed bounceGroundHitstunReduction = 0;

	// Token: 0x04000EF2 RID: 3826
	public Fixed bounceNongroundHitstunReduction = 0;

	// Token: 0x04000EF3 RID: 3827
	public Fixed hitlagMultiplier = (Fixed)0.3330000042915344;

	// Token: 0x04000EF4 RID: 3828
	public int hitlagAdd = 4;

	// Token: 0x04000EF5 RID: 3829
	public int hitlagMin = 3;

	// Token: 0x04000EF6 RID: 3830
	public Fixed hitstunMultiplier = (Fixed)0.47999998927116394;

	// Token: 0x04000EF7 RID: 3831
	public Fixed hitlagShieldMulti = (Fixed)0.5;

	// Token: 0x04000EF8 RID: 3832
	public bool hitLagDebug;

	// Token: 0x04000EF9 RID: 3833
	public bool enableHitVibration;

	// Token: 0x04000EFA RID: 3834
	public int maxHitLagFrames = 300;

	// Token: 0x04000EFB RID: 3835
	public Fixed clankHitlagMultiplier = (Fixed)1.0;

	// Token: 0x04000EFC RID: 3836
	public int clankHitlagAdd = 3;

	// Token: 0x04000EFD RID: 3837
	public int clankHitlagMin = 8;

	// Token: 0x04000EFE RID: 3838
	public bool clankLagOnAttackAnimations = true;

	// Token: 0x04000EFF RID: 3839
	public bool clankUseRecoilAnimation = true;

	// Token: 0x04000F00 RID: 3840
	public Fixed clankKnockback = (Fixed)1.0;

	// Token: 0x04000F01 RID: 3841
	public Fixed clankKnockbackMin = (Fixed)1.0;

	// Token: 0x04000F02 RID: 3842
	public Fixed clankKnockbackYForce = -(Fixed)0.5;

	// Token: 0x04000F03 RID: 3843
	public bool clankDebug;

	// Token: 0x04000F04 RID: 3844
	public Fixed clankDebugDamage = 0;

	// Token: 0x04000F05 RID: 3845
	public float hitlagVibration = 1f;

	// Token: 0x04000F06 RID: 3846
	public float hitlagVibrationAttacker = 0.1f;

	// Token: 0x04000F07 RID: 3847
	public float hitlagVibrationTaperSpeed = 0.12f;

	// Token: 0x04000F08 RID: 3848
	public float hitlagVibrationRandomizer = 0.75f;

	// Token: 0x04000F09 RID: 3849
	public Fixed tumbleKnockbackThreshold = 16;

	// Token: 0x04000F0A RID: 3850
	public int minimumDownedFrames = 10;

	// Token: 0x04000F0B RID: 3851
	public int maxDownedFrames = 60;

	// Token: 0x04000F0C RID: 3852
	public int tumbleWiggleOutInputCount = 5;

	// Token: 0x04000F0D RID: 3853
	public Fixed spinKnockbackThreshold = 35;

	// Token: 0x04000F0E RID: 3854
	public int minTumbleUpAngle = 70;

	// Token: 0x04000F0F RID: 3855
	public int maxTumbleUpAngle = 110;

	// Token: 0x04000F10 RID: 3856
	public int forcedGetUpHitstunFrames = 10;

	// Token: 0x04000F11 RID: 3857
	public Fixed shieldStunMultiplier = (Fixed)0.30000001192092896;

	// Token: 0x04000F12 RID: 3858
	public int shieldStunAdd = 1;

	// Token: 0x04000F13 RID: 3859
	public int techInputThresholdFrames = 20;

	// Token: 0x04000F14 RID: 3860
	public int techCooldownFrames = 50;

	// Token: 0x04000F15 RID: 3861
	public bool enableSdiTeching = true;

	// Token: 0x04000F16 RID: 3862
	public bool allowShoveEnemyMoves;

	// Token: 0x04000F17 RID: 3863
	public Fixed ignoreShoveThreshold = 6;

	// Token: 0x04000F18 RID: 3864
	public Fixed shoveSpeed = (Fixed)0.94875;

	// Token: 0x04000F19 RID: 3865
	public bool globalCapBrakePrivotSpeed = true;

	// Token: 0x04000F1A RID: 3866
	public HitData throwHit;
}
