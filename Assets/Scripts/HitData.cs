// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class HitData : ICloneable, IPreloadedGameAsset
{
	[FormerlySerializedAs("activeFramesBegin")]
	public int startFrame;

	[FormerlySerializedAs("activeFramesEnds")]
	public int endFrame;

	[FormerlySerializedAs("hitConfirmType")]
	public HitType hitType;

	public HitBoxColor hitBoxColor;

	public HitEffectType hitEffectType;

	public HitDataType dataType = HitDataType.Move;

	public bool phantomInterpolation;

	public MaterialAnimationTrigger[] materialAnimationTriggers = new MaterialAnimationTrigger[0];

	public bool overrideAttackSound;

	public AudioData attackSound;

	public bool overrideImpactSound;

	public AudioData impactSound;

	public bool overrideAltImpactSound;

	public AudioData altImpactSound;

	public bool overrideHitActionState;

	public ActionState hitActionState;

	public bool overrideHitParticle;

	public HitParticleData[] hitParticles = new HitParticleData[0];

	public BodyPart AttachToBodyPart;

	public Vector3 hitParticleOffset;

	public ParticleOffsetSpace hitParticleOffsetSpace;

	public int counterHitLagFrames;

	public HitCounterFilter counterFilter;

	public bool uncappedHitLag;

	public bool useTeleportCorrection;

	public Vector3F teleportCorrection = Vector3F.zero;

	public GrabType grabType = GrabType.Normal;

	public MoveData onGrabMove;

	public BodyPart overrideGrabbedBone;

	public bool isChainGrabThrow;

	public bool releaseGrabbedOpponent = true;

	public bool ignoreRegrabLimit;

	[FormerlySerializedAs("damageOnHit")]
	public float damage;

	public int bonusShieldDamage;

	public bool useBonusDamageFromSeed;

	public Fixed seedDamageMultiplier = 0;

	public Fixed seedDamageBonusMin = 0;

	public Fixed seedDamageBonusMax = 999;

	[FormerlySerializedAs("resetPreviousHorizontalPush")]
	public bool resetXVelocity = true;

	[FormerlySerializedAs("resetPreviousVerticalPush")]
	public bool resetYVelocity = true;

	[FormerlySerializedAs("resetPreviousHorizontal")]
	public bool resetXVelocitySelf;

	[FormerlySerializedAs("resetPreviousVertical")]
	public bool resetYVelocitySelf;

	[FormerlySerializedAs("appliedForce")]
	public Vector2 selfForce;

	public float baseKnockback;

	public bool useOverrideGravity;

	public Fixed overrideGravity;

	public int overrideGravityFrames;

	public float knockbackScaling;

	public float knockbackAngle;

	public float hitlagMulti = 1f;

	public float hitlagShieldMulti = -1f;

	public int hitlag;

	public bool useNoHitLag;

	public bool overrideCameraShake;

	public CameraShakeData cameraShake = new CameraShakeData();

	public float cameraShakeMulti = 1f;

	public bool useCameraShakeAngleOverride;

	public float overrideCameraShakeAngle;

	public Fixed comboEscapeMulitplier = 1;

	public Fixed comboEscapeAngleMulti = 1;

	public bool knockbackCausesFlinching = true;

	public bool ignoreWeight;

	public bool cameraZoom;

	public int impactEmissionFrames;

	public AnimatingColor impactEmission = new AnimatingColor();

	public bool forcesGetUp;

	public bool cannotTech;

	public bool enableReverseHitboxes;

	public bool preventHelplessness;

	public bool reflectsProjectiles;

	public AudioData reflectSound;

	public HitConditionType conditionType;

	public SelfHitData selfHitData;

	public bool applyTrailOnHit;

	public TrailEmitterData trailData;

	public List<HitBox> hitBoxes = new List<HitBox>();

	public HitDisableType disableType = HitDisableType.UntilNextGap;

	public int fixedDisabledFrames;

	public bool skipMoveValidation;

	public bool toggleEditorHitBoxes
	{
		get;
		set;
	}

	public bool toggleEditorImpact
	{
		get;
		set;
	}

	public bool toggleEditorMaterialAnimations
	{
		get;
		set;
	}

	public virtual Color DebugDrawColor
	{
		get
		{
			if (this.hitBoxColor != HitBoxColor.Default)
			{
				switch (this.hitBoxColor)
				{
				case HitBoxColor.Orange:
					return WColor.DebugHitboxOrange;
				case HitBoxColor.Yellow:
					return WColor.DebugHitboxYellow;
				}
				return WColor.DebugHitboxRed;
			}
			switch (this.hitType)
			{
			case HitType.Grab:
			case HitType.BlockableGrab:
				return WColor.DebugHitboxColor_Grab;
			case HitType.Projectile:
				return WColor.DebugHitboxColor_Projectile;
			case HitType.Counter:
				return WColor.DebugHitboxColor_Counter;
			case HitType.SelfHit:
				return WColor.DebugHitboxColor_SelfHit;
			}
			return WColor.DebugHitboxColor_Hit;
		}
	}

	public bool IsActiveOnFrame(int frame)
	{
		return frame >= this.startFrame && frame <= this.endFrame;
	}

	public void Rescale(Fixed rescale, bool knockbackOnly = false)
	{
		if (!knockbackOnly)
		{
			foreach (HitBox current in this.hitBoxes)
			{
				current.Rescale(rescale);
			}
		}
		this.baseKnockback *= (float)rescale;
	}

	public object Clone()
	{
		object obj = base.MemberwiseClone();
		HitData hitData = (HitData)obj;
		if (hitData.hitBoxes != null)
		{
			hitData.hitBoxes = new List<HitBox>();
			for (int i = 0; i < this.hitBoxes.Count; i++)
			{
				hitData.hitBoxes.Add((HitBox)this.hitBoxes[i].Clone());
			}
		}
		return obj;
	}

	public void RegisterPreload(PreloadContext context)
	{
		HitParticleData[] array = this.hitParticles;
		for (int i = 0; i < array.Length; i++)
		{
			HitParticleData hitParticleData = array[i];
			if (hitParticleData != null)
			{
				hitParticleData.RegisterPreload(context);
			}
		}
		if (this.applyTrailOnHit)
		{
			this.trailData.RegisterPreload(context);
		}
	}
}
