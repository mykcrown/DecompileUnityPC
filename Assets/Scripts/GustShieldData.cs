// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class GustShieldData
{
	public bool enableGustShield = true;

	public bool useFixedGustShieldSize;

	public Fixed fixedGustShieldRadius = (Fixed)1.2999999523162842;

	public Fixed maxSizeMultiplier = (Fixed)1.2999999523162842;

	public bool gustSizeDependentOnHealth;

	public bool gustPlayerTriggersSuccess = true;

	public int expandFrames = 30;

	public int holdFrames = 5;

	public int shrinkFrames = 5;

	public int reflectFrameStart;

	public int reflectFrameEnd;

	public int knockbackFrameStart;

	public int knockbackFrameEnd;

	public GustShieldCostType costType = GustShieldCostType.Mixed;

	public bool subtractHealthOnUse;

	public Fixed gustUsableThresholdPercent = (Fixed)0.40000000596046448;

	public Fixed gustUsableThresholdValue = (Fixed)0.0;

	public Fixed shieldHealthCostOfMaxPercent = (Fixed)0.0;

	public Fixed shieldHealthCostOfCurrentPercent = (Fixed)0.5;

	public Fixed shieldHealthCostFlatValue = (Fixed)0.0;

	public Fixed shieldHealthAfterUseFlatValue = (Fixed)0.0;

	public Fixed shieldHealthAfterUsePercentOfMax = (Fixed)0.15000000596046448;

	public bool resetsXVelocity_ground = true;

	public bool resetsYVelocity_ground = true;

	public bool resetsXVelocity_air = true;

	public bool resetsYVelocity_air = true;

	public bool causesFlinching_air;

	public bool causesFlinching_ground;

	public bool allowGustShieldBreak = true;

	public ParticleData gustParticle = new ParticleData();

	public Color gustColor;

	public AnimationCurve gustColorCurve = new AnimationCurve();

	public Fixed rotationSpeed = (Fixed)360.0;

	public Fixed gustKnockback_ground = (Fixed)80.0;

	[FormerlySerializedAs("gustKnockback")]
	public Fixed gustKnockback_air = (Fixed)80.0;

	public int gustKnockbackAngleClamp_ground = 90;

	[FormerlySerializedAs("gustKnockbackAngleClamp")]
	public int gustKnockbackAngleClamp_air = 90;

	public bool gustShieldIgnoresWeight;

	public bool gustShieldIgnoresDamage;

	public int TotalActiveFrames
	{
		get
		{
			return this.expandFrames + this.holdFrames + this.shrinkFrames;
		}
	}
}
