// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerDelegate : IItemHolder, IPlayerInputActor, IPositionOwner, IPlayerDataOwner, PlayerStateActor.IPlayerActorDelegate, IFacing
{
	SkinData SkinData
	{
		get;
	}

	PlayerNum PlayerNum
	{
		get;
	}

	TeamNum Team
	{
		get;
	}

	Transform Transform
	{
		get;
	}

	bool IsActive
	{
		get;
		set;
	}

	List<ICharacterComponent> Components
	{
		get;
	}

	bool IsLedgeGrabbing
	{
		get;
	}

	bool IsOffstage
	{
		get;
	}

	bool IsLedgeGrabbingBlocked
	{
		get;
	}

	bool IsRotationRolled
	{
		get;
	}

	IBodyOwner Body
	{
		get;
	}

	IMoveInput PlayerInput
	{
		get;
	}

	IInvincibilityController Invincibility
	{
		get;
	}

	TrailEmitter TrailEmitter
	{
		get;
	}

	TrailEmitter KnockbackEmitter
	{
		get;
	}

	StaleMoveQueue StaleMoves
	{
		get;
	}

	IMoveUseTracker MoveUseTracker
	{
		get;
	}

	IPlayerStateActor StateActor
	{
		get;
	}

	Dictionary<HitBoxState, CapsuleShape> HitCapsules
	{
		get;
	}

	IMoveSet MoveSet
	{
		get;
	}

	ICombatController Combat
	{
		get;
	}

	IFrameOwner FrameOwner
	{
		get;
	}

	ConfigData Config
	{
		get;
	}

	InputController InputController
	{
		get;
	}

	IAudioOwner AudioOwner
	{
		get;
	}

	GrabData GrabData
	{
		get;
	}

	bool IsHitActive(Hit hit, IHitOwner other, bool excludePhantomHitbox);

	T GetCharacterComponent<T>() where T : class;

	bool ExecuteCharacterComponents<T>(PlayerController.ComponentExecution<T> execute);

	bool IsStandingOnStageSurface(out RaycastHitData surfaceHit);

	bool CanWallJump(HorizontalDirection wallJumpDirection);

	void RestartCurrentActionState(bool startAnimationAtCurrentFrame);

	void PlayHologram(HologramData hologramData);

	void PlayVoiceTaunt(VoiceTauntData voiceTauntData);

	void SetMove(MoveData move, InputButtonsData inputButtonsData, HorizontalDirection inputDirection = HorizontalDirection.None, int uid = 0, int startFrame = 0, Vector3F assistTarget = default(Vector3F), MoveTransferSettings transferSettings = null, List<MoveLinkComponentData> linkComponentData = null, MoveSeedData seedData = default(MoveSeedData), ButtonPress buttonUsed = ButtonPress.None);

	void StaleMove(MoveLabel move, string name, int uid);

	void OnDamageDealt(Fixed damage, ImpactType impactType, bool chargesMeter);

	void LoadInstanceData(IPlayerDelegate other);

	void AddHostedHit(HostedHit hit);

	void AddHostedMaterialAnimation(MaterialAnimationTrigger material);

	void ForceGetUp();

	void CharacterDeath(bool isTotemPartner);

	void OnFlinched();

	void OnGrabbed();

	void DispatchInteraction(PlayerController.InteractionSignalData.Type type);

	void ReduceStunFrames(int frames);
}
