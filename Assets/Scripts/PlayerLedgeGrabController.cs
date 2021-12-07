// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class PlayerLedgeGrabController : ILedgeGrabController, ITickable
{
	private IPlayerDelegate player;

	private IFrameOwner frameOwner;

	private StageSceneData stage;

	private LedgeConfig ledgeConfig;

	private static Fixed LEDGE_BOTTOM_BUFFER = (Fixed)0.25;

	private Ledge GrabbedLedge
	{
		get
		{
			return this.stage.getLedge(this.player.Model.grabbedLedgeIndex);
		}
	}

	public bool IsLedgeGrabbing
	{
		get
		{
			return this.player.Model.grabbedLedgeIndex != -1;
		}
	}

	public PlayerLedgeGrabController(IPlayerDelegate player, IFrameOwner frameOwner, StageSceneData stage, LedgeConfig ledgeConfig)
	{
		this.player = player;
		this.frameOwner = frameOwner;
		this.stage = stage;
		this.ledgeConfig = ledgeConfig;
	}

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

	void ILedgeGrabController.OnLedgeGrabComplete()
	{
		this.snapToPosition(this.GrabbedLedge.Position, this.GrabbedLedge.GetPlayerSlot(this.player.PlayerNum));
	}
}
