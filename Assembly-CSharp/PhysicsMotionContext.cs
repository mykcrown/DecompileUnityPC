using System;
using FixedPoint;

// Token: 0x02000564 RID: 1380
public class PhysicsMotionContext
{
	// Token: 0x04001880 RID: 6272
	public Vector3F initialVelocity;

	// Token: 0x04001881 RID: 6273
	public Vector3F initialMovementVelocity;

	// Token: 0x04001882 RID: 6274
	public Vector3F initialKnockbackVelocity;

	// Token: 0x04001883 RID: 6275
	public Vector3F travelDelta;

	// Token: 0x04001884 RID: 6276
	public Fixed maxTravelDist;

	// Token: 0x04001885 RID: 6277
	public Fixed distanceTraveled;

	// Token: 0x04001886 RID: 6278
	public bool completedMovement;
}
