using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x020005C8 RID: 1480
public class FloatCharacterComponent : CharacterComponent, IRollbackStateOwner, ICharacterPhysicsOverrideComponent, ITickCharacterComponent, IDeathListener, ILandListener, IJumpListener, IDropListener, IFlinchListener, IGrabListener, IMoveUsedListener, IMoveKillListener, IWallJumpBlocker, IFallThroughPlatformBlocker, IJumpBlocker, ICharacterAnimationComponent, IRotationCharacterComponent, IMoveBlockerComponent, ILedgeGrabBlocker
{
	// Token: 0x17000744 RID: 1860
	// (get) Token: 0x060020EA RID: 8426 RVA: 0x000A4F33 File Offset: 0x000A3333
	// (set) Token: 0x060020EB RID: 8427 RVA: 0x000A4F3B File Offset: 0x000A333B
	[Inject]
	public IRollbackStatePooling pooling { get; set; }

	// Token: 0x17000745 RID: 1861
	// (get) Token: 0x060020EC RID: 8428 RVA: 0x000A4F44 File Offset: 0x000A3344
	// (set) Token: 0x060020ED RID: 8429 RVA: 0x000A4F4C File Offset: 0x000A334C
	[Inject]
	public IPhysicsCalculator physicsCalculator { get; set; }

	// Token: 0x17000746 RID: 1862
	// (get) Token: 0x060020EE RID: 8430 RVA: 0x000A4F55 File Offset: 0x000A3355
	public bool IsActive
	{
		get
		{
			return this.model.ticksRemaining > 0 || this.isAerialLockActive;
		}
	}

	// Token: 0x17000747 RID: 1863
	// (get) Token: 0x060020EF RID: 8431 RVA: 0x000A4F71 File Offset: 0x000A3371
	public bool AllowFastFall
	{
		get
		{
			return !this.IsActive;
		}
	}

	// Token: 0x17000748 RID: 1864
	// (get) Token: 0x060020F0 RID: 8432 RVA: 0x000A4F7C File Offset: 0x000A337C
	public CharacterPhysicsOverride Override
	{
		get
		{
			if (this.IsActive)
			{
				return this.configData.physicsData;
			}
			return null;
		}
	}

	// Token: 0x17000749 RID: 1865
	// (get) Token: 0x060020F1 RID: 8433 RVA: 0x000A4F96 File Offset: 0x000A3396
	public string AnimationSuffix
	{
		get
		{
			return "Float";
		}
	}

	// Token: 0x1700074A RID: 1866
	// (get) Token: 0x060020F2 RID: 8434 RVA: 0x000A4F9D File Offset: 0x000A339D
	public bool CanJump
	{
		get
		{
			return !this.IsActive || this.configData.allowJumpCancel;
		}
	}

	// Token: 0x1700074B RID: 1867
	// (get) Token: 0x060020F3 RID: 8435 RVA: 0x000A4FB8 File Offset: 0x000A33B8
	public bool CanWallJump
	{
		get
		{
			return !this.IsActive;
		}
	}

	// Token: 0x1700074C RID: 1868
	// (get) Token: 0x060020F4 RID: 8436 RVA: 0x000A4FC3 File Offset: 0x000A33C3
	public bool CanFallThroughPlatform
	{
		get
		{
			return !this.IsActive || this.configData.allowFallThroughPlatform;
		}
	}

	// Token: 0x1700074D RID: 1869
	// (get) Token: 0x060020F5 RID: 8437 RVA: 0x000A4FDE File Offset: 0x000A33DE
	public bool PreventActionStateAnimations
	{
		get
		{
			return this.IsActive;
		}
	}

	// Token: 0x1700074E RID: 1870
	// (get) Token: 0x060020F6 RID: 8438 RVA: 0x000A4FE6 File Offset: 0x000A33E6
	public bool IsRotationRolled
	{
		get
		{
			return this.IsActive;
		}
	}

	// Token: 0x1700074F RID: 1871
	// (get) Token: 0x060020F7 RID: 8439 RVA: 0x000A4FEE File Offset: 0x000A33EE
	public Fixed Roll
	{
		get
		{
			return this.model.rollAngle;
		}
	}

