// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using Xft;

[SkipValidation]
[Serializable]
public class MoveModel : RollbackStateTyped<MoveModel>
{
	public class EffectHandle
	{
		private MoveEffectCancelCondition cancelCondition;

		public Effect Effect
		{
			get;
			private set;
		}

		public EffectHandle(Effect effect, MoveEffectCancelCondition cancelCondition)
		{
			this.Effect = effect;
			this.cancelCondition = ((cancelCondition != MoveEffectCancelCondition.Default) ? cancelCondition : MoveEffectCancelCondition.MoveEndWithoutTransition);
		}

		public bool TryToStop(MoveEndType moveEndType, bool transitioningToContinuingMove)
		{
			return MoveModel.EffectHandle.StopConditionMatches(this.cancelCondition, moveEndType, transitioningToContinuingMove) && this.Effect.EnterSoftKill();
		}

		public static bool StopConditionMatches(MoveEffectCancelCondition condition, MoveEndType moveEndType, bool transitioningToContinuingMove)
		{
			switch (condition)
			{
			case MoveEffectCancelCondition.MoveEnd:
				return true;
			case MoveEffectCancelCondition.MoveEndWithoutTransition:
				return !transitioningToContinuingMove;
			case MoveEffectCancelCondition.MoveCancelled:
				return !transitioningToContinuingMove && moveEndType == MoveEndType.Cancelled;
			case MoveEffectCancelCondition.Never:
				return false;
			default:
				return false;
			}
		}
	}

	public class MoveVisualData : ICopyable<MoveModel.MoveVisualData>, ICopyable
	{
		[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Todo)]
		[NonSerialized]
		public List<MoveModel.EffectHandle> spawnedParticles = new List<MoveModel.EffectHandle>(10);

