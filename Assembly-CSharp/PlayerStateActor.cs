using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x020005FA RID: 1530
public class PlayerStateActor : IPlayerStateActor
{
	// Token: 0x17000910 RID: 2320
	// (get) Token: 0x060024F3 RID: 9459 RVA: 0x000B7B3A File Offset: 0x000B5F3A
	// (set) Token: 0x060024F4 RID: 9460 RVA: 0x000B7B42 File Offset: 0x000B5F42
	[Inject]
	public ICombatCalculator combatCalculator { get; set; }

	// Token: 0x17000911 RID: 2321
	// (get) Token: 0x060024F5 RID: 9461 RVA: 0x000B7B4B File Offset: 0x000B5F4B
	// (set) Token: 0x060024F6 RID: 9462 RVA: 0x000B7B53 File Offset: 0x000B5F53
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000912 RID: 2322
	// (get) Token: 0x060024F7 RID: 9463 RVA: 0x000B7B5C File Offset: 0x000B5F5C
	// (set) Token: 0x060024F8 RID: 9464 RVA: 0x000B7B64 File Offset: 0x000B5F64
	[Inject]
	public UserAudioSettings userAudioSettings { get; set; }

	// Token: 0x17000913 RID: 2323
	// (get) Token: 0x060024F9 RID: 9465 RVA: 0x000B7B6D File Offset: 0x000B5F6D
	private IGameInput input
	{
		get
		{
			return this.actor.GetGameInput();
		}
	}

	// Token: 0x060024FA RID: 9466 RVA: 0x000B7B7C File Offset: 0x000B5F7C
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

	// Token: 0x060024FB RID: 9467 RVA: 0x000B7F50 File Offset: 0x000B6350
	bool IPlayerStateActor.AttemptWavedash()
	{
		if (this.data.State.IsGrounded && this.AttemptJump(ButtonPress.Wavedash))
		{
			this.data.Model.queuedWavedashDodge = true;
			return true;
		}
		return false;
	}