	// Token: 0x17000750 RID: 1872
	// (get) Token: 0x060020F8 RID: 8440 RVA: 0x000A4FFB File Offset: 0x000A33FB
	public bool IsLedgeGrabbingBlocked
	{
		get
		{
			return this.IsActive;
		}
	}

	// Token: 0x17000751 RID: 1873
	// (get) Token: 0x060020F9 RID: 8441 RVA: 0x000A5003 File Offset: 0x000A3403
	private bool isAerialLockActive
	{
		get
		{
			return this.configData.deferEndUntilAerialMoveComplete && this.model.aerialMoveLock;
		}
	}

	// Token: 0x060020FA RID: 8442 RVA: 0x000A5024 File Offset: 0x000A3424
	public override void Init(IPlayerDelegate playerDelegate)
	{
		base.Init(playerDelegate);
		foreach (MoveLabel item in this.configData.moveWhitelist)
		{
			this.whitelistedMoves.Add(item);
		}
	}

	// Token: 0x060020FB RID: 8443 RVA: 0x000A506C File Offset: 0x000A346C
	public void BeginFloat(int duration)
	{
		int frame = base.gameController.currentGame.Frame;
		this.model.ticksRemaining = duration - 1;
		this.model.floatDirection = RelativeDirection.Forward;
		this.model.verticalSpeed = 0;
		if (this.configData.loopAudio.sound != null)
		{
			this.loopingSoundId = this.playerDelegate.Audio.PlayLoopingSound(new AudioRequest(this.configData.loopAudio, this.playerDelegate.AudioOwner, null));
		}
		IGameVFX gameVFX = this.playerDelegate.GameVFX;
		List<ParticleData> loopParticles = this.configData.loopParticles;
		List<Effect> activeEffects = this.model.activeEffects;
		gameVFX.PlayParticleList(loopParticles, null, activeEffects);
		this.playFloatLoopAnimation();
	}

	// Token: 0x060020FC RID: 8444 RVA: 0x000A5134 File Offset: 0x000A3534
	private void endFloat()
	{
		this.model.ticksRemaining = 0;
		this.model.aerialMoveLock = false;
		this.model.floatDirection = RelativeDirection.Up;
		this.model.floatTransitionFrames = 0;
		this.playerDelegate.RestartCurrentActionState(false);
		this.cleanup(true);
	}

	// Token: 0x060020FD RID: 8445 RVA: 0x000A5184 File Offset: 0x000A3584
	private void stopLoopingSound()
	{
		if (this.loopingSoundId.sourceId > -1)
		{
			this.playerDelegate.Audio.StopSound(this.loopingSoundId, 0f);
		}
	}

	// Token: 0x060020FE RID: 8446 RVA: 0x000A51B4 File Offset: 0x000A35B4
	private void cleanup(bool killparticles)
	{
		this.stopLoopingSound();
		if (killparticles)
		{
			foreach (Effect effect in this.model.activeEffects)
			{
				effect.EnterSoftKill();
			}
			this.model.activeEffects.Clear();
		}
	}

	// Token: 0x060020FF RID: 8447 RVA: 0x000A5234 File Offset: 0x000A3634
	public void TryEndFloat()
	{
		if (this.IsActive)
		{
			this.endFloat();
		}
	}

	// Token: 0x06002100 RID: 8448 RVA: 0x000A5247 File Offset: 0x000A3647
	public void OnDeath()
	{
		this.TryEndFloat();
	}

	// Token: 0x06002101 RID: 8449 RVA: 0x000A524F File Offset: 0x000A364F
	public void OnLand()
	{
		this.TryEndFloat();
	}

	// Token: 0x06002102 RID: 8450 RVA: 0x000A5257 File Offset: 0x000A3657
	public void OnDrop()
	{
		if (this.configData.allowFastFallCancel)
		{
			this.TryEndFloat();
		}
	}

	// Token: 0x06002103 RID: 8451 RVA: 0x000A526F File Offset: 0x000A366F
	public void OnJump()
	{
		if (this.configData.allowJumpCancel)
		{
			this.TryEndFloat();
		}
	}

