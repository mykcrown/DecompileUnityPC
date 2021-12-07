using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x02000356 RID: 854
public class ArticleController : HostedGameBehavior, ITickable, IExpirable, IResetable, IHitOwner, IArticleDelegate, IRollbackStateOwner, IFacing, IPhysicsStateOwner, IEffectOwner, IPhysicsColliderOwner
{
	// Token: 0x17000324 RID: 804
	// (get) Token: 0x060011FE RID: 4606 RVA: 0x00067A89 File Offset: 0x00065E89
	// (set) Token: 0x060011FF RID: 4607 RVA: 0x00067A91 File Offset: 0x00065E91
	[Inject]
	public ICombatCalculator combatCalculator { get; set; }

	// Token: 0x17000325 RID: 805
	// (get) Token: 0x06001200 RID: 4608 RVA: 0x00067A9A File Offset: 0x00065E9A
	// (set) Token: 0x06001201 RID: 4609 RVA: 0x00067AA2 File Offset: 0x00065EA2
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x06001202 RID: 4610 RVA: 0x00067AAB File Offset: 0x00065EAB
	// (set) Token: 0x06001203 RID: 4611 RVA: 0x00067AB3 File Offset: 0x00065EB3
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x06001204 RID: 4612 RVA: 0x00067ABC File Offset: 0x00065EBC
	// (set) Token: 0x06001205 RID: 4613 RVA: 0x00067AC4 File Offset: 0x00065EC4
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000328 RID: 808
	// (get) Token: 0x06001206 RID: 4614 RVA: 0x00067ACD File Offset: 0x00065ECD
	// (set) Token: 0x06001207 RID: 4615 RVA: 0x00067AD5 File Offset: 0x00065ED5
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000329 RID: 809
	// (get) Token: 0x06001208 RID: 4616 RVA: 0x00067ADE File Offset: 0x00065EDE
	// (set) Token: 0x06001209 RID: 4617 RVA: 0x00067AE6 File Offset: 0x00065EE6
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x1700032A RID: 810
	// (get) Token: 0x0600120A RID: 4618 RVA: 0x00067AEF File Offset: 0x00065EEF
	// (set) Token: 0x0600120B RID: 4619 RVA: 0x00067AF7 File Offset: 0x00065EF7
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x1700032B RID: 811
	// (get) Token: 0x0600120C RID: 4620 RVA: 0x00067B00 File Offset: 0x00065F00
	// (set) Token: 0x0600120D RID: 4621 RVA: 0x00067B08 File Offset: 0x00065F08
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x1700032C RID: 812
	// (get) Token: 0x0600120E RID: 4622 RVA: 0x00067B11 File Offset: 0x00065F11
	// (set) Token: 0x0600120F RID: 4623 RVA: 0x00067B19 File Offset: 0x00065F19
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x1700032D RID: 813
	// (get) Token: 0x06001210 RID: 4624 RVA: 0x00067B22 File Offset: 0x00065F22
	// (set) Token: 0x06001211 RID: 4625 RVA: 0x00067B2A File Offset: 0x00065F2A
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x06001212 RID: 4626 RVA: 0x00067B33 File Offset: 0x00065F33
	protected GameManager gameManager
	{
		get
		{
			return this.gameController.currentGame;
		}
	}

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x06001213 RID: 4627 RVA: 0x00067B40 File Offset: 0x00065F40
	protected GameLog log
	{
		get
		{
			return (!(this.gameManager == null)) ? this.gameManager.Log : null;
		}
	}

	// Token: 0x17000330 RID: 816
	// (get) Token: 0x06001214 RID: 4628 RVA: 0x00067B64 File Offset: 0x00065F64
	public Vector3F Velocity
	{
		get
		{
			return this.context.model.totalVelocity;
		}
	}

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x06001215 RID: 4629 RVA: 0x00067B76 File Offset: 0x00065F76
	// (set) Token: 0x06001216 RID: 4630 RVA: 0x00067B7E File Offset: 0x00065F7E
	public PhysicsSimulator Simulator { get; private set; }

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x06001217 RID: 4631 RVA: 0x00067B87 File Offset: 0x00065F87
	// (set) Token: 0x06001218 RID: 4632 RVA: 0x00067B8F File Offset: 0x00065F8F
	public IPhysicsCollider Collider { get; private set; }

	// Token: 0x17000333 RID: 819
	// (get) Token: 0x06001219 RID: 4633 RVA: 0x00067B98 File Offset: 0x00065F98
	public PlayerPhysicsController Physics
	{
		get
		{
			return null;
		}
	}

	// Token: 0x1700031F RID: 799
	// (get) Token: 0x0600121A RID: 4634 RVA: 0x00067B9B File Offset: 0x00065F9B
	// (set) Token: 0x0600121B RID: 4635 RVA: 0x00067BAD File Offset: 0x00065FAD
	PhysicsOverride IPhysicsStateOwner.PhysicsOverride
	{
		get
		{
			return this.model.physicsModel.physicsOverride;
		}
		set
		{
			this.model.physicsModel.physicsOverride = value;
		}
	}

	// Token: 0x17000320 RID: 800
	// (get) Token: 0x0600121C RID: 4636 RVA: 0x00067BC0 File Offset: 0x00065FC0
	MoveData IPhysicsStateOwner.CurrentMove
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000321 RID: 801
	// (get) Token: 0x0600121D RID: 4637 RVA: 0x00067BC3 File Offset: 0x00065FC3
	MoveController IHitOwner.ActiveMove
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000334 RID: 820
	// (get) Token: 0x0600121E RID: 4638 RVA: 0x00067BC6 File Offset: 0x00065FC6
	public IPlayerDelegate player
	{
		get
		{
			return this.gameManager.GetPlayerController(this.model.playerOwner);
		}
	}

	// Token: 0x17000322 RID: 802
	// (get) Token: 0x0600121F RID: 4639 RVA: 0x00067BDE File Offset: 0x00065FDE
	ArticleData IArticleDelegate.Data
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x06001220 RID: 4640 RVA: 0x00067BE6 File Offset: 0x00065FE6
	public ArticleModel Model
	{
		get
		{
			return this.model;
		}
	}

