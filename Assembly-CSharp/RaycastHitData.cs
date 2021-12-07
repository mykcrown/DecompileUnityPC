using System;
using FixedPoint;

// Token: 0x0200056C RID: 1388
public struct RaycastHitData
{
	// Token: 0x17000674 RID: 1652
	// (get) Token: 0x06001E74 RID: 7796 RVA: 0x0009BAD3 File Offset: 0x00099ED3
	public SurfaceType surfaceType
	{
		get
		{
			return (this.collider != null) ? this.collider.Edge.GetSurfaceType(this.segmentIndex) : SurfaceType.Other;
		}
	}

	// Token: 0x040018A4 RID: 6308
	public IPhysicsCollider collider;

	// Token: 0x040018A5 RID: 6309
	public int segmentIndex;

	// Token: 0x040018A6 RID: 6310
	public Fixed distance;

	// Token: 0x040018A7 RID: 6311
	public Fixed remainingDistance;

	// Token: 0x040018A8 RID: 6312
	public Vector2F origin;

	// Token: 0x040018A9 RID: 6313
	public Vector2F direction;

	// Token: 0x040018AA RID: 6314
	public Vector2F point;

	// Token: 0x040018AB RID: 6315
	public Vector2F normal;

	// Token: 0x040018AC RID: 6316
	public int layerMask;

	// Token: 0x040018AD RID: 6317
	public static readonly RaycastHitData Empty = default(RaycastHitData);
}
