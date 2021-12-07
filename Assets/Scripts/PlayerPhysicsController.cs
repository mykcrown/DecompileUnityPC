// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsController : GameBehavior, IMovePhysics, IRollbackStateOwner, IPhysicsStateOwner, IPhysicsColliderOwner, ICharacterPhysicsData
{
	public static int UNCAPPED_MAX_SPEED = 10000;

	public static Fixed MIN_ENVIRONMENT_BOUNDS_DELTA = (Fixed)0.001;

	public static readonly Fixed BOUNDS_CAST_TOLERANCE = (Fixed)0.005;

	public static readonly Vector3F BOUNDS_CAST_UP_OFFSET = Vector3F.down * PlayerPhysicsController.BOUNDS_CAST_TOLERANCE;

	public static readonly Vector3F BOUNDS_CAST_DOWN_OFFSET = Vector3F.up * PlayerPhysicsController.BOUNDS_CAST_TOLERANCE;

	public static readonly Vector3F BOUNDS_CAST_LEFT_OFFSET = Vector3F.right * PlayerPhysicsController.BOUNDS_CAST_TOLERANCE;

	public static readonly Vector3F BOUNDS_CAST_RIGHT_OFFSET = Vector3F.left * PlayerPhysicsController.BOUNDS_CAST_TOLERANCE;

	private PhysicsModel state;

	private PlayerPhysicsModel playerState;

	private PhysicsContext context;

	private IPhysicsDelegate player;

	private ECBOverrideData defaultECBData = new ECBOverrideData();

	private List<CollisionData> collisionsBuffer = new List<CollisionData>();

	private EdgeData previousBoundsBuffer;

	private static Fixed MIN_COLLISION_DIAMOND_WIDTH = (Fixed)0.40000000596046448;

	private static Fixed MIN_COLLISION_DIAMOND_HEIGHT = (Fixed)0.40000000596046448;

	PhysicsOverride IPhysicsStateOwner.PhysicsOverride
	{
		get
		{
			return this.state.physicsOverride;
		}
		set
		{
			this.state.physicsOverride = value;
		}
	}

	MoveData IPhysicsStateOwner.CurrentMove
	{
		get
		{
			return this.player.CurrentMove;
		}
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	public PhysicsSimulator Simulator
	{
		get;
		private set;
	}

	public Vector3F Velocity
	{
		get
		{
			return this.state.totalVelocity;
		}
	}

	public Vector3F KnockbackVelocity
	{
		get
		{
			return this.state.GetVelocity(VelocityType.Knockback);
		}
	}

	public Vector3F MovementVelocity
	{
		get
		{
			return this.state.GetVelocity(VelocityType.Movement);
		}
	}

	public Vector3F ForcedVelocity
	{
		get
		{
			return this.state.GetVelocity(VelocityType.Forced);
		}
	}

	public Vector3F Center
	{
		get
		{
			return this.state.center;
		}
	}

	public bool IsFastFalling
	{
		get
		{
			return this.playerState.isFastFalling;
		}
	}

	public HorizontalDirection TeeteringDirection
	{
		get
		{
			return this.playerState.teeteringDirection;
		}
	}

	public PhysicsModel State
	{
		get
		{
			return this.state;
		}
	}

	public PlayerPhysicsModel PlayerState
	{
		get
		{
			return this.playerState;
		}
	}

	public bool IsGrounded
	{
		get
		{
			return this.state.isGrounded;
		}
	}

	public bool UsedAirJump
	{
		get
		{
			return this.playerState.usedAirJump;
		}
	}

	public bool UsedGroundJump
	{
		get
		{
			return this.playerState.usedGroundJump;
		}
	}

	public bool WasHit
	{
		get
		{
			return this.playerState.wasHit;
		}
		set
		{
			this.playerState.wasHit = value;
		}
	}

	public Vector3F GroundedNormal
	{
		get
		{
			return this.state.groundedNormal;
		}
	}

	public bool IsAboveStage
	{
		get
		{
			return this.Simulator.CheckIfAboveStage(this.context);
		}
	}

	public IMovingObject GroundedMovingObject
	{
		get
		{
			return this.state.groundedMovingStageObject;
		}
	}

	public Fixed SteerMomentumMaxAnglePerFrame
	{
		get
		{
			return (this.physicsOverride == null) ? 0 : this.physicsOverride.steerMomentumMaxAnglePerFrame;
		}
	}

	public Fixed SteerMomentumMinOverallAngle
	{
		get
		{
			return (this.physicsOverride == null) ? 0 : this.physicsOverride.steerMomentumMinOverallAngle;
		}
	}

	public Fixed SteerMomentumMaxOverallAngle
	{
		get
		{
			return (this.physicsOverride == null) ? 0 : this.physicsOverride.steerMomentumMaxOverallAngle;
		}
	}

	public bool SteerMomentumFaceVelocity
	{
		get
		{
			return this.physicsOverride != null && this.physicsOverride.steerMomentumFaceVelocity;
		}
	}

	public bool PreventGroundedness
	{
		get
		{
			return this.physicsOverride != null && this.physicsOverride.preventGroundedness;
		}
	}

	public IPhysicsCollider Collider
	{
		get;
		private set;
	}

	private CharacterPhysicsData data
	{
		get
		{
			return (this.context == null) ? null : this.context.defaultCharacterData;
		}
	}

	private CharacterPhysicsOverride dataOverride
	{
		get
		{
			return this.state.characterPhysicsOverride;
		}
	}

	private PhysicsOverride physicsOverride
	{
		get
		{
			return (this.state == null) ? null : this.state.physicsOverride;
		}
	}

	public PhysicsContext Context
	{
		get
		{
			return this.context;
		}
	}

	public EnvironmentBounds Bounds
	{
		get
		{
			return this.state.bounds;
		}
	}

	public Fixed SlowWalkMaxSpeed
	{
		get
		{
			return this.dataOverride.slowWalkMaxSpeed.GetValueOrDefault((Fixed)((double)this.data.slowWalkMaxSpeed));
		}
	}

	public Fixed MediumWalkMaxSpeed
	{
		get
		{
			return this.dataOverride.mediumWalkMaxSpeed.GetValueOrDefault((Fixed)((double)this.data.mediumWalkMaxSpeed));
		}
	}

	public Fixed FastWalkMaxSpeed
	{
		get
		{
			return this.dataOverride.fastWalkMaxSpeed.GetValueOrDefault((Fixed)((double)this.data.fastWalkMaxSpeed));
		}
	}

	public Fixed RunMaxSpeed
	{
		get
		{
			return this.dataOverride.runMaxSpeed.GetValueOrDefault((Fixed)((double)this.data.runMaxSpeed));
		}
	}

	public Fixed GroundToAirMaxSpeed
	{
		get
		{
			return this.dataOverride.groundToAirMaxSpeed.GetValueOrDefault((Fixed)((double)this.data.groundToAirMaxSpeed));
		}
	}

	public Fixed WalkAcceleration
	{
		get
		{
			return this.dataOverride.walkAcceleration.GetValueOrDefault((Fixed)((double)this.data.walkAcceleration));
		}
	}

	public Fixed RunPivotAcceleration
	{
		get
		{
			return this.dataOverride.runPivotAcceleration.GetValueOrDefault((Fixed)((double)this.data.runPivotAcceleration));
		}
	}

	public Fixed Friction
	{
		get
		{
			return this.dataOverride.friction.GetValueOrDefault((Fixed)((double)this.data.friction));
		}
	}

	public Fixed HighSpeedFriction
	{
		get
		{
			return this.dataOverride.highSpeedFriction.GetValueOrDefault((Fixed)((double)this.data.highSpeedFriction));
		}
	}

	public Fixed DashStartSpeed
	{
		get
		{
			return this.dataOverride.dashStartSpeed.GetValueOrDefault((Fixed)((double)this.data.dashStartSpeed));
		}
	}

	public Fixed DashMaxSpeed
	{
		get
		{
			return this.dataOverride.dashMaxSpeed.GetValueOrDefault((Fixed)((double)this.data.dashMaxSpeed));
		}
	}

	public Fixed DashAcceleration
	{
		get
		{
			return this.dataOverride.dashAcceleration.GetValueOrDefault((Fixed)((double)this.data.dashAcceleration));
		}
	}

	public Fixed AirMaxSpeed
	{
		get
		{
			return this.dataOverride.airMaxSpeed.GetValueOrDefault((Fixed)((double)this.data.airMaxSpeed));
		}
	}

	public Fixed AirAcceleration
	{
		get
		{
			return this.dataOverride.airAcceleration.GetValueOrDefault((Fixed)((double)this.data.airAcceleration));
		}
	}

	public Fixed AirFriction
	{
		get
		{
			return this.dataOverride.airFriction.GetValueOrDefault((Fixed)((double)this.data.airFriction));
		}
	}

	public Fixed Gravity
	{
		get
		{
			return this.dataOverride.gravity.GetValueOrDefault((Fixed)((double)this.data.gravity));
		}
	}

	public Fixed MaxFallSpeed
	{
		get
		{
			return this.dataOverride.maxFallSpeed.GetValueOrDefault((Fixed)((double)this.data.maxFallSpeed));
		}
	}

	public Fixed HelplessAirSpeedMultiplier
	{
		get
		{
			return this.dataOverride.helplessAirSpeedMultiplier.GetValueOrDefault((Fixed)((double)this.data.helplessAirSpeedMultiplier));
		}
	}

	public Fixed HelplessAirAccelerationMultiplier
	{
		get
		{
			return this.dataOverride.helplessAirAccelerationMultiplier.GetValueOrDefault((Fixed)((double)this.data.helplessAirAccelerationMultiplier));
		}
	}

	public Fixed JumpSpeed
	{
		get
		{
			return this.dataOverride.jumpSpeed.GetValueOrDefault((Fixed)((double)this.data.jumpSpeed));
		}
	}

	public Fixed SecondaryJumpSpeed
	{
		get
		{
			return this.dataOverride.secondaryJumpSpeed.GetValueOrDefault((Fixed)((double)this.data.secondaryJumpSpeed));
		}
	}

	public Fixed ShortJumpSpeed
	{
		get
		{
			return this.dataOverride.shortJumpSpeed.GetValueOrDefault((Fixed)((double)this.data.shortJumpSpeed));
		}
	}

	public int JumpCount
	{
		get
		{
			return this.dataOverride.jumpCount.GetValueOrDefault(this.data.jumpCount);
		}
	}

	public Fixed Weight
	{
		get
		{
			return this.dataOverride.weight.GetValueOrDefault((Fixed)((double)this.data.weight));
		}
	}

	public Fixed FastFallSpeed
	{
		get
		{
			return this.dataOverride.fastFallSpeed.GetValueOrDefault((Fixed)((double)this.data.fastFallSpeed));
		}
	}

	public Fixed ShieldBreakSpeed
	{
		get
		{
			return this.dataOverride.shieldBreakSpeed.GetValueOrDefault((Fixed)((double)this.data.shieldBreakSpeed));
		}
	}

	public bool IgnorePlatforms
	{
		get
		{
			return this.dataOverride.ignorePlatforms.GetValueOrDefault(this.data.ignorePlatforms);
		}
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<PhysicsModel>(this.state));
		container.WriteState(this.rollbackStatePooling.Clone<PlayerPhysicsModel>(this.playerState));
		return true;
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<PhysicsModel>(ref this.state);
		container.ReadState<PlayerPhysicsModel>(ref this.playerState);
		base.transform.position = (Vector3)this.state.position;
		this.context.model = this.state;
		this.context.playerPhysicsModel = this.playerState;
		return true;
	}

	public void Initialize(IPhysicsDelegate player)
	{
		this.player = player;
		this.initEnvironmentBoundsBoneList();
		this.state = new PhysicsModel();
		this.playerState = new PlayerPhysicsModel();
		this.Simulator = base.gameManager.Physics;
		this.state.position = (Vector3F)base.transform.position;
		this.Collider = new PhysicsCollider(new EdgeData(this.state.center, true, EdgeData.CacheFlag.NoSurface, new Vector2F[]
		{
			this.state.bounds.up,
			this.state.bounds.right,
			this.state.bounds.down,
			this.state.bounds.left
		}), LayerMask.NameToLayer("Player"));
		this.previousBoundsBuffer = new EdgeData(new Vector2F[4], true, EdgeData.CacheFlag.NoSurface);
		this.context = this.CreateContext(this.state, this.playerState, player, true);
	}

	private void initEnvironmentBoundsBoneList()
	{
	}

	public PhysicsContext CreateContext(PhysicsModel model, PlayerPhysicsModel playerPhysicsModel, IPhysicsDelegate playerDelegate, bool hasLandCallback)
	{
		return new PhysicsContext
		{
			impactHandler = new PlayerPhysicsImpactHandler(),
			collisionMotion = new PlayerPhysicsCollisionMotion(),
			physicsState = this,
			defaultCharacterData = this.player.DefaultData,
			characterData = this,
			world = base.gameManager.PhysicsWorld,
			fallThroughPlatformsCallback = new FallThroughPlatformsCallback(this.player.ShouldFallThroughPlatforms),
			isPlatformLastDropped = new Func<IPhysicsCollider, bool>(this.player.IsPlatformLastDropped),
			isFallingThroughPlatformCallback = new PlatformDroppingCallback(this.isFallingThroughPlatform),
			shouldBounceCallback = new ShouldBounceCallback(this.player.ShouldBounce),
			onKnockbackBounceCallback = new OnKnockbackBounceCallback(this.onKnockbackBounce),
			calculateMaxHorizontalSpeedCallback = new CalculateMaxSpeedCallback(this.player.CalculateMaxHorizontalSpeed),
			cliffProtectionCallback = new CliffProtectionCallback(this.cliffProtectionCallback),
			beginFallCallback = new BeginFallCallback(this.onGroundToAir),
			fallCallback = new FallCallback(this.fallCallback),
			onCliffProtection = new Action<Vector3F, Vector3F>(this.onCliffProtection),
			availableTechCallback = new AvailableTechCallback(this.player.AvailableTech),
			performTechCallback = new PerformTechCallback(this.techCallback),
			ignoreCollisionsCallback = new IgnoreCollisionsCallback(this.player.IgnorePhysicsCollisions),
			shouldCheckGrounded = new CheckGroundedCallback(this.shouldCheckGroundCollision),
			shouldHaltOnCollision = new HaltOnCollisionCallback(this.shouldHaltOnCollision),
			shouldMaintainVelocityCallback = new MaintainVelocityCallback(this.player.ShouldMaintainVelocityOnCollision),
			shouldIgnoreForcesCallback = new ShouldIgnoreForcesCallback(this.shouldIgnoreForces),
			colliderOwner = this,
			knockbackConfig = base.config.knockbackConfig,
			lagConfig = base.config.lagConfig,
			model = model,
			playerPhysicsModel = playerPhysicsModel,
			playerDelegate = playerDelegate,
			landCallback = ((!hasLandCallback) ? null : new LandCallback(this.landCallback))
		};
	}

	private bool shouldIgnoreForces()
	{
		return this.player.State.ShouldIgnoreForces;
	}

	private bool shouldHaltOnCollision(PhysicsMotionContext motionContext, CollisionData collision)
	{
		return this.player.IsUnderContinuousForce && Vector3F.Dot(collision.normal, Vector3F.down) > 0 && Vector3F.Dot(motionContext.initialMovementVelocity, Vector3F.down) >= 0;
	}

	private bool isFallingThroughPlatform(IPhysicsCollider platformCollider)
	{
		return this.player.State.IsPlatformDropping && this.player.IsPlatformLastDropped(platformCollider);
	}

	private void landCallback(ref Vector3F previousVelocity)
	{
		this.resetStateOnLand();
		this.player.OnLand(ref previousVelocity);
	}

	private void fallCallback()
	{
		this.player.OnFall();
	}

	private void onKnockbackBounce(CollisionData collision)
	{
		if (collision.CollisionSurfaceType == SurfaceType.Floor)
		{
			this.context.playerPhysicsModel.usedAirJump = false;
			this.player.OnGroundBounce();
		}
		this.context.model.IsGrounded = false;
		this.player.State.MetaState = MetaState.Jump;
		this.player.Combat.OnKnockbackBounce(collision);
	}

	private void techCallback(TechType techType, CollisionData collision)
	{
		if (techType == TechType.Ground)
		{
			this.resetStateOnLand();
		}
		this.player.PerformTech(techType, collision);
	}

	private void resetStateOnLand()
	{
		this.context.playerPhysicsModel.usedAirJump = false;
		this.context.playerPhysicsModel.usedGroundJump = false;
		this.context.playerPhysicsModel.isFastFalling = false;
		if (!this.context.model.isGrounded)
		{
			this.context.model.isGrounded = true;
		}
	}

	public void ClearFastFall()
	{
		this.playerState.isFastFalling = false;
	}

	public void ApplyAcceleration(HorizontalDirection horizontalDirection, Fixed horizontalAxisValue)
	{
		int directionMultiplier = InputUtils.GetDirectionMultiplier(horizontalDirection);
		if (this.IsGrounded)
		{
			Vector3F a = new Vector3F(this.state.groundedNormal.y, -this.state.groundedNormal.x, 0);
			Fixed d = this.player.GetHorizontalAcceleration(true) * directionMultiplier;
			this.state.acceleration = d * a;
		}
		else
		{
			Fixed x = this.player.GetHorizontalAcceleration(false) * directionMultiplier * FixedMath.Abs(horizontalAxisValue);
			this.state.acceleration.x = x;
		}
	}

	public void BeginFastFalling()
	{
		Vector3F movementVelocity = this.state.movementVelocity;
		movementVelocity.y = -(Fixed)((double)this.data.fastFallSpeed);
		this.state.SetVelocity(movementVelocity, VelocityType.Movement);
		this.playerState.isFastFalling = true;
	}

	public void DelayedFastFall()
	{
		Vector3F movementVelocity = this.state.movementVelocity;
		movementVelocity.y = -(Fixed)((double)this.data.fastFallSpeed);
		this.state.SetVelocity(movementVelocity, VelocityType.Movement);
	}

	public void BeginDashBrakingOrPivoting()
	{
		if (base.config.knockbackConfig.globalCapBrakePrivotSpeed)
		{
			this.Simulator.CapHorizontalSpeed(this.state, (Fixed)((double)this.data.fastWalkMaxSpeed), VelocityType.Movement);
		}
	}

	public void Jump(Fixed horizontalInput)
	{
		if (!this.state.isGrounded)
		{
			this.applyJump((Fixed)((double)this.data.secondaryJumpSpeed), true, horizontalInput);
		}
		else
		{
			this.applyJump((Fixed)((double)this.data.jumpSpeed), false, horizontalInput);
		}
	}

	public void CancelIntoShorthop()
	{
		if (!this.state.IsGrounded && this.state.movementVelocity.y != (Fixed)((double)this.data.shortJumpSpeed))
		{
			Fixed src = (Fixed)((double)this.data.jumpSpeed) - (Fixed)((double)this.data.shortJumpSpeed);
			Vector3F vector3F = new Vector3F(0, -src, 0);
			this.state.AddVelocity(ref vector3F, VelocityType.Movement);
		}
	}

	public void ShortJump(Fixed horizontalInput)
	{
		if (!this.state.isGrounded)
		{
			this.Jump(horizontalInput);
		}
		else
		{
			this.applyJump((Fixed)((double)this.data.shortJumpSpeed), false, horizontalInput);
		}
	}

	private void applyJump(Fixed force, bool isAirJump, Fixed horizontalInput)
	{
		Vector3F movementVelocity = this.state.movementVelocity;
		if (isAirJump)
		{
			this.playerState.usedAirJump = true;
		}
		else
		{
			this.onGroundToAir();
			movementVelocity = this.state.movementVelocity;
			Fixed @fixed = (Fixed)((double)this.data.groundToAirMaxSpeed);
			bool flag = horizontalInput * movementVelocity.x < 0;
			if (flag)
			{
				@fixed = 0;
			}
			if (FixedMath.Abs(movementVelocity.x) > @fixed)
			{
				if (movementVelocity.x > 0)
				{
					movementVelocity.x = @fixed;
				}
				else if (movementVelocity.x < 0)
				{
					movementVelocity.x = -@fixed;
				}
			}
		}
		this.state.isGrounded = false;
		this.playerState.isFastFalling = false;
		this.playerState.usedGroundJump = true;
		this.state.pivotJump = false;
		if (isAirJump)
		{
			Fixed x = this.AirMaxSpeed * horizontalInput;
			movementVelocity.x = x;
		}
		else if (MathUtil.SignsMatch(this.state.movementVelocity.x, -horizontalInput))
		{
			movementVelocity.x = 0;
		}
		movementVelocity.y = force;
		this.state.SetVelocity(movementVelocity, VelocityType.Movement);
		this.player.OnJump();
	}

	private void tickAirAccelerationHorizontal(ref Vector3F newVelocity)
	{
		this.tickAirAcceleration(ref newVelocity.x, ref this.state.targetMoveVelocityHorizontalFrames, this.state.movementVelocity.x, this.state.targetMoveVelocity.x, (Fixed)((double)this.data.jumpHorizontalAccel));
	}

	private void tickJumpAccelerationVertical(ref Vector3F newVelocity)
	{
		this.tickAirAcceleration(ref newVelocity.y, ref this.state.targetMoveVelocityVerticalFrames, this.state.movementVelocity.y, this.state.targetMoveVelocity.y, (Fixed)((double)this.data.jumpVerticalAccel));
	}

	private void tickAirAcceleration(ref Fixed newVelocity, ref int frameCounter, Fixed baseVelocity, Fixed targetVelocity, Fixed accel)
	{
		if (FixedMath.Abs(baseVelocity - targetVelocity) <= accel)
		{
			newVelocity = targetVelocity;
			frameCounter = 0;
		}
		else
		{
			int multi = (!(baseVelocity < targetVelocity)) ? (-1) : 1;
			newVelocity += accel * multi;
		}
	}

	private void onGroundToAir()
	{
		if (!this.player.State.IsDownState)
		{
			Vector3F knockbackVelocity = this.KnockbackVelocity;
			this.state.ClearVelocity(true, true, true, VelocityType.Knockback);
			this.state.AddVelocity(ref knockbackVelocity, VelocityType.Movement);
		}
	}

	public void StopMovement(bool stopX, bool stopY, VelocityType velocityType)
	{
		this.ResetDelayedMovement();
		this.state.ClearVelocity(stopX, stopY, false, velocityType);
		if (stopX)
		{
			this.state.acceleration.x = 0;
		}
		if (stopY)
		{
			this.state.acceleration.y = 0;
		}
	}

	public void ResetDelayedMovement()
	{
		this.state.pivotJump = false;
		this.state.targetMoveVelocity = Vector2F.zero;
		this.state.targetMoveVelocityHorizontalFrames = 0;
		this.state.targetMoveVelocityVerticalFrames = 0;
	}

	public void AddGroundedHorizontalVelocity(Fixed velocity)
	{
		if (!this.IsGrounded || !(this.state.groundedNormal != Vector3F.zero))
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Grounded state doesn't match normal- grounded : ",
				this.IsGrounded,
				" normal : ",
				this.GroundedNormal
			}));
		}
		Vector3F vector3F = new Vector3F(this.state.groundedNormal.y, -this.state.groundedNormal.x, 0) * velocity;
		this.state.AddVelocity(ref vector3F, VelocityType.Movement);
	}

	public void ReverseHorizontalMovement()
	{
		this.state.targetMoveVelocity = this.state.GetReverseHorizontalVelocity(VelocityType.Movement);
		Vector3F movementVelocity = this.state.movementVelocity;
		if (base.config.moveData.bReverseAccelerationFrames > 0)
		{
			this.state.targetMoveVelocityHorizontalFrames = base.config.moveData.bReverseAccelerationFrames;
			this.tickAirAccelerationHorizontal(ref movementVelocity);
		}
		this.state.SetVelocity(movementVelocity, VelocityType.Movement);
	}

	public void AddVelocity(Vector2F push, int mirror, VelocityType velocityType)
	{
		push.x *= mirror;
		Vector3F vector3F = push;
		this.state.AddVelocity(ref vector3F, velocityType);
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Physics))
		{
			DebugDraw.Instance.CreateArrow(this.state.position, push.normalized, (float)push.magnitude / 20f, Color.red, 30);
		}
		if (push.y > 0)
		{
			this.context.model.IsGrounded = false;
			this.player.State.MetaState = MetaState.Jump;
		}
	}

	public void SetOverride(PhysicsOverride input)
	{
		this.state.physicsOverride = input;
	}

	public void TickFrame(int iterations)
	{
		this.state.BeginPhysicsUpdate();
		this.OnCollisionBoundsChanged(false);
		this.Simulator.AdvanceState(this.context, iterations);
		this.state.EndPhysicsUpdate(base.transform);
		if (!this.player.State.ShouldIgnoreForces)
		{
			this.state.acceleration = Vector3F.zero;
		}
		if (this.playerState.teeteringDirection != HorizontalDirection.None && (this.state.position - this.playerState.teeteringPosition).sqrMagnitude > (Fixed)0.01)
		{
			this.playerState.teeteringDirection = HorizontalDirection.None;
			if (this.player.State.ActionState == ActionState.TeeterLoop)
			{
				this.player.State.ActionState = ActionState.None;
			}
		}
		if (!this.player.State.IsHitLagPaused)
		{
			if (!this.IsGrounded)
			{
				this.state.framesSpentAirborne += iterations;
			}
			else
			{
				this.state.framesSpentAirborne = 0;
			}
			if (this.state.platformFallPreventFastfall > 0)
			{
				this.state.platformFallPreventFastfall--;
			}
			if (this.context.playerPhysicsModel.gravityAssistFrames > 0)
			{
				this.context.playerPhysicsModel.gravityAssistFrames--;
			}
			if (this.context.playerPhysicsModel.hitOverrideGravityFrames > 0)
			{
				this.context.playerPhysicsModel.hitOverrideGravityFrames--;
			}
			if (this.context.playerPhysicsModel.platformDropFrames > 0)
			{
				this.context.playerPhysicsModel.platformDropFrames--;
			}
		}
		if (this.state.targetMoveVelocityHorizontalFrames > 0)
		{
			this.state.targetMoveVelocityHorizontalFrames--;
			Vector3F movementVelocity = this.state.movementVelocity;
			if (this.state.targetMoveVelocityHorizontalFrames == 0)
			{
				movementVelocity.x = this.state.targetMoveVelocity.x;
			}
			else
			{
				this.tickAirAccelerationHorizontal(ref movementVelocity);
			}
			this.state.SetVelocity(movementVelocity, VelocityType.Movement);
		}
		if (this.state.targetMoveVelocityVerticalFrames > 0)
		{
			this.state.targetMoveVelocityVerticalFrames--;
			Vector3F movementVelocity2 = this.state.movementVelocity;
			if (this.state.targetMoveVelocityVerticalFrames == 0)
			{
				movementVelocity2.y = this.state.targetMoveVelocity.y;
			}
			else
			{
				this.tickJumpAccelerationVertical(ref movementVelocity2);
			}
			this.state.SetVelocity(movementVelocity2, VelocityType.Movement);
		}
	}

	public void OnCollisionBoundsChanged(bool updateImmediately)
	{
		this.state.bounds.dirty = true;
		if (updateImmediately)
		{
			this.ProcessBoundsIfDirty();
		}
	}

	private bool updateEnvironmentBounds()
	{
		Fixed x = this.Bounds.dimensions.x;
		Fixed y = this.Bounds.up.y;
		Fixed y2 = this.Bounds.down.y;
		this.Bounds.lastUp = this.Bounds.up;
		this.Bounds.lastRight = this.Bounds.right;
		this.Bounds.lastLeft = this.Bounds.left;
		this.Bounds.lastDown = this.Bounds.down;
		this.Bounds.lastCenterOffset = this.Bounds.centerOffset;
		Fixed @fixed = Fixed.MaxValue;
		Fixed fixed2 = -Fixed.MaxValue;
		Fixed fixed3 = -Fixed.MaxValue;
		Fixed fixed4 = Fixed.MaxValue;
		ECBOverrideData ecbData = this.getEcbData();
		this.Bounds.d_leftCalf = Vector3F.zero;
		this.Bounds.d_rightCalf = Vector3F.zero;
		this.Bounds.d_leftUpperArm = Vector3F.zero;
		this.Bounds.d_rightUpperArm = Vector3F.zero;
		this.Bounds.d_animationName = this.player.Body.AnimationName;
		this.Bounds.d_animationFrame = this.player.Body.AnimationFrame;
		for (int i = 0; i < ecbData.boneList.Length; i++)
		{
			BodyPart bodyPart = ecbData.boneList[i];
			Vector3F bonePosition = this.player.Body.GetBonePosition(bodyPart, false);
			@fixed = FixedMath.Min(@fixed, bonePosition.x);
			fixed2 = FixedMath.Max(fixed2, bonePosition.x);
			fixed4 = FixedMath.Min(fixed4, bonePosition.y);
			fixed3 = FixedMath.Max(fixed3, bonePosition.y);
			switch (i)
			{
			case 0:
				this.Bounds.d_leftUpperArm = bonePosition;
				break;
			case 1:
				this.Bounds.d_rightUpperArm = bonePosition;
				break;
			case 2:
				this.Bounds.d_leftCalf = bonePosition;
				break;
			case 3:
				this.Bounds.d_rightCalf = bonePosition;
				break;
			}
		}
		this.Bounds.d_rotated = this.player.IsRotationRolled;
		if (ecbData.addHeadToVerticalOnly)
		{
			Vector3F bonePosition2 = this.player.Body.GetBonePosition(BodyPart.head, false);
			fixed4 = FixedMath.Min(fixed4, bonePosition2.y);
			fixed3 = FixedMath.Max(fixed3, bonePosition2.y);
		}
		if (this.player is PlayerController && !this.state.IsGrounded)
		{
			fixed4 -= (this.player as PlayerController).CharacterData.airECBExtend;
		}
		bool flag = false;
		if (!this.IsGrounded && this.player.GrabController.GrabbedOpponent != PlayerNum.None)
		{
			PlayerController playerController = base.gameManager.GetPlayerController(this.player.GrabController.GrabbedOpponent);
			flag = true;
			@fixed = FixedMath.Min(this.State.position.x, FixedMath.Min(@fixed, (playerController.Center + playerController.Bounds.left).x));
			fixed2 = FixedMath.Max(this.State.position.x, FixedMath.Max(fixed2, (playerController.Center + playerController.Bounds.right).x));
			fixed4 = FixedMath.Min(fixed4, (playerController.Center + playerController.Bounds.down).y);
			fixed3 = FixedMath.Max(fixed3, (playerController.Center + playerController.Bounds.up).y);
		}
		Fixed fixed5 = FixedMath.Max(PlayerPhysicsController.MIN_COLLISION_DIAMOND_WIDTH, fixed2 - @fixed);
		bool flag2 = true;
		if (this.player.State.IsGrabbedState || this.player.State.IsLedgeGrabbing || this.player.State.IsLedgeHangingState)
		{
			flag2 = false;
		}
		else if (!this.IsGrounded)
		{
			if (this.player.State.IsStunned)
			{
				flag2 = false;
			}
			else if (this.state.framesSpentAirborne >= base.config.lagConfig.collisionDiamondAirDelayFrames && !this.player.State.IsLedgeRecovering)
			{
				flag2 = false;
			}
		}
		Fixed one = (!flag2) ? fixed4 : this.state.position.y;
		Fixed fixed6 = (fixed4 + fixed3) * 0.5f;
		Fixed fixed7 = fixed3 - fixed6;
		Fixed fixed8 = one - fixed6;
		Fixed fixed9 = fixed6 - this.state.position.y;
		if (fixed7 - fixed8 < PlayerPhysicsController.MIN_COLLISION_DIAMOND_HEIGHT)
		{
			if (flag2)
			{
				Fixed fixed10 = one + PlayerPhysicsController.MIN_COLLISION_DIAMOND_HEIGHT;
				fixed6 = (one + fixed10) * 0.5f;
				fixed9 = fixed6 - this.state.position.y;
				fixed7 = fixed10 - fixed6;
				fixed8 = one - fixed6;
			}
			else
			{
				fixed7 = PlayerPhysicsController.MIN_COLLISION_DIAMOND_HEIGHT * 0.5f;
				fixed8 = -PlayerPhysicsController.MIN_COLLISION_DIAMOND_HEIGHT * 0.5f;
			}
		}
		fixed6 = this.state.position.y + fixed9;
		this.Bounds.centerOffset = new Vector3F(0, fixed9, 0);
		this.Bounds.dimensions = new Vector2F(fixed5, fixed7 - fixed8);
		this.Bounds.down.Set(0, fixed8, 0);
		this.Bounds.up.Set(0, fixed7, 0);
		if (flag)
		{
			this.Bounds.left.Set(@fixed - this.state.position.x, 0, 0);
			this.Bounds.right.Set(fixed2 - this.state.position.x, 0, 0);
		}
		else
		{
			this.Bounds.left.Set(-fixed5 / 2, 0, 0);
			this.Bounds.right.Set(fixed5 / 2, 0, 0);
		}
		bool flag3 = fixed5 != x || this.Bounds.up.y != y || this.Bounds.down.y != y2;
		return flag3 && !this.player.IgnorePhysicsCollisions();
	}

	private ECBOverrideData getEcbData()
	{
		if (this.player.State.IsMoveActive && this.player.CurrentMove != null && this.player.CurrentMove.ecbOverrides.Length != 0)
		{
			int internalFrame = this.player.ActiveMove.Model.internalFrame;
			ECBOverrideData[] ecbOverrides = this.player.CurrentMove.ecbOverrides;
			for (int i = 0; i < ecbOverrides.Length; i++)
			{
				ECBOverrideData eCBOverrideData = ecbOverrides[i];
				if (internalFrame >= eCBOverrideData.startFrame && internalFrame <= eCBOverrideData.endFrame)
				{
					return eCBOverrideData;
				}
			}
		}
		return this.defaultECBData;
	}

	public void ProcessBoundsIfDirty()
	{
		int num = 3;
		int num2 = 0;
		while (this.state.bounds.dirty && num2 < num)
		{
			num2++;
			this.state.bounds.dirty = false;
			Vector2F v = this.state.bounds.down + this.state.center;
			PhysicsUtil.UpdateContextCollider(this.context);
			this.previousBoundsBuffer.LoadData(this.context.collider.Edge);
			EdgeData previousBoundsEdge = this.previousBoundsBuffer;
			bool flag = this.updateEnvironmentBounds();
			if (flag)
			{
				if (this.IsGrounded && (!this.player.State.IsStunned || !(this.state.totalVelocity.y > 0)))
				{
					this.SetPosition(v);
				}
				else
				{
					Vector3F a = this.state.bounds.lastDown + this.state.bounds.lastCenterOffset;
					Vector3F b = this.state.bounds.down + this.state.bounds.centerOffset;
					Vector3F position = this.state.position;
					Vector3F b2 = a - b;
					b2.x = 0;
					b2.y = FixedMath.Max(b2.y, 0);
					this.SetPosition(this.state.position + b2);
					PhysicsUtil.UpdateContextCollider(this.context);
					bool checkIfGrounded = this.context.shouldCheckGrounded == null || this.context.shouldCheckGrounded();
					if (!this.Simulator.OnCollisionBoundsChanged(this.context, previousBoundsEdge, checkIfGrounded))
					{
						this.ForceTranslate(position - this.state.position, true, true);
					}
					else
					{
						this.SetPosition(this.state.position);
					}
				}
			}
			else if (!this.IsGrounded && !this.context.ignoreCollisionsCallback())
			{
				Vector3F position2 = this.state.position;
				bool checkIfGrounded2 = this.context.shouldCheckGrounded == null || this.context.shouldCheckGrounded();
				if (!this.Simulator.OnCollisionBoundsChanged(this.context, previousBoundsEdge, checkIfGrounded2))
				{
					this.ForceTranslate(position2 - this.state.position, true, true);
				}
				else
				{
					this.SetPosition(this.state.position);
				}
			}
		}
		if (num2 == num)
		{
			UnityEngine.Debug.LogWarning("Reached maxIterations when processing dirty bounds for player in state " + this.player.State.ActionState);
		}
	}

	public void LoadPhysicsData(CharacterPhysicsData physicsData)
	{
		this.context.defaultCharacterData = physicsData;
	}

	public Vector3F GetPosition()
	{
		return this.state.position;
	}

	public void SetPosition(Vector3F position)
	{
		position.z = 0;
		base.transform.position = (Vector3)position;
		this.state.position = position;
	}

	public bool SetInitialPosition(Vector3F position)
	{
		RaycastHitData[] array = new RaycastHitData[1];
		int num = base.gameManager.PhysicsWorld.RaycastTerrain(position, Vector2F.down, 1000, PhysicsSimulator.GroundAndPlatformMask, array, RaycastFlags.Default, default(Fixed));
		bool flag = num > 0;
		if (flag)
		{
			this.SetPosition(array[0].point);
			this.ForceTranslate(Vector3F.down, true, true);
		}
		else
		{
			this.SetPosition(position);
		}
		return flag;
	}

	public void ForceTranslate(Vector3F delta, bool checkFeet, bool detectCliffs)
	{
		PhysicsMotionContext motionContext = this.context.motionContext;
		motionContext.initialVelocity = this.context.model.totalVelocity;
		motionContext.initialMovementVelocity = this.context.model.movementVelocity;
		motionContext.initialKnockbackVelocity = this.context.model.knockbackVelocity;
		motionContext.travelDelta = delta;
		motionContext.maxTravelDist = motionContext.travelDelta.magnitude;
		motionContext.distanceTraveled = 0;
		this.collisionsBuffer.Clear();
		this.state.BeginPhysicsUpdate();
		Vector3F travelDelta = motionContext.travelDelta;
		this.Simulator.Translate(this.context, this.collisionsBuffer, detectCliffs);
		if (this.context.model.RestoreVelocity == RestoreVelocityType.Restore)
		{
			this.context.model.SetVelocity(motionContext.initialMovementVelocity, VelocityType.Movement);
			this.context.model.SetVelocity(motionContext.initialKnockbackVelocity, VelocityType.Knockback);
		}
		if (checkFeet)
		{
			this.Simulator.CheckIfGrounded(this.context, this.collisionsBuffer, travelDelta);
		}
		this.state.EndPhysicsUpdate(base.transform);
	}

	public void SyncGroundState()
	{
		Vector3F zero = Vector3F.zero;
		this.collisionsBuffer.Clear();
		this.Simulator.CheckIfGrounded(this.context, this.collisionsBuffer, zero);
	}

	public void OnGrabLedge()
	{
		this.StopMovement(true, true, VelocityType.Total);
		this.ResetAirJump();
		this.playerState.isFastFalling = false;
	}

	public void ResetAirJump()
	{
		this.playerState.usedAirJump = false;
	}

	public void ResetGroundJump()
	{
		this.playerState.usedGroundJump = false;
	}

	public void ResetAllJumps()
	{
		this.ResetAirJump();
		this.ResetGroundJump();
	}

	public void ResetStateOnDeath()
	{
		this.StopMovement(true, true, VelocityType.Total);
		this.ResetAllJumps();
		this.context.playerPhysicsModel.isFastFalling = false;
		this.context.model.isGrounded = true;
	}

	public bool PerformBoundCast(AbsoluteDirection boundPoint, Vector3F originOffset, Fixed castDist, Vector2F castDirection, int castMask, out RaycastHitData hit)
	{
		Vector3F a = this.getBoundPointFromAbsoluteDirection(boundPoint);
		return PhysicsRaycastCalculator.GetFirstRaycastHit(this.context, a + originOffset, castDirection, castDist + PlayerPhysicsController.BOUNDS_CAST_TOLERANCE, castMask, out hit, default(Fixed));
	}

	public bool PerformBoundCast(RelativeDirection boundPoint, Vector3F originOffset, Fixed castDist, Vector2F castDirection, int castMask, out RaycastHitData hit)
	{
		return this.PerformBoundCast(this.getAbsoluteFromRelativeDirection(boundPoint), originOffset, castDist, castDirection, castMask, out hit);
	}

	public bool PerformBoundCast(RelativeDirection boundPoint, Vector3F originOffset, Fixed castDist, int castMask, out RaycastHitData hit)
	{
		AbsoluteDirection absoluteFromRelativeDirection = this.getAbsoluteFromRelativeDirection(boundPoint);
		Vector2F castDirectionFromAbsoluteDirection = this.getCastDirectionFromAbsoluteDirection(absoluteFromRelativeDirection);
		return this.PerformBoundCast(absoluteFromRelativeDirection, originOffset, castDist + PlayerPhysicsController.BOUNDS_CAST_TOLERANCE, castDirectionFromAbsoluteDirection, castMask, out hit);
	}

	private AbsoluteDirection getAbsoluteFromRelativeDirection(RelativeDirection direction)
	{
		switch (direction)
		{
		case RelativeDirection.Up:
			return AbsoluteDirection.Up;
		case RelativeDirection.Down:
			return AbsoluteDirection.Down;
		case RelativeDirection.Backward:
			return (this.player.Facing != HorizontalDirection.Left) ? AbsoluteDirection.Left : AbsoluteDirection.Right;
		}
		return (this.player.Facing != HorizontalDirection.Right) ? AbsoluteDirection.Left : AbsoluteDirection.Right;
	}

	private Vector2F getBoundPointFromAbsoluteDirection(AbsoluteDirection direction)
	{
		switch (direction)
		{
		case AbsoluteDirection.Up:
			return this.state.bounds.up + this.Center + PlayerPhysicsController.BOUNDS_CAST_UP_OFFSET;
		case AbsoluteDirection.Down:
			return this.state.bounds.down + this.Center + PlayerPhysicsController.BOUNDS_CAST_DOWN_OFFSET;
		case AbsoluteDirection.Left:
			return this.state.bounds.left + this.Center + PlayerPhysicsController.BOUNDS_CAST_LEFT_OFFSET;
		case AbsoluteDirection.Right:
			return this.state.bounds.right + this.Center + PlayerPhysicsController.BOUNDS_CAST_RIGHT_OFFSET;
		default:
			return this.Center;
		}
	}

	private Vector2F getCastDirectionFromAbsoluteDirection(AbsoluteDirection direction)
	{
		switch (direction)
		{
		case AbsoluteDirection.Up:
			return Vector2F.up;
		case AbsoluteDirection.Down:
			return Vector2F.down;
		case AbsoluteDirection.Left:
			return Vector2F.left;
		case AbsoluteDirection.Right:
			return Vector2F.right;
		default:
			return Vector2F.zero;
		}
	}

	private bool shouldCheckGroundCollision()
	{
		if (this.PreventGroundedness)
		{
			return false;
		}
		if (this.player.State.IsGrounded)
		{
			return true;
		}
		if (this.player.State.IsHitLagPaused)
		{
			return false;
		}
		if (this.player.State.IsMoveActive && this.player.CurrentMove != null)
		{
			MoveLabel label = this.player.CurrentMove.label;
			return label != MoveLabel.LedgeJump;
		}
		return true;
	}

	private bool cliffProtectionCallback()
	{
		if (!this.IsGrounded)
		{
			return false;
		}
		if (Vector3F.Dot(this.Velocity.normalized, this.GroundedNormal) >= (Fixed)0.05)
		{
			return false;
		}
		if (this.player.CurrentMove != null)
		{
			return this.player.CurrentMove.enableCliffProtection;
		}
		return this.player.State.IsDazed || this.player.Combat.IsMeteorStunned || (!this.player.State.IsStunned && !this.player.State.IsGrabbedState && !this.player.State.IsDashing && !this.player.State.IsRunning && !this.player.State.IsLanding && !this.player.State.IsRespawning && !this.player.State.IsDownState && ((!this.player.State.IsTeetering && !this.player.State.IsWalking) || !this.player.IsDirectionHeld(this.player.Facing) || !(FixedMath.Abs(this.player.GetDirectionHeldAmount) >= (Fixed)((double)base.config.inputConfig.walkOptions.walkOffEdgeThreshold))) && (this.player.Facing == ((!(this.Velocity.x > 0)) ? HorizontalDirection.Left : HorizontalDirection.Right) || this.Velocity.x == 0));
	}

	private void onCliffProtection(Vector3F delta, Vector3F position)
	{
		this.playerState.teeteringDirection = ((!(delta.x > 0)) ? HorizontalDirection.Left : HorizontalDirection.Right);
		this.playerState.teeteringPosition = position;
	}

	public bool CheckIsOnPlatform(out IPhysicsCollider collider)
	{
		collider = null;
		if (!this.IsGrounded)
		{
			return false;
		}
		RaycastHitData raycastHitData;
		if (this.PerformBoundCast(RelativeDirection.Down, this.state.movingPlatformDeltaPosition, PhysicsSimulator.GROUNDED_RAYCAST_TOLERACE, PhysicsSimulator.PlatformMask, out raycastHitData))
		{
			collider = raycastHitData.collider;
			return true;
		}
		return false;
	}

	public bool IsStandingOnStageSurface(out RaycastHitData surfaceHit)
	{
		Vector3F originOffset = this.state.movingPlatformDeltaPosition + Vector3F.up * PhysicsSimulator.GROUNDED_RAYCAST_TOLERACE;
		RaycastHitData raycastHitData;
		if (this.IsGrounded && this.PerformBoundCast(RelativeDirection.Down, originOffset, PhysicsSimulator.GROUNDED_RAYCAST_TOLERACE * 2, PhysicsSimulator.GroundAndPlatformMask, out raycastHitData))
		{
			surfaceHit = raycastHitData;
			return true;
		}
		surfaceHit = RaycastHitData.Empty;
		return false;
	}

	private void OnDrawGizmos()
	{
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Physics))
		{
			Vector3 b = (Vector3)this.state.totalVelocity * WTime.frameTime * 5f;
			Vector3 position = base.transform.position;
			Vector3 end = base.transform.position + b;
			Color color = Color.white;
			if (this.context.model.RestoreVelocity == RestoreVelocityType.Restore)
			{
				color = Color.green;
			}
			GizmoUtil.GizmosDrawArrow(position, end, color, false, 0f, 33f);
		}
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Bounds))
		{
			Vector3 b2 = (Vector3)this.state.lastCenter;
			GizmoUtil.GizmosDrawQuadrilateral((Vector3)this.Bounds.lastUp + b2, (Vector3)this.Bounds.lastRight + b2, (Vector3)this.Bounds.lastDown + b2, (Vector3)this.Bounds.lastLeft + b2, Color.blue);
			GizmoUtil.GizmosDrawQuadrilateral((Vector3)this.Bounds.up + (Vector3)this.Center, (Vector3)this.Bounds.right + (Vector3)this.Center, (Vector3)this.Bounds.down + (Vector3)this.Center, (Vector3)this.Bounds.left + (Vector3)this.Center, Color.red);
			if (this.physicsOverride != null)
			{
				GizmoUtil.GizmosDrawQuadrilateral((Vector3)this.Bounds.down + (Vector3)this.Center + Vector3.left * 0.01f + Vector3.up * (float)this.physicsOverride.groundCheckUp, (Vector3)this.Bounds.down + (Vector3)this.Center + Vector3.right * 0.01f + Vector3.up * (float)this.physicsOverride.groundCheckUp, (Vector3)this.Bounds.down + (Vector3)this.Center + Vector3.right * 0.01f + Vector3.down * (float)this.physicsOverride.groundCheckDown, (Vector3)this.Bounds.down + (Vector3)this.Center + Vector3.left * 0.01f + Vector3.down * (float)this.physicsOverride.groundCheckDown, Color.red);
			}
			GizmoUtil.GizmosDrawLine((Vector3)this.Bounds.lastUp + b2, (Vector3)this.Bounds.up + (Vector3)this.Center, new Color(0f, 1f, 0.5f));
			GizmoUtil.GizmosDrawLine((Vector3)this.Bounds.lastRight + b2, (Vector3)this.Bounds.right + (Vector3)this.Center, new Color(0f, 1f, 0.5f));
			GizmoUtil.GizmosDrawLine((Vector3)this.Bounds.lastDown + b2, (Vector3)this.Bounds.down + (Vector3)this.Center, new Color(0f, 1f, 0.5f));
			GizmoUtil.GizmosDrawLine((Vector3)this.Bounds.lastLeft + b2, (Vector3)this.Bounds.left + (Vector3)this.Center, new Color(0f, 1f, 0.5f));
			GizmoUtil.GizmosDrawCircle((Vector2)this.state.position, 0.1f, Color.yellow, 10);
			EdgeData edge = this.context.collider.Edge;
			FixedRect rect = PhysicsUtil.ExtendBoundingBox(edge.BoundingBox, this.state.totalVelocity * WTime.fixedDeltaTime);
			GizmoUtil.GizmosDrawRectangle(rect, Color.cyan, false);
		}
	}
}
