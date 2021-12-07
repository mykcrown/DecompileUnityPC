// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Xft;

public class MoveController : IMoveDelegate, IRollbackStateOwner
{
	public delegate bool ComponentExecution<T>(T component);

	private sealed class _onEffectReleased_c__AnonStorey0
	{
		internal Effect theEffect;

		internal bool __m__0(MoveModel.EffectHandle item)
		{
			return item.Effect == this.theEffect;
		}
	}

	private MoveModel model;

	private IPlayerDelegate playerDelegate;

	private IHitOwner hitOwner;

	private IAnimationPlayer animationPlayer;

	private IBodyOwner body;

	private IMovePhysics physics;

	private GameManager gameManager;

	private IEvents events;

	private Func<XWeaponTrail> createWeaponTrail;

	private List<MoveLinkComponentData> linkComponentBuffer = new List<MoveLinkComponentData>();

	int IMoveDelegate.TotalFrames
	{
		get
		{
			return this.Data.totalInternalFrames;
		}
	}

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	[Inject]
	public MoveArticleSpawnCalculator articleSpawnCalculator
	{
		get;
		set;
	}

	[Inject]
	public IMoveAnimationCalculator moveAnimationCalculator
	{
		get;
		set;
	}

	[Inject]
	public IWeaponTrailHelper weaponTrailHelper
	{
		get;
		set;
	}