	// Token: 0x060024FC RID: 9468 RVA: 0x000B7F88 File Offset: 0x000B6388
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
					Fixed other = (this.data.Facing != HorizontalDirection.Right) ? -1 : 1;
					this.data.Physics.ForceTranslate(new Vector3F(this.data.CharacterData.runPivotJumpOffset * other, 0, 0), false, true);
				}
				this.data.Model.jumpBeginFrame = this.game.Frame;
				return true;
			}
		}
		return false;
	}

	// Token: 0x060024FD RID: 9469 RVA: 0x000B8117 File Offset: 0x000B6517
	bool IPlayerStateActor.AttemptRecoveryJump()
	{
		return this.data.State.ShouldUseRecoveryJump && this.actor.TryBeginMove(this.recoveryButtons);
	}

	// Token: 0x060024FE RID: 9470 RVA: 0x000B8144 File Offset: 0x000B6544
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

	// Token: 0x060024FF RID: 9471 RVA: 0x000B81B4 File Offset: 0x000B65B4
	private bool isJumpButtonStillHeld()
	{
		ButtonPress jumpButtonSource = this.data.Model.jumpButtonSource;
		if (jumpButtonSource != ButtonPress.UpTap)
		{
			return jumpButtonSource != ButtonPress.Wavedash && jumpButtonSource != ButtonPress.ShortJump && (jumpButtonSource == ButtonPress.FullJump || this.input.GetButton(this.data.Model.jumpButtonSource));
		}
		return this.input.IsTapJumpInputPressed;
	}

	// Token: 0x06002500 RID: 9472 RVA: 0x000B8224 File Offset: 0x000B6624
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

	// Token: 0x06002501 RID: 9473 RVA: 0x000B82C4 File Offset: 0x000B66C4
	private bool hasLeftFacingAnimation(ActionState action)
	{
		CharacterActionData action2 = this.moveSet.Actions.GetAction(action, false);
		return action2 != null && action2.leftAnimation != null;
	}

	// Token: 0x06002502 RID: 9474 RVA: 0x000B82F9 File Offset: 0x000B66F9
	private void syncFacingIfLeftAnimation(HorizontalDirection direction, ActionState action)
	{
		if (this.hasLeftFacingAnimation(action))
		{
			this.actor.SetFacing(direction);
			this.actor.SetRotation(direction, false);
		}
	}

	// Token: 0x06002503 RID: 9475 RVA: 0x000B8320 File Offset: 0x000B6720
	void IPlayerStateActor.BeginPivot(HorizontalDirection direction)
	{
		this.StartCharacterAction(ActionState.Pivot, null, null, true, 0, false);
		this.actor.SetFacing(direction);
	}

	// Token: 0x06002504 RID: 9476 RVA: 0x000B833B File Offset: 0x000B673B
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

	// Token: 0x06002505 RID: 9477 RVA: 0x000B8374 File Offset: 0x000B6774
	public void BeginDashing(HorizontalDirection direction)
	{
		Fixed other = this.data.Physics.DashAcceleration * WTime.fixedDeltaTime;
		this.data.State.MetaState = MetaState.Stand;
		this.data.Physics.StopMovement(true, true, VelocityType.Movement);
		this.data.Physics.AddGroundedHorizontalVelocity((this.data.Physics.DashStartSpeed - other) * ((direction != HorizontalDirection.Right) ? -1 : 1));
		this.StartCharacterAction(ActionState.Dash, null, null, true, 0, false);
		this.data.GameVFX.PlayParticle(this.config.defaultCharacterEffects.dash, false, TeamNum.None);
	}

	// Token: 0x06002506 RID: 9478 RVA: 0x000B842C File Offset: 0x000B682C
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

	// Token: 0x06002507 RID: 9479 RVA: 0x000B84A4 File Offset: 0x000B68A4
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

	// Token: 0x06002508 RID: 9480 RVA: 0x000B8514 File Offset: 0x000B6914
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

	// Token: 0x06002509 RID: 9481 RVA: 0x000B85B8 File Offset: 0x000B69B8
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

	// Token: 0x0600250A RID: 9482 RVA: 0x000B8659 File Offset: 0x000B6A59
	public void BeginIdling()
	{
		this.StartCharacterAction(ActionState.Idle, null, null, true, 0, false);
		this.data.State.MetaState = MetaState.Stand;
		this.data.State.SubState = SubStates.Resting;
	}

	// Token: 0x0600250B RID: 9483 RVA: 0x000B8689 File Offset: 0x000B6A89
	public void BeginFallingStraight()
	{
		this.BeginFalling(ActionState.FallStraight, false);
	}

	// Token: 0x0600250C RID: 9484 RVA: 0x000B8694 File Offset: 0x000B6A94
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

	// Token: 0x0600250D RID: 9485 RVA: 0x000B86F8 File Offset: 0x000B6AF8
	public void BeginFalling(ActionState fallActionState, bool startAnimationAtCurrentFrame = false)
	{
		if (fallActionState != ActionState.FallStraight && fallActionState != ActionState.FallForward && fallActionState != ActionState.FallBack)
		{
			Debug.LogError("Attempt to call BeginFalling with an invalid action state for fall.  Must be FallStraight, FallForward, or FallBack.");
			return;
		}
		int num = (!startAnimationAtCurrentFrame) ? 0 : this.data.Model.actionStateFrame;
		int actionStateFrame = num;
		this.StartCharacterAction(fallActionState, null, null, true, actionStateFrame, false);
		this.data.State.MetaState = MetaState.Jump;
		this.data.State.SubState = SubStates.Resting;
	}

	// Token: 0x0600250E RID: 9486 RVA: 0x000B8775 File Offset: 0x000B6B75
	private void onFallDownComplete()
	{
		this.StartCharacterAction(ActionState.DownedLoop, null, null, true, 0, false);
	}

	// Token: 0x0600250F RID: 9487 RVA: 0x000B8784 File Offset: 0x000B6B84
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

	// Token: 0x06002510 RID: 9488 RVA: 0x000B88BA File Offset: 0x000B6CBA
	bool IPlayerStateActor.TryBeginBraking()
	{
		if (this.data.State.CanBeginBraking)
		{
			this.beginBraking(null);
			return true;
		}
		return false;
	}

	// Token: 0x06002511 RID: 9489 RVA: 0x000B88DB File Offset: 0x000B6CDB
	private void beginDashBraking()
	{
		this.data.Physics.BeginDashBrakingOrPivoting();
		this.StartCharacterAction(ActionState.DashBrake, null, null, true, 0, false);
	}

	// Token: 0x06002512 RID: 9490 RVA: 0x000B88FC File Offset: 0x000B6CFC
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

	// Token: 0x06002513 RID: 9491 RVA: 0x000B8957 File Offset: 0x000B6D57
	private void onDashPivotComplete()
	{
		this.BeginDashing(this.data.Facing);
	}

	// Token: 0x06002514 RID: 9492 RVA: 0x000B896C File Offset: 0x000B6D6C
	private void beginBraking(Action callback = null)
	{
		this.data.State.MetaState = MetaState.Stand;
		this.StartCharacterAction(ActionState.Brake, null, null, true, 0, false);
		this.data.GameVFX.PlayParticle(this.config.defaultCharacterEffects.brake, false, TeamNum.None);
	}

	// Token: 0x06002515 RID: 9493 RVA: 0x000B89BB File Offset: 0x000B6DBB
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

	// Token: 0x06002516 RID: 9494 RVA: 0x000B89FC File Offset: 0x000B6DFC
	void IPlayerStateActor.BeginRunPivot(HorizontalDirection direction)
	{
		this.data.State.MetaState = MetaState.Stand;
		this.data.Physics.BeginDashBrakingOrPivoting();
		this.StartCharacterAction(ActionState.RunPivot, null, null, true, 0, false);
		this.actor.SetFacing(this.data.OppositeFacing);
	}

	// Token: 0x06002517 RID: 9495 RVA: 0x000B8A4D File Offset: 0x000B6E4D
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

	// Token: 0x06002518 RID: 9496 RVA: 0x000B8A82 File Offset: 0x000B6E82
	bool IPlayerStateActor.TryBeginCrouching()
	{
		if (this.data.State.CanBeginCrouching)
		{
			this.beginCrouching();
			return true;
		}
		return false;
	}

	// Token: 0x06002519 RID: 9497 RVA: 0x000B8AA2 File Offset: 0x000B6EA2
	private void beginCrouching()
	{
		this.actor.EndActiveMove(MoveEndType.Cancelled, false, false);
		this.StartCharacterAction(ActionState.CrouchBegin, null, null, true, 0, false);
	}

	// Token: 0x0600251A RID: 9498 RVA: 0x000B8ABF File Offset: 0x000B6EBF
	private void onCrouchBeginComplete()
	{
		this.StartCharacterAction(ActionState.Crouching, null, null, true, 0, false);
	}

	// Token: 0x0600251B RID: 9499 RVA: 0x000B8ACE File Offset: 0x000B6ECE
	private void onCrouchEndComplete()
	{
		this.BeginIdling();
	}

	// Token: 0x0600251C RID: 9500 RVA: 0x000B8AD6 File Offset: 0x000B6ED6
	private void beginTumbling()
	{
		this.data.State.MetaState = MetaState.Jump;
		this.data.Model.ClearLastTumbleData();
		this.StartCharacterAction(ActionState.Tumble, null, null, true, 0, false);
	}

	// Token: 0x0600251D RID: 9501 RVA: 0x000B8B06 File Offset: 0x000B6F06
	public bool TryBeginShield(bool triggeredByMove)
	{
		if (this.data.State.CanBeginShield || triggeredByMove)
		{
			this.beginShield(triggeredByMove);
			return true;
		}
		return false;
	}

	// Token: 0x0600251E RID: 9502 RVA: 0x000B8B2D File Offset: 0x000B6F2D
	public bool TryResumeShield()
	{
		if (this.data.State.IsShieldingState && this.data.State.ActionState != ActionState.ShieldLoop)
		{
			this.StartCharacterAction(ActionState.ShieldLoop, null, null, true, 0, false);
			return true;
		}
		return false;
	}

	// Token: 0x0600251F RID: 9503 RVA: 0x000B8B6C File Offset: 0x000B6F6C
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

	// Token: 0x06002520 RID: 9504 RVA: 0x000B8BD7 File Offset: 0x000B6FD7
	void IPlayerStateActor.BeginTeetering()
	{
		this.StartCharacterAction(ActionState.TeeterBegin, null, null, true, 0, false);
		this.data.Physics.State.ClearVelocity(true, true, true, VelocityType.Total);
	}

	// Token: 0x06002521 RID: 9505 RVA: 0x000B8BFF File Offset: 0x000B6FFF
	private void onTeeterBeginComplete()
	{
		this.StartCharacterAction(ActionState.TeeterLoop, null, null, true, 0, false);
	}

	// Token: 0x06002522 RID: 9506 RVA: 0x000B8C10 File Offset: 0x000B7010
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

	// Token: 0x06002523 RID: 9507 RVA: 0x000B8D14 File Offset: 0x000B7114
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

	// Token: 0x06002524 RID: 9508 RVA: 0x000B8DC2 File Offset: 0x000B71C2
	void IPlayerStateActor.BeginDaze()
	{
		this.data.State.MetaState = MetaState.Stand;
		this.data.Renderer.SetColorModeFlag(ColorMode.Dazed, true);
		this.StartCharacterAction(ActionState.DazedBegin, null, null, true, 0, false);
	}

	// Token: 0x06002525 RID: 9509 RVA: 0x000B8DF5 File Offset: 0x000B71F5
	private void onDazedBeginComplete()
	{
		this.StartCharacterAction(ActionState.DazedLoop, null, null, true, 0, false);
	}

	// Token: 0x06002526 RID: 9510 RVA: 0x000B8E04 File Offset: 0x000B7204
	private void onDazedEndComplete()
	{
		this.BeginIdling();
	}

	// Token: 0x06002527 RID: 9511 RVA: 0x000B8E0C File Offset: 0x000B720C
	private void onLedgeGrabComplete()
	{
		this.data.State.MetaState = MetaState.LedgeHang;
		this.StartCharacterAction(ActionState.EdgeHang, null, null, true, 0, false);
		this.data.LedgeGrabController.OnLedgeGrabComplete();
	}

	// Token: 0x06002528 RID: 9512 RVA: 0x000B8E3C File Offset: 0x000B723C
	private void onGrabbedBeginComplete()
	{
		this.StartCharacterAction(ActionState.GrabbedLoop, null, null, true, 0, false);
	}

	// Token: 0x06002529 RID: 9513 RVA: 0x000B8E4B File Offset: 0x000B724B
	private void onGrabbedPummelledComplete()
	{
		this.StartCharacterAction(ActionState.GrabbedLoop, null, null, true, 0, false);
	}

	// Token: 0x0600252A RID: 9514 RVA: 0x000B8E5C File Offset: 0x000B725C
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

	// Token: 0x0600252B RID: 9515 RVA: 0x000B8FF0 File Offset: 0x000B73F0
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

	// Token: 0x0600252C RID: 9516 RVA: 0x000B90F7 File Offset: 0x000B74F7
	private void onCliffBreakShield()
	{
		this.actor.ClearBufferedInput();
		this.data.Model.clearInputBufferOnStunEnd = true;
	}

	// Token: 0x0600252D RID: 9517 RVA: 0x000B9118 File Offset: 0x000B7518
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

	// Token: 0x0600252E RID: 9518 RVA: 0x000B9218 File Offset: 0x000B7618
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

	// Token: 0x0600252F RID: 9519 RVA: 0x000B9274 File Offset: 0x000B7674
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

	// Token: 0x06002530 RID: 9520 RVA: 0x000B932C File Offset: 0x000B772C
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

	// Token: 0x06002531 RID: 9521 RVA: 0x000B93B4 File Offset: 0x000B77B4
	private void onShieldBeginComplete()
	{
		this.StartCharacterAction(ActionState.ShieldLoop, null, null, true, 0, false);
	}

	// Token: 0x06002532 RID: 9522 RVA: 0x000B93C3 File Offset: 0x000B77C3
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

	// Token: 0x06002533 RID: 9523 RVA: 0x000B93F8 File Offset: 0x000B77F8
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

	// Token: 0x06002534 RID: 9524 RVA: 0x000B9478 File Offset: 0x000B7878
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

	// Token: 0x06002535 RID: 9525 RVA: 0x000B9608 File Offset: 0x000B7A08
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

	// Token: 0x06002536 RID: 9526 RVA: 0x000B96CC File Offset: 0x000B7ACC
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

	// Token: 0x06002537 RID: 9527 RVA: 0x000B9734 File Offset: 0x000B7B34
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

	// Token: 0x06002538 RID: 9528 RVA: 0x000B986C File Offset: 0x000B7C6C
	public void StartCharacterAction(ActionState reference, string overrideAnimation = null, string overrideLeftAnimation = null, bool cancelsMove = true, int actionStateFrame = 0, bool neverRotateFacing = false)
	{
		this.StartCharacterAction(reference, 1, overrideAnimation, overrideLeftAnimation, cancelsMove, actionStateFrame, neverRotateFacing);
	}

	// Token: 0x06002539 RID: 9529 RVA: 0x000B9884 File Offset: 0x000B7C84
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
			Debug.Log("No character action for " + reference);
		}
	}

	// Token: 0x04001B9E RID: 7070
	public bool RecoverJump;

	// Token: 0x04001B9F RID: 7071
	private Dictionary<ActionState, Action> basicMoveCallbackMap = new Dictionary<ActionState, Action>();

	// Token: 0x04001BA0 RID: 7072
	private ConfigData config;

	// Token: 0x04001BA1 RID: 7073
	private PlayerStateActor.IPlayerActorDelegate actor;

	// Token: 0x04001BA2 RID: 7074
	private IMoveSet moveSet;

	// Token: 0x04001BA3 RID: 7075
	private IPlayerDataOwner data;

	// Token: 0x04001BA4 RID: 7076
	private IGame game;

	// Token: 0x04001BA5 RID: 7077
	private IAudioOwner audioOwner;

	// Token: 0x04001BA6 RID: 7078
	private InputButtonsData recoveryButtons = new InputButtonsData();

	// Token: 0x04001BA7 RID: 7079
	private InputButtonsData waveLandButtons = new InputButtonsData();

	// Token: 0x020005FB RID: 1531
	public interface IPlayerActorDelegate
	{
		// Token: 0x0600253A RID: 9530
		bool CanJump();

		// Token: 0x0600253B RID: 9531
		IGameInput GetGameInput();

		// Token: 0x0600253C RID: 9532
		void EndActiveMove(MoveEndType endType, bool processBufferedInput = true, bool transitioningToContinuingMove = false);

		// Token: 0x0600253D RID: 9533
		void SetRotation(HorizontalDirection direction, bool allowMirror = true);

		// Token: 0x0600253E RID: 9534
		void SetFacing(HorizontalDirection direction);

		// Token: 0x0600253F RID: 9535
		void SetFacingAndRotation(HorizontalDirection direction);

		// Token: 0x06002540 RID: 9536
		void BeginStun(int frames, StunType stunType, bool cannotTech, bool isSpike, Fixed knockbackForce, Vector2F knockbackVelocity);

		// Token: 0x06002541 RID: 9537
		bool HasAnimationOverride(ActionState actionState, HorizontalDirection facing, ref string animName);

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06002542 RID: 9538
		bool PreventActionStateAnimations { get; }

		// Token: 0x06002543 RID: 9539
		void DismountRespawnPlatform();

		// Token: 0x06002544 RID: 9540
		bool TryBeginMove(InputButtonsData inputButtonsData);

		// Token: 0x06002545 RID: 9541
		bool TryBeginMove(MoveData moveData, InterruptData interrupt, ButtonPress buttonUsed, InputButtonsData inputButtonsData);

		// Token: 0x06002546 RID: 9542
		bool TryBeginBufferedInterrupt(InputButtonsData inputButtonsData, bool isHighPriority);

		// Token: 0x06002547 RID: 9543
		void Cache(InputButtonsData inputButtonsData);

		// Token: 0x06002548 RID: 9544
		List<ButtonPress> GetBufferableInput(InputButtonsData inputButtonsData);

		// Token: 0x06002549 RID: 9545
		InputButtonsData ProcessInput(bool retainBuffer);

		// Token: 0x0600254A RID: 9546
		bool ProcessBufferedInput();

		// Token: 0x0600254B RID: 9547
		void ClearBufferedInput();
	}
}
