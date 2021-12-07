using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x0200055E RID: 1374
public class PhysicsContext
{
	// Token: 0x1700065E RID: 1630
	// (get) Token: 0x06001E10 RID: 7696 RVA: 0x00098622 File Offset: 0x00096A22
	public IPhysicsCollider collider
	{
		get
		{
			return (this.colliderOwner != null) ? this.colliderOwner.Collider : null;
		}
	}

	// Token: 0x06001E11 RID: 7697 RVA: 0x00098640 File Offset: 0x00096A40
	public void LoadDebugPrediction(List<Vector3F> list)
	{
		this.debugPredictionFrame = 0;
		this.debugPrediction = list.ToArray();
	}

	// Token: 0x1700065F RID: 1631
	// (get) Token: 0x06001E12 RID: 7698 RVA: 0x00098655 File Offset: 0x00096A55
	public bool isFallThroughPlatformsInput
	{
		get
		{
			return this.fallThroughPlatformsCallback != null && this.fallThroughPlatformsCallback();
		}
	}

	// Token: 0x17000660 RID: 1632
	// (get) Token: 0x06001E13 RID: 7699 RVA: 0x00098670 File Offset: 0x00096A70
	public bool ignorePlatforms
	{
		get
		{
			return (this.characterData != null && this.characterData.IgnorePlatforms) || (this.physicsState.PhysicsOverride != null && this.physicsState.PhysicsOverride.ignorePlatforms);
		}
	}

	// Token: 0x06001E14 RID: 7700 RVA: 0x000986BE File Offset: 0x00096ABE
	public bool IsFallingThroughPlatform(IPhysicsCollider platformCollider)
	{
		return this.isFallingThroughPlatformCallback != null && this.isFallingThroughPlatformCallback(platformCollider);
	}

	// Token: 0x04001827 RID: 6183
	[AllowCachedState]
	public PhysicsModel model;

	// Token: 0x04001828 RID: 6184
	public KnockbackConfig knockbackConfig;

	// Token: 0x04001829 RID: 6185
	public LagConfig lagConfig;

	// Token: 0x0400182A RID: 6186
	[AllowCachedState]
	public PlayerPhysicsModel playerPhysicsModel;

	// Token: 0x0400182B RID: 6187
	public ICharacterPhysicsData characterData;

	// Token: 0x0400182C RID: 6188
	public CharacterPhysicsData defaultCharacterData;

	// Token: 0x0400182D RID: 6189
	public IPhysicsDelegate playerDelegate;

	// Token: 0x0400182E RID: 6190
	public ArticleData articleData;

	// Token: 0x0400182F RID: 6191
	public ArticleController articleController;

	// Token: 0x04001830 RID: 6192
	public IPhysicsStateOwner physicsState;

	// Token: 0x04001831 RID: 6193
	public IPhysicsImpactHandler impactHandler;

	// Token: 0x04001832 RID: 6194
	public IPhysicsCollisionMotion collisionMotion;

	// Token: 0x04001833 RID: 6195
	public IPhysicsColliderOwner colliderOwner;

	// Token: 0x04001834 RID: 6196
	public PhysicsWorld world;

	// Token: 0x04001835 RID: 6197
	public PhysicsMotionContext motionContext = new PhysicsMotionContext();

	// Token: 0x04001836 RID: 6198
	public PerformTechCallback performTechCallback;

	// Token: 0x04001837 RID: 6199
	public AvailableTechCallback availableTechCallback;

	// Token: 0x04001838 RID: 6200
	public FallThroughPlatformsCallback fallThroughPlatformsCallback;

	// Token: 0x04001839 RID: 6201
	public Func<IPhysicsCollider, bool> isPlatformLastDropped;

	// Token: 0x0400183A RID: 6202
	public PlatformDroppingCallback isFallingThroughPlatformCallback;

	// Token: 0x0400183B RID: 6203
	public ShouldBounceCallback shouldBounceCallback;

	// Token: 0x0400183C RID: 6204
	public CalculateMaxSpeedCallback calculateMaxHorizontalSpeedCallback;

	// Token: 0x0400183D RID: 6205
	public CalculateMaxSpeedCallback calculateMaxVerticalSpeedCallback;

	// Token: 0x0400183E RID: 6206
	public LandCallback landCallback;

	// Token: 0x0400183F RID: 6207
	public BeginFallCallback beginFallCallback;

	// Token: 0x04001840 RID: 6208
	public FallCallback fallCallback;

	// Token: 0x04001841 RID: 6209
	public CliffProtectionCallback cliffProtectionCallback;

	// Token: 0x04001842 RID: 6210
	public Action<Vector3F, Vector3F> onCliffProtection;

	// Token: 0x04001843 RID: 6211
	public IgnoreCollisionsCallback ignoreCollisionsCallback;

	// Token: 0x04001844 RID: 6212
	public CheckGroundedCallback shouldCheckGrounded;

	// Token: 0x04001845 RID: 6213
	public HaltOnCollisionCallback shouldHaltOnCollision;

	// Token: 0x04001846 RID: 6214
	public OnKnockbackBounceCallback onKnockbackBounceCallback;

	// Token: 0x04001847 RID: 6215
	public MaintainVelocityCallback shouldMaintainVelocityCallback;

	// Token: 0x04001848 RID: 6216
	public ShouldIgnoreForcesCallback shouldIgnoreForcesCallback;

	// Token: 0x04001849 RID: 6217
	public int debugPredictionFrame;

	// Token: 0x0400184A RID: 6218
	public Vector3F[] debugPrediction;
}
