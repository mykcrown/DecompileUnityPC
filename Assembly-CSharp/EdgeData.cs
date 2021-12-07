using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000541 RID: 1345
public class EdgeData
{
	// Token: 0x06001D53 RID: 7507 RVA: 0x00096188 File Offset: 0x00094588
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

	// Token: 0x06001D54 RID: 7508 RVA: 0x0009626C File Offset: 0x0009466C
	public EdgeData(Vector2F[] points, bool isLoop, EdgeData.CacheFlag cacheFlags = EdgeData.CacheFlag.All) : this(Vector2F.zero, points, isLoop, cacheFlags)
	{
	}

	// Token: 0x06001D55 RID: 7509 RVA: 0x0009627C File Offset: 0x0009467C
	public EdgeData(EdgeData other) : this(other.points, other.IsLoop, other.cacheFlags)
	{
	}

	// Token: 0x06001D56 RID: 7510 RVA: 0x00096296 File Offset: 0x00094696
	public EdgeData(Vector2F center, bool isLoop, EdgeData.CacheFlag cacheFlags, params Vector2F[] points) : this(center, points, isLoop, cacheFlags)
	{
	}

	// Token: 0x17000643 RID: 1603
	// (get) Token: 0x06001D57 RID: 7511 RVA: 0x000962A3 File Offset: 0x000946A3
	// (set) Token: 0x06001D58 RID: 7512 RVA: 0x000962AB File Offset: 0x000946AB
	public bool IsLoop { get; private set; }

	// Token: 0x17000644 RID: 1604
	// (get) Token: 0x06001D59 RID: 7513 RVA: 0x000962B4 File Offset: 0x000946B4
	// (set) Token: 0x06001D5A RID: 7514 RVA: 0x000962BC File Offset: 0x000946BC
	public FixedRect BoundingBox { get; private set; }

	// Token: 0x17000645 RID: 1605
	// (get) Token: 0x06001D5B RID: 7515 RVA: 0x000962C5 File Offset: 0x000946C5
	// (set) Token: 0x06001D5C RID: 7516 RVA: 0x000962CC File Offset: 0x000946CC
	private static Fixed CeilingDotThreshold { get; set; } = FixedMath.Cos(EdgeData.CEILING_DEGREES_TOLERANCE * FixedMath.Deg2Rad);

	// Token: 0x17000646 RID: 1606
	// (get) Token: 0x06001D5D RID: 7517 RVA: 0x000962D4 File Offset: 0x000946D4
	// (set) Token: 0x06001D5E RID: 7518 RVA: 0x000962DB File Offset: 0x000946DB
	private static Fixed WallDotThreshold { get; set; } = FixedMath.Cos(EdgeData.WALL_DEGREES_TOLERANCE * FixedMath.Deg2Rad);

	// Token: 0x17000647 RID: 1607
	// (get) Token: 0x06001D5F RID: 7519 RVA: 0x000962E3 File Offset: 0x000946E3
	// (set) Token: 0x06001D60 RID: 7520 RVA: 0x000962EA File Offset: 0x000946EA
	private static Fixed LandableFloorDotThreshold { get; set; } = FixedMath.Cos(EdgeData.LANDABLE_FLOOR_DEGREES_TOLERANCE * FixedMath.Deg2Rad);

	// Token: 0x17000648 RID: 1608
	// (get) Token: 0x06001D61 RID: 7521 RVA: 0x000962F2 File Offset: 0x000946F2
	public int SegmentCount
	{
		get
		{
			return (!this.IsLoop) ? (this.points.Length - 1) : this.points.Length;
		}
	}

	// Token: 0x17000649 RID: 1609
	// (get) Token: 0x06001D62 RID: 7522 RVA: 0x00096316 File Offset: 0x00094716
	public int Length
	{
		get
		{
			return this.points.Length;
		}
	}

	// Token: 0x1700064A RID: 1610
	// (get) Token: 0x06001D63 RID: 7523 RVA: 0x00096320 File Offset: 0x00094720
	public Fixed EdgeLength
	{
		get
		{
			return this.edgeLength;
		}
	}

