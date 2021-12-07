using System;
using FixedPoint;

// Token: 0x02000570 RID: 1392
public interface IPhysicsDelegate
{
	// Token: 0x17000675 RID: 1653
	// (get) Token: 0x06001E79 RID: 7801
	ICharacterPhysicsData Data { get; }

	// Token: 0x17000676 RID: 1654
	// (get) Token: 0x06001E7A RID: 7802
	CharacterPhysicsData DefaultData { get; }

	// Token: 0x06001E7B RID: 7803
	bool IsDirectionHeld(HorizontalDirection direction);

	// Token: 0x17000677 RID: 1655
	// (get) Token: 0x06001E7C RID: 7804
	Fixed GetDirectionHeldAmount { get; }

	// Token: 0x17000678 RID: 1656
	// (get) Token: 0x06001E7D RID: 7805
	IPlayerState State { get; }

	// Token: 0x17000679 RID: 1657
	// (get) Token: 0x06001E7E RID: 7806
	HorizontalDirection Facing { get; }

	// Token: 0x1700067A RID: 1658
	// (get) Token: 0x06001E7F RID: 7807
	MoveData CurrentMove { get; }

	// Token: 0x1700067B RID: 1659
	// (get) Token: 0x06001E80 RID: 7808
	MoveController ActiveMove { get; }

	// Token: 0x1700067C RID: 1660
	// (get) Token: 0x06001E81 RID: 7809
	IBodyOwner Body { get; }

	// Token: 0x06001E82 RID: 7810
	Fixed GetHorizontalAcceleration(bool grounded);

	// Token: 0x1700067D RID: 1661
	// (get) Token: 0x06001E83 RID: 7811
	bool IsUnderContinuousForce { get; }

	// Token: 0x1700067E RID: 1662
	// (get) Token: 0x06001E84 RID: 7812
	ICombatController Combat { get; }

	// Token: 0x1700067F RID: 1663
	// (get) Token: 0x06001E85 RID: 7813
	IGrabController GrabController { get; }

	// Token: 0x17000680 RID: 1664
	// (get) Token: 0x06001E86 RID: 7814
	bool IsRotationRolled { get; }

	// Token: 0x06001E87 RID: 7815
	Fixed CalculateMaxHorizontalSpeed();

	// Token: 0x06001E88 RID: 7816
	void OnLand(ref Vector3F previousVelocity);

	// Token: 0x06001E89 RID: 7817
	void OnFall();

	// Token: 0x06001E8A RID: 7818
	void OnJump();

	// Token: 0x06001E8B RID: 7819
	void OnGroundBounce();

	// Token: 0x06001E8C RID: 7820
	TechType AvailableTech(CollisionData collision);

	// Token: 0x06001E8D RID: 7821
	void PerformTech(TechType techType, CollisionData collision);

	// Token: 0x06001E8E RID: 7822
	bool ShouldFallThroughPlatforms();

	// Token: 0x06001E8F RID: 7823
	bool IsPlatformLastDropped(IPhysicsCollider platformCollider);

	// Token: 0x06001E90 RID: 7824
	bool ShouldBounce();

	// Token: 0x06001E91 RID: 7825
	bool ShouldMaintainVelocityOnCollision();

	// Token: 0x06001E92 RID: 7826
	bool IgnorePhysicsCollisions();
}
