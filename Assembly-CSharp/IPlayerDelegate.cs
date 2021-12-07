using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x020005D7 RID: 1495
public interface IPlayerDelegate : IItemHolder, IPlayerInputActor, IPositionOwner, IPlayerDataOwner, PlayerStateActor.IPlayerActorDelegate, IFacing
{
	// Token: 0x17000778 RID: 1912
	// (get) Token: 0x060021A7 RID: 8615
	SkinData SkinData { get; }

	// Token: 0x17000779 RID: 1913
	// (get) Token: 0x060021A8 RID: 8616
	PlayerNum PlayerNum { get; }

	// Token: 0x1700077A RID: 1914
	// (get) Token: 0x060021A9 RID: 8617
	TeamNum Team { get; }

	// Token: 0x060021AA RID: 8618
	bool IsHitActive(Hit hit, IHitOwner other, bool excludePhantomHitbox);

	// Token: 0x1700077B RID: 1915
	// (get) Token: 0x060021AB RID: 8619
	Transform Transform { get; }

	// Token: 0x1700077C RID: 1916
	// (get) Token: 0x060021AC RID: 8620
	// (set) Token: 0x060021AD RID: 8621
	bool IsActive { get; set; }

	// Token: 0x1700077D RID: 1917
	// (get) Token: 0x060021AE RID: 8622
	List<ICharacterComponent> Components { get; }

	// Token: 0x060021AF RID: 8623
	T GetCharacterComponent<T>() where T : class;

	// Token: 0x060021B0 RID: 8624
	bool ExecuteCharacterComponents<T>(PlayerController.ComponentExecution<T> execute);

	// Token: 0x1700077E RID: 1918
	// (get) Token: 0x060021B1 RID: 8625
	bool IsLedgeGrabbing { get; }

	// Token: 0x060021B2 RID: 8626
	bool IsStandingOnStageSurface(out RaycastHitData surfaceHit);

	// Token: 0x1700077F RID: 1919
	// (get) Token: 0x060021B3 RID: 8627
	bool IsOffstage { get; }

	// Token: 0x060021B4 RID: 8628
	bool CanWallJump(HorizontalDirection wallJumpDirection);

	// Token: 0x17000780 RID: 1920
	// (get) Token: 0x060021B5 RID: 8629
	bool IsLedgeGrabbingBlocked { get; }

	// Token: 0x060021B6 RID: 8630
	void RestartCurrentActionState(bool startAnimationAtCurrentFrame);

	// Token: 0x17000781 RID: 1921
	// (get) Token: 0x060021B7 RID: 8631
	bool IsRotationRolled { get; }

	// Token: 0x060021B8 RID: 8632
	void PlayHologram(HologramData hologramData);

	// Token: 0x060021B9 RID: 8633
	void PlayVoiceTaunt(VoiceTauntData voiceTauntData);

	// Token: 0x17000782 RID: 1922
	// (get) Token: 0x060021BA RID: 8634
	IBodyOwner Body { get; }

	// Token: 0x17000783 RID: 1923
	// (get) Token: 0x060021BB RID: 8635
	IMoveInput PlayerInput { get; }

	// Token: 0x17000784 RID: 1924
	// (get) Token: 0x060021BC RID: 8636
	IInvincibilityController Invincibility { get; }

	// Token: 0x17000785 RID: 1925
	// (get) Token: 0x060021BD RID: 8637
	TrailEmitter TrailEmitter { get; }

	// Token: 0x17000786 RID: 1926
	// (get) Token: 0x060021BE RID: 8638
	TrailEmitter KnockbackEmitter { get; }

	// Token: 0x17000787 RID: 1927
	// (get) Token: 0x060021BF RID: 8639
	StaleMoveQueue StaleMoves { get; }

	// Token: 0x17000788 RID: 1928
	// (get) Token: 0x060021C0 RID: 8640
	IMoveUseTracker MoveUseTracker { get; }

	// Token: 0x17000789 RID: 1929
	// (get) Token: 0x060021C1 RID: 8641
	IPlayerStateActor StateActor { get; }

	// Token: 0x1700078A RID: 1930
	// (get) Token: 0x060021C2 RID: 8642
	Dictionary<HitBoxState, CapsuleShape> HitCapsules { get; }

	// Token: 0x1700078B RID: 1931
	// (get) Token: 0x060021C3 RID: 8643
	IMoveSet MoveSet { get; }

	// Token: 0x1700078C RID: 1932
	// (get) Token: 0x060021C4 RID: 8644
	ICombatController Combat { get; }

	// Token: 0x1700078D RID: 1933
	// (get) Token: 0x060021C5 RID: 8645
	IFrameOwner FrameOwner { get; }

	// Token: 0x1700078E RID: 1934
	// (get) Token: 0x060021C6 RID: 8646
	ConfigData Config { get; }

	// Token: 0x1700078F RID: 1935
	// (get) Token: 0x060021C7 RID: 8647
	InputController InputController { get; }

	// Token: 0x17000790 RID: 1936
	// (get) Token: 0x060021C8 RID: 8648
	IAudioOwner AudioOwner { get; }

	// Token: 0x060021C9 RID: 8649
	void SetMove(MoveData move, InputButtonsData inputButtonsData, HorizontalDirection inputDirection = HorizontalDirection.None, int uid = 0, int startFrame = 0, Vector3F assistTarget = default(Vector3F), MoveTransferSettings transferSettings = null, List<MoveLinkComponentData> linkComponentData = null, MoveSeedData seedData = default(MoveSeedData), ButtonPress buttonUsed = ButtonPress.None);

	// Token: 0x060021CA RID: 8650
	void StaleMove(MoveLabel move, string name, int uid);

	// Token: 0x060021CB RID: 8651
	void OnDamageDealt(Fixed damage, ImpactType impactType, bool chargesMeter);

	// Token: 0x060021CC RID: 8652
	void LoadInstanceData(IPlayerDelegate other);

	// Token: 0x060021CD RID: 8653
	void AddHostedHit(HostedHit hit);

	// Token: 0x060021CE RID: 8654
	void AddHostedMaterialAnimation(MaterialAnimationTrigger material);

	// Token: 0x060021CF RID: 8655
	void ForceGetUp();

	// Token: 0x060021D0 RID: 8656
	void CharacterDeath(bool isTotemPartner);

	// Token: 0x060021D1 RID: 8657
	void OnFlinched();

	// Token: 0x060021D2 RID: 8658
	void OnGrabbed();

	// Token: 0x060021D3 RID: 8659
	void DispatchInteraction(PlayerController.InteractionSignalData.Type type);

	// Token: 0x060021D4 RID: 8660
	void ReduceStunFrames(int frames);

	// Token: 0x17000791 RID: 1937
	// (get) Token: 0x060021D5 RID: 8661
	GrabData GrabData { get; }
}
