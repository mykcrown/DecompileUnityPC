using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020004FE RID: 1278
public class MoveData : ScriptableObject, IPreloadedGameAsset
{
	// Token: 0x170005E1 RID: 1505
	// (get) Token: 0x06001BC3 RID: 7107 RVA: 0x0008C92E File Offset: 0x0008AD2E
	public AnimationClip animationClip
	{
		get
		{
			return this.animationClipFile.obj;
		}
	}

	// Token: 0x170005E2 RID: 1506
	// (get) Token: 0x06001BC4 RID: 7108 RVA: 0x0008C93B File Offset: 0x0008AD3B
	public AnimationClip animationClipLeft
	{
		get
		{
			return this.animationClipLeftFile.obj;
		}
	}

	// Token: 0x170005E3 RID: 1507
	// (get) Token: 0x06001BC5 RID: 7109 RVA: 0x0008C948 File Offset: 0x0008AD48
	public AnimationClip opponentAnimationClip
	{
		get
		{
			return this.opponentAnimationClipFile.obj;
		}
	}

	// Token: 0x170005E4 RID: 1508
	// (get) Token: 0x06001BC6 RID: 7110 RVA: 0x0008C955 File Offset: 0x0008AD55
	public float baseAnimationSpeed
	{
		get
		{
			return (!(this.animationClipFile.obj == null)) ? (this.animationClipFile.obj.length * WTime.fps / (float)this.totalInternalFrames) : 0f;
		}
	}

	// Token: 0x170005E5 RID: 1509
	// (get) Token: 0x06001BC7 RID: 7111 RVA: 0x0008C995 File Offset: 0x0008AD95
	public float baseLeftClipAnimationSpeed
	{
		get
		{
			return this.baseAnimationSpeed;
		}
	}

	// Token: 0x170005E6 RID: 1510
	// (get) Token: 0x06001BC8 RID: 7112 RVA: 0x0008C99D File Offset: 0x0008AD9D
	public string leftClipName
	{
		get
		{
			return (!(this.animationClipLeftFile.obj != null)) ? base.name : (base.name + "_left");
		}
	}

	// Token: 0x170005E7 RID: 1511
	// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x0008C9D0 File Offset: 0x0008ADD0
	// (set) Token: 0x06001BCA RID: 7114 RVA: 0x0008C9D8 File Offset: 0x0008ADD8
	public InputProfile activeInputProfile { get; set; }

	// Token: 0x170005E8 RID: 1512
	// (get) Token: 0x06001BCB RID: 7115 RVA: 0x0008C9E1 File Offset: 0x0008ADE1
	public bool IsLedgeRecovery
	{
		get
		{
			return this.label == MoveLabel.LedgeAttack || this.label == MoveLabel.LedgeRoll || this.label == MoveLabel.LedgeStand || this.label == MoveLabel.LedgeJump;
		}
	}

	// Token: 0x06001BCC RID: 7116 RVA: 0x0008CA17 File Offset: 0x0008AE17
	public bool IsTauntMove()
	{
		return this.label == MoveLabel.Emote || this.label == MoveLabel.Hologram || this.label == MoveLabel.VoiceLineTaunt;
	}

	// Token: 0x06001BCD RID: 7117 RVA: 0x0008CA40 File Offset: 0x0008AE40
	public void Rescale(Fixed rescale, bool knockbackOnly = false)
	{
		foreach (HitData hitData in this.hitData)
		{
			hitData.Rescale(rescale, knockbackOnly);
		}
		if (!knockbackOnly)
		{
			foreach (ForceData forceData in this.forces)
			{
				forceData.force *= (float)rescale;
				forceData.stickInfluence *= rescale;
				forceData.inputDirectionForce *= (float)rescale;
			}
			foreach (CreateArticleAction createArticleAction in this.articles)
			{
				createArticleAction.data.Rescale(rescale);
				createArticleAction.speed *= (float)rescale;
			}
		}
	}

