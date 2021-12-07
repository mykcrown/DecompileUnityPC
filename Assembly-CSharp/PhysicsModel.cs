using System;
using FixedPoint;
using UnityEngine;

// Token: 0x0200055F RID: 1375
[Serializable]
public class PhysicsModel : RollbackStateTyped<PhysicsModel>
{
	// Token: 0x17000661 RID: 1633
	// (get) Token: 0x06001E16 RID: 7702 RVA: 0x00098795 File Offset: 0x00096B95
	// (set) Token: 0x06001E17 RID: 7703 RVA: 0x0009879D File Offset: 0x00096B9D
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

	// Token: 0x06001E18 RID: 7704 RVA: 0x000987BC File Offset: 0x00096BBC
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

	// Token: 0x06001E19 RID: 7705 RVA: 0x000988E4 File Offset: 0x00096CE4
	public override object Clone()
	{
		PhysicsModel physicsModel = new PhysicsModel();
		this.CopyTo(physicsModel);
		return physicsModel;
	}

	// Token: 0x06001E1A RID: 7706 RVA: 0x000988FF File Offset: 0x00096CFF
	public void Reset()
	{
		PhysicsModel.EMPTY.CopyTo(this);
	}

	// Token: 0x17000662 RID: 1634
	// (get) Token: 0x06001E1B RID: 7707 RVA: 0x0009890C File Offset: 0x00096D0C
	private Vector3F positionDelta
	{
		get
		{
			return this.position - this.lastPosition;
		}
	}

	// Token: 0x17000663 RID: 1635
	// (get) Token: 0x06001E1C RID: 7708 RVA: 0x0009891F File Offset: 0x00096D1F
	public Vector3F center
	{
		get
		{
			return this.position + this.bounds.centerOffset;
		}
	}

	// Token: 0x17000664 RID: 1636
	// (get) Token: 0x06001E1D RID: 7709 RVA: 0x00098937 File Offset: 0x00096D37
	public Vector3F lastCenter
	{
		get
		{
			return this.lastPosition + this.bounds.lastCenterOffset;
		}
	}

	// Token: 0x17000665 RID: 1637
	// (get) Token: 0x06001E1E RID: 7710 RVA: 0x0009894F File Offset: 0x00096D4F
	public Vector3F totalVelocity
	{
		get
		{
			return this.movementVelocityComponent + this.knockbackVelocityComponent + this.forcedVelocityComponent + this.windVelocityComponent;
		}
	}

	// Token: 0x17000666 RID: 1638
	// (get) Token: 0x06001E1F RID: 7711 RVA: 0x00098978 File Offset: 0x00096D78
	public Vector3F movementVelocity
	{
		get
		{
			return this.movementVelocityComponent;
		}
	}

	// Token: 0x17000667 RID: 1639
	// (get) Token: 0x06001E20 RID: 7712 RVA: 0x00098980 File Offset: 0x00096D80
	public Vector3F knockbackVelocity
	{
		get
		{
			return this.knockbackVelocityComponent;
		}
	}

	// Token: 0x17000668 RID: 1640
	// (get) Token: 0x06001E21 RID: 7713 RVA: 0x00098988 File Offset: 0x00096D88
	public Vector3F windVelocity
	{
		get
		{
			return this.windVelocityComponent;
		}
	}

	// Token: 0x17000669 RID: 1641
	// (get) Token: 0x06001E22 RID: 7714 RVA: 0x00098990 File Offset: 0x00096D90
	public Vector3F forcedVelocity
	{
		get
		{
			return this.forcedVelocityComponent;
		}
	}

	// Token: 0x1700066A RID: 1642
	// (get) Token: 0x06001E23 RID: 7715 RVA: 0x00098998 File Offset: 0x00096D98
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

