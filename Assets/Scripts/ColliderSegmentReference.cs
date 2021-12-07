// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class ColliderSegmentReference
{
	public IPhysicsCollider collider;

	public int segmentIndex;

	public FixedRect Bounds
	{
		get
		{
			return this.collider.Edge.GetSegmentBoundingBox(this.segmentIndex);
		}
	}

	public ColliderSegmentReference(IPhysicsCollider collider, int segmentIndex)
	{
		this.collider = collider;
		this.segmentIndex = segmentIndex;
	}
}
