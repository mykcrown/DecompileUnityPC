// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

public class PhysicsContext
{
	[AllowCachedState]
	public PhysicsModel model;

	public KnockbackConfig knockbackConfig;

	public LagConfig lagConfig;

	[AllowCachedState]
	public PlayerPhysicsModel playerPhysicsModel;

	public ICharacterPhysicsData characterData;

	public CharacterPhysicsData defaultCharacterData;

	public IPhysicsDelegate playerDelegate;

	public ArticleData articleData;

	public ArticleController articleController;

	public IPhysicsStateOwner physicsState;

	public IPhysicsImpactHandler impactHandler;

	public IPhysicsCollisionMotion collisionMotion;

	public IPhysicsColliderOwner colliderOwner;

	public PhysicsWorld world;

	public PhysicsMotionContext motionContext = new PhysicsMotionContext();

	public PerformTechCallback performTechCallback;

	public AvailableTechCallback availableTechCallback;

	public FallThroughPlatformsCallback fallThroughPlatformsCallback;

	public Func<IPhysicsCollider, bool> isPlatformLastDropped;

	public PlatformDroppingCallback isFallingThroughPlatformCallback;

	public ShouldBounceCallback shouldBounceCallback;

	public CalculateMaxSpeedCallback calculateMaxHorizontalSpeedCallback;

	public CalculateMaxSpeedCallback calculateMaxVerticalSpeedCallback;

	public LandCallback landCallback;

	public BeginFallCallback beginFallCallback;

	public FallCallback fallCallback;

	public CliffProtectionCallback cliffProtectionCallback;

	public Action<Vector3F, Vector3F> onCliffProtection;

	public IgnoreCollisionsCallback ignoreCollisionsCallback;

	public CheckGroundedCallback shouldCheckGrounded;

	public HaltOnCollisionCallback shouldHaltOnCollision;

	public OnKnockbackBounceCallback onKnockbackBounceCallback;

	public MaintainVelocityCallback shouldMaintainVelocityCallback;

	public ShouldIgnoreForcesCallback shouldIgnoreForcesCallback;

	public int debugPredictionFrame;

	public Vector3F[] debugPrediction;

	public IPhysicsCollider collider
	{
		get
		{
			return (this.colliderOwner != null) ? this.colliderOwner.Collider : null;
		}
	}

	public bool isFallThroughPlatformsInput
	{
		get
		{
			return this.fallThroughPlatformsCallback != null && this.fallThroughPlatformsCallback();
		}
	}

	public bool ignorePlatforms
	{
		get
		{
			return (this.characterData != null && this.characterData.IgnorePlatforms) || (this.physicsState.PhysicsOverride != null && this.physicsState.PhysicsOverride.ignorePlatforms);
		}
	}

	public void LoadDebugPrediction(List<Vector3F> list)
	{
		this.debugPredictionFrame = 0;
		this.debugPrediction = list.ToArray();
	}

	public bool IsFallingThroughPlatform(IPhysicsCollider platformCollider)
	{
		return this.isFallingThroughPlatformCallback != null && this.isFallingThroughPlatformCallback(platformCollider);
	}
}
