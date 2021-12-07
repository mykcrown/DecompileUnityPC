// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class PlayerState : IPlayerState
{
	private IPlayerDataOwner data;

	private IPlayerStateActor actor;

	private IFrameOwner frameOwner;

	private ConfigData config;

	private IBattleServerAPI battleServerAPI;

	bool IPlayerState.CanMove
	{
		get
		{
			return !this.IsDownState && !this.IsLedgeHangingState && !this.IsCrouching && !this.IsBeginningCrouching && !this.IsGrabbedState && !this.IsStandardGrabbingState && !this.IsShieldingState && !this.IsHitLagPaused && !this.IsStunned && !this.IsDead && !this.IsDashPivoting && (!this.IsBusyWithMove || !this.isBlockMovement() || (this.data.ActiveMove.Data.allowReversal && this.data.ActiveMove.Model.internalFrame <= this.data.ActiveMove.Data.reversalFrames)) && !this.IsDownState && !this.actionBlocksStateChange(this.ActionState, false) && !this.IsReleasingShield && !this.IsBusyRespawning;
		}
	}

	bool IPlayerState.IsBlockMovement
	{
		get
		{
			return this.isBlockMovement();
		}
	}

	bool IPlayerState.IsBlockFastFall
	{
		get
		{
			if (this.isBlockMovement())
			{
				return true;
			}
			BlockMovementData[] blockMovementData = this.data.ActiveMove.Data.blockMovementData;
			for (int i = 0; i < blockMovementData.Length; i++)
			{
				BlockMovementData blockMovementData2 = blockMovementData[i];
				if (this.data.ActiveMove.Model.internalFrame >= blockMovementData2.startFrame && this.data.ActiveMove.Model.internalFrame <= blockMovementData2.endFrame && blockMovementData2.blockFastFall)
				{
					return true;
				}
			}
			return false;
		}
	}

	bool IPlayerState.CanGrabLedge
	{
		get
		{
			return !this.IsGrounded && !this.IsStunned && ((this.data.GameInput.VerticalAxisValue >= 0 && !this.IsBusyWithMove) || this.IsHelpless || (this.IsBusyWithMove && this.data.ActiveMove.IsActive && this.data.ActiveMove.IsLedgeGrabEnabled)) && !this.data.LedgeGrabController.IsLedgeGrabbing && this.frameOwner.Frame - this.data.Model.ledgeReleaseFrame > this.config.ledgeConfig.ledgeCooldownFrames;
		}
	}

	bool IPlayerState.CanDropThroughPlatform
	{
		get
		{
			return (!this.IsBusyWithMove && (!this.IsLanding || !this.IsBusyWithAction) && !this.IsShieldingState && !this.IsReleasingShield && !this.IsTakingOff && !this.IsJumpingState && !this.IsDownState && !this.IsDownedLooping) || this.IsShieldingState;
		}
	}

	bool IPlayerState.ShouldUseRecoveryJump
	{
		get
		{
			return this.data.Physics.UsedAirJump && this.data.Physics.Velocity.y <= 0 && !this.data.Physics.IsAboveStage;
		}
	}

	bool IPlayerState.CanReleaseLedge
	{
		get
		{
			return !this.IsLedgeGrabbing && this.IsLedgeHangingState && this.data.Model.ledgeLagFrames <= 0;
		}
	}

	bool IPlayerState.CanFallThroughPlatforms
	{
		get
		{
			return this.IsHelpless || this.IsFalling || this.IsAirJumping || this.IsJumpingUp || this.IsPlatformDropping;
		}
	}

	bool IPlayerState.CanDieOffTop
	{
		get
		{
			return this.data.Physics.KnockbackVelocity.sqrMagnitude > 0 && (this.IsTumbling || this.data.State.IsStunned);
		}
	}

	bool IPlayerState.CanBeginWalking
	{
		get
		{
			return (!this.IsBusyWithMove || (!this.isBlockMovement() && !this.IsGrounded)) && !this.IsHelpless && !this.IsStunned && !this.IsDashing && !this.IsRunning && !this.IsWalking && !this.IsBraking && !this.IsDashBraking && !this.IsRunPivoting && !this.IsPivoting && !this.IsCrouching && this.IsGrounded && !this.IsJumpingState && !this.CanBeginShield && !this.IsReleasingShield && !this.IsTakingOff;
		}
	}

	bool IPlayerState.CanBeginFalling
	{
		get
		{
			return !this.IsGrounded && !this.IsTumbling && !this.IsHitStunned && !this.IsStunned && !this.IsLedgeHangingState && !this.IsGrabbedState && !this.IsFalling && !this.IsReleasingShield && !this.data.LedgeGrabController.IsLedgeGrabbing && (!this.data.ActiveMove.IsActive || this.data.ActiveMove.CancelOnFall) && this.ShouldPlayFallOrLandAction && !this.IsHelpless && !this.IsJumpingUp && !this.IsAirJumping && !this.IsRespawning && !this.IsDead && !this.IsGrabEscaping && !this.IsPlatformDropping;
		}
	}

	bool IPlayerState.CanResumeGrabbing
	{
		get
		{
			return !this.IsMoveActive && this.IsStandardGrabbingState && !this.IsGrabbing;
		}
	}

	bool IPlayerState.CanEndCrouch
	{
		get
		{
			return this.IsCrouching && !this.IsBusyWithMove;
		}
	}

	bool IPlayerState.CanBeginBraking
	{
		get
		{
			return this.IsRunning && !this.IsBraking && !this.IsDashBraking && (!this.data.GameInput.IsCrouchingInputPressed || !this.CanBeginCrouching);
		}
	}

	bool IPlayerState.CanBeginTeetering
	{
		get
		{
			return !this.IsTeetering && this.data.Physics.TeeteringDirection == this.data.Facing && !this.IsMoveActive && (this.IsWalking || this.IsIdling || this.IsBraking || this.IsDashBraking);
		}
	}

	bool IPlayerState.CanReleaseShieldVoluntarily
	{
		get
		{
			return !this.IsTakingOff && !this.IsLedgeHangingState && !this.data.Shield.IsGusting && !this.IsStunned && !this.IsBeginningShield;
		}
	}

	bool IPlayerState.IsInControl
	{
		get
		{
			return !this.IsDownState && (!this.IsHelpless && !this.IsStunned);
		}
	}

	public ActionState ActionState
	{
		get
		{
			return this.data.Model.actionState;
		}
		set
		{
			this.data.Model.actionState = value;
		}
	}

	public MetaState MetaState
	{
		get
		{
			return this.data.Model.state;
		}
		set
		{
			this.setState(value);
		}
	}

	public SubStates SubState
	{
		get
		{
			return this.data.Model.subState;
		}
		set
		{
			this.setSubState(value);
		}
	}

	public int ActionStateFrame
	{
		get
		{
			return this.data.Model.actionStateFrame;
		}
	}

	public bool IsShieldingState
	{
		get
		{
			return this.MetaState == MetaState.Shielding;
		}
	}

	public bool IsStandingState
	{
		get
		{
			return this.MetaState == MetaState.Stand;
		}
	}

	public bool IsJumpingState
	{
		get
		{
			return this.MetaState == MetaState.Jump;
		}
	}

	public bool IsGrabbedState
	{
		get
		{
			return this.MetaState == MetaState.Grabbed;
		}
	}

	public bool IsStandardGrabbingState
	{
		get
		{
			return this.MetaState == MetaState.StandardGrabbing;
		}
	}

	public bool IsLedgeHangingState
	{
		get
		{
			return this.MetaState == MetaState.LedgeHang;
		}
	}

	public bool IsDownState
	{
		get
		{
			return this.MetaState == MetaState.Down;
		}
	}

	public bool IsCrouching
	{
		get
		{
			return this.ActionState == ActionState.Crouching;
		}
	}

	public bool IsBeginningCrouching
	{
		get
		{
			return this.ActionState == ActionState.CrouchBegin;
		}
	}

	public bool IsEndCrouching
	{
		get
		{
			return this.ActionState == ActionState.CrouchEnd;
		}
	}

	public bool IsBraking
	{
		get
		{
			return this.ActionState == ActionState.Brake;
		}
	}

	public bool IsDashBraking
	{
		get
		{
			return this.ActionState == ActionState.DashBrake;
		}
	}

	public bool IsRunPivotBraking
	{
		get
		{
			return this.ActionState == ActionState.RunPivotBrake;
		}
	}

	public bool IsJumpingUp
	{
		get
		{
			return this.ActionState == ActionState.JumpStraight;
		}
	}

	public bool IsAirJumping
	{
		get
		{
			return this.ActionState == ActionState.AirJump;
		}
	}

	public bool IsFalling
	{
		get
		{
			return this.ActionState == ActionState.FallStraight || this.ActionState == ActionState.FallForward || this.ActionState == ActionState.FallBack;
		}
	}

	public bool IsRunPivoting
	{
		get
		{
			return this.ActionState == ActionState.RunPivot;
		}
	}

	public bool IsPivoting
	{
		get
		{
			return this.ActionState == ActionState.Pivot;
		}
	}

	public bool IsLanding
	{
		get
		{
			return this.ActionState == ActionState.Landing;
		}
	}

	public bool IsTakingOff
	{
		get
		{
			return this.ActionState == ActionState.TakeOff;
		}
	}

	public bool IsRecoiling
	{
		get
		{
			return this.ActionState == ActionState.Recoil;
		}
	}

	public bool IsRunning
	{
		get
		{
			return this.ActionState == ActionState.Run;
		}
	}

	public bool IsDashing
	{
		get
		{
			return this.ActionState == ActionState.Dash;
		}
	}

	public bool IsDashPivoting
	{
		get
		{
			return this.ActionState == ActionState.DashPivot;
		}
	}

	public bool IsIdling
	{
		get
		{
			return this.ActionState == ActionState.Idle;
		}
	}

	public bool IsWalking
	{
		get
		{
			return this.ActionState == ActionState.WalkFast || this.ActionState == ActionState.WalkSlow || this.ActionState == ActionState.WalkMedium;
		}
	}

	public bool IsReleasingShield
	{
		get
		{
			return this.ActionState == ActionState.ShieldEnd;
		}
	}

	public bool IsLedgeGrabbing
	{
		get
		{
			return this.ActionState == ActionState.EdgeGrab;
		}
	}

	public bool IsLedgeHanging
	{
		get
		{
			return this.ActionState == ActionState.EdgeHang;
		}
	}

	public bool IsTeetering
	{
		get
		{
			return this.ActionState == ActionState.TeeterLoop || this.ActionState == ActionState.TeeterBegin;
		}
	}

	public bool IsGrabbing
	{
		get
		{
			return this.ActionState == ActionState.Grabbing;
		}
	}

	public bool IsGrabReleasing
	{
		get
		{
			return this.ActionState == ActionState.GrabRelease;
		}
	}

	public bool IsGrabEscaping
	{
		get
		{
			return this.ActionState == ActionState.GrabEscapeGround || this.ActionState == ActionState.GrabEscapeAir;
		}
	}

	public bool IsDazed
	{
		get
		{
			return this.ActionState == ActionState.DazedBegin || this.ActionState == ActionState.DazedLoop || this.ActionState == ActionState.DazedEnd;
		}
	}

	public bool IsDownedLooping
	{
		get
		{
			return this.ActionState == ActionState.DownedLoop;
		}
	}

	public bool IsThrown
	{
		get
		{
			return this.ActionState == ActionState.Thrown;
		}
	}

	public bool IsTumbling
	{
		get
		{
			ActionState actionState = this.ActionState;
			switch (actionState)
			{
			case ActionState.HitTumbleHigh:
			case ActionState.HitTumbleLow:
			case ActionState.HitTumbleNeutral:
			case ActionState.HitTumbleSpin:
			case ActionState.HitTumbleTop:
				break;
			default:
				if (actionState != ActionState.Tumble)
				{
					return false;
				}
				break;
			}
			return true;
		}
	}

	public bool IsBeginningShield
	{
		get
		{
			return this.ActionState == ActionState.ShieldBegin;
		}
	}

	public bool IsPlatformDropping
	{
		get
		{
			return this.data.Physics.PlayerState.platformDropFrames > 0;
		}
	}

	public bool IsHitStunned
	{
		get
		{
			switch (this.ActionState)
			{
			case ActionState.HitStunAirS:
			case ActionState.HitStunAirM:
			case ActionState.HitStunAirL:
			case ActionState.HitStunGroundS:
			case ActionState.HitStunGroundM:
			case ActionState.HitStunGroundL:
			case ActionState.HitStunMeteorS:
			case ActionState.HitStunMeteorM:
			case ActionState.HitStunMeteorL:
				return true;
			default:
				return false;
			}
		}
	}

	public bool CanUseMoves
	{
		get
		{
			return !this.IsHelpless && !this.IsStunned && !this.IsLedgeGrabbing && !this.IsDead && !this.actionBlocksStateChange(this.ActionState, false) && !this.IsBusyRespawning && !this.IsReadyToGrabRelease;
		}
	}

	public bool CanUseEmotes
	{
		get
		{
			return this.data.Model.emoteCooldownFrames <= 0 && (!this.config.tauntSettings.useEmotesPerTime || !this.battleServerAPI.IsSinglePlayerNetworkGame || this.data.Model.emoteFrameLimitStart == 0 || this.frameOwner.Frame - this.data.Model.emoteFrameLimitStart > this.config.tauntSettings.emotesPerTimeFrames || this.data.Model.emoteFrameLimitCounter < this.config.tauntSettings.emotesPerTimeMax);
		}
	}

	public bool CanJump
	{
		get
		{
			return !this.IsHitLagPaused && (!this.IsBusyWithMove || this.data.ActiveMove.Model.IsInterruptibleByAction(this.data, PlayerMovementAction.Jump)) && !this.IsHelpless && !this.IsDownState && !this.IsShieldBroken && !this.IsLedgeHangingState && !this.IsTakingOff && !this.IsGrabbedState && !this.IsDead && !this.IsBusyRespawning && !this.actionBlocksStateChange(this.ActionState, false) && (this.IsGrounded || !this.data.Physics.UsedAirJump) && (!this.IsGrounded || !this.data.Physics.UsedGroundJump) && ((!this.IsJumpStunned && !this.IsGrounded) || (!this.IsStunned && this.IsGrounded));
		}
	}

	public bool CanBeginIdling
	{
		get
		{
			return !this.IsHitLagPaused && !this.IsDead && !this.IsStunned && !this.IsHitStunned && this.IsGrounded && !this.data.ActiveMove.IsActive && !this.IsIdling && !this.IsCrouching && !this.IsBeginningCrouching && !this.IsEndCrouching && !this.IsShieldingState && !this.IsStandardGrabbingState && !this.IsGrabbedState && !this.IsDashing && !this.IsRunning && !this.IsRunPivoting && !this.IsDashPivoting && !this.IsPivoting && !this.IsDownState && !this.CanBeginShield && !this.IsShieldBroken && !this.IsTakingOff && !this.IsBraking && !this.IsDashBraking && !this.IsRunPivotBraking && !this.IsReleasingShield && !this.actionBlocksStateChange(this.ActionState, true) && !this.IsTeetering && (!this.IsHelpless || Vector3F.Dot(this.data.Physics.Velocity.normalized, this.data.Physics.GroundedNormal) < (Fixed)0.1);
		}
	}

	public bool CanBeginCrouching
	{
		get
		{
			return (!this.IsBusyWithMove || this.data.ActiveMove.Model.IsInterruptibleByAction(this.data, PlayerMovementAction.Crouch)) && this.IsGrounded && this.IsStandingState && !this.CanBeginShield && !this.IsStunned && !this.IsShieldingState && !this.IsCrouching && !this.IsBeginningCrouching && !this.IsEndCrouching && !this.IsTakingOff && !this.IsDashing && !this.IsDashPivoting && !this.IsReleasingShield && !this.actionBlocksStateChange(this.ActionState, false) && !this.IsHelpless && (!this.IsRunPivoting || !this.IsBusyWithAction) && !this.IsRespawning;
		}
	}

	public bool CanMaintainShield
	{
		get
		{
			return (!this.IsBusyWithMove || this.data.ActiveMove.Data.label == MoveLabel.ShieldGust) && this.IsGrounded && this.IsShieldingState && !this.IsShieldBroken && !this.IsTakingOff && (!this.IsStunned || this.data.Model.stunType == StunType.ShieldStun);
		}
	}

	public bool CanBeginShield
	{
		get
		{
			return this.data.GameInput.IsShieldInputPressed && !this.IsDead && !this.IsHitLagPaused && !this.IsStunned && !this.IsShieldingState && this.IsStandingState && !this.IsTakingOff && (!this.IsLanding || !this.IsBusyWithAction) && this.IsGrounded && (!this.IsBusyWithMove || this.data.ActiveMove.Model.IsInterruptibleByAction(this.data, PlayerMovementAction.Shield)) && !this.IsShieldingState && !this.IsReleasingShield && !this.IsRecoiling && !this.IsRunPivoting && !this.IsDashPivoting && !this.actionBlocksStateChange(this.ActionState, false) && !this.IsRespawning;
		}
	}

	public bool IsTechOffCooldown
	{
		get
		{
			return this.frameOwner.Frame - this.data.Model.lastTechFrame < this.config.knockbackConfig.techInputThresholdFrames;
		}
	}

	public bool IsTechableMode
	{
		get
		{
			return this.data.Model.stunTechMode == StunTechMode.Techable || (this.data.Model.stunTechMode == StunTechMode.FirstBounceUntechable && this.data.Model.untechableBounceUsed);
		}
	}

	public bool IsInUntechableStun
	{
		get
		{
			return this.IsStunned && (this.data.Model.stunType == StunType.ShieldStun || this.data.Model.stunType == StunType.ShieldBreakStun || (!this.config.knockbackConfig.enableSdiTeching && this.IsHitLagPaused));
		}
	}

	public bool IsRecovered
	{
		get
		{
			return this.IsIdling && this.CanUseMoves;
		}
	}

	public bool IsBusyWithAction
	{
		get
		{
			return this.data.ActionData != null && (this.data.ActionData.wrapMode == WrapMode.Loop || this.data.Model.actionStateFrame < this.data.AnimationPlayer.CurrentAnimationGameFramelength - Mathf.Max(this.data.Model.overrideActionStateInterruptibilityFrames, this.data.ActionData.interruptibleFrames));
		}
	}

	public bool IsBusyWithMove
	{
		get
		{
			return this.data.ActiveMove.IsActive && !this.data.ActiveMove.Model.IsInterruptibleByAnything(this.data);
		}
	}

	public bool IsMoveActive
	{
		get
		{
			return this.data.ActiveMove.IsActive;
		}
	}

	public bool IsStunned
	{
		get
		{
			return this.data.Model.stunFrames > 0;
		}
	}

	public bool IsJumpStunned
	{
		get
		{
			return this.data.Model.jumpStunFrames > 0;
		}
	}

	public bool IsHitLagPaused
	{
		get
		{
			return this.data.Model.hitLagFrames > 0 && this.data.Model.ignoreHitLagFrames <= 0;
		}
	}

	public bool IsUnderChainGrabPrevention
	{
		get
		{
			return this.data.Model.chainGrabPreventionFrames > 0;
		}
	}

	public bool IsCameraFlourishMode
	{
		get
		{
			return this.IsHitLagPaused && this.data.Model.isKillCamHitlag;
		}
	}

	public bool IsCameraZoomMode
	{
		get
		{
			return this.IsHitLagPaused && this.data.Model.isCameraZoomHitLag;
		}
	}

	public bool IsGrounded
	{
		get
		{
			return this.data.Physics.IsGrounded;
		}
	}

	public bool IsShieldBroken
	{
		get
		{
			return this.data.Shield.IsBroken;
		}
	}

	public bool IsReadyToGrabRelease
	{
		get
		{
			return false;
		}
	}

	public bool IsHelpless
	{
		get
		{
			return this.data.Model.subState == SubStates.Helpless;
		}
	}

	public bool IsBusyRespawning
	{
		get
		{
			return this.IsRespawning && !this.data.RespawnController.HasArrived;
		}
	}

	public bool IsRespawning
	{
		get
		{
			return this.data.Model.isRespawning;
		}
	}

	public bool IsDead
	{
		get
		{
			return this.data.Model.isDead;
		}
	}

	public bool IsImmuneToBlastZone
	{
		get
		{
			return this.data.Model.blastZoneImmunityFrames > 0;
		}
	}

	public bool IsAffectedByUnflinchingKnockback
	{
		get
		{
			return !this.IsLedgeHangingState && (!this.data.ActiveMove.IsActive || !this.data.ActiveMove.Data.IsLedgeRecovery);
		}
	}

	public bool IsLedgeRecovering
	{
		get
		{
			if (!this.data.ActiveMove.IsActive)
			{
				return false;
			}
			switch (this.data.ActiveMove.Data.label)
			{
			case MoveLabel.LedgeAttack:
			case MoveLabel.LedgeStand:
			case MoveLabel.LedgeRoll:
				return true;
			}
			return false;
		}
	}

	public bool IsWallJumping
	{
		get
		{
			return this.data.ActiveMove.IsActive && this.data.ActiveMove.Data.GetComponent<WallJumpComponent>() != null;
		}
	}

	public bool ShouldIgnoreForces
	{
		get
		{
			return this.data.LedgeGrabController.IsLedgeGrabbing || this.IsGrabbedState || this.IsRespawning || this.IsHitLagPaused || this.IsDead;
		}
	}

	public bool ShouldPlayFallOrLandAction
	{
		get
		{
			return !this.IsGrabReleasing && this.ActionState != ActionState.GrabEscapeGround;
		}
	}

	public PlayerState(IPlayerStateActor actor, IPlayerDataOwner data, ConfigData config, IBattleServerAPI battleServerAPI, IFrameOwner frameOwner)
	{
		this.actor = actor;
		this.data = data;
		this.config = config;
		this.frameOwner = frameOwner;
		this.battleServerAPI = battleServerAPI;
	}

	private bool isBlockMovement()
	{
		if (this.data.ActiveMove.Data.blockMovement)
		{
			return true;
		}
		BlockMovementData[] blockMovementData = this.data.ActiveMove.Data.blockMovementData;
		for (int i = 0; i < blockMovementData.Length; i++)
		{
			BlockMovementData blockMovementData2 = blockMovementData[i];
			if (this.data.ActiveMove.Model.internalFrame >= blockMovementData2.startFrame && this.data.ActiveMove.Model.internalFrame <= blockMovementData2.endFrame && blockMovementData2.blockAllMovement)
			{
				return true;
			}
		}
		return false;
	}

	bool IPlayerState.CanBeginPivot(HorizontalDirection direction)
	{
		return direction != this.data.Facing && this.IsGrounded && !this.IsJumpingState && !this.IsRunPivoting && !this.IsPivoting && !this.IsDashing && !this.IsShieldingState && !this.CanBeginShield && !this.IsReleasingShield && !this.IsBusyWithMove && !this.IsTakingOff;
	}

	bool IPlayerState.CanBeginRunPivot(HorizontalDirection newDirection)
	{
		return newDirection != this.data.Facing && this.IsGrounded && !this.IsJumpingState && (this.IsRunning || this.IsBraking) && !this.IsRunPivoting && !this.IsTakingOff;
	}

	bool IPlayerState.CanBeginDashing(HorizontalDirection direction)
	{
		return this.IsGrounded && !this.IsJumpingState && !this.IsRunning && (!this.IsBraking || !this.IsBusyWithAction) && (!this.IsRunPivoting || !this.IsBusyWithAction) && (!this.IsDashing || direction == this.data.OppositeFacing) && !this.CanBeginShield && !this.IsGrabbedState && !this.IsStandardGrabbingState && !this.IsBusyWithMove && !this.IsReleasingShield && !this.IsBeginningCrouching && !this.IsTakingOff;
	}

	public bool CanWallJump(HorizontalDirection wallJumpDirection)
	{
		if (!this.IsGrounded && this.frameOwner.Frame - this.data.Model.ledgeReleaseFrame >= this.config.wallJumpConfig.ledgeReleaseLockoutFrames && !this.IsWallJumping)
		{
			if (wallJumpDirection != HorizontalDirection.Left)
			{
				if (wallJumpDirection == HorizontalDirection.Right)
				{
					int num = this.data.Model.lastLeftInputFrame;
				}
			}
			else
			{
				int num = this.data.Model.lastRightInputFrame;
			}
			HorizontalDirection direction = InputUtils.GetDirection(this.data.Model.bufferedInput.inputButtonsData.horizontalAxisValue);
			return wallJumpDirection != direction;
		}
		return false;
	}

	public bool CanTech(SurfaceType surfaceType)
	{
		return this.IsTumbling && this.IsTechOffCooldown && !this.IsInUntechableStun && (surfaceType != SurfaceType.Floor || this.IsTechableMode);
	}

	private void setState(MetaState newState)
	{
		if (newState == this.data.Model.state)
		{
			return;
		}
		if (newState != MetaState.Down)
		{
			if (newState != MetaState.Stand)
			{
				if (newState != MetaState.Shielding)
				{
				}
			}
			else
			{
				this.data.Model.ClearLastHitData();
			}
		}
		else
		{
			this.data.Model.downedFrames = 0;
		}
		MetaState state = this.data.Model.state;
		if (state != MetaState.Down)
		{
			if (state == MetaState.Shielding)
			{
				if (newState != MetaState.Shielding)
				{
					this.actor.ReleaseShield(false, false);
				}
			}
		}
		else
		{
			this.data.Model.downedFrames = 0;
		}
		this.data.Model.state = newState;
	}

	private void setSubState(SubStates newSubState)
	{
		if (newSubState != SubStates.Helpless)
		{
			if (newSubState == SubStates.Tumbling)
			{
				this.data.Renderer.SetColorModeFlag(ColorMode.Tumbling, true);
			}
		}
		else
		{
			this.data.Renderer.SetColorModeFlag(ColorMode.Helpless, true);
		}
		SubStates subState = this.data.Model.subState;
		if (subState != SubStates.Helpless)
		{
			if (subState == SubStates.Tumbling)
			{
				this.data.Renderer.SetColorModeFlag(ColorMode.Tumbling, false);
			}
		}
		else
		{
			this.data.Renderer.SetColorModeFlag(ColorMode.Helpless, false);
		}
		this.data.Model.subState = newSubState;
	}

	private bool actionBlocksStateChange(ActionState characterAction, bool toIdle = false)
	{
		switch (characterAction)
		{
		case ActionState.GrabEscapeAir:
			return this.IsBusyWithAction;
		case ActionState.RunPivotBrake:
		case ActionState.WalkSlow:
		case ActionState.WalkMedium:
		case ActionState.TeeterBegin:
			IL_29:
			switch (characterAction)
			{
			case ActionState.DazedBegin:
			case ActionState.Recoil:
			case ActionState.GrabRelease:
			case ActionState.GrabEscapeGround:
				goto IL_6A;
			case ActionState.Pivot:
			case ActionState.ShieldEnd:
			case ActionState.ShieldBegin:
				IL_4E:
				if (characterAction != ActionState.Landing && characterAction != ActionState.EdgeGrab)
				{
					return false;
				}
				goto IL_6A;
			}
			goto IL_4E;
		case ActionState.DazedLoop:
		case ActionState.DazedEnd:
		case ActionState.FallDown:
			goto IL_6A;
		}
		goto IL_29;
		IL_6A:
		return toIdle || this.IsBusyWithAction;
	}
}
