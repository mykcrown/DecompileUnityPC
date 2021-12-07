using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;
using Xft;

// Token: 0x0200051E RID: 1310
public class MoveController : IMoveDelegate, IRollbackStateOwner
{
	// Token: 0x06001C1A RID: 7194 RVA: 0x0008E2BB File Offset: 0x0008C6BB
	public MoveController()
	{
		this.createWeaponTrail = (() => this.createWeaponTrailFunc());
	}

	// Token: 0x170005F8 RID: 1528
	// (get) Token: 0x06001C1B RID: 7195 RVA: 0x0008E2E0 File Offset: 0x0008C6E0
	// (set) Token: 0x06001C1C RID: 7196 RVA: 0x0008E2E8 File Offset: 0x0008C6E8
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x170005F9 RID: 1529
	// (get) Token: 0x06001C1D RID: 7197 RVA: 0x0008E2F1 File Offset: 0x0008C6F1
	// (set) Token: 0x06001C1E RID: 7198 RVA: 0x0008E2F9 File Offset: 0x0008C6F9
	[Inject]
	public MoveArticleSpawnCalculator articleSpawnCalculator { get; set; }

	// Token: 0x170005FA RID: 1530
	// (get) Token: 0x06001C1F RID: 7199 RVA: 0x0008E302 File Offset: 0x0008C702
	// (set) Token: 0x06001C20 RID: 7200 RVA: 0x0008E30A File Offset: 0x0008C70A
	[Inject]
	public IMoveAnimationCalculator moveAnimationCalculator { get; set; }

	// Token: 0x170005FB RID: 1531
	// (get) Token: 0x06001C21 RID: 7201 RVA: 0x0008E313 File Offset: 0x0008C713
	// (set) Token: 0x06001C22 RID: 7202 RVA: 0x0008E31B File Offset: 0x0008C71B
	[Inject]
	public IWeaponTrailHelper weaponTrailHelper { get; set; }

	// Token: 0x170005FC RID: 1532
	// (get) Token: 0x06001C23 RID: 7203 RVA: 0x0008E324 File Offset: 0x0008C724
	// (set) Token: 0x06001C24 RID: 7204 RVA: 0x0008E32C File Offset: 0x0008C72C
	[Inject]
	public DeveloperConfig devConfig { get; set; }

	// Token: 0x170005FD RID: 1533
	// (get) Token: 0x06001C25 RID: 7205 RVA: 0x0008E335 File Offset: 0x0008C735
	// (set) Token: 0x06001C26 RID: 7206 RVA: 0x0008E33D File Offset: 0x0008C73D
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x170005FE RID: 1534
	// (get) Token: 0x06001C27 RID: 7207 RVA: 0x0008E346 File Offset: 0x0008C746
	// (set) Token: 0x06001C28 RID: 7208 RVA: 0x0008E34E File Offset: 0x0008C74E
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x170005FF RID: 1535
	// (get) Token: 0x06001C29 RID: 7209 RVA: 0x0008E357 File Offset: 0x0008C757
	// (set) Token: 0x06001C2A RID: 7210 RVA: 0x0008E35F File Offset: 0x0008C75F
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x17000600 RID: 1536
	// (get) Token: 0x06001C2B RID: 7211 RVA: 0x0008E368 File Offset: 0x0008C768
	// (set) Token: 0x06001C2C RID: 7212 RVA: 0x0008E370 File Offset: 0x0008C770
	[Inject]
	public IHitContextPool hitContextPool { get; set; }

	// Token: 0x17000601 RID: 1537
	// (get) Token: 0x06001C2D RID: 7213 RVA: 0x0008E379 File Offset: 0x0008C779
	// (set) Token: 0x06001C2E RID: 7214 RVA: 0x0008E381 File Offset: 0x0008C781
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000602 RID: 1538
	// (get) Token: 0x06001C2F RID: 7215 RVA: 0x0008E38A File Offset: 0x0008C78A
	// (set) Token: 0x06001C30 RID: 7216 RVA: 0x0008E392 File Offset: 0x0008C792
	[Inject]
	public UserAudioSettings audioSettings { get; set; }

