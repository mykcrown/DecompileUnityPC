using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x020003BF RID: 959
public interface IHitOwner
{
	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x060014B1 RID: 5297
	// (set) Token: 0x060014B2 RID: 5298
	int HitOwnerID { get; set; }

	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x060014B3 RID: 5299
	List<Hit> Hits { get; }

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x060014B4 RID: 5300
	List<HurtBoxState> HurtBoxes { get; }

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x060014B5 RID: 5301
	List<HitBoxState> ShieldBoxes { get; }

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x060014B6 RID: 5302
	MoveController ActiveMove { get; }

	// Token: 0x060014B7 RID: 5303
	bool AllowClanking(HitData hitData, IHitOwner other);

	// Token: 0x060014B8 RID: 5304
	bool ForceCollisionChecks(CollisionCheckType type, HitData hitData);

	// Token: 0x060014B9 RID: 5305
	bool IsImmune(HitData hitData, IHitOwner enemy);

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x060014BA RID: 5306
	HitOwnerType Type { get; }

	// Token: 0x060014BB RID: 5307
	bool IsHitActive(Hit hit, IHitOwner other, bool excludePhantomHitbox);

	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x060014BC RID: 5308
	Vector3F Position { get; }

	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x060014BD RID: 5309
	Vector3F Center { get; }

	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x060014BE RID: 5310
	HorizontalDirection Facing { get; }

	// Token: 0x060014BF RID: 5311
	HorizontalDirection CalculateVictimFacing(bool hitWasReversed);

	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x060014C0 RID: 5312
	TeamNum Team { get; }

	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x060014C1 RID: 5313
	PlayerNum PlayerNum { get; }

	// Token: 0x17000406 RID: 1030
	// (get) Token: 0x060014C2 RID: 5314
	Fixed DamageMultiplier { get; }

	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x060014C3 RID: 5315
	Fixed StaleDamageMultiplier { get; }

	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x060014C4 RID: 5316
	Fixed HitLagMultiplier { get; }

	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x060014C5 RID: 5317
	bool IsProjectile { get; }

	// Token: 0x060014C6 RID: 5318
	bool HandleComponentHitInteraction(Hit otherHit, IHitOwner other, CollisionCheckType checkType, HitContext hitContext);

	// Token: 0x060014C7 RID: 5319
	bool ShouldCancelClankedMove(Hit myHit, Hit otherHit, IHitOwner other);

	// Token: 0x060014C8 RID: 5320
	void BeginHitLag(int hitLagFrames, IHitOwner owner, HitData hitData);

	// Token: 0x060014C9 RID: 5321
	bool ResistsHit(HitData hitData, IHitOwner hitInstigator, Vector3F hitPosition);

	// Token: 0x060014CA RID: 5322
	bool ArmorResistsHit(HitData hitData, IHitOwner hitInstigator, Vector3F hitPosition);

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x060014CB RID: 5323
	PlayerPhysicsController Physics { get; }

	// Token: 0x060014CC RID: 5324
	bool CanReflect(HitData hitData);

	// Token: 0x060014CD RID: 5325
	bool ShouldReflect(IHitOwner other, ref Vector3F collisionPoint, CollisionCheckType type, Hit myHit = null);

	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x060014CE RID: 5326
	bool IsInvincible { get; }

	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x060014CF RID: 5327
	bool IsAllyAssist { get; }

	// Token: 0x1700040D RID: 1037
	// (get) Token: 0x060014D0 RID: 5328
	bool AssistAbsorbsHits { get; }

	// Token: 0x060014D1 RID: 5329
	bool OnHitSuccess(Hit hit, IHitOwner other, ImpactType impactType, ref Vector3F hitPosition, ref Vector3F hitVelocity, HitContext hitContext);

	// Token: 0x060014D2 RID: 5330
	void OnDamageTaken(Fixed damage, ImpactType impactType);

	// Token: 0x060014D3 RID: 5331
	void ReceiveHit(HitData hitData, IHitOwner other, ImpactType impactType, HitContext hitContext);

	// Token: 0x060014D4 RID: 5332
	void OnHitBoxCollision(Hit myHit, IHitOwner other, Hit otherHit, ref Vector3F hitPostion, bool cancelMine, bool makeClank);
}
