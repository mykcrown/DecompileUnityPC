// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

public class GrabManager : IGrabManager, ITickable
{
	private IPlayerLookup players;

	public GrabManager(IPlayerLookup players)
	{
		this.players = players;
	}

	void ITickable.TickFrame()
	{
		List<PlayerController> list = this.players.GetPlayers();
		foreach (PlayerController current in list)
		{
			if (current.GrabController.IsGrabbing)
			{
				this.tickGrabber(current);
			}
		}
		foreach (PlayerController current2 in list)
		{
			if (current2.GrabController.IsGrabbed)
			{
				this.tickGrabbee(current2);
			}
		}
	}

	private void tickGrabber(PlayerController player)
	{
		IGrabController grabController = player.GrabController;
		PlayerController playerController = this.players.GetPlayerController(grabController.GrabbedOpponent);
		if (playerController == null)
		{
			grabController.ReleaseGrabbedOpponent(true);
			return;
		}
		if (playerController.State.IsStunned && playerController.Physics.KnockbackVelocity != Vector3F.zero)
		{
			grabController.ReleaseGrabbedOpponent(false);
		}
		if (grabController.IsReadyToRelease && (!player.ActiveMove.IsActive || player.ActiveMove.Data.label == MoveLabel.GrabPummel))
		{
			grabController.ReleaseGrabbedOpponent(true);
		}
	}

	private void tickGrabbee(PlayerController grabbee)
	{
		PlayerController playerController = this.players.GetPlayerController(grabbee.GrabController.GrabbingOpponent);
		if (playerController != null)
		{
			MoveData data = playerController.ActiveMove.Data;
			if ((playerController.ActiveMove.IsActive && data.IsThrow) || playerController.GrabData.grabType != GrabType.Normal)
			{
				string opponentAnimationClipName = data.GetOpponentAnimationClipName(playerController.CharacterData);
				if (data.opponentAnimationClip != null && grabbee.AnimationPlayer.CurrentAnimationName != opponentAnimationClipName)
				{
					grabbee.AnimationPlayer.PlayThrow(opponentAnimationClipName, playerController.Bones.IsInverted);
					grabbee.Model.actionState = ActionState.Thrown;
					grabbee.Model.actionStateFrame = 0;
				}
				else if (grabbee.State.ActionState != ActionState.Thrown)
				{
					grabbee.StateActor.StartCharacterAction(ActionState.Thrown, null, null, true, 0, false);
				}
				this.updateGrabPosition(playerController, grabbee);
			}
			else if (playerController.GrabData.grabType == GrabType.Normal)
			{
				grabbee.GrabController.TickStandardGrabbed();
			}
		}
	}

	private void updateGrabPosition(PlayerController grabber, PlayerController grabbee)
	{
		if (grabber == null || grabbee == null)
		{
			return;
		}
		BodyPart bodyPart = BodyPart.throwBone;
		QuaternionF rotation = grabber.Bones.GetRotation(bodyPart, false);
		Vector3F a = grabber.Bones.GetBonePosition(bodyPart, false);
		BodyPart grabbedBodyPart = grabber.GrabData.grabbedBodyPart;
		Fixed d = (Fixed)((double)grabber.CharacterData.grab.ThrowDistOffset);
		if (!grabber.GrabData.ignoreThrowBoneRotation)
		{
			QuaternionF quaternionF = rotation * QuaternionF.Euler(0, 180, 0);
			grabbee.Orientation.Rotate(quaternionF.eulerAngles);
		}
		Vector3F bonePosition = grabbee.Bones.GetBonePosition(grabbedBodyPart, false);
		Vector3F a2 = rotation * Vector3F.forward;
		a += a2 * d;
		Vector3F delta = a - bonePosition;
		grabbee.Physics.ForceTranslate(delta, false, false);
		if (!grabber.State.IsGrounded)
		{
			grabber.Physics.OnCollisionBoundsChanged(true);
		}
	}
}