	// Token: 0x06002104 RID: 8452 RVA: 0x000A5287 File Offset: 0x000A3687
	public void OnFlinch()
	{
		this.TryEndFloat();
	}

	// Token: 0x06002105 RID: 8453 RVA: 0x000A528F File Offset: 0x000A368F
	public void OnGrabbed()
	{
		this.TryEndFloat();
	}

	// Token: 0x06002106 RID: 8454 RVA: 0x000A5297 File Offset: 0x000A3697
	public void OnMoveUsed(MoveData moveData)
	{
		if (!this.whitelistedMoves.Contains(moveData.label))
		{
			this.TryEndFloat();
		}
		else if (this.IsActive)
		{
			this.model.aerialMoveLock = true;
		}
	}

	// Token: 0x06002107 RID: 8455 RVA: 0x000A52D1 File Offset: 0x000A36D1
	public void OnMoveKilled(MoveData moveData)
	{
		this.model.aerialMoveLock = false;
		this.playFloatLoopAnimation();
	}

	// Token: 0x06002108 RID: 8456 RVA: 0x000A52E8 File Offset: 0x000A36E8
	private bool isSideSpecialInput(InputButtonsData input)
	{
		Fixed one = 0;
		if (input.horizontalAxisValue == 0)
		{
			return false;
		}
		one = FixedMath.Abs(input.verticalAxisValue / input.horizontalAxisValue);
		for (int i = 0; i < input.buttonsHeld.Count; i++)
		{
			ButtonPress buttonPress = input.buttonsHeld[i];
			if (InputUtils.IsHorizontalLeftStick(buttonPress) && one < MoveSet.RECOVERY_Y_TO_X_RATIO)
			{
				return this.playerDelegate.InputController.GetButtonDown(ButtonPress.Special);
			}
		}
		return false;
	}

	// Token: 0x06002109 RID: 8457 RVA: 0x000A5380 File Offset: 0x000A3780
	public void TickFrame(InputButtonsData input)
	{
		if (this.IsActive)
		{
			if (this.configData.allowSpecialInputCancel && ((this.model.aerialMoveLock && this.playerDelegate.InputController.GetButtonDown(ButtonPress.Special)) || this.isSideSpecialInput(input)))
			{
				this.TryEndFloat();
				return;
			}
			Fixed axis = this.playerDelegate.InputController.GetAxis(this.playerDelegate.InputController.horizontalAxis);
			Fixed axis2 = this.playerDelegate.InputController.GetAxis(this.playerDelegate.InputController.verticalAxis);
			if (this.model.verticalSpeed > 0)
			{
				this.model.verticalSpeed = FixedMath.Max(this.model.verticalSpeed - this.configData.verticalFriction * WTime.fixedDeltaTime, 0);
			}
			else if (this.model.verticalSpeed < 0)
			{
				this.model.verticalSpeed = FixedMath.Min(this.model.verticalSpeed + this.configData.verticalFriction * WTime.fixedDeltaTime, 0);
			}
			Fixed other = axis2 * this.configData.verticalAcceleration * WTime.fixedDeltaTime;
			this.model.verticalSpeed += other;
			this.model.verticalSpeed = FixedMath.Clamp(this.model.verticalSpeed, -this.configData.verticalMaxSpeed, this.configData.verticalMaxSpeed);
			this.playerDelegate.Physics.State.SetVelocity(Vector3F.up * this.model.verticalSpeed, VelocityType.Forced);
			RelativeDirection floatDirection = this.getFloatDirection(this.playerDelegate.Facing, axis);
			this.setFloatDirection(floatDirection);
			this.model.rollAngle = -axis2 * this.configData.maxRollAngle;
			if (floatDirection == RelativeDirection.Backward)
			{
				this.model.rollAngle *= -1;
			}
			if (this.model.floatTransitionFrames > 0)
			{
				this.model.floatTransitionFrames--;
				if (this.model.floatTransitionFrames <= 0)
				{
					this.playFloatLoopAnimation();
				}
			}
			if (!this.playerDelegate.State.IsHitLagPaused)
			{
				this.model.ticksRemaining--;
				if (this.model.ticksRemaining <= 0)
				{
					this.stopLoopingSound();
					if (!this.isAerialLockActive)
					{
						this.endFloat();
					}
				}
			}
		}
		else
		{
			this.cleanup(true);
			this.playerDelegate.Physics.State.SetVelocity(Vector3F.zero, VelocityType.Forced);
		}
	}

