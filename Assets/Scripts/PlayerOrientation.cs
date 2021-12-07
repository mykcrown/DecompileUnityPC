// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class PlayerOrientation : IPlayerOrientation, IRollbackStateOwner, ITickable
{
	private IRotationController rotationController;

	private PlayerController playerController;

	private PlayerModel model
	{
		get
		{
			return this.playerController.Model;
		}
	}

	private MoveController activeMove
	{
		get
		{
			return this.playerController.ActiveMove;
		}
	}

	private PlayerPhysicsController physics
	{
		get
		{
			return this.playerController.Physics;
		}
	}

	public QuaternionF Rotation
	{
		get
		{
			return this.rotationController.Rotation;
		}
	}

	public void Init(IRotationController rotationController, PlayerController playerController)
	{
		this.rotationController = rotationController;
		this.playerController = playerController;
	}

	public void OrientToTumbleSpin(Vector3F velocity)
	{
		if (this.playerController.Facing == HorizontalDirection.Right)
		{
			velocity.x *= -1;
		}
		this.RotateX(MathUtil.VectorToAngle(ref velocity) - 90);
	}

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

	public void RotateY(Fixed value)
	{
		this.model.rotation.y = value;
		this.syncRotation();
	}

	public void RotateX(Fixed value)
	{
		this.model.rotation.x = value;
		this.syncRotation();
	}

	public void SyncToFacing()
	{
		this.model.rotation.y = ((this.playerController.Facing != HorizontalDirection.Right) ? (-90) : 90);
		this.syncRotation();
	}

	private void syncRotation()
	{
		this.rotationController.Rotate(this.model.rotation);
	}

	public void Rotate(Vector3F rotation)
	{
		this.model.rotation = rotation;
		this.syncRotation();
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		return this.rotationController.ExportState(ref container);
	}

	public bool LoadState(RollbackStateContainer container)
	{
		return this.rotationController.LoadState(container);
	}
}
