using System;
using FixedPoint;

// Token: 0x02000546 RID: 1350
public interface IPhysicsCalculator
{
	// Token: 0x06001D8E RID: 7566
	bool IsCheckGrounded(PhysicsContext context);

	// Token: 0x06001D8F RID: 7567
	bool IgnoreCollisions(PhysicsContext context);

	// Token: 0x06001D90 RID: 7568
	bool IsCheckPlatformForGrounded(PhysicsContext context, Vector2F deltaPos);

	// Token: 0x06001D91 RID: 7569
	bool IsPlatformCollision(PhysicsContext context, Vector2F deltaPos);

	// Token: 0x06001D92 RID: 7570
	bool IsPlatformUndersideCollision(PhysicsContext context, Vector2F deltaPos);

	// Token: 0x06001D93 RID: 7571
	bool IsGroundCollision(PhysicsContext context, Vector2F deltaPos);

	// Token: 0x06001D94 RID: 7572
	Fixed GetGravity(PhysicsContext context);

	// Token: 0x06001D95 RID: 7573
	Vector2F GetAirFriction(PhysicsContext context);

	// Token: 0x06001D96 RID: 7574
	Fixed GetGroundFriction(PhysicsContext context, ref Vector3F newMovementVelocity);

	// Token: 0x06001D97 RID: 7575
	Fixed GetMaxDownwardVelocity(PhysicsContext context);

	// Token: 0x06001D98 RID: 7576
	Fixed GetMaxHorizontalAirVelocity(PhysicsContext context);
}
