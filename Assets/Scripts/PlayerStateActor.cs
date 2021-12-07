// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateActor : IPlayerStateActor
{
	public interface IPlayerActorDelegate
	{
		bool PreventActionStateAnimations
		{
			get;
		}

		bool CanJump();

		IGameInput GetGameInput();

		void EndActiveMove(MoveEndType endType, bool processBufferedInput = true, bool transitioningToContinuingMove = false);

		void SetRotation(HorizontalDirection direction, bool allowMirror = true);

		void SetFacing(HorizontalDirection direction);

		void SetFacingAndRotation(HorizontalDirection direction);

		void BeginStun(int frames, StunType stunType, bool cannotTech, bool isSpike, Fixed knockbackForce, Vector2F knockbackVelocity);

		bool HasAnimationOverride(ActionState actionState, HorizontalDirection facing, ref string animName);

		void DismountRespawnPlatform();

		bool TryBeginMove(InputButtonsData inputButtonsData);

		bool TryBeginMove(MoveData moveData, InterruptData interrupt, ButtonPress buttonUsed, InputButtonsData inputButtonsData);

		bool TryBeginBufferedInterrupt(InputButtonsData inputButtonsData, bool isHighPriority);

		void Cache(InputButtonsData inputButtonsData);

		List<ButtonPress> GetBufferableInput(InputButtonsData inputButtonsData);

		InputButtonsData ProcessInput(bool retainBuffer);

		bool ProcessBufferedInput();

		void ClearBufferedInput();
	}

	public bool RecoverJump;

	private Dictionary<ActionState, Action> basicMoveCallbackMap = new Dictionary<ActionState, Action>();

	private ConfigData config;

	private PlayerStateActor.IPlayerActorDelegate actor;

	private IMoveSet moveSet;

	private IPlayerDataOwner data;

	private IGame game;

	private IAudioOwner audioOwner;

	private InputButtonsData recoveryButtons = new InputButtonsData();

	private InputButtonsData waveLandButtons = new InputButtonsData();

	[Inject]
	public ICombatCalculator combatCalculator
	{
		get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public UserAudioSettings userAudioSettings
	{
		get;
		set;
	}

	private IGameInput input
	{
		get
		{
			return this.actor.GetGameInput();
		}
	}

	public PlayerStateActor Setup(PlayerStateActor.IPlayerActorDelegate actor, IGameInput input, IMoveSet moveSet, IPlayerDataOwner data, IGame game, IAudioOwner audioOwner, ConfigData config)
	{
		this.actor = actor;
		this.moveSet = moveSet;
		this.data = data;
		this.game = game;
		this.config = config;
		this.audioOwner = audioOwner;
		this.basicMoveCallbackMap = new Dictionary<ActionState, Action>
		{
			{
				ActionState.Recoil,
				new Action(this.BeginIdling)
			},
			{
				ActionState.Pivot,
				new Action(this.onPivotComplete)
			},
			{
				ActionState.Dash,
				new Action(this.onDashComplete)
			},
			{
				ActionState.DashBrake,
				new Action(this.onDashBrakeComplete)
			},
			{
				ActionState.DashPivot,
				new Action(this.onDashPivotComplete)
			},
			{
				ActionState.Brake,
				new Action(this.onBrakeComplete)
			},
			{
				ActionState.RunPivot,
				new Action(this.onRunPivotComplete)
			},
			{
				ActionState.RunPivotBrake,
				new Action(this.BeginIdling)
			},
			{
				ActionState.CrouchBegin,
				new Action(this.onCrouchBeginComplete)
			},
			{
				ActionState.CrouchEnd,
				new Action(this.onCrouchEndComplete)
			},
			{
				ActionState.DazedBegin,
				new Action(this.onDazedBeginComplete)
			},
			{
				ActionState.DazedEnd,
				new Action(this.onDazedEndComplete)
			},
			{
				ActionState.TeeterBegin,
				new Action(this.onTeeterBeginComplete)
			},
			{
				ActionState.TakeOff,
				new Action(this.onTakeOffComplete)
			},
			{
				ActionState.Landing,
				new Action(this.onLandComplete)
			},
			{
				ActionState.Wavedash,
				new Action(this.onLandComplete)
			},
			{
				ActionState.ShieldBegin,
				new Action(this.onShieldBeginComplete)
			},
			{
				ActionState.ShieldEnd,
				new Action(this.onShieldEndComplete)
			},
			{
				ActionState.EdgeGrab,
				new Action(this.onLedgeGrabComplete)
			},
			{
				ActionState.GrabbedBegin,
				new Action(this.onGrabbedBeginComplete)
			},
			{
				ActionState.GrabbedPummelled,
				new Action(this.onGrabbedPummelledComplete)
			},
			{
				ActionState.GrabRelease,
				new Action(this.BeginIdling)
			},
			{
				ActionState.GrabEscapeGround,
				new Action(this.BeginIdling)
			},
			{
				ActionState.GrabEscapeAir,
				new Action(this.BeginIdling)
			},
			{
				ActionState.AirJump,
				new Action(this.BeginFallingStraight)
			},
			{
				ActionState.JumpStraight,
				new Action(this.BeginFallingStraight)
			},
			{
				ActionState.FallDown,
				new Action(this.onFallDownComplete)
			},
			{
				ActionState.HitStunAirS,
				new Action(this.BeginFallingStraight)
			},
			{
				ActionState.HitStunAirM,
				new Action(this.BeginFallingStraight)
			},
			{
				ActionState.HitStunAirL,
				new Action(this.BeginFallingStraight)
			},
			{
				ActionState.HitStunGroundS,
				new Action(this.BeginIdling)
			},
			{
				ActionState.HitStunGroundM,
				new Action(this.BeginIdling)
			},
			{
				ActionState.HitStunGroundL,
				new Action(this.BeginIdling)
			},
			{
				ActionState.HitStunMeteorS,
				new Action(this.BeginIdling)
			},
			{
				ActionState.HitStunMeteorM,
				new Action(this.BeginIdling)
			},
			{
				ActionState.HitStunMeteorL,
				new Action(this.BeginIdling)
			},
			{
				ActionState.HitTumbleHigh,
				new Action(this.beginTumbling)
			},
			{
				ActionState.HitTumbleLow,
				new Action(this.beginTumbling)
			},
			{
				ActionState.HitTumbleNeutral,
				new Action(this.beginTumbling)
			},
			{
				ActionState.HitTumbleSpin,
				new Action(this.beginTumbling)
			},
			{
				ActionState.HitTumbleTop,
				new Action(this.beginTumbling)
			},
			{
				ActionState.PlatformDrop,
				new Action(this.BeginFallingStraight)
			}
		};
		this.recoveryButtons.buttonsHeld.Add(ButtonPress.Up);
		this.recoveryButtons.moveButtonsPressed.Add(ButtonPress.Special);
		this.waveLandButtons.moveButtonsPressed.Add(ButtonPress.Shield1);
		this.waveLandButtons.moveButtonsPressed.Add(ButtonPress.Shield2);
		return this;
	}

	bool IPlayerStateActor.AttemptWavedash()
	{
		if (this.data.State.IsGrounded && this.AttemptJump(ButtonPress.Wavedash))
		{
			this.data.Model.queuedWavedashDodge = true;
			return true;
		}
		return false;
	}

	public bool AttemptJump(ButtonPress buttonSource)
	{
		if (this.actor.CanJump())
		{
			this.actor.EndActiveMove(MoveEndType.Cancelled, true, false);
			this.data.Model.jumpButtonSource = buttonSource;
			if (!this.data.State.IsGrounded)
			{
				if (buttonSource == ButtonPress.ShortJump)
				{
					this.data.Physics.ShortJump(this.input.HorizontalAxisValue);
				}
				else
				{
					this.data.Physics.Jump(this.input.HorizontalAxisValue);
				}
				this.onJump(true);
				return true;
			}
			if (this.data.State.IsGrounded && (this.data.State.IsStandingState || this.data.State.IsShieldingState))
			{
				bool isRunPivoting = this.data.State.IsRunPivoting;
				this.StartCharacterAction(ActionState.TakeOff, null, null, true, 0, false);
				if (isRunPivoting && this.data.CharacterData.runPivotJumpOffset != 0)
				{
					Fixed other = (this.data.Facing != HorizontalDirection.Right) ? (-1) : 1;
					this.data.Physics.ForceTranslate(new Vector3F(this.data.CharacterData.runPivotJumpOffset * other, 0, 0), false, true);
				}
				this.data.Model.jumpBeginFrame = this.game.Frame;
				return true;
			}
		}
		return false;
	}

	bool IPlayerStateActor.AttemptRecoveryJump()
	{
		return this.data.State.ShouldUseRecoveryJump && this.actor.TryBeginMove(this.recoveryButtons);
	}

	private void onTakeOffComplete()
	{
		if (this.data.ActiveMove.IsActive)
		{
			return;
		}
		Fixed horizontalAxisValue = this.input.HorizontalAxisValue;
		if (this.isJumpButtonStillHeld())
		{
			this.data.Physics.Jump(horizontalAxisValue);
			this.onJump(false);
		}
		else
		{
			this.data.Physics.ShortJump(horizontalAxisValue);
			this.onJump(false);
		}
	}

	private bool isJumpButtonStillHeld()
	{
		ButtonPress jumpButtonSource = this.data.Model.jumpButtonSource;
		if (jumpButtonSource != ButtonPress.UpTap)
		{
			return jumpButtonSource != ButtonPress.Wavedash && jumpButtonSource != ButtonPress.ShortJump && (jumpButtonSource == ButtonPress.FullJump || this.input.GetButton(this.data.Model.jumpButtonSource));
		}
		return this.input.IsTapJumpInputPressed;
	}

	private void onJump(bool isAirJump)
	{
		this.data.LedgeGrabController.ReleaseGrabbedLedge(true, false);
		this.data.State.MetaState = MetaState.Jump;
		this.data.State.SubState = SubStates.Resting;
		this.data.Physics.PlayerState.platformDropFrames = 0;
		if (isAirJump)
		{
			this.data.GameVFX.PlayParticle(this.config.defaultCharacterEffects.airJump, false, TeamNum.None);
			this.StartCharacterAction(ActionState.AirJump, null, null, true, 0, false);
		}
		else
		{
			this.StartCharacterAction(ActionState.JumpStraight, null, null, true, 0, false);
		}
	}

	private bool hasLeftFacingAnimation(ActionState action)
	{
		CharacterActionData action2 = this.moveSet.Actions.GetAction(action, false);
		return action2 != null && action2.leftAnimation != null;
	}

	private void syncFacingIfLeftAnimation(HorizontalDirection direction, ActionState action)
	{
		if (this.hasLeftFacingAnimation(action))
		{
			this.actor.SetFacing(direction);
			this.actor.SetRotation(direction, false);
		}
	}

	void IPlayerStateActor.BeginPivot(HorizontalDirection direction)
	{
		this.StartCharacterAction(ActionState.Pivot, null, null, true, 0, false);
		this.actor.SetFacing(direction);
	}

	private void onPivotComplete()
	{
		if (this.input.IsHorizontalDirectionHeld(this.data.Facing))
		{
			this.BeginWalking(this.input.HorizontalAxisValue);
		}
		else
		{
			this.BeginIdling();
		}
	}

	public void BeginDashing(HorizontalDirection direction)
	{
		Fixed other = this.data.Physics.DashAcceleration * WTime.fixedDeltaTime;
		this.data.State.MetaState = MetaState.Stand;
		this.data.Physics.StopMovement(true, true, VelocityType.Movement);
		this.data.Physics.AddGroundedHorizontalVelocity((this.data.Physics.DashStartSpeed - other) * ((direction != HorizontalDirection.Right) ? (-1) : 1));
		this.StartCharacterAction(ActionState.Dash, null, null, true, 0, false);
		this.data.GameVFX.PlayParticle(this.config.defaultCharacterEffects.dash, false, TeamNum.None);
	}

	public void BeginDashPivot(HorizontalDirection direction)
	{
		int dashPivotFrames = this.config.lagConfig.dashPivotFrames;
		Fixed length = this.data.AnimationPlayer.CurrentAnimationData.length;
		Fixed other = dashPivotFrames * WTime.fixedDeltaTime;
		Fixed overrideSpeed = length / other;
		this.data.Physics.StopMovement(true, true, VelocityType.Movement);
		this.StartCharacterAction(ActionState.DashPivot, overrideSpeed, null, null, true, 0, false);
		this.actor.SetFacing(direction);
	}

	private void onDashComplete()
	{
		if (!this.data.State.IsBraking)
		{
			if (this.input.IsHorizontalDirectionHeld(this.data.Facing) || this.input.IsHorizontalDirectionHeld(this.data.OppositeFacing))
			{
				this.beginRunning();
			}
			else
			{
				this.beginDashBraking();
			}
		}
	}

	private void beginRunning()
	{
		this.data.State.MetaState = MetaState.Stand;
		Fixed fastWalkMaxSpeed = this.data.Physics.FastWalkMaxSpeed;
		if (FixedMath.Abs(this.data.Physics.State.movementVelocity.x) < fastWalkMaxSpeed)
		{
			this.data.Physics.StopMovement(true, false, VelocityType.Movement);
			this.data.Physics.AddGroundedHorizontalVelocity(fastWalkMaxSpeed * InputUtils.GetDirectionMultiplier(this.data.Facing));
		}
		this.StartCharacterAction(ActionState.Run, null, null, true, 0, false);
	}

	public void BeginWalking(Fixed horizontalAxisValue)
	{
		horizontalAxisValue = FixedMath.Abs(horizontalAxisValue);
		this.data.State.MetaState = MetaState.Stand;
		ActionState actionState = ActionState.WalkFast;
		if (horizontalAxisValue < (Fixed)((double)this.config.inputConfig.walkOptions.mediumWalkThreshold))
		{
			actionState = ActionState.WalkSlow;
		}
		else if (horizontalAxisValue < (Fixed)((double)this.config.inputConfig.walkOptions.fastWalkThreshold))
		{
			actionState = ActionState.WalkMedium;
		}
		if (actionState != this.data.State.ActionState)
		{
			this.StartCharacterAction(actionState, null, null, true, 0, false);
		}
	}

	public void BeginIdling()
	{
		this.StartCharacterAction(ActionState.Idle, null, null, true, 0, false);
		this.data.State.MetaState = MetaState.Stand;
		this.data.State.SubState = SubStates.Resting;
	}

	public void BeginFallingStraight()
	{
		this.BeginFalling(ActionState.FallStraight, false);
	}

	public void RestartCurrentActionState(bool startAnimationAtCurrentFrame)
	{
		if (this.data.State.ActionState == ActionState.UsingMove)
		{
			return;
		}
		int num = (!startAnimationAtCurrentFrame) ? 0 : this.data.Model.actionStateFrame;
		ActionState actionState = this.data.State.ActionState;
		int actionStateFrame = num;
		this.StartCharacterAction(actionState, null, null, true, actionStateFrame, false);
	}

	public void BeginFalling(ActionState fallActionState, bool startAnimationAtCurrentFrame = false)
	{
		if (fallActionState != ActionState.FallStraight && fallActionState != ActionState.FallForward && fallActionState != ActionState.FallBack)
		{
			UnityEngine.Debug.LogError("Attempt to call BeginFalling with an invalid action state for fall.  Must be FallStraight, FallForward, or FallBack.");
			return;
		}
		int num = (!startAnimationAtCurrentFrame) ? 0 : this.data.Model.actionStateFrame;
		int actionStateFrame = num;
		this.StartCharacterAction(fallActionState, null, null, true, actionStateFrame, false);
		this.data.State.MetaState = MetaState.Jump;
		this.data.State.SubState = SubStates.Resting;
	}

	private void onFallDownComplete()
	{
		this.StartCharacterAction(ActionState.DownedLoop, null, null, true, 0, false);
	}

	void IPlayerStateActor.BeginDowned(ref Vector3F previousVelocity)
	{
		if (!this.data.State.IsShieldBroken)
		{
			this.data.Model.stunFrames = this.config.knockbackConfig.minimumDownedFrames;
			this.data.Model.clearInputBufferOnStunEnd = false;
		}
		this.StartCharacterAction(ActionState.FallDown, null, null, true, 0, false);
		this.data.State.MetaState = MetaState.Down;
		this.data.State.SubState = SubStates.Resting;
		if (previousVelocity.y < 0)
		{
			float num = -10f;
			float value = (float)previousVelocity.y / num;
			value = Mathf.Clamp(value, 0.01f, 1f);
			CameraShakeRequest request = new CameraShakeRequest(this.gameController.currentGame.Camera.cameraOptions.shakeData.downedShake);
			request.useMulti(value);
			request.useAngle((float)MathUtil.VectorToAngle(ref previousVelocity), false);
			this.gameController.currentGame.Camera.ShakeCamera(request);
			this.data.Audio.PlayGameSound(SoundKey.inGame_tumbleHitGround, this.audioOwner);
		}
		this.actor.EndActiveMove(MoveEndType.Cancelled, true, false);
	}

	bool IPlayerStateActor.TryBeginBraking()
	{
		if (this.data.State.CanBeginBraking)
		{
			this.beginBraking(null);
			return true;
		}
		return false;
	}

	private void beginDashBraking()
	{
		this.data.Physics.BeginDashBrakingOrPivoting();
		this.StartCharacterAction(ActionState.DashBrake, null, null, true, 0, false);
	}

	private void onDashBrakeComplete()
	{
		if (this.input.IsCrouchingInputPressed)
		{
			this.beginCrouching();
		}
		else if (this.input.IsHorizontalDirectionHeld(HorizontalDirection.Left) || this.input.IsHorizontalDirectionHeld(HorizontalDirection.Right))
		{
			this.beginRunning();
		}
		else
		{
			this.BeginIdling();
		}
	}

	private void onDashPivotComplete()
	{
		this.BeginDashing(this.data.Facing);
	}

	private void beginBraking(Action callback = null)
	{
		this.data.State.MetaState = MetaState.Stand;
		this.StartCharacterAction(ActionState.Brake, null, null, true, 0, false);
		this.data.GameVFX.PlayParticle(this.config.defaultCharacterEffects.brake, false, TeamNum.None);
	}

	private void onBrakeComplete()
	{
		if (this.input.IsShieldInputPressed)
		{
			this.beginShield(false);
		}
		else if (this.input.IsCrouchingInputPressed)
		{
			this.beginCrouching();
		}
		else
		{
			this.BeginIdling();
		}
	}

	void IPlayerStateActor.BeginRunPivot(HorizontalDirection direction)
	{
		this.data.State.MetaState = MetaState.Stand;
		this.data.Physics.BeginDashBrakingOrPivoting();
		this.StartCharacterAction(ActionState.RunPivot, null, null, true, 0, false);
		this.actor.SetFacing(this.data.OppositeFacing);
	}

	private void onRunPivotComplete()
	{
		if (this.input.IsHorizontalDirectionHeld(this.data.Facing))
		{
			this.beginRunning();
		}
		else
		{
			this.StartCharacterAction(ActionState.RunPivotBrake, null, null, true, 0, false);
		}
	}

	bool IPlayerStateActor.TryBeginCrouching()
	{
		if (this.data.State.CanBeginCrouching)
		{
			this.beginCrouching();
			return true;
		}
		return false;
	}

	private void beginCrouching()
	{
		this.actor.EndActiveMove(MoveEndType.Cancelled, false, false);
		this.StartCharacterAction(ActionState.CrouchBegin, null, null, true, 0, false);
	}

	private void onCrouchBeginComplete()
	{
		this.StartCharacterAction(ActionState.Crouching, null, null, true, 0, false);
	}

	private void onCrouchEndComplete()
	{
		this.BeginIdling();
	}

	private void beginTumbling()
	{
		this.data.State.MetaState = MetaState.Jump;
		this.data.Model.ClearLastTumbleData();
		this.StartCharacterAction(ActionState.Tumble, null, null, true, 0, false);
	}

	public bool TryBeginShield(bool triggeredByMove)
	{
		if (this.data.State.CanBeginShield || triggeredByMove)
		{
			this.beginShield(triggeredByMove);
			return true;
		}
		return false;
	}

	public bool TryResumeShield()
	{
		if (this.data.State.IsShieldingState && this.data.State.ActionState != ActionState.ShieldLoop)
		{
			this.StartCharacterAction(ActionState.ShieldLoop, null, null, true, 0, false);
			return true;
		}
		return false;
	}

	private void onLandComplete()
	{
		if (this.data.AreInputsLocked)
		{
			this.BeginIdling();
			return;
		}
		this.data.Model.ClearLastHitData();
		this.data.Physics.PlayerState.lastPlatformDroppedThrough = null;
		if (this.input.IsCrouchingInputPressed)
		{
			this.beginCrouching();
		}
		else
		{
			this.BeginIdling();
		}
	}

	void IPlayerStateActor.BeginTeetering()
	{
		this.StartCharacterAction(ActionState.TeeterBegin, null, null, true, 0, false);
		this.data.Physics.State.ClearVelocity(true, true, true, VelocityType.Total);
	}

	private void onTeeterBeginComplete()
	{
		this.StartCharacterAction(ActionState.TeeterLoop, null, null, true, 0, false);
	}

	void IPlayerStateActor.BeginPlatformDrop(IPhysicsCollider platformCollider)
	{
		this.StartCharacterAction(ActionState.PlatformDrop, null, null, true, 0, false);
		this.data.Physics.State.platformFallPreventFastfall = this.config.lagConfig.platformFallPreventFastfall;
		this.data.State.MetaState = MetaState.Jump;
		this.data.Physics.PlayerState.platformDropFrames = this.config.lagConfig.fallThroughPlatformDurationFrames;
		this.data.Physics.PlayerState.lastPlatformDroppedThrough = platformCollider;
		this.data.Physics.AddVelocity(new Vector3F(0, Fixed.Create(-4.0), 0), -InputUtils.GetDirectionMultiplier(this.data.Facing), VelocityType.Movement);
		this.data.Physics.ForceTranslate(new Vector3F(0, Fixed.Create(-0.4), 0), false, false);
	}

	void IPlayerStateActor.ReleaseFromGrab()
	{
		this.data.Physics.StopMovement(true, true, VelocityType.Movement);
		ActionState reference = ActionState.GrabEscapeGround;
		Vector2F airGrabEscapeVelocity;
		if (this.input.IsJumpInputPressed)
		{
			this.data.Physics.State.isGrounded = false;
			reference = ActionState.GrabEscapeAir;
			airGrabEscapeVelocity = this.config.grabConfig.airGrabEscapeVelocity;
		}
		else
		{
			airGrabEscapeVelocity = new Vector2F(this.config.grabConfig.grabEscapeSpeed, 0);
		}
		this.StartCharacterAction(reference, null, null, true, 0, false);
		this.data.Physics.AddVelocity(airGrabEscapeVelocity, -InputUtils.GetDirectionMultiplier(this.data.Facing), VelocityType.Movement);
	}

	void IPlayerStateActor.BeginDaze()
	{
		this.data.State.MetaState = MetaState.Stand;
		this.data.Renderer.SetColorModeFlag(ColorMode.Dazed, true);
		this.StartCharacterAction(ActionState.DazedBegin, null, null, true, 0, false);
	}

	private void onDazedBeginComplete()
	{
		this.StartCharacterAction(ActionState.DazedLoop, null, null, true, 0, false);
	}

	private void onDazedEndComplete()
	{
		this.BeginIdling();
	}

	private void onLedgeGrabComplete()
	{
		this.data.State.MetaState = MetaState.LedgeHang;
		this.StartCharacterAction(ActionState.EdgeHang, null, null, true, 0, false);
		this.data.LedgeGrabController.OnLedgeGrabComplete();
	}

	private void onGrabbedBeginComplete()
	{
		this.StartCharacterAction(ActionState.GrabbedLoop, null, null, true, 0, false);
	}

	private void onGrabbedPummelledComplete()
	{
		this.StartCharacterAction(ActionState.GrabbedLoop, null, null, true, 0, false);
	}

	public void ReleaseStun()
	{
		this.data.Model.stunFrames = 0;
		this.data.Model.smokeTrailFrames = 0;
		this.data.Model.stunTechMode = StunTechMode.Techable;
		this.data.Model.stunIsSpike = false;
		this.data.Model.jumpStunFrames = 0;
		if (this.data.Model.clearInputBufferOnStunEnd)
		{
			this.actor.ClearBufferedInput();
		}
		else
		{
			this.actor.ProcessBufferedInput();
		}
		this.data.Model.clearInputBufferOnStunEnd = false;
		this.data.Model.stunType = StunType.None;
		if (this.data.State.CanBeginIdling && !this.input.IsHorizontalDirectionHeld(HorizontalDirection.Any))
		{
			this.BeginIdling();
		}
		else if (this.data.State.IsGrounded && this.data.State.IsHitStunned && this.shouldExitActionState())
		{
			this.exitActionState(this.data.State.ActionState);
		}
		else if (this.data.State.IsTumbling && this.data.Model.actionStateFrame >= this.data.AnimationPlayer.CurrentAnimationGameFramelength)
		{
			this.exitActionState(this.data.State.ActionState);
		}
		this.actor.ProcessInput(true);
	}

	void IPlayerStateActor.CheckShielding()
	{
		if (this.data.State.IsHitLagPaused || this.data.AreInputsLocked)
		{
			return;
		}
		if (!this.data.State.IsShieldingState && !this.data.State.IsShieldBroken)
		{
			if (this.input != null && this.input.IsShieldInputPressed)
			{
				this.TryBeginShield(false);
			}
		}
		else if (this.data.State.IsShieldingState && !this.data.State.CanMaintainShield)
		{
			if (this.data.State.IsShieldBroken)
			{
				this.BreakShield();
			}
			else
			{
				if (!this.data.State.IsGrounded && this.data.State.IsStunned)
				{
					this.onCliffBreakShield();
				}
				this.ReleaseShield(false, true);
			}
		}
	}

	private void onCliffBreakShield()
	{
		this.actor.ClearBufferedInput();
		this.data.Model.clearInputBufferOnStunEnd = true;
	}

	public void BreakShield()
	{
		this.ReleaseShield(false, true);
		this.data.Shield.BreakShield();
		this.data.Audio.PlayGameSound(new AudioRequest(this.config.defaultCharacterEffects.shieldBreakSound, this.audioOwner, null));
		Vector2F vector2F = Vector3F.up * this.data.Physics.ShieldBreakSpeed;
		Fixed knockbackForce = vector2F.magnitude / this.config.knockbackConfig.knockbackToSpeedConversion;
		this.data.Physics.StopMovement(true, true, VelocityType.Total);
		this.data.Physics.AddVelocity(vector2F, 1, VelocityType.Knockback);
		this.actor.BeginStun(this.combatCalculator.CalculateShieldBreakFrames(this.data.Model.damage), StunType.ShieldBreakStun, true, false, knockbackForce, vector2F);
		this.data.Model.ClearLastTumbleData();
		this.StartCharacterAction(ActionState.Tumble, null, null, true, 0, false);
	}

	public void ReleaseShieldBreak()
	{
		this.data.Shield.ResetHealth();
		this.ReleaseStun();
		this.data.Renderer.SetColorModeFlag(ColorMode.Dazed, false);
		if (this.data.State.ActionState == ActionState.DazedLoop)
		{
			this.StartCharacterAction(ActionState.DazedEnd, null, null, true, 0, false);
		}
	}

	private void beginShield(bool triggeredByMove)
	{
		bool wasRunning = this.data.State.IsDashing || this.data.State.IsRunning;
		if (!triggeredByMove)
		{
			this.actor.EndActiveMove(MoveEndType.Cancelled, true, false);
		}
		if (this.data.State.IsDashing)
		{
			this.data.Physics.StopMovement(true, true, VelocityType.Movement);
		}
		this.data.State.SubState = SubStates.Resting;
		if (!triggeredByMove)
		{
			this.StartCharacterAction(ActionState.ShieldBegin, null, null, true, 0, false);
		}
		this.data.State.MetaState = MetaState.Shielding;
		this.data.Shield.OnShieldBegin(wasRunning);
	}

	public void ReleaseShield(bool playShieldEnd, bool changeState)
	{
		if (!this.data.Shield.IsActive)
		{
			return;
		}
		this.data.Shield.OnShieldReleased();
		if (changeState)
		{
			if (this.data.State.IsGrounded)
			{
				this.data.State.MetaState = MetaState.Stand;
			}
			else
			{
				this.data.State.MetaState = MetaState.Jump;
			}
		}
		if (playShieldEnd)
		{
			this.StartCharacterAction(ActionState.ShieldEnd, null, null, true, 0, false);
		}
	}

	private void onShieldBeginComplete()
	{
		this.StartCharacterAction(ActionState.ShieldLoop, null, null, true, 0, false);
	}

	private void onShieldEndComplete()
	{
		if (this.input.IsHorizontalDirectionHeld(HorizontalDirection.Any))
		{
			this.BeginWalking(this.input.HorizontalAxisValue);
		}
		else
		{
			this.StartCharacterAction(ActionState.Idle, null, null, true, 0, false);
		}
	}

	private AudioData selectRandomSound(SoundEffect effect)
	{
		if (this.userAudioSettings.UseAltSounds() && effect.altSounds.Length > 0)
		{
			return effect.altSounds[UnityEngine.Random.Range(0, effect.altSounds.Length)];
		}
		if (effect.sounds.Length > 0)
		{
			return effect.sounds[UnityEngine.Random.Range(0, effect.sounds.Length)];
		}
		return effect.sound;
	}

	public void TickActionState()
	{
		if (this.data.State.ActionState != ActionState.None && !this.data.State.IsHitLagPaused)
		{
			this.data.Model.actionStateFrame++;
			CharacterActionData actionData = this.data.ActionData;
			if (actionData != null)
			{
				for (int i = 0; i < actionData.soundEffects.Length; i++)
				{
					SoundEffect soundEffect = actionData.soundEffects[i];
					bool flag = soundEffect.frame - 1 == (this.data.Model.actionStateFrame - 1) % actionData.frameDuration;
					if (soundEffect.frame != 1 && flag)
					{
						AudioData audioData = this.selectRandomSound(soundEffect);
						this.data.Audio.PlayGameSound(new AudioRequest(audioData, this.audioOwner, null));
					}
				}
			}
			if (this.shouldExitActionState())
			{
				this.exitActionState(this.data.State.ActionState);
			}
			else if ((this.data.State.ActionState == ActionState.Landing || this.data.State.ActionState == ActionState.Wavedash) && this.data.Model.actionStateFrame == this.data.AnimationPlayer.CurrentAnimationGameFramelength - Mathf.Max(this.data.Model.overrideActionStateInterruptibilityFrames, this.data.ActionData.interruptibleFrames))
			{
				this.actor.ProcessBufferedInput();
			}
		}
	}

	private bool shouldExitActionState()
	{
		if (this.data.Model.actionStateFrame >= this.data.AnimationPlayer.CurrentAnimationGameFramelength)
		{
			if (this.data.AnimationPlayer.CurrentAnimationData.wrapMode != WrapMode.Loop && this.data.AnimationPlayer.CurrentAnimationData.wrapMode != WrapMode.ClampForever)
			{
				return true;
			}
			if (this.data.Model.stunFrames <= 0)
			{
				if (this.data.State.IsTumbling)
				{
					return true;
				}
				if (this.data.State.IsHitStunned && this.data.State.IsGrounded)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void exitActionState(ActionState basicMove)
	{
		this.data.Model.actionStateFrame = 0;
		if (this.basicMoveCallbackMap.ContainsKey(basicMove))
		{
			Action action = this.basicMoveCallbackMap[basicMove];
			if (action != null)
			{
				action();
			}
		}
		this.actor.ProcessBufferedInput();
		this.data.Model.actionStateFrame = 0;
	}

	private void handleJumpTracking(ActionState reference)
	{
		this.data.Physics.State.pivotJump = false;
		if (this.data.CharacterData.useDashPivotState)
		{
			if (this.data.State.ActionState == ActionState.Dash && reference == ActionState.DashPivot)
			{
				this.data.Physics.State.dashPivotFrame = this.game.Frame;
			}
		}
		else if (this.data.State.ActionState == ActionState.Dash && reference == ActionState.Dash)
		{
			this.data.Physics.State.dashPivotFrame = this.game.Frame;
		}
		if (reference == ActionState.TakeOff && (this.data.State.IsRunPivoting || this.data.State.IsDashPivoting || this.game.Frame - this.data.Physics.State.dashPivotFrame <= this.config.inputConfig.pivotJumpFrames))
		{
			this.data.Physics.State.pivotJump = true;
		}
	}

	public void StartCharacterAction(ActionState reference, string overrideAnimation = null, string overrideLeftAnimation = null, bool cancelsMove = true, int actionStateFrame = 0, bool neverRotateFacing = false)
	{
		this.StartCharacterAction(reference, 1, overrideAnimation, overrideLeftAnimation, cancelsMove, actionStateFrame, neverRotateFacing);
	}

	public void StartCharacterAction(ActionState reference, Fixed overrideSpeed, string overrideAnimation = null, string overrideLeftAnimation = null, bool cancelsMove = true, int actionStateFrame = 0, bool neverRotateFacing = false)
	{
		this.syncFacingIfLeftAnimation(this.data.Facing, reference);
		this.handleJumpTracking(reference);
		CharacterActionData action = this.moveSet.Actions.GetAction(reference, false);
		this.data.State.ActionState = reference;
		this.data.Model.actionStateFrame = actionStateFrame;
		this.data.Model.overrideActionStateInterruptibilityFrames = 0;
		this.data.Model.isClankLag = false;
		this.data.Model.isKillCamHitlag = false;
		this.data.Model.isFlourishOwner = false;
		this.data.Model.isCameraZoomHitLag = false;
		if (this.game.StartedGame && action != null)
		{
			for (int i = 0; i < action.soundEffects.Length; i++)
			{
				SoundEffect soundEffect = action.soundEffects[i];
				if (soundEffect.frame == 1)
				{
					AudioData audioData = this.selectRandomSound(soundEffect);
					if (reference == ActionState.Death)
					{
						this.data.Audio.PlayGameSound(new AudioRequest(audioData, (Vector3)this.data.Physics.GetPosition(), null));
					}
					else
					{
						this.data.Audio.PlayGameSound(new AudioRequest(audioData, this.audioOwner, null));
					}
				}
			}
		}
		if (action != null)
		{
			if (this.actor.PreventActionStateAnimations)
			{
				return;
			}
			if (this.data.ActiveMove.IsActive && action.characterActionState != ActionState.UsingMove && cancelsMove)
			{
				this.actor.EndActiveMove(MoveEndType.Cancelled, false, false);
			}
			bool mirror = this.data.CharacterData.reversesStance && this.data.Facing == HorizontalDirection.Left;
			bool flag = true;
			string animationName = action.name;
			string text = null;
			if (overrideAnimation != null)
			{
				animationName = overrideAnimation;
			}
			else if (this.actor.HasAnimationOverride(reference, HorizontalDirection.Right, ref text))
			{
				animationName = text;
			}
			if (this.data.Facing == HorizontalDirection.Left)
			{
				if (overrideLeftAnimation != null)
				{
					animationName = overrideLeftAnimation;
					flag = false;
					mirror = false;
				}
				else if (this.actor.HasAnimationOverride(reference, this.data.Facing, ref text))
				{
					animationName = text;
					flag = false;
					mirror = false;
				}
				else if (action.leftAnimation != null)
				{
					animationName = action.LeftAnimationName;
					flag = false;
					mirror = false;
				}
			}
			this.data.AnimationPlayer.PlayCharacterAction(action, animationName, mirror, overrideSpeed, true);
			this.actor.SetFacing(this.data.Facing);
			if (flag && !neverRotateFacing)
			{
				this.actor.SetRotation(this.data.Facing, !this.hasLeftFacingAnimation(reference));
			}
			this.data.Physics.OnCollisionBoundsChanged(false);
		}
		else if (reference != ActionState.UsingMove)
		{
			UnityEngine.Debug.Log("No character action for " + reference);
		}
	}
}
