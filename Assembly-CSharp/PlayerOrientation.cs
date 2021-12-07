using System;
using FixedPoint;

// Token: 0x020005ED RID: 1517
public class PlayerOrientation : IPlayerOrientation, IRollbackStateOwner, ITickable
{
	// Token: 0x1700083A RID: 2106
	// (get) Token: 0x060023CB RID: 9163 RVA: 0x000B539D File Offset: 0x000B379D
	private PlayerModel model
	{
		get
		{
			return this.playerController.Model;
		}
	}

	// Token: 0x1700083B RID: 2107
	// (get) Token: 0x060023CC RID: 9164 RVA: 0x000B53AA File Offset: 0x000B37AA
	private MoveController activeMove
	{
		get
		{
			return this.playerController.ActiveMove;
		}
	}

	// Token: 0x1700083C RID: 2108
	// (get) Token: 0x060023CD RID: 9165 RVA: 0x000B53B7 File Offset: 0x000B37B7
	private PlayerPhysicsController physics
	{
		get
		{
			return this.playerController.Physics;
		}
	}

	// Token: 0x060023CE RID: 9166 RVA: 0x000B53C4 File Offset: 0x000B37C4
	public void Init(IRotationController rotationController, PlayerController playerController)
	{
		this.rotationController = rotationController;
		this.playerController = playerController;
	}

	// Token: 0x060023CF RID: 9167 RVA: 0x000B53D4 File Offset: 0x000B37D4
	public void OrientToTumbleSpin(Vector3F velocity)
	{
		if (this.playerController.Facing == HorizontalDirection.Right)
		{
			velocity.x *= -1;
		}
		this.RotateX(MathUtil.VectorToAngle(ref velocity) - 90);
	}

	// Token: 0x060023D0 RID: 9168 RVA: 0x000B5410 File Offset: 0x000B3810
	public void TickFrame()
	{
		if (!this.playerController.State.IsGrabbedState)
		{
			this.model.rotation.z = 0;
			if (this.playerController.State.ActionState == ActionState.HitTumbleSpin && !this.playerController.State.IsHitLagPaused)
			{
				this.OrientToTumbleSpin(this.playerController.Physics.Velocity);
			}
			else if (this.activeMove.IsActive && this.activeMove.Data.rotateInMovementDirection)
			{
				Vector3F vector3F = this.physics.Velocity;
				if (this.activeMove.Model.applyForceContinuouslyEndFrame != -1 && this.activeMove.Model.lastAppliedForce.sqrMagnitude > 0)
				{
					vector3F = this.activeMove.Model.lastAppliedForce;
				}
				int num = this.activeMove.Data.directionRotationOffsetAngle;
				if (this.playerController.Facing == HorizontalDirection.Left)
				{
					vector3F.x *= -1;
					num *= -1;
				}
				this.RotateX(-MathUtil.VectorToAngle(ref vector3F) - this.activeMove.Data.directionRotationOffsetAngle);
			}
			else if (this.playerController.IsComponentRollingPlayer)
			{
				this.RotateX(this.playerController.ComponentPlayerRoll);
			}
			else
			{
				this.RotateX(0);
			}
		}
	}

	// Token: 0x1700083D RID: 2109
	// (get) Token: 0x060023D1 RID: 9169 RVA: 0x000B55A3 File Offset: 0x000B39A3
	public QuaternionF Rotation
	{
		get
		{
			return this.rotationController.Rotation;
		}
	}

	// Token: 0x060023D2 RID: 9170 RVA: 0x000B55B0 File Offset: 0x000B39B0
	public void RotateY(Fixed value)
	{
		this.model.rotation.y = value;
		this.syncRotation();
	}

	// Token: 0x060023D3 RID: 9171 RVA: 0x000B55C9 File Offset: 0x000B39C9
	public void RotateX(Fixed value)
	{
		this.model.rotation.x = value;
		this.syncRotation();
	}

	// Token: 0x060023D4 RID: 9172 RVA: 0x000B55E2 File Offset: 0x000B39E2
	public void SyncToFacing()
	{
		this.model.rotation.y = ((this.playerController.Facing != HorizontalDirection.Right) ? -90 : 90);
		this.syncRotation();
	}

	// Token: 0x060023D5 RID: 9173 RVA: 0x000B5619 File Offset: 0x000B3A19
	private void syncRotation()
	{
		this.rotationController.Rotate(this.model.rotation);
	}

	// Token: 0x060023D6 RID: 9174 RVA: 0x000B5631 File Offset: 0x000B3A31
	public void Rotate(Vector3F rotation)
	{
		this.model.rotation = rotation;
		this.syncRotation();
	}

	// Token: 0x060023D7 RID: 9175 RVA: 0x000B5645 File Offset: 0x000B3A45
	public bool ExportState(ref RollbackStateContainer container)
	{
		return this.rotationController.ExportState(ref container);
	}

	// Token: 0x060023D8 RID: 9176 RVA: 0x000B5653 File Offset: 0x000B3A53
	public bool LoadState(RollbackStateContainer container)
	{
		return this.rotationController.LoadState(container);
	}

	// Token: 0x04001B45 RID: 6981
	private IRotationController rotationController;

	// Token: 0x04001B46 RID: 6982
	private PlayerController playerController;
}
