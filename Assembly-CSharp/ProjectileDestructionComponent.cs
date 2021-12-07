using System;

// Token: 0x0200036F RID: 879
public class ProjectileDestructionComponent : ArticleComponent, IArticleCollisionController
{
	// Token: 0x060012B1 RID: 4785 RVA: 0x0006AFEC File Offset: 0x000693EC
	public override void Init(IArticleDelegate articleDelegate, GameManager manager)
	{
		base.Init(articleDelegate, manager);
		this.instaKillHit = new Hit(new HitData
		{
			damage = 1000000f
		});
	}

	// Token: 0x060012B2 RID: 4786 RVA: 0x0006B01E File Offset: 0x0006941E
	public bool HandleHitBoxCollision(Hit hit, IHitOwner other, IHitOwner self, CollisionCheckType type, HitContext hitContext)
	{
		if (other.IsProjectile)
		{
			other.OnHitBoxCollision(hit, other, this.instaKillHit, ref hitContext.collisionPosition, false, false);
			return true;
		}
		return false;
	}

	// Token: 0x060012B3 RID: 4787 RVA: 0x0006B045 File Offset: 0x00069445
	bool IArticleCollisionController.OnHitPlatform()
	{
		return false;
	}

	// Token: 0x060012B4 RID: 4788 RVA: 0x0006B048 File Offset: 0x00069448
	bool IArticleCollisionController.OnHitTerrain()
	{
		return false;
	}

	// Token: 0x060012B5 RID: 4789 RVA: 0x0006B04B File Offset: 0x0006944B
	bool IArticleCollisionController.OnHitSuccess(Hit hit, IHitOwner other, ImpactType impactType)
	{
		return false;
	}

	// Token: 0x04000C30 RID: 3120
	private Hit instaKillHit;
}
