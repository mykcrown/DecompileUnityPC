using System;
using System.Collections.Generic;
using FixedPoint;
using Xft;

// Token: 0x02000528 RID: 1320
[SkipValidation]
[Serializable]
public class MoveModel : RollbackStateTyped<MoveModel>
{
	// Token: 0x06001C7F RID: 7295 RVA: 0x00092600 File Offset: 0x00090A00
	public MoveModel()
	{
		for (int i = 0; i < this.hits.Capacity; i++)
		{
			this.hitsPool.Add(new Hit());
		}
	}

	// Token: 0x17000616 RID: 1558
	// (get) Token: 0x06001C80 RID: 7296 RVA: 0x000926D1 File Offset: 0x00090AD1
	public bool disabledForAll
	{
		get
		{
			return this.disabledHits.disabledForAll;
		}
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x000926E0 File Offset: 0x00090AE0
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

	// Token: 0x06001C82 RID: 7298 RVA: 0x0009291C File Offset: 0x00090D1C
	public override object Clone()
	{
		MoveModel moveModel = new MoveModel();
		this.CopyTo(moveModel);
		return moveModel;
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x00092938 File Offset: 0x00090D38
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
		foreach (KeyValuePair<HitBoxState, CapsuleShape> keyValuePair in this.hitBoxCapsules)
		{
			CapsuleShape value = keyValuePair.Value;
			value.Clear();
		}
		this.hitBoxCapsules.Clear();
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x00092A88 File Offset: 0x00090E88
	public void CopyDisabledHits(HitDisableDataMap other)
	{
		other.Clear();
		this.disabledHits.CopyTo(other);
	}

	// Token: 0x06001C85 RID: 7301 RVA: 0x00092A9C File Offset: 0x00090E9C
	public bool IsActiveFor(IHitOwner other, int currentFrame)
	{
		return this.disabledHits.IsActiveFor(other, currentFrame);
	}

	// Token: 0x17000617 RID: 1559
	// (get) Token: 0x06001C86 RID: 7302 RVA: 0x00092AAC File Offset: 0x00090EAC
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

	// Token: 0x17000618 RID: 1560
	// (get) Token: 0x06001C87 RID: 7303 RVA: 0x00092AE4 File Offset: 0x00090EE4
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

	// Token: 0x17000619 RID: 1561
	// (get) Token: 0x06001C88 RID: 7304 RVA: 0x00092B28 File Offset: 0x00090F28
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

	// Token: 0x1700061A RID: 1562
	// (get) Token: 0x06001C89 RID: 7305 RVA: 0x00092B6B File Offset: 0x00090F6B
	public Fixed StaleDamageMultiplier
	{
		get
		{
			return this.staleDamageMultiplier;
		}
	}

	// Token: 0x1700061B RID: 1563
	// (get) Token: 0x06001C8A RID: 7306 RVA: 0x00092B73 File Offset: 0x00090F73
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

	// Token: 0x06001C8B RID: 7307 RVA: 0x00092BA8 File Offset: 0x00090FA8
	public bool IsInterruptibleByAnything(IPlayerDataOwner player)
	{
		foreach (InterruptData interruptData in this.data.interrupts)
		{
			if (this.internalFrame >= interruptData.startFrame && this.internalFrame <= interruptData.endFrame && interruptData.interruptType == InterruptType.Any && (!interruptData.allowOnSuccessfulGust || player.Shield.GustSuccess))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001C8C RID: 7308 RVA: 0x00092C28 File Offset: 0x00091028
	public bool IsInterruptibleByAction(IPlayerDataOwner player, PlayerMovementAction action)
	{
		foreach (InterruptData interruptData in this.data.interrupts)
		{
			if (this.internalFrame >= interruptData.startFrame && this.internalFrame <= interruptData.endFrame && interruptData.interruptType == InterruptType.Action && (!interruptData.allowOnSuccessfulGust || player.Shield.GustSuccess))
			{
				foreach (PlayerMovementAction playerMovementAction in interruptData.interruptActions)
				{
					if (playerMovementAction == action)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06001C8D RID: 7309 RVA: 0x00092CD4 File Offset: 0x000910D4
	public T GetComponent<T>() where T : class
	{
		foreach (IMoveComponent moveComponent in this.moveComponents)
		{
			if (moveComponent is T)
			{
				return moveComponent as T;
			}
		}
		return (T)((object)null);
	}

	// Token: 0x04001773 RID: 6003
	[IgnoreRollback(IgnoreRollbackType.Static)]
	[NonSerialized]
	public MoveData data;

	// Token: 0x04001774 RID: 6004
	[IgnoreRollback(IgnoreRollbackType.Visual)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public MoveModel.MoveVisualData visualData = new MoveModel.MoveVisualData();

	// Token: 0x04001775 RID: 6005
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public List<IMoveComponent> moveComponents = new List<IMoveComponent>(8);

	// Token: 0x04001776 RID: 6006
	[IsClonedManually]
	[IgnoreCopyValidation]
	public List<Hit> hits = new List<Hit>(32);

	// Token: 0x04001777 RID: 6007
	[IsClonedManually]
	[IgnoreCopyValidation]
	[IgnoreRollback(IgnoreRollbackType.Static)]
	private List<Hit> hitsPool = new List<Hit>();

	// Token: 0x04001778 RID: 6008
	[IgnoreRollback(IgnoreRollbackType.Todo)]
	[IsClonedManually]
	[IgnoreCopyValidation]
	[NonSerialized]
	public HitDisableDataMap disabledHits = new HitDisableDataMap();

	// Token: 0x04001779 RID: 6009
	[IgnoreRollback(IgnoreRollbackType.Debug)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public Dictionary<HitBoxState, CapsuleShape> hitBoxCapsules = new Dictionary<HitBoxState, CapsuleShape>(32, default(HitBoxStateComparer));

	// Token: 0x0400177A RID: 6010
	[IgnoreRollback(IgnoreRollbackType.Static)]
	[NonSerialized]
	public ChargeConfig ChargeData;

	// Token: 0x0400177B RID: 6011
	public int totalGameFrames;

	// Token: 0x0400177C RID: 6012
	public int uid;

	// Token: 0x0400177D RID: 6013
	public bool paused;

	// Token: 0x0400177E RID: 6014
	public int gameFrame;

	// Token: 0x0400177F RID: 6015
	public int internalFrame;

	// Token: 0x04001780 RID: 6016
	public int firstFrameOfMove;

	// Token: 0x04001781 RID: 6017
	public bool isCharging;

	// Token: 0x04001782 RID: 6018
	public bool beganChargeDisplay;

	// Token: 0x04001783 RID: 6019
	public int chargeFireDelay;

	// Token: 0x04001784 RID: 6020
	public int chargeFrames;

	// Token: 0x04001785 RID: 6021
	public Fixed chargeFractionOverride = -1;

	// Token: 0x04001786 RID: 6022
	public ButtonPress chargeButton;

	// Token: 0x04001787 RID: 6023
	public Vector2F lastAppliedForce;

	// Token: 0x04001788 RID: 6024
	public int applyForceContinuouslyEndFrame = -1;

	// Token: 0x04001789 RID: 6025
	public int applyPhysicsOverrideEndFrame = -1;

	// Token: 0x0400178A RID: 6026
	public HorizontalDirection inputDirection;

	// Token: 0x0400178B RID: 6027
	public HorizontalDirection reversalDirection;

	// Token: 0x0400178C RID: 6028
	public HorizontalDirection initialFacing;

	// Token: 0x0400178D RID: 6029
	public HorizontalDirection deferredFacing;

	// Token: 0x0400178E RID: 6030
	public Fixed staleDamageMultiplier = (Fixed)1.0;

	// Token: 0x0400178F RID: 6031
	public bool wasSwitched;

	// Token: 0x04001790 RID: 6032
	public Vector3F assistTarget;

	// Token: 0x04001791 RID: 6033
	public bool didBReverse;

	// Token: 0x04001792 RID: 6034
	public MoveSeedData seedData;

	// Token: 0x04001793 RID: 6035
	public bool overrideFireAngle;

	// Token: 0x04001794 RID: 6036
	public Fixed articleFireAngle;

	// Token: 0x04001795 RID: 6037
	public bool isArticleFireAngleInitialized;

	// Token: 0x04001796 RID: 6038
	public bool immediateButtonPressed;

	// Token: 0x04001797 RID: 6039
	public bool repeatButtonPressed;

	// Token: 0x04001798 RID: 6040
	public int addImpulseCountdown;

	// Token: 0x04001799 RID: 6041
	public Vector2F addImpulse = default(Vector2F);

	// Token: 0x02000529 RID: 1321
	public class EffectHandle
	{
		// Token: 0x06001C8E RID: 7310 RVA: 0x00092D50 File Offset: 0x00091150
		public EffectHandle(Effect effect, MoveEffectCancelCondition cancelCondition)
		{
			this.Effect = effect;
			this.cancelCondition = ((cancelCondition != MoveEffectCancelCondition.Default) ? cancelCondition : MoveEffectCancelCondition.MoveEndWithoutTransition);
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x00092D72 File Offset: 0x00091172
		public bool TryToStop(MoveEndType moveEndType, bool transitioningToContinuingMove)
		{
			return MoveModel.EffectHandle.StopConditionMatches(this.cancelCondition, moveEndType, transitioningToContinuingMove) && this.Effect.EnterSoftKill();
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x00092D93 File Offset: 0x00091193
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

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06001C91 RID: 7313 RVA: 0x00092DCA File Offset: 0x000911CA
		// (set) Token: 0x06001C92 RID: 7314 RVA: 0x00092DD2 File Offset: 0x000911D2
		public Effect Effect { get; private set; }

		// Token: 0x0400179A RID: 6042
		private MoveEffectCancelCondition cancelCondition;
	}

	// Token: 0x0200052A RID: 1322
	public class MoveVisualData : ICopyable<MoveModel.MoveVisualData>, ICopyable
	{
		// Token: 0x06001C94 RID: 7316 RVA: 0x00092E0A File Offset: 0x0009120A
		public void Clear()
		{
			this.spawnedParticles.Clear();
			this.spawnedAudio.Clear();
			this.weaponTrailMap.Clear();
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x00092E2D File Offset: 0x0009122D
		public void Load(MoveModel.MoveVisualData other)
		{
			this.spawnedParticles = other.spawnedParticles;
			this.spawnedAudio = other.spawnedAudio;
			this.weaponTrailMap = other.weaponTrailMap;
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x00092E54 File Offset: 0x00091254
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
			foreach (KeyValuePair<WeaponTrailData, XWeaponTrail> keyValuePair in this.weaponTrailMap)
			{
				target.weaponTrailMap[keyValuePair.Key] = this.weaponTrailMap[keyValuePair.Key];
			}
		}

		// Token: 0x0400179C RID: 6044
		[IgnoreRollback(IgnoreRollbackType.Todo)]
		[IgnoreCopyValidation]
		[NonSerialized]
		public List<MoveModel.EffectHandle> spawnedParticles = new List<MoveModel.EffectHandle>(10);

		// Token: 0x0400179D RID: 6045
		[IgnoreRollback(IgnoreRollbackType.Todo)]
		[IgnoreCopyValidation]
		[NonSerialized]
		public List<MoveAudioHandle> spawnedAudio = new List<MoveAudioHandle>(10);

		// Token: 0x0400179E RID: 6046
		[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
		[IgnoreCopyValidation]
		[NonSerialized]
		public Dictionary<WeaponTrailData, XWeaponTrail> weaponTrailMap = new Dictionary<WeaponTrailData, XWeaponTrail>(10);
	}

	// Token: 0x0200052B RID: 1323
	public struct HitDisableData : IEquatable<MoveModel.HitDisableData>
	{
		// Token: 0x06001C97 RID: 7319 RVA: 0x00092F5C File Offset: 0x0009135C
		public bool Equals(MoveModel.HitDisableData other)
		{
			return this.disableType == other.disableType && this.nextEnabledFrame == other.nextEnabledFrame;
		}

		// Token: 0x0400179F RID: 6047
		public HitDisableType disableType;

		// Token: 0x040017A0 RID: 6048
		public int nextEnabledFrame;
	}
}
