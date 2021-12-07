// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MoveData : ScriptableObject, IPreloadedGameAsset
{
	public string moveName;

	public MoveLabel label;

	public CharacterDefinition runtimeUpdateCharacter;

	public bool reverseFacing;

	public bool deferFacingChanges;

	public bool chargesMeter;

	public bool shovesEnemies;

	public bool makeHelpless;

	public LandingOverrideData helplessLandingData;

	public HelplessStateData helplessStateData = new HelplessStateData();

	public bool blockMovement = true;

	public BlockMovementData[] blockMovementData = new BlockMovementData[0];

	public bool allowReversal;

	public int reversalFrames = 4;

	public bool treatAsThrow;

	public BodyPart overrideThrowGrabbedBodyPart;

	public bool ignoreThrowBoneRotation;

	[FormerlySerializedAs("ledgeReleaseFrame")]
	public int ledgeLockDuration;

	public bool isRecovery;

	public bool neverTeamAttack;

	public bool enableCliffProtection = true;

	public bool ignoreAllCollision;

	public bool ignoreHitBoxCollision;

	public bool ignorePhysicsCollisions;

	public bool clearFastFall;

	public bool bufferAutoFastFall;

	public bool canBufferInput = true;

	public bool readAnyBufferedInput;

	public bool dontReadMovementInputs;

	public AnimationClipFile animationClipFile = new AnimationClipFile();

	public AnimationClipFile animationClipLeftFile = new AnimationClipFile();

	public AnimationClipFile opponentAnimationClipFile = new AnimationClipFile();

	public bool ignoreAssistVelocity;

	public bool autoFaceEnemy;

	public MoveData moveOverrideAhead;

	public MoveData moveOverrideBehind;

	public MoveData moveOverrideAbove;

	public MoveData moveOverrideBelow;

	public Fixed moveOverrideFarDistance = 0;

	public MoveData moveOverrideFarBehind;

	public MoveData moveOverrideFarAhead;

	public bool hasAttackAssistNudgeOverride;

	public Fixed attackAssistNudgeMultiplier;

	public Fixed attackAssistNudgeBase;

	public Fixed attackAssistNudgeMinDistance;

	public Fixed attackAssistNudgeMaxDistance;

	public WrapMode wrapMode;

	[FormerlySerializedAs("totalFrames")]
	public int totalInternalFrames = 15;

	public List<AnimationSpeedMultiplier> animationSpeedMultipliers = new List<AnimationSpeedMultiplier>();

	public bool overrideBlendingIn = true;

	public bool overrideBlendingOut;

	public float blendingIn;

	public float blendingOut;

	public bool disableRootMotion;

	public bool rotateInMovementDirection;

	public int directionRotationOffsetAngle;

	public bool hasChainGrabAlternate;

	public InputProfile inputProfile;

	public InputProfile assistProfile;

	public bool nullAssistProfile;

	[FormerlySerializedAs("previousMoves")]
	public MoveData[] requiredMoves;

	[FormerlySerializedAs("links")]
	public InterruptData[] interrupts;

	public MoveParticleEffect[] particleEffects = new MoveParticleEffect[0];

	[FormerlySerializedAs("appliedForces")]
	public ForceData[] forces = new ForceData[0];

	public PhysicsOverride[] physicsOverrides = new PhysicsOverride[0];

	public MoveCameraShakeData[] cameraShakes = new MoveCameraShakeData[0];

	public WeaponTrailData[] weaponTrails = new WeaponTrailData[0];

	public SoundEffect[] soundEffects = new SoundEffect[0];

	public LedgeGrabEnableData[] ledgeGrabs = new LedgeGrabEnableData[0];

	public ECBOverrideData[] ecbOverrides = new ECBOverrideData[0];

	[FormerlySerializedAs("hits")]
	public HitData[] hitData = new HitData[0];

	public MaterialAnimationTrigger[] materialAnimationTriggers = new MaterialAnimationTrigger[0];

	[FormerlySerializedAs("invincibleBodyParts")]
	public IntangibleBodyParts[] intangibleBodyParts = new IntangibleBodyParts[0];

	public IntangibleBodyParts[] projectileInvincibility = new IntangibleBodyParts[0];

	public ArmorOptions armorOptions;

	public MoveCancelData cancelOptions = new MoveCancelData();

	public ChargeOptions chargeOptions = new ChargeOptions();

	public MoveTrailEmitterData[] trailEmitters = new MoveTrailEmitterData[0];

	[FormerlySerializedAs("projectiles")]
	public CreateArticleAction[] articles = new CreateArticleAction[0];

	public MoveUseOptions moveUseOptions = new MoveUseOptions();

	public MoveComponent[] components = new MoveComponent[0];

	public AnimationClip animationClip
	{
		get
		{
			return this.animationClipFile.obj;
		}
	}

	public AnimationClip animationClipLeft
	{
		get
		{
			return this.animationClipLeftFile.obj;
		}
	}

	public AnimationClip opponentAnimationClip
	{
		get
		{
			return this.opponentAnimationClipFile.obj;
		}
	}

	public float baseAnimationSpeed
	{
		get
		{
			return (!(this.animationClipFile.obj == null)) ? (this.animationClipFile.obj.length * WTime.fps / (float)this.totalInternalFrames) : 0f;
		}
	}

	public float baseLeftClipAnimationSpeed
	{
		get
		{
			return this.baseAnimationSpeed;
		}
	}

	public string leftClipName
	{
		get
		{
			return (!(this.animationClipLeftFile.obj != null)) ? base.name : (base.name + "_left");
		}
	}

	public InputProfile activeInputProfile
	{
		get;
		set;
	}

	public bool IsLedgeRecovery
	{
		get
		{
			return this.label == MoveLabel.LedgeAttack || this.label == MoveLabel.LedgeRoll || this.label == MoveLabel.LedgeStand || this.label == MoveLabel.LedgeJump;
		}
	}

	public bool IsGrab
	{
		get
		{
			return this.label == MoveLabel.Grab || this.label == MoveLabel.PivotGrab || this.label == MoveLabel.RunGrab;
		}
	}

	public bool IsThrow
	{
		get
		{
			if (this.treatAsThrow)
			{
				return true;
			}
			HitData[] array = this.hitData;
			for (int i = 0; i < array.Length; i++)
			{
				HitData hitData = array[i];
				if (hitData.hitType == HitType.Throw)
				{
					return true;
				}
			}
			return false;
		}
	}

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

	public bool IsTauntMove()
	{
		return this.label == MoveLabel.Emote || this.label == MoveLabel.Hologram || this.label == MoveLabel.VoiceLineTaunt;
	}

	public void Rescale(Fixed rescale, bool knockbackOnly = false)
	{
		HitData[] array = this.hitData;
		for (int i = 0; i < array.Length; i++)
		{
			HitData hitData = array[i];
			hitData.Rescale(rescale, knockbackOnly);
		}
		if (!knockbackOnly)
		{
			ForceData[] array2 = this.forces;
			for (int j = 0; j < array2.Length; j++)
			{
				ForceData forceData = array2[j];
				forceData.force *= (float)rescale;
				forceData.stickInfluence *= rescale;
				forceData.inputDirectionForce *= (float)rescale;
			}
			CreateArticleAction[] array3 = this.articles;
			for (int k = 0; k < array3.Length; k++)
			{
				CreateArticleAction createArticleAction = array3[k];
				createArticleAction.data.Rescale(rescale);
				createArticleAction.speed *= (float)rescale;
			}
		}
	}

	public bool Equals(MoveData moveData)
	{
		return !(moveData == null) && (this.label == moveData.label && base.name == moveData.name) && this.moveName == moveData.moveName;
	}

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

	public string GetOpponentAnimationClipName(CharacterData character)
	{
		return character.characterName + "_" + base.name + "_opponent";
	}

	public static void UpdateHitboxPosition(Hit hit, Dictionary<HitBoxState, CapsuleShape> hitBoxCapsules, IHitOwner owner, IBodyOwner body)
	{
		bool flag = false;
		if (owner.Facing == HorizontalDirection.Left && owner.ActiveMove != null && owner.ActiveMove.Data != null && owner.ActiveMove.Data.animationClipLeftFile.obj != null)
		{
			flag = true;
		}
		if (owner.IsHitActive(hit, null, false))
		{
			foreach (HitBoxState current in hit.hitBoxes)
			{
				current.lastPosition = current.position;
				BodyPart bodyPart = current.data.bodyPart;
				Vector3F bonePosition = body.GetBonePosition(bodyPart, flag);
				Vector3F a = body.GetRotation(bodyPart, flag).eulerAngles;
				if (flag)
				{
					a = -a;
				}
				current.position = current.CalculatePosition(bonePosition, a.z, owner.Facing, Vector3F.one);
				if (current.lastPosition.sqrMagnitude == 0)
				{
					current.lastPosition = current.position;
				}
				if (hitBoxCapsules.ContainsKey(current))
				{
					bool flag2 = false;
					if (hit.data.phantomInterpolation && hit.data.startFrame == owner.ActiveMove.Model.internalFrame)
					{
						flag2 = true;
					}
					CapsuleShape capsuleShape = hitBoxCapsules[current];
					capsuleShape.Visible = !flag2;
					capsuleShape.SetPositions(current.position, current.lastPosition, current.IsCircle);
				}
			}
		}
		else
		{
			foreach (HitBoxState current2 in hit.hitBoxes)
			{
				if (hitBoxCapsules.ContainsKey(current2))
				{
					CapsuleShape capsuleShape2 = hitBoxCapsules[current2];
					capsuleShape2.Visible = false;
				}
			}
		}
	}

	public static void UpdateHitboxPositions(List<Hit> hits, Dictionary<HitBoxState, CapsuleShape> hitBoxCapsules, IHitOwner owner, IBodyOwner body)
	{
		for (int i = 0; i < hits.Count; i++)
		{
			Hit hit = hits[i];
			MoveData.UpdateHitboxPosition(hit, hitBoxCapsules, owner, body);
		}
	}

	public void RegisterPreload(PreloadContext context)
	{
		if (!context.AlreadyChecked(this))
		{
			CreateArticleAction[] array = this.articles;
			for (int i = 0; i < array.Length; i++)
			{
				CreateArticleAction createArticleAction = array[i];
				if (createArticleAction != null)
				{
					createArticleAction.RegisterPreload(context);
				}
			}
			HitData[] array2 = this.hitData;
			for (int j = 0; j < array2.Length; j++)
			{
				HitData hitData = array2[j];
				if (hitData != null)
				{
					hitData.RegisterPreload(context);
				}
			}
			MoveTrailEmitterData[] array3 = this.trailEmitters;
			for (int k = 0; k < array3.Length; k++)
			{
				MoveTrailEmitterData moveTrailEmitterData = array3[k];
				if (moveTrailEmitterData != null)
				{
					moveTrailEmitterData.RegisterPreload(context);
				}
			}
			InterruptData[] array4 = this.interrupts;
			for (int l = 0; l < array4.Length; l++)
			{
				InterruptData interruptData = array4[l];
				if (interruptData != null)
				{
					interruptData.RegisterPreload(context);
				}
			}
			MoveData[] array5 = this.requiredMoves;
			for (int m = 0; m < array5.Length; m++)
			{
				MoveData moveData = array5[m];
				if (!(moveData == null))
				{
					moveData.RegisterPreload(context);
				}
			}
			MoveParticleEffect[] array6 = this.particleEffects;
			for (int n = 0; n < array6.Length; n++)
			{
				MoveParticleEffect moveParticleEffect = array6[n];
				if (moveParticleEffect != null)
				{
					moveParticleEffect.RegisterPreload(context);
				}
			}
			MoveComponent[] array7 = this.components;
			for (int num = 0; num < array7.Length; num++)
			{
				MoveComponent moveComponent = array7[num];
				if (!(moveComponent == null))
				{
					moveComponent.RegisterPreload(context);
				}
			}
			this.armorOptions.RegisterPreload(context);
		}
	}
}
