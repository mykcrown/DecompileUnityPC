using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x02000574 RID: 1396
public class ProjectilePhysicsCollisionMotion : IPhysicsCollisionMotion
{
	// Token: 0x06001F18 RID: 7960 RVA: 0x0009F1C8 File Offset: 0x0009D5C8
	public ProjectilePhysicsCollisionMotion()
	{
		this.groundLayer = LayerMask.NameToLayer(Layers.Ground);
		this.GroundMask = 1 << this.groundLayer;
		this.platformLayer = LayerMask.NameToLayer(Layers.Platform);
		this.PlatformMask = 1 << this.platformLayer;
	}

	// Token: 0x170006BE RID: 1726
	// (get) Token: 0x06001F19 RID: 7961 RVA: 0x0009F21D File Offset: 0x0009D61D
	// (set) Token: 0x06001F1A RID: 7962 RVA: 0x0009F225 File Offset: 0x0009D625
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x170006BF RID: 1727
	// (get) Token: 0x06001F1B RID: 7963 RVA: 0x0009F22E File Offset: 0x0009D62E
	// (set) Token: 0x06001F1C RID: 7964 RVA: 0x0009F236 File Offset: 0x0009D636
	public int GroundMask { get; private set; }

	// Token: 0x170006C0 RID: 1728
	// (get) Token: 0x06001F1D RID: 7965 RVA: 0x0009F23F File Offset: 0x0009D63F
	// (set) Token: 0x06001F1E RID: 7966 RVA: 0x0009F247 File Offset: 0x0009D647
	public int PlatformMask { get; private set; }

	// Token: 0x170006C1 RID: 1729
	// (get) Token: 0x06001F1F RID: 7967 RVA: 0x0009F250 File Offset: 0x0009D650
	public int GroundAndPlatformMask
	{
		get
		{
			return this.GroundMask | this.PlatformMask;
		}
	}

	// Token: 0x06001F20 RID: 7968 RVA: 0x0009F260 File Offset: 0x0009D660
	public bool HandleMotion(PhysicsContext context, List<CollisionData> sharedCollisions)
	{
		PhysicsMotionContext motionContext = context.motionContext;
		CollisionData collisionData = sharedCollisions[0];
		ArticleData articleData = context.articleData;
		Vector3F vector3F = collisionData.normal;
		bool flag = false;
		if (collisionData.terrainCollider != null)
		{
			if (collisionData.terrainCollider.Layer == this.groundLayer)
			{
				ArticleController.ComponentExecution<IArticleCollisionController> execute = (IArticleCollisionController hitController) => hitController.OnHitTerrain();
				flag |= context.articleController.ExecuteArticleComponents<IArticleCollisionController>(execute);
			}
			else if (collisionData.terrainCollider.Layer == this.platformLayer)
			{
				ArticleController.ComponentExecution<IArticleCollisionController> execute2 = (IArticleCollisionController hitController) => hitController.OnHitPlatform();
				flag |= context.articleController.ExecuteArticleComponents<IArticleCollisionController>(execute2);
			}
		}
		if (!flag)
		{
			switch (articleData.environmentCollisionBehavior)
			{
			case ArticleCollisionBehavior.Explode:
				context.articleController.Explode(true);
				return true;
			case ArticleCollisionBehavior.Halt:
				context.model.ClearVelocity(true, true, true, VelocityType.Total);
				return true;
			case ArticleCollisionBehavior.Slide:
			{
				Fixed src = Vector3F.Dot(context.model.GetVelocity(VelocityType.Movement), vector3F);
				Vector3F vector3F2 = -src * vector3F;
				context.model.AddVelocity(ref vector3F2, VelocityType.Movement);
				motionContext.travelDelta = (motionContext.maxTravelDist - motionContext.distanceTraveled) * context.model.totalVelocity.normalized;
				motionContext.completedMovement = (motionContext.distanceTraveled >= motionContext.maxTravelDist);
				return false;
			}
			case ArticleCollisionBehavior.Reflect:
			{
				Vector3F vector3F3 = Vector3F.Reflect(context.model.GetVelocity(VelocityType.Movement), vector3F);
				Vector3F vector3F4 = Vector3F.Project(vector3F3, vector3F);
				Vector3F a = vector3F3 - vector3F4;
				Vector3F a2 = vector3F4 * context.articleData.reflectNormalVelocityMultiplier;
				Vector3F b = a * context.articleData.reflectTangentVelocityMultiplier;
				if (a2.sqrMagnitude < context.articleData.reflectNormalVelocityMin * context.articleData.reflectNormalVelocityMin)
				{
					a2 = vector3F4.normalized * context.articleData.reflectNormalVelocityMin;
				}
				if (b.sqrMagnitude < context.articleData.reflectTangentVelocityMin * context.articleData.reflectTangentVelocityMin)
				{
					b = a.normalized * context.articleData.reflectTangentVelocityMin;
				}
				Vector3F newVelocity = a2 + b;
				bool switchFacing = newVelocity.x * context.model.GetVelocity(VelocityType.Movement).x < 0;
				context.model.SetVelocity(newVelocity, VelocityType.Movement);
				motionContext.initialVelocity = context.model.totalVelocity;
				motionContext.initialMovementVelocity = context.model.movementVelocity;
				motionContext.initialKnockbackVelocity = context.model.knockbackVelocity;
				motionContext.travelDelta = (motionContext.maxTravelDist - motionContext.distanceTraveled) * context.model.totalVelocity.normalized;
				context.model.position += motionContext.travelDelta;
				motionContext.distanceTraveled += motionContext.travelDelta.magnitude;
				motionContext.completedMovement = (motionContext.distanceTraveled >= motionContext.maxTravelDist);
				context.articleController.OnCollisionSpawn(context.articleData.reflectArticle, switchFacing, Vector3F.zero, context.articleController.Position, true);
				return false;
			}
			case ArticleCollisionBehavior.Spawn:
			{
				CollisionArticleData collisionArticleFor = context.articleData.GetCollisionArticleFor(context.articleController.Model.currentFrame + 1);
				if (collisionArticleFor != null && !context.articleController.model.didCollisionSpawn)
				{
					context.model.SetVelocity(Vector3F.zero, VelocityType.Movement);
					motionContext.initialVelocity = context.model.totalVelocity;
					motionContext.initialMovementVelocity = context.model.movementVelocity;
					motionContext.initialKnockbackVelocity = context.model.knockbackVelocity;
					motionContext.travelDelta = (motionContext.maxTravelDist - motionContext.distanceTraveled) * context.model.totalVelocity.normalized;
					motionContext.distanceTraveled += motionContext.travelDelta.magnitude;
					motionContext.completedMovement = (motionContext.distanceTraveled >= motionContext.maxTravelDist);
					context.articleController.model.didCollisionSpawn = true;
					context.articleController.OnCollisionSpawn(collisionArticleFor.spawnedArticleData, false, collisionArticleFor.offset, collisionData.point, false);
					return false;
				}
				context.model.ClearVelocity(true, true, true, VelocityType.Total);
				return true;
			}
			}
		}
		return true;
	}

	// Token: 0x040018CE RID: 6350
	private int groundLayer;

	// Token: 0x040018D0 RID: 6352
	private int platformLayer;
}
