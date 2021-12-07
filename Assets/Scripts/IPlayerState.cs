// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IPlayerState
{
	MetaState MetaState
	{
		get;
		set;
	}

	ActionState ActionState
	{
		get;
		set;
	}

	SubStates SubState
	{
		get;
		set;
	}

	bool CanEndCrouch
	{
		get;
	}

	bool CanDropThroughPlatform
	{
		get;
	}

	bool CanBeginWalking
	{
		get;
	}

	bool CanBeginIdling
	{
		get;
	}

	bool CanBeginCrouching
	{
		get;
	}

	bool CanBeginBraking
	{
		get;
	}

	bool CanBeginTeetering
	{
		get;
	}

	bool CanBeginFalling
	{
		get;
	}

	bool CanResumeGrabbing
	{
		get;
	}

	bool CanReleaseShieldVoluntarily
	{
		get;
	}

	bool CanMaintainShield
	{
		get;
	}

	bool CanBeginShield
	{
		get;
	}

	bool CanMove
	{
		get;
	}

	bool CanUseMoves
	{
		get;
	}

	bool CanUseEmotes
	{
		get;
	}

	bool CanJump
	{
		get;
	}

	bool CanReleaseLedge
	{
		get;
	}

	bool CanGrabLedge
	{
		get;
	}

	bool CanFallThroughPlatforms
	{
		get;
	}

	bool CanDieOffTop
	{
		get;
	}

	bool IsInUntechableStun
	{
		get;
	}

	bool IsTechOffCooldown
	{
		get;
	}

	bool IsTechableMode
	{
		get;
	}

	bool IsDownState
	{
		get;
	}

	bool IsGrabbedState
	{
		get;
	}

	bool IsStandardGrabbingState
	{
		get;
	}

	bool IsShieldingState
	{
		get;
	}

	bool IsStandingState
	{
		get;
	}

	bool IsJumpingState
	{
		get;
	}

	bool IsLedgeHangingState
	{
		get;
	}

	bool IsDashing
	{
		get;
	}

	bool IsDashPivoting
	{
		get;
	}

	bool IsTumbling
	{
		get;
	}

	bool IsLedgeGrabbing
	{
		get;
	}

	bool IsBraking
	{
		get;
	}

	bool IsDashBraking
	{
		get;
	}

	bool IsPivoting
	{
		get;
	}

	bool IsTakingOff
	{
		get;
	}

	bool IsLanding
	{
		get;
	}

	bool IsHelpless
	{
		get;
	}

	bool IsJumpingUp
	{
		get;
	}

	bool IsAirJumping
	{
		get;
	}

	bool IsCrouching
	{
		get;
	}

	bool IsRunning
	{
		get;
	}

	bool IsTeetering
	{
		get;
	}

	bool IsIdling
	{
		get;
	}

	bool IsWalking
	{
		get;
	}

	bool IsGrabReleasing
	{
		get;
	}

	bool IsGrabEscaping
	{
		get;
	}

	bool IsDazed
	{
		get;
	}

	bool IsRunPivoting
	{
		get;
	}

	bool IsDownedLooping
	{
		get;
	}

	bool IsHitStunned
	{
		get;
	}

	bool IsThrown
	{
		get;
	}

	bool IsPlatformDropping
	{
		get;
	}

	bool IsRespawning
	{
		get;
	}

	bool IsBusyRespawning
	{
		get;
	}

	bool IsHitLagPaused
	{
		get;
	}

	bool IsUnderChainGrabPrevention
	{
		get;
	}

	bool IsCameraFlourishMode
	{
		get;
	}

	bool IsCameraZoomMode
	{
		get;
	}

	bool IsBusyWithMove
	{
		get;
	}

	bool IsBlockMovement
	{
		get;
	}

	bool IsBlockFastFall
	{
		get;
	}

	bool IsBusyWithAction
	{
		get;
	}

	bool IsMoveActive
	{
		get;
	}

	bool IsFalling
	{
		get;
	}

	bool IsInControl
	{
		get;
	}

	bool IsRecovered
	{
		get;
	}

	bool IsGrounded
	{
		get;
	}

	bool IsStunned
	{
		get;
	}

	bool IsJumpStunned
	{
		get;
	}

	bool IsShieldBroken
	{
		get;
	}

	bool IsDead
	{
		get;
	}

	bool IsImmuneToBlastZone
	{
		get;
	}

	bool IsAffectedByUnflinchingKnockback
	{
		get;
	}

	bool IsLedgeRecovering
	{
		get;
	}

	bool IsLedgeHanging
	{
		get;
	}

	bool IsWallJumping
	{
		get;
	}

	bool ShouldIgnoreForces
	{
		get;
	}

	bool ShouldPlayFallOrLandAction
	{
		get;
	}

	bool ShouldUseRecoveryJump
	{
		get;
	}

	int ActionStateFrame
	{
		get;
	}

	bool CanBeginDashing(HorizontalDirection direction);

	bool CanBeginRunPivot(HorizontalDirection direction);

	bool CanBeginPivot(HorizontalDirection direction);

	bool CanWallJump(HorizontalDirection wallJumpDirection);

	bool CanTech(SurfaceType surfaceType);
}