	// Token: 0x06001221 RID: 4641 RVA: 0x00067BF0 File Offset: 0x00065FF0
	public override void Reset()
	{
		base.Reset();
		this.model.Reset();
		if (this.articleComponents != null)
		{
			foreach (IArticleComponent articleComponent in this.articleComponents)
			{
				if (articleComponent is IResetable)
				{
					((IResetable)articleComponent).Reset();
				}
			}
		}
		for (int i = 0; i < this.model.activeSoundEffects.Count; i++)
		{
			this.model.activeSoundEffects[i].Stop();
		}
		this.model.activeSoundEffects.Clear();
	}

	// Token: 0x06001222 RID: 4642 RVA: 0x00067CC0 File Offset: 0x000660C0
	[PostConstruct]
	public void OnPostConstruct()
	{
		this.impactHandler = this.injector.GetInstance<ProjectilePhysicsImpactHandler>();
		this.collisionMotion = this.injector.GetInstance<ProjectilePhysicsCollisionMotion>();
		this.gameVFX = new GameVFX();
		this.injector.Inject(this.gameVFX);
		(this.gameVFX as GameVFX).Initialize(this.gameManager.DynamicObjects, null, this, this, this.config, this.gameManager.Audio, false, null);
		this.audioOwner.Init(base.gameObject, false);
	}

	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06001223 RID: 4643 RVA: 0x00067D4F File Offset: 0x0006614F
	public new Transform transform
	{
		get
		{
			return (!(this.hostObject == null)) ? this.hostObject.transform : base.transform;
		}
	}

	// Token: 0x06001224 RID: 4644 RVA: 0x00067D78 File Offset: 0x00066178
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ArticleModel>(ref this.model);
		this.context.model = this.model.physicsModel;
		foreach (IArticleComponent articleComponent in this.articleComponents)
		{
			if (articleComponent is IRollbackStateOwner)
			{
				((IRollbackStateOwner)articleComponent).LoadState(container);
			}
		}
		this.transform.position = (Vector3)this.model.physicsModel.position;
		if (this.data.rotateWithAngle)
		{
			this.SyncToRotation(this.model.rotationAngle);
		}
		else
		{
			this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
		this.updateFacing(this.model.currentFacing);
		return true;
	}

