// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public struct CollisionData
{
	public Vector2F point;

	public Vector2F normal;

	public Vector2F deltaToCollision;

	public bool bottomBoundGrounded;

	public CollisionType collisionType;

	public IPhysicsCollider terrainCollider;

	public int terrainEdgeIndex;

	public IPhysicsCollider objectCollider;

	public int objectEdgeIndex;

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

	public bool IsPlatformCollision
	{
		get
		{
			return this.terrainCollider != null && this.terrainCollider.Layer == PhysicsSimulator.PlatformLayer;
		}
	}

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
}
