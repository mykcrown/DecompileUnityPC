using System;

// Token: 0x02000367 RID: 871
public interface IArticleCollisionController
{
	// Token: 0x0600128A RID: 4746
	bool OnHitPlatform();

	// Token: 0x0600128B RID: 4747
	bool OnHitTerrain();

	// Token: 0x0600128C RID: 4748
	bool OnHitSuccess(Hit hit, IHitOwner other, ImpactType impactType);

	// Token: 0x0600128D RID: 4749
	bool HandleHitBoxCollision(Hit hit, IHitOwner other, IHitOwner self, CollisionCheckType type, HitContext hitContext);
}