	// Token: 0x06001D64 RID: 7524 RVA: 0x00096328 File Offset: 0x00094728
	public void LoadData(EdgeData other)
	{
		if (other.points.Length != this.points.Length)
		{
			Debug.LogError("EdgeData.LoadData can only be used if they have the same number of points");
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

	// Token: 0x06001D65 RID: 7525 RVA: 0x0009646E File Offset: 0x0009486E
	private bool hasFlag(EdgeData.CacheFlag flag)
	{
		return (this.cacheFlags & flag) > (EdgeData.CacheFlag)0;
	}

	// Token: 0x06001D66 RID: 7526 RVA: 0x0009647C File Offset: 0x0009487C
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
					Debug.LogWarningFormat("Segment {0} of collider was calculated as an 'Other' surface.  Will not be able to tech off of it.", new object[]
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

	// Token: 0x06001D67 RID: 7527 RVA: 0x0009676C File Offset: 0x00094B6C
	public Vector2F GetClockwiseCast(int fromIndex)
	{
		int num = MathUtil.Modulo(fromIndex, this.SegmentCount);
		return this.clockwiseCasts[num];
	}

	// Token: 0x06001D68 RID: 7528 RVA: 0x00096798 File Offset: 0x00094B98
	public Vector2F GetCounterClockwiseCast(int fromIndex)
	{
		int num = MathUtil.Modulo(fromIndex - 1, this.SegmentCount);
		return -this.clockwiseCasts[num];
	}

	// Token: 0x06001D69 RID: 7529 RVA: 0x000967CA File Offset: 0x00094BCA
	public Vector2F GetNormal(int clockwiseIndex)
	{
		return this.normals[MathUtil.Modulo(clockwiseIndex, this.SegmentCount)];
	}

	// Token: 0x06001D6A RID: 7530 RVA: 0x000967E8 File Offset: 0x00094BE8
	public SurfaceType GetSurfaceType(int clockwiseIndex)
	{
		return this.surfaceTypes[MathUtil.Modulo(clockwiseIndex, this.SegmentCount)];
	}

	// Token: 0x06001D6B RID: 7531 RVA: 0x000967FD File Offset: 0x00094BFD
	public FixedRect GetSegmentBoundingBox(int clockwiseIndex)
	{
		return this.segmentBounds[MathUtil.Modulo(clockwiseIndex, this.SegmentCount)];
	}

	// Token: 0x06001D6C RID: 7532 RVA: 0x0009681C File Offset: 0x00094C1C
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

	// Token: 0x06001D6D RID: 7533 RVA: 0x00096868 File Offset: 0x00094C68
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

	// Token: 0x06001D6E RID: 7534 RVA: 0x000968F8 File Offset: 0x00094CF8
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

	// Token: 0x06001D6F RID: 7535 RVA: 0x00096998 File Offset: 0x00094D98
	public bool ShouldEnableReverseProjection(int clockwiseIndex, Vector2F delta, Vector2F pointA, Vector2F pointB, Vector2F intersection, Fixed sqrTolerance)
	{
		Vector2F normal = this.GetNormal(clockwiseIndex);
		Vector2F adjacentNormalNearIntersection = this.getAdjacentNormalNearIntersection(clockwiseIndex, pointA, pointB, intersection, sqrTolerance);
		return !(adjacentNormalNearIntersection != Vector2F.zero) || (Vector2F.Dot(delta, normal) < 0 && Vector2F.Dot(delta, adjacentNormalNearIntersection) < 0);
	}

	// Token: 0x06001D70 RID: 7536 RVA: 0x000969F0 File Offset: 0x00094DF0
	public Vector2F GetNextPoint(int index)
	{
		return this.GetPoint(index + 1);
	}

	// Token: 0x06001D71 RID: 7537 RVA: 0x000969FB File Offset: 0x00094DFB
	public Vector2F GetPoint(int index)
	{
		return this.points[MathUtil.Modulo(index, this.Length)];
	}

	// Token: 0x06001D72 RID: 7538 RVA: 0x00096A1C File Offset: 0x00094E1C
	public Fixed GetLength(int index)
	{
		if (this.hasFlag(EdgeData.CacheFlag.Lengths))
		{
			return this.segmentLengths[MathUtil.Modulo(index, this.SegmentCount)];
		}
		return (this.GetNextPoint(index) - this.GetPoint(index)).magnitude;
	}

	// Token: 0x06001D73 RID: 7539 RVA: 0x00096A6E File Offset: 0x00094E6E
	public void SetPoints(EdgeData other)
	{
		if (other.points.Length != this.points.Length)
		{
			Debug.LogError("Points array length mismatch.");
			return;
		}
		this.SetPoints(other.points);
	}

	// Token: 0x06001D74 RID: 7540 RVA: 0x00096A9C File Offset: 0x00094E9C
	public void SetPoints(params Vector2F[] newPoints)
	{
		this.SetPointsRelative(Vector2F.zero, newPoints);
	}

	// Token: 0x06001D75 RID: 7541 RVA: 0x00096AAC File Offset: 0x00094EAC
	public void SetPointsRelative(Vector2F rootPosition, Vector2F point1, Vector2F point2, Vector2F point3, Vector2F point4)
	{
		if (this.points.Length != 4)
		{
			Debug.LogError("Points array length mismatch.");
			return;
		}
		this.points[0] = point1 + rootPosition;
		this.points[1] = point2 + rootPosition;
		this.points[2] = point3 + rootPosition;
		this.points[3] = point4 + rootPosition;
		this.RecalculateCachedValues();
	}

	// Token: 0x06001D76 RID: 7542 RVA: 0x00096B3C File Offset: 0x00094F3C
	public void SetPointsRelative(Vector2F rootPosition, params Vector2F[] newPoints)
	{
		if (newPoints.Length != this.points.Length)
		{
			Debug.LogError("Points array length mismatch.");
			return;
		}
		for (int i = 0; i < newPoints.Length; i++)
		{
			this.points[i] = newPoints[i] + rootPosition;
		}
		this.RecalculateCachedValues();
	}

	// Token: 0x06001D77 RID: 7543 RVA: 0x00096BA4 File Offset: 0x00094FA4
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

	// Token: 0x06001D78 RID: 7544 RVA: 0x00096C18 File Offset: 0x00095018
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

	// Token: 0x06001D79 RID: 7545 RVA: 0x00096C9C File Offset: 0x0009509C
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

	// Token: 0x06001D7A RID: 7546 RVA: 0x00096CE0 File Offset: 0x000950E0
	public Fixed GetNearestNormalizedLocation(int segmentIndex, Vector2F position)
	{
		Vector2F nearestPointOnEdgeSegment = this.GetNearestPointOnEdgeSegment(segmentIndex, position);
		Vector2F point = this.GetPoint(segmentIndex);
		Fixed edgeLengthAtSegment = this.GetEdgeLengthAtSegment(segmentIndex);
		Fixed one = edgeLengthAtSegment + (nearestPointOnEdgeSegment - point).magnitude;
		return one / this.edgeLength;
	}

	// Token: 0x06001D7B RID: 7547 RVA: 0x00096D2C File Offset: 0x0009512C
	public Vector2F GetNearestPointOnEdgeSegment(int segmentIndex, Vector2F position)
	{
		Vector2F point = this.GetPoint(segmentIndex);
		Vector2F clockwiseCast = this.GetClockwiseCast(segmentIndex);
		Vector2F rhs = position - point;
		Fixed d = FixedMath.Clamp(Vector2F.Dot(clockwiseCast, rhs), 0, this.GetLength(segmentIndex));
		return point + clockwiseCast * d;
	}

	// Token: 0x06001D7C RID: 7548 RVA: 0x00096D78 File Offset: 0x00095178
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

	// Token: 0x040017F3 RID: 6131
	private Vector2F[] points;

	// Token: 0x040017F6 RID: 6134
	private static readonly Fixed CEILING_DEGREES_TOLERANCE = 35;

	// Token: 0x040017F7 RID: 6135
	private static readonly Fixed WALL_DEGREES_TOLERANCE = 60;

	// Token: 0x040017F8 RID: 6136
	private static readonly Fixed LANDABLE_FLOOR_DEGREES_TOLERANCE = (Fixed)66.4;

	// Token: 0x040017FC RID: 6140
	private Vector2F[] normals;

	// Token: 0x040017FD RID: 6141
	private Vector2F[] clockwiseCasts;

	// Token: 0x040017FE RID: 6142
	private FixedRect[] segmentBounds;

	// Token: 0x040017FF RID: 6143
	private SurfaceType[] surfaceTypes;

	// Token: 0x04001800 RID: 6144
	private Fixed edgeLength;

	// Token: 0x04001801 RID: 6145
	private Fixed[] segmentLengths;

	// Token: 0x04001802 RID: 6146
	private Fixed[] segmentNormalizedPositions;

	// Token: 0x04001803 RID: 6147
	private EdgeData.CacheFlag cacheFlags;

	// Token: 0x02000542 RID: 1346
	public enum CacheFlag
	{
		// Token: 0x04001805 RID: 6149
		BoundingBox = 1,
		// Token: 0x04001806 RID: 6150
		SegmentBounds,
		// Token: 0x04001807 RID: 6151
		Casts = 4,
		// Token: 0x04001808 RID: 6152
		Normals = 8,
		// Token: 0x04001809 RID: 6153
		SurfaceTypes = 16,
		// Token: 0x0400180A RID: 6154
		Lengths = 32,
		// Token: 0x0400180B RID: 6155
		All = -1,
		// Token: 0x0400180C RID: 6156
		NoSurface = -49
	}
}
