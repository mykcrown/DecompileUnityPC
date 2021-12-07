using System;
using FixedPoint;

// Token: 0x020005FD RID: 1533
public interface IPlayerStateActor
{
	// Token: 0x0600255F RID: 9567
	void StartCharacterAction(ActionState action, string overrideAnimation = null, string overrideLeftAnimation = null, bool cancelsMove = true, int actionStateFrame = 0, bool neverRotateFacing = false);

	// Token: 0x06002560 RID: 9568
	void StartCharacterAction(ActionState action, Fixed overrideSpeed, string overrideAnimation = null, string overrideLeftAnimation = null, bool cancelsMove = true, int actionStateFrame = 0, bool neverRotateFacing = false);

	// Token: 0x06002561 RID: 9569
	void TickActionState();

	// Token: 0x06002562 RID: 9570
	void BeginDaze();

	// Token: 0x06002563 RID: 9571
	bool AttemptJump(ButtonPress buttonSource);

	// Token: 0x06002564 RID: 9572
	bool AttemptWavedash();

	// Token: 0x06002565 RID: 9573
	bool AttemptRecoveryJump();

	// Token: 0x06002566 RID: 9574
	bool TryBeginShield(bool force = false);

	// Token: 0x06002567 RID: 9575
	bool TryResumeShield();

	// Token: 0x06002568 RID: 9576
	bool TryBeginCrouching();

	// Token: 0x06002569 RID: 9577
	bool TryBeginBraking();

	// Token: 0x0600256A RID: 9578
	void BeginDashing(HorizontalDirection direction);

	// Token: 0x0600256B RID: 9579
	void BeginDashPivot(HorizontalDirection direction);

	// Token: 0x0600256C RID: 9580
	void BeginPivot(HorizontalDirection direction);

	// Token: 0x0600256D RID: 9581
	void BeginRunPivot(HorizontalDirection direction);

	// Token: 0x0600256E RID: 9582
	void BeginWalking(Fixed horizontalAxisValue);

	// Token: 0x0600256F RID: 9583
	void BeginIdling();

	// Token: 0x06002570 RID: 9584
	void BeginFalling(ActionState fallActionState, bool startAnimationAtCurrentFrame = false);

	// Token: 0x06002571 RID: 9585
	void BeginDowned(ref Vector3F previousVelocity);

	// Token: 0x06002572 RID: 9586
	void BeginTeetering();

	// Token: 0x06002573 RID: 9587
	void BeginPlatformDrop(IPhysicsCollider collider);

	// Token: 0x06002574 RID: 9588
	void RestartCurrentActionState(bool startAnimationAtCurrentFrame);

	// Token: 0x06002575 RID: 9589
	void ReleaseFromGrab();

	// Token: 0x06002576 RID: 9590
	void BreakShield();

	// Token: 0x06002577 RID: 9591
	void ReleaseShieldBreak();

	// Token: 0x06002578 RID: 9592
	void CheckShielding();

	// Token: 0x06002579 RID: 9593
	void ReleaseShield(bool playShieldEnd, bool changeState);

	// Token: 0x0600257A RID: 9594
	void ReleaseStun();
}
