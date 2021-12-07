﻿using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x0200056D RID: 1389
public class PlayerPhysicsCollisionMotion : IPhysicsCollisionMotion
{
	// Token: 0x06001E77 RID: 7799 RVA: 0x0009BB20 File Offset: 0x00099F20
	public bool HandleMotion(PhysicsContext context, List<CollisionData> sharedCollisions)
	{
		PhysicsMotionContext motionContext = context.motionContext;
		CollisionData collision = sharedCollisions[0];
		if ((context.shouldHaltOnCollision != null && context.shouldHaltOnCollision(motionContext, collision)) || context.model.RestoreVelocity == RestoreVelocityType.PreventRestore)
		{
			return true;
		}
		if (context.model.RestoreVelocity == RestoreVelocityType.Restore)
		{
			Fixed one = Vector3F.Dot(collision.normal, Vector3F.down);
			if (one > 0 && one < PlayerPhysicsCollisionMotion.MAX_DOWN_DOT_TO_RESTORE_VELOCITY)
			{
				Vector3F vector3F;
				if (Vector3F.Dot(collision.normal, Vector3F.right) > 0)
				{
					vector3F = -MathUtil.GetPerpendicularVector(collision.normal);
				}
				else
				{
					vector3F = MathUtil.GetPerpendicularVector(collision.normal);
				}
				Fixed x = motionContext.initialVelocity.y * vector3F.x / vector3F.y;
				Vector3F newVelocity = new Vector3F(x, motionContext.initialVelocity.y);
				if (newVelocity.sqrMagnitude == 0)
				{
					context.model.SetVelocity(newVelocity, VelocityType.Movement);
				}
				else
				{
					context.model.SetVelocity(newVelocity, VelocityType.Total);
				}
				motionContext.maxTravelDist = context.model.totalVelocity.magnitude * WTime.fixedDeltaTime;
				motionContext.travelDelta = (motionContext.maxTravelDist - motionContext.distanceTraveled) * context.model.totalVelocity.normalized;
				motionContext.completedMovement = (motionContext.distanceTraveled >= motionContext.maxTravelDist);
			}
		}
		return false;
	}

	// Token: 0x040018AE RID: 6318
	private static Fixed MAX_DOWN_DOT_TO_RESTORE_VELOCITY = (Fixed)0.9;
}
