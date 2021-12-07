using System;
using FixedPoint;

// Token: 0x020004ED RID: 1261
public class WallJumpComponent : MoveComponent
{
	// Token: 0x06001B94 RID: 7060 RVA: 0x0008BB58 File Offset: 0x00089F58
	public override bool ValidateRequirements(MoveData data, IPlayerDelegate player, InputButtonsData input)
	{
		bool flag = input.buttonsHeld.Contains(ButtonPress.Jump);
		HorizontalDirection direction = InputUtils.GetDirection(input.horizontalAxisValue);
		HorizontalDirection horizontalDirection = (direction != HorizontalDirection.Left) ? HorizontalDirection.Left : HorizontalDirection.Right;
		AbsoluteDirection absoluteDirection = (horizontalDirection != HorizontalDirection.Right) ? AbsoluteDirection.Right : AbsoluteDirection.Left;
		Vector3F v = (absoluteDirection != AbsoluteDirection.Right) ? Vector2F.left : Vector2F.right;
		Fixed wallFlushDistance = base.config.wallJumpConfig.wallFlushDistance;
		if (direction == HorizontalDirection.None)
		{
			return false;
		}
		if (!player.CanWallJump(horizontalDirection))
		{
			return false;
		}
		RaycastHitData hit;
		if ((player.Physics.PerformBoundCast(absoluteDirection, Vector3F.zero, wallFlushDistance, v, PhysicsSimulator.GroundMask, out hit) || (hit.distance != 0 && hit.distance > -wallFlushDistance)) && this.canWallJumpOffHit(hit))
		{
			if (player.State.IsAirJumping && player.State.ActionStateFrame == 0)
			{
				player.Physics.ResetAirJump();
			}
			return true;
		}
		bool flag2 = player.Physics.PerformBoundCast(AbsoluteDirection.Up, Vector3F.zero, wallFlushDistance + player.Physics.State.bounds.HalfWidth, v, PhysicsSimulator.GroundMask, out hit);
		if (flag2 && this.canWallJumpOffHit(hit))
		{
			if (player.State.IsAirJumping && player.State.ActionStateFrame == 0)
			{
				player.Physics.ResetAirJump();
			}
			return true;
		}
		return false;
	}

	// Token: 0x06001B95 RID: 7061 RVA: 0x0008BCF0 File Offset: 0x0008A0F0
	public override void Init(IMoveDelegate move, IPlayerDelegate player, InputButtonsData input)
	{
		base.Init(move, player, input);
		HorizontalDirection facing = (player.Facing != HorizontalDirection.Left) ? HorizontalDirection.Left : HorizontalDirection.Right;
		player.SetFacingAndRotation(this.getDirectionFromInput(input, facing));
		player.Model.ledgeGrabCooldownFrames = base.config.wallJumpConfig.ledgeGrabCooldown;
	}

	// Token: 0x06001B96 RID: 7062 RVA: 0x0008BD44 File Offset: 0x0008A144
	protected HorizontalDirection getDirectionFromInput(InputButtonsData input, HorizontalDirection facing)
	{
		for (int i = 0; i < input.buttonsHeld.Count; i++)
		{
			ButtonPress buttonPress = input.buttonsHeld[i];
			if (InputUtils.IsHorizontal(buttonPress))
			{
				return InputUtils.GetDirectionFromButton(facing, buttonPress);
			}
		}
		return HorizontalDirection.None;
	}

	// Token: 0x06001B97 RID: 7063 RVA: 0x0008BD8E File Offset: 0x0008A18E
	protected HorizontalDirection getOppositeDirection(HorizontalDirection direction)
	{
		if (direction == HorizontalDirection.Left)
		{
			return HorizontalDirection.Right;
		}
		if (direction == HorizontalDirection.Right)
		{
			return HorizontalDirection.Left;
		}
		return HorizontalDirection.None;
	}

	// Token: 0x06001B98 RID: 7064 RVA: 0x0008BDA3 File Offset: 0x0008A1A3
	protected HorizontalDirection getOppositeDirectionFromInput(InputButtonsData input, HorizontalDirection facing)
	{
		return this.getOppositeDirection(this.getDirectionFromInput(input, facing));
	}

	// Token: 0x06001B99 RID: 7065 RVA: 0x0008BDB3 File Offset: 0x0008A1B3
	protected bool canWallJumpOffHit(RaycastHitData hit)
	{
		return hit.surfaceType == SurfaceType.Wall;
	}
}
