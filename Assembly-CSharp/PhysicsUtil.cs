using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000566 RID: 1382
public static class PhysicsUtil
{
	// Token: 0x06001E55 RID: 7765 RVA: 0x0009ABB4 File Offset: 0x00098FB4
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

	// Token: 0x06001E56 RID: 7766 RVA: 0x0009AC68 File Offset: 0x00099068
	public static FixedRect ExtendBoundingBox(FixedRect aabb, Fixed extend)
	{
		Fixed other = extend / 2;
		aabb.position.x = aabb.position.x - other;
		aabb.position.y = aabb.position.y - other;
		aabb.dimensions.x = aabb.dimensions.x + other;
		aabb.dimensions.y = aabb.dimensions.y + other;
		return aabb;
	}

	// Token: 0x06001E57 RID: 7767 RVA: 0x0009ACE0 File Offset: 0x000990E0
	public static FixedRect GetEdgeBoundingBox(Vector2F point1, Vector2F point2)
	{
		return new FixedRect(FixedMath.Min(point1.x, point2.x), FixedMath.Max(point1.y, point2.y), FixedMath.Abs(point2.x - point1.x), FixedMath.Abs(point2.y - point1.y));
	}

	// Token: 0x06001E58 RID: 7768 RVA: 0x0009AD48 File Offset: 0x00099148
	public static FixedRect GetRaycastBoundingBox(Vector2F origin, Vector2F direction, Fixed distance)
	{
		return PhysicsUtil.GetEdgeBoundingBox(origin, origin + direction * distance);
	}

	// Token: 0x06001E59 RID: 7769 RVA: 0x0009AD60 File Offset: 0x00099160
	public static IPhysicsCollider UpdateBoundsCollider(IPhysicsCollider collider, Vector3F center, EnvironmentBounds bounds)
	{
		if (collider == null || collider.Edge.Length != 4)
		{
			Debug.LogError("Invalid bounds collider.");
			return null;
		}
		collider.SetPointsRelative(center, bounds.up, bounds.right, bounds.down, bounds.left);
		return collider;
	}

	// Token: 0x06001E5A RID: 7770 RVA: 0x0009ADC9 File Offset: 0x000991C9
	public static IPhysicsCollider UpdateContextCollider(PhysicsContext context)
	{
		return PhysicsUtil.UpdateBoundsCollider(context.collider, context.model.center, context.model.bounds);
	}
}