	// Token: 0x17000603 RID: 1539
	// (get) Token: 0x06001C31 RID: 7217 RVA: 0x0008E39B File Offset: 0x0008C79B
	public MoveData Data
	{
		get
		{
			return this.Model.data;
		}
	}

	// Token: 0x17000604 RID: 1540
	// (get) Token: 0x06001C32 RID: 7218 RVA: 0x0008E3A8 File Offset: 0x0008C7A8
	// (set) Token: 0x06001C33 RID: 7219 RVA: 0x0008E3B0 File Offset: 0x0008C7B0
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

	// Token: 0x06001C34 RID: 7220 RVA: 0x0008E3BC File Offset: 0x0008C7BC
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

	// Token: 0x06001C35 RID: 7221 RVA: 0x0008E47C File Offset: 0x0008C87C
	public void WasReversed()
	{
		if (this.Model.data.animationClipLeft != null)
		{
			MoveAnimationResult playableData = this.moveAnimationCalculator.GetPlayableData(this.Model.data, this.playerDelegate);
			string animationName = playableData.animationName;
			bool mirror = playableData.mirror;
			if (!this.animationPlayer.PlayAnimation(animationName, mirror, this.Model.gameFrame, (!this.Model.data.overrideBlendingIn) ? -1f : this.Model.data.blendingIn, (!this.Model.data.overrideBlendingOut) ? ((float)this.config.animationConfig.defaultMoveBlendOutDuration) : this.Model.data.blendingOut))
			{
				Debug.LogError(string.Concat(new string[]
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

	// Token: 0x06001C36 RID: 7222 RVA: 0x0008E5AC File Offset: 0x0008C9AC
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
		foreach (MoveComponent original in components)
		{
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
			Debug.Log("Terrible");
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
			Debug.LogError(string.Concat(new string[]
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
			foreach (MoveLinkComponentData moveLinkComponentData in previousMoveLinkedData)
			{
				moveLinkComponentData.Apply(ref inModel);
			}
		}
		inModel.internalFrame = (int)this.animationPlayer.GetAnimationFrameFromGameFrame(this.animationPlayer.CurrentAnimationName, inModel.gameFrame);
		this.model.firstFrameOfMove = 0;
		int num2 = 0;
		MoveData moveData = null;
		foreach (IMoveComponent moveComponent2 in this.Model.moveComponents)
		{
			if (moveComponent2 is IMoveStartComponent)
			{
				(moveComponent2 as IMoveStartComponent).OnStart(this.playerDelegate, input);
			}
			if (moveComponent2 is IMoveSkipAheadComponent)
			{
				IMoveSkipAheadComponent moveSkipAheadComponent = moveComponent2 as IMoveSkipAheadComponent;
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
			IPlayerDelegate playerDelegate = this.playerDelegate;
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
			playerDelegate.SetMove(move, input, inputDirection, uid, 0, default(Vector3F), transferSettings, allLinkComponentData, default(MoveSeedData), this.Model.chargeButton);
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

	// Token: 0x06001C37 RID: 7223 RVA: 0x0008EDB0 File Offset: 0x0008D1B0
	private void onToggleDebugChannel(GameEvent message)
	{
		ToggleDebugDrawChannelCommand toggleDebugDrawChannelCommand = message as ToggleDebugDrawChannelCommand;
		if (toggleDebugDrawChannelCommand.channel == DebugDrawChannel.HitBoxes)
		{
			this.toggleHitBoxCapsules(toggleDebugDrawChannelCommand.enabled);
		}
	}

	// Token: 0x06001C38 RID: 7224 RVA: 0x0008EDDC File Offset: 0x0008D1DC
	private void toggleHitBoxCapsules(bool enabled)
	{
		if (this.Model == null || this.Model.hits.Count == 0)
		{
			return;
		}
		if (enabled)
		{
			foreach (Hit hit in this.Model.hits)
			{
				foreach (HitBoxState hitBoxState in hit.hitBoxes)
				{
					CapsuleShape capsule = CapsulePool.Instance.GetCapsule(this.playerDelegate.Transform);
					capsule.Load((Vector3)hitBoxState.position, (Vector3)hitBoxState.lastPosition, (float)hitBoxState.Radius, hit.data.DebugDrawColor, hitBoxState.IsCircle);
					this.Model.hitBoxCapsules[hitBoxState] = capsule;
				}
			}
		}
		else
		{
			foreach (KeyValuePair<HitBoxState, CapsuleShape> keyValuePair in this.Model.hitBoxCapsules)
			{
				CapsuleShape value = keyValuePair.Value;
				value.Clear();
			}
			this.Model.hitBoxCapsules.Clear();
		}
	}

	// Token: 0x17000605 RID: 1541
	// (get) Token: 0x06001C39 RID: 7225 RVA: 0x0008EF74 File Offset: 0x0008D374
	public bool IsActive
	{
		get
		{
			return this.Data != null;
		}
	}

	// Token: 0x17000606 RID: 1542
	// (get) Token: 0x06001C3A RID: 7226 RVA: 0x0008EF82 File Offset: 0x0008D382
	public bool MightCollide
	{
		get
		{
			return this.IsActive && !this.Data.ignoreAllCollision;
		}
	}

	// Token: 0x17000607 RID: 1543
	// (get) Token: 0x06001C3B RID: 7227 RVA: 0x0008EFA0 File Offset: 0x0008D3A0
	public bool EmitTrail
	{
		get
		{
			foreach (MoveTrailEmitterData moveTrailEmitterData in this.Data.trailEmitters)
			{
				if (this.Model.internalFrame >= moveTrailEmitterData.startFrame && this.Model.internalFrame <= moveTrailEmitterData.endFrame)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x17000608 RID: 1544
	// (get) Token: 0x06001C3C RID: 7228 RVA: 0x0008F000 File Offset: 0x0008D400
	public bool IsLedgeGrabEnabled
	{
		get
		{
			foreach (LedgeGrabEnableData ledgeGrabEnableData in this.Data.ledgeGrabs)
			{
				if (this.Model.internalFrame >= ledgeGrabEnableData.startFrame && this.Model.internalFrame <= ledgeGrabEnableData.endFrame)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x06001C3D RID: 7229 RVA: 0x0008F060 File Offset: 0x0008D460
	public void UpdateHitboxPositions()
	{
		MoveData.UpdateHitboxPositions(this.Model.hits, this.Model.hitBoxCapsules, this.hitOwner, this.body);
	}

	// Token: 0x06001C3E RID: 7230 RVA: 0x0008F08C File Offset: 0x0008D48C
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
						IPlayerDelegate playerDelegate = this.playerDelegate;
						MoveData move = moveData;
						HorizontalDirection inputDirection = HorizontalDirection.None;
						int uid = this.Model.uid;
						int nextMoveStartupFrame = interruptData.nextMoveStartupFrame;
						List<MoveLinkComponentData> allLinkComponentData = this.GetAllLinkComponentData();
						playerDelegate.SetMove(move, input, inputDirection, uid, nextMoveStartupFrame, default(Vector3F), new MoveTransferSettings
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

	// Token: 0x06001C3F RID: 7231 RVA: 0x0008F480 File Offset: 0x0008D880
	private void onMoveChainEnd()
	{
		if (this.Model.deferredFacing != HorizontalDirection.None)
		{
			this.playerDelegate.SetFacingAndRotation(this.Model.deferredFacing);
		}
	}

	// Token: 0x06001C40 RID: 7232 RVA: 0x0008F4A8 File Offset: 0x0008D8A8
	private void incrementGameFrame()
	{
		this.Model.gameFrame++;
	}

	// Token: 0x06001C41 RID: 7233 RVA: 0x0008F4BD File Offset: 0x0008D8BD
	private void setGameFrame(int toFrame)
	{
		this.Model.gameFrame = toFrame;
		if (GameClient.IsRollingBack)
		{
			this.playerDelegate.AnimationPlayer.ChangedAnimationDuringRollback();
		}
	}

	// Token: 0x06001C42 RID: 7234 RVA: 0x0008F4E8 File Offset: 0x0008D8E8
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
				foreach (IMoveComponent moveComponent in inModel.moveComponents)
				{
					if (moveComponent is IMoveChargeComponent)
					{
						(moveComponent as IMoveChargeComponent).OnContinueCharge();
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
		foreach (IMoveComponent moveComponent2 in this.Model.moveComponents)
		{
			if (moveComponent2 is IMoveTickGameFrameComponent)
			{
				(moveComponent2 as IMoveTickGameFrameComponent).TickGameFrame(input);
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

	// Token: 0x06001C43 RID: 7235 RVA: 0x0008F964 File Offset: 0x0008DD64
	private void ApplyMaterialAnimationTriggersForSelf(MoveData move)
	{
		foreach (MaterialAnimationTrigger materialAnimationTrigger in move.materialAnimationTriggers)
		{
			if (materialAnimationTrigger.MatchesTarget(MaterialAnimationTrigger.TargetType.Attacker) && this.Model.internalFrame == materialAnimationTrigger.startFrame)
			{
				this.playerDelegate.AddHostedMaterialAnimation(materialAnimationTrigger);
			}
		}
	}

	// Token: 0x06001C44 RID: 7236 RVA: 0x0008F9C0 File Offset: 0x0008DDC0
	public bool WillReleaseGrabNextTick(MoveModel inModel)
	{
		MoveData data = inModel.data;
		int num = inModel.internalFrame + 1;
		foreach (Hit hit in inModel.hits)
		{
			if (this.config.grabConfig.useRegrabDelay || !data.hasChainGrabAlternate || hit.data.hitType != HitType.Throw || !hit.data.releaseGrabbedOpponent || this.playerDelegate.GrabData.victimUnderChainGrabPrevention == hit.data.isChainGrabThrow)
			{
				if (hit.data.startFrame == num && hit.data.hitType == HitType.Throw)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001C45 RID: 7237 RVA: 0x0008FABC File Offset: 0x0008DEBC
	private void processMoveFrame(MoveModel inModel, int previousFrame, InputButtonsData input)
	{
		MoveData data = inModel.data;
		if (data == null)
		{
			Debug.LogError("Attempted to process a null move");
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
		foreach (Hit hit in inModel.hits)
		{
			b = Mathf.Max(hit.data.endFrame, b);
			if (this.config.grabConfig.useRegrabDelay || !data.hasChainGrabAlternate || hit.data.hitType != HitType.Throw || !hit.data.releaseGrabbedOpponent || this.playerDelegate.GrabData.victimUnderChainGrabPrevention == hit.data.isChainGrabThrow)
			{
				bool flag = false;
				if (hit.data.startFrame == inModel.internalFrame)
				{
					if (hit.data.hitType == HitType.Throw)
					{
						IHitOwner hitOwner = this.hitOwner;
						PlayerController playerController = this.gameManager.GetPlayerController(this.playerDelegate.GrabData.grabbedOpponent);
						if (playerController == null)
						{
							Debug.LogWarning("Attempting to throw an opponent when no opponent was grabbed");
							break;
						}
						bool flag2 = false;
						if (hit.data.releaseGrabbedOpponent)
						{
							flag2 = true;
							this.playerDelegate.GrabController.ReleaseGrabbedOpponent(false);
						}
						Vector3F bonePosition = this.playerDelegate.Bones.GetBonePosition(BodyPart.throwBone, false);
						HitContext next = this.hitContextPool.GetNext();
						next.collisionPosition = bonePosition;
						Vector3F zero = Vector3F.zero;
						hitOwner.OnHitSuccess(hit, playerController, ImpactType.Hit, ref bonePosition, ref zero, next);
						playerController.ReceiveHit(hit.data, hitOwner, ImpactType.Hit, next);
						if (hit.data.useTeleportCorrection)
						{
							Vector3F teleportCorrection = hit.data.teleportCorrection;
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
							TimedHostedHit hit2 = new TimedHostedHit(playerController.StunFrames, this.config.knockbackConfig.throwHit, playerController, playerController.Bones, hitOwner, this.playerDelegate.HitCapsules, true);
							this.playerDelegate.AddHostedHit(hit2);
						}
					}
					bool flag3 = false;
					foreach (Hit hit3 in inModel.hits)
					{
						if (hit3 != hit)
						{
							if ((hit3.data.IsActiveOnFrame(inModel.internalFrame) || hit3.data.endFrame == inModel.internalFrame - 1) && (hit3.data.startFrame != inModel.internalFrame || flag))
							{
								flag3 = true;
								break;
							}
						}
					}
					if (!flag3)
					{
						AudioData data2 = default(AudioData);
						if (hit.data.overrideAttackSound)
						{
							data2 = hit.data.attackSound;
						}
						else
						{
							HitEffectsData effect;
							if (hit.data.hitEffectType == HitEffectType.Auto)
							{
								effect = this.config.hitConfig.GetEffect((Fixed)((double)hit.data.damage));
							}
							else
							{
								effect = this.config.hitConfig.GetEffect(hit.data.hitEffectType);
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
		foreach (MoveCameraShakeData moveCameraShakeData in data.cameraShakes)
		{
			if (moveCameraShakeData.frame == inModel.internalFrame)
			{
				this.gameManager.Camera.ShakeCamera(new CameraShakeRequest(moveCameraShakeData.shake));
			}
		}
		foreach (MoveTrailEmitterData moveTrailEmitterData in data.trailEmitters)
		{
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
		foreach (CreateArticleAction createArticleAction in data.articles)
		{
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
		foreach (MoveParticleEffect moveParticleEffect in data.particleEffects)
		{
			if (moveParticleEffect.particleEffect.HasPrefab() && this.Model.internalFrame == moveParticleEffect.frame)
			{
				GeneratedEffect generatedEffect = this.playerDelegate.GameVFX.PlayParticle(moveParticleEffect.particleEffect, this.playerDelegate.SkinData, this.playerDelegate.Team);
				if (generatedEffect != null && moveParticleEffect.particleEffect.attach)
				{
					inModel.visualData.spawnedParticles.Add(new MoveModel.EffectHandle(generatedEffect.EffectController, moveParticleEffect.cancelCondition));
				}
			}
		}
		foreach (ForceData forceData in data.forces)
		{
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
				inModel.applyForceContinuouslyEndFrame = ((!forceData.isContinuous) ? -1 : this.Data.totalInternalFrames);
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
					foreach (IMoveComponent moveComponent in this.Model.moveComponents)
					{
						if (moveComponent is IMoveForceInputDirectionReader && (moveComponent as IMoveForceInputDirectionReader).ReadInputDirection(ref vector2F4))
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
						Debug.LogWarning("Unsupported move force type " + forceData.forceType);
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
		foreach (PhysicsOverride physicsOverride in data.physicsOverrides)
		{
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
						inModel.applyPhysicsOverrideEndFrame = ((!physicsOverride.chargeScalesDuration) ? -1 : this.Data.totalInternalFrames);
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
		foreach (SoundEffect soundEffect in data.soundEffects)
		{
			if (this.Model.internalFrame == soundEffect.frame && soundEffect.sounds.Length > 0)
			{
				inModel.visualData.spawnedAudio.Add(new MoveAudioHandle(this.gameManager.Audio, soundEffect, this.Model, this.playerDelegate.AudioOwner));
			}
		}
		if (data.intangibleBodyParts.Length > 0)
		{
			foreach (IntangibleBodyParts intangibleBodyParts2 in data.intangibleBodyParts)
			{
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
			foreach (IntangibleBodyParts intangibleBodyParts3 in data.projectileInvincibility)
			{
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
		foreach (IMoveComponent moveComponent2 in this.Model.moveComponents)
		{
			if (moveComponent2 is IMoveTickMoveFrameComponent)
			{
				(moveComponent2 as IMoveTickMoveFrameComponent).TickMoveFrame(input);
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

	// Token: 0x06001C46 RID: 7238 RVA: 0x000912EC File Offset: 0x0008F6EC
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
				IPlayerDelegate playerDelegate = this.playerDelegate;
				MoveData move = moveData;
				HorizontalDirection inputDirection = HorizontalDirection.None;
				int uid = inModel.uid;
				int nextMoveStartupFrame = interruptData.nextMoveStartupFrame;
				List<MoveLinkComponentData> allLinkComponentData = this.GetAllLinkComponentData();
				playerDelegate.SetMove(move, input, inputDirection, uid, nextMoveStartupFrame, default(Vector3F), new MoveTransferSettings
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

	// Token: 0x06001C47 RID: 7239 RVA: 0x00091438 File Offset: 0x0008F838
	private Fixed getChargeFractionOverride()
	{
		IMoveOverrideChargeComponent component = this.Model.GetComponent<IMoveOverrideChargeComponent>();
		if (component == null)
		{
			return -1;
		}
		return component.OverrideChargeFraction;
	}

	// Token: 0x06001C48 RID: 7240 RVA: 0x00091464 File Offset: 0x0008F864
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

	// Token: 0x06001C49 RID: 7241 RVA: 0x000914B0 File Offset: 0x0008F8B0
	public List<MoveLinkComponentData> GetAllLinkComponentData()
	{
		this.linkComponentBuffer.Clear();
		foreach (IMoveComponent moveComponent in this.Model.moveComponents)
		{
			if (moveComponent is IMoveLinkComponent)
			{
				this.linkComponentBuffer.Add((moveComponent as IMoveLinkComponent).GetLinkComponentData());
			}
		}
		return this.linkComponentBuffer;
	}

	// Token: 0x06001C4A RID: 7242 RVA: 0x0009153C File Offset: 0x0008F93C
	public bool onParticleCreated(ParticleData particle, GameObject gameObject)
	{
		foreach (IMoveComponent moveComponent in this.Model.moveComponents)
		{
			if (moveComponent is ITaggedParticleListener)
			{
				(moveComponent as ITaggedParticleListener).OnCreateTaggedParticle(particle.tag, gameObject);
			}
		}
		return true;
	}

	// Token: 0x06001C4B RID: 7243 RVA: 0x000915B4 File Offset: 0x0008F9B4
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

	// Token: 0x06001C4C RID: 7244 RVA: 0x00091633 File Offset: 0x0008FA33
	private XWeaponTrail createWeaponTrailFunc()
	{
		return this.gameManager.DynamicObjects.InstantiateDynamicObject<XWeaponTrail>(this.config.moveData.WeaponTrailPrefab, 4, true);
	}

	// Token: 0x06001C4D RID: 7245 RVA: 0x00091658 File Offset: 0x0008FA58
	private void startChargedMove()
	{
		this.Model.isCharging = true;
		foreach (IMoveComponent moveComponent in this.Model.moveComponents)
		{
			if (moveComponent is IMoveChargeComponent)
			{
				(moveComponent as IMoveChargeComponent).OnStartCharge();
			}
		}
	}

	// Token: 0x06001C4E RID: 7246 RVA: 0x000916D4 File Offset: 0x0008FAD4
	private void syncMoveToFrame(int frame)
	{
		this.setGameFrame(this.animationPlayer.GetNextGameFrameFromAnimationFrame(this.animationPlayer.CurrentAnimationData.clipName, frame) - 1);
		this.Model.internalFrame = frame;
	}

	// Token: 0x06001C4F RID: 7247 RVA: 0x0009170C File Offset: 0x0008FB0C
	private void releaseChargedMove(MoveData move)
	{
		this.syncMoveToFrame(move.chargeOptions.chargeReleaseFrame);
		this.playerDelegate.Renderer.SetColorModeFlag(ColorMode.Charging, false);
		this.Model.isCharging = false;
		int num = 0;
		foreach (IMoveComponent moveComponent in this.Model.moveComponents)
		{
			if (moveComponent is IMoveChargeComponent)
			{
				IMoveChargeComponent moveChargeComponent = moveComponent as IMoveChargeComponent;
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

	// Token: 0x06001C50 RID: 7248 RVA: 0x000917DC File Offset: 0x0008FBDC
	private void fireChargedMove(MoveData move)
	{
		foreach (IMoveComponent moveComponent in this.Model.moveComponents)
		{
			if (moveComponent is IMoveChargeComponent)
			{
				(moveComponent as IMoveChargeComponent).OnFireCharge();
			}
		}
		this.Model.chargeFireDelay = 0;
	}

	// Token: 0x170005F7 RID: 1527
	// (get) Token: 0x06001C51 RID: 7249 RVA: 0x00091858 File Offset: 0x0008FC58
	int IMoveDelegate.TotalFrames
	{
		get
		{
			return this.Data.totalInternalFrames;
		}
	}

	// Token: 0x06001C52 RID: 7250 RVA: 0x00091868 File Offset: 0x0008FC68
	public bool LoadState(RollbackStateContainer container)
	{
		List<IMoveComponent> moveComponents = this.Model.moveComponents;
		container.ReadState<MoveModel>(ref this.model);
		foreach (IMoveComponent moveComponent in moveComponents)
		{
			if (!this.Model.moveComponents.Contains(moveComponent) && moveComponent is IMoveEndComponent)
			{
				(moveComponent as IMoveEndComponent).OnEnd();
			}
		}
		foreach (IMoveComponent moveComponent2 in this.Model.moveComponents)
		{
			if (moveComponent2 is IRollbackStateOwner)
			{
				((IRollbackStateOwner)moveComponent2).LoadState(container);
			}
		}
		foreach (KeyValuePair<WeaponTrailData, XWeaponTrail> keyValuePair in this.Model.visualData.weaponTrailMap)
		{
			keyValuePair.Value.LoadState(container);
		}
		return true;
	}

	// Token: 0x06001C53 RID: 7251 RVA: 0x000919C0 File Offset: 0x0008FDC0
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<MoveModel>(this.Model));
		foreach (IMoveComponent moveComponent in this.Model.moveComponents)
		{
			if (moveComponent is IRollbackStateOwner)
			{
				((IRollbackStateOwner)moveComponent).ExportState(ref container);
			}
		}
		foreach (KeyValuePair<WeaponTrailData, XWeaponTrail> keyValuePair in this.Model.visualData.weaponTrailMap)
		{
			XWeaponTrail xweaponTrail = this.Model.visualData.weaponTrailMap[keyValuePair.Key];
			xweaponTrail.ExportState(ref container);
		}
		return true;
	}

	// Token: 0x06001C54 RID: 7252 RVA: 0x00091AC4 File Offset: 0x0008FEC4
	private void onEffectReleased(Effect theEffect)
	{
		this.Model.visualData.spawnedParticles.RemoveFirst((MoveModel.EffectHandle item) => item.Effect == theEffect);
	}

	// Token: 0x06001C55 RID: 7253 RVA: 0x00091B00 File Offset: 0x0008FF00
	public void Reset(MoveEndType moveEndType, bool transitioningToContinuingMove)
	{
		if (this.IsActive)
		{
			foreach (IMoveComponent moveComponent in this.Model.moveComponents)
			{
				if (moveComponent is IMoveEndComponent)
				{
					(moveComponent as IMoveEndComponent).OnEnd();
				}
			}
		}
		this.clearFinishedParticles(moveEndType, transitioningToContinuingMove);
		this.clearFinishedAudio(moveEndType, transitioningToContinuingMove);
		this.Model.Clear();
	}

	// Token: 0x06001C56 RID: 7254 RVA: 0x00091B98 File Offset: 0x0008FF98
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

	// Token: 0x06001C57 RID: 7255 RVA: 0x00091C28 File Offset: 0x00090028
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

	// Token: 0x06001C58 RID: 7256 RVA: 0x00091CA0 File Offset: 0x000900A0
	private void clearAllParticles()
	{
		this.weaponTrailHelper.ClearWeaponTrails(this.model.visualData.weaponTrailMap);
		for (int i = this.Model.visualData.spawnedParticles.Count - 1; i >= 0; i--)
		{
			this.Model.visualData.spawnedParticles[i].Effect.Destroy();
		}
		this.Model.visualData.spawnedParticles.Clear();
	}

	// Token: 0x06001C59 RID: 7257 RVA: 0x00091D28 File Offset: 0x00090128
	private void clearAllAudio()
	{
		for (int i = this.Model.visualData.spawnedAudio.Count - 1; i >= 0; i--)
		{
			this.Model.visualData.spawnedAudio[i].Stop();
		}
		this.Model.visualData.spawnedAudio.Clear();
	}

	// Token: 0x06001C5A RID: 7258 RVA: 0x00091D90 File Offset: 0x00090190
	public void Destroy()
	{
		this.clearAllParticles();
		this.clearAllAudio();
		this.Model.Clear();
		this.events.Unsubscribe(typeof(ToggleDebugDrawChannelCommand), new Events.EventHandler(this.onToggleDebugChannel));
		this.signalBus.GetSignal<EffectReleaseSignal>().RemoveListener(new Action<Effect>(this.onEffectReleased));
	}

	// Token: 0x06001C5B RID: 7259 RVA: 0x00091DF4 File Offset: 0x000901F4
	private void GetWaveDashParticle(ref ParticleData landParticle)
	{
		if (this.Data.label == MoveLabel.AirDodge)
		{
			bool flag = false;
			foreach (PhysicsOverride physicsOverride in this.Data.physicsOverrides)
			{
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

	// Token: 0x06001C5C RID: 7260 RVA: 0x00091F30 File Offset: 0x00090330
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
				IPlayerDelegate playerDelegate = this.playerDelegate;
				MoveData landMove = landCancelOptions.landMove;
				InputButtonsData currentInputData = inputProcessor.CurrentInputData;
				HorizontalDirection inputDirection = HorizontalDirection.None;
				int uid = this.Model.uid;
				int startFrame = num;
				List<MoveLinkComponentData> allLinkComponentData = this.GetAllLinkComponentData();
				playerDelegate.SetMove(landMove, currentInputData, inputDirection, uid, startFrame, default(Vector3F), new MoveTransferSettings
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

	// Token: 0x06001C5D RID: 7261 RVA: 0x000922C4 File Offset: 0x000906C4
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
			IPlayerDelegate playerDelegate = this.playerDelegate;
			MoveData fallMove = fallCancelOptions.fallMove;
			InputButtonsData currentInputData = inputProcessor.CurrentInputData;
			HorizontalDirection inputDirection = HorizontalDirection.None;
			int uid = this.Model.uid;
			int startFrame = num;
			List<MoveLinkComponentData> allLinkComponentData = this.GetAllLinkComponentData();
			playerDelegate.SetMove(fallMove, currentInputData, inputDirection, uid, startFrame, default(Vector3F), new MoveTransferSettings
			{
				transitioningToContinuingMove = fallCancelOptions.startFallAtCurrentFrame,
				transferHitDisableTargets = fallCancelOptions.transferHitDisableTargets
			}, allLinkComponentData, default(MoveSeedData), this.Model.chargeButton);
			return true;
		}
		return false;
	}

	// Token: 0x17000609 RID: 1545
	// (get) Token: 0x06001C5E RID: 7262 RVA: 0x0009242C File Offset: 0x0009082C
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

	// Token: 0x06001C5F RID: 7263 RVA: 0x00092498 File Offset: 0x00090898
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

	// Token: 0x06001C60 RID: 7264 RVA: 0x00092510 File Offset: 0x00090910
	public bool DoesArmorResistHit(Fixed knockbackForce)
	{
		return this.IsActive && this.Data.armorOptions != null && this.Data.armorOptions.hasArmor && this.Model.internalFrame >= this.Data.armorOptions.startFrame && this.Model.internalFrame <= this.Data.armorOptions.endFrame && knockbackForce <= this.Data.armorOptions.knockbackThreshold;
	}

	// Token: 0x04001746 RID: 5958
	private MoveModel model;

	// Token: 0x04001747 RID: 5959
	private IPlayerDelegate playerDelegate;

	// Token: 0x04001748 RID: 5960
	private IHitOwner hitOwner;

	// Token: 0x04001749 RID: 5961
	private IAnimationPlayer animationPlayer;

	// Token: 0x0400174A RID: 5962
	private IBodyOwner body;

	// Token: 0x0400174B RID: 5963
	private IMovePhysics physics;

	// Token: 0x0400174C RID: 5964
	private GameManager gameManager;

	// Token: 0x0400174D RID: 5965
	private IEvents events;

	// Token: 0x0400174E RID: 5966
	private Func<XWeaponTrail> createWeaponTrail;

	// Token: 0x0400174F RID: 5967
	private List<MoveLinkComponentData> linkComponentBuffer = new List<MoveLinkComponentData>();

	// Token: 0x0200051F RID: 1311
	// (Invoke) Token: 0x06001C63 RID: 7267
	public delegate bool ComponentExecution<T>(T component);
}