	// Token: 0x06001BCE RID: 7118 RVA: 0x0008CB28 File Offset: 0x0008AF28
	public bool Equals(MoveData moveData)
	{
		return !(moveData == null) && (this.label == moveData.label && base.name == moveData.name) && this.moveName == moveData.moveName;
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x0008CB80 File Offset: 0x0008AF80
	public T GetComponent<T>() where T : MoveComponent
	{
		for (int i = this.components.Length - 1; i >= 0; i--)
		{
			MoveComponent moveComponent = this.components[i];
			if (moveComponent is T)
			{
				return this.components[i] as T;
			}
		}
		return (T)((object)null);
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x0008CBD5 File Offset: 0x0008AFD5
	public string GetOpponentAnimationClipName(CharacterData character)
	{
		return character.characterName + "_" + base.name + "_opponent";
	}

	// Token: 0x06001BD1 RID: 7121 RVA: 0x0008CBF4 File Offset: 0x0008AFF4
	public static void UpdateHitboxPosition(Hit hit, Dictionary<HitBoxState, CapsuleShape> hitBoxCapsules, IHitOwner owner, IBodyOwner body)
	{
		bool flag = false;
		if (owner.Facing == HorizontalDirection.Left && owner.ActiveMove != null && owner.ActiveMove.Data != null && owner.ActiveMove.Data.animationClipLeftFile.obj != null)
		{
			flag = true;
		}
		if (owner.IsHitActive(hit, null, false))
		{
			foreach (HitBoxState hitBoxState in hit.hitBoxes)
			{
				hitBoxState.lastPosition = hitBoxState.position;
				BodyPart bodyPart = hitBoxState.data.bodyPart;
				Vector3F bonePosition = body.GetBonePosition(bodyPart, flag);
				Vector3F a = body.GetRotation(bodyPart, flag).eulerAngles;
				if (flag)
				{
					a = -a;
				}
				hitBoxState.position = hitBoxState.CalculatePosition(bonePosition, a.z, owner.Facing, Vector3F.one);
				if (hitBoxState.lastPosition.sqrMagnitude == 0)
				{
					hitBoxState.lastPosition = hitBoxState.position;
				}
				if (hitBoxCapsules.ContainsKey(hitBoxState))
				{
					bool flag2 = false;
					if (hit.data.phantomInterpolation && hit.data.startFrame == owner.ActiveMove.Model.internalFrame)
					{
						flag2 = true;
					}
					CapsuleShape capsuleShape = hitBoxCapsules[hitBoxState];
					capsuleShape.Visible = !flag2;
					capsuleShape.SetPositions(hitBoxState.position, hitBoxState.lastPosition, hitBoxState.IsCircle);
				}
			}
		}
		else
		{
			foreach (HitBoxState key in hit.hitBoxes)
			{
				if (hitBoxCapsules.ContainsKey(key))
				{
					CapsuleShape capsuleShape2 = hitBoxCapsules[key];
					capsuleShape2.Visible = false;
				}
			}
		}
	}

	// Token: 0x06001BD2 RID: 7122 RVA: 0x0008CE20 File Offset: 0x0008B220
	public static void UpdateHitboxPositions(List<Hit> hits, Dictionary<HitBoxState, CapsuleShape> hitBoxCapsules, IHitOwner owner, IBodyOwner body)
	{
		for (int i = 0; i < hits.Count; i++)
		{
			Hit hit = hits[i];
			MoveData.UpdateHitboxPosition(hit, hitBoxCapsules, owner, body);
		}
	}

	// Token: 0x06001BD3 RID: 7123 RVA: 0x0008CE58 File Offset: 0x0008B258
	public void RegisterPreload(PreloadContext context)
	{
		if (!context.AlreadyChecked(this))
		{
			foreach (CreateArticleAction createArticleAction in this.articles)
			{
				if (createArticleAction != null)
				{
					createArticleAction.RegisterPreload(context);
				}
			}
			foreach (HitData hitData in this.hitData)
			{
				if (hitData != null)
				{
					hitData.RegisterPreload(context);
				}
			}
			foreach (MoveTrailEmitterData moveTrailEmitterData in this.trailEmitters)
			{
				if (moveTrailEmitterData != null)
				{
					moveTrailEmitterData.RegisterPreload(context);
				}
			}
			foreach (InterruptData interruptData in this.interrupts)
			{
				if (interruptData != null)
				{
					interruptData.RegisterPreload(context);
				}
			}
			foreach (MoveData moveData in this.requiredMoves)
			{
				if (!(moveData == null))
				{
					moveData.RegisterPreload(context);
				}
			}
			foreach (MoveParticleEffect moveParticleEffect in this.particleEffects)
			{
				if (moveParticleEffect != null)
				{
					moveParticleEffect.RegisterPreload(context);
				}
			}
			foreach (MoveComponent moveComponent in this.components)
			{
				if (!(moveComponent == null))
				{
					moveComponent.RegisterPreload(context);
				}
			}
			this.armorOptions.RegisterPreload(context);
		}
	}

	// Token: 0x170005E9 RID: 1513
	// (get) Token: 0x06001BD4 RID: 7124 RVA: 0x0008D01A File Offset: 0x0008B41A
	public bool IsGrab
	{
		get
		{
			return this.label == MoveLabel.Grab || this.label == MoveLabel.PivotGrab || this.label == MoveLabel.RunGrab;
		}
	}

	// Token: 0x170005EA RID: 1514
	// (get) Token: 0x06001BD5 RID: 7125 RVA: 0x0008D044 File Offset: 0x0008B444
	public bool IsThrow
	{
		get
		{
			if (this.treatAsThrow)
			{
				return true;
			}
			foreach (HitData hitData in this.hitData)
			{
				if (hitData.hitType == HitType.Throw)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x06001BD6 RID: 7126 RVA: 0x0008D08C File Offset: 0x0008B48C
	public bool CancelOnLand
	{
		get
		{
			if (this.cancelOptions.cancelOnLand)
			{
				return true;
			}
			for (int i = 0; i < this.interrupts.Length; i++)
			{
				InterruptData interruptData = this.interrupts[i];
				if (interruptData.triggerType == InterruptTriggerType.OnLand)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x170005EC RID: 1516
	// (get) Token: 0x06001BD7 RID: 7127 RVA: 0x0008D0E0 File Offset: 0x0008B4E0
	public bool CancelOnFall
	{
		get
		{
			if (this.cancelOptions.cancelOnFall)
			{
				return true;
			}
			for (int i = 0; i < this.interrupts.Length; i++)
			{
				InterruptData interruptData = this.interrupts[i];
				if (interruptData.triggerType == InterruptTriggerType.OnFall)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x040015DA RID: 5594
	public string moveName;

	// Token: 0x040015DB RID: 5595
	public MoveLabel label;

	// Token: 0x040015DC RID: 5596
	public CharacterDefinition runtimeUpdateCharacter;

	// Token: 0x040015DD RID: 5597
	public bool reverseFacing;

	// Token: 0x040015DE RID: 5598
	public bool deferFacingChanges;

	// Token: 0x040015DF RID: 5599
	public bool chargesMeter;

	// Token: 0x040015E0 RID: 5600
	public bool shovesEnemies;

	// Token: 0x040015E1 RID: 5601
	public bool makeHelpless;

	// Token: 0x040015E2 RID: 5602
	public LandingOverrideData helplessLandingData;

	// Token: 0x040015E3 RID: 5603
	public HelplessStateData helplessStateData = new HelplessStateData();

	// Token: 0x040015E4 RID: 5604
	public bool blockMovement = true;

	// Token: 0x040015E5 RID: 5605
	public BlockMovementData[] blockMovementData = new BlockMovementData[0];

	// Token: 0x040015E6 RID: 5606
	public bool allowReversal;

	// Token: 0x040015E7 RID: 5607
	public int reversalFrames = 4;

	// Token: 0x040015E8 RID: 5608
	public bool treatAsThrow;

	// Token: 0x040015E9 RID: 5609
	public BodyPart overrideThrowGrabbedBodyPart;

	// Token: 0x040015EA RID: 5610
	public bool ignoreThrowBoneRotation;

	// Token: 0x040015EB RID: 5611
	[FormerlySerializedAs("ledgeReleaseFrame")]
	public int ledgeLockDuration;

	// Token: 0x040015EC RID: 5612
	public bool isRecovery;

	// Token: 0x040015ED RID: 5613
	public bool neverTeamAttack;

	// Token: 0x040015EE RID: 5614
	public bool enableCliffProtection = true;

	// Token: 0x040015EF RID: 5615
	public bool ignoreAllCollision;

	// Token: 0x040015F0 RID: 5616
	public bool ignoreHitBoxCollision;

	// Token: 0x040015F1 RID: 5617
	public bool ignorePhysicsCollisions;

	// Token: 0x040015F2 RID: 5618
	public bool clearFastFall;

	// Token: 0x040015F3 RID: 5619
	public bool bufferAutoFastFall;

	// Token: 0x040015F4 RID: 5620
	public bool canBufferInput = true;

	// Token: 0x040015F5 RID: 5621
	public bool readAnyBufferedInput;

	// Token: 0x040015F6 RID: 5622
	public bool dontReadMovementInputs;

	// Token: 0x040015F7 RID: 5623
	public AnimationClipFile animationClipFile = new AnimationClipFile();

	// Token: 0x040015F8 RID: 5624
	public AnimationClipFile animationClipLeftFile = new AnimationClipFile();

	// Token: 0x040015F9 RID: 5625
	public AnimationClipFile opponentAnimationClipFile = new AnimationClipFile();

	// Token: 0x040015FA RID: 5626
	public bool ignoreAssistVelocity;

	// Token: 0x040015FB RID: 5627
	public bool autoFaceEnemy;

	// Token: 0x040015FC RID: 5628
	public MoveData moveOverrideAhead;

	// Token: 0x040015FD RID: 5629
	public MoveData moveOverrideBehind;

	// Token: 0x040015FE RID: 5630
	public MoveData moveOverrideAbove;

	// Token: 0x040015FF RID: 5631
	public MoveData moveOverrideBelow;

	// Token: 0x04001600 RID: 5632
	public Fixed moveOverrideFarDistance = 0;

	// Token: 0x04001601 RID: 5633
	public MoveData moveOverrideFarBehind;

	// Token: 0x04001602 RID: 5634
	public MoveData moveOverrideFarAhead;

	// Token: 0x04001603 RID: 5635
	public bool hasAttackAssistNudgeOverride;

	// Token: 0x04001604 RID: 5636
	public Fixed attackAssistNudgeMultiplier;

	// Token: 0x04001605 RID: 5637
	public Fixed attackAssistNudgeBase;

	// Token: 0x04001606 RID: 5638
	public Fixed attackAssistNudgeMinDistance;

	// Token: 0x04001607 RID: 5639
	public Fixed attackAssistNudgeMaxDistance;

	// Token: 0x04001608 RID: 5640
	public WrapMode wrapMode;

	// Token: 0x04001609 RID: 5641
	[FormerlySerializedAs("totalFrames")]
	public int totalInternalFrames = 15;

	// Token: 0x0400160A RID: 5642
	public List<AnimationSpeedMultiplier> animationSpeedMultipliers = new List<AnimationSpeedMultiplier>();

	// Token: 0x0400160B RID: 5643
	public bool overrideBlendingIn = true;

	// Token: 0x0400160C RID: 5644
	public bool overrideBlendingOut;

	// Token: 0x0400160D RID: 5645
	public float blendingIn;

	// Token: 0x0400160E RID: 5646
	public float blendingOut;

	// Token: 0x0400160F RID: 5647
	public bool disableRootMotion;

	// Token: 0x04001610 RID: 5648
	public bool rotateInMovementDirection;

	// Token: 0x04001611 RID: 5649
	public int directionRotationOffsetAngle;

	// Token: 0x04001612 RID: 5650
	public bool hasChainGrabAlternate;

	// Token: 0x04001613 RID: 5651
	public InputProfile inputProfile;

	// Token: 0x04001614 RID: 5652
	public InputProfile assistProfile;

	// Token: 0x04001615 RID: 5653
	public bool nullAssistProfile;

	// Token: 0x04001617 RID: 5655
	[FormerlySerializedAs("previousMoves")]
	public MoveData[] requiredMoves;

	// Token: 0x04001618 RID: 5656
	[FormerlySerializedAs("links")]
	public InterruptData[] interrupts;

	// Token: 0x04001619 RID: 5657
	public MoveParticleEffect[] particleEffects = new MoveParticleEffect[0];

	// Token: 0x0400161A RID: 5658
	[FormerlySerializedAs("appliedForces")]
	public ForceData[] forces = new ForceData[0];

	// Token: 0x0400161B RID: 5659
	public PhysicsOverride[] physicsOverrides = new PhysicsOverride[0];

	// Token: 0x0400161C RID: 5660
	public MoveCameraShakeData[] cameraShakes = new MoveCameraShakeData[0];

	// Token: 0x0400161D RID: 5661
	public WeaponTrailData[] weaponTrails = new WeaponTrailData[0];

	// Token: 0x0400161E RID: 5662
	public SoundEffect[] soundEffects = new SoundEffect[0];

	// Token: 0x0400161F RID: 5663
	public LedgeGrabEnableData[] ledgeGrabs = new LedgeGrabEnableData[0];

	// Token: 0x04001620 RID: 5664
	public ECBOverrideData[] ecbOverrides = new ECBOverrideData[0];

	// Token: 0x04001621 RID: 5665
	[FormerlySerializedAs("hits")]
	public HitData[] hitData = new HitData[0];

	// Token: 0x04001622 RID: 5666
	public MaterialAnimationTrigger[] materialAnimationTriggers = new MaterialAnimationTrigger[0];

	// Token: 0x04001623 RID: 5667
	[FormerlySerializedAs("invincibleBodyParts")]
	public IntangibleBodyParts[] intangibleBodyParts = new IntangibleBodyParts[0];

	// Token: 0x04001624 RID: 5668
	public IntangibleBodyParts[] projectileInvincibility = new IntangibleBodyParts[0];

	// Token: 0x04001625 RID: 5669
	public ArmorOptions armorOptions;

	// Token: 0x04001626 RID: 5670
	public MoveCancelData cancelOptions = new MoveCancelData();

	// Token: 0x04001627 RID: 5671
	public ChargeOptions chargeOptions = new ChargeOptions();

	// Token: 0x04001628 RID: 5672
	public MoveTrailEmitterData[] trailEmitters = new MoveTrailEmitterData[0];

	// Token: 0x04001629 RID: 5673
	[FormerlySerializedAs("projectiles")]
	public CreateArticleAction[] articles = new CreateArticleAction[0];

	// Token: 0x0400162A RID: 5674
	public MoveUseOptions moveUseOptions = new MoveUseOptions();

	// Token: 0x0400162B RID: 5675
	public MoveComponent[] components = new MoveComponent[0];
}
