// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IPhysicsCollisionMotion
{
	bool HandleMotion(PhysicsContext context, List<CollisionData> sharedCollisions);
}
