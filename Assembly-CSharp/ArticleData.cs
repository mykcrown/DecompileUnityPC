using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200035B RID: 859
public class ArticleData : ScriptableObject, IPreloadedGameAsset
{
	// Token: 0x06001273 RID: 4723 RVA: 0x0006A280 File Offset: 0x00068680
	public static ArticleController CreateArticleController(DynamicObjectContainer container, ArticleType type, GameObject prefab, int poolSize = 4)
	{
		GameObject gameObject;
		if (type != ArticleType.Projectile)
		{
			if (type != ArticleType.Explosion)
			{
			}
			return container.InstantiateDynamicArticle<ArticleController>(prefab, out gameObject, poolSize);
		}
		return container.InstantiateDynamicArticle<ProjectileController>(prefab, out gameObject, poolSize);
	}

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x06001274 RID: 4724 RVA: 0x0006A2B4 File Offset: 0x000686B4
	public bool DoesCollide
	{
		get
		{
			return this.collideWithTerrain || this.collideWithPlatforms;
		}
	}

	// Token: 0x06001275 RID: 4725 RVA: 0x0006A2CC File Offset: 0x000686CC
	public void Rescale(Fixed rescale)
	{
		foreach (HurtBox hurtBox in this.hurtBoxes)
		{
			hurtBox.Rescale(rescale);
		}
		foreach (HitData hitData in this.hitData)
		{
			hitData.Rescale(rescale, false);
		}
		this.deathOffset *= (float)rescale;
		EnvironmentBounds environmentBounds = this.collisionBounds;
		environmentBounds.dimensions.x = environmentBounds.dimensions.x * rescale;
		EnvironmentBounds environmentBounds2 = this.collisionBounds;
		environmentBounds2.dimensions.y = environmentBounds2.dimensions.y * rescale;
		this.collisionBounds.centerOffset *= rescale;
		if (this.deathArticle != null)
		{
			this.deathArticle.Rescale(rescale);
		}
	}

	// Token: 0x1700034E RID: 846
	// (get) Token: 0x06001276 RID: 4726 RVA: 0x0006A3D8 File Offset: 0x000687D8
	private PreloadType PreloadType
	{
		get
		{
			ArticleType articleType = this.type;
			if (articleType == ArticleType.Projectile)
			{
				return PreloadType.PROJECTILE;
			}
			if (articleType != ArticleType.Explosion)
			{
				return PreloadType.EFFECT;
			}
			return PreloadType.ARTICLE;
		}
	}

	// Token: 0x06001277 RID: 4727 RVA: 0x0006A404 File Offset: 0x00068804
	public void RegisterPreload(PreloadContext context)
	{
		if (!context.AlreadyChecked(this))
		{
			PreloadType preloadType = this.PreloadType;
			if (this.teamParticle)
			{
				context.Register(new PreloadDef(this.bluePrefab, preloadType), this.preloadCount);
				context.Register(new PreloadDef(this.redPrefab, preloadType), this.preloadCount);
			}
			else
			{
				context.Register(new PreloadDef(this.prefab, preloadType), this.preloadCount);
			}
			context.Register(new PreloadDef(this.deathParticle, PreloadType.EFFECT), this.deathPreloadCount);
			foreach (CollisionArticleData collisionArticleData in this.collisionArticles)
			{
				if (collisionArticleData.spawnedArticleData != null)
				{
					collisionArticleData.spawnedArticleData.RegisterPreload(context);
				}
			}
			foreach (HitData hitData in this.hitData)
			{
				hitData.RegisterPreload(context);
			}
			if (this.reflectArticle != null)
			{
				this.reflectArticle.RegisterPreload(context);
			}
			if (this.deathArticle != null)
			{
				this.deathArticle.RegisterPreload(context);
			}
		}
	}

	// Token: 0x06001278 RID: 4728 RVA: 0x0006A53C File Offset: 0x0006893C
	public CollisionArticleData GetCollisionArticleFor(int frame)
	{
		foreach (CollisionArticleData collisionArticleData in this.collisionArticles)
		{
			if (frame >= collisionArticleData.startFrame && frame <= collisionArticleData.endFrame)
			{
				return collisionArticleData;
			}
		}
		return null;
	}

	// Token: 0x04000BC4 RID: 3012
	[FormerlySerializedAs("name")]
	public string articleName = "New Article";