		[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Todo)]
		[NonSerialized]
		public List<MoveAudioHandle> spawnedAudio = new List<MoveAudioHandle>(10);

		[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
		[NonSerialized]
		public Dictionary<WeaponTrailData, XWeaponTrail> weaponTrailMap = new Dictionary<WeaponTrailData, XWeaponTrail>(10);

		public void Clear()
		{
			this.spawnedParticles.Clear();
			this.spawnedAudio.Clear();
			this.weaponTrailMap.Clear();
		}

		public void Load(MoveModel.MoveVisualData other)
		{
			this.spawnedParticles = other.spawnedParticles;
			this.spawnedAudio = other.spawnedAudio;
			this.weaponTrailMap = other.weaponTrailMap;
		}

		public void CopyTo(MoveModel.MoveVisualData target)
		{
			target.spawnedParticles.Clear();
			int count = this.spawnedParticles.Count;
			for (int i = 0; i < count; i++)
			{
				target.spawnedParticles.Add(this.spawnedParticles[i]);
			}
			target.spawnedAudio.Clear();
			count = this.spawnedAudio.Count;
			for (int j = 0; j < count; j++)
			{
				target.spawnedAudio.Add(this.spawnedAudio[j]);
			}
			target.weaponTrailMap.Clear();
			foreach (KeyValuePair<WeaponTrailData, XWeaponTrail> current in this.weaponTrailMap)
			{
				target.weaponTrailMap[current.Key] = this.weaponTrailMap[current.Key];
			}
		}
	}

	public struct HitDisableData : IEquatable<MoveModel.HitDisableData>
	{
		public HitDisableType disableType;

		public int nextEnabledFrame;

		public bool Equals(MoveModel.HitDisableData other)
		{
			return this.disableType == other.disableType && this.nextEnabledFrame == other.nextEnabledFrame;
		}
	}

	[IgnoreRollback(IgnoreRollbackType.Static)]
	[NonSerialized]
	public MoveData data;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Visual)]
	[NonSerialized]
	public MoveModel.MoveVisualData visualData = new MoveModel.MoveVisualData();

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public List<IMoveComponent> moveComponents = new List<IMoveComponent>(8);

	[IgnoreCopyValidation, IsClonedManually]
	public List<Hit> hits = new List<Hit>(32);

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Static), IsClonedManually]
	private List<Hit> hitsPool = new List<Hit>();

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Todo), IsClonedManually]
	[NonSerialized]
	public HitDisableDataMap disabledHits = new HitDisableDataMap();

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Debug)]
	[NonSerialized]
	public Dictionary<HitBoxState, CapsuleShape> hitBoxCapsules = new Dictionary<HitBoxState, CapsuleShape>(32, default(HitBoxStateComparer));

	[IgnoreRollback(IgnoreRollbackType.Static)]
	[NonSerialized]
	public ChargeConfig ChargeData;

	public int totalGameFrames;

	public int uid;

	public bool paused;

	public int gameFrame;

	public int internalFrame;

	public int firstFrameOfMove;

	public bool isCharging;

	public bool beganChargeDisplay;

	public int chargeFireDelay;

	public int chargeFrames;

	public Fixed chargeFractionOverride = -1;

	public ButtonPress chargeButton;

	public Vector2F lastAppliedForce;

	public int applyForceContinuouslyEndFrame = -1;

	public int applyPhysicsOverrideEndFrame = -1;

	public HorizontalDirection inputDirection;

	public HorizontalDirection reversalDirection;

	public HorizontalDirection initialFacing;

	public HorizontalDirection deferredFacing;

	public Fixed staleDamageMultiplier = (Fixed)1.0;

	public bool wasSwitched;

	public Vector3F assistTarget;

	public bool didBReverse;

	public MoveSeedData seedData;

	public bool overrideFireAngle;

	public Fixed articleFireAngle;

	public bool isArticleFireAngleInitialized;

	public bool immediateButtonPressed;

	public bool repeatButtonPressed;

	public int addImpulseCountdown;

	public Vector2F addImpulse = default(Vector2F);

	public bool disabledForAll
	{
		get
		{
			return this.disabledHits.disabledForAll;
		}
	}

	public Fixed ChargeDurationMultiplier
	{
		get
		{
			Fixed @fixed = 1;
			if (this.ChargeData != null)
			{
				@fixed *= this.ChargeData.GetScaledPercent(this.ChargeFraction);
			}
			return @fixed;
		}
	}

	public Fixed ChargeForceMultiplier
	{
		get
		{
			Fixed @fixed = 1;
			if (this.ChargeData != null)
			{
				@fixed *= this.ChargeData.GetScaledValue(this.ChargeData.maxChargeForceMultiplier, this.ChargeFraction);
			}
			return @fixed;
		}
	}

	public Fixed DamageMultiplier
	{
		get
		{
			Fixed @fixed = 1;
			if (this.ChargeData != null)
			{
				@fixed *= this.ChargeData.GetScaledValue(this.ChargeData.maxChargeDamageMultiplier, this.ChargeFraction);
			}
			return @fixed;
		}
	}

	public Fixed StaleDamageMultiplier
	{
		get
		{
			return this.staleDamageMultiplier;
		}
	}

	public Fixed ChargeFraction
	{
		get
		{
			if (this.chargeFractionOverride < 0)
			{
				return this.chargeFrames / this.ChargeData.maxChargeFrames;
			}
			return this.chargeFractionOverride;
		}
	}

	public MoveModel()
	{
		for (int i = 0; i < this.hits.Capacity; i++)
		{
			this.hitsPool.Add(new Hit());
		}
	}

	public override void CopyTo(MoveModel targetIn)
	{
		targetIn.data = this.data;
		targetIn.totalGameFrames = this.totalGameFrames;
		targetIn.uid = this.uid;
		targetIn.paused = this.paused;
		targetIn.gameFrame = this.gameFrame;
		targetIn.internalFrame = this.internalFrame;
		targetIn.firstFrameOfMove = this.firstFrameOfMove;
		targetIn.isCharging = this.isCharging;
		targetIn.beganChargeDisplay = this.beganChargeDisplay;
		targetIn.chargeFireDelay = this.chargeFireDelay;
		targetIn.chargeFrames = this.chargeFrames;
		targetIn.chargeFractionOverride = this.chargeFractionOverride;
		targetIn.chargeButton = this.chargeButton;
		targetIn.lastAppliedForce = this.lastAppliedForce;
		targetIn.applyForceContinuouslyEndFrame = this.applyForceContinuouslyEndFrame;
		targetIn.applyPhysicsOverrideEndFrame = this.applyPhysicsOverrideEndFrame;
		targetIn.inputDirection = this.inputDirection;
		targetIn.reversalDirection = this.reversalDirection;
		targetIn.initialFacing = this.initialFacing;
		targetIn.deferredFacing = this.deferredFacing;
		targetIn.staleDamageMultiplier = this.staleDamageMultiplier;
		targetIn.wasSwitched = this.wasSwitched;
		targetIn.didBReverse = this.didBReverse;
		targetIn.assistTarget = this.assistTarget;
		targetIn.overrideFireAngle = this.overrideFireAngle;
		targetIn.articleFireAngle = this.articleFireAngle;
		targetIn.isArticleFireAngleInitialized = this.isArticleFireAngleInitialized;
		targetIn.immediateButtonPressed = this.immediateButtonPressed;
		targetIn.repeatButtonPressed = this.repeatButtonPressed;
		targetIn.seedData = this.seedData;
		targetIn.addImpulseCountdown = this.addImpulseCountdown;
		targetIn.addImpulse = this.addImpulse;
		targetIn.ChargeData = this.ChargeData;
		this.disabledHits.CopyTo(targetIn.disabledHits);
		base.copyDictionary<HitBoxState, CapsuleShape>(this.hitBoxCapsules, targetIn.hitBoxCapsules);
		this.visualData.CopyTo(targetIn.visualData);
		base.copyList<IMoveComponent>(this.moveComponents, targetIn.moveComponents);
		targetIn.hits.Clear();
		int count = this.hits.Count;
		for (int i = 0; i < count; i++)
		{
			targetIn.hits.Add(targetIn.hitsPool[i]);
			this.hits[i].CopyTo(targetIn.hits[i]);
		}
	}

	public override object Clone()
	{
		MoveModel moveModel = new MoveModel();
		this.CopyTo(moveModel);
		return moveModel;
	}

	public override void Clear()
	{
		base.Clear();
		this.visualData.Clear();
		this.data = null;
		this.hits.Clear();
		this.disabledHits.Clear();
		this.lastAppliedForce = default(Vector2F);
		this.isCharging = false;
		this.beganChargeDisplay = false;
		this.chargeFrames = 0;
		this.chargeFireDelay = 0;
		this.chargeFractionOverride = -1;
		this.internalFrame = 0;
		this.firstFrameOfMove = 0;
		this.immediateButtonPressed = false;
		this.repeatButtonPressed = false;
		this.applyForceContinuouslyEndFrame = -1;
		this.applyPhysicsOverrideEndFrame = -1;
		this.inputDirection = HorizontalDirection.None;
		this.initialFacing = HorizontalDirection.None;
		this.reversalDirection = HorizontalDirection.None;
		this.deferredFacing = HorizontalDirection.None;
		this.staleDamageMultiplier = (Fixed)1.0;
		this.overrideFireAngle = false;
		this.articleFireAngle = 0;
		this.isArticleFireAngleInitialized = false;
		this.didBReverse = false;
		foreach (KeyValuePair<HitBoxState, CapsuleShape> current in this.hitBoxCapsules)
		{
			CapsuleShape value = current.Value;
			value.Clear();
		}
		this.hitBoxCapsules.Clear();
	}

	public void CopyDisabledHits(HitDisableDataMap other)
	{
		other.Clear();
		this.disabledHits.CopyTo(other);
	}

	public bool IsActiveFor(IHitOwner other, int currentFrame)
	{
		return this.disabledHits.IsActiveFor(other, currentFrame);
	}

	public bool IsInterruptibleByAnything(IPlayerDataOwner player)
	{
		InterruptData[] interrupts = this.data.interrupts;
		for (int i = 0; i < interrupts.Length; i++)
		{
			InterruptData interruptData = interrupts[i];
			if (this.internalFrame >= interruptData.startFrame && this.internalFrame <= interruptData.endFrame && interruptData.interruptType == InterruptType.Any && (!interruptData.allowOnSuccessfulGust || player.Shield.GustSuccess))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsInterruptibleByAction(IPlayerDataOwner player, PlayerMovementAction action)
	{
		InterruptData[] interrupts = this.data.interrupts;
		for (int i = 0; i < interrupts.Length; i++)
		{
			InterruptData interruptData = interrupts[i];
			if (this.internalFrame >= interruptData.startFrame && this.internalFrame <= interruptData.endFrame && interruptData.interruptType == InterruptType.Action && (!interruptData.allowOnSuccessfulGust || player.Shield.GustSuccess))
			{
				PlayerMovementAction[] interruptActions = interruptData.interruptActions;
				for (int j = 0; j < interruptActions.Length; j++)
				{
					PlayerMovementAction playerMovementAction = interruptActions[j];
					if (playerMovementAction == action)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public T GetComponent<T>() where T : class
	{
		foreach (IMoveComponent current in this.moveComponents)
		{
			if (current is T)
			{
				return current as T;
			}
		}
		return (T)((object)null);
	}
}