	// Token: 0x0600210A RID: 8458 RVA: 0x000A566C File Offset: 0x000A3A6C
	private RelativeDirection getFloatDirection(HorizontalDirection facing, Fixed horizontalAxis)
	{
		if (facing == HorizontalDirection.Left || facing != HorizontalDirection.Right)
		{
			if (horizontalAxis < 0)
			{
				return RelativeDirection.Forward;
			}
			if (horizontalAxis > 0)
			{
				return RelativeDirection.Backward;
			}
			return this.model.floatDirection;
		}
		else
		{
			if (horizontalAxis < 0)
			{
				return RelativeDirection.Backward;
			}
			if (horizontalAxis > 0)
			{
				return RelativeDirection.Forward;
			}
			return this.model.floatDirection;
		}
	}

	// Token: 0x0600210B RID: 8459 RVA: 0x000A56DC File Offset: 0x000A3ADC
	private void setFloatDirection(RelativeDirection direction)
	{
		if (this.model.floatDirection == RelativeDirection.Up)
		{
			this.model.floatDirection = direction;
			this.playFloatLoopAnimation();
		}
		else if (this.model.floatDirection != direction)
		{
			this.model.floatDirection = direction;
			CharacterAnimation transitionToAnim = this.getTransitionToAnim(this.model.floatDirection);
			int floatTransitionFrames = this.model.floatTransitionFrames;
			if (!this.model.aerialMoveLock)
			{
				this.playAnimation(transitionToAnim.AnimationName, floatTransitionFrames);
			}
			this.model.floatTransitionFrames = transitionToAnim.frameDuration - floatTransitionFrames;
		}
	}

	// Token: 0x0600210C RID: 8460 RVA: 0x000A577B File Offset: 0x000A3B7B
	private void playAnimation(string animName, int animFrame)
	{
		this.playerDelegate.AnimationPlayer.PlayAnimation(animName, false, animFrame, -1f, -1f);
		this.playerDelegate.RestartCurrentActionState(false);
	}

	// Token: 0x0600210D RID: 8461 RVA: 0x000A57A7 File Offset: 0x000A3BA7
	private CharacterAnimation getTransitionToAnim(RelativeDirection transitionTo)
	{
		if (transitionTo == RelativeDirection.Forward || transitionTo != RelativeDirection.Backward)
		{
			return this.configData.floatBackToForwardAnim;
		}
		return this.configData.floatForwardToBackAnim;
	}

	// Token: 0x0600210E RID: 8462 RVA: 0x000A57D4 File Offset: 0x000A3BD4
	private void playFloatLoopAnimation()
	{
		if (!this.model.aerialMoveLock && this.model.floatTransitionFrames <= 0)
		{
			if (this.model.floatDirection == RelativeDirection.Forward)
			{
				this.playAnimation(this.configData.floatForwardAnim.AnimationName, 0);
			}
			else if (this.model.floatDirection == RelativeDirection.Backward)
			{
				this.playAnimation(this.configData.floatBackAnim.AnimationName, 0);
			}
		}
	}

	// Token: 0x0600210F RID: 8463 RVA: 0x000A5857 File Offset: 0x000A3C57
	public CharacterAnimation[] GetCharacterAnimations()
	{
		return new CharacterAnimation[]
		{
			this.configData.floatForwardAnim,
			this.configData.floatBackAnim,
			this.configData.floatForwardToBackAnim,
			this.configData.floatBackToForwardAnim
		};
	}

	// Token: 0x06002110 RID: 8464 RVA: 0x000A5897 File Offset: 0x000A3C97
	public bool IsOverridingActionStateAnimation(ActionState actionState, HorizontalDirection facing, ref string animationName)
	{
		return false;
	}

	// Token: 0x06002111 RID: 8465 RVA: 0x000A589A File Offset: 0x000A3C9A
	public override void Destroy()
	{
		this.cleanup(false);
		base.Destroy();
	}

