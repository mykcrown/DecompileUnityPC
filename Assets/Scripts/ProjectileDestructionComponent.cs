// Decompile from assembly: Assembly-CSharp.dll

using System;

public class ProjectileDestructionComponent : ArticleComponent, IArticleCollisionController
{
	private Hit instaKillHit;

	public override void Init(IArticleDelegate articleDelegate, GameManager manager)
	{
		base.Init(articleDelegate, manager);
		this.instaKillHit = new Hit(new HitData
		{
			damage = 1000000f
		});
	}

	public bool HandleHitBoxCollision(Hit hit, IHitOwner other, IHitOwner self, CollisionCheckType type, HitContext hitContext)
	{
		if (other.IsProjectile)
		{
			other.OnHitBoxCollision(hit, other, this.instaKillHit, ref hitContext.collisionPosition, false, false);
			return true;
		}
		return false;
	}

	bool IArticleCollisionController.OnHitPlatform()
	{
		return false;
	}

	bool IArticleCollisionController.OnHitTerrain()
	{
		return false;
	}

	bool IArticleCollisionController.OnHitSuccess(Hit hit, IHitOwner other, ImpactType impactType)
	{
		return false;
	}
}
