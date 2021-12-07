using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FixedPoint;
using UnityEngine;

// Token: 0x0200056A RID: 1386
public class PhysicsWorld
{
	// Token: 0x06001E5D RID: 7773 RVA: 0x0009AE1C File Offset: 0x0009921C
	public PhysicsWorld(IDevConsole devConsole)
	{
		if (devConsole != null)
		{
			devConsole.AddConsoleVariable<PhysicsWorldSortMode>("physics", "sort_mode", "Physics Segment Sort Mode", "Determines the sorting heuristic for line segments.", new Func<PhysicsWorldSortMode>(this.get_SortMode), delegate(PhysicsWorldSortMode value)
			{
				this.SortMode = value;
			});
		}
	}

	// Token: 0x17000673 RID: 1651
	// (get) Token: 0x06001E5E RID: 7774 RVA: 0x0009AE9B File Offset: 0x0009929B
	// (set) Token: 0x06001E5F RID: 7775 RVA: 0x0009AEA3 File Offset: 0x000992A3
	public PhysicsWorldSortMode SortMode
	{
		get
		{
			return this.sortMode;
		}
		set
		{
			this.sortMode = value;
			PhysicsWorld.SortSegments(this.colliderSegments, this.sortMode);
		}
	}

	// Token: 0x06001E60 RID: 7776 RVA: 0x0009AEBD File Offset: 0x000992BD
	public List<IPhysicsCollider> GetRelevantColliders()
	{
		return this.colliders;
	}

	// Token: 0x06001E61 RID: 7777 RVA: 0x0009AEC8 File Offset: 0x000992C8
	public void GetRelevantSegments(FixedRect bounds, List<ColliderSegmentReference> segmentsOut)
	{
		if (segmentsOut == null)
		{
			throw new ArgumentException("Please no be null");
		}
		segmentsOut.Clear();
		PhysicsWorldSortMode physicsWorldSortMode = this.SortMode;
		if (physicsWorldSortMode != PhysicsWorldSortMode.Horizontal)
		{
			if (physicsWorldSortMode == PhysicsWorldSortMode.Vertical)
			{
				Fixed bottom = bounds.Bottom;
				Fixed top = bounds.Top;
				int num = 0;
				while (num < this.colliderSegments.Count && this.colliderSegments[num].Bounds.Top < bottom)
				{
					num++;
				}
				int num2 = num;
				while (num2 < this.colliderSegments.Count && this.colliderSegments[num2].Bounds.Bottom <= top)
				{
					num2++;
				}
				for (int i = num; i < num2; i++)
				{
					if (this.colliderSegments[i].collider.Enabled && this.colliderSegments[i].Bounds.Overlaps(bounds))
					{
						segmentsOut.Add(this.colliderSegments[i]);
					}
				}
			}
		}
		else
		{
			Fixed left = bounds.Left;
			Fixed right = bounds.Right;
			int num3 = 0;
			while (num3 < this.colliderSegments.Count && this.colliderSegments[num3].Bounds.Right < left)
			{
				num3++;
			}
			int num4 = num3;
			while (num4 < this.colliderSegments.Count && this.colliderSegments[num4].Bounds.Left <= right)
			{
				num4++;
			}
			for (int j = num3; j < num4; j++)
			{
				if (this.colliderSegments[j].collider.Enabled && this.colliderSegments[j].Bounds.Overlaps(bounds))
				{
					segmentsOut.Add(this.colliderSegments[j]);
				}
			}
		}
	}

	// Token: 0x06001E62 RID: 7778 RVA: 0x0009B118 File Offset: 0x00099518
	public void RegisterCollider(IPhysicsCollider collider)
	{
		this.colliders.Add(collider);
		for (int i = 0; i < collider.Edge.SegmentCount; i++)
		{
			this.colliderSegments.Add(new ColliderSegmentReference(collider, i));
		}
		PhysicsWorld.SortSegments(this.colliderSegments, this.SortMode);
	}

