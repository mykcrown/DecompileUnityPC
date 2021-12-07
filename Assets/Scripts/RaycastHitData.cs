// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public struct RaycastHitData
{
	public IPhysicsCollider collider;

	public int segmentIndex;

	public Fixed distance;

	public Fixed remainingDistance;

	public Vector2F origin;

	public Vector2F direction;

	public Vector2F point;

	public Vector2F normal;

	public int layerMask;

	public static readonly RaycastHitData Empty = default(RaycastHitData);

	public SurfaceType surfaceType
	{
		get
		{
			return (this.collider != null) ? this.collider.Edge.GetSurfaceType(this.segmentIndex) : SurfaceType.Other;
		}
	}
}
