using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x020005D2 RID: 1490
public class GrabManager : IGrabManager, ITickable
{
	// Token: 0x0600216F RID: 8559 RVA: 0x000A70DE File Offset: 0x000A54DE
	public GrabManager(IPlayerLookup players)
	{
		this.players = players;
	}

	// Token: 0x06002170 RID: 8560 RVA: 0x000A70F0 File Offset: 0x000A54F0
	void ITickable.TickFrame()
	{
		List<PlayerController> list = this.players.GetPlayers();
		foreach (PlayerController playerController in list)
		{
			if (playerController.GrabController.IsGrabbing)
			{
				this.tickGrabber(playerController);
			}
		}
		foreach (PlayerController playerController2 in list)
		{
			if (playerController2.GrabController.IsGrabbed)
			{
				this.tickGrabbee(playerController2);
			}
		}
	}

	// Token: 0x06002171 RID: 8561 RVA: 0x000A71BC File Offset: 0x000A55BC
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

	// Token: 0x06002172 RID: 8562 RVA: 0x000A7260 File Offset: 0x000A5660
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

	// Token: 0x06002173 RID: 8563 RVA: 0x000A7380 File Offset: 0x000A5780
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

	// Token: 0x04001A53 RID: 6739
	private IPlayerLookup players;
}