	// Token: 0x04000BC5 RID: 3013
	public ArticleType type;

	// Token: 0x04000BC6 RID: 3014
	public GameObject prefab;

	// Token: 0x04000BC7 RID: 3015
	public bool teamParticle;

	// Token: 0x04000BC8 RID: 3016
	public GameObject bluePrefab;

	// Token: 0x04000BC9 RID: 3017
	public GameObject redPrefab;

	// Token: 0x04000BCA RID: 3018
	public int preloadCount = 4;

	// Token: 0x04000BCB RID: 3019
	[FormerlySerializedAs("hits")]
	public HitData[] hitData = new HitData[0];

	// Token: 0x04000BCC RID: 3020
	public ArticlePhysicsData physics = new ArticlePhysicsData();

	// Token: 0x04000BCD RID: 3021
	public List<HurtBox> hurtBoxes;

	// Token: 0x04000BCE RID: 3022
	public int lifetimeFrames = 120;

	// Token: 0x04000BCF RID: 3023
	public int stopEmitFrames = 100;

	// Token: 0x04000BD0 RID: 3024
	public bool chargesMeter;

	// Token: 0x04000BD1 RID: 3025
	public int loopFrames = 30;

	// Token: 0x04000BD2 RID: 3026
	public bool loops = true;

	// Token: 0x04000BD3 RID: 3027
	public bool prewarm;

	// Token: 0x04000BD4 RID: 3028
	public bool skipHitLag;

	// Token: 0x04000BD5 RID: 3029
	public bool collideWithTerrain;

	// Token: 0x04000BD6 RID: 3030
	public bool collideWithPlatforms;

	// Token: 0x04000BD7 RID: 3031
	public ArticleCollisionBehavior environmentCollisionBehavior = ArticleCollisionBehavior.Explode;

	// Token: 0x04000BD8 RID: 3032
	public bool collideWithHitBoxes = true;

	// Token: 0x04000BD9 RID: 3033
	public EnvironmentBounds collisionBounds;

	// Token: 0x04000BDA RID: 3034
	public ArticleData reflectArticle;

	// Token: 0x04000BDB RID: 3035
	public bool reflectArticleInheritFrameCount = true;

	// Token: 0x04000BDC RID: 3036
	public bool refreshLifespanOnReflect = true;

	// Token: 0x04000BDD RID: 3037
	public CollisionArticleData[] collisionArticles = new CollisionArticleData[0];

	// Token: 0x04000BDE RID: 3038
	public bool destroyOnImpact = true;

	// Token: 0x04000BDF RID: 3039
	public ArticleData deathArticle;

	// Token: 0x04000BE0 RID: 3040
	public GameObject deathParticle;

	// Token: 0x04000BE1 RID: 3041
	public int deathParticleDurationFrames = 120;

	// Token: 0x04000BE2 RID: 3042
	public int deathPreloadCount = 4;

	// Token: 0x04000BE3 RID: 3043
	public Vector3 deathOffset;

	// Token: 0x04000BE4 RID: 3044
	public bool collideWithSelf;

	// Token: 0x04000BE5 RID: 3045
	public bool rotateWithAngle = true;

	// Token: 0x04000BE6 RID: 3046
	public bool flipWithFacing = true;

	// Token: 0x04000BE7 RID: 3047
	public ArticleComponent[] components = new ArticleComponent[0];

	// Token: 0x04000BE8 RID: 3048
	public AudioData reflectSound;

	// Token: 0x04000BE9 RID: 3049
	public bool reflectPlatformUnderside;

	// Token: 0x04000BEA RID: 3050
	public Fixed reflectNormalVelocityMultiplier = 1;

	// Token: 0x04000BEB RID: 3051
	public Fixed reflectTangentVelocityMultiplier = 1;

	// Token: 0x04000BEC RID: 3052
	public Fixed reflectNormalVelocityMin = 0;

	// Token: 0x04000BED RID: 3053
	public Fixed reflectTangentVelocityMin = 0;

	// Token: 0x04000BEE RID: 3054
	public SoundEffect[] soundEffects = new SoundEffect[0];

	// Token: 0x04000BEF RID: 3055
	public MoveCameraShakeData[] cameraShakes = new MoveCameraShakeData[0];
}