	// Token: 0x06001225 RID: 4645 RVA: 0x00067E80 File Offset: 0x00066280
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<ArticleModel>(this.model));
		foreach (IArticleComponent articleComponent in this.articleComponents)
		{
			if (articleComponent is IRollbackStateOwner)
			{
				((IRollbackStateOwner)articleComponent).ExportState(ref container);
			}
		}
		return true;
	}

	// Token: 0x06001226 RID: 4646 RVA: 0x00067F08 File Offset: 0x00066308
	public void Awake()
	{
		if (this.events != null)
		{
			this.events.Subscribe(typeof(ToggleDebugDrawChannelCommand), new Events.EventHandler(this.onToggleDebugChannel));
		}
	}

	// Token: 0x06001227 RID: 4647 RVA: 0x00067F36 File Offset: 0x00066336
	public virtual void SetEffect(Effect effect)
	{
		this.model.effect = effect;
	}

	// Token: 0x06001228 RID: 4648 RVA: 0x00067F44 File Offset: 0x00066344
	public virtual void Init(ArticleData data)
	{
		this.data = data;
		data.collisionBounds.CopyTo(this.model.physicsModel.bounds);
		this.initPhysics();
		this.transform.position = (Vector3)this.model.physicsModel.position;
		this.model.hits.Clear();
		for (int i = 0; i < data.hitData.Length; i++)
		{
			this.model.hits.Add(new Hit(data.hitData[i]));
		}
		this.model.disabledHits.Init(data.lifetimeFrames, this.model.hits, true);
		if (data.rotateWithAngle)
		{
			this.SyncToRotation(this.model.rotationAngle);
		}
		else
		{
			this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			this.updateFacing(this.Facing);
		}
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HitBoxes))
		{
			this.toggleHitBoxCapsules(true);
		}
		if (this.articleComponents == null)
		{
			this.articleComponents = new List<IArticleComponent>();
			for (int j = 0; j < data.components.Length; j++)
			{
				ArticleComponent original = data.components[j];
				ArticleComponent articleComponent = UnityEngine.Object.Instantiate<ArticleComponent>(original);
				this.injector.Inject(articleComponent);
				articleComponent.Init(this, this.gameManager);
				articleComponent.OnArticleInstantiate();
				this.articleComponents.Add(articleComponent);
			}
		}
		else
		{
			for (int k = 0; k < this.articleComponents.Count; k++)
			{
				this.articleComponents[k].OnArticleInstantiate();
			}
		}
		if (this.model.chargeData == null && this.config != null)
		{
			this.model.chargeData = this.config.chargeConfig;
		}
		if (this.model.chargeData != null)
		{
			Fixed scaledValue = this.model.chargeData.GetScaledValue(this.model.chargeData.maxChargeProjectileScale, this.model.chargeFraction);
			this.transform.localScale = this.transform.localScale * (float)scaledValue;
		}
		if (this.model.effect != null)
		{
			ParticleData particleData = new ParticleData();
			particleData.prewarm = data.prewarm;
			if (this.model.chargeData != null)
			{
				particleData.lifetimeScale = (float)this.model.chargeData.GetScaledValue(this.model.chargeData.maxChargeProjectileLifetimeMultiplier, this.model.chargeFraction);
			}
			this.model.effect.Init(-1, particleData, 1f);
		}
		else
		{
			Debug.LogWarning("Unable to find effect on this ArticleController.  Effect may not have been set. " + base.name);
		}
		this.gameManager.Hits.Register(this);
		int mask = PhysicsSimulator.GroundMask;
		if (data.collideWithPlatforms)
		{
			mask = PhysicsSimulator.GroundAndPlatformMask;
		}
		if (this.gameManager.PhysicsWorld.CollidersContainEnvironmentBounds(data.collisionBounds, this.model.physicsModel.center, mask))
		{
			this.extract();
		}
		foreach (MoveCameraShakeData moveCameraShakeData in data.cameraShakes)
		{
			this.gameManager.Camera.ShakeCamera(new CameraShakeRequest(moveCameraShakeData.shake));
		}
	}

	// Token: 0x06001229 RID: 4649 RVA: 0x000682E4 File Offset: 0x000666E4
	private void initPhysics()
	{
		if (this.gameManager != null)
		{
			this.Simulator = this.gameManager.Physics;
			this.Collider = new PhysicsCollider(new EdgeData(this.model.physicsModel.center, true, EdgeData.CacheFlag.NoSurface, new Vector2F[]
			{
				this.model.physicsModel.bounds.up,
				this.model.physicsModel.bounds.right,
				this.model.physicsModel.bounds.down,
				this.model.physicsModel.bounds.left
			}), LayerMask.NameToLayer("Player"));
			this.context = this.createContext(this.model.physicsModel, true);
		}
	}

	// Token: 0x0600122A RID: 4650 RVA: 0x000683FC File Offset: 0x000667FC
	private PhysicsContext createContext(PhysicsModel state, bool hasLandCallback)
	{
		PhysicsContext physicsContext = new PhysicsContext();
		physicsContext.impactHandler = this.impactHandler;
		physicsContext.collisionMotion = this.collisionMotion;
		physicsContext.physicsState = this;
		physicsContext.world = this.gameManager.PhysicsWorld;
		physicsContext.calculateMaxVerticalSpeedCallback = new CalculateMaxSpeedCallback(this.calculateMaxSpeed);
		physicsContext.calculateMaxHorizontalSpeedCallback = new CalculateMaxSpeedCallback(this.calculateMaxSpeed);
		physicsContext.colliderOwner = this;
		physicsContext.articleData = this.data;
		physicsContext.articleController = this;
		state.physicsOverride = new PhysicsOverride();
		physicsContext.knockbackConfig = this.config.knockbackConfig;
		physicsContext.lagConfig = this.config.lagConfig;
		physicsContext.model = state;
		return physicsContext;
	}

	// Token: 0x0600122B RID: 4651 RVA: 0x000684B4 File Offset: 0x000668B4
	private void extract()
	{
		Vector3F v;
		if (this.context.model.totalVelocity != Vector3F.zero)
		{
			v = -this.context.model.totalVelocity;
			v.Normalize();
		}
		else
		{
			v = ((this.Facing != HorizontalDirection.Left) ? Vector3F.left : Vector3F.right);
		}
		CollisionData item = PhysicsCollisionCalculator.CalculateExtractionWithVelocity(this.context, v, this.context.world.GetRelevantColliders(), PhysicsSimulator.GroundAndPlatformMask);
		if (item.collisionType != CollisionType.None)
		{
			this.context.model.position += item.deltaToCollision;
			List<CollisionData> sharedCollisions = new List<CollisionData>
			{
				item
			};
			PhysicsMotionContext motionContext = this.context.motionContext;
			motionContext.initialVelocity = this.context.model.totalVelocity;
			motionContext.initialMovementVelocity = this.context.model.movementVelocity;
			motionContext.initialKnockbackVelocity = this.context.model.knockbackVelocity;
			motionContext.travelDelta = motionContext.initialVelocity * WTime.fixedDeltaTime;
			motionContext.maxTravelDist = motionContext.travelDelta.magnitude;
			motionContext.distanceTraveled = 0;
			this.context.collisionMotion.HandleMotion(this.context, sharedCollisions);
		}
	}

	// Token: 0x0600122C RID: 4652 RVA: 0x0006862C File Offset: 0x00066A2C
	public void OnReflectedArticle(Vector2F collisionPosition, Vector2F reflectedNormal)
	{
		CollisionData item = default(CollisionData);
		List<CollisionData> list = new List<CollisionData>();
		item.normal = reflectedNormal;
		this.context.model.position = collisionPosition;
		if (this.data.refreshLifespanOnReflect)
		{
			this.model.currentFrame = 0;
		}
		list.Add(item);
		PhysicsMotionContext motionContext = this.context.motionContext;
		motionContext.initialVelocity = this.context.model.totalVelocity;
		motionContext.initialMovementVelocity = this.context.model.movementVelocity;
		motionContext.initialKnockbackVelocity = this.context.model.knockbackVelocity;
		motionContext.travelDelta = motionContext.initialVelocity * WTime.fixedDeltaTime;
		motionContext.maxTravelDist = motionContext.travelDelta.magnitude;
		motionContext.distanceTraveled = 0;
		ArticleCollisionBehavior environmentCollisionBehavior = this.context.articleData.environmentCollisionBehavior;
		this.context.articleData.environmentCollisionBehavior = ArticleCollisionBehavior.Reflect;
		this.context.collisionMotion.HandleMotion(this.context, list);
		this.context.articleData.environmentCollisionBehavior = environmentCollisionBehavior;
	}

	// Token: 0x0600122D RID: 4653 RVA: 0x00068754 File Offset: 0x00066B54
	public void OnDamageTaken(Fixed damage, ImpactType impactType)
	{
	}

	// Token: 0x0600122E RID: 4654 RVA: 0x00068756 File Offset: 0x00066B56
	public void BeginHitLag(int hitLagFrames, IHitOwner owner, HitData hitData)
	{
		this.beginHitLag(hitLagFrames);
	}

	// Token: 0x0600122F RID: 4655 RVA: 0x0006875F File Offset: 0x00066B5F
	protected void beginHitLag(int hitLagFrames)
	{
		if (this.data.skipHitLag)
		{
			this.model.hitLagFrames = 1;
		}
		else
		{
			this.model.hitLagFrames = hitLagFrames;
		}
	}

	// Token: 0x06001230 RID: 4656 RVA: 0x0006878E File Offset: 0x00066B8E
	private Fixed calculateMaxSpeed()
	{
		return 0;
	}

	// Token: 0x06001231 RID: 4657 RVA: 0x00068798 File Offset: 0x00066B98
	private void onToggleDebugChannel(GameEvent message)
	{
		ToggleDebugDrawChannelCommand toggleDebugDrawChannelCommand = message as ToggleDebugDrawChannelCommand;
		if (toggleDebugDrawChannelCommand.channel == DebugDrawChannel.HitBoxes)
		{
			this.toggleHitBoxCapsules(toggleDebugDrawChannelCommand.enabled);
		}
	}

	// Token: 0x06001232 RID: 4658 RVA: 0x000687C4 File Offset: 0x00066BC4
	private void toggleHitBoxCapsules(bool enabled)
	{
		if (this.model.isExpired)
		{
			return;
		}
		if (enabled)
		{
			foreach (Hit hit in this.model.hits)
			{
				foreach (HitBoxState hitBoxState in hit.hitBoxes)
				{
					CapsuleShape capsule = CapsulePool.Instance.GetCapsule(this.player.Transform);
					capsule.Load(hitBoxState.position, hitBoxState.lastPosition, (Fixed)((double)hitBoxState.data.radius), hit.data.DebugDrawColor, hitBoxState.IsCircle);
					this.model.hitBoxCapsules[hitBoxState] = capsule;
				}
			}
		}
		else
		{
			foreach (CapsuleShape capsuleShape in this.model.hitBoxCapsules.Values)
			{
				capsuleShape.Clear();
			}
			this.model.hitBoxCapsules.Clear();
		}
	}

	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06001233 RID: 4659 RVA: 0x00068944 File Offset: 0x00066D44
	public virtual List<Hit> Hits
	{
		get
		{
			return this.model.hits;
		}
	}

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x06001234 RID: 4660 RVA: 0x00068951 File Offset: 0x00066D51
	public List<HurtBoxState> HurtBoxes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000339 RID: 825
	// (get) Token: 0x06001235 RID: 4661 RVA: 0x00068954 File Offset: 0x00066D54
	public List<HitBoxState> ShieldBoxes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000323 RID: 803
	// (get) Token: 0x06001236 RID: 4662 RVA: 0x00068957 File Offset: 0x00066D57
	// (set) Token: 0x06001237 RID: 4663 RVA: 0x00068964 File Offset: 0x00066D64
	int IHitOwner.HitOwnerID
	{
		get
		{
			return this.model.hitOwnerID;
		}
		set
		{
			this.model.hitOwnerID = value;
		}
	}

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x06001238 RID: 4664 RVA: 0x00068972 File Offset: 0x00066D72
	public bool IsInvincible
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700033B RID: 827
	// (get) Token: 0x06001239 RID: 4665 RVA: 0x00068975 File Offset: 0x00066D75
	public bool IsAllyAssist
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700033C RID: 828
	// (get) Token: 0x0600123A RID: 4666 RVA: 0x00068978 File Offset: 0x00066D78
	public bool AssistAbsorbsHits
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600123B RID: 4667 RVA: 0x0006897C File Offset: 0x00066D7C
	public bool AllowClanking(HitData hitData, IHitOwner other)
	{
		HitType hitType = hitData.hitType;
		return (hitType != HitType.Hit && hitType != HitType.Projectile) || this.data.collideWithHitBoxes;
	}

	// Token: 0x0600123C RID: 4668 RVA: 0x000689B0 File Offset: 0x00066DB0
	bool IHitOwner.ForceCollisionChecks(CollisionCheckType type, HitData hitData)
	{
		if (type != CollisionCheckType.HitBox)
		{
			return false;
		}
		foreach (IArticleComponent articleComponent in this.articleComponents)
		{
			ArticleComponent articleComponent2 = (ArticleComponent)articleComponent;
			if (articleComponent2 is IArticleCollisionController)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600123D RID: 4669 RVA: 0x00068A30 File Offset: 0x00066E30
	public bool IsImmune(HitData hitData, IHitOwner enemy)
	{
		return hitData.hitType == HitType.Grab || hitData.hitType == HitType.BlockableGrab;
	}

	// Token: 0x0600123E RID: 4670 RVA: 0x00068A4A File Offset: 0x00066E4A
	public bool CanReflect(HitData hitData)
	{
		return false;
	}

	// Token: 0x0600123F RID: 4671 RVA: 0x00068A4D File Offset: 0x00066E4D
	public bool ShouldReflect(IHitOwner other, ref Vector3F collisionPoint, CollisionCheckType type, Hit myHit = null)
	{
		return false;
	}

	// Token: 0x1700033D RID: 829
	// (get) Token: 0x06001240 RID: 4672 RVA: 0x00068A50 File Offset: 0x00066E50
	public bool IsProjectile
	{
		get
		{
			return this.data.type == ArticleType.Projectile;
		}
	}

	// Token: 0x1700033E RID: 830
	// (get) Token: 0x06001241 RID: 4673 RVA: 0x00068A60 File Offset: 0x00066E60
	public HitOwnerType Type
	{
		get
		{
			return HitOwnerType.Article;
		}
	}

	// Token: 0x1700033F RID: 831
	// (get) Token: 0x06001242 RID: 4674 RVA: 0x00068A63 File Offset: 0x00066E63
	public TeamNum Team
	{
		get
		{
			return this.model.team;
		}
	}

	// Token: 0x17000340 RID: 832
	// (get) Token: 0x06001243 RID: 4675 RVA: 0x00068A70 File Offset: 0x00066E70
	public PlayerNum PlayerNum
	{
		get
		{
			return this.model.playerOwner;
		}
	}

	// Token: 0x06001244 RID: 4676 RVA: 0x00068A80 File Offset: 0x00066E80
	public bool IsHitActive(Hit hit, IHitOwner other, bool excludePhantomHitbox)
	{
		if (other != null)
		{
			if (other.Equals(this))
			{
				return false;
			}
			if (other.PlayerNum == this.model.playerOwner)
			{
				for (int i = 0; i < this.articleComponents.Count; i++)
				{
					IArticleComponent articleComponent = this.articleComponents[i];
					if (articleComponent is IArticleCanHitOwnerController)
					{
						return ((IArticleCanHitOwnerController)articleComponent).CanHitOwner();
					}
				}
				if (hit.data.hitType != HitType.SelfHit && !this.data.collideWithSelf)
				{
					if (!other.IsProjectile)
					{
						return false;
					}
					if (this.model.destroyOnHitLag)
					{
						return false;
					}
				}
				else if (hit.data.hitType == HitType.SelfHit)
				{
					return false;
				}
			}
			if (!this.gameManager.BattleSettings.isTeamAttack && other.PlayerNum != this.model.playerOwner && other.Team == this.Team)
			{
				for (int j = 0; j < this.articleComponents.Count; j++)
				{
					IArticleComponent articleComponent2 = this.articleComponents[j];
					if (articleComponent2 is IArticleCanHitAlliesController)
					{
						return ((IArticleCanHitAlliesController)articleComponent2).CanHitAllies();
					}
				}
				return false;
			}
		}
		for (int k = 0; k < this.articleComponents.Count; k++)
		{
			IArticleComponent articleComponent3 = this.articleComponents[k];
			if (articleComponent3 is IArticleCanHitEnemiesController && !((IArticleCanHitEnemiesController)articleComponent3).CanHitEnemies())
			{
				return false;
			}
		}
		if (!this.model.IsActiveFor(other, this.model.currentFrame))
		{
			return false;
		}
		Fixed scaledValue = this.model.chargeData.GetScaledValue(this.model.chargeData.maxChargeProjectileLifetimeMultiplier, this.model.chargeFraction);
		if (this.data.loops)
		{
			return this.model.currentFrame % (this.data.loopFrames * scaledValue) >= hit.data.startFrame * scaledValue && this.model.currentFrame % (this.data.loopFrames * scaledValue) <= hit.data.endFrame * scaledValue;
		}
		return this.model.currentFrame >= hit.data.startFrame * scaledValue && this.model.currentFrame <= hit.data.endFrame * scaledValue;
	}

	// Token: 0x17000341 RID: 833
	// (get) Token: 0x06001245 RID: 4677 RVA: 0x00068D3D File Offset: 0x0006713D
	public Vector3F Position
	{
		get
		{
			return this.model.physicsModel.position;
		}
	}

	// Token: 0x17000342 RID: 834
	// (get) Token: 0x06001246 RID: 4678 RVA: 0x00068D4F File Offset: 0x0006714F
	public Vector3F Center
	{
		get
		{
			return this.model.physicsModel.center;
		}
	}

	// Token: 0x06001247 RID: 4679 RVA: 0x00068D61 File Offset: 0x00067161
	private void updateFacing(HorizontalDirection facing)
	{
		this.model.currentFacing = facing;
		this.updateRotation();
	}

	// Token: 0x06001248 RID: 4680 RVA: 0x00068D78 File Offset: 0x00067178
	private void updateRotation()
	{
		Vector3 euler;
		if (this.data.rotateWithAngle)
		{
			euler = this.getSyncToRotationAngle();
		}
		else
		{
			euler = default(Vector3);
			if (this.model.currentFacing == HorizontalDirection.Left)
			{
				euler.y += 180f;
			}
		}
		this.transform.rotation = Quaternion.Euler(euler);
	}

	// Token: 0x06001249 RID: 4681 RVA: 0x00068DDE File Offset: 0x000671DE
	private Vector3 getSyncToRotationAngle()
	{
		return new Vector3(0f, 0f, (float)this.model.rotationAngle);
	}

	// Token: 0x0600124A RID: 4682 RVA: 0x00068E00 File Offset: 0x00067200
	public void SyncToRotation(Fixed angle)
	{
		this.model.rotationAngle = angle;
		this.updateRotation();
		Fixed x = this.data.collisionBounds.centerOffset.x;
		Fixed y = this.data.collisionBounds.centerOffset.y;
		Fixed one = FixedMath.Atan2(y, x);
		Fixed one2 = FixedMath.Sqrt(y * y + x * x);
		Fixed other = FixedMath.Sin(one + angle * FixedMath.Deg2Rad);
		Fixed other2 = FixedMath.Cos(one + angle * FixedMath.Deg2Rad);
		this.context.model.bounds.centerOffset.y = one2 * other;
		this.context.model.bounds.centerOffset.x = one2 * other2;
		float num = 1f;
		if (this.Facing == HorizontalDirection.Left)
		{
			num = -1f;
		}
		Vector3 localScale = this.transform.localScale;
		localScale.y = Math.Abs(localScale.y) * num;
		this.transform.localScale = localScale;
	}

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x0600124B RID: 4683 RVA: 0x00068F2A File Offset: 0x0006732A
	// (set) Token: 0x0600124C RID: 4684 RVA: 0x00068F37 File Offset: 0x00067337
	public virtual HorizontalDirection Facing
	{
		get
		{
			return this.model.currentFacing;
		}
		set
		{
			if (value != this.model.currentFacing)
			{
				this.updateFacing(value);
			}
		}
	}

	// Token: 0x17000344 RID: 836
	// (get) Token: 0x0600124D RID: 4685 RVA: 0x00068F51 File Offset: 0x00067351
	public virtual HorizontalDirection OppositeFacing
	{
		get
		{
			return (this.model.currentFacing != HorizontalDirection.Left) ? HorizontalDirection.Left : HorizontalDirection.Right;
		}
	}

	// Token: 0x17000345 RID: 837
	// (get) Token: 0x0600124E RID: 4686 RVA: 0x00068F6C File Offset: 0x0006736C
	public Fixed DamageMultiplier
	{
		get
		{
			Fixed @fixed = 1;
			if (this.model.chargeData != null)
			{
				@fixed *= this.model.chargeData.GetScaledValue(this.model.chargeData.maxChargeDamageMultiplier, this.model.chargeFraction);
			}
			return @fixed;
		}
	}

	// Token: 0x17000346 RID: 838
	// (get) Token: 0x0600124F RID: 4687 RVA: 0x00068FC3 File Offset: 0x000673C3
	public Fixed StaleDamageMultiplier
	{
		get
		{
			return this.model.staleDamageMultiplier;
		}
	}

	// Token: 0x17000347 RID: 839
	// (get) Token: 0x06001250 RID: 4688 RVA: 0x00068FD0 File Offset: 0x000673D0
	public Fixed HitLagMultiplier
	{
		get
		{
			Fixed @fixed = 1;
			if (this.model.chargeData != null)
			{
				@fixed *= this.model.chargeData.GetScaledValue(this.model.chargeData.maxChargeProjectileHitLagMultiplier, this.model.chargeFraction);
			}
			return @fixed;
		}
	}

	// Token: 0x17000348 RID: 840
	// (get) Token: 0x06001251 RID: 4689 RVA: 0x00069027 File Offset: 0x00067427
	public bool IsExpired
	{
		get
		{
			return this.model.isExpired;
		}
	}

	// Token: 0x06001252 RID: 4690 RVA: 0x00069034 File Offset: 0x00067434
	public HorizontalDirection CalculateVictimFacing(bool hitWasReversed)
	{
		return (!hitWasReversed) ? InputUtils.GetOppositeDirection(this.Facing) : this.Facing;
	}

	// Token: 0x06001253 RID: 4691 RVA: 0x00069052 File Offset: 0x00067452
	public virtual bool ShouldCancelClankedMove(Hit myHit, Hit otherHit, IHitOwner other)
	{
		return false;
	}

	// Token: 0x06001254 RID: 4692 RVA: 0x00069058 File Offset: 0x00067458
	public virtual bool OnHitSuccess(Hit hit, IHitOwner other, ImpactType impactType, ref Vector3F hitPosition, ref Vector3F hitVelocity, HitContext hitContext)
	{
		ArticleController.ComponentExecution<IArticleCollisionController> execute = (IArticleCollisionController hitController) => hitController.OnHitSuccess(hit, other, impactType);
		bool flag = this.ExecuteArticleComponents<IArticleCollisionController>(execute);
		if (!flag)
		{
			HitData hitData = hit.data;
			this.model.disabledHits.Disable(hitData, other, this.model.currentFrame);
			this.gameVFX.CreateHitEffect(this, hitData, hitPosition, -1, impactType, other);
			if (!other.IsInvincible && this.player != null)
			{
				this.player.OnDamageDealt(hitData.damage * this.DamageMultiplier, impactType, this.data.chargesMeter);
				this.player.StaleMove(this.model.moveLabel, this.model.moveName, this.model.moveUID);
				other.OnDamageTaken(hitData.damage * this.DamageMultiplier, impactType);
			}
			ParticleData lethalHit = this.config.defaultCharacterEffects.lethalHit;
			if ((this.config.defaultCharacterEffects.lethalHitSound.sound != null || (lethalHit != null && lethalHit.prefab != null)) && this.combatCalculator.IsLethalHit(hitData, this, other, hitPosition, 1))
			{
				if (this.config.defaultCharacterEffects.lethalHitSound.sound != null)
				{
					this.gameManager.Audio.PlayGameSound(new AudioRequest(this.config.defaultCharacterEffects.lethalHitSound, (Vector3)hitPosition, null));
				}
				if (lethalHit != null && lethalHit.prefab != null)
				{
					this.gameVFX.PlayParticle(lethalHit, false, TeamNum.None);
				}
			}
		}
		return flag;
	}

	// Token: 0x06001255 RID: 4693 RVA: 0x00069270 File Offset: 0x00067670
	bool IHitOwner.HandleComponentHitInteraction(Hit otherHit, IHitOwner other, CollisionCheckType checkType, HitContext hitContext)
	{
		return this.ExecuteArticleComponents<IArticleCollisionController>((IArticleCollisionController component) => component.HandleHitBoxCollision(otherHit, other, this, checkType, hitContext));
	}

	// Token: 0x06001256 RID: 4694 RVA: 0x000692B9 File Offset: 0x000676B9
	public virtual void ReceiveHit(HitData hitData, IHitOwner other, ImpactType impactType, HitContext hitContext)
	{
	}

	// Token: 0x06001257 RID: 4695 RVA: 0x000692BB File Offset: 0x000676BB
	public virtual void OnHitBoxCollision(Hit myHit, IHitOwner other, Hit otherHit, ref Vector3F hitPosition, bool cancelMine, bool makeClank)
	{
	}

	// Token: 0x06001258 RID: 4696 RVA: 0x000692C0 File Offset: 0x000676C0
	public virtual void TickFrame()
	{
		if (this.model.isExpired)
		{
			return;
		}
		if (this.model.hitLagFrames != 0)
		{
			this.model.hitLagFrames--;
			if (this.model.hitLagFrames == 0)
			{
				this.Explode(this.model.destroyOnHitLag);
				if (this.model.isExpired)
				{
					return;
				}
			}
		}
		if (this.data.stopEmitFrames > 0 && this.model.currentFrame == this.data.stopEmitFrames && this.model.effect != null)
		{
			this.model.effect.StopEmissions();
		}
		int num = this.data.lifetimeFrames;
		if (this.model.chargeData != null)
		{
			Fixed scaledValue = this.model.chargeData.GetScaledValue(this.model.chargeData.maxChargeProjectileLifetimeMultiplier, this.model.chargeFraction);
			num = (int)(num * scaledValue);
		}
		if (this.model.currentFrame == num)
		{
			this.Explode(true);
			return;
		}
		foreach (SoundEffect soundEffect in this.data.soundEffects)
		{
			if (this.model.currentFrame == soundEffect.frame && soundEffect.sounds.Length > 0)
			{
				this.model.activeSoundEffects.Add(new AudioHandle(this.gameManager.Audio, soundEffect, this.audioOwner));
			}
		}
		this.model.currentFrame++;
		this.ExecuteArticleComponents<IArticleMovementController>(delegate(IArticleMovementController movementController)
		{
			Vector3F zero = Vector3F.zero;
			bool result = movementController.TickMovement(ref zero, ref this.context.model.position);
			this.context.model.SetVelocity(zero, VelocityType.Movement);
			return result;
		});
		Vector3F totalVelocity = this.context.model.totalVelocity;
		if (this.data.rotateWithAngle && totalVelocity != Vector3F.zero)
		{
			this.SyncToRotation(MathUtil.VectorToAngle(ref totalVelocity));
		}
		this.UpdateHitBoxes();
		if (this.model.isExpired)
		{
			throw new Exception("ExpiredArticleAdded");
		}
		this.gameManager.Hits.QueueCollisionCheck(this);
	}

	// Token: 0x06001259 RID: 4697 RVA: 0x00069500 File Offset: 0x00067900
	public virtual void Explode(bool killMe = true)
	{
		bool canExplode = true;
		ArticleController.ComponentExecution<IArticleExplosionController> execute = delegate(IArticleExplosionController explosionController)
		{
			canExplode = explosionController.CanExplode();
			return canExplode;
		};
		this.ExecuteArticleComponents<IArticleExplosionController>(execute);
		if (!canExplode)
		{
			return;
		}
		Vector3F b;
		if (this.data.rotateWithAngle)
		{
			b = MathUtil.RotateVector((Vector3F)this.data.deathOffset, this.model.rotationAngle);
		}
		else
		{
			b = (Vector3F)this.data.deathOffset;
			if (this.model.currentFacing == HorizontalDirection.Left)
			{
				b.x *= -1;
			}
		}
		if (this.data.deathArticle != null)
		{
			ArticleController articleController = ArticleData.CreateArticleController(this.gameManager.DynamicObjects, this.data.deathArticle.type, this.data.deathArticle.prefab, 4);
			articleController.model.physicsModel.Reset();
			articleController.model.physicsModel.position = this.Position + b;
			articleController.model.playerOwner = this.model.playerOwner;
			articleController.model.team = this.model.team;
			articleController.model.moveLabel = this.model.moveLabel;
			articleController.model.moveName = this.model.moveName;
			articleController.model.moveUID = this.model.moveUID;
			articleController.model.staleDamageMultiplier = this.model.staleDamageMultiplier;
			articleController.Init(UnityEngine.Object.Instantiate<ArticleData>(this.data.deathArticle));
			articleController.Facing = this.Facing;
		}
		if (this.data.deathParticle != null)
		{
			Effect effect = this.gameManager.DynamicObjects.InstantiateDynamicObject<Effect>(this.data.deathParticle, 4, true);
			if (effect != null)
			{
				Vector3F v = this.Position + b;
				effect.transform.position = (Vector3)v;
				if (!this.data.rotateWithAngle)
				{
					Vector3 localScale = effect.transform.localScale;
					int num = (this.model.currentFacing != HorizontalDirection.Left) ? 1 : -1;
					localScale.x = Math.Abs(localScale.x) * (float)num;
					effect.transform.localScale = localScale;
				}
				effect.Init(this.data.deathParticleDurationFrames, null, 1f);
			}
		}
		if (killMe)
		{
			this.OnKilled();
			if (this.model.effect != null)
			{
				this.model.effect.EnterSoftKill();
				this.model.effect = null;
			}
			else
			{
				this.hostObject.DestroySafe();
			}
		}
	}

	// Token: 0x0600125A RID: 4698 RVA: 0x000697E8 File Offset: 0x00067BE8
	protected virtual void OnKilled()
	{
		foreach (CapsuleShape capsuleShape in this.model.hitBoxCapsules.Values)
		{
			capsuleShape.Clear();
		}
		this.model.hitBoxCapsules.Clear();
		this.model.isExpired = true;
		this.gameManager.Hits.Unregister(this);
	}

	// Token: 0x0600125B RID: 4699 RVA: 0x0006987C File Offset: 0x00067C7C
	private void OnDisable()
	{
		this.OnDestroy();
	}

	// Token: 0x0600125C RID: 4700 RVA: 0x00069884 File Offset: 0x00067C84
	public void OnDestroy()
	{
		if (this.events != null)
		{
			this.events.Unsubscribe(typeof(ToggleDebugDrawChannelCommand), new Events.EventHandler(this.onToggleDebugChannel));
		}
	}

	// Token: 0x0600125D RID: 4701 RVA: 0x000698B4 File Offset: 0x00067CB4
	public void UpdateHitBoxes()
	{
		for (int i = 0; i < this.model.hits.Count; i++)
		{
			Hit hit = this.model.hits[i];
			if (this.IsHitActive(hit, null, false))
			{
				for (int j = 0; j < hit.hitBoxes.Count; j++)
				{
					HitBoxState hitBoxState = hit.hitBoxes[j];
					hitBoxState.lastPosition = hitBoxState.position;
					Fixed @fixed = this.model.rotationAngle;
					if (this.model.currentFacing == HorizontalDirection.Left)
					{
						@fixed = 180 - @fixed;
					}
					Vector3F vector3F = Vector3F.one;
					if (this.model.chargeData != null)
					{
						vector3F *= this.model.chargeData.GetScaledValue(this.model.chargeData.maxChargeProjectileScale, this.model.chargeFraction);
					}
					hitBoxState.position = hitBoxState.CalculatePosition(this.model.physicsModel.position, @fixed, this.model.currentFacing, vector3F);
					if (hitBoxState.lastPosition.sqrMagnitude == 0)
					{
						hitBoxState.lastPosition = hitBoxState.position;
					}
					if (this.model.hitBoxCapsules.ContainsKey(hitBoxState))
					{
						CapsuleShape capsuleShape = this.model.hitBoxCapsules[hitBoxState];
						capsuleShape.Visible = true;
						capsuleShape.SetPositions(hitBoxState.position, hitBoxState.lastPosition, hitBoxState.IsCircle);
						capsuleShape.Radius = hitBoxState.data.radius * (float)vector3F.x;
					}
				}
			}
			else
			{
				foreach (HitBoxState key in hit.hitBoxes)
				{
					if (this.model.hitBoxCapsules.ContainsKey(key))
					{
						CapsuleShape capsuleShape2 = this.model.hitBoxCapsules[key];
						capsuleShape2.Visible = false;
					}
				}
			}
		}
	}

	// Token: 0x0600125E RID: 4702 RVA: 0x00069AE8 File Offset: 0x00067EE8
	public void OnDrawGizmos()
	{
		if (this.data == null || this.model == null)
		{
			return;
		}
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Bounds))
		{
			PhysicsModel physicsModel = this.model.physicsModel;
			float d = 1f;
			Vector3 b = (this.gameManager.Camera.current.transform.position - (Vector3)physicsModel.center).normalized * d;
			b = (this.gameManager.Camera.current.transform.position - (Vector3)physicsModel.center).normalized * d;
			GizmoUtil.GizmosDrawQuadrilateral((Vector3)physicsModel.bounds.up + (Vector3)physicsModel.center + b, (Vector3)physicsModel.bounds.right + (Vector3)physicsModel.center + b, (Vector3)physicsModel.bounds.down + (Vector3)physicsModel.center + b, (Vector3)physicsModel.bounds.left + (Vector3)physicsModel.center + b, Color.red);
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere((Vector3)physicsModel.position, 0.1f);
		}
	}

	// Token: 0x0600125F RID: 4703 RVA: 0x00069C68 File Offset: 0x00068068
	public bool ExecuteArticleComponents<T>(ArticleController.ComponentExecution<T> execute)
	{
		for (int i = 0; i < this.articleComponents.Count; i++)
		{
			IArticleComponent articleComponent = this.articleComponents[i];
			if (articleComponent is T)
			{
				bool flag = execute((T)((object)articleComponent));
				if (flag)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001260 RID: 4704 RVA: 0x00069CC0 File Offset: 0x000680C0
	public T GetArticleComponent<T>() where T : class, IArticleComponent
	{
		foreach (IArticleComponent articleComponent in this.articleComponents)
		{
			if (articleComponent is T)
			{
				return articleComponent as T;
			}
		}
		return (T)((object)null);
	}

	// Token: 0x06001261 RID: 4705 RVA: 0x00069D3C File Offset: 0x0006813C
	public virtual void OnCollisionSpawn(ArticleData spawnedArticle, bool switchFacing, Vector3F offset, Vector3F collisionPoint, bool destroyOriginal)
	{
		if (this.data.reflectSound.sound != null)
		{
			this.audioManager.PlayGameSound(new AudioRequest(this.data.reflectSound, (Vector3)this.Position, null));
		}
		if (switchFacing)
		{
			this.Facing = this.OppositeFacing;
		}
		if (spawnedArticle != null)
		{
			GameObject prefab = spawnedArticle.prefab;
			if (spawnedArticle.teamParticle)
			{
				UIColor uicolorFromTeam = PlayerUtil.GetUIColorFromTeam(this.Team);
				if (uicolorFromTeam == UIColor.Blue)
				{
					prefab = spawnedArticle.bluePrefab;
				}
				else if (uicolorFromTeam == UIColor.Red)
				{
					prefab = spawnedArticle.redPrefab;
				}
			}
			ArticleController articleController = ArticleData.CreateArticleController(this.gameManager.DynamicObjects, spawnedArticle.type, prefab, 4);
			articleController.model.physicsModel.Reset();
			offset.x *= InputUtils.GetDirectionMultiplier(this.Facing);
			articleController.model.physicsModel.position = collisionPoint + offset;
			articleController.model.rotationAngle = this.model.rotationAngle;
			Vector3F totalVelocity = this.context.model.totalVelocity;
			articleController.model.physicsModel.AddVelocity(ref totalVelocity, VelocityType.Movement);
			articleController.model.currentFacing = this.Facing;
			articleController.model.playerOwner = this.model.playerOwner;
			articleController.model.team = this.model.team;
			articleController.model.movementType = this.model.movementType;
			articleController.model.moveLabel = this.model.moveLabel;
			articleController.model.moveName = this.model.moveName;
			articleController.model.moveUID = this.model.moveUID;
			articleController.model.staleDamageMultiplier = this.model.staleDamageMultiplier;
			articleController.model.chargeData = this.model.chargeData;
			articleController.model.chargeFraction = this.model.chargeFraction;
			articleController.model.currentFrame = 1;
			articleController.Init(spawnedArticle);
			articleController.UpdateHitBoxes();
			if (destroyOriginal)
			{
				this.OnKilled();
				this.hostObject.DestroySafe();
			}
		}
	}

	// Token: 0x06001262 RID: 4706 RVA: 0x00069F88 File Offset: 0x00068388
	public IEvents getEvents()
	{
		return this.gameManager.events;
	}

	// Token: 0x06001263 RID: 4707 RVA: 0x00069F95 File Offset: 0x00068395
	bool IHitOwner.ResistsHit(HitData hitData, IHitOwner hitInstigator, Vector3F hitPosition)
	{
		return false;
	}

	// Token: 0x06001264 RID: 4708 RVA: 0x00069F98 File Offset: 0x00068398
	public bool ArmorResistsHit(HitData hitData, IHitOwner hitInstigator, Vector3F hitPosition)
	{
		return false;
	}

	// Token: 0x06001265 RID: 4709 RVA: 0x00069F9C File Offset: 0x0006839C
	public bool PerformBoundCast(AbsoluteDirection boundPoint, Fixed castDist, Vector2F castDirection, int castMask, out RaycastHitData hit)
	{
		Vector3F v = this.getBoundPointFromAbsoluteDirection(boundPoint);
		return PhysicsRaycastCalculator.GetFirstRaycastHit(this.context, v, castDirection, castDist, castMask, out hit, default(Fixed));
	}

	// Token: 0x06001266 RID: 4710 RVA: 0x00069FD8 File Offset: 0x000683D8
	private Vector2F getBoundPointFromAbsoluteDirection(AbsoluteDirection direction)
	{
		switch (direction)
		{
		case AbsoluteDirection.Up:
			return this.model.physicsModel.bounds.up + this.model.physicsModel.center;
		case AbsoluteDirection.Down:
			return this.model.physicsModel.bounds.down + this.model.physicsModel.center;
		case AbsoluteDirection.Left:
			return this.model.physicsModel.bounds.left + this.model.physicsModel.center;
		case AbsoluteDirection.Right:
			return this.model.physicsModel.bounds.right + this.model.physicsModel.center;
		default:
			return this.model.physicsModel.center;
		}
	}

	// Token: 0x04000BB0 RID: 2992
	public ArticleModel model = new ArticleModel();

	// Token: 0x04000BB1 RID: 2993
	protected ArticleData data;

	// Token: 0x04000BB4 RID: 2996
	protected PhysicsContext context;

	// Token: 0x04000BB5 RID: 2997
	protected IGameVFX gameVFX;

	// Token: 0x04000BB6 RID: 2998
	private List<IArticleComponent> articleComponents;

	// Token: 0x04000BB7 RID: 2999
	private ProjectilePhysicsImpactHandler impactHandler;

	// Token: 0x04000BB8 RID: 3000
	private ProjectilePhysicsCollisionMotion collisionMotion;

	// Token: 0x04000BB9 RID: 3001
	private AudioOwner audioOwner = new AudioOwner();

	// Token: 0x02000357 RID: 855
	// (Invoke) Token: 0x06001269 RID: 4713
	public delegate bool ComponentExecution<T>(T component);
}
