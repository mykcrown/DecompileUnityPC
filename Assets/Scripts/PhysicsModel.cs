// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

[Serializable]
public class PhysicsModel : RollbackStateTyped<PhysicsModel>
{
	private static PhysicsModel EMPTY = new PhysicsModel();

	public Vector3F groundedNormal;

	private Vector3F movementVelocityComponent = default(Vector3F);

	private Vector3F knockbackVelocityComponent = default(Vector3F);

	private Vector3F windVelocityComponent = default(Vector3F);

	private Vector3F forcedVelocityComponent = default(Vector3F);

	public Vector2F acceleration = default(Vector2F);

	public Vector3F position = default(Vector3F);

	public Vector3F lastPosition = default(Vector3F);

	public int platformFallPreventFastfall;

	public int framesSpentAirborne;

	public bool pivotJump;

	public int dashPivotFrame;

	public Vector2F targetMoveVelocity = default(Vector2F);

	public int targetMoveVelocityHorizontalFrames;

	public int targetMoveVelocityVerticalFrames;

	public bool isGrounded;

	private RestoreVelocityType restoreVelocity;

	private int physicsUpdateDepth;

	[IgnoreCopyValidation, IsClonedManually]
	public EnvironmentBounds bounds = new EnvironmentBounds();

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy), IsClonedManually]
	public PhysicsOverride physicsOverride;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public CharacterPhysicsOverride characterPhysicsOverride = CharacterPhysicsOverride.NoOverride;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public IMovingObject groundedMovingStageObject;

	[SerializeField]
	private Vector3F cachedCurrentPosition = default(Vector3F);

	public RestoreVelocityType RestoreVelocity
	{
		get
		{
			return this.restoreVelocity;
		}
		set
		{
			if (value != RestoreVelocityType.Restore || this.restoreVelocity != RestoreVelocityType.PreventRestore)
			{
				this.restoreVelocity = value;
			}
		}
	}

	private Vector3F positionDelta
	{
		get
		{
			return this.position - this.lastPosition;
		}
	}

	public Vector3F center
	{
		get
		{
			return this.position + this.bounds.centerOffset;
		}
	}

	public Vector3F lastCenter
	{
		get
		{
			return this.lastPosition + this.bounds.lastCenterOffset;
		}
	}

	public Vector3F totalVelocity
	{
		get
		{
			return this.movementVelocityComponent + this.knockbackVelocityComponent + this.forcedVelocityComponent + this.windVelocityComponent;
		}
	}

	public Vector3F movementVelocity
	{
		get
		{
			return this.movementVelocityComponent;
		}
	}

	public Vector3F knockbackVelocity
	{
		get
		{
			return this.knockbackVelocityComponent;
		}
	}

	public Vector3F windVelocity
	{
		get
		{
			return this.windVelocityComponent;
		}
	}

	public Vector3F forcedVelocity
	{
		get
		{
			return this.forcedVelocityComponent;
		}
	}

	public Vector3F movingPlatformDeltaPosition
	{
		get
		{
			if (this.groundedMovingStageObject != null)
			{
				return this.groundedMovingStageObject.DeltaPosition;
			}
			return Vector3F.zero;
		}
	}

	public bool IsGrounded
	{
		get
		{
			return this.isGrounded;
		}
		set
		{
			this.isGrounded = value;
			if (!this.isGrounded)
			{
				this.groundedMovingStageObject = null;
			}
		}
	}

	public override void CopyTo(PhysicsModel targetIn)
	{
		targetIn.groundedNormal = this.groundedNormal;
		targetIn.movementVelocityComponent = this.movementVelocityComponent;
		targetIn.knockbackVelocityComponent = this.knockbackVelocityComponent;
		targetIn.windVelocityComponent = this.windVelocityComponent;
		targetIn.forcedVelocityComponent = this.forcedVelocityComponent;
		targetIn.acceleration = this.acceleration;
		targetIn.position = this.position;
		targetIn.lastPosition = this.lastPosition;
		targetIn.platformFallPreventFastfall = this.platformFallPreventFastfall;
		targetIn.framesSpentAirborne = this.framesSpentAirborne;
		targetIn.dashPivotFrame = this.dashPivotFrame;
		targetIn.pivotJump = this.pivotJump;
		targetIn.targetMoveVelocity = this.targetMoveVelocity;
		targetIn.targetMoveVelocityHorizontalFrames = this.targetMoveVelocityHorizontalFrames;
		targetIn.targetMoveVelocityVerticalFrames = this.targetMoveVelocityVerticalFrames;
		targetIn.isGrounded = this.isGrounded;
		targetIn.physicsUpdateDepth = this.physicsUpdateDepth;
		targetIn.cachedCurrentPosition = this.cachedCurrentPosition;
		targetIn.restoreVelocity = this.restoreVelocity;
		targetIn.characterPhysicsOverride = this.characterPhysicsOverride;
		targetIn.groundedMovingStageObject = this.groundedMovingStageObject;
		this.bounds.CopyTo(targetIn.bounds);
		targetIn.physicsOverride = this.physicsOverride;
	}

	public override object Clone()
	{
		PhysicsModel physicsModel = new PhysicsModel();
		this.CopyTo(physicsModel);
		return physicsModel;
	}

	public void Reset()
	{
		PhysicsModel.EMPTY.CopyTo(this);
	}

	public Vector3F GetVelocity(VelocityType velocityType)
	{
		switch (velocityType)
		{
		case VelocityType.Movement:
			return this.movementVelocityComponent;
		case VelocityType.Knockback:
			return this.knockbackVelocityComponent;
		case VelocityType.Forced:
			return this.forcedVelocityComponent;
		case VelocityType.Wind:
			return this.windVelocityComponent;
		case VelocityType.Total:
			return this.knockbackVelocityComponent + this.movementVelocityComponent + this.windVelocityComponent + this.forcedVelocityComponent;
		default:
			UnityEngine.Debug.LogWarning("Failed to find velocity for type " + velocityType);
			return Vector3F.zero;
		}
	}

	public void SetVelocity(Vector3F newVelocity, VelocityType velocityType)
	{
		switch (velocityType)
		{
		case VelocityType.Movement:
			this.movementVelocityComponent = newVelocity;
			break;
		case VelocityType.Knockback:
			this.knockbackVelocityComponent = newVelocity;
			break;
		case VelocityType.Forced:
			this.forcedVelocityComponent = newVelocity;
			break;
		case VelocityType.Wind:
			this.windVelocityComponent = newVelocity;
			break;
		default:
		{
			Fixed magnitude = this.knockbackVelocityComponent.magnitude;
			Fixed magnitude2 = this.forcedVelocityComponent.magnitude;
			Vector3F normalized = newVelocity.normalized;
			this.knockbackVelocityComponent = normalized * magnitude;
			this.movementVelocityComponent = normalized * (newVelocity.magnitude - magnitude - magnitude2);
			this.forcedVelocityComponent = normalized * magnitude2;
			break;
		}
		}
	}

	public void SetVelocity(Fixed x, Fixed y, Fixed z, VelocityType velocityType)
	{
		switch (velocityType)
		{
		case VelocityType.Movement:
			this.movementVelocityComponent.Set(x, y, z);
			break;
		case VelocityType.Knockback:
			this.knockbackVelocityComponent.Set(x, y, z);
			break;
		case VelocityType.Forced:
			this.forcedVelocityComponent.Set(x, y, z);
			break;
		case VelocityType.Wind:
			this.windVelocityComponent.Set(x, y, z);
			break;
		default:
			this.SetVelocity(new Vector3F(x, y, z), velocityType);
			break;
		}
	}

	public void AddVelocity(ref Vector3F addInput, VelocityType velocityType)
	{
		switch (velocityType)
		{
		case VelocityType.Movement:
			this.SetVelocity(this.movementVelocityComponent.x + addInput.x, this.movementVelocityComponent.y + addInput.y, this.movementVelocityComponent.z + addInput.z, VelocityType.Movement);
			break;
		case VelocityType.Knockback:
			this.SetVelocity(this.knockbackVelocityComponent.x + addInput.x, this.knockbackVelocityComponent.y + addInput.y, this.knockbackVelocityComponent.z + addInput.z, VelocityType.Knockback);
			break;
		case VelocityType.Forced:
			this.SetVelocity(this.forcedVelocityComponent.x + addInput.x, this.forcedVelocityComponent.y + addInput.y, this.forcedVelocityComponent.z + addInput.z, VelocityType.Forced);
			break;
		case VelocityType.Wind:
			this.SetVelocity(this.windVelocityComponent.x + addInput.x, this.windVelocityComponent.y + addInput.y, this.windVelocityComponent.z + addInput.z, VelocityType.Wind);
			break;
		default:
			UnityEngine.Debug.LogWarning(string.Concat(new object[]
			{
				"Failed to add velocity for type ",
				velocityType,
				" with value ",
				addInput
			}));
			break;
		}
	}

	public Vector3F GetReverseHorizontalVelocity(VelocityType velocityType)
	{
		Vector3F zero = Vector3F.zero;
		switch (velocityType)
		{
		case VelocityType.Movement:
			zero = this.movementVelocityComponent;
			break;
		case VelocityType.Knockback:
			zero = this.knockbackVelocityComponent;
			break;
		case VelocityType.Forced:
			zero = this.forcedVelocityComponent;
			break;
		case VelocityType.Wind:
			zero = this.windVelocityComponent;
			break;
		default:
			UnityEngine.Debug.LogWarning("Failed to scale velocity for type " + velocityType);
			break;
		}
		zero.x *= -1;
		return zero;
	}

	public void ScaleVelocity(Fixed scale, VelocityType velocityType)
	{
		switch (velocityType)
		{
		case VelocityType.Movement:
			this.movementVelocityComponent *= scale;
			break;
		case VelocityType.Knockback:
			this.knockbackVelocityComponent *= scale;
			break;
		case VelocityType.Forced:
			this.forcedVelocityComponent *= scale;
			break;
		case VelocityType.Wind:
			this.windVelocityComponent *= scale;
			break;
		case VelocityType.Total:
			this.ScaleVelocity(scale, VelocityType.Knockback);
			this.ScaleVelocity(scale, VelocityType.Movement);
			this.ScaleVelocity(scale, VelocityType.Forced);
			this.ScaleVelocity(scale, VelocityType.Wind);
			break;
		default:
			UnityEngine.Debug.LogWarning(string.Concat(new object[]
			{
				"Failed to scale velocity for type ",
				velocityType,
				" with values: ",
				scale
			}));
			break;
		}
	}

	public void ClearVelocity(bool x, bool y, bool z, VelocityType velocityType)
	{
		switch (velocityType)
		{
		case VelocityType.Movement:
			if (x)
			{
				this.movementVelocityComponent.x = 0;
			}
			if (y)
			{
				this.movementVelocityComponent.y = 0;
			}
			if (z)
			{
				this.movementVelocityComponent.z = 0;
			}
			break;
		case VelocityType.Knockback:
			if (x)
			{
				this.knockbackVelocityComponent.x = 0;
			}
			if (y)
			{
				this.knockbackVelocityComponent.y = 0;
			}
			if (z)
			{
				this.knockbackVelocityComponent.z = 0;
			}
			break;
		case VelocityType.Forced:
			if (x)
			{
				this.forcedVelocityComponent.x = 0;
			}
			if (y)
			{
				this.forcedVelocityComponent.y = 0;
			}
			if (z)
			{
				this.forcedVelocityComponent.z = 0;
			}
			break;
		case VelocityType.Wind:
			if (x)
			{
				this.windVelocityComponent.x = 0;
			}
			if (y)
			{
				this.windVelocityComponent.y = 0;
			}
			if (z)
			{
				this.windVelocityComponent.z = 0;
			}
			break;
		case VelocityType.Total:
			this.ClearVelocity(x, y, z, VelocityType.Knockback);
			this.ClearVelocity(x, y, z, VelocityType.Movement);
			this.ClearVelocity(x, y, z, VelocityType.Forced);
			break;
		default:
			UnityEngine.Debug.LogWarning("Failed to clear velocity type " + velocityType);
			break;
		}
	}

	public void BeginPhysicsUpdate()
	{
		if (this.physicsUpdateDepth == 0)
		{
			this.cachedCurrentPosition = this.position;
		}
		this.physicsUpdateDepth++;
	}

	public void EndPhysicsUpdate(Transform transform)
	{
		this.physicsUpdateDepth--;
		if (this.physicsUpdateDepth == 0)
		{
			this.lastPosition = this.cachedCurrentPosition;
		}
		transform.position = (Vector3)this.position;
	}
}