	[Inject]
	public DeveloperConfig devConfig
	{
		get;
		set;
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	[Inject]
	public IHitContextPool hitContextPool
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public UserAudioSettings audioSettings
	{
		get;
		set;
	}

	public MoveData Data
	{
		get
		{
			return this.Model.data;
		}
	}

	public MoveModel Model
	{
		get
		{
			return this.model;
		}
		private set
		{
			this.model = value;
		}
	}

	public bool IsActive
	{
		get
		{
			return this.Data != null;
		}
	}

	public bool MightCollide
	{
		get
		{
			return this.IsActive && !this.Data.ignoreAllCollision;
		}
	}

	public bool EmitTrail
	{
		get
		{
			MoveTrailEmitterData[] trailEmitters = this.Data.trailEmitters;
			for (int i = 0; i < trailEmitters.Length; i++)
			{
				MoveTrailEmitterData moveTrailEmitterData = trailEmitters[i];
				if (this.Model.internalFrame >= moveTrailEmitterData.startFrame && this.Model.internalFrame <= moveTrailEmitterData.endFrame)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool IsLedgeGrabEnabled
	{
		get
		{
			LedgeGrabEnableData[] ledgeGrabs = this.Data.ledgeGrabs;
			for (int i = 0; i < ledgeGrabs.Length; i++)
			{
				LedgeGrabEnableData ledgeGrabEnableData = ledgeGrabs[i];
				if (this.Model.internalFrame >= ledgeGrabEnableData.startFrame && this.Model.internalFrame <= ledgeGrabEnableData.endFrame)
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
			if (this.Data.cancelOptions.cancelOnFall)
			{
				return true;
			}
			for (int i = 0; i < this.Data.interrupts.Length; i++)
			{
				InterruptData interruptData = this.Data.interrupts[i];
				if (interruptData.ShouldUseLink(LinkCheckType.OnFall, this.playerDelegate, this.model, null))
				{
					return true;
				}
			}
			return false;
		}
	}

	public MoveController()
	{
		this.createWeaponTrail = new Func<XWeaponTrail>(this._MoveController_m__0);
	}

	public void Init(MoveContext context)
	{
		this.playerDelegate = context.playerDelegate;
		this.gameManager = context.gameManager;
		this.hitOwner = context.hitOwner;
		this.animationPlayer = this.playerDelegate.AnimationPlayer;
		this.body = this.playerDelegate.Body;
		this.physics = this.playerDelegate.Physics;
		this.events = this.gameManager.events;
		this.Model = new MoveModel();
		this.events.Subscribe(typeof(ToggleDebugDrawChannelCommand), new Events.EventHandler(this.onToggleDebugChannel));
		this.signalBus.GetSignal<EffectReleaseSignal>().AddListener(new Action<Effect>(this.onEffectReleased));
	}

	public void WasReversed()
	{
		if (this.Model.data.animationClipLeft != null)
		{
			MoveAnimationResult playableData = this.moveAnimationCalculator.GetPlayableData(this.Model.data, this.playerDelegate);
			string animationName = playableData.animationName;
			bool mirror = playableData.mirror;
			if (!this.animationPlayer.PlayAnimation(animationName, mirror, this.Model.gameFrame, (!this.Model.data.overrideBlendingIn) ? -1f : this.Model.data.blendingIn, (!this.Model.data.overrideBlendingOut) ? ((float)this.config.animationConfig.defaultMoveBlendOutDuration) : this.Model.data.blendingOut))
			{
				UnityEngine.Debug.LogError(string.Concat(new string[]
				{
					"Failed to locate preloaded animation for move ",
					this.playerDelegate.CharacterData.characterName,
					".",
					animationName,
					". Make sure the move is listed in the character's move set!"
				}));
				this.playerDelegate.EndActiveMove(MoveEndType.Cancelled, false, false);
			}
		}
	}

	public bool Initialize(MoveModel inModel, InputButtonsData input, bool isChainedMove, bool faceInputDirection = false, List<MoveLinkComponentData> previousMoveLinkedData = null, bool processMoveFrame = true)
	{
		bool isActive = this.IsActive;
		inModel.wasSwitched = true;
		inModel.uid = this.Model.uid + 1;
		this.Model = inModel;
		if (inModel.paused)
		{
			inModel.paused = false;
			this.animationPlayer.SetPause(false);
		}
		this.Model.moveComponents.Clear();
		MoveComponent[] components = this.Data.components;
		MoveComponent[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			MoveComponent original = array[i];
			MoveComponent moveComponent = UnityEngine.Object.Instantiate<MoveComponent>(original);
			if (moveComponent is AimReticleComponent)
			{
				(moveComponent as AimReticleComponent).articleSpawnController = this.articleSpawnCalculator;
			}
			if (moveComponent is IMoveTauntComponent && ButtonPressUtil.isTauntButton(this.model.chargeButton))
			{
				(moveComponent as IMoveTauntComponent).TauntSlot = ButtonPressUtil.getTauntSlotForButton(this.model.chargeButton);
			}
			this.injector.Inject(moveComponent);
			moveComponent.Init(this, this.playerDelegate, input);
			this.Model.moveComponents.Add(moveComponent);
		}
		if (!this.IsActive)
		{
			return false;
		}
		this.model.disabledHits.Init(this.Data.totalInternalFrames, this.model.hits, true);
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HitBoxes))
		{
			this.toggleHitBoxCapsules(true);
		}
		HorizontalDirection horizontalDirection = HorizontalDirection.None;
		if (inModel.inputDirection != HorizontalDirection.None && faceInputDirection)
		{
			if (this.playerDelegate.Facing != inModel.inputDirection)
			{
				horizontalDirection = this.playerDelegate.Facing;
			}
			this.setFacing(inModel.inputDirection, true);
		}
		if (this.Data.label == MoveLabel.NeutralSpecial && !this.playerDelegate.State.IsGrounded && !isChainedMove && this.gameManager.Frame - this.playerDelegate.Model.lastBackTapFrame <= this.config.moveData.neutralSpecialReversalFrames)
		{
			if (horizontalDirection == HorizontalDirection.None && this.playerDelegate.Facing != this.playerDelegate.OppositeFacing)
			{
				horizontalDirection = this.playerDelegate.Facing;
			}
			this.setFacing(this.playerDelegate.OppositeFacing, true);
			this.playerDelegate.Model.lastBackTapFrame = 0;
		}
		MoveAnimationResult playableData = this.moveAnimationCalculator.GetPlayableData(inModel.data, this.playerDelegate);
		string animationName = playableData.animationName;
		bool mirror = playableData.mirror;
		if (this.Model.data.reverseFacing && processMoveFrame)
		{
			if (horizontalDirection == HorizontalDirection.None && !this.Model.data.deferFacingChanges && this.playerDelegate.Facing != this.playerDelegate.OppositeFacing)
			{
				horizontalDirection = this.playerDelegate.Facing;
			}
			this.setFacing(this.playerDelegate.OppositeFacing, false);
		}
		if (this.gameDataManager.IsFeatureEnabled(FeatureID.AttackAssist) && this.playerDelegate.InputController.AttackAssistThisFrame && input != null)
		{
			UnityEngine.Debug.Log("Terrible");
			if (!input.buttonsHeld.Contains(ButtonPress.UpStrike) && !input.buttonsHeld.Contains(ButtonPress.DownStrike) && !input.buttonsHeld.Contains(ButtonPress.BackwardStrike) && !input.buttonsHeld.Contains(ButtonPress.ForwardStrike))
			{
				PlayerController playerController = this.gameManager.GetPlayerController(this.playerDelegate.PlayerNum);
				PlayerReference playerReference = PlayerUtil.FindClosestEnemy(playerController, this.gameManager.PlayerReferences);
				if (playerReference != null && playerController.InputController != null && playerController.InputController.AttackAssistThisFrame && this.Data.autoFaceEnemy)
				{
					if (playerReference.Controller.Position.x < playerController.Position.x)
					{
						this.setFacing(HorizontalDirection.Left, true);
					}
					else
					{
						this.setFacing(HorizontalDirection.Right, true);
					}
				}
			}
		}
		if (!this.animationPlayer.PlayAnimation(animationName, mirror, inModel.gameFrame, (!inModel.data.overrideBlendingIn) ? -1f : inModel.data.blendingIn, (!inModel.data.overrideBlendingOut) ? ((float)this.config.animationConfig.defaultMoveBlendOutDuration) : inModel.data.blendingOut))
		{
			UnityEngine.Debug.LogError(string.Concat(new string[]
			{
				"Failed to locate preloaded animation for move ",
				this.playerDelegate.CharacterData.characterName,
				".",
				animationName,
				". Make sure the move is listed in the character's move set!"
			}));
			this.playerDelegate.EndActiveMove(MoveEndType.Cancelled, false, false);
			if (horizontalDirection != HorizontalDirection.None)
			{
				this.setFacing(horizontalDirection, true);
			}
			return false;
		}
		MoveData data = this.Model.data;
		this.playerDelegate.Physics.OnCollisionBoundsChanged(true);
		if (data != this.Model.data)
		{
			if (horizontalDirection != HorizontalDirection.None)
			{
				this.setFacing(horizontalDirection, true);
			}
			return false;
		}
		if (this.Data.clearFastFall)
		{
			this.playerDelegate.Physics.ClearFastFall();
		}
		inModel.totalGameFrames = this.animationPlayer.CurrentAnimationGameFramelength;
		this.playerDelegate.LedgeGrabController.ReleaseGrabbedLedge(false, false);
		if (this.playerDelegate.GrabController.IsThrowMove(this.Data))
		{
			this.playerDelegate.GrabController.OnBeginThrow(this.Data);
		}
		CreateArticleAction[] articles = this.Data.articles;
		int num = 0;
		if (num < articles.Length)
		{
			CreateArticleAction createArticleAction = articles[num];
			inModel.articleFireAngle = createArticleAction.fireAngle;
		}
		if (previousMoveLinkedData != null)
		{
			foreach (MoveLinkComponentData current in previousMoveLinkedData)
			{
				current.Apply(ref inModel);
			}
		}
		inModel.internalFrame = (int)this.animationPlayer.GetAnimationFrameFromGameFrame(this.animationPlayer.CurrentAnimationName, inModel.gameFrame);
		this.model.firstFrameOfMove = 0;
		int num2 = 0;
		MoveData moveData = null;
		foreach (IMoveComponent current2 in this.Model.moveComponents)
		{
			if (current2 is IMoveStartComponent)
			{
				(current2 as IMoveStartComponent).OnStart(this.playerDelegate, input);
			}
			if (current2 is IMoveSkipAheadComponent)
			{
				IMoveSkipAheadComponent moveSkipAheadComponent = current2 as IMoveSkipAheadComponent;
				if (moveSkipAheadComponent.ShouldSkipToFrame)
				{
					num2 = Math.Max(num2, moveSkipAheadComponent.SkipToFrame);
				}
				if (moveSkipAheadComponent.ShouldSkipToMove)
				{
					moveData = moveSkipAheadComponent.SkipToMove;
				}
			}
		}
		if (moveData != null)
		{
			IPlayerDelegate arg_797_0 = this.playerDelegate;
			MoveData move = moveData;
			HorizontalDirection inputDirection = HorizontalDirection.None;
			int uid = this.Model.uid;
			List<MoveLinkComponentData> allLinkComponentData = this.GetAllLinkComponentData();
			MoveTransferSettings transferSettings = new MoveTransferSettings
			{
				transferHitDisableTargets = false,
				transferChargeData = true,
				chargeFractionOverride = this.getChargeFractionOverride()
			};
			arg_797_0.SetMove(move, input, inputDirection, uid, 0, default(Vector3F), transferSettings, allLinkComponentData, default(MoveSeedData), this.Model.chargeButton);
		}
		else if (num2 > 0)
		{
			this.syncMoveToFrame(num2);
			this.model.firstFrameOfMove = num2;
		}
		if (processMoveFrame)
		{
			this.processMoveFrame(this.Model, inModel.gameFrame, input);
		}
		return true;
	}

	private void onToggleDebugChannel(GameEvent message)
	{
		ToggleDebugDrawChannelCommand toggleDebugDrawChannelCommand = message as ToggleDebugDrawChannelCommand;
		if (toggleDebugDrawChannelCommand.channel == DebugDrawChannel.HitBoxes)
		{
			this.toggleHitBoxCapsules(toggleDebugDrawChannelCommand.enabled);
		}
	}

	private void toggleHitBoxCapsules(bool enabled)
	{
		if (this.Model == null || this.Model.hits.Count == 0)
		{
			return;
		}
		if (enabled)
		{
			foreach (Hit current in this.Model.hits)
			{
				foreach (HitBoxState current2 in current.hitBoxes)
				{
					CapsuleShape capsule = CapsulePool.Instance.GetCapsule(this.playerDelegate.Transform);
					capsule.Load((Vector3)current2.position, (Vector3)current2.lastPosition, (float)current2.Radius, current.data.DebugDrawColor, current2.IsCircle);
					this.Model.hitBoxCapsules[current2] = capsule;
				}
			}
		}
		else
		{
			foreach (KeyValuePair<HitBoxState, CapsuleShape> current3 in this.Model.hitBoxCapsules)
			{
				CapsuleShape value = current3.Value;
				value.Clear();
			}
			this.Model.hitBoxCapsules.Clear();
		}
	}

	public void UpdateHitboxPositions()
	{
		MoveData.UpdateHitboxPositions(this.Model.hits, this.Model.hitBoxCapsules, this.hitOwner, this.body);
	}

	public void TickFrame(InputButtonsData input)
	{
		if (this.Model == null || !this.IsActive || this.Model.paused)
		{
			return;
		}
		this.Model.wasSwitched = false;
		int totalGameFrames = this.Model.totalGameFrames;
		if (this.Model.gameFrame >= totalGameFrames)
		{
			for (int i = 0; i < this.Data.interrupts.Length; i++)
			{
				InterruptData interruptData = this.Data.interrupts[i];
				if (interruptData.ShouldUseLink(LinkCheckType.MoveEnd, this.playerDelegate, this.Model, input))
				{
					MoveData moveData = this.chooseLinkMove(interruptData);
					if (this.Model.repeatButtonPressed && this.playerDelegate.Model.lastMoveName == moveData.moveName)
					{
						this.playerDelegate.Model.repeatTrackMoveCount++;
					}
					bool flag = interruptData.triggerType != InterruptTriggerType.OnRepeatButtonPress || interruptData.repeatButtonPressMaxCount <= 0 || this.playerDelegate.Model.repeatTrackMoveCount < interruptData.repeatButtonPressMaxCount;
					if (flag)
					{
						if (interruptData.reverseFacing)
						{
							this.setFacing((this.playerDelegate.Facing != HorizontalDirection.Right) ? HorizontalDirection.Right : HorizontalDirection.Left, true);
						}
						IPlayerDelegate arg_1AE_0 = this.playerDelegate;
						MoveData move = moveData;
						HorizontalDirection inputDirection = HorizontalDirection.None;
						int uid = this.Model.uid;
						int nextMoveStartupFrame = interruptData.nextMoveStartupFrame;
						List<MoveLinkComponentData> allLinkComponentData = this.GetAllLinkComponentData();
						arg_1AE_0.SetMove(move, input, inputDirection, uid, nextMoveStartupFrame, default(Vector3F), new MoveTransferSettings
						{
							transferHitDisableTargets = interruptData.transferHitDisabledTargets,
							transferChargeData = interruptData.transferChargeData
						}, allLinkComponentData, default(MoveSeedData), this.Model.chargeButton);
						break;
					}
				}
			}
			if (!this.Model.wasSwitched)
			{
				this.onMoveChainEnd();
				this.playerDelegate.EndActiveMove(MoveEndType.Finished, true, false);
			}
			if (this.Model.wasSwitched)
			{
				this.TickFrame(input);
				return;
			}
		}
		if (this.IsActive && this.Model.data.label == MoveLabel.Tech && !this.playerDelegate.State.IsGrounded)
		{
			this.playerDelegate.EndActiveMove(MoveEndType.Cancelled, true, false);
		}
		if (this.IsActive && this.config.moveData.bReverseFrames > 0 && !this.Model.didBReverse && this.Data.label == MoveLabel.NeutralSpecial && !this.playerDelegate.State.IsGrounded && this.Model.internalFrame <= this.config.moveData.bReverseFrames && input.HorizontalDirection != HorizontalDirection.None && input.HorizontalDirection != this.playerDelegate.Facing)
		{
			this.Model.didBReverse = true;
			this.setFacing(this.playerDelegate.OppositeFacing, true);
			this.playerDelegate.Physics.ReverseHorizontalMovement();
		}
		if (this.IsActive)
		{
			int j = (int)this.animationPlayer.GetAnimationFrameFromGameFrame(this.animationPlayer.CurrentAnimationData.clipName, this.Model.gameFrame);
			int internalFrame = this.Model.internalFrame;
			while (j >= this.Model.internalFrame)
			{
				this.Model.internalFrame++;
				this.processMoveFrame(this.Model, internalFrame, input);
				if (this.Model.data == null)
				{
					this.onMoveChainEnd();
					return;
				}
				if (this.Model.wasSwitched)
				{
					this.TickFrame(input);
					return;
				}
			}
			if (this.IsActive)
			{
				this.processGameFrame(this.Model, input);
			}
		}
		this.incrementGameFrame();
	}

	private void onMoveChainEnd()
	{
		if (this.Model.deferredFacing != HorizontalDirection.None)
		{
			this.playerDelegate.SetFacingAndRotation(this.Model.deferredFacing);
		}
	}

	private void incrementGameFrame()
	{
		this.Model.gameFrame++;
	}

	private void setGameFrame(int toFrame)
	{
		this.Model.gameFrame = toFrame;
		if (GameClient.IsRollingBack)
		{
			this.playerDelegate.AnimationPlayer.ChangedAnimationDuringRollback();
		}
	}

	private void processGameFrame(MoveModel inModel, InputButtonsData input)
	{
		MoveData data = inModel.data;
		if (inModel.isCharging)
		{
			if (inModel.chargeFrames == 0 && inModel.ChargeData.flashWhileCharging)
			{
				this.playerDelegate.Renderer.SetColorModeFlag(ColorMode.Charging, true);
				inModel.beganChargeDisplay = true;
			}
			else if (!inModel.beganChargeDisplay && inModel.chargeFrames > 0 && inModel.ChargeData.flashWhileCharging)
			{
				this.playerDelegate.Renderer.SetColorModeFlag(ColorMode.Charging, true);
				inModel.beganChargeDisplay = true;
			}
			if (!this.isHoldingChargeButton(input))
			{
				this.releaseChargedMove(data);
			}
			else
			{
				inModel.chargeFrames++;
				foreach (IMoveComponent current in inModel.moveComponents)
				{
					if (current is IMoveChargeComponent)
					{
						(current as IMoveChargeComponent).OnContinueCharge();
					}
				}
			}
		}
		else if (this.Model.chargeFireDelay > 0)
		{
			this.Model.chargeFireDelay--;
			if (this.Model.chargeFireDelay <= 0)
			{
				this.fireChargedMove(data);
			}
		}
		if (this.physics.SteerMomentumMaxAnglePerFrame > 0)
		{
			Vector2F vector2F = this.physics.MovementVelocity;
			Vector2F v = -MathUtil.GetPerpendicularVector(vector2F);
			v.Normalize();
			Vector2F vector2F2 = new Vector2F(input.horizontalAxisValue, input.verticalAxisValue);
			Vector2F normalized = vector2F2.normalized;
			Fixed @fixed = Vector3F.Dot(normalized, v);
			@fixed = FixedMath.Clamp(@fixed, -1, 1);
			Fixed rotateZDegrees = @fixed * this.physics.SteerMomentumMaxAnglePerFrame;
			Vector2F vector2F3 = MathUtil.RotateVector(vector2F, rotateZDegrees);
			if (this.physics.SteerMomentumMinOverallAngle >= 0 && this.physics.SteerMomentumMaxOverallAngle >= 0)
			{
				Fixed fixed2 = MathUtil.VectorToAngle(ref vector2F3);
				Fixed fixed3 = FixedMath.ClampAngle(fixed2, this.physics.SteerMomentumMinOverallAngle, this.physics.SteerMomentumMaxOverallAngle);
				if (!FixedMath.ApproximatelyEqual(fixed3, fixed2))
				{
					vector2F3 = MathUtil.AngleToVector(fixed3) * vector2F.magnitude;
				}
			}
			this.physics.StopMovement(true, true, VelocityType.Movement);
			this.physics.AddVelocity(vector2F3, 1, VelocityType.Movement);
			if (inModel.lastAppliedForce != Vector2F.zero && inModel.applyForceContinuouslyEndFrame != -1)
			{
				inModel.lastAppliedForce = vector2F3;
			}
			if (this.physics.SteerMomentumFaceVelocity && !FixedMath.ApproximatelyEqual(vector2F3.x, 0))
			{
				this.setFacing((!(vector2F3.x < 0)) ? HorizontalDirection.Right : HorizontalDirection.Left, true);
			}
		}
		if (inModel.lastAppliedForce != Vector2F.zero && inModel.applyForceContinuouslyEndFrame != -1)
		{
			this.physics.StopMovement(true, true, VelocityType.Movement);
			this.physics.AddVelocity(inModel.lastAppliedForce, 1, VelocityType.Movement);
		}
		foreach (IMoveComponent current2 in this.Model.moveComponents)
		{
			if (current2 is IMoveTickGameFrameComponent)
			{
				(current2 as IMoveTickGameFrameComponent).TickGameFrame(input);
			}
		}
		if (this.body.HasRootMotion && !this.Data.disableRootMotion)
		{
			Vector3F deltaPosition = this.body.DeltaPosition;
			if (deltaPosition.sqrMagnitude > 0)
			{
				Vector3 vector = (!this.playerDelegate.State.IsGrounded) ? Vector3.up : ((Vector3)this.playerDelegate.Physics.GroundedNormal);
				Vector3 perpendicularVector = MathUtil.GetPerpendicularVector(vector);
				Vector3F delta = deltaPosition.x * (Vector3F)perpendicularVector + deltaPosition.y * (Vector3F)vector;
				this.physics.ForceTranslate(delta, true, true);
			}
		}
	}

	private void ApplyMaterialAnimationTriggersForSelf(MoveData move)
	{
		MaterialAnimationTrigger[] materialAnimationTriggers = move.materialAnimationTriggers;
		for (int i = 0; i < materialAnimationTriggers.Length; i++)
		{
			MaterialAnimationTrigger materialAnimationTrigger = materialAnimationTriggers[i];
			if (materialAnimationTrigger.MatchesTarget(MaterialAnimationTrigger.TargetType.Attacker) && this.Model.internalFrame == materialAnimationTrigger.startFrame)
			{
				this.playerDelegate.AddHostedMaterialAnimation(materialAnimationTrigger);
			}
		}
	}

	public bool WillReleaseGrabNextTick(MoveModel inModel)
	{
		MoveData data = inModel.data;
		int num = inModel.internalFrame + 1;
		foreach (Hit current in inModel.hits)
		{
			if (this.config.grabConfig.useRegrabDelay || !data.hasChainGrabAlternate || current.data.hitType != HitType.Throw || !current.data.releaseGrabbedOpponent || this.playerDelegate.GrabData.victimUnderChainGrabPrevention == current.data.isChainGrabThrow)
			{
				if (current.data.startFrame == num && current.data.hitType == HitType.Throw)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void processMoveFrame(MoveModel inModel, int previousFrame, InputButtonsData input)
	{
		MoveData data = inModel.data;
		if (data == null)
		{
			UnityEngine.Debug.LogError("Attempted to process a null move");
			return;
		}
		if (data.IsLedgeRecovery && this.Model.internalFrame > data.ledgeLockDuration)
		{
			this.playerDelegate.LedgeGrabController.ReleaseGrabbedLedge(true, false);
		}
		this.ApplyMaterialAnimationTriggersForSelf(data);
		if (data.chargeOptions.canCharge && this.Model.internalFrame >= data.chargeOptions.chargeBeginFrame && this.Model.internalFrame <= data.chargeOptions.chargeReleaseFrame)
		{
			if (!inModel.isCharging)
			{
				this.startChargedMove();
			}
			if (previousFrame < data.chargeOptions.chargeReleaseFrame && this.Model.internalFrame >= data.chargeOptions.chargeReleaseFrame)
			{
				this.releaseChargedMove(data);
			}
			else if (!this.isHoldingChargeButton(input) && this.Model.internalFrame == data.chargeOptions.chargeBeginFrame)
			{
				this.releaseChargedMove(data);
			}
		}
		int b = 0;
		foreach (Hit current in inModel.hits)
		{
			b = Mathf.Max(current.data.endFrame, b);
			if (this.config.grabConfig.useRegrabDelay || !data.hasChainGrabAlternate || current.data.hitType != HitType.Throw || !current.data.releaseGrabbedOpponent || this.playerDelegate.GrabData.victimUnderChainGrabPrevention == current.data.isChainGrabThrow)
			{
				bool flag = false;
				if (current.data.startFrame == inModel.internalFrame)
				{
					if (current.data.hitType == HitType.Throw)
					{
						IHitOwner hitOwner = this.hitOwner;
						PlayerController playerController = this.gameManager.GetPlayerController(this.playerDelegate.GrabData.grabbedOpponent);
						if (playerController == null)
						{
							UnityEngine.Debug.LogWarning("Attempting to throw an opponent when no opponent was grabbed");
							break;
						}
						bool flag2 = false;
						if (current.data.releaseGrabbedOpponent)
						{
							flag2 = true;
							this.playerDelegate.GrabController.ReleaseGrabbedOpponent(false);
						}
						Vector3F bonePosition = this.playerDelegate.Bones.GetBonePosition(BodyPart.throwBone, false);
						HitContext next = this.hitContextPool.GetNext();
						next.collisionPosition = bonePosition;
						Vector3F zero = Vector3F.zero;
						hitOwner.OnHitSuccess(current, playerController, ImpactType.Hit, ref bonePosition, ref zero, next);
						playerController.ReceiveHit(current.data, hitOwner, ImpactType.Hit, next);
						if (current.data.useTeleportCorrection)
						{
							Vector3F teleportCorrection = current.data.teleportCorrection;
							if (hitOwner.Facing == HorizontalDirection.Left)
							{
								teleportCorrection.x *= -1;
							}
							Vector3F a = hitOwner.Position + teleportCorrection;
							Vector3F b2 = a - playerController.Position;
							playerController.Physics.SetPosition(playerController.Physics.GetPosition() + b2);
						}
						if (flag2)
						{
							TimedHostedHit hit = new TimedHostedHit(playerController.StunFrames, this.config.knockbackConfig.throwHit, playerController, playerController.Bones, hitOwner, this.playerDelegate.HitCapsules, true);
							this.playerDelegate.AddHostedHit(hit);
						}
					}
					bool flag3 = false;
					foreach (Hit current2 in inModel.hits)
					{
						if (current2 != current)
						{
							if ((current2.data.IsActiveOnFrame(inModel.internalFrame) || current2.data.endFrame == inModel.internalFrame - 1) && (current2.data.startFrame != inModel.internalFrame || flag))
							{
								flag3 = true;
								break;
							}
						}
					}
					if (!flag3)
					{
						AudioData data2 = default(AudioData);
						if (current.data.overrideAttackSound)
						{
							data2 = current.data.attackSound;
						}
						else
						{
							HitEffectsData effect;
							if (current.data.hitEffectType == HitEffectType.Auto)
							{
								effect = this.config.hitConfig.GetEffect((Fixed)((double)current.data.damage));
							}
							else
							{
								effect = this.config.hitConfig.GetEffect(current.data.hitEffectType);
							}
							if (effect != null)
							{
								if (this.audioSettings.UseAltSounds() && effect.altAttackSound.sound != null)
								{
									data2 = effect.altAttackSound;
								}
								else
								{
									data2 = effect.attackSound;
								}
							}
						}
						if (data2.sound != null)
						{
							this.gameManager.Audio.PlayGameSound(new AudioRequest(data2, this.playerDelegate.AudioOwner, null));
							flag = true;
						}
					}
				}
			}
		}
		MoveCameraShakeData[] cameraShakes = data.cameraShakes;
		for (int i = 0; i < cameraShakes.Length; i++)
		{
			MoveCameraShakeData moveCameraShakeData = cameraShakes[i];
			if (moveCameraShakeData.frame == inModel.internalFrame)
			{
				this.gameManager.Camera.ShakeCamera(new CameraShakeRequest(moveCameraShakeData.shake));
			}
		}
		MoveTrailEmitterData[] trailEmitters = data.trailEmitters;
		for (int j = 0; j < trailEmitters.Length; j++)
		{
			MoveTrailEmitterData moveTrailEmitterData = trailEmitters[j];
			if (moveTrailEmitterData.startFrame == inModel.internalFrame)
			{
				this.playerDelegate.TrailEmitter.LoadEmitterData(moveTrailEmitterData.trailData);
			}
			else if (moveTrailEmitterData.endFrame == inModel.internalFrame)
			{
				this.playerDelegate.TrailEmitter.ResetData();
			}
		}
		if (this.config.defaultCharacterEffects.enableWeaponTrails)
		{
			this.weaponTrailHelper.UpdateWeaponTrailMap(this.model.visualData.weaponTrailMap, this.model.internalFrame, this.Data, this.body, this.createWeaponTrail);
		}
		CreateArticleAction[] articles = data.articles;
		for (int k = 0; k < articles.Length; k++)
		{
			CreateArticleAction createArticleAction = articles[k];
			if (this.Model.internalFrame == createArticleAction.fireFrame)
			{
				ArticleSpawnParameters articleSpawnParameters = this.articleSpawnCalculator.Calculate(createArticleAction, input, this.playerDelegate, this);
				ArticleController articleController = ArticleData.CreateArticleController(this.gameManager.DynamicObjects, createArticleAction.data.type, createArticleAction.data.prefab, 4);
				articleController.model.physicsModel.Reset();
				articleController.model.physicsModel.position = articleSpawnParameters.sourcePosition;
				articleController.model.rotationAngle = articleSpawnParameters.rotation;
				articleController.model.physicsModel.AddVelocity(ref articleSpawnParameters.velocity, VelocityType.Movement);
				articleController.model.currentFacing = articleSpawnParameters.facing;
				articleController.model.playerOwner = this.playerDelegate.PlayerNum;
				articleController.model.team = this.playerDelegate.Team;
				articleController.model.movementType = createArticleAction.movementType;
				articleController.model.moveLabel = this.Data.label;
				articleController.model.moveName = this.Data.moveName;
				articleController.model.moveUID = inModel.uid;
				articleController.model.staleDamageMultiplier = inModel.staleDamageMultiplier;
				articleController.model.chargeData = inModel.ChargeData;
				articleController.model.chargeFraction = inModel.ChargeFraction;
				articleController.Init(createArticleAction.data);
			}
		}
		MoveParticleEffect[] particleEffects = data.particleEffects;
		for (int l = 0; l < particleEffects.Length; l++)
		{
			MoveParticleEffect moveParticleEffect = particleEffects[l];
			if (moveParticleEffect.particleEffect.HasPrefab() && this.Model.internalFrame == moveParticleEffect.frame)
			{
				GeneratedEffect generatedEffect = this.playerDelegate.GameVFX.PlayParticle(moveParticleEffect.particleEffect, this.playerDelegate.SkinData, this.playerDelegate.Team);
				if (generatedEffect != null && moveParticleEffect.particleEffect.attach)
				{
					inModel.visualData.spawnedParticles.Add(new MoveModel.EffectHandle(generatedEffect.EffectController, moveParticleEffect.cancelCondition));
				}
			}
		}
		ForceData[] forces = data.forces;
		for (int m = 0; m < forces.Length; m++)
		{
			ForceData forceData = forces[m];
			bool flag4;
			if (forceData.applyOnChargeScaledDurationComplete)
			{
				flag4 = (this.Model.internalFrame == this.Model.applyForceContinuouslyEndFrame);
			}
			else if (forceData.repeatCast)
			{
				flag4 = (this.Model.internalFrame >= forceData.frame && this.Model.internalFrame <= forceData.endFrame);
			}
			else
			{
				flag4 = (this.Model.internalFrame == forceData.frame);
			}
			if (flag4 && (forceData.legalState == ForceData.LegalForceState.All || (forceData.legalState == ForceData.LegalForceState.AirOnly && !this.playerDelegate.State.IsGrounded) || (forceData.legalState == ForceData.LegalForceState.GroundOnly && this.playerDelegate.State.IsGrounded)))
			{
				Vector2F vector2F = Vector2F.one;
				if (forceData.forceStaling != null && forceData.forceStaling.Length > 0)
				{
					int moveUsedCount = this.playerDelegate.MoveUseTracker.GetMoveUsedCount(this.Data.label);
					for (int n = forceData.forceStaling.Length - 1; n >= 0; n--)
					{
						if (moveUsedCount >= forceData.forceStaling[n].numUses)
						{
							vector2F = forceData.forceStaling[n].multiplier;
							break;
						}
					}
				}
				if (forceData.chargeScalesForce)
				{
					vector2F *= this.model.ChargeForceMultiplier;
				}
				inModel.applyForceContinuouslyEndFrame = ((!forceData.isContinuous) ? (-1) : this.Data.totalInternalFrames);
				if (forceData.isContinuous && forceData.chargeScalesDuration)
				{
					int num = (int)((forceData.maxChargedDuration - forceData.minChargedDuration) * this.model.ChargeDurationMultiplier);
					inModel.applyForceContinuouslyEndFrame = forceData.frame + forceData.minChargedDuration + num;
				}
				this.physics.StopMovement(forceData.resetXVelocity, forceData.resetYVelocity, VelocityType.Movement);
				if (forceData.forceType != MoveForceType.Normal && forceData.forceType != MoveForceType.NormalInfluenced)
				{
					Vector2F vector2F2 = Vector2F.zero;
					if (forceData.hasDefaultInputDirection)
					{
						vector2F2 = MathUtil.AngleToVector(forceData.defaultInputDirection);
						vector2F2.x *= InputUtils.GetDirectionMultiplier(this.playerDelegate.Facing);
					}
					Vector2F vector2F3 = Vector2F.zero;
					Vector2F a2 = Vector2F.zero;
					Vector2F vector2F4 = Vector2F.zero;
					bool flag5 = false;
					foreach (IMoveComponent current3 in this.Model.moveComponents)
					{
						if (current3 is IMoveForceInputDirectionReader && (current3 as IMoveForceInputDirectionReader).ReadInputDirection(ref vector2F4))
						{
							flag5 = true;
							break;
						}
					}
					if (!flag5)
					{
						Vector2F vector2F5 = new Vector2F(input.horizontalAxisValue, input.verticalAxisValue);
						vector2F4 = vector2F5.normalized;
					}
					if (forceData.forceType == MoveForceType.Input360)
					{
						if (vector2F4.sqrMagnitude == 0)
						{
							vector2F4 = vector2F2;
						}
						else
						{
							this.playerDelegate.Model.bufferedInput.Clear();
						}
						a2 = vector2F4;
						if (forceData.minInput360Angle != 0 || forceData.maxInput360Angle != 0)
						{
							Vector2F vector2F6 = vector2F4;
							if (this.playerDelegate.Facing == HorizontalDirection.Left)
							{
								vector2F6.x = -vector2F6.x;
							}
							Fixed @fixed = FixedMath.Rad2Deg * FixedMath.Atan2(vector2F6.y, vector2F6.x);
							if (@fixed < forceData.minInput360Angle || @fixed > forceData.maxInput360Angle)
							{
								Fixed one = FixedMath.Abs(FixedMath.DeltaAngle(@fixed, forceData.minInput360Angle));
								Fixed other = FixedMath.Abs(FixedMath.DeltaAngle(@fixed, forceData.maxInput360Angle));
								if (one < other)
								{
									@fixed = forceData.minInput360Angle;
								}
								else
								{
									@fixed = forceData.maxInput360Angle;
								}
							}
							a2 = MathUtil.AngleToVector(@fixed);
							if (this.playerDelegate.Facing == HorizontalDirection.Left)
							{
								a2.x = -a2.x;
							}
						}
					}
					else if (forceData.forceType == MoveForceType.InputHorizontal)
					{
						if (vector2F4.sqrMagnitude == 0)
						{
							vector2F4 = vector2F2;
						}
						else
						{
							this.playerDelegate.Model.bufferedInput.Clear();
						}
						Vector2F vector2F7 = vector2F4;
						if (this.playerDelegate.Facing == HorizontalDirection.Left)
						{
							vector2F7.x = -vector2F7.x;
						}
						Fixed fixed2 = FixedMath.Rad2Deg * FixedMath.Atan2(vector2F7.y, vector2F7.x);
						while (fixed2 < 0)
						{
							fixed2 += 360;
						}
						if (fixed2 < 90)
						{
							fixed2 = 0;
						}
						else if (fixed2 < 180)
						{
							fixed2 = 180;
						}
						else if (fixed2 < 270)
						{
							if (fixed2 > 180 + forceData.maxInput360Angle)
							{
								fixed2 = 180 + forceData.maxInput360Angle;
							}
						}
						else if (fixed2 < 360 - forceData.maxInput360Angle)
						{
							fixed2 = 360 - forceData.maxInput360Angle;
						}
						a2 = MathUtil.AngleToVector(fixed2);
						if (this.playerDelegate.Facing == HorizontalDirection.Left)
						{
							a2.x = -a2.x;
						}
					}
					else if (forceData.forceType == MoveForceType.InputRotate)
					{
						if (vector2F4.sqrMagnitude != 0)
						{
							this.playerDelegate.Model.bufferedInput.Clear();
						}
						Vector2F input2 = MathUtil.AngleToVector(forceData.neutralRotationAngle);
						Vector2F v = -MathUtil.GetPerpendicularVector(input2);
						v.Normalize();
						Fixed fixed3 = Vector3F.Dot(vector2F4, v);
						fixed3 = FixedMath.Clamp(fixed3, -1, 1);
						Fixed rotateZDegrees = fixed3 * forceData.maxRotationAngleAmount;
						a2 = MathUtil.RotateVector(vector2F2, rotateZDegrees);
					}
					else if (forceData.forceType == MoveForceType.AssistTarget)
					{
						Vector3F assistTarget = inModel.assistTarget;
						Vector3F position = this.playerDelegate.Physics.GetPosition();
						Vector3 zero2 = Vector3.zero;
						zero2.x = (float)assistTarget.x - (float)position.x;
						zero2.y = (float)assistTarget.y - (float)position.y;
						float num2 = 57.29578f * Mathf.Atan2(zero2.y, zero2.x);
						a2 = MathUtil.AngleToVector((Fixed)((double)num2));
					}
					else if (forceData.forceType == MoveForceType.Input8Way)
					{
						if (vector2F4.sqrMagnitude == 0)
						{
							if (forceData.hasDefaultInputDirection)
							{
								vector2F4 = vector2F2;
							}
						}
						else
						{
							this.playerDelegate.Model.bufferedInput.Clear();
							vector2F4 = InputUtils.ClampVectorToEightWayInput(vector2F4);
						}
						a2 = vector2F4;
					}
					else
					{
						UnityEngine.Debug.LogWarning("Unsupported move force type " + forceData.forceType);
					}
					vector2F3 = (Fixed)((double)forceData.inputDirectionForce) * a2 * vector2F;
					inModel.lastAppliedForce = vector2F3;
					this.physics.AddVelocity(vector2F3, 1, VelocityType.Movement);
					if (!(vector2F3.x == 0))
					{
						if (forceData.setFacingDirection)
						{
							this.setFacing((!(vector2F3.x < 0)) ? HorizontalDirection.Right : HorizontalDirection.Left, true);
						}
					}
				}
				else
				{
					int directionMultiplier = InputUtils.GetDirectionMultiplier(this.playerDelegate.Facing);
					Vector2F vector2F8 = (Vector2F)forceData.force;
					vector2F8.x *= directionMultiplier;
					if (forceData.forceType == MoveForceType.NormalInfluenced)
					{
						Vector2F vector2F9 = new Vector2F(input.horizontalAxisValue, input.verticalAxisValue);
						Vector2F vector = vector2F9.normalized;
						vector = InputUtils.ClampVectorToEightWayInput(vector);
						if (vector.x != 0)
						{
							vector2F8.x += forceData.stickInfluence.x * -vector.x;
							if (directionMultiplier / vector.x >= 0)
							{
								vector2F8.y -= forceData.stickInfluence.y * FixedMath.Abs(vector.x);
							}
							else
							{
								vector2F8.y += forceData.stickInfluence.y * FixedMath.Abs(vector.x);
							}
						}
					}
					vector2F8 *= vector2F;
					if (forceData.isFrictionForce)
					{
						Vector2F vector2F10 = this.playerDelegate.Physics.Velocity;
						if (MathUtil.SignsMatch(vector2F10.x, vector2F8.x))
						{
							vector2F8.x = 0;
						}
						else if (FixedMath.Abs(vector2F8.x) > FixedMath.Abs(vector2F10.x))
						{
							vector2F8.x = -vector2F10.x;
						}
						if (MathUtil.SignsMatch(vector2F10.y, vector2F8.y))
						{
							vector2F8.y = 0;
						}
						else if (FixedMath.Abs(vector2F8.y) > FixedMath.Abs(vector2F10.y))
						{
							vector2F8.y = -vector2F10.y;
						}
					}
					inModel.lastAppliedForce = vector2F8;
					this.physics.AddVelocity(vector2F8, 1, VelocityType.Movement);
				}
			}
		}
		PhysicsOverride[] physicsOverrides = data.physicsOverrides;
		for (int num3 = 0; num3 < physicsOverrides.Length; num3++)
		{
			PhysicsOverride physicsOverride = physicsOverrides[num3];
			if (physicsOverride != null)
			{
				bool flag6;
				if (physicsOverride.applyOnChargeScaledDurationComplete)
				{
					flag6 = (this.Model.internalFrame == inModel.applyPhysicsOverrideEndFrame);
				}
				else
				{
					flag6 = (this.Model.internalFrame == physicsOverride.frame);
				}
				if (flag6)
				{
					if (physicsOverride.restoreDefaults)
					{
						this.physics.SetOverride(null);
					}
					else
					{
						inModel.applyPhysicsOverrideEndFrame = ((!physicsOverride.chargeScalesDuration) ? (-1) : this.Data.totalInternalFrames);
						if (physicsOverride.chargeScalesDuration)
						{
							int num4 = (int)((physicsOverride.maxChargedDuration - physicsOverride.minChargedDuration) * this.model.ChargeDurationMultiplier);
							inModel.applyPhysicsOverrideEndFrame = physicsOverride.frame + physicsOverride.minChargedDuration + num4;
						}
						this.physics.SetOverride(physicsOverride);
					}
				}
			}
		}
		if (this.Model.addImpulseCountdown > 0 && --this.Model.addImpulseCountdown == 0 && (this.Model.addImpulse.x != 0 || this.Model.addImpulse.y != 0))
		{
			this.physics.AddVelocity(this.Model.addImpulse, 1, VelocityType.Movement);
			this.Model.addImpulse.x = 0;
			this.Model.addImpulse.y = 0;
		}
		SoundEffect[] soundEffects = data.soundEffects;
		for (int num5 = 0; num5 < soundEffects.Length; num5++)
		{
			SoundEffect soundEffect = soundEffects[num5];
			if (this.Model.internalFrame == soundEffect.frame && soundEffect.sounds.Length > 0)
			{
				inModel.visualData.spawnedAudio.Add(new MoveAudioHandle(this.gameManager.Audio, soundEffect, this.Model, this.playerDelegate.AudioOwner));
			}
		}
		if (data.intangibleBodyParts.Length > 0)
		{
			IntangibleBodyParts[] intangibleBodyParts = data.intangibleBodyParts;
			for (int num6 = 0; num6 < intangibleBodyParts.Length; num6++)
			{
				IntangibleBodyParts intangibleBodyParts2 = intangibleBodyParts[num6];
				if (this.Model.internalFrame == intangibleBodyParts2.startFrame)
				{
					if (intangibleBodyParts2.completelyIntangible)
					{
						this.playerDelegate.Invincibility.BeginMoveIntangibility(null);
					}
					else
					{
						this.playerDelegate.Invincibility.BeginMoveIntangibility(intangibleBodyParts2.bodyParts);
					}
				}
				if (this.Model.internalFrame == intangibleBodyParts2.endFrame)
				{
					this.playerDelegate.Invincibility.EndMoveIntangibility();
				}
			}
		}
		if (data.projectileInvincibility.Length > 0)
		{
			IntangibleBodyParts[] projectileInvincibility = data.projectileInvincibility;
			for (int num7 = 0; num7 < projectileInvincibility.Length; num7++)
			{
				IntangibleBodyParts intangibleBodyParts3 = projectileInvincibility[num7];
				if (this.Model.internalFrame == intangibleBodyParts3.startFrame)
				{
					if (intangibleBodyParts3.completelyIntangible)
					{
						this.playerDelegate.Invincibility.BeginMoveProjectileIntangibility(null);
					}
					else
					{
						this.playerDelegate.Invincibility.BeginMoveProjectileIntangibility(intangibleBodyParts3.bodyParts);
					}
				}
				if (this.Model.internalFrame == intangibleBodyParts3.endFrame)
				{
					this.playerDelegate.Invincibility.EndMoveProjectileIntangibility();
				}
			}
		}
		foreach (IMoveComponent current4 in this.Model.moveComponents)
		{
			if (current4 is IMoveTickMoveFrameComponent)
			{
				(current4 as IMoveTickMoveFrameComponent).TickMoveFrame(input);
			}
		}
		if (!this.TryToLinkMove(inModel, input))
		{
			for (int num8 = 0; num8 < this.Data.interrupts.Length; num8++)
			{
				InterruptData interruptData = this.Data.interrupts[num8];
				if (this.Data.canBufferInput && interruptData.startFrame == inModel.internalFrame)
				{
					this.playerDelegate.ProcessBufferedInput();
					break;
				}
			}
		}
	}

	public bool TryToLinkMove(MoveModel inModel, InputButtonsData input)
	{
		if (inModel == null || this.Data == null)
		{
			return false;
		}
		for (int i = 0; i < this.Data.interrupts.Length; i++)
		{
			InterruptData interruptData = this.Data.interrupts[i];
			if (interruptData.RepeatButtonsPressed(inModel, input))
			{
				inModel.repeatButtonPressed = true;
			}
			if (interruptData.ImmediateButtonPressed(inModel, input))
			{
				inModel.immediateButtonPressed = true;
			}
			if (interruptData.ShouldUseLink(LinkCheckType.MoveFrame, this.playerDelegate, inModel, input))
			{
				if (interruptData.reverseFacing)
				{
					this.setFacing(InputUtils.GetOppositeDirection(this.playerDelegate.Facing), true);
				}
				MoveData moveData = this.chooseLinkMove(interruptData);
				IPlayerDelegate arg_10F_0 = this.playerDelegate;
				MoveData move = moveData;
				HorizontalDirection inputDirection = HorizontalDirection.None;
				int uid = inModel.uid;
				int nextMoveStartupFrame = interruptData.nextMoveStartupFrame;
				List<MoveLinkComponentData> allLinkComponentData = this.GetAllLinkComponentData();
				arg_10F_0.SetMove(move, input, inputDirection, uid, nextMoveStartupFrame, default(Vector3F), new MoveTransferSettings
				{
					transferHitDisableTargets = interruptData.transferHitDisabledTargets,
					transferChargeData = interruptData.transferChargeData,
					chargeFractionOverride = this.getChargeFractionOverride()
				}, allLinkComponentData, default(MoveSeedData), inModel.chargeButton);
				inModel.repeatButtonPressed = false;
				inModel.immediateButtonPressed = false;
				return true;
			}
		}
		return false;
	}

	private Fixed getChargeFractionOverride()
	{
		IMoveOverrideChargeComponent component = this.Model.GetComponent<IMoveOverrideChargeComponent>();
		if (component == null)
		{
			return -1;
		}
		return component.OverrideChargeFraction;
	}

	private MoveData chooseLinkMove(InterruptData link)
	{
		if (link.getLinkableMoveFromComponent)
		{
			IMoveLinkProvider component = this.Model.GetComponent<IMoveLinkProvider>();
			if (component != null)
			{
				return component.GetLinkedMove(link);
			}
		}
		if (link.linkableMoves.Length > 0)
		{
			return link.linkableMoves[0];
		}
		return null;
	}

	public List<MoveLinkComponentData> GetAllLinkComponentData()
	{
		this.linkComponentBuffer.Clear();
		foreach (IMoveComponent current in this.Model.moveComponents)
		{
			if (current is IMoveLinkComponent)
			{
				this.linkComponentBuffer.Add((current as IMoveLinkComponent).GetLinkComponentData());
			}
		}
		return this.linkComponentBuffer;
	}

	public bool onParticleCreated(ParticleData particle, GameObject gameObject)
	{
		foreach (IMoveComponent current in this.Model.moveComponents)
		{
			if (current is ITaggedParticleListener)
			{
				(current as ITaggedParticleListener).OnCreateTaggedParticle(particle.tag, gameObject);
			}
		}
		return true;
	}

	private bool isHoldingChargeButton(InputButtonsData input)
	{
		if (input.buttonsHeld.Contains(this.Model.chargeButton))
		{
			return true;
		}
		if (this.playerDelegate.Facing != this.Model.initialFacing && InputUtils.IsHorizontal(this.Model.chargeButton))
		{
			ButtonPress oppositeHorizontalButton = InputUtils.GetOppositeHorizontalButton(this.Model.chargeButton);
			if (input.buttonsHeld.Contains(oppositeHorizontalButton))
			{
				return true;
			}
		}
		return false;
	}

	private XWeaponTrail createWeaponTrailFunc()
	{
		return this.gameManager.DynamicObjects.InstantiateDynamicObject<XWeaponTrail>(this.config.moveData.WeaponTrailPrefab, 4, true);
	}

	private void startChargedMove()
	{
		this.Model.isCharging = true;
		foreach (IMoveComponent current in this.Model.moveComponents)
		{
			if (current is IMoveChargeComponent)
			{
				(current as IMoveChargeComponent).OnStartCharge();
			}
		}
	}

	private void syncMoveToFrame(int frame)
	{
		this.setGameFrame(this.animationPlayer.GetNextGameFrameFromAnimationFrame(this.animationPlayer.CurrentAnimationData.clipName, frame) - 1);
		this.Model.internalFrame = frame;
	}

	private void releaseChargedMove(MoveData move)
	{
		this.syncMoveToFrame(move.chargeOptions.chargeReleaseFrame);
		this.playerDelegate.Renderer.SetColorModeFlag(ColorMode.Charging, false);
		this.Model.isCharging = false;
		int num = 0;
		foreach (IMoveComponent current in this.Model.moveComponents)
		{
			if (current is IMoveChargeComponent)
			{
				IMoveChargeComponent moveChargeComponent = current as IMoveChargeComponent;
				moveChargeComponent.OnEndCharge();
				num = Math.Max(num, moveChargeComponent.ChargeFireDelay);
			}
		}
		if (num > 0)
		{
			this.Model.chargeFireDelay = num;
		}
		else
		{
			this.fireChargedMove(move);
		}
	}

	private void fireChargedMove(MoveData move)
	{
		foreach (IMoveComponent current in this.Model.moveComponents)
		{
			if (current is IMoveChargeComponent)
			{
				(current as IMoveChargeComponent).OnFireCharge();
			}
		}
		this.Model.chargeFireDelay = 0;
	}

	public bool LoadState(RollbackStateContainer container)
	{
		List<IMoveComponent> moveComponents = this.Model.moveComponents;
		container.ReadState<MoveModel>(ref this.model);
		foreach (IMoveComponent current in moveComponents)
		{
			if (!this.Model.moveComponents.Contains(current) && current is IMoveEndComponent)
			{
				(current as IMoveEndComponent).OnEnd();
			}
		}
		foreach (IMoveComponent current2 in this.Model.moveComponents)
		{
			if (current2 is IRollbackStateOwner)
			{
				((IRollbackStateOwner)current2).LoadState(container);
			}
		}
		foreach (KeyValuePair<WeaponTrailData, XWeaponTrail> current3 in this.Model.visualData.weaponTrailMap)
		{
			current3.Value.LoadState(container);
		}
		return true;
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<MoveModel>(this.Model));
		foreach (IMoveComponent current in this.Model.moveComponents)
		{
			if (current is IRollbackStateOwner)
			{
				((IRollbackStateOwner)current).ExportState(ref container);
			}
		}
		foreach (KeyValuePair<WeaponTrailData, XWeaponTrail> current2 in this.Model.visualData.weaponTrailMap)
		{
			XWeaponTrail xWeaponTrail = this.Model.visualData.weaponTrailMap[current2.Key];
			xWeaponTrail.ExportState(ref container);
		}
		return true;
	}

	private void onEffectReleased(Effect theEffect)
	{
		MoveController._onEffectReleased_c__AnonStorey0 _onEffectReleased_c__AnonStorey = new MoveController._onEffectReleased_c__AnonStorey0();
		_onEffectReleased_c__AnonStorey.theEffect = theEffect;
		this.Model.visualData.spawnedParticles.RemoveFirst(new Predicate<MoveModel.EffectHandle>(_onEffectReleased_c__AnonStorey.__m__0));
	}

	public void Reset(MoveEndType moveEndType, bool transitioningToContinuingMove)
	{
		if (this.IsActive)
		{
			foreach (IMoveComponent current in this.Model.moveComponents)
			{
				if (current is IMoveEndComponent)
				{
					(current as IMoveEndComponent).OnEnd();
				}
			}
		}
		this.clearFinishedParticles(moveEndType, transitioningToContinuingMove);
		this.clearFinishedAudio(moveEndType, transitioningToContinuingMove);
		this.Model.Clear();
	}

	private void clearFinishedParticles(MoveEndType moveEndType, bool transitioningToContinuingMove)
	{
		if (!transitioningToContinuingMove)
		{
			this.weaponTrailHelper.ClearWeaponTrails(this.model.visualData.weaponTrailMap);
		}
		for (int i = this.Model.visualData.spawnedParticles.Count - 1; i >= 0; i--)
		{
			if (this.Model.visualData.spawnedParticles[i].TryToStop(moveEndType, transitioningToContinuingMove))
			{
				this.Model.visualData.spawnedParticles.RemoveAt(i);
			}
		}
	}

	private void clearFinishedAudio(MoveEndType moveEndType, bool transitioningToContinuingMove)
	{
		for (int i = this.Model.visualData.spawnedAudio.Count - 1; i >= 0; i--)
		{
			if (this.Model.visualData.spawnedAudio[i].TryToStop(moveEndType, transitioningToContinuingMove, this.Model.gameFrame))
			{
				this.Model.visualData.spawnedAudio.RemoveAt(i);
			}
		}
	}

	private void clearAllParticles()
	{
		this.weaponTrailHelper.ClearWeaponTrails(this.model.visualData.weaponTrailMap);
		for (int i = this.Model.visualData.spawnedParticles.Count - 1; i >= 0; i--)
		{
			this.Model.visualData.spawnedParticles[i].Effect.Destroy();
		}
		this.Model.visualData.spawnedParticles.Clear();
	}

	private void clearAllAudio()
	{
		for (int i = this.Model.visualData.spawnedAudio.Count - 1; i >= 0; i--)
		{
			this.Model.visualData.spawnedAudio[i].Stop();
		}
		this.Model.visualData.spawnedAudio.Clear();
	}

	public void Destroy()
	{
		this.clearAllParticles();
		this.clearAllAudio();
		this.Model.Clear();
		this.events.Unsubscribe(typeof(ToggleDebugDrawChannelCommand), new Events.EventHandler(this.onToggleDebugChannel));
		this.signalBus.GetSignal<EffectReleaseSignal>().RemoveListener(new Action<Effect>(this.onEffectReleased));
	}

	private void GetWaveDashParticle(ref ParticleData landParticle)
	{
		if (this.Data.label == MoveLabel.AirDodge)
		{
			bool flag = false;
			PhysicsOverride[] physicsOverrides = this.Data.physicsOverrides;
			for (int i = 0; i < physicsOverrides.Length; i++)
			{
				PhysicsOverride physicsOverride = physicsOverrides[i];
				if (physicsOverride.restoreDefaults && this.Model.internalFrame < physicsOverride.frame)
				{
					flag = true;
				}
			}
			if (flag)
			{
				CharacterParticleData particles = this.playerDelegate.CharacterData.particles;
				float num = Mathf.Abs((float)this.playerDelegate.Physics.MovementVelocity.x);
				if (num >= particles.wavedashMax)
				{
					landParticle = this.config.defaultCharacterEffects.wavedashMax;
				}
				else if (num >= particles.wavedashMedium)
				{
					landParticle = this.config.defaultCharacterEffects.wavedashMedium;
				}
				else if (num >= particles.wavedashShort)
				{
					landParticle = this.config.defaultCharacterEffects.wavedashShort;
				}
				else if (this.config.defaultCharacterEffects.wavedashInPlace != null)
				{
					landParticle = this.config.defaultCharacterEffects.wavedashInPlace;
				}
			}
		}
	}

	public bool OnLand(ref ParticleData landParticle, ref bool bounced, ref int landInterruptFrames, ref int landVisualFrames, ref string overrideAnimationName, ref string overrideLeftAnimationName, ref MoveData nextMove, ICharacterInputProcessor inputProcessor)
	{
		this.GetWaveDashParticle(ref landParticle);
		InterruptData interruptData = null;
		for (int i = 0; i < this.Data.interrupts.Length; i++)
		{
			InterruptData interruptData2 = this.Data.interrupts[i];
			if (interruptData2.ShouldUseLink(LinkCheckType.OnLand, this.playerDelegate, this.model, inputProcessor.CurrentInputData))
			{
				interruptData = interruptData2;
				break;
			}
		}
		LandCancelOptions landCancelOptions;
		if (interruptData != null)
		{
			landCancelOptions = interruptData.GetLandCancelOptions();
			landCancelOptions.landMove = this.chooseLinkMove(interruptData);
		}
		else
		{
			if (!this.Data.cancelOptions.cancelOnLand)
			{
				return false;
			}
			landCancelOptions = this.Data.cancelOptions.GetLandCancelOptions();
		}
		if (landCancelOptions.haltMovementOnLand)
		{
			Fixed x = FixedMath.Clamp(this.playerDelegate.Physics.Velocity.x, -landCancelOptions.maxHorizontalLandSpeed, landCancelOptions.maxHorizontalLandSpeed);
			this.playerDelegate.Physics.StopMovement(true, true, VelocityType.Movement);
			this.physics.AddVelocity(new Vector2F(x, 0), 1, VelocityType.Movement);
		}
		if (landCancelOptions.landMove != null)
		{
			int num = 1;
			if (landCancelOptions.startLandAtCurrentFrame)
			{
				num = this.Model.gameFrame;
			}
			if (num <= this.model.totalGameFrames)
			{
				if (num == this.model.totalGameFrames)
				{
					num--;
				}
				IPlayerDelegate arg_1FE_0 = this.playerDelegate;
				MoveData landMove = landCancelOptions.landMove;
				InputButtonsData currentInputData = inputProcessor.CurrentInputData;
				HorizontalDirection inputDirection = HorizontalDirection.None;
				int uid = this.Model.uid;
				int startFrame = num;
				List<MoveLinkComponentData> allLinkComponentData = this.GetAllLinkComponentData();
				arg_1FE_0.SetMove(landMove, currentInputData, inputDirection, uid, startFrame, default(Vector3F), new MoveTransferSettings
				{
					transitioningToContinuingMove = landCancelOptions.startLandAtCurrentFrame,
					transferHitDisableTargets = landCancelOptions.transferHitDisabledTargets,
					transferChargeData = landCancelOptions.transferChargeData
				}, allLinkComponentData, default(MoveSeedData), this.Model.chargeButton);
				return false;
			}
		}
		if (landCancelOptions.bounceSpeed > 0)
		{
			this.playerDelegate.Physics.StopMovement(false, true, VelocityType.Movement);
			this.physics.AddVelocity(new Vector2F(0, landCancelOptions.bounceSpeed), 1, VelocityType.Movement);
			bounced = true;
			if (landCancelOptions.bounceParticle != null && landCancelOptions.bounceParticle.prefab != null)
			{
				landParticle = landCancelOptions.bounceParticle;
			}
			if (landCancelOptions.bounceCancelMove != null)
			{
				nextMove = landCancelOptions.bounceCancelMove;
			}
		}
		if (landCancelOptions.useLandCameraShake)
		{
			this.gameManager.Camera.ShakeCamera(new CameraShakeRequest(landCancelOptions.landingCameraShake.shake));
		}
		if (landCancelOptions.sound.sound != null)
		{
			this.gameManager.Audio.PlayGameSound(new AudioRequest(landCancelOptions.sound, this.playerDelegate.AudioOwner, null));
		}
		if (this.Model.internalFrame >= landCancelOptions.noAutoCancelFramesBegin && this.Model.internalFrame <= landCancelOptions.noAutoCancelFramesEnd)
		{
			if (landCancelOptions.landCancelClip != null)
			{
				overrideAnimationName = MoveCancelData.GetClipName(this.Data, false);
			}
			if (landCancelOptions.leftLandCancelClip != null)
			{
				overrideLeftAnimationName = MoveCancelData.GetClipName(this.Data, true);
			}
			landInterruptFrames = landCancelOptions.landLagFrames;
			landVisualFrames = landCancelOptions.landLagVisualFrames;
		}
		return true;
	}

	public bool OnFall(ICharacterInputProcessor inputProcessor)
	{
		InterruptData interruptData = null;
		for (int i = 0; i < this.Data.interrupts.Length; i++)
		{
			InterruptData interruptData2 = this.Data.interrupts[i];
			if (interruptData2.ShouldUseLink(LinkCheckType.OnFall, this.playerDelegate, this.model, inputProcessor.CurrentInputData))
			{
				interruptData = interruptData2;
				break;
			}
		}
		FallCancelOptions fallCancelOptions;
		if (interruptData != null)
		{
			fallCancelOptions = interruptData.GetFallCancelOptions();
			fallCancelOptions.fallMove = this.chooseLinkMove(interruptData);
		}
		else
		{
			if (!this.Data.cancelOptions.cancelOnFall)
			{
				return false;
			}
			fallCancelOptions = this.Data.cancelOptions.GetFallCancelOptions();
		}
		if (fallCancelOptions.fallMove != null)
		{
			int num = 1;
			if (fallCancelOptions.startFallAtCurrentFrame)
			{
				num = this.Model.gameFrame;
			}
			IPlayerDelegate arg_151_0 = this.playerDelegate;
			MoveData fallMove = fallCancelOptions.fallMove;
			InputButtonsData currentInputData = inputProcessor.CurrentInputData;
			HorizontalDirection inputDirection = HorizontalDirection.None;
			int uid = this.Model.uid;
			int startFrame = num;
			List<MoveLinkComponentData> allLinkComponentData = this.GetAllLinkComponentData();
			arg_151_0.SetMove(fallMove, currentInputData, inputDirection, uid, startFrame, default(Vector3F), new MoveTransferSettings
			{
				transitioningToContinuingMove = fallCancelOptions.startFallAtCurrentFrame,
				transferHitDisableTargets = fallCancelOptions.transferHitDisableTargets
			}, allLinkComponentData, default(MoveSeedData), this.Model.chargeButton);
			return true;
		}
		return false;
	}

	private void setFacing(HorizontalDirection direction, bool doRotate)
	{
		if (this.Model.deferredFacing != HorizontalDirection.None || (this.Model.data != null && this.Model.data.deferFacingChanges))
		{
			this.Model.deferredFacing = direction;
		}
		else
		{
			this.playerDelegate.SetFacing(direction);
			if (doRotate)
			{
				this.playerDelegate.SetRotation(direction, true);
			}
		}
	}

	public bool DoesArmorResistHit(Fixed knockbackForce)
	{
		return this.IsActive && this.Data.armorOptions != null && this.Data.armorOptions.hasArmor && this.Model.internalFrame >= this.Data.armorOptions.startFrame && this.Model.internalFrame <= this.Data.armorOptions.endFrame && knockbackForce <= this.Data.armorOptions.knockbackThreshold;
	}

	private XWeaponTrail _MoveController_m__0()
	{
		return this.createWeaponTrailFunc();
	}
}
