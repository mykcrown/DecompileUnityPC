using System;
using FixedPoint;

// Token: 0x02000523 RID: 1315
public interface IMovePhysics
{
	// Token: 0x06001C6B RID: 7275
	void StopMovement(bool stopX, bool stopY, VelocityType velocityType);

	// Token: 0x06001C6C RID: 7276
	void AddVelocity(Vector2F velocity, int directionMultiplier, VelocityType velocityType);

	// Token: 0x06001C6D RID: 7277
	void SetOverride(PhysicsOverride physicsOverride);

	// Token: 0x06001C6E RID: 7278
	void ForceTranslate(Vector3F delta, bool checkFeet, bool detectCliffs = true);

	// Token: 0x1700060D RID: 1549
	// (get) Token: 0x06001C6F RID: 7279
	Vector3F Velocity { get; }

	// Token: 0x1700060E RID: 1550
	// (get) Token: 0x06001C70 RID: 7280
	Vector3F GroundedNormal { get; }

	// Token: 0x1700060F RID: 1551
	// (get) Token: 0x06001C71 RID: 7281
	Vector3F Center { get; }

	// Token: 0x17000610 RID: 1552
	// (get) Token: 0x06001C72 RID: 7282
	Vector3F MovementVelocity { get; }

	// Token: 0x17000611 RID: 1553
	// (get) Token: 0x06001C73 RID: 7283
	Fixed SteerMomentumMaxAnglePerFrame { get; }

	// Token: 0x17000612 RID: 1554
	// (get) Token: 0x06001C74 RID: 7284
	Fixed SteerMomentumMinOverallAngle { get; }

	// Token: 0x17000613 RID: 1555
	// (get) Token: 0x06001C75 RID: 7285
	Fixed SteerMomentumMaxOverallAngle { get; }

	// Token: 0x17000614 RID: 1556
	// (get) Token: 0x06001C76 RID: 7286
	bool SteerMomentumFaceVelocity { get; }

	// Token: 0x17000615 RID: 1557
	// (get) Token: 0x06001C77 RID: 7287
	bool PreventGroundedness { get; }
}
