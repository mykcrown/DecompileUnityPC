// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class PhysicsCollisionCalculator
{
	private static Fixed EXTRACT_DIST = (Fixed)0.35;

	public static Fixed REVERSE_EXTRACT_DIST = PhysicsCollisionCalculator.EXTRACT_DIST * 0.5f;

	private static Fixed DELTA_TO_COLLISION_TOLERANCE = (Fixed)0.001;

	private static Fixed REVERSE_PROJECTION_SQR_TOLERANCE = (Fixed)0.004;

	private static Fixed IS_UPWARD_FACING_EDGE_DOT_THRESHOLD = (Fixed)0.95;

	private static Fixed RAYCAST_TOLERANCE = (Fixed)0.001;

	private static Fixed DIRECTION_NORMAL_DOT_TOLERANCE = (Fixed)0.001;

	private static Fixed TERRAIN_CORNER_COLLISION_RESOLVE_TOLERANCE = (Fixed)0.01;

	private static RaycastHitData[] sharedRaycasts = new RaycastHitData[10];

	private static ListUtil.LessThanDelegate<CollisionData> __f__mg_cache0;

	public static bool DetectCollisions(PhysicsContext context, List<ColliderSegmentReference> segments, Vector2F delta, int mask, List<CollisionData> outCollisions)
	{
		int count = outCollisions.Count;
		IPhysicsCollider collider = context.collider;
		for (int i = 0; i < segments.Count; i++)
		{
			ColliderSegmentReference colliderSegmentReference = segments[i];
			IPhysicsCollider collider2 = colliderSegmentReference.collider;
			if (collider2.LayerIntersects(mask))
			{
				PhysicsCollisionCalculator.detectCollisionBoundsToTerrainSegment(context, colliderSegmentReference, collider, delta, outCollisions);
				if (mask != PhysicsSimulator.PlatformMask)
				{
					PhysicsCollisionCalculator.detectCollisionTerrainSegmentToBounds(context, colliderSegmentReference, collider, delta, outCollisions);
				}
			}
		}
		if (outCollisions.Count > count && outCollisions.Count > 1)
		{
			if (PhysicsCollisionCalculator.__f__mg_cache0 == null)
			{
				PhysicsCollisionCalculator.__f__mg_cache0 = new ListUtil.LessThanDelegate<CollisionData>(PhysicsCollisionCalculator.collisionLessThan);
			}
			ListUtil.Sort<CollisionData>(outCollisions, PhysicsCollisionCalculator.__f__mg_cache0);
		}
		return outCollisions.Count > count;
	}

	private static bool collisionLessThan(CollisionData a, CollisionData b)
	{
		return a.collisionType > b.collisionType || (a.collisionType == b.collisionType && a.deltaToCollision.sqrMagnitude < b.deltaToCollision.sqrMagnitude);
	}

	public static CollisionData CalculateExtractionWithVelocity(PhysicsContext context, Vector2F direction, List<IPhysicsCollider> colliders, int mask)
	{
		Fixed @fixed = 1;
		IPhysicsCollider collider = context.collider;
		PhysicsWorld.ResetHitBuffer(PhysicsCollisionCalculator.sharedRaycasts);
		int num = 0;
		for (int i = 0; i < collider.Edge.SegmentCount; i++)
		{
			Vector2F point = collider.Edge.GetPoint(i);
			Fixed maxDistance = @fixed;
			num += context.world.RaycastTerrain(point, direction, maxDistance, mask, PhysicsCollisionCalculator.sharedRaycasts, RaycastFlags.None, default(Fixed));
		}
		if (num > PhysicsCollisionCalculator.sharedRaycasts.Length)
		{
			UnityEngine.Debug.LogWarning("Ran of out room in sharedRaycasts when attempting to calculate extraction.  Consider increasing size of sharedRaycasts array.  Insufficient space could cause collision errors.");
			num = PhysicsCollisionCalculator.sharedRaycasts.Length;
		}
		CollisionData result;
		if (num > 0)
		{
			IPhysicsCollider physicsCollider = null;
			Fixed other = Fixed.MaxValue;
			int num2 = 0;
			for (int j = 0; j < num; j++)
			{
				if (PhysicsCollisionCalculator.sharedRaycasts[j].distance < other)
				{
					physicsCollider = PhysicsCollisionCalculator.sharedRaycasts[j].collider;
					other = PhysicsCollisionCalculator.sharedRaycasts[j].distance;
					num2 = j;
				}
			}
			Fixed other2 = -Fixed.MaxValue;
			for (int k = 0; k < num; k++)
			{
				if (PhysicsCollisionCalculator.sharedRaycasts[k].collider == physicsCollider && PhysicsCollisionCalculator.sharedRaycasts[k].distance > other2)
				{
					other2 = PhysicsCollisionCalculator.sharedRaycasts[k].distance;
					num2 = k;
				}
			}
			RaycastHitData raycastHitData = PhysicsCollisionCalculator.sharedRaycasts[num2];
			result = new CollisionData(raycastHitData.point, raycastHitData.normal, raycastHitData.point - raycastHitData.origin, CollisionType.TerrainEdge, raycastHitData.collider, -1, collider, num2, false);
			return result;
		}
		PhysicsWorld.ResetHitBuffer(PhysicsCollisionCalculator.sharedRaycasts);
		for (int l = 0; l < colliders.Count; l++)
		{
			IPhysicsCollider physicsCollider2 = colliders[l];
			if (physicsCollider2.Enabled && physicsCollider2.LayerIntersects(mask))
			{
				for (int m = 0; m < physicsCollider2.Edge.SegmentCount; m++)
				{
					Vector2F point2 = physicsCollider2.Edge.GetPoint(m);
					if (collider.ContainsPoint(point2))
					{
						Fixed magnitude = (collider.Edge.GetPoint(0) - collider.Edge.GetPoint(2)).magnitude;
						Fixed magnitude2 = (collider.Edge.GetPoint(1) - collider.Edge.GetPoint(3)).magnitude;
						Fixed maxDistance2 = FixedMath.Max(magnitude, magnitude2);
						int num3 = PhysicsWorld.RaycastCollider(collider, point2, -direction, maxDistance2, 1 << LayerMask.NameToLayer("Player"), PhysicsCollisionCalculator.sharedRaycasts, RaycastFlags.None, PhysicsCollisionCalculator.RAYCAST_TOLERANCE);
						if (num3 > 0)
						{
							RaycastHitData raycastHitData2 = PhysicsCollisionCalculator.sharedRaycasts[0];
							result = new CollisionData(raycastHitData2.point, raycastHitData2.normal, direction * (raycastHitData2.distance + PhysicsCollisionCalculator.TERRAIN_CORNER_COLLISION_RESOLVE_TOLERANCE), CollisionType.TerrainCorner, physicsCollider2, m, collider, -1, false);
							return result;
						}
					}
				}
			}
		}
		result = new CollisionData
		{
			collisionType = CollisionType.None
		};
		return result;
	}

	public static CollisionData CalculateExtraction(PhysicsContext context, List<IPhysicsCollider> colliders, EdgeData previousBoundsEdge, int mask)
	{
		IPhysicsCollider collider = context.collider;
		if (collider.Edge.SegmentCount != previousBoundsEdge.SegmentCount)
		{
			throw new ArgumentException("Object collider and previous bounds edge don't have the same number of points.  Are you sure these are both representative of player bounds?");
		}
		PhysicsWorld.ResetHitBuffer(PhysicsCollisionCalculator.sharedRaycasts);
		int num = 0;
		for (int i = 0; i < collider.Edge.SegmentCount; i++)
		{
			Vector2F point = previousBoundsEdge.GetPoint(i);
			Vector2F point2 = collider.Edge.GetPoint(i);
			Vector2F zero = Vector2F.zero;
			Fixed maxDistance = 0;
			(point2 - point).Decompose(ref zero, ref maxDistance);
			num += context.world.RaycastTerrain(point, zero, maxDistance, mask, PhysicsCollisionCalculator.sharedRaycasts, RaycastFlags.EnableBackfaceCulling, default(Fixed));
		}
		if (num > PhysicsCollisionCalculator.sharedRaycasts.Length)
		{
			UnityEngine.Debug.LogWarning("Ran of out room in sharedRaycasts when attempting to calculate extraction.  Consider increasing size of sharedRaycasts array.  Insufficient space could cause collision errors.");
			num = PhysicsCollisionCalculator.sharedRaycasts.Length;
		}
		CollisionData result;
		if (num > 0)
		{
			Fixed other = -Fixed.MaxValue;
			int num2 = 0;
			for (int j = 0; j < num; j++)
			{
				if (PhysicsCollisionCalculator.sharedRaycasts[j].remainingDistance > other)
				{
					other = PhysicsCollisionCalculator.sharedRaycasts[j].remainingDistance;
					num2 = j;
				}
			}
			RaycastHitData raycastHitData = PhysicsCollisionCalculator.sharedRaycasts[num2];
			Vector2F b = raycastHitData.origin + raycastHitData.direction * (raycastHitData.distance + raycastHitData.remainingDistance);
			result = new CollisionData(raycastHitData.point, raycastHitData.normal, raycastHitData.point - b, CollisionType.TerrainEdge, raycastHitData.collider, -1, collider, num2, false);
			return result;
		}
		PhysicsWorld.ResetHitBuffer(PhysicsCollisionCalculator.sharedRaycasts);
		int num3 = 0;
		Vector2F vector2F = new Vector2F((collider.Edge.GetPoint(1).x + collider.Edge.GetPoint(3).x) / 2, (collider.Edge.GetPoint(0).y + collider.Edge.GetPoint(2).y) / 2);
		for (int k = 0; k < collider.Edge.SegmentCount; k++)
		{
			Vector2F point3 = collider.Edge.GetPoint(k);
			Vector2F zero2 = Vector2F.zero;
			Fixed maxDistance2 = 0;
			(point3 - vector2F).Decompose(ref zero2, ref maxDistance2);
			num3 += context.world.RaycastTerrain(vector2F, zero2, maxDistance2, mask, PhysicsCollisionCalculator.sharedRaycasts, RaycastFlags.EnableBackfaceCulling, default(Fixed));
		}
		if (num3 > PhysicsCollisionCalculator.sharedRaycasts.Length)
		{
			UnityEngine.Debug.LogWarning("Ran of out room in sharedRaycasts when attempting to calculate extraction.  Consider increasing size of sharedRaycasts array.  Insufficient space could cause collision errors.");
			num3 = PhysicsCollisionCalculator.sharedRaycasts.Length;
		}
		if (num3 > 0)
		{
			Fixed other2 = -Fixed.MaxValue;
			int num4 = 0;
			for (int l = 0; l < num3; l++)
			{
				if (PhysicsCollisionCalculator.sharedRaycasts[l].remainingDistance > other2)
				{
					other2 = PhysicsCollisionCalculator.sharedRaycasts[l].remainingDistance;
					num4 = l;
				}
			}
			RaycastHitData raycastHitData2 = PhysicsCollisionCalculator.sharedRaycasts[num4];
			Vector2F b2 = raycastHitData2.origin + raycastHitData2.direction * (raycastHitData2.distance + raycastHitData2.remainingDistance);
			result = new CollisionData(raycastHitData2.point, raycastHitData2.normal, raycastHitData2.point - b2, CollisionType.TerrainEdge, raycastHitData2.collider, -1, collider, num4, false);
			return result;
		}
		PhysicsWorld.ResetHitBuffer(PhysicsCollisionCalculator.sharedRaycasts);
		for (int m = 0; m < colliders.Count; m++)
		{
			IPhysicsCollider physicsCollider = colliders[m];
			if (physicsCollider.Enabled && physicsCollider.LayerIntersects(mask))
			{
				for (int n = 0; n < physicsCollider.Edge.SegmentCount; n++)
				{
					Vector2F point4 = physicsCollider.Edge.GetPoint(n);
					if (collider.ContainsPoint(point4))
					{
						Vector2F normal = physicsCollider.Edge.GetNormal(n);
						Vector2F normal2 = physicsCollider.Edge.GetNormal(n - 1);
						Vector2F a;
						if (Vector2F.Dot(normal, Vector2F.up) >= PhysicsCollisionCalculator.IS_UPWARD_FACING_EDGE_DOT_THRESHOLD || Vector2F.Dot(normal2, Vector2F.up) >= PhysicsCollisionCalculator.IS_UPWARD_FACING_EDGE_DOT_THRESHOLD)
						{
							a = Vector2F.up;
						}
						else
						{
							a = ((normal + normal2) / 2).normalized;
						}
						Vector2F normalizedDirection = -a;
						Fixed magnitude = (collider.Edge.GetPoint(0) - collider.Edge.GetPoint(2)).magnitude;
						Fixed magnitude2 = (collider.Edge.GetPoint(1) - collider.Edge.GetPoint(3)).magnitude;
						Fixed maxDistance3 = FixedMath.Max(magnitude, magnitude2);
						int num5 = PhysicsWorld.RaycastCollider(collider, point4, normalizedDirection, maxDistance3, 1 << LayerMask.NameToLayer("Player"), PhysicsCollisionCalculator.sharedRaycasts, RaycastFlags.None, PhysicsCollisionCalculator.RAYCAST_TOLERANCE);
						if (num5 > 0)
						{
							RaycastHitData raycastHitData3 = PhysicsCollisionCalculator.sharedRaycasts[0];
							result = new CollisionData(raycastHitData3.point, raycastHitData3.normal, a * (raycastHitData3.distance + PhysicsCollisionCalculator.TERRAIN_CORNER_COLLISION_RESOLVE_TOLERANCE), CollisionType.TerrainCorner, physicsCollider, n, collider, -1, false);
							return result;
						}
					}
				}
			}
		}
		result = new CollisionData
		{
			collisionType = CollisionType.None
		};
		return result;
	}

	private static void detectCollisionBoundsToTerrainSegment(PhysicsContext context, ColliderSegmentReference segment, IPhysicsCollider objectCollider, Vector2F delta, List<CollisionData> outCollisions)
	{
		IPhysicsCollider collider = segment.collider;
		int segmentIndex = segment.segmentIndex;
		for (int i = 0; i < objectCollider.Edge.Length; i++)
		{
			bool flag = (objectCollider.Edge.Length == 1 && i == 0) || (objectCollider.Edge.Length == 4 && i == 2);
			Vector2F point = objectCollider.Edge.GetPoint(i);
			if (segment.collider.Layer != PhysicsSimulator.PlatformLayer || flag)
			{
				Vector2F rhs = Vector2F.zero;
				Vector2F rhs2 = Vector2F.zero;
				if (objectCollider.Edge.IsLoop && objectCollider.Edge.Length > 1)
				{
					rhs = objectCollider.Edge.GetCounterClockwiseCast(i + 1);
					rhs2 = objectCollider.Edge.GetClockwiseCast(i - 1);
				}
				Vector2F point2 = collider.Edge.GetPoint(segmentIndex);
				Vector2F nextPoint = collider.Edge.GetNextPoint(segmentIndex);
				Vector2F normal = collider.Edge.GetNormal(segmentIndex);
				Fixed magnitude = delta.magnitude;
				Vector2F normalized = delta.normalized;
				Fixed one = Vector2F.Dot(normal, rhs);
				Fixed one2 = Vector2F.Dot(normal, rhs2);
				Fixed a = Vector2F.Dot(normalized, normal);
				if (!(one > 0) && !(one2 > 0) && !FixedMath.ApproximatelyOrGreater(a, 0, PhysicsCollisionCalculator.DIRECTION_NORMAL_DOT_TOLERANCE))
				{
					bool flag2 = false;
					FixedRect raycastBoundingBox = PhysicsUtil.GetRaycastBoundingBox(point, normalized, magnitude);
					FixedRect edgeBoundingBox = PhysicsUtil.GetEdgeBoundingBox(point2, nextPoint);
					Fixed d = 0;
					if (edgeBoundingBox.Overlaps(raycastBoundingBox))
					{
						flag2 = PhysicsWorld.RayLineSegmentIntersection(point, normalized, point2, nextPoint, magnitude, ref d, PhysicsCollisionCalculator.RAYCAST_TOLERANCE);
					}
					Vector2F vector2F = point + normalized * d;
					Vector2F vector2F2 = point - normalized * PhysicsCollisionCalculator.REVERSE_EXTRACT_DIST;
					Fixed maxDistance = magnitude + PhysicsCollisionCalculator.REVERSE_EXTRACT_DIST;
					Fixed d2 = 0;
					bool flag3 = false;
					if (segment.collider.Layer != PhysicsSimulator.PlatformLayer && collider.Edge.ShouldEnableReverseProjection(segmentIndex, delta, point2, nextPoint, vector2F, PhysicsCollisionCalculator.REVERSE_PROJECTION_SQR_TOLERANCE))
					{
						flag3 = PhysicsWorld.RayLineSegmentIntersection(vector2F2, normalized, point2, nextPoint, maxDistance, ref d2, default(Fixed));
					}
					Vector2F vector2F3 = vector2F2 + normalized * d2;
					if (flag2 || flag3)
					{
						if (!flag2 && flag3)
						{
							vector2F = vector2F3;
						}
						Vector3F v = vector2F - point;
						if (FixedMath.Abs(v.x) < PhysicsCollisionCalculator.DELTA_TO_COLLISION_TOLERANCE)
						{
							v.x = 0;
						}
						if (FixedMath.Abs(v.y) < PhysicsCollisionCalculator.DELTA_TO_COLLISION_TOLERANCE)
						{
							v.y = 0;
						}
						outCollisions.Add(new CollisionData(vector2F, normal, v, CollisionType.TerrainEdge, collider, segmentIndex, objectCollider, i, flag));
					}
				}
			}
		}
	}

	private static void detectCollisionTerrainSegmentToBounds(PhysicsContext context, ColliderSegmentReference segment, IPhysicsCollider objectCollider, Vector2F delta, List<CollisionData> outCollisions)
	{
		IPhysicsCollider collider = segment.collider;
		int segmentIndex = segment.segmentIndex;
		if (!collider.Edge.IsLoop && segmentIndex == 0)
		{
			return;
		}
		Vector2F point = collider.Edge.GetPoint(segmentIndex);
		for (int i = 0; i < objectCollider.Edge.SegmentCount; i++)
		{
			Vector2F point2 = objectCollider.Edge.GetPoint(i);
			Vector2F nextPoint = objectCollider.Edge.GetNextPoint(i);
			Vector2F normal = objectCollider.Edge.GetNormal(i);
			Fixed magnitude = delta.magnitude;
			Vector2F vector2F = -delta.normalized;
			Vector2F rhs = Vector2F.zero;
			Vector2F rhs2 = Vector2F.zero;
			Vector2F rhs3 = Vector2F.zero;
			Vector2F rhs4 = Vector2F.zero;
			rhs = collider.Edge.GetNormal(segmentIndex - 1);
			rhs4 = collider.Edge.GetClockwiseCast(segmentIndex - 1);
			rhs2 = collider.Edge.GetNormal(segmentIndex);
			rhs3 = collider.Edge.GetCounterClockwiseCast(segmentIndex + 1);
			Fixed one = Vector2F.Dot(vector2F, normal);
			Fixed one2 = Vector2F.Dot(vector2F, rhs);
			Fixed one3 = Vector2F.Dot(vector2F, rhs4);
			Fixed one4 = Vector2F.Dot(vector2F, rhs2);
			Fixed one5 = Vector2F.Dot(vector2F, rhs3);
			bool flag = one3 > 0 || one5 > 0;
			bool flag2 = flag && (one2 > 0 || one4 > 0);
			if (!(one >= 0) && flag2)
			{
				Fixed d = 0;
				bool flag3 = PhysicsWorld.RayLineSegmentIntersection(point, vector2F, point2, nextPoint, magnitude, ref d, PhysicsCollisionCalculator.RAYCAST_TOLERANCE);
				Vector2F vector2F2 = point + vector2F * d;
				if (flag3 && (vector2F2 == point2 || vector2F2 == nextPoint) && (one4 == 0 || one2 == 0))
				{
					flag3 = false;
				}
				if (flag3 && (point - point2).magnitude > PhysicsCollisionCalculator.DELTA_TO_COLLISION_TOLERANCE)
				{
					Vector2F nearestPointOnEdgeSegment = objectCollider.Edge.GetNearestPointOnEdgeSegment(i, point);
					(point - nearestPointOnEdgeSegment).Decompose(ref vector2F, ref magnitude);
					Vector3F v = vector2F * (magnitude - PhysicsCollisionCalculator.TERRAIN_CORNER_COLLISION_RESOLVE_TOLERANCE);
					outCollisions.Add(new CollisionData(vector2F2, -1 * normal, v, CollisionType.TerrainCorner, collider, segmentIndex, objectCollider, i, false));
					break;
				}
			}
		}
	}
}
