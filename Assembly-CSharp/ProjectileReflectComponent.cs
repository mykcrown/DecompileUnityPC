using System;
using FixedPoint;

// Token: 0x02000370 RID: 880
public class ProjectileReflectComponent : ArticleComponent, IArticleCollisionController
{
	// Token: 0x060012B7 RID: 4791 RVA: 0x0006B058 File Offset: 0x00069458
	public bool HandleHitBoxCollision(Hit hit, IHitOwner other, IHitOwner self, CollisionCheckType type, HitContext hitContext)
	{
		bool flag = false;
		if (other.IsProjectile)
		{
			IArticleDelegate articleDelegate = other as IArticleDelegate;
			flag = (this.collidableObject == null || articleDelegate.Data == this.collidableObject);
			if (flag)
			{
				Vector3F totalVelocity = articleDelegate.Model.physicsModel.totalVelocity;
				Vector2F vector2F = new Vector2F(FixedMath.Abs(totalVelocity.x), FixedMath.Abs(totalVelocity.y));
				Fixed f = MathUtil.VectorToAngle(ref vector2F);
				Vector2F reflectedNormal;
				if (FixedMath.Abs(f) < this.ReflectAngle)
				{
					reflectedNormal = ((!(totalVelocity.x > 0)) ? Vector2F.right : Vector2F.left);
				}
				else
				{
					reflectedNormal = ((!(totalVelocity.y > 0)) ? Vector2F.up : Vector2F.down);
				}
				(other as ArticleController).OnReflectedArticle(hitContext.collisionPosition, reflectedNormal);
				(self as ArticleController).OnReflectedArticle(hitContext.collisionPosition, Vector2F.up);
			}
		}
		return flag;
	}

	// Token: 0x060012B8 RID: 4792 RVA: 0x0006B177 File Offset: 0x00069577
	bool IArticleCollisionController.OnHitPlatform()
	{
		return false;
	}

	// Token: 0x060012B9 RID: 4793 RVA: 0x0006B17A File Offset: 0x0006957A
	bool IArticleCollisionController.OnHitTerrain()
	{
		return false;
	}

	// Token: 0x060012BA RID: 4794 RVA: 0x0006B17D File Offset: 0x0006957D
	bool IArticleCollisionController.OnHitSuccess(Hit hit, IHitOwner other, ImpactType impactType)
	{
		return false;
	}

	// Token: 0x04000C31 RID: 3121
	public ArticleData collidableObject;

	// Token: 0x04000C32 RID: 3122
	public Fixed ReflectAngle;
}
