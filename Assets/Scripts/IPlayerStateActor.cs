// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface IPlayerStateActor
{
	void StartCharacterAction(ActionState action, string overrideAnimation = null, string overrideLeftAnimation = null, bool cancelsMove = true, int actionStateFrame = 0, bool neverRotateFacing = false);

	void StartCharacterAction(ActionState action, Fixed overrideSpeed, string overrideAnimation = null, string overrideLeftAnimation = null, bool cancelsMove = true, int actionStateFrame = 0, bool neverRotateFacing = false);

	void TickActionState();

	void BeginDaze();

	bool AttemptJump(ButtonPress buttonSource);

	bool AttemptWavedash();

	bool AttemptRecoveryJump();

	bool TryBeginShield(bool force = false);

	bool TryResumeShield();

	bool TryBeginCrouching();

	bool TryBeginBraking();

	void BeginDashing(HorizontalDirection direction);

	void BeginDashPivot(HorizontalDirection direction);

	void BeginPivot(HorizontalDirection direction);

	void BeginRunPivot(HorizontalDirection direction);

	void BeginWalking(Fixed horizontalAxisValue);

	void BeginIdling();

	void BeginFalling(ActionState fallActionState, bool startAnimationAtCurrentFrame = false);

	void BeginDowned(ref Vector3F previousVelocity);

	void BeginTeetering();

	void BeginPlatformDrop(IPhysicsCollider collider);

	void RestartCurrentActionState(bool startAnimationAtCurrentFrame);

	void ReleaseFromGrab();

	void BreakShield();

	void ReleaseShieldBreak();

	void CheckShielding();

	void ReleaseShield(bool playShieldEnd, bool changeState);

	void ReleaseStun();
}
