using System;
using FixedPoint;
using UnityEngine;

// Token: 0x020005CF RID: 1487
public class GrabController : IGrabController
{
	// Token: 0x0600214E RID: 8526 RVA: 0x000A6791 File Offset: 0x000A4B91
	public GrabController(IPlayerDelegate player, ConfigData config, IMoveSet moveSet, IFrameOwner frameOwner, IPlayerLookup playerLookup)
	{
		this.player = player;
		this.config = config;
		this.frameOwner = frameOwner;
		this.playerLookup = playerLookup;
	}

	// Token: 0x17000760 RID: 1888
	// (get) Token: 0x0600214F RID: 8527 RVA: 0x000A67B7 File Offset: 0x000A4BB7
	public GrabData GrabData
	{
		get
		{
			return this.player.Model.grabData;
		}
	}

	// Token: 0x17000761 RID: 1889
	// (get) Token: 0x06002150 RID: 8528 RVA: 0x000A67C9 File Offset: 0x000A4BC9
	private PlayerController grabbingOpponent
	{
		get
		{
			return (this.GrabData.grabbingOpponent != PlayerNum.None) ? this.playerLookup.GetPlayerController(this.GrabData.grabbingOpponent) : null;
		}
	}

	// Token: 0x17000762 RID: 1890
	// (get) Token: 0x06002151 RID: 8529 RVA: 0x000A67F9 File Offset: 0x000A4BF9
	private PlayerController grabbedOpponent
	{
		get
		{
			return (this.GrabData.grabbedOpponent != PlayerNum.None) ? this.playerLookup.GetPlayerController(this.GrabData.grabbedOpponent) : null;
		}
	}

	// Token: 0x1700075C RID: 1884
	// (get) Token: 0x06002152 RID: 8530 RVA: 0x000A6829 File Offset: 0x000A4C29
	PlayerNum IGrabController.GrabbedOpponent
	{
		get
		{
			return this.GrabData.grabbedOpponent;
		}
	}

	// Token: 0x1700075D RID: 1885
	// (get) Token: 0x06002153 RID: 8531 RVA: 0x000A6836 File Offset: 0x000A4C36
	PlayerNum IGrabController.GrabbingOpponent
	{
		get
		{
			return this.GrabData.grabbingOpponent;
		}
	}

	// Token: 0x1700075E RID: 1886
	// (get) Token: 0x06002154 RID: 8532 RVA: 0x000A6843 File Offset: 0x000A4C43
	bool IGrabController.IsGrabbing
	{
		get
		{
			return this.GrabData.grabbedOpponent != PlayerNum.None;
		}
	}

	// Token: 0x1700075F RID: 1887
	// (get) Token: 0x06002155 RID: 8533 RVA: 0x000A6857 File Offset: 0x000A4C57
	bool IGrabController.IsGrabbed
	{
		get
		{
			return this.GrabData.grabbingOpponent != PlayerNum.None;
		}
	}

	// Token: 0x17000763 RID: 1891
	// (get) Token: 0x06002156 RID: 8534 RVA: 0x000A686C File Offset: 0x000A4C6C
	public bool IsReadyToRelease
	{
		get
		{
			return this.GrabData.grabType == GrabType.Normal && (!this.player.State.IsStandardGrabbingState || this.frameOwner.Frame - this.GrabData.grabbedStartFrame >= this.GrabData.grabDurationFrames);
		}
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x000A68CC File Offset: 0x000A4CCC
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
		Debug.LogError("Resolved unhandled grabType " + hitData.grabType);
	}

	// Token: 0x06002158 RID: 8536 RVA: 0x000A6B30 File Offset: 0x000A4F30
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

	// Token: 0x06002159 RID: 8537 RVA: 0x000A6BDC File Offset: 0x000A4FDC
	public bool IsThrowMove(MoveData throwMove)
	{
		switch (this.GrabData.grabType)
		{
		default:
			return false;
		case GrabType.Normal:
			return throwMove.IsThrow;
		case GrabType.Move:
			return this.GrabData.grabbedOpponent != PlayerNum.None;
		}
	}

	// Token: 0x0600215A RID: 8538 RVA: 0x000A6C28 File Offset: 0x000A5028
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

	// Token: 0x0600215B RID: 8539 RVA: 0x000A6C74 File Offset: 0x000A5074
	public void ReleaseFromGrab(bool didEscape)
	{
		this.player.State.MetaState = MetaState.Stand;
		this.GrabData.grabbingOpponent = PlayerNum.None;
		this.player.Orientation.RotateY((this.player.Facing != HorizontalDirection.Left) ? 90 : -90);
		this.player.Orientation.RotateX(0);
		Vector3F position = this.player.Position;
		position.z = 0;
		this.player.Physics.SetPosition(position);
		if (didEscape)
		{
			this.player.StateActor.ReleaseFromGrab();
		}
	}

	// Token: 0x0600215C RID: 8540 RVA: 0x000A6D24 File Offset: 0x000A5124
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

	// Token: 0x0600215D RID: 8541 RVA: 0x000A6F2C File Offset: 0x000A532C
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

	// Token: 0x04001A45 RID: 6725
	private IPlayerDelegate player;

	// Token: 0x04001A46 RID: 6726
	private ConfigData config;

	// Token: 0x04001A47 RID: 6727
	private IFrameOwner frameOwner;

	// Token: 0x04001A48 RID: 6728
	private IPlayerLookup playerLookup;
}
