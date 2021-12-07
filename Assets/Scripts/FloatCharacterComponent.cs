// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

public class FloatCharacterComponent : CharacterComponent, IRollbackStateOwner, ICharacterPhysicsOverrideComponent, ITickCharacterComponent, IDeathListener, ILandListener, IJumpListener, IDropListener, IFlinchListener, IGrabListener, IMoveUsedListener, IMoveKillListener, IWallJumpBlocker, IFallThroughPlatformBlocker, IJumpBlocker, ICharacterAnimationComponent, IRotationCharacterComponent, IMoveBlockerComponent, ILedgeGrabBlocker
{
	[Serializable]
	public class FloatCharacterComponentModel : RollbackStateTyped<FloatCharacterComponent.FloatCharacterComponentModel>
	{
		public int ticksRemaining;

		public bool aerialMoveLock;

		public RelativeDirection floatDirection;

		public int floatTransitionFrames;

		public Fixed rollAngle = 0;

		public Fixed verticalSpeed = 0;

		[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy), IsClonedManually]
		[NonSerialized]
		public List<Effect> activeEffects = new List<Effect>(16);

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

		public override object Clone()
		{
			FloatCharacterComponent.FloatCharacterComponentModel floatCharacterComponentModel = new FloatCharacterComponent.FloatCharacterComponentModel();
			this.CopyTo(floatCharacterComponentModel);
			return floatCharacterComponentModel;
		}

		public override void Clear()
		{
			base.Clear();
			this.activeEffects.Clear();
		}
	}

	public FloatCharacterComponentData configData;

	private static readonly HashSet<MoveLabel> specialMoveLabels = new HashSet<MoveLabel>(default(MoveLabelComparer))
	{
		MoveLabel.NeutralSpecial,
		MoveLabel.DownSpecial,
		MoveLabel.SideSpecial,
		MoveLabel.UpSpecial
	};

	private HashSet<MoveLabel> whitelistedMoves = new HashSet<MoveLabel>();

	private FloatCharacterComponent.FloatCharacterComponentModel model = new FloatCharacterComponent.FloatCharacterComponentModel();

	private AudioReference loopingSoundId = new AudioReference(null, -1);

	[Inject]
	public IRollbackStatePooling pooling
	{
		get;
		set;
	}

	[Inject]
	public IPhysicsCalculator physicsCalculator
	{
		get;
		set;
	}

	public bool IsActive
	{
		get
		{
			return this.model.ticksRemaining > 0 || this.isAerialLockActive;
		}
	}

	public bool AllowFastFall
	{
		get
		{
			return !this.IsActive;
		}
	}

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

	public string AnimationSuffix
	{
		get
		{
			return "Float";
		}
	}

	public bool CanJump
	{
		get
		{
			return !this.IsActive || this.configData.allowJumpCancel;
		}
	}

	public bool CanWallJump
	{
		get
		{
			return !this.IsActive;
		}
	}

	public bool CanFallThroughPlatform
	{
		get
		{
			return !this.IsActive || this.configData.allowFallThroughPlatform;
		}
	}

	public bool PreventActionStateAnimations
	{
		get
		{
			return this.IsActive;
		}
	}

	public bool IsRotationRolled
	{
		get
		{
			return this.IsActive;
		}
	}

	public Fixed Roll
	{
		get
		{
			return this.model.rollAngle;
		}
	}

	public bool IsLedgeGrabbingBlocked
	{
		get
		{
			return this.IsActive;
		}
	}

	private bool isAerialLockActive
	{
		get
		{
			return this.configData.deferEndUntilAerialMoveComplete && this.model.aerialMoveLock;
		}
	}

	public override void Init(IPlayerDelegate playerDelegate)
	{
		base.Init(playerDelegate);
		MoveLabel[] moveWhitelist = this.configData.moveWhitelist;
		for (int i = 0; i < moveWhitelist.Length; i++)
		{
			MoveLabel item = moveWhitelist[i];
			this.whitelistedMoves.Add(item);
		}
	}

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
		IGameVFX arg_AF_0 = this.playerDelegate.GameVFX;
		List<ParticleData> loopParticles = this.configData.loopParticles;
		List<Effect> activeEffects = this.model.activeEffects;
		arg_AF_0.PlayParticleList(loopParticles, null, activeEffects);
		this.playFloatLoopAnimation();
	}

	private void endFloat()
	{
		this.model.ticksRemaining = 0;
		this.model.aerialMoveLock = false;
		this.model.floatDirection = RelativeDirection.Up;
		this.model.floatTransitionFrames = 0;
		this.playerDelegate.RestartCurrentActionState(false);
		this.cleanup(true);
	}

	private void stopLoopingSound()
	{
		if (this.loopingSoundId.sourceId > -1)
		{
			this.playerDelegate.Audio.StopSound(this.loopingSoundId, 0f);
		}
	}

	private void cleanup(bool killparticles)
	{
		this.stopLoopingSound();
		if (killparticles)
		{
			foreach (Effect current in this.model.activeEffects)
			{
				current.EnterSoftKill();
			}
			this.model.activeEffects.Clear();
		}
	}

	public void TryEndFloat()
	{
		if (this.IsActive)
		{
			this.endFloat();
		}
	}

	public void OnDeath()
	{
		this.TryEndFloat();
	}

	public void OnLand()
	{
		this.TryEndFloat();
	}

	public void OnDrop()
	{
		if (this.configData.allowFastFallCancel)
		{
			this.TryEndFloat();
		}
	}

	public void OnJump()
	{
		if (this.configData.allowJumpCancel)
		{
			this.TryEndFloat();
		}
	}

	public void OnFlinch()
	{
		this.TryEndFloat();
	}

	public void OnGrabbed()
	{
		this.TryEndFloat();
	}

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

	public void OnMoveKilled(MoveData moveData)
	{
		this.model.aerialMoveLock = false;
		this.playFloatLoopAnimation();
	}

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

	private void playAnimation(string animName, int animFrame)
	{
		this.playerDelegate.AnimationPlayer.PlayAnimation(animName, false, animFrame, -1f, -1f);
		this.playerDelegate.RestartCurrentActionState(false);
	}

	private CharacterAnimation getTransitionToAnim(RelativeDirection transitionTo)
	{
		if (transitionTo == RelativeDirection.Forward || transitionTo != RelativeDirection.Backward)
		{
			return this.configData.floatBackToForwardAnim;
		}
		return this.configData.floatForwardToBackAnim;
	}

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

	public bool IsOverridingActionStateAnimation(ActionState actionState, HorizontalDirection facing, ref string animationName)
	{
		return false;
	}

	public override void Destroy()
	{
		this.cleanup(false);
		base.Destroy();
	}

	public bool IsMoveBlocked(MoveData moveData)
	{
		return this.configData.allowSpecialInputCancel && this.IsActive && ((FloatCharacterComponent.specialMoveLabels.Contains(moveData.label) && this.model.aerialMoveLock) || moveData.label == MoveLabel.SideSpecial);
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.pooling.Clone<FloatCharacterComponent.FloatCharacterComponentModel>(this.model));
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<FloatCharacterComponent.FloatCharacterComponentModel>(ref this.model);
		return true;
	}

	public override void RegisterPreload(PreloadContext context)
	{
		foreach (ParticleData current in this.configData.loopParticles)
		{
			current.RegisterPreload(context);
		}
	}
}
