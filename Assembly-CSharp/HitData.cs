using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000515 RID: 1301
[Serializable]
public class HitData : ICloneable, IPreloadedGameAsset
{
	// Token: 0x170005F3 RID: 1523
	// (get) Token: 0x06001C01 RID: 7169 RVA: 0x0008D7C9 File Offset: 0x0008BBC9
	// (set) Token: 0x06001C02 RID: 7170 RVA: 0x0008D7D1 File Offset: 0x0008BBD1
	public bool toggleEditorHitBoxes { get; set; }

	// Token: 0x170005F4 RID: 1524
	// (get) Token: 0x06001C03 RID: 7171 RVA: 0x0008D7DA File Offset: 0x0008BBDA
	// (set) Token: 0x06001C04 RID: 7172 RVA: 0x0008D7E2 File Offset: 0x0008BBE2
	public bool toggleEditorImpact { get; set; }

	// Token: 0x170005F5 RID: 1525
	// (get) Token: 0x06001C05 RID: 7173 RVA: 0x0008D7EB File Offset: 0x0008BBEB
	// (set) Token: 0x06001C06 RID: 7174 RVA: 0x0008D7F3 File Offset: 0x0008BBF3
	public bool toggleEditorMaterialAnimations { get; set; }

	// Token: 0x06001C07 RID: 7175 RVA: 0x0008D7FC File Offset: 0x0008BBFC
	public bool IsActiveOnFrame(int frame)
	{
		return frame >= this.startFrame && frame <= this.endFrame;
	}

	// Token: 0x06001C08 RID: 7176 RVA: 0x0008D81C File Offset: 0x0008BC1C
	public void Rescale(Fixed rescale, bool knockbackOnly = false)
	{
		if (!knockbackOnly)
		{
			foreach (HitBox hitBox in this.hitBoxes)
			{
				hitBox.Rescale(rescale);
			}
		}
		this.baseKnockback *= (float)rescale;
	}

	// Token: 0x06001C09 RID: 7177 RVA: 0x0008D894 File Offset: 0x0008BC94
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

