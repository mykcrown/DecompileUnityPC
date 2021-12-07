using System;
using FixedPoint;

// Token: 0x0200054B RID: 1355
public struct CollisionData
{
	// Token: 0x06001DC3 RID: 7619 RVA: 0x0009855C File Offset: 0x0009695C
	public CollisionData(Vector2F point, Vector2F normal, Vector2F deltaToCollision, CollisionType collisionType, IPhysicsCollider terrainCollider, int terrainEdgeIndex, IPhysicsCollider objectCollider, int objectEdgeIndex, bool bottomBoundGrounded = false)
	{
		this.point = point;
		this.normal = normal;
		this.deltaToCollision = deltaToCollision;
		this.collisionType = collisionType;
		this.terrainCollider = terrainCollider;
		this.terrainEdgeIndex = terrainEdgeIndex;
		this.objectCollider = objectCollider;
		this.objectEdgeIndex = objectEdgeIndex;
		this.bottomBoundGrounded = bottomBoundGrounded;
	}

	// Token: 0x1700065B RID: 1627
	// (get) Token: 0x06001DC4 RID: 7620 RVA: 0x000985AE File Offset: 0x000969AE
	public IMovingObject MovingObject
	{
		get
		{
			if (this.terrainCollider == null)
			{
				return null;
			}
			return this.terrainCollider.MovingObject;
		}
	}

	// Token: 0x1700065C RID: 1628
	// (get) Token: 0x06001DC5 RID: 7621 RVA: 0x000985C8 File Offset: 0x000969C8
	public bool IsPlatformCollision
	{
		get
		{
			return this.terrainCollider != null && this.terrainCollider.Layer == PhysicsSimulator.PlatformLayer;
		}
	}

	// Token: 0x1700065D RID: 1629
	// (get) Token: 0x06001DC6 RID: 7622 RVA: 0x000985EA File Offset: 0x000969EA
	public SurfaceType CollisionSurfaceType
	{
		get
		{
			if (this.terrainCollider != null)
			{
				return this.terrainCollider.Edge.GetSurfaceType(this.terrainEdgeIndex);
			}
			return SurfaceType.Other;
		}
	}

	// Token: 0x0400181E RID: 6174
	public Vector2F point;

	// Token: 0x0400181F RID: 6175
	public Vector2F normal;

	// Token: 0x04001820 RID: 6176
	public Vector2F deltaToCollision;

	// Token: 0x04001821 RID: 6177
	public bool bottomBoundGrounded;

	// Token: 0x04001822 RID: 6178
	public CollisionType collisionType;

	// Token: 0x04001823 RID: 6179
	public IPhysicsCollider terrainCollider;

	// Token: 0x04001824 RID: 6180
	public int terrainEdgeIndex;

	// Token: 0x04001825 RID: 6181
	public IPhysicsCollider objectCollider;

	// Token: 0x04001826 RID: 6182
	public int objectEdgeIndex;
}
