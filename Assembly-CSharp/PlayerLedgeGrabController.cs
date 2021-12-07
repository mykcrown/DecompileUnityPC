using System;
using FixedPoint;

// Token: 0x020005E9 RID: 1513
public class PlayerLedgeGrabController : ILedgeGrabController, ITickable
{
	// Token: 0x060023B6 RID: 9142 RVA: 0x000B44B5 File Offset: 0x000B28B5
	public PlayerLedgeGrabController(IPlayerDelegate player, IFrameOwner frameOwner, StageSceneData stage, LedgeConfig ledgeConfig)
	{
		this.player = player;
		this.frameOwner = frameOwner;
		this.stage = stage;
		this.ledgeConfig = ledgeConfig;
	}

	// Token: 0x17000837 RID: 2103
	// (get) Token: 0x060023B7 RID: 9143 RVA: 0x000B44DA File Offset: 0x000B28DA
	private Ledge GrabbedLedge
	{
		get
		{
			return this.stage.getLedge(this.player.Model.grabbedLedgeIndex);
		}
	}

	// Token: 0x17000838 RID: 2104
	// (get) Token: 0x060023B8 RID: 9144 RVA: 0x000B44F7 File Offset: 0x000B28F7
	public bool IsLedgeGrabbing
	{
		get
		{
			return this.player.Model.grabbedLedgeIndex != -1;
		}
	}

	// Token: 0x060023B9 RID: 9145 RVA: 0x000B4510 File Offset: 0x000B2910
	private void snapToPosition(Vector3F snapPosition, int slot)
	{
		CharacterActionData action = this.player.MoveSet.Actions.GetAction(this.player.State.ActionState, false);
		if (action != null)
		{
			Vector3F position = this.player.Physics.State.position;
			Vector3F delta = snapPosition - position;
			Fixed one = 0;
			if (slot == 1)
			{
				one = this.player.Config.ledgeConfig.multiGrabOffset;
			}
			else if (slot == 2)
			{
				one = -this.player.Config.ledgeConfig.multiGrabOffset;
			}
			else if (slot == 3)
			{
				one = 2 * this.player.Config.ledgeConfig.multiGrabOffset;
			}
			else if (slot == 4)
			{
				one = -2 * this.player.Config.ledgeConfig.multiGrabOffset;
			}
			Fixed @fixed = one / this.player.Config.ledgeConfig.multigrabTranslateFrames;
			delta.z = one - position.z;
			if ((one > position.z && delta.z > @fixed) || (one < position.z && delta.z < @fixed))
			{
				delta.z = @fixed;
			}
			this.player.Physics.ForceTranslate(delta, false, false);
		}
	}

	// Token: 0x060023BA RID: 9146 RVA: 0x000B46A0 File Offset: 0x000B2AA0
	public void ReleaseGrabbedLedge(bool unlockLedge, bool reposition)
	{
		if (this.IsLedgeGrabbing)
		{
			if (reposition && !this.player.State.IsGrounded)
			{
				Vector2F b = this.player.CharacterMenusData.bounds.ledgeReleaseOffset;
				if (this.GrabbedLedge.facesRight)
				{
					b.x *= -1;
				}
				this.player.Physics.SetPosition(this.GrabbedLedge.Position + b);
			}
			if (unlockLedge)
			{
				this.GrabbedLedge.RemovePlayer(this.player.PlayerNum);
				this.player.Model.grabbedLedgeIndex = -1;
				this.player.Model.ledgeReleaseFrame = this.frameOwner.Frame;
				this.player.Model.ledgeGrabbedFrame = 0;
				this.player.Model.ledgeLagFrames = 0;
			}
			if (this.player.State.IsGrounded)
			{
				this.player.State.MetaState = MetaState.Stand;
			}
			else
			{
				this.player.State.MetaState = MetaState.Jump;
			}
		}
	}

	// Token: 0x060023BB RID: 9147 RVA: 0x000B47E4 File Offset: 0x000B2BE4
	void ITickable.TickFrame()
	{
		if (this.player.State.CanGrabLedge && this.player.Physics.Velocity.y < -(Fixed)0.01)
		{
			FixedRect ledgeGrabBox = this.GetLedgeGrabBox(this.player.Facing, this.player.Physics.Bounds);
			int grabbedLedgeIndex = this.stage.TestLedgeCollision(ledgeGrabBox, this.player.Position + this.player.Physics.Bounds.centerOffset);
			if (this.canGrabLedge(grabbedLedgeIndex))
			{
				this.grabLedge(grabbedLedgeIndex);
			}
		}
		if (this.player.IsLedgeGrabbing)
		{
			if (this.player.State.IsLedgeGrabbing || this.player.State.IsLedgeHangingState)
			{
				this.snapToPosition(this.GrabbedLedge.Position, this.GrabbedLedge.GetPlayerSlot(this.player.PlayerNum));
			}
			if (this.player.Model.ledgeLagFrames > 0)
			{
				this.player.Model.ledgeLagFrames--;
				if (this.player.Model.ledgeLagFrames == 0)
				{
					this.player.ProcessBufferedInput();
				}
			}
			this.player.Renderer.SetColorModeFlag(ColorMode.LedgeVulnerable, this.player.Model.ledgeLagFrames > 0);
			if (!this.player.ActiveMove.IsActive && this.frameOwner.Frame - this.player.Model.ledgeGrabbedFrame > this.ledgeConfig.maxEdgeHoldFrames)
			{
				this.ReleaseGrabbedLedge(true, true);
			}
		}
	}

