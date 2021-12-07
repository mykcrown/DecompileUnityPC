// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public static class PhysicsUtil
{
	public static FixedRect ExtendBoundingBox(FixedRect aabb, Vector2F extend)
	{
		aabb.dimensions.x = aabb.dimensions.x + FixedMath.Abs(extend.x);
		aabb.dimensions.y = aabb.dimensions.y + FixedMath.Abs(extend.y);
		if (extend.x < 0)
		{
			aabb.position.x = aabb.position.x + extend.x;
		}
		if (extend.y > 0)
		{
			aabb.position.y = aabb.position.y + extend.y;
		}
		return aabb;
	}

	public static FixedRect ExtendBoundingBox(FixedRect aabb, Fixed extend)
	{
		Fixed other = extend / 2;
		aabb.position.x = aabb.position.x - other;
		aabb.position.y = aabb.position.y - other;
		aabb.dimensions.x = aabb.dimensions.x + other;
		aabb.dimensions.y = aabb.dimensions.y + other;
		return aabb;
	}

	public static FixedRect GetEdgeBoundingBox(Vector2F point1, Vector2F point2)
	{
		return new FixedRect(FixedMath.Min(point1.x, point2.x), FixedMath.Max(point1.y, point2.y), FixedMath.Abs(point2.x - point1.x), FixedMath.Abs(point2.y - point1.y));
	}

	public static FixedRect GetRaycastBoundingBox(Vector2F origin, Vector2F direction, Fixed distance)
	{
		return PhysicsUtil.GetEdgeBoundingBox(origin, origin + direction * distance);
	}

	public static IPhysicsCollider UpdateBoundsCollider(IPhysicsCollider collider, Vector3F center, EnvironmentBounds bounds)
	{
		if (collider == null || collider.Edge.Length != 4)
		{
			UnityEngine.Debug.LogError("Invalid bounds collider.");
			return null;
		}
		collider.SetPointsRelative(center, bounds.up, bounds.right, bounds.down, bounds.left);
		return collider;
	}

	public static IPhysicsCollider UpdateContextCollider(PhysicsContext context)
	{
		return PhysicsUtil.UpdateBoundsCollider(context.collider, context.model.center, context.model.bounds);
	}
}
