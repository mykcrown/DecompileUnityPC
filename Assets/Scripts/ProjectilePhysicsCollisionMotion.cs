// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ProjectilePhysicsCollisionMotion : IPhysicsCollisionMotion
{
	private int groundLayer;

	private int platformLayer;

	private static ArticleController.ComponentExecution<IArticleCollisionController> __f__am_cache0;

	private static ArticleController.ComponentExecution<IArticleCollisionController> __f__am_cache1;

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	public int GroundMask
	{
		get;
		private set;
	}

	public int PlatformMask
	{
		get;
		private set;
	}

	public int GroundAndPlatformMask
	{
		get
		{
			return this.GroundMask | this.PlatformMask;
		}
	}

	public ProjectilePhysicsCollisionMotion()
	{
		this.groundLayer = LayerMask.NameToLayer(Layers.Ground);
		this.GroundMask = 1 << this.groundLayer;
		this.platformLayer = LayerMask.NameToLayer(Layers.Platform);
		this.PlatformMask = 1 << this.platformLayer;
	}

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
				if (ProjectilePhysicsCollisionMotion.__f__am_cache0 == null)
				{
					ProjectilePhysicsCollisionMotion.__f__am_cache0 = new ArticleController.ComponentExecution<IArticleCollisionController>(ProjectilePhysicsCollisionMotion._HandleMotion_m__0);
				}
				ArticleController.ComponentExecution<IArticleCollisionController> execute = ProjectilePhysicsCollisionMotion.__f__am_cache0;
				flag |= context.articleController.ExecuteArticleComponents<IArticleCollisionController>(execute);
			}
			else if (collisionData.terrainCollider.Layer == this.platformLayer)
			{
				if (ProjectilePhysicsCollisionMotion.__f__am_cache1 == null)
				{
					ProjectilePhysicsCollisionMotion.__f__am_cache1 = new ArticleController.ComponentExecution<IArticleCollisionController>(ProjectilePhysicsCollisionMotion._HandleMotion_m__1);
				}
				ArticleController.ComponentExecution<IArticleCollisionController> execute2 = ProjectilePhysicsCollisionMotion.__f__am_cache1;
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

	private static bool _HandleMotion_m__0(IArticleCollisionController hitController)
	{
		return hitController.OnHitTerrain();
	}

	private static bool _HandleMotion_m__1(IArticleCollisionController hitController)
	{
		return hitController.OnHitPlatform();
	}
}