	// Token: 0x060023BC RID: 9148 RVA: 0x000B49C4 File Offset: 0x000B2DC4
	public FixedRect GetLedgeGrabBox(HorizontalDirection facing, EnvironmentBounds bounds)
	{
		FixedRect rect = this.player.CharacterMenusData.bounds.ledgeGrabBox.rect;
		if (facing == HorizontalDirection.Left)
		{
			rect.position.x = -rect.Left - rect.Width;
		}
		if (rect.Bottom < bounds.down.y + PlayerLedgeGrabController.LEDGE_BOTTOM_BUFFER)
		{
			rect.dimensions.y = FixedMath.Abs(bounds.down.y - rect.Top) - PlayerLedgeGrabController.LEDGE_BOTTOM_BUFFER;
		}
		return rect;
	}

	// Token: 0x060023BD RID: 9149 RVA: 0x000B4A70 File Offset: 0x000B2E70
	private bool canGrabLedge(int grabbedLedgeIndex)
	{
		if (grabbedLedgeIndex == -1)
		{
			return false;
		}
		Ledge ledge = this.stage.getLedge(grabbedLedgeIndex);
		if (ledge == null)
		{
			return false;
		}
		bool flag = this.player.State.IsHelpless || (this.player.State.IsBusyWithMove && this.player.ActiveMove.IsActive && this.player.ActiveMove.IsLedgeGrabEnabled);
		bool flag2 = this.player.GameInput.IsHorizontalDirectionHeld((!ledge.facesRight) ? HorizontalDirection.Left : HorizontalDirection.Right);
		return !this.player.IsLedgeGrabbingBlocked && (!flag2 || flag);
	}

	// Token: 0x060023BE RID: 9150 RVA: 0x000B4B38 File Offset: 0x000B2F38
	private void grabLedge(int grabbedLedgeIndex)
	{
		Ledge ledge = this.stage.getLedge(grabbedLedgeIndex);
		this.player.EndActiveMove(MoveEndType.Cancelled, true, false);
		this.ReleaseGrabbedLedge(true, false);
		this.player.GrabController.ReleaseGrabbedOpponent(false);
		bool flag = ledge.IsOccupied();
		int openSlot = ledge.GetOpenSlot();
		ledge.AddPlayer(this.player.PlayerNum, openSlot);
		this.player.StateActor.StartCharacterAction(ActionState.EdgeGrab, null, null, true, 0, false);
		this.player.ClearBufferedInput();
		this.player.MoveUseTracker.OnGrabLedge();
		this.player.AnimationPlayer.Update(0);
		this.player.Physics.OnGrabLedge();
		this.player.State.SubState = SubStates.Resting;
		this.player.SetFacingAndRotation((!ledge.facesRight) ? HorizontalDirection.Right : HorizontalDirection.Left);
		this.player.Model.landingOverrideData = null;
		this.player.Model.helplessStateData = null;
		this.player.Model.untechableBounceUsed = false;
		this.player.Model.grabbedLedgeIndex = grabbedLedgeIndex;
		Fixed @fixed = this.ledgeConfig.invincibilityFrames * (1 - this.ledgeConfig.invincibilityDecay * this.player.Model.ledgeGrabsSinceLanding);
		if (@fixed < this.ledgeConfig.minInvincibilityCliffFrames)
		{
			@fixed = 0;
		}
		if (flag && this.ledgeConfig.secondPlayerNoIntangible)
		{
			@fixed = 0;
		}
		if (@fixed > 0)
		{
			this.player.Invincibility.BeginLedgeIntangibility((int)@fixed);
		}
		this.player.Model.ledgeGrabsSinceLanding++;
		this.player.Model.ledgeGrabbedFrame = this.frameOwner.Frame;
		if (flag)
		{
			this.player.Model.ledgeLagFrames = this.ledgeConfig.secondPlayerLedgeLag;
		}
		else
		{
			this.player.Model.ledgeLagFrames = 0;
		}
		this.player.Model.ClearLastHitData();
		this.snapToPosition(this.GrabbedLedge.Position, openSlot);
		this.player.GameVFX.PlayParticle(this.player.Config.defaultCharacterEffects.ledgeGrab, false, TeamNum.None);
	}

	// Token: 0x060023BF RID: 9151 RVA: 0x000B4DA6 File Offset: 0x000B31A6
	void ILedgeGrabController.OnLedgeGrabComplete()
	{
		this.snapToPosition(this.GrabbedLedge.Position, this.GrabbedLedge.GetPlayerSlot(this.player.PlayerNum));
	}

	// Token: 0x04001AE6 RID: 6886
	private IPlayerDelegate player;

	// Token: 0x04001AE7 RID: 6887
	private IFrameOwner frameOwner;

	// Token: 0x04001AE8 RID: 6888
	private StageSceneData stage;

	// Token: 0x04001AE9 RID: 6889
	private LedgeConfig ledgeConfig;

	// Token: 0x04001AEA RID: 6890
	private static Fixed LEDGE_BOTTOM_BUFFER = (Fixed)0.25;
}