	// Token: 0x06002112 RID: 8466 RVA: 0x000A58AC File Offset: 0x000A3CAC
	public bool IsMoveBlocked(MoveData moveData)
	{
		return this.configData.allowSpecialInputCancel && this.IsActive && ((FloatCharacterComponent.specialMoveLabels.Contains(moveData.label) && this.model.aerialMoveLock) || moveData.label == MoveLabel.SideSpecial);
	}

	// Token: 0x06002113 RID: 8467 RVA: 0x000A5909 File Offset: 0x000A3D09
	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.pooling.Clone<FloatCharacterComponent.FloatCharacterComponentModel>(this.model));
	}

	// Token: 0x06002114 RID: 8468 RVA: 0x000A5923 File Offset: 0x000A3D23
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<FloatCharacterComponent.FloatCharacterComponentModel>(ref this.model);
		return true;
	}

	// Token: 0x06002115 RID: 8469 RVA: 0x000A5934 File Offset: 0x000A3D34
	public override void RegisterPreload(PreloadContext context)
	{
		foreach (ParticleData particleData in this.configData.loopParticles)
		{
			particleData.RegisterPreload(context);
		}
	}

	// Token: 0x04001A1F RID: 6687
	public FloatCharacterComponentData configData;

	// Token: 0x04001A20 RID: 6688
	private static readonly HashSet<MoveLabel> specialMoveLabels = new HashSet<MoveLabel>(default(MoveLabelComparer))
	{
		MoveLabel.NeutralSpecial,
		MoveLabel.DownSpecial,
		MoveLabel.SideSpecial,
		MoveLabel.UpSpecial
	};

	// Token: 0x04001A21 RID: 6689
	private HashSet<MoveLabel> whitelistedMoves = new HashSet<MoveLabel>();

	// Token: 0x04001A22 RID: 6690
	private FloatCharacterComponent.FloatCharacterComponentModel model = new FloatCharacterComponent.FloatCharacterComponentModel();

	// Token: 0x04001A23 RID: 6691
	private AudioReference loopingSoundId = new AudioReference(null, -1);

	// Token: 0x020005C9 RID: 1481
	[Serializable]
	public class FloatCharacterComponentModel : RollbackStateTyped<FloatCharacterComponent.FloatCharacterComponentModel>
	{
		// Token: 0x06002118 RID: 8472 RVA: 0x000A5A10 File Offset: 0x000A3E10
		public override void CopyTo(FloatCharacterComponent.FloatCharacterComponentModel target)
		{
			target.ticksRemaining = this.ticksRemaining;
			target.aerialMoveLock = this.aerialMoveLock;
			target.floatDirection = this.floatDirection;
			target.floatTransitionFrames = this.floatTransitionFrames;
			target.rollAngle = this.rollAngle;
			target.verticalSpeed = this.verticalSpeed;
			base.copyList<Effect>(this.activeEffects, target.activeEffects);
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x000A5A78 File Offset: 0x000A3E78
		public override object Clone()
		{
			FloatCharacterComponent.FloatCharacterComponentModel floatCharacterComponentModel = new FloatCharacterComponent.FloatCharacterComponentModel();
			this.CopyTo(floatCharacterComponentModel);
			return floatCharacterComponentModel;
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x000A5A93 File Offset: 0x000A3E93
		public override void Clear()
		{
			base.Clear();
			this.activeEffects.Clear();
		}

		// Token: 0x04001A24 RID: 6692
		public int ticksRemaining;

		// Token: 0x04001A25 RID: 6693
		public bool aerialMoveLock;

		// Token: 0x04001A26 RID: 6694
		public RelativeDirection floatDirection;

		// Token: 0x04001A27 RID: 6695
		public int floatTransitionFrames;

		// Token: 0x04001A28 RID: 6696
		public Fixed rollAngle = 0;

		// Token: 0x04001A29 RID: 6697
		public Fixed verticalSpeed = 0;

		// Token: 0x04001A2A RID: 6698
		[IgnoreCopyValidation]
		[IsClonedManually]
		[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
		[NonSerialized]
		public List<Effect> activeEffects = new List<Effect>(16);
	}
}
