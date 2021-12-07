// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class GrabController : IGrabController
{
	private IPlayerDelegate player;

	private ConfigData config;

	private IFrameOwner frameOwner;

	private IPlayerLookup playerLookup;

	PlayerNum IGrabController.GrabbedOpponent
	{
		get
		{
			return this.GrabData.grabbedOpponent;
		}
	}

	PlayerNum IGrabController.GrabbingOpponent
	{
		get
		{
			return this.GrabData.grabbingOpponent;
		}
	}

	bool IGrabController.IsGrabbing
	{
		get
		{
			return this.GrabData.grabbedOpponent != PlayerNum.None;
		}
	}

	bool IGrabController.IsGrabbed
	{
		get
		{
			return this.GrabData.grabbingOpponent != PlayerNum.None;
		}
	}

	public GrabData GrabData
	{
		get
		{
			return this.player.Model.grabData;
		}
	}

	private PlayerController grabbingOpponent
	{
		get
		{
			return (this.GrabData.grabbingOpponent != PlayerNum.None) ? this.playerLookup.GetPlayerController(this.GrabData.grabbingOpponent) : null;
		}
	}

	private PlayerController grabbedOpponent
	{
		get
		{
			return (this.GrabData.grabbedOpponent != PlayerNum.None) ? this.playerLookup.GetPlayerController(this.GrabData.grabbedOpponent) : null;
		}
	}

	public bool IsReadyToRelease
	{
		get
		{
			return this.GrabData.grabType == GrabType.Normal && (!this.player.State.IsStandardGrabbingState || this.frameOwner.Frame - this.GrabData.grabbedStartFrame >= this.GrabData.grabDurationFrames);
		}
	}

	public GrabController(IPlayerDelegate player, ConfigData config, IMoveSet moveSet, IFrameOwner frameOwner, IPlayerLookup playerLookup)
	{
		this.player = player;
		this.config = config;
		this.frameOwner = frameOwner;
		this.playerLookup = playerLookup;
	}

	void IGrabController.OnGrabOpponent(PlayerController opponent, MoveData moveData, HitData hitData)
	{
		MoveData moveData2 = null;
		BodyPart grabbedBodyPart = BodyPart.upperTorso;
		if (hitData.overrideGrabbedBone != BodyPart.none)
		{
			grabbedBodyPart = hitData.overrideGrabbedBone;
		}
		this.player.EndActiveMove(MoveEndType.Cancelled, false, false);
		GrabType grabType = hitData.grabType;
		if (grabType != GrabType.None)
		{
			if (grabType != GrabType.Normal)
			{
				if (grabType == GrabType.Move)
				{
					moveData2 = hitData.onGrabMove;
				}
			}
			else
			{
				this.player.State.MetaState = MetaState.StandardGrabbing;
				this.GrabData.grabDurationFrames = this.config.grabConfig.baseDurationFrames + (int)(this.config.grabConfig.dmgScaling * opponent.Damage);
			}
			this.GrabData.grabbedOpponent = opponent.PlayerNum;
			this.GrabData.grabbedStartFrame = this.frameOwner.Frame;
			opponent.SetFacingAndRotation(this.player.OppositeFacing);
			this.GrabData.grabbedBodyPart = grabbedBodyPart;
			this.GrabData.grabType = hitData.grabType;
			this.GrabData.grabbedWithMoveName = moveData.moveName;
			this.GrabData.ignoreThrowBoneRotation = moveData.ignoreThrowBoneRotation;
			this.GrabData.victimUnderChainGrabPrevention = opponent.State.IsUnderChainGrabPrevention;
			this.player.Physics.StopMovement(this.player.State.IsGrounded, this.player.State.IsGrounded, VelocityType.Movement);
			if (this.grabbedOpponent != null && this.grabbedOpponent.GrabData.grabbedOpponent == this.player.PlayerNum)
			{
				this.grabbedOpponent.GrabController.ReleaseGrabbedOpponent(false);
				this.grabbedOpponent.StateActor.StartCharacterAction(ActionState.GrabRelease, null, null, true, 0, false);
				this.ReleaseGrabbedOpponent(false);
				this.player.StateActor.StartCharacterAction(ActionState.GrabRelease, null, null, true, 0, false);
			}
			else if (moveData2 != null)
			{
				this.player.SetMove(moveData2, InputButtonsData.EmptyInput, HorizontalDirection.None, 0, 1, default(Vector3F), null, null, default(MoveSeedData), ButtonPress.None);
			}
			else if (hitData.grabType == GrabType.Normal)
			{
				this.player.StateActor.StartCharacterAction(ActionState.Grabbing, null, null, true, 0, false);
			}
			return;
		}
		UnityEngine.Debug.LogError("Resolved unhandled grabType " + hitData.grabType);
	}

	public void ReleaseGrabbedOpponent(bool escape)
	{
		if (this.GrabData.grabbedOpponent == PlayerNum.None)
		{
			return;
		}
		this.player.Physics.StopMovement(true, true, VelocityType.Movement);
		if (escape)
		{
			this.player.StateActor.StartCharacterAction(ActionState.GrabRelease, null, null, true, 0, false);
		}
		this.player.Model.state = ((!this.player.Physics.IsGrounded) ? MetaState.Jump : MetaState.Stand);
		if (this.grabbedOpponent != null)
		{
			this.grabbedOpponent.GrabController.ReleaseFromGrab(escape);
		}
		this.GrabData.Clear();
	}

	public bool IsThrowMove(MoveData throwMove)
	{
		switch (this.GrabData.grabType)
		{
		case GrabType.None:
			return false;
		case GrabType.Normal:
			return throwMove.IsThrow;
		case GrabType.Move:
			return this.GrabData.grabbedOpponent != PlayerNum.None;
		}
		return false;
	}

	public void OnBeginThrow(MoveData throwMove)
	{
		if (this.grabbedOpponent == null)
		{
			return;
		}
		if (throwMove.overrideThrowGrabbedBodyPart != BodyPart.none)
		{
			this.GrabData.grabbedBodyPart = throwMove.overrideThrowGrabbedBodyPart;
		}
		this.GrabData.ignoreThrowBoneRotation = throwMove.ignoreThrowBoneRotation;
	}

	public void ReleaseFromGrab(bool didEscape)
	{
		this.player.State.MetaState = MetaState.Stand;
		this.GrabData.grabbingOpponent = PlayerNum.None;
		this.player.Orientation.RotateY((this.player.Facing != HorizontalDirection.Left) ? 90 : (-90));
		this.player.Orientation.RotateX(0);
		Vector3F position = this.player.Position;
		position.z = 0;
		this.player.Physics.SetPosition(position);
		if (didEscape)
		{
			this.player.StateActor.ReleaseFromGrab();
		}
	}

	void IGrabController.OnGrabbedBy(PlayerController other, GrabType grabType)
	{
		if (this.player.Invincibility.IsInvincible)
		{
			return;
		}
		if (other.GrabController.GrabbedOpponent != this.player.PlayerNum)
		{
			return;
		}
		this.player.DispatchInteraction(PlayerController.InteractionSignalData.Type.Grabbed);
		this.player.StateActor.ReleaseShield(false, true);
		if (this.player.State.IsShieldBroken)
		{
			this.player.StateActor.ReleaseShieldBreak();
		}
		this.player.LedgeGrabController.ReleaseGrabbedLedge(true, false);
		this.player.State.MetaState = MetaState.Grabbed;
		this.player.State.SubState = SubStates.Resting;
		this.player.Physics.StopMovement(true, true, VelocityType.Total);
		this.player.Physics.ResetAirJump();
		this.GrabData.grabbingOpponent = other.PlayerNum;
		this.player.OnGrabbed();
		this.player.EndActiveMove(MoveEndType.Cancelled, true, false);
		if (this.config.spikeConfig.resetUntechableSpikeWhenGrabbed)
		{
			this.player.Model.untechableBounceUsed = false;
		}
		if (grabType == GrabType.Normal)
		{
			this.player.StateActor.StartCharacterAction(ActionState.GrabbedBegin, null, null, true, 0, false);
			BodyPart bodyPart = BodyPart.throwBone;
			Vector3F bonePosition = this.grabbingOpponent.Body.GetBonePosition(bodyPart, false);
			bonePosition.x += (Fixed)((double)this.grabbingOpponent.CharacterData.grab.GrabXOffset) * InputUtils.GetDirectionMultiplier(this.grabbingOpponent.Facing);
			bonePosition.y = this.grabbingOpponent.Position.y;
			Vector3F delta = bonePosition - this.player.Position;
			delta.z = 0;
			this.player.Physics.ForceTranslate(delta, true, false);
			this.GrabData.initialGrabDisplacement = bonePosition - this.grabbingOpponent.Position;
		}
	}

	public void TickStandardGrabbed()
	{
		if (this.grabbingOpponent == null)
		{
			return;
		}
		if (this.player.Model.buttonsPressedThisFrame > 0)
		{
			this.grabbingOpponent.GrabData.grabDurationFrames -= this.config.grabConfig.buttonMashEscapeFrames;
		}
		Vector3F position = this.grabbingOpponent.Position + this.GrabData.initialGrabDisplacement;
		this.player.Physics.SetPosition(position);
	}
}
