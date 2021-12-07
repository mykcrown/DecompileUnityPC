// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class EdgeData
{
	public enum CacheFlag
	{
		BoundingBox = 1,
		SegmentBounds,
		Casts = 4,
		Normals = 8,
		SurfaceTypes = 16,
		Lengths = 32,
		All = -1,
		NoSurface = -49
	}

	private Vector2F[] points;

	private static readonly Fixed CEILING_DEGREES_TOLERANCE;

	private static readonly Fixed WALL_DEGREES_TOLERANCE;

	private static readonly Fixed LANDABLE_FLOOR_DEGREES_TOLERANCE;

	private Vector2F[] normals;

	private Vector2F[] clockwiseCasts;

	private FixedRect[] segmentBounds;

	private SurfaceType[] surfaceTypes;

	private Fixed edgeLength;

	private Fixed[] segmentLengths;

	private Fixed[] segmentNormalizedPositions;

	private EdgeData.CacheFlag cacheFlags;

	public bool IsLoop
	{
		get;
		private set;
	}

	public FixedRect BoundingBox
	{
		get;
		private set;
	}

	private static Fixed CeilingDotThreshold
	{
		get;
		set;
	}

	private static Fixed WallDotThreshold
	{
		get;
		set;
	}

	private static Fixed LandableFloorDotThreshold
	{
		get;
		set;
	}

	public int SegmentCount
	{
		get
		{
			return (!this.IsLoop) ? (this.points.Length - 1) : this.points.Length;
		}
	}

	public int Length
	{
		get
		{
			return this.points.Length;
		}
	}

	public Fixed EdgeLength
	{
		get
		{
			return this.edgeLength;
		}
	}

	static EdgeData()
	{
		EdgeData.CEILING_DEGREES_TOLERANCE = 35;
		EdgeData.WALL_DEGREES_TOLERANCE = 60;
		EdgeData.LANDABLE_FLOOR_DEGREES_TOLERANCE = (Fixed)66.4;
		EdgeData.CeilingDotThreshold = FixedMath.Cos(EdgeData.CEILING_DEGREES_TOLERANCE * FixedMath.Deg2Rad);
		EdgeData.WallDotThreshold = FixedMath.Cos(EdgeData.WALL_DEGREES_TOLERANCE * FixedMath.Deg2Rad);
		EdgeData.LandableFloorDotThreshold = FixedMath.Cos(EdgeData.LANDABLE_FLOOR_DEGREES_TOLERANCE * FixedMath.Deg2Rad);
	}

	public EdgeData(Vector2F rootPosition, Vector2F[] relativePoints, bool isLoop, EdgeData.CacheFlag cacheFlags = EdgeData.CacheFlag.All)
	{
		this.points = new Vector2F[relativePoints.Length];
		this.IsLoop = isLoop;
		this.cacheFlags = cacheFlags;
		for (int i = 0; i < this.points.Length; i++)
		{
			this.points[i] = relativePoints[i] + rootPosition;
		}
		this.normals = new Vector2F[this.SegmentCount];
		this.clockwiseCasts = new Vector2F[this.SegmentCount];
		this.segmentBounds = new FixedRect[this.SegmentCount];
		this.surfaceTypes = new SurfaceType[this.SegmentCount];
		this.segmentLengths = new Fixed[this.SegmentCount];
		this.segmentNormalizedPositions = new Fixed[this.SegmentCount];
		this.edgeLength = 0;
		this.RecalculateCachedValues();
	}

	public EdgeData(Vector2F[] points, bool isLoop, EdgeData.CacheFlag cacheFlags = EdgeData.CacheFlag.All) : this(Vector2F.zero, points, isLoop, cacheFlags)
	{
	}

	public EdgeData(EdgeData other) : this(other.points, other.IsLoop, other.cacheFlags)
	{
	}

	public EdgeData(Vector2F center, bool isLoop, EdgeData.CacheFlag cacheFlags, params Vector2F[] points) : this(center, points, isLoop, cacheFlags)
	{
	}

	public void LoadData(EdgeData other)
	{
		if (other.points.Length != this.points.Length)
		{
			UnityEngine.Debug.LogError("EdgeData.LoadData can only be used if they have the same number of points");
			return;
		}
		this.IsLoop = other.IsLoop;
		this.BoundingBox = other.BoundingBox;
		for (int i = 0; i < other.points.Length; i++)
		{
			this.points[i] = other.points[i];
			this.normals[i] = other.normals[i];
			this.clockwiseCasts[i] = other.clockwiseCasts[i];
			this.segmentBounds[i] = other.segmentBounds[i];
			this.surfaceTypes[i] = other.surfaceTypes[i];
			this.segmentLengths[i] = other.segmentLengths[i];
			this.segmentNormalizedPositions[i] = other.segmentNormalizedPositions[i];
		}
		this.edgeLength = other.edgeLength;
	}

	private bool hasFlag(EdgeData.CacheFlag flag)
	{
		return (this.cacheFlags & flag) > (EdgeData.CacheFlag)0;
	}

	public void RecalculateCachedValues()
	{
		if (this.hasFlag(EdgeData.CacheFlag.BoundingBox))
		{
			this.BoundingBox = FixedRect.CalculateBounds(this.points);
		}
		this.edgeLength = 0;
		for (int i = 0; i < this.SegmentCount; i++)
		{
			int num = (i + 1) % this.points.Length;
			if (this.hasFlag(EdgeData.CacheFlag.SegmentBounds))
			{
				this.segmentBounds[i] = PhysicsUtil.GetEdgeBoundingBox(this.points[i], this.points[num]);
			}
			if (this.hasFlag(EdgeData.CacheFlag.Casts))
			{
				this.clockwiseCasts[i] = (this.points[num] - this.points[i]).normalized;
			}
			if (this.hasFlag(EdgeData.CacheFlag.Normals))
			{
				this.normals[i] = -MathUtil.GetPerpendicularVector(this.clockwiseCasts[i]);
			}
			if (this.hasFlag(EdgeData.CacheFlag.SurfaceTypes))
			{
				SurfaceType surfaceType = SurfaceType.Other;
				Vector2F lhs = this.normals[i];
				if (Vector2F.Dot(lhs, Vector2F.up) >= EdgeData.LandableFloorDotThreshold)
				{
					surfaceType = SurfaceType.Floor;
				}
				else if (Vector2F.Dot(lhs, Vector2F.down) >= EdgeData.CeilingDotThreshold)
				{
					surfaceType = SurfaceType.Ceiling;
				}
				else if (Vector2F.Dot(lhs, Vector2F.right) >= EdgeData.WallDotThreshold || Vector2F.Dot(lhs, Vector2F.left) >= EdgeData.WallDotThreshold)
				{
					surfaceType = SurfaceType.Wall;
				}
				else
				{
					UnityEngine.Debug.LogWarningFormat("Segment {0} of collider was calculated as an 'Other' surface.  Will not be able to tech off of it.", new object[]
					{
						i
					});
				}
				this.surfaceTypes[i] = surfaceType;
			}
			if (this.hasFlag(EdgeData.CacheFlag.Lengths))
			{
				Vector2F vector2F = this.points[num] - this.points[i];
				this.segmentLengths[i] = vector2F.magnitude;
				this.edgeLength += this.segmentLengths[i];
			}
		}
		if (this.hasFlag(EdgeData.CacheFlag.Lengths))
		{
			Fixed one = 0;
			for (int j = 0; j < this.SegmentCount; j++)
			{
				if (this.edgeLength > 0)
				{
					this.segmentNormalizedPositions[j] = one / this.edgeLength;
				}
				else
				{
					this.segmentNormalizedPositions[j] = 0;
				}
				one += this.segmentLengths[j];
			}
		}
	}

	public Vector2F GetClockwiseCast(int fromIndex)
	{
		int num = MathUtil.Modulo(fromIndex, this.SegmentCount);
		return this.clockwiseCasts[num];
	}

	public Vector2F GetCounterClockwiseCast(int fromIndex)
	{
		int num = MathUtil.Modulo(fromIndex - 1, this.SegmentCount);
		return -this.clockwiseCasts[num];
	}

	public Vector2F GetNormal(int clockwiseIndex)
	{
		return this.normals[MathUtil.Modulo(clockwiseIndex, this.SegmentCount)];
	}

	public SurfaceType GetSurfaceType(int clockwiseIndex)
	{
		return this.surfaceTypes[MathUtil.Modulo(clockwiseIndex, this.SegmentCount)];
	}

	public FixedRect GetSegmentBoundingBox(int clockwiseIndex)
	{
		return this.segmentBounds[MathUtil.Modulo(clockwiseIndex, this.SegmentCount)];
	}

	public Fixed GetEdgeLengthAtSegment(int segmentIndex)
	{
		int num = MathUtil.Modulo(segmentIndex, this.SegmentCount);
		Fixed @fixed = 0;
		for (int i = 0; i < num; i++)
		{
			@fixed += this.segmentLengths[i];
		}
		return @fixed;
	}

	public Vector2F GetNearestAdjacentNormal(int clockwiseIndex, Vector2F point)
	{
		Fixed magnitude = (point - this.GetPoint(clockwiseIndex)).magnitude;
		Fixed magnitude2 = (point - this.GetPoint(clockwiseIndex + 1)).magnitude;
		if (magnitude < magnitude2)
		{
			if (this.IsLoop || clockwiseIndex > 0)
			{
				return this.GetNormal(clockwiseIndex - 1);
			}
		}
		else if (this.IsLoop || clockwiseIndex < this.Length - 1)
		{
			return this.GetNormal(clockwiseIndex + 1);
		}
		return Vector2F.zero;
	}

	private Vector2F getAdjacentNormalNearIntersection(int clockwiseIndex, Vector2F segmentPointA, Vector2F segmentPointB, Vector2F intersection, Fixed sqrTolerance)
	{
		Fixed sqrMagnitude = (segmentPointA - intersection).sqrMagnitude;
		Fixed sqrMagnitude2 = (segmentPointB - intersection).sqrMagnitude;
		if (sqrMagnitude < sqrMagnitude2)
		{
			if (sqrMagnitude <= sqrTolerance && (this.IsLoop || clockwiseIndex > 0))
			{
				return this.GetNormal(clockwiseIndex - 1);
			}
		}
		else if (sqrMagnitude2 <= sqrTolerance && (this.IsLoop || clockwiseIndex < this.Length - 2))
		{
			return this.GetNormal(clockwiseIndex + 1);
		}
		return Vector2F.zero;
	}

	public bool ShouldEnableReverseProjection(int clockwiseIndex, Vector2F delta, Vector2F pointA, Vector2F pointB, Vector2F intersection, Fixed sqrTolerance)
	{
		Vector2F normal = this.GetNormal(clockwiseIndex);
		Vector2F adjacentNormalNearIntersection = this.getAdjacentNormalNearIntersection(clockwiseIndex, pointA, pointB, intersection, sqrTolerance);
		return !(adjacentNormalNearIntersection != Vector2F.zero) || (Vector2F.Dot(delta, normal) < 0 && Vector2F.Dot(delta, adjacentNormalNearIntersection) < 0);
	}

	public Vector2F GetNextPoint(int index)
	{
		return this.GetPoint(index + 1);
	}

	public Vector2F GetPoint(int index)
	{
		return this.points[MathUtil.Modulo(index, this.Length)];
	}

	public Fixed GetLength(int index)
	{
		if (this.hasFlag(EdgeData.CacheFlag.Lengths))
		{
			return this.segmentLengths[MathUtil.Modulo(index, this.SegmentCount)];
		}
		return (this.GetNextPoint(index) - this.GetPoint(index)).magnitude;
	}

	public void SetPoints(EdgeData other)
	{
		if (other.points.Length != this.points.Length)
		{
			UnityEngine.Debug.LogError("Points array length mismatch.");
			return;
		}
		this.SetPoints(other.points);
	}

	public void SetPoints(params Vector2F[] newPoints)
	{
		this.SetPointsRelative(Vector2F.zero, newPoints);
	}

	public void SetPointsRelative(Vector2F rootPosition, Vector2F point1, Vector2F point2, Vector2F point3, Vector2F point4)
	{
		if (this.points.Length != 4)
		{
			UnityEngine.Debug.LogError("Points array length mismatch.");
			return;
		}
		this.points[0] = point1 + rootPosition;
		this.points[1] = point2 + rootPosition;
		this.points[2] = point3 + rootPosition;
		this.points[3] = point4 + rootPosition;
		this.RecalculateCachedValues();
	}

	public void SetPointsRelative(Vector2F rootPosition, params Vector2F[] newPoints)
	{
		if (newPoints.Length != this.points.Length)
		{
			UnityEngine.Debug.LogError("Points array length mismatch.");
			return;
		}
		for (int i = 0; i < newPoints.Length; i++)
		{
			this.points[i] = newPoints[i] + rootPosition;
		}
		this.RecalculateCachedValues();
	}

	public int GetSegmentIndexAtNormalizedLocation(Fixed normalizedLocation)
	{
		if (!this.IsLoop)
		{
			normalizedLocation = FixedMath.Clamp01(normalizedLocation);
		}
		else
		{
			normalizedLocation = FixedMath.Wrap01(normalizedLocation);
		}
		int result = this.SegmentCount - 1;
		for (int i = 1; i < this.SegmentCount; i++)
		{
			if (this.segmentNormalizedPositions[i] > normalizedLocation)
			{
				result = i - 1;
				break;
			}
		}
		return result;
	}

	public Vector2F GetPositionAtNormalizedLocation(Fixed normalizedLocation)
	{
		int segmentIndexAtNormalizedLocation = this.GetSegmentIndexAtNormalizedLocation(normalizedLocation);
		Vector2F point = this.GetPoint(segmentIndexAtNormalizedLocation);
		Vector2F nextPoint = this.GetNextPoint(segmentIndexAtNormalizedLocation);
		Fixed other = this.segmentNormalizedPositions[segmentIndexAtNormalizedLocation];
		Fixed one = 1;
		if (segmentIndexAtNormalizedLocation < this.SegmentCount - 1)
		{
			one = this.segmentNormalizedPositions[segmentIndexAtNormalizedLocation + 1];
		}
		Fixed t = (normalizedLocation - other) / (one - other);
		return Vector2F.Lerp(point, nextPoint, t);
	}

	public SurfaceType GetSurfaceTypeAtNormalizedLocation(Fixed normalizedLocation)
	{
		if (!this.IsLoop)
		{
			if (normalizedLocation < 0)
			{
				return SurfaceType.None;
			}
			if (normalizedLocation > 1)
			{
				return SurfaceType.None;
			}
		}
		int segmentIndexAtNormalizedLocation = this.GetSegmentIndexAtNormalizedLocation(normalizedLocation);
		return this.surfaceTypes[segmentIndexAtNormalizedLocation];
	}

	public Fixed GetNearestNormalizedLocation(int segmentIndex, Vector2F position)
	{
		Vector2F nearestPointOnEdgeSegment = this.GetNearestPointOnEdgeSegment(segmentIndex, position);
		Vector2F point = this.GetPoint(segmentIndex);
		Fixed edgeLengthAtSegment = this.GetEdgeLengthAtSegment(segmentIndex);
		Fixed one = edgeLengthAtSegment + (nearestPointOnEdgeSegment - point).magnitude;
		return one / this.edgeLength;
	}

	public Vector2F GetNearestPointOnEdgeSegment(int segmentIndex, Vector2F position)
	{
		Vector2F point = this.GetPoint(segmentIndex);
		Vector2F clockwiseCast = this.GetClockwiseCast(segmentIndex);
		Vector2F rhs = position - point;
		Fixed d = FixedMath.Clamp(Vector2F.Dot(clockwiseCast, rhs), 0, this.GetLength(segmentIndex));
		return point + clockwiseCast * d;
	}

	public Fixed ClampNormalizedMovementToLandableSurface(Fixed start, ref Fixed delta)
	{
		int segmentIndexAtNormalizedLocation = this.GetSegmentIndexAtNormalizedLocation(start);
		int segmentIndexAtNormalizedLocation2 = this.GetSegmentIndexAtNormalizedLocation(start + delta);
		int num = (int)FixedMath.Sign(delta);
		int num2 = segmentIndexAtNormalizedLocation;
		while (num2 != segmentIndexAtNormalizedLocation2 && this.GetSurfaceType(num2 + num) == SurfaceType.Floor)
		{
			num2 += num;
			num2 = MathUtil.Modulo(num2, this.SegmentCount);
		}
		if (num2 == segmentIndexAtNormalizedLocation2)
		{
			if (this.IsLoop)
			{
				return start + delta;
			}
			return FixedMath.Clamp01(start + delta);
		}
		else
		{
			if (num < 0)
			{
				Fixed @fixed = this.segmentNormalizedPositions[MathUtil.Modulo(num2, this.SegmentCount)];
				if (@fixed > start)
				{
					delta = -(start + (1 - @fixed));
				}
				else
				{
					delta = -(start - @fixed);
				}
				return @fixed;
			}
			Fixed fixed2 = this.segmentNormalizedPositions[MathUtil.Modulo(num2 + 1, this.SegmentCount)] - Fixed.MinValue;
			if (fixed2 < start)
			{
				delta = 1 - start + fixed2;
			}
			else
			{
				delta = fixed2 - start;
			}
			return fixed2;
		}
	}
}
