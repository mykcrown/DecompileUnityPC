// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PhysicsWorld
{
	private struct RaycastDebugData
	{
		public Vector2F origin;

		public Vector2F direction;

		public Fixed distance;

		public bool hasHit;

		public Vector2F hitPoint;
	}

	private List<IPhysicsCollider> colliders = new List<IPhysicsCollider>();

	private List<ColliderSegmentReference> colliderSegments = new List<ColliderSegmentReference>();

	private List<PhysicsWorld.RaycastDebugData> raycastsThisFrame = new List<PhysicsWorld.RaycastDebugData>();

	private PhysicsWorldSortMode sortMode = PhysicsWorldSortMode.Vertical;

	private RaycastHitData[] raycastHitDataBuffer = new RaycastHitData[1];

	private static List<IPhysicsCollider> relevantCollidersBuffer = new List<IPhysicsCollider>();

	public static bool EnableRaycastDebugging = false;

	private static ListUtil.LessThanDelegate<ColliderSegmentReference> __f__mg_cache0;

	private static ListUtil.LessThanDelegate<ColliderSegmentReference> __f__mg_cache1;

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

	public PhysicsWorld(IDevConsole devConsole)
	{
		if (devConsole != null)
		{
			devConsole.AddConsoleVariable<PhysicsWorldSortMode>("physics", "sort_mode", "Physics Segment Sort Mode", "Determines the sorting heuristic for line segments.", new Func<PhysicsWorldSortMode>(this.get_SortMode), new Action<PhysicsWorldSortMode>(this._PhysicsWorld_m__0));
		}
	}

	public List<IPhysicsCollider> GetRelevantColliders()
	{
		return this.colliders;
	}

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

	public void RegisterCollider(IPhysicsCollider collider)
	{
		this.colliders.Add(collider);
		for (int i = 0; i < collider.Edge.SegmentCount; i++)
		{
			this.colliderSegments.Add(new ColliderSegmentReference(collider, i));
		}
		PhysicsWorld.SortSegments(this.colliderSegments, this.SortMode);
	}

	public bool CollidersContainPoint(Vector2F point)
	{
		foreach (IPhysicsCollider current in this.colliders)
		{
			if (current.ContainsPoint(point))
			{
				return true;
			}
		}
		return false;
	}

	public bool CollidersContainEnvironmentBounds(EnvironmentBounds bounds, Vector2F center, int mask)
	{
		foreach (IPhysicsCollider current in this.colliders)
		{
			if (current.ContainsPoint(center))
			{
				bool result = true;
				return result;
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

	public static void SortSegments(List<ColliderSegmentReference> segments, PhysicsWorldSortMode sortMode)
	{
		ListUtil.LessThanDelegate<ColliderSegmentReference> lessThanFunc;
		if (sortMode == PhysicsWorldSortMode.Horizontal || sortMode != PhysicsWorldSortMode.Vertical)
		{
			if (PhysicsWorld.__f__mg_cache0 == null)
			{
				PhysicsWorld.__f__mg_cache0 = new ListUtil.LessThanDelegate<ColliderSegmentReference>(PhysicsWorld.segmentHorizontalLessThan);
			}
			lessThanFunc = PhysicsWorld.__f__mg_cache0;
		}
		else
		{
			if (PhysicsWorld.__f__mg_cache1 == null)
			{
				PhysicsWorld.__f__mg_cache1 = new ListUtil.LessThanDelegate<ColliderSegmentReference>(PhysicsWorld.segmentVerticalLessThan);
			}
			lessThanFunc = PhysicsWorld.__f__mg_cache1;
		}
		ListUtil.InsertionSort<ColliderSegmentReference>(segments, lessThanFunc);
	}

	public void SortSegments()
	{
		PhysicsWorld.SortSegments(this.colliderSegments, this.SortMode);
	}

	private static bool segmentHorizontalLessThan(ColliderSegmentReference a, ColliderSegmentReference b)
	{
		return a.Bounds.Left < b.Bounds.Left;
	}

	private static bool segmentVerticalLessThan(ColliderSegmentReference a, ColliderSegmentReference b)
	{
		return a.Bounds.Bottom < b.Bounds.Bottom;
	}

	public int RaycastTerrain(Vector2F origin, Vector2F normalizedDirection, Fixed maxDistance, int layerMask, RaycastHitData[] resultsBuffer, RaycastFlags flags = RaycastFlags.Default, Fixed tolerance = default(Fixed))
	{
		FixedRect raycastBoundingBox = PhysicsUtil.GetRaycastBoundingBox(origin, normalizedDirection, maxDistance);
		PhysicsWorld.relevantCollidersBuffer.Clear();
		foreach (IPhysicsCollider current in this.colliders)
		{
			if (current.Enabled && current.LayerIntersects(layerMask) && current.BoundingBox.Overlaps(raycastBoundingBox))
			{
				PhysicsWorld.relevantCollidersBuffer.Add(current);
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

	public static void ResetHitBuffer(RaycastHitData[] buffer)
	{
		for (int i = 0; i < buffer.Length; i++)
		{
			buffer[i].distance = Fixed.MaxValue;
		}
	}

	public void ClearFrameDebuggingInfo()
	{
		this.raycastsThisFrame.Clear();
	}

	public void DrawDebugInfo()
	{
		if (!PhysicsWorld.EnableRaycastDebugging)
		{
			return;
		}
		foreach (PhysicsWorld.RaycastDebugData current in this.raycastsThisFrame)
		{
			GizmoUtil.GizmosDrawCircle((Vector2)current.origin, 0.02f, Color.green, 10);
			GizmoUtil.GizmosDrawArrow((Vector3)current.origin, (Vector3)(current.origin + current.direction * current.distance), Color.green, true, 0.1f, 33f);
			if (current.hasHit)
			{
				GizmoUtil.GizmosDrawCircle((Vector2)current.hitPoint, 0.01f, Color.red, 10);
			}
		}
	}

	private void _PhysicsWorld_m__0(PhysicsWorldSortMode value)
	{
		this.SortMode = value;
	}
}
