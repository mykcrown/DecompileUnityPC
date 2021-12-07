// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ArticleController : HostedGameBehavior, ITickable, IExpirable, IResetable, IHitOwner, IArticleDelegate, IRollbackStateOwner, IFacing, IPhysicsStateOwner, IEffectOwner, IPhysicsColliderOwner
{
	public delegate bool ComponentExecution<T>(T component);

	private sealed class _OnHitSuccess_c__AnonStorey0
	{
		internal Hit hit;

		internal IHitOwner other;

		internal ImpactType impactType;

		internal bool __m__0(IArticleCollisionController hitController)
		{
			return hitController.OnHitSuccess(this.hit, this.other, this.impactType);
		}
	}

	private sealed class _IHitOwner_HandleComponentHitInteraction_c__AnonStorey1
	{
		internal Hit otherHit;

		internal IHitOwner other;

		internal CollisionCheckType checkType;

		internal HitContext hitContext;

		internal ArticleController _this;

		internal bool __m__0(IArticleCollisionController component)
		{
			return component.HandleHitBoxCollision(this.otherHit, this.other, this._this, this.checkType, this.hitContext);
		}
	}

	private sealed class _Explode_c__AnonStorey2
	{
		internal bool canExplode;

		internal bool __m__0(IArticleExplosionController explosionController)
		{
			this.canExplode = explosionController.CanExplode();
			return this.canExplode;
		}
	}

	public ArticleModel model = new ArticleModel();

	protected ArticleData data;

	protected PhysicsContext context;

	protected IGameVFX gameVFX;

	private List<IArticleComponent> articleComponents;

	private ProjectilePhysicsImpactHandler impactHandler;

	private ProjectilePhysicsCollisionMotion collisionMotion;

	private AudioOwner audioOwner = new AudioOwner();

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

	MoveData IPhysicsStateOwner.CurrentMove
	{
		get
		{
			return null;
		}
	}

	MoveController IHitOwner.ActiveMove
	{
		get
		{
			return null;
		}
	}

	ArticleData IArticleDelegate.Data
	{
		get
		{
			return this.data;
		}
	}

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

	[Inject]
	public ICombatCalculator combatCalculator
	{
		get;
		set;
	}

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServerAPI
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	protected GameManager gameManager
	{
		get
		{
			return this.gameController.currentGame;
		}
	}

	protected GameLog log
	{
		get
		{
			return (!(this.gameManager == null)) ? this.gameManager.Log : null;
		}
	}

	public Vector3F Velocity
	{
		get
		{
			return this.context.model.totalVelocity;
		}
	}

	public PhysicsSimulator Simulator
	{
		get;
		private set;
	}

	public IPhysicsCollider Collider
	{
		get;
		private set;
	}

	public PlayerPhysicsController Physics
	{
		get
		{
			return null;
		}
	}

	public IPlayerDelegate player
	{
		get
		{
			return this.gameManager.GetPlayerController(this.model.playerOwner);
		}
	}

	public ArticleModel Model
	{
		get
		{
			return this.model;
		}
	}

	public new Transform transform
	{
		get
		{
			return (!(this.hostObject == null)) ? this.hostObject.transform : base.transform;
		}
	}

	public virtual List<Hit> Hits
	{
		get
		{
			return this.model.hits;
		}
	}

	public List<HurtBoxState> HurtBoxes
	{
		get
		{
			return null;
		}
	}

	public List<HitBoxState> ShieldBoxes
	{
		get
		{
			return null;
		}
	}

	public bool IsInvincible
	{
		get
		{
			return false;
		}
	}

	public bool IsAllyAssist
	{
		get
		{
			return false;
		}
	}

	public bool AssistAbsorbsHits
	{
		get
		{
			return false;
		}
	}

	public bool IsProjectile
	{
		get
		{
			return this.data.type == ArticleType.Projectile;
		}
	}

	public HitOwnerType Type
	{
		get
		{
			return HitOwnerType.Article;
		}
	}

	public TeamNum Team
	{
		get
		{
			return this.model.team;
		}
	}

	public PlayerNum PlayerNum
	{
		get
		{
			return this.model.playerOwner;
		}
	}

	public Vector3F Position
	{
		get
		{
			return this.model.physicsModel.position;
		}
	}

	public Vector3F Center
	{
		get
		{
			return this.model.physicsModel.center;
		}
	}

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

	public virtual HorizontalDirection OppositeFacing
	{
		get
		{
			return (this.model.currentFacing != HorizontalDirection.Left) ? HorizontalDirection.Left : HorizontalDirection.Right;
		}
	}

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

	public Fixed StaleDamageMultiplier
	{
		get
		{
			return this.model.staleDamageMultiplier;
		}
	}

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

	public bool IsExpired
	{
		get
		{
			return this.model.isExpired;
		}
	}

	public override void Reset()
	{
		base.Reset();
		this.model.Reset();
		if (this.articleComponents != null)
		{
			foreach (IArticleComponent current in this.articleComponents)
			{
				if (current is IResetable)
				{
					((IResetable)current).Reset();
				}
			}
		}
		for (int i = 0; i < this.model.activeSoundEffects.Count; i++)
		{
			this.model.activeSoundEffects[i].Stop();
		}
		this.model.activeSoundEffects.Clear();
	}

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

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ArticleModel>(ref this.model);
		this.context.model = this.model.physicsModel;
		foreach (IArticleComponent current in this.articleComponents)
		{
			if (current is IRollbackStateOwner)
			{
				((IRollbackStateOwner)current).LoadState(container);
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

	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<ArticleModel>(this.model));
		foreach (IArticleComponent current in this.articleComponents)
		{
			if (current is IRollbackStateOwner)
			{
				((IRollbackStateOwner)current).ExportState(ref container);
			}
		}
		return true;
	}

	public void Awake()
	{
		if (this.events != null)
		{
			this.events.Subscribe(typeof(ToggleDebugDrawChannelCommand), new Events.EventHandler(this.onToggleDebugChannel));
		}
	}

	public virtual void SetEffect(Effect effect)
	{
		this.model.effect = effect;
	}

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
			UnityEngine.Debug.LogWarning("Unable to find effect on this ArticleController.  Effect may not have been set. " + base.name);
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
		MoveCameraShakeData[] cameraShakes = data.cameraShakes;
		for (int l = 0; l < cameraShakes.Length; l++)
		{
			MoveCameraShakeData moveCameraShakeData = cameraShakes[l];
			this.gameManager.Camera.ShakeCamera(new CameraShakeRequest(moveCameraShakeData.shake));
		}
	}

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

	public void OnDamageTaken(Fixed damage, ImpactType impactType)
	{
	}

	public void BeginHitLag(int hitLagFrames, IHitOwner owner, HitData hitData)
	{
		this.beginHitLag(hitLagFrames);
	}

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

	private Fixed calculateMaxSpeed()
	{
		return 0;
	}

	private void onToggleDebugChannel(GameEvent message)
	{
		ToggleDebugDrawChannelCommand toggleDebugDrawChannelCommand = message as ToggleDebugDrawChannelCommand;
		if (toggleDebugDrawChannelCommand.channel == DebugDrawChannel.HitBoxes)
		{
			this.toggleHitBoxCapsules(toggleDebugDrawChannelCommand.enabled);
		}
	}

	private void toggleHitBoxCapsules(bool enabled)
	{
		if (this.model.isExpired)
		{
			return;
		}
		if (enabled)
		{
			foreach (Hit current in this.model.hits)
			{
				foreach (HitBoxState current2 in current.hitBoxes)
				{
					CapsuleShape capsule = CapsulePool.Instance.GetCapsule(this.player.Transform);
					capsule.Load(current2.position, current2.lastPosition, (Fixed)((double)current2.data.radius), current.data.DebugDrawColor, current2.IsCircle);
					this.model.hitBoxCapsules[current2] = capsule;
				}
			}
		}
		else
		{
			foreach (CapsuleShape current3 in this.model.hitBoxCapsules.Values)
			{
				current3.Clear();
			}
			this.model.hitBoxCapsules.Clear();
		}
	}

	public bool AllowClanking(HitData hitData, IHitOwner other)
	{
		HitType hitType = hitData.hitType;
		return (hitType != HitType.Hit && hitType != HitType.Projectile) || this.data.collideWithHitBoxes;
	}

	bool IHitOwner.ForceCollisionChecks(CollisionCheckType type, HitData hitData)
	{
		if (type != CollisionCheckType.HitBox)
		{
			return false;
		}
		using (List<IArticleComponent>.Enumerator enumerator = this.articleComponents.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ArticleComponent articleComponent = (ArticleComponent)enumerator.Current;
				if (articleComponent is IArticleCollisionController)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsImmune(HitData hitData, IHitOwner enemy)
	{
		return hitData.hitType == HitType.Grab || hitData.hitType == HitType.BlockableGrab;
	}

	public bool CanReflect(HitData hitData)
	{
		return false;
	}

	public bool ShouldReflect(IHitOwner other, ref Vector3F collisionPoint, CollisionCheckType type, Hit myHit = null)
	{
		return false;
	}

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

	private void updateFacing(HorizontalDirection facing)
	{
		this.model.currentFacing = facing;
		this.updateRotation();
	}

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

	private Vector3 getSyncToRotationAngle()
	{
		return new Vector3(0f, 0f, (float)this.model.rotationAngle);
	}

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

	public HorizontalDirection CalculateVictimFacing(bool hitWasReversed)
	{
		return (!hitWasReversed) ? InputUtils.GetOppositeDirection(this.Facing) : this.Facing;
	}

	public virtual bool ShouldCancelClankedMove(Hit myHit, Hit otherHit, IHitOwner other)
	{
		return false;
	}

	public virtual bool OnHitSuccess(Hit hit, IHitOwner other, ImpactType impactType, ref Vector3F hitPosition, ref Vector3F hitVelocity, HitContext hitContext)
	{
		ArticleController._OnHitSuccess_c__AnonStorey0 _OnHitSuccess_c__AnonStorey = new ArticleController._OnHitSuccess_c__AnonStorey0();
		_OnHitSuccess_c__AnonStorey.hit = hit;
		_OnHitSuccess_c__AnonStorey.other = other;
		_OnHitSuccess_c__AnonStorey.impactType = impactType;
		ArticleController.ComponentExecution<IArticleCollisionController> execute = new ArticleController.ComponentExecution<IArticleCollisionController>(_OnHitSuccess_c__AnonStorey.__m__0);
		bool flag = this.ExecuteArticleComponents<IArticleCollisionController>(execute);
		if (!flag)
		{
			HitData hitData = _OnHitSuccess_c__AnonStorey.hit.data;
			this.model.disabledHits.Disable(hitData, _OnHitSuccess_c__AnonStorey.other, this.model.currentFrame);
			this.gameVFX.CreateHitEffect(this, hitData, hitPosition, -1, _OnHitSuccess_c__AnonStorey.impactType, _OnHitSuccess_c__AnonStorey.other);
			if (!_OnHitSuccess_c__AnonStorey.other.IsInvincible && this.player != null)
			{
				this.player.OnDamageDealt(hitData.damage * this.DamageMultiplier, _OnHitSuccess_c__AnonStorey.impactType, this.data.chargesMeter);
				this.player.StaleMove(this.model.moveLabel, this.model.moveName, this.model.moveUID);
				_OnHitSuccess_c__AnonStorey.other.OnDamageTaken(hitData.damage * this.DamageMultiplier, _OnHitSuccess_c__AnonStorey.impactType);
			}
			ParticleData lethalHit = this.config.defaultCharacterEffects.lethalHit;
			if ((this.config.defaultCharacterEffects.lethalHitSound.sound != null || (lethalHit != null && lethalHit.prefab != null)) && this.combatCalculator.IsLethalHit(hitData, this, _OnHitSuccess_c__AnonStorey.other, hitPosition, 1))
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

	bool IHitOwner.HandleComponentHitInteraction(Hit otherHit, IHitOwner other, CollisionCheckType checkType, HitContext hitContext)
	{
		ArticleController._IHitOwner_HandleComponentHitInteraction_c__AnonStorey1 _IHitOwner_HandleComponentHitInteraction_c__AnonStorey = new ArticleController._IHitOwner_HandleComponentHitInteraction_c__AnonStorey1();
		_IHitOwner_HandleComponentHitInteraction_c__AnonStorey.otherHit = otherHit;
		_IHitOwner_HandleComponentHitInteraction_c__AnonStorey.other = other;
		_IHitOwner_HandleComponentHitInteraction_c__AnonStorey.checkType = checkType;
		_IHitOwner_HandleComponentHitInteraction_c__AnonStorey.hitContext = hitContext;
		_IHitOwner_HandleComponentHitInteraction_c__AnonStorey._this = this;
		return this.ExecuteArticleComponents<IArticleCollisionController>(new ArticleController.ComponentExecution<IArticleCollisionController>(_IHitOwner_HandleComponentHitInteraction_c__AnonStorey.__m__0));
	}

	public virtual void ReceiveHit(HitData hitData, IHitOwner other, ImpactType impactType, HitContext hitContext)
	{
	}

	public virtual void OnHitBoxCollision(Hit myHit, IHitOwner other, Hit otherHit, ref Vector3F hitPosition, bool cancelMine, bool makeClank)
	{
	}

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
		SoundEffect[] soundEffects = this.data.soundEffects;
		for (int i = 0; i < soundEffects.Length; i++)
		{
			SoundEffect soundEffect = soundEffects[i];
			if (this.model.currentFrame == soundEffect.frame && soundEffect.sounds.Length > 0)
			{
				this.model.activeSoundEffects.Add(new AudioHandle(this.gameManager.Audio, soundEffect, this.audioOwner));
			}
		}
		this.model.currentFrame++;
		this.ExecuteArticleComponents<IArticleMovementController>(new ArticleController.ComponentExecution<IArticleMovementController>(this._TickFrame_m__0));
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

	public virtual void Explode(bool killMe = true)
	{
		ArticleController._Explode_c__AnonStorey2 _Explode_c__AnonStorey = new ArticleController._Explode_c__AnonStorey2();
		_Explode_c__AnonStorey.canExplode = true;
		ArticleController.ComponentExecution<IArticleExplosionController> execute = new ArticleController.ComponentExecution<IArticleExplosionController>(_Explode_c__AnonStorey.__m__0);
		this.ExecuteArticleComponents<IArticleExplosionController>(execute);
		if (!_Explode_c__AnonStorey.canExplode)
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
					int num = (this.model.currentFacing != HorizontalDirection.Left) ? 1 : (-1);
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

	protected virtual void OnKilled()
	{
		foreach (CapsuleShape current in this.model.hitBoxCapsules.Values)
		{
			current.Clear();
		}
		this.model.hitBoxCapsules.Clear();
		this.model.isExpired = true;
		this.gameManager.Hits.Unregister(this);
	}

	private void OnDisable()
	{
		this.OnDestroy();
	}

	public void OnDestroy()
	{
		if (this.events != null)
		{
			this.events.Unsubscribe(typeof(ToggleDebugDrawChannelCommand), new Events.EventHandler(this.onToggleDebugChannel));
		}
	}

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
				foreach (HitBoxState current in hit.hitBoxes)
				{
					if (this.model.hitBoxCapsules.ContainsKey(current))
					{
						CapsuleShape capsuleShape2 = this.model.hitBoxCapsules[current];
						capsuleShape2.Visible = false;
					}
				}
			}
		}
	}

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

	public T GetArticleComponent<T>() where T : class, IArticleComponent
	{
		foreach (IArticleComponent current in this.articleComponents)
		{
			if (current is T)
			{
				return current as T;
			}
		}
		return (T)((object)null);
	}

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
				UIColor uIColorFromTeam = PlayerUtil.GetUIColorFromTeam(this.Team);
				if (uIColorFromTeam == UIColor.Blue)
				{
					prefab = spawnedArticle.bluePrefab;
				}
				else if (uIColorFromTeam == UIColor.Red)
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

	public IEvents getEvents()
	{
		return this.gameManager.events;
	}

	bool IHitOwner.ResistsHit(HitData hitData, IHitOwner hitInstigator, Vector3F hitPosition)
	{
		return false;
	}

	public bool ArmorResistsHit(HitData hitData, IHitOwner hitInstigator, Vector3F hitPosition)
	{
		return false;
	}

	public bool PerformBoundCast(AbsoluteDirection boundPoint, Fixed castDist, Vector2F castDirection, int castMask, out RaycastHitData hit)
	{
		Vector3F v = this.getBoundPointFromAbsoluteDirection(boundPoint);
		return PhysicsRaycastCalculator.GetFirstRaycastHit(this.context, v, castDirection, castDist, castMask, out hit, default(Fixed));
	}

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

	private bool _TickFrame_m__0(IArticleMovementController movementController)
	{
		Vector3F zero = Vector3F.zero;
		bool result = movementController.TickMovement(ref zero, ref this.context.model.position);
		this.context.model.SetVelocity(zero, VelocityType.Movement);
		return result;
	}
}