	// Token: 0x06001E24 RID: 7716 RVA: 0x000989B8 File Offset: 0x00096DB8
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
			Debug.LogWarning("Failed to find velocity for type " + velocityType);
			return Vector3F.zero;
		}
	}

	// Token: 0x06001E25 RID: 7717 RVA: 0x00098A44 File Offset: 0x00096E44
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

	// Token: 0x06001E26 RID: 7718 RVA: 0x00098AFC File Offset: 0x00096EFC
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

	// Token: 0x06001E27 RID: 7719 RVA: 0x00098B88 File Offset: 0x00096F88
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
			Debug.LogWarning(string.Concat(new object[]
			{
				"Failed to add velocity for type ",
				velocityType,
				" with value ",
				addInput
			}));
			break;
		}
	}

	// Token: 0x06001E28 RID: 7720 RVA: 0x00098D24 File Offset: 0x00097124
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
			Debug.LogWarning("Failed to scale velocity for type " + velocityType);
			break;
		}
		zero.x *= -1;
		return zero;
	}

	// Token: 0x06001E29 RID: 7721 RVA: 0x00098DB0 File Offset: 0x000971B0
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
			Debug.LogWarning(string.Concat(new object[]
			{
				"Failed to scale velocity for type ",
				velocityType,
				" with values: ",
				scale
			}));
			break;
		}
	}

	// Token: 0x06001E2A RID: 7722 RVA: 0x00098E94 File Offset: 0x00097294
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
			Debug.LogWarning("Failed to clear velocity type " + velocityType);
			break;
		}
	}

	// Token: 0x06001E2B RID: 7723 RVA: 0x00099027 File Offset: 0x00097427
	public void BeginPhysicsUpdate()
	{
		if (this.physicsUpdateDepth == 0)
		{
			this.cachedCurrentPosition = this.position;
		}
		this.physicsUpdateDepth++;
	}

	// Token: 0x06001E2C RID: 7724 RVA: 0x0009904E File Offset: 0x0009744E
	public void EndPhysicsUpdate(Transform transform)
	{
		this.physicsUpdateDepth--;
		if (this.physicsUpdateDepth == 0)
		{
			this.lastPosition = this.cachedCurrentPosition;
		}
		transform.position = (Vector3)this.position;
	}

	// Token: 0x1700066B RID: 1643
	// (get) Token: 0x06001E2D RID: 7725 RVA: 0x00099086 File Offset: 0x00097486
	// (set) Token: 0x06001E2E RID: 7726 RVA: 0x0009908E File Offset: 0x0009748E
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

	// Token: 0x0400184B RID: 6219
	private static PhysicsModel EMPTY = new PhysicsModel();

	// Token: 0x0400184C RID: 6220
	public Vector3F groundedNormal;

	// Token: 0x0400184D RID: 6221
	private Vector3F movementVelocityComponent = default(Vector3F);

	// Token: 0x0400184E RID: 6222
	private Vector3F knockbackVelocityComponent = default(Vector3F);

	// Token: 0x0400184F RID: 6223
	private Vector3F windVelocityComponent = default(Vector3F);

	// Token: 0x04001850 RID: 6224
	private Vector3F forcedVelocityComponent = default(Vector3F);

	// Token: 0x04001851 RID: 6225
	public Vector2F acceleration = default(Vector2F);

	// Token: 0x04001852 RID: 6226
	public Vector3F position = default(Vector3F);

	// Token: 0x04001853 RID: 6227
	public Vector3F lastPosition = default(Vector3F);

	// Token: 0x04001854 RID: 6228
	public int platformFallPreventFastfall;

	// Token: 0x04001855 RID: 6229
	public int framesSpentAirborne;

	// Token: 0x04001856 RID: 6230
	public bool pivotJump;

	// Token: 0x04001857 RID: 6231
	public int dashPivotFrame;

	// Token: 0x04001858 RID: 6232
	public Vector2F targetMoveVelocity = default(Vector2F);

	// Token: 0x04001859 RID: 6233
	public int targetMoveVelocityHorizontalFrames;

	// Token: 0x0400185A RID: 6234
	public int targetMoveVelocityVerticalFrames;

	// Token: 0x0400185B RID: 6235
	public bool isGrounded;

	// Token: 0x0400185C RID: 6236
	private RestoreVelocityType restoreVelocity;

	// Token: 0x0400185D RID: 6237
	private int physicsUpdateDepth;

	// Token: 0x0400185E RID: 6238
	[IsClonedManually]
	[IgnoreCopyValidation]
	public EnvironmentBounds bounds = new EnvironmentBounds();

	// Token: 0x0400185F RID: 6239
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IsClonedManually]
	[IgnoreCopyValidation]
	public PhysicsOverride physicsOverride;

	// Token: 0x04001860 RID: 6240
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public CharacterPhysicsOverride characterPhysicsOverride = CharacterPhysicsOverride.NoOverride;

	// Token: 0x04001861 RID: 6241
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public IMovingObject groundedMovingStageObject;

	// Token: 0x04001862 RID: 6242
	[SerializeField]
	private Vector3F cachedCurrentPosition = default(Vector3F);
}
