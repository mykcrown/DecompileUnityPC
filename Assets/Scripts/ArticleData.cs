// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ArticleData : ScriptableObject, IPreloadedGameAsset
{
	[FormerlySerializedAs("name")]
	public string articleName = "New Article";

	public ArticleType type;

	public GameObject prefab;

	public bool teamParticle;

	public GameObject bluePrefab;

	public GameObject redPrefab;

	public int preloadCount = 4;

	[FormerlySerializedAs("hits")]
	public HitData[] hitData = new HitData[0];

	public ArticlePhysicsData physics = new ArticlePhysicsData();

	public List<HurtBox> hurtBoxes;

	public int lifetimeFrames = 120;

	public int stopEmitFrames = 100;

	public bool chargesMeter;

	public int loopFrames = 30;

	public bool loops = true;

	public bool prewarm;

	public bool skipHitLag;

	public bool collideWithTerrain;

	public bool collideWithPlatforms;

	public ArticleCollisionBehavior environmentCollisionBehavior = ArticleCollisionBehavior.Explode;

	public bool collideWithHitBoxes = true;

	public EnvironmentBounds collisionBounds;

	public ArticleData reflectArticle;

	public bool reflectArticleInheritFrameCount = true;

	public bool refreshLifespanOnReflect = true;

	public CollisionArticleData[] collisionArticles = new CollisionArticleData[0];

	public bool destroyOnImpact = true;

	public ArticleData deathArticle;

	public GameObject deathParticle;

	public int deathParticleDurationFrames = 120;

	public int deathPreloadCount = 4;

	public Vector3 deathOffset;

	public bool collideWithSelf;

	public bool rotateWithAngle = true;

	public bool flipWithFacing = true;

	public ArticleComponent[] components = new ArticleComponent[0];

	public AudioData reflectSound;

	public bool reflectPlatformUnderside;

	public Fixed reflectNormalVelocityMultiplier = 1;

	public Fixed reflectTangentVelocityMultiplier = 1;

	public Fixed reflectNormalVelocityMin = 0;

	public Fixed reflectTangentVelocityMin = 0;

	public SoundEffect[] soundEffects = new SoundEffect[0];

	public MoveCameraShakeData[] cameraShakes = new MoveCameraShakeData[0];

	public bool DoesCollide
	{
		get
		{
			return this.collideWithTerrain || this.collideWithPlatforms;
		}
	}

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

	public void Rescale(Fixed rescale)
	{
		foreach (HurtBox current in this.hurtBoxes)
		{
			current.Rescale(rescale);
		}
		HitData[] array = this.hitData;
		for (int i = 0; i < array.Length; i++)
		{
			HitData hitData = array[i];
			hitData.Rescale(rescale, false);
		}
		this.deathOffset *= (float)rescale;
		EnvironmentBounds expr_8D_cp_0 = this.collisionBounds;
		expr_8D_cp_0.dimensions.x = expr_8D_cp_0.dimensions.x * rescale;
		EnvironmentBounds expr_A9_cp_0 = this.collisionBounds;
		expr_A9_cp_0.dimensions.y = expr_A9_cp_0.dimensions.y * rescale;
		this.collisionBounds.centerOffset *= rescale;
		if (this.deathArticle != null)
		{
			this.deathArticle.Rescale(rescale);
		}
	}

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
			CollisionArticleData[] array = this.collisionArticles;
			for (int i = 0; i < array.Length; i++)
			{
				CollisionArticleData collisionArticleData = array[i];
				if (collisionArticleData.spawnedArticleData != null)
				{
					collisionArticleData.spawnedArticleData.RegisterPreload(context);
				}
			}
			HitData[] array2 = this.hitData;
			for (int j = 0; j < array2.Length; j++)
			{
				HitData hitData = array2[j];
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

	public CollisionArticleData GetCollisionArticleFor(int frame)
	{
		CollisionArticleData[] array = this.collisionArticles;
		for (int i = 0; i < array.Length; i++)
		{
			CollisionArticleData collisionArticleData = array[i];
			if (frame >= collisionArticleData.startFrame && frame <= collisionArticleData.endFrame)
			{
				return collisionArticleData;
			}
		}
		return null;
	}
}
