using System;
using System.Collections.Generic;

// Token: 0x02000543 RID: 1347
public interface IPhysicsCollisionMotion
{
	// Token: 0x06001D7D RID: 7549
	bool HandleMotion(PhysicsContext context, List<CollisionData> sharedCollisions);
}