	// Token: 0x06001E63 RID: 7779 RVA: 0x0009B170 File Offset: 0x00099570
	public bool CollidersContainPoint(Vector2F point)
	{
		foreach (IPhysicsCollider physicsCollider in this.colliders)
		{
			if (physicsCollider.ContainsPoint(point))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001E64 RID: 7780 RVA: 0x0009B1DC File Offset: 0x000995DC
	public bool CollidersContainEnvironmentBounds(EnvironmentBounds bounds, Vector2F center, int mask)
	{
		foreach (IPhysicsCollider physicsCollider in this.colliders)
		{
			if (physicsCollider.ContainsPoint(center))
			{
				return true;
			}
		}
		Vector2F vector2F = bounds.right - bounds.up;
		if (this.RaycastTerrain(bounds.up + center, vector2F.normalized, vector2F.magnitude, mask, this.raycastHitDataBuffer, RaycastFlags.Default, default(Fixed)) > 0)
		{
			return true;
		}
		Vector2F vector2F2 = bounds.down - bounds.right;
		if (this.RaycastTerrain(bounds.right + center, vector2F2.normalized, vector2F2.magnitude, mask, this.raycastHitDataBuffer, RaycastFlags.Default, default(Fixed)) > 0)
		{
			return true;
		}
		Vector2F vector2F3 = bounds.left - bounds.down;
		if (this.RaycastTerrain(bounds.down + center, vector2F3.normalized, vector2F3.magnitude, mask, this.raycastHitDataBuffer, RaycastFlags.Default, default(Fixed)) > 0)
		{
			return true;
		}
		Vector2F vector2F4 = bounds.up - bounds.left;
		return this.RaycastTerrain(bounds.left + center, vector2F4.normalized, vector2F4.magnitude, mask, this.raycastHitDataBuffer, RaycastFlags.Default, default(Fixed)) > 0;
	}

	// Token: 0x06001E65 RID: 7781 RVA: 0x0009B3A4 File Offset: 0x000997A4
	public static void SortSegments(List<ColliderSegmentReference> segments, PhysicsWorldSortMode sortMode)
	{
		ListUtil.LessThanDelegate<ColliderSegmentReference> lessThanFunc;
		if (sortMode == PhysicsWorldSortMode.Horizontal || sortMode != PhysicsWorldSortMode.Vertical)
		{
			if (PhysicsWorld.f__mg_cache0 == null)
			{
				PhysicsWorld.f__mg_cache0 = new ListUtil.LessThanDelegate<ColliderSegmentReference>(PhysicsWorld.segmentHorizontalLessThan);
			}
			lessThanFunc = PhysicsWorld.f__mg_cache0;
		}
		else
		{
			if (PhysicsWorld.f__mg_cache1 == null)
			{
				PhysicsWorld.f__mg_cache1 = new ListUtil.LessThanDelegate<ColliderSegmentReference>(PhysicsWorld.segmentVerticalLessThan);
			}
			lessThanFunc = PhysicsWorld.f__mg_cache1;
		}
		ListUtil.InsertionSort<ColliderSegmentReference>(segments, lessThanFunc);
	}

	// Token: 0x06001E66 RID: 7782 RVA: 0x0009B412 File Offset: 0x00099812
	public void SortSegments()
	{
		PhysicsWorld.SortSegments(this.colliderSegments, this.SortMode);
	}

	// Token: 0x06001E67 RID: 7783 RVA: 0x0009B428 File Offset: 0x00099828
	private static bool segmentHorizontalLessThan(ColliderSegmentReference a, ColliderSegmentReference b)
	{
		return a.Bounds.Left < b.Bounds.Left;
	}

	// Token: 0x06001E68 RID: 7784 RVA: 0x0009B458 File Offset: 0x00099858
	private static bool segmentVerticalLessThan(ColliderSegmentReference a, ColliderSegmentReference b)
	{
		return a.Bounds.Bottom < b.Bounds.Bottom;
	}

	// Token: 0x06001E69 RID: 7785 RVA: 0x0009B488 File Offset: 0x00099888
	public int RaycastTerrain(Vector2F origin, Vector2F normalizedDirection, Fixed maxDistance, int layerMask, RaycastHitData[] resultsBuffer, RaycastFlags flags = RaycastFlags.Default, Fixed tolerance = default(Fixed))
	{
		FixedRect raycastBoundingBox = PhysicsUtil.GetRaycastBoundingBox(origin, normalizedDirection, maxDistance);
		PhysicsWorld.relevantCollidersBuffer.Clear();
		foreach (IPhysicsCollider physicsCollider in this.colliders)
		{
			if (physicsCollider.Enabled && physicsCollider.LayerIntersects(layerMask) && physicsCollider.BoundingBox.Overlaps(raycastBoundingBox))
			{
				PhysicsWorld.relevantCollidersBuffer.Add(physicsCollider);
			}
		}
		int num = PhysicsWorld.RaycastColliders(PhysicsWorld.relevantCollidersBuffer, origin, normalizedDirection, maxDistance, layerMask, resultsBuffer, flags, tolerance);
		if (PhysicsWorld.EnableRaycastDebugging)
		{
			this.raycastsThisFrame.Add(new PhysicsWorld.RaycastDebugData
			{
				origin = origin,
				direction = normalizedDirection,
				distance = maxDistance,
				hasHit = (num > 0),
				hitPoint = ((num <= 0 || resultsBuffer == null) ? Vector2F.zero : resultsBuffer[0].point)
			});
		}
		return num;
	}

	// Token: 0x06001E6A RID: 7786 RVA: 0x0009B5B0 File Offset: 0x000999B0
	public static int RaycastColliders(List<IPhysicsCollider> localColliders, Vector2F origin, Vector2F normalizedDirection, Fixed maxDistance, int layerMask, RaycastHitData[] resultsBuffer, RaycastFlags flags = RaycastFlags.Default, Fixed tolerance = default(Fixed))
	{
		if (resultsBuffer != null && (flags & RaycastFlags.ResetBuffer) != RaycastFlags.None)
		{
			PhysicsWorld.ResetHitBuffer(resultsBuffer);
		}
		flags &= ~RaycastFlags.ResetBuffer;
		int num = 0;
		for (int i = 0; i < localColliders.Count; i++)
		{
			if (localColliders[i].Enabled)
			{
				num += PhysicsWorld.RaycastCollider(localColliders[i], origin, normalizedDirection, maxDistance, layerMask, resultsBuffer, flags, tolerance);
			}
		}
		return num;
	}

	// Token: 0x06001E6B RID: 7787 RVA: 0x0009B628 File Offset: 0x00099A28
	public static int RaycastCollider(IPhysicsCollider collider, Vector2F origin, Vector2F normalizedDirection, Fixed maxDistance, int layerMask, RaycastHitData[] resultsBuffer, RaycastFlags flags = RaycastFlags.Default, Fixed tolerance = default(Fixed))
	{
		if (!collider.LayerIntersects(layerMask))
		{
			return 0;
		}
		bool flag = (flags & RaycastFlags.ResetBuffer) != RaycastFlags.None;
		if (resultsBuffer != null && flag)
		{
			PhysicsWorld.ResetHitBuffer(resultsBuffer);
		}
		bool flag2 = (flags & RaycastFlags.EnableMultipleHits) != RaycastFlags.None;
		bool flag3 = (flags & RaycastFlags.EnableBackfaceCulling) != RaycastFlags.None;
		int num = 0;
		FixedRect raycastBoundingBox = PhysicsUtil.GetRaycastBoundingBox(origin, normalizedDirection, maxDistance);
		for (int i = 0; i < collider.Edge.SegmentCount; i++)
		{
			FixedRect segmentBoundingBox = collider.Edge.GetSegmentBoundingBox(i);
			if (raycastBoundingBox.Overlaps(segmentBoundingBox))
			{
				Vector2F point = collider.Edge.GetPoint(i);
				Vector2F nextPoint = collider.Edge.GetNextPoint(i);
				Fixed @fixed = 0;
				if (PhysicsWorld.RayLineSegmentIntersection(origin, normalizedDirection, point, nextPoint, maxDistance, ref @fixed, tolerance))
				{
					Vector2F normal = collider.Edge.GetNormal(i);
					if (!flag3 || !(Vector2F.Dot(normalizedDirection, normal) > 0))
					{
						if (resultsBuffer != null)
						{
							PhysicsWorld.insertHitData(resultsBuffer, new RaycastHitData
							{
								origin = origin,
								distance = @fixed,
								direction = normalizedDirection,
								layerMask = layerMask,
								remainingDistance = maxDistance - @fixed,
								collider = collider,
								segmentIndex = i,
								normal = normal,
								point = origin + normalizedDirection * @fixed
							});
						}
						num++;
						if (!flag2)
						{
							break;
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06001E6C RID: 7788 RVA: 0x0009B7B8 File Offset: 0x00099BB8
	private static void insertHitData(RaycastHitData[] hitArray, RaycastHitData data)
	{
		for (int i = 0; i < hitArray.Length; i++)
		{
			if (hitArray[i].distance >= data.distance)
			{
				for (int j = hitArray.Length - 1; j > i; j--)
				{
					hitArray[j] = hitArray[j - 1];
				}
				hitArray[i] = data;
				break;
			}
		}
	}

	// Token: 0x06001E6D RID: 7789 RVA: 0x0009B838 File Offset: 0x00099C38
	public static bool RayLineSegmentIntersection(Vector2F origin, Vector2F normalizedDirection, Vector2F segmentPointA, Vector2F segmentPointB, Fixed maxDistance, ref Fixed hitDistance, Fixed tolerance = default(Fixed))
	{
		Vector2F vector2F = origin - segmentPointA;
		Vector2F lhs = segmentPointB - segmentPointA;
		Vector2F rhs = new Vector2F(-normalizedDirection.y, normalizedDirection.x);
		Fixed @fixed = Vector2F.Dot(lhs, rhs);
		if (@fixed == 0)
		{
			hitDistance = Fixed.NaN;
			return false;
		}
		hitDistance = Vector2F.Determinant(lhs, vector2F) / @fixed;
		Fixed one = Vector2F.Dot(vector2F, rhs) / @fixed;
		return hitDistance >= -tolerance && hitDistance <= maxDistance + tolerance && one >= -tolerance && one <= 1 + tolerance;
	}

	// Token: 0x06001E6E RID: 7790 RVA: 0x0009B90C File Offset: 0x00099D0C
	public static bool LineSegmentIntersectionPoint(Vector2F point1A, Vector2F point1B, Vector2F point2A, Vector2F point2B, out Vector2F pointOut)
	{
		Vector2F vector2F = point1B - point1A;
		Vector2F normalized = vector2F.normalized;
		Fixed magnitude = vector2F.magnitude;
		Fixed @fixed = 0;
		bool result = PhysicsWorld.RayLineSegmentIntersection(point1A, normalized, point2A, point2B, magnitude, ref @fixed, default(Fixed));
		if (@fixed == Fixed.NaN)
		{
			pointOut = Vector2F.nan;
		}
		else
		{
			pointOut = point1A + normalized * @fixed;
		}
		return result;
	}

	// Token: 0x06001E6F RID: 7791 RVA: 0x0009B98C File Offset: 0x00099D8C
	public static void ResetHitBuffer(RaycastHitData[] buffer)
	{
		for (int i = 0; i < buffer.Length; i++)
		{
			buffer[i].distance = Fixed.MaxValue;
		}
	}

	// Token: 0x06001E70 RID: 7792 RVA: 0x0009B9BE File Offset: 0x00099DBE
	public void ClearFrameDebuggingInfo()
	{
		this.raycastsThisFrame.Clear();
	}

	// Token: 0x06001E71 RID: 7793 RVA: 0x0009B9CC File Offset: 0x00099DCC
	public void DrawDebugInfo()
	{
		if (!PhysicsWorld.EnableRaycastDebugging)
		{
			return;
		}
		foreach (PhysicsWorld.RaycastDebugData raycastDebugData in this.raycastsThisFrame)
		{
			GizmoUtil.GizmosDrawCircle((Vector2)raycastDebugData.origin, 0.02f, Color.green, 10);
			GizmoUtil.GizmosDrawArrow((Vector3)raycastDebugData.origin, (Vector3)(raycastDebugData.origin + raycastDebugData.direction * raycastDebugData.distance), Color.green, true, 0.1f, 33f);
			if (raycastDebugData.hasHit)
			{
				GizmoUtil.GizmosDrawCircle((Vector2)raycastDebugData.hitPoint, 0.01f, Color.red, 10);
			}
		}
	}

	// Token: 0x04001896 RID: 6294
	private List<IPhysicsCollider> colliders = new List<IPhysicsCollider>();

	// Token: 0x04001897 RID: 6295
	private List<ColliderSegmentReference> colliderSegments = new List<ColliderSegmentReference>();

	// Token: 0x04001898 RID: 6296
	private List<PhysicsWorld.RaycastDebugData> raycastsThisFrame = new List<PhysicsWorld.RaycastDebugData>();

	// Token: 0x04001899 RID: 6297
	private PhysicsWorldSortMode sortMode = PhysicsWorldSortMode.Vertical;

	// Token: 0x0400189A RID: 6298
	private RaycastHitData[] raycastHitDataBuffer = new RaycastHitData[1];

	// Token: 0x0400189B RID: 6299
	private static List<IPhysicsCollider> relevantCollidersBuffer = new List<IPhysicsCollider>();

	// Token: 0x0400189C RID: 6300
	public static bool EnableRaycastDebugging = false;

	// Token: 0x0400189D RID: 6301
	[CompilerGenerated]
	private static ListUtil.LessThanDelegate<ColliderSegmentReference> f__mg_cache0;

	// Token: 0x0400189E RID: 6302
	[CompilerGenerated]
	private static ListUtil.LessThanDelegate<ColliderSegmentReference> f__mg_cache1;

	// Token: 0x0200056B RID: 1387
	private struct RaycastDebugData
	{
		// Token: 0x0400189F RID: 6303
		public Vector2F origin;

		// Token: 0x040018A0 RID: 6304
		public Vector2F direction;

		// Token: 0x040018A1 RID: 6305
		public Fixed distance;

		// Token: 0x040018A2 RID: 6306
		public bool hasHit;

		// Token: 0x040018A3 RID: 6307
		public Vector2F hitPoint;
	}
}
