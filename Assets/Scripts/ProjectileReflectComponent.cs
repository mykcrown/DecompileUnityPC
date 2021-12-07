// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class ProjectileReflectComponent : ArticleComponent, IArticleCollisionController
{
	public ArticleData collidableObject;

	public Fixed ReflectAngle;

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
