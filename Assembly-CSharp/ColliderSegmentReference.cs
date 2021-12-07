using System;
using FixedPoint;

// Token: 0x02000568 RID: 1384
public class ColliderSegmentReference
{
	// Token: 0x06001E5B RID: 7771 RVA: 0x0009ADEC File Offset: 0x000991EC
	public ColliderSegmentReference(IPhysicsCollider collider, int segmentIndex)
	{
		this.collider = collider;
		this.segmentIndex = segmentIndex;
	}

	// Token: 0x17000672 RID: 1650
	// (get) Token: 0x06001E5C RID: 7772 RVA: 0x0009AE02 File Offset: 0x00099202
	public FixedRect Bounds
	{
		get
		{
			return this.collider.Edge.GetSegmentBoundingBox(this.segmentIndex);
		}
	}

	// Token: 0x04001891 RID: 6289
	public IPhysicsCollider collider;

	// Token: 0x04001892 RID: 6290
	public int segmentIndex;
}