	// Token: 0x06001C0A RID: 7178 RVA: 0x0008D904 File Offset: 0x0008BD04
	public void RegisterPreload(PreloadContext context)
	{
		foreach (HitParticleData hitParticleData in this.hitParticles)
		{
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

	// Token: 0x170005F6 RID: 1526
	// (get) Token: 0x06001C0B RID: 7179 RVA: 0x0008D95C File Offset: 0x0008BD5C
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

	// Token: 0x040016CB RID: 5835
	[FormerlySerializedAs("activeFramesBegin")]
	public int startFrame;

	// Token: 0x040016CC RID: 5836
	[FormerlySerializedAs("activeFramesEnds")]
	public int endFrame;

	// Token: 0x040016CD RID: 5837
	[FormerlySerializedAs("hitConfirmType")]
	public HitType hitType;

	// Token: 0x040016CE RID: 5838
	public HitBoxColor hitBoxColor;

	// Token: 0x040016CF RID: 5839
	public HitEffectType hitEffectType;

	// Token: 0x040016D0 RID: 5840
	public HitDataType dataType = HitDataType.Move;

	// Token: 0x040016D1 RID: 5841
	public bool phantomInterpolation;

	// Token: 0x040016D2 RID: 5842
	public MaterialAnimationTrigger[] materialAnimationTriggers = new MaterialAnimationTrigger[0];

	// Token: 0x040016D3 RID: 5843
	public bool overrideAttackSound;

	// Token: 0x040016D4 RID: 5844
	public AudioData attackSound;

	// Token: 0x040016D5 RID: 5845
	public bool overrideImpactSound;

	// Token: 0x040016D6 RID: 5846
	public AudioData impactSound;

	// Token: 0x040016D7 RID: 5847
	public bool overrideAltImpactSound;

	// Token: 0x040016D8 RID: 5848
	public AudioData altImpactSound;

	// Token: 0x040016D9 RID: 5849
	public bool overrideHitActionState;

	// Token: 0x040016DA RID: 5850
	public ActionState hitActionState;

	// Token: 0x040016DB RID: 5851
	public bool overrideHitParticle;

	// Token: 0x040016DC RID: 5852
	public HitParticleData[] hitParticles = new HitParticleData[0];

	// Token: 0x040016DD RID: 5853
	public BodyPart AttachToBodyPart;

	// Token: 0x040016DE RID: 5854
	public Vector3 hitParticleOffset;

	// Token: 0x040016DF RID: 5855
	public ParticleOffsetSpace hitParticleOffsetSpace;

	// Token: 0x040016E0 RID: 5856
	public int counterHitLagFrames;

	// Token: 0x040016E1 RID: 5857
	public HitCounterFilter counterFilter;

	// Token: 0x040016E2 RID: 5858
	public bool uncappedHitLag;

	// Token: 0x040016E3 RID: 5859
	public bool useTeleportCorrection;

	// Token: 0x040016E4 RID: 5860
	public Vector3F teleportCorrection = Vector3F.zero;

	// Token: 0x040016E5 RID: 5861
	public GrabType grabType = GrabType.Normal;

	// Token: 0x040016E6 RID: 5862
	public MoveData onGrabMove;

	// Token: 0x040016E7 RID: 5863
	public BodyPart overrideGrabbedBone;

	// Token: 0x040016E8 RID: 5864
	public bool isChainGrabThrow;

	// Token: 0x040016E9 RID: 5865
	public bool releaseGrabbedOpponent = true;

	// Token: 0x040016EA RID: 5866
	public bool ignoreRegrabLimit;

	// Token: 0x040016EB RID: 5867
	[FormerlySerializedAs("damageOnHit")]
	public float damage;

	// Token: 0x040016EC RID: 5868
	public int bonusShieldDamage;

	// Token: 0x040016ED RID: 5869
	public bool useBonusDamageFromSeed;

	// Token: 0x040016EE RID: 5870
	public Fixed seedDamageMultiplier = 0;

	// Token: 0x040016EF RID: 5871
	public Fixed seedDamageBonusMin = 0;

	// Token: 0x040016F0 RID: 5872
	public Fixed seedDamageBonusMax = 999;

	// Token: 0x040016F1 RID: 5873
	[FormerlySerializedAs("resetPreviousHorizontalPush")]
	public bool resetXVelocity = true;

	// Token: 0x040016F2 RID: 5874
	[FormerlySerializedAs("resetPreviousVerticalPush")]
	public bool resetYVelocity = true;

	// Token: 0x040016F3 RID: 5875
	[FormerlySerializedAs("resetPreviousHorizontal")]
	public bool resetXVelocitySelf;

	// Token: 0x040016F4 RID: 5876
	[FormerlySerializedAs("resetPreviousVertical")]
	public bool resetYVelocitySelf;

	// Token: 0x040016F5 RID: 5877
	[FormerlySerializedAs("appliedForce")]
	public Vector2 selfForce;

	// Token: 0x040016F6 RID: 5878
	public float baseKnockback;

	// Token: 0x040016F7 RID: 5879
	public bool useOverrideGravity;

	// Token: 0x040016F8 RID: 5880
	public Fixed overrideGravity;

	// Token: 0x040016F9 RID: 5881
	public int overrideGravityFrames;

	// Token: 0x040016FA RID: 5882
	public float knockbackScaling;

	// Token: 0x040016FB RID: 5883
	public float knockbackAngle;

	// Token: 0x040016FC RID: 5884
	public float hitlagMulti = 1f;

	// Token: 0x040016FD RID: 5885
	public float hitlagShieldMulti = -1f;

	// Token: 0x040016FE RID: 5886
	public int hitlag;

	// Token: 0x040016FF RID: 5887
	public bool useNoHitLag;

	// Token: 0x04001700 RID: 5888
	public bool overrideCameraShake;

	// Token: 0x04001701 RID: 5889
	public CameraShakeData cameraShake = new CameraShakeData();

	// Token: 0x04001702 RID: 5890
	public float cameraShakeMulti = 1f;

	// Token: 0x04001703 RID: 5891
	public bool useCameraShakeAngleOverride;

	// Token: 0x04001704 RID: 5892
	public float overrideCameraShakeAngle;

	// Token: 0x04001705 RID: 5893
	public Fixed comboEscapeMulitplier = 1;

	// Token: 0x04001706 RID: 5894
	public Fixed comboEscapeAngleMulti = 1;

	// Token: 0x04001707 RID: 5895
	public bool knockbackCausesFlinching = true;

	// Token: 0x04001708 RID: 5896
	public bool ignoreWeight;

	// Token: 0x04001709 RID: 5897
	public bool cameraZoom;

	// Token: 0x0400170A RID: 5898
	public int impactEmissionFrames;

	// Token: 0x0400170B RID: 5899
	public AnimatingColor impactEmission = new AnimatingColor();

	// Token: 0x0400170C RID: 5900
	public bool forcesGetUp;

	// Token: 0x0400170D RID: 5901
	public bool cannotTech;

	// Token: 0x0400170E RID: 5902
	public bool enableReverseHitboxes;

	// Token: 0x0400170F RID: 5903
	public bool preventHelplessness;

	// Token: 0x04001710 RID: 5904
	public bool reflectsProjectiles;

	// Token: 0x04001711 RID: 5905
	public AudioData reflectSound;

	// Token: 0x04001712 RID: 5906
	public HitConditionType conditionType;

	// Token: 0x04001713 RID: 5907
	public SelfHitData selfHitData;

	// Token: 0x04001714 RID: 5908
	public bool applyTrailOnHit;

	// Token: 0x04001715 RID: 5909
	public TrailEmitterData trailData;

	// Token: 0x04001716 RID: 5910
	public List<HitBox> hitBoxes = new List<HitBox>();

	// Token: 0x04001717 RID: 5911
	public HitDisableType disableType = HitDisableType.UntilNextGap;

	// Token: 0x04001718 RID: 5912
	public int fixedDisabledFrames;

	// Token: 0x04001719 RID: 5913
	public bool skipMoveValidation;
}
