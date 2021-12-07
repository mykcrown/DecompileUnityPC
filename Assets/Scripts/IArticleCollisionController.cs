// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IArticleCollisionController
{
	bool OnHitPlatform();

	bool OnHitTerrain();

	bool OnHitSuccess(Hit hit, IHitOwner other, ImpactType impactType);

	bool HandleHitBoxCollision(Hit hit, IHitOwner other, IHitOwner self, CollisionCheckType type, HitContext hitContext);
}
