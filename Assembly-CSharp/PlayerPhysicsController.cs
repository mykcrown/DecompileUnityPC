using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x02000571 RID: 1393
public class PlayerPhysicsController : GameBehavior, IMovePhysics, IRollbackStateOwner, IPhysicsStateOwner, IPhysicsColliderOwner, ICharacterPhysicsData
{
	// Token: 0x17000683 RID: 1667
	// (get) Token: 0x06001E94 RID: 7828 RVA: 0x0009BD0A File Offset: 0x0009A10A
	// (set) Token: 0x06001E95 RID: 7829 RVA: 0x0009BD12 File Offset: 0x0009A112
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x17000684 RID: 1668
	// (get) Token: 0x06001E96 RID: 7830 RVA: 0x0009BD1B File Offset: 0x0009A11B
	// (set) Token: 0x06001E97 RID: 7831 RVA: 0x0009BD23 File Offset: 0x0009A123
	public PhysicsSimulator Simulator { get; private set; }

	// Token: 0x17000685 RID: 1669
	// (get) Token: 0x06001E98 RID: 7832 RVA: 0x0009BD2C File Offset: 0x0009A12C
	public Vector3F Velocity
	{
		get
		{
			return this.state.totalVelocity;
		}
	}

	// Token: 0x17000686 RID: 1670
	// (get) Token: 0x06001E99 RID: 7833 RVA: 0x0009BD39 File Offset: 0x0009A139
	public Vector3F KnockbackVelocity
	{
		get
		{
			return this.state.GetVelocity(VelocityType.Knockback);
		}
	}

	// Token: 0x17000687 RID: 1671
	// (get) Token: 0x06001E9A RID: 7834 RVA: 0x0009BD47 File Offset: 0x0009A147
	public Vector3F MovementVelocity
	{
		get
		{
			return this.state.GetVelocity(VelocityType.Movement);
		}
	}

	// Token: 0x17000688 RID: 1672
	// (get) Token: 0x06001E9B RID: 7835 RVA: 0x0009BD55 File Offset: 0x0009A155
	public Vector3F ForcedVelocity
	{
		get
		{
			return this.state.GetVelocity(VelocityType.Forced);
		}
	}

	// Token: 0x17000689 RID: 1673
	// (get) Token: 0x06001E9C RID: 7836 RVA: 0x0009BD63 File Offset: 0x0009A163
	public Vector3F Center
	{
		get
		{
			return this.state.center;
		}
	}

	// Token: 0x1700068A RID: 1674
	// (get) Token: 0x06001E9D RID: 7837 RVA: 0x0009BD70 File Offset: 0x0009A170
	public bool IsFastFalling
	{
		get
		{
			return this.playerState.isFastFalling;
		}
	}

	// Token: 0x1700068B RID: 1675
	// (get) Token: 0x06001E9E RID: 7838 RVA: 0x0009BD7D File Offset: 0x0009A17D
	public HorizontalDirection TeeteringDirection
	{
		get
		{
			return this.playerState.teeteringDirection;
		}
	}

	// Token: 0x1700068C RID: 1676
	// (get) Token: 0x06001E9F RID: 7839 RVA: 0x0009BD8A File Offset: 0x0009A18A
	public PhysicsModel State
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x1700068D RID: 1677
	// (get) Token: 0x06001EA0 RID: 7840 RVA: 0x0009BD92 File Offset: 0x0009A192
	public PlayerPhysicsModel PlayerState
	{
		get
		{
			return this.playerState;
		}
	}

	// Token: 0x1700068E RID: 1678
	// (get) Token: 0x06001EA1 RID: 7841 RVA: 0x0009BD9A File Offset: 0x0009A19A
	public bool IsGrounded
	{
		get
		{
			return this.state.isGrounded;
		}
	}

	// Token: 0x1700068F RID: 1679
	// (get) Token: 0x06001EA2 RID: 7842 RVA: 0x0009BDA7 File Offset: 0x0009A1A7
	public bool UsedAirJump
	{
		get
		{
			return this.playerState.usedAirJump;
		}
	}

	// Token: 0x17000690 RID: 1680
	// (get) Token: 0x06001EA3 RID: 7843 RVA: 0x0009BDB4 File Offset: 0x0009A1B4
	public bool UsedGroundJump
	{
		get
		{
			return this.playerState.usedGroundJump;
		}
	}

	// Token: 0x17000691 RID: 1681
	// (get) Token: 0x06001EA4 RID: 7844 RVA: 0x0009BDC1 File Offset: 0x0009A1C1
	// (set) Token: 0x06001EA5 RID: 7845 RVA: 0x0009BDCE File Offset: 0x0009A1CE
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

	// Token: 0x17000692 RID: 1682
	// (get) Token: 0x06001EA6 RID: 7846 RVA: 0x0009BDDC File Offset: 0x0009A1DC
	public Vector3F GroundedNormal
	{
		get
		{
			return this.state.groundedNormal;
		}
	}

	// Token: 0x17000693 RID: 1683
	// (get) Token: 0x06001EA7 RID: 7847 RVA: 0x0009BDE9 File Offset: 0x0009A1E9
	public bool IsAboveStage
	{
		get
		{
			return this.Simulator.CheckIfAboveStage(this.context);
		}
	}

	// Token: 0x17000694 RID: 1684
	// (get) Token: 0x06001EA8 RID: 7848 RVA: 0x0009BDFC File Offset: 0x0009A1FC
	public IMovingObject GroundedMovingObject
	{
		get
		{
			return this.state.groundedMovingStageObject;
		}
	}

	// Token: 0x17000695 RID: 1685
	// (get) Token: 0x06001EA9 RID: 7849 RVA: 0x0009BE09 File Offset: 0x0009A209
	public Fixed SteerMomentumMaxAnglePerFrame
	{
		get
		{
			return (this.physicsOverride == null) ? 0 : this.physicsOverride.steerMomentumMaxAnglePerFrame;
		}
	}

	// Token: 0x17000696 RID: 1686
	// (get) Token: 0x06001EAA RID: 7850 RVA: 0x0009BE2C File Offset: 0x0009A22C
	public Fixed SteerMomentumMinOverallAngle
	{
		get
		{
			return (this.physicsOverride == null) ? 0 : this.physicsOverride.steerMomentumMinOverallAngle;
		}
	}

	// Token: 0x17000697 RID: 1687
	// (get) Token: 0x06001EAB RID: 7851 RVA: 0x0009BE4F File Offset: 0x0009A24F
	public Fixed SteerMomentumMaxOverallAngle
	{
		get
		{
			return (this.physicsOverride == null) ? 0 : this.physicsOverride.steerMomentumMaxOverallAngle;
		}
	}

	// Token: 0x17000698 RID: 1688
	// (get) Token: 0x06001EAC RID: 7852 RVA: 0x0009BE72 File Offset: 0x0009A272
	public bool SteerMomentumFaceVelocity
	{
		get
		{
			return this.physicsOverride != null && this.physicsOverride.steerMomentumFaceVelocity;
		}
	}

	// Token: 0x17000699 RID: 1689
	// (get) Token: 0x06001EAD RID: 7853 RVA: 0x0009BE90 File Offset: 0x0009A290
	public bool PreventGroundedness
	{
		get
		{
			return this.physicsOverride != null && this.physicsOverride.preventGroundedness;
		}
	}

	// Token: 0x17000681 RID: 1665
	// (get) Token: 0x06001EAE RID: 7854 RVA: 0x0009BEAE File Offset: 0x0009A2AE
	// (set) Token: 0x06001EAF RID: 7855 RVA: 0x0009BEBB File Offset: 0x0009A2BB
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

	// Token: 0x17000682 RID: 1666
	// (get) Token: 0x06001EB0 RID: 7856 RVA: 0x0009BEC9 File Offset: 0x0009A2C9
	MoveData IPhysicsStateOwner.CurrentMove
	{
		get
		{
			return this.player.CurrentMove;
		}
	}

	// Token: 0x1700069A RID: 1690
	// (get) Token: 0x06001EB1 RID: 7857 RVA: 0x0009BED6 File Offset: 0x0009A2D6
	// (set) Token: 0x06001EB2 RID: 7858 RVA: 0x0009BEDE File Offset: 0x0009A2DE
	public IPhysicsCollider Collider { get; private set; }

	// Token: 0x1700069B RID: 1691
	// (get) Token: 0x06001EB3 RID: 7859 RVA: 0x0009BEE7 File Offset: 0x0009A2E7
	private CharacterPhysicsData data
	{
		get
		{
			return (this.context == null) ? null : this.context.defaultCharacterData;
		}
	}

	// Token: 0x1700069C RID: 1692
	// (get) Token: 0x06001EB4 RID: 7860 RVA: 0x0009BF05 File Offset: 0x0009A305
	private CharacterPhysicsOverride dataOverride
	{
		get
		{
			return this.state.characterPhysicsOverride;
		}
	}

	// Token: 0x1700069D RID: 1693
	// (get) Token: 0x06001EB5 RID: 7861 RVA: 0x0009BF12 File Offset: 0x0009A312
	private PhysicsOverride physicsOverride
	{
		get
		{
			return (this.state == null) ? null : this.state.physicsOverride;
		}
	}

	// Token: 0x1700069E RID: 1694
	// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x0009BF30 File Offset: 0x0009A330
	public PhysicsContext Context
	{
		get
		{
			return this.context;
		}
	}

	// Token: 0x1700069F RID: 1695
	// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x0009BF38 File Offset: 0x0009A338
	public EnvironmentBounds Bounds
	{
		get
		{
			return this.state.bounds;
		}
	}

	// Token: 0x170006A0 RID: 1696
	// (get) Token: 0x06001EB8 RID: 7864 RVA: 0x0009BF45 File Offset: 0x0009A345
	public Fixed SlowWalkMaxSpeed
	{
		get
		{
			return this.dataOverride.slowWalkMaxSpeed.GetValueOrDefault((Fixed)((double)this.data.slowWalkMaxSpeed));
		}
	}

	// Token: 0x170006A1 RID: 1697
	// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x0009BF68 File Offset: 0x0009A368
	public Fixed MediumWalkMaxSpeed
	{
		get
		{
			return this.dataOverride.mediumWalkMaxSpeed.GetValueOrDefault((Fixed)((double)this.data.mediumWalkMaxSpeed));
		}
	}

	// Token: 0x170006A2 RID: 1698
	// (get) Token: 0x06001EBA RID: 7866 RVA: 0x0009BF8B File Offset: 0x0009A38B
	public Fixed FastWalkMaxSpeed
	{
		get
		{
			return this.dataOverride.fastWalkMaxSpeed.GetValueOrDefault((Fixed)((double)this.data.fastWalkMaxSpeed));
		}
	}

	// Token: 0x170006A3 RID: 1699
	// (get) Token: 0x06001EBB RID: 7867 RVA: 0x0009BFAE File Offset: 0x0009A3AE
	public Fixed RunMaxSpeed
	{
		get
		{
			return this.dataOverride.runMaxSpeed.GetValueOrDefault((Fixed)((double)this.data.runMaxSpeed));
		}
	}

	// Token: 0x170006A4 RID: 1700
	// (get) Token: 0x06001EBC RID: 7868 RVA: 0x0009BFD1 File Offset: 0x0009A3D1
	public Fixed GroundToAirMaxSpeed
	{
		get
		{
			return this.dataOverride.groundToAirMaxSpeed.GetValueOrDefault((Fixed)((double)this.data.groundToAirMaxSpeed));
		}
	}

	// Token: 0x170006A5 RID: 1701
	// (get) Token: 0x06001EBD RID: 7869 RVA: 0x0009BFF4 File Offset: 0x0009A3F4
	public Fixed WalkAcceleration
	{
		get
		{
			return this.dataOverride.walkAcceleration.GetValueOrDefault((Fixed)((double)this.data.walkAcceleration));
		}
	}

	// Token: 0x170006A6 RID: 1702
	// (get) Token: 0x06001EBE RID: 7870 RVA: 0x0009C017 File Offset: 0x0009A417
	public Fixed RunPivotAcceleration
	{
		get
		{
			return this.dataOverride.runPivotAcceleration.GetValueOrDefault((Fixed)((double)this.data.runPivotAcceleration));
		}
	}

	// Token: 0x170006A7 RID: 1703
	// (get) Token: 0x06001EBF RID: 7871 RVA: 0x0009C03A File Offset: 0x0009A43A
	public Fixed Friction
	{
		get
		{
			return this.dataOverride.friction.GetValueOrDefault((Fixed)((double)this.data.friction));
		}
	}

	// Token: 0x170006A8 RID: 1704
	// (get) Token: 0x06001EC0 RID: 7872 RVA: 0x0009C05D File Offset: 0x0009A45D
	public Fixed HighSpeedFriction
	{
		get
		{
			return this.dataOverride.highSpeedFriction.GetValueOrDefault((Fixed)((double)this.data.highSpeedFriction));
		}
	}

	// Token: 0x170006A9 RID: 1705
	// (get) Token: 0x06001EC1 RID: 7873 RVA: 0x0009C080 File Offset: 0x0009A480
	public Fixed DashStartSpeed
	{
		get
		{
			return this.dataOverride.dashStartSpeed.GetValueOrDefault((Fixed)((double)this.data.dashStartSpeed));
		}
	}

	// Token: 0x170006AA RID: 1706
	// (get) Token: 0x06001EC2 RID: 7874 RVA: 0x0009C0A3 File Offset: 0x0009A4A3
	public Fixed DashMaxSpeed
	{
		get
		{
			return this.dataOverride.dashMaxSpeed.GetValueOrDefault((Fixed)((double)this.data.dashMaxSpeed));
		}
	}

	// Token: 0x170006AB RID: 1707
	// (get) Token: 0x06001EC3 RID: 7875 RVA: 0x0009C0C6 File Offset: 0x0009A4C6
	public Fixed DashAcceleration
	{
		get
		{
			return this.dataOverride.dashAcceleration.GetValueOrDefault((Fixed)((double)this.data.dashAcceleration));
		}
	}

	// Token: 0x170006AC RID: 1708
	// (get) Token: 0x06001EC4 RID: 7876 RVA: 0x0009C0E9 File Offset: 0x0009A4E9
	public Fixed AirMaxSpeed
	{
		get
		{
			return this.dataOverride.airMaxSpeed.GetValueOrDefault((Fixed)((double)this.data.airMaxSpeed));
		}
	}

	// Token: 0x170006AD RID: 1709
	// (get) Token: 0x06001EC5 RID: 7877 RVA: 0x0009C10C File Offset: 0x0009A50C
	public Fixed AirAcceleration
	{
		get
		{
			return this.dataOverride.airAcceleration.GetValueOrDefault((Fixed)((double)this.data.airAcceleration));
		}
	}

	// Token: 0x170006AE RID: 1710
	// (get) Token: 0x06001EC6 RID: 7878 RVA: 0x0009C12F File Offset: 0x0009A52F
	public Fixed AirFriction
	{
		get
		{
			return this.dataOverride.airFriction.GetValueOrDefault((Fixed)((double)this.data.airFriction));
		}
	}

	// Token: 0x170006AF RID: 1711
	// (get) Token: 0x06001EC7 RID: 7879 RVA: 0x0009C152 File Offset: 0x0009A552
	public Fixed Gravity
	{
		get
		{
			return this.dataOverride.gravity.GetValueOrDefault((Fixed)((double)this.data.gravity));
		}
	}

	// Token: 0x170006B0 RID: 1712
	// (get) Token: 0x06001EC8 RID: 7880 RVA: 0x0009C175 File Offset: 0x0009A575
	public Fixed MaxFallSpeed
	{
		get
		{
			return this.dataOverride.maxFallSpeed.GetValueOrDefault((Fixed)((double)this.data.maxFallSpeed));
		}
	}

	// Token: 0x170006B1 RID: 1713
	// (get) Token: 0x06001EC9 RID: 7881 RVA: 0x0009C198 File Offset: 0x0009A598
	public Fixed HelplessAirSpeedMultiplier
	{
		get
		{
			return this.dataOverride.helplessAirSpeedMultiplier.GetValueOrDefault((Fixed)((double)this.data.helplessAirSpeedMultiplier));
		}
	}

	// Token: 0x170006B2 RID: 1714
	// (get) Token: 0x06001ECA RID: 7882 RVA: 0x0009C1BB File Offset: 0x0009A5BB
	public Fixed HelplessAirAccelerationMultiplier
	{
		get
		{
			return this.dataOverride.helplessAirAccelerationMultiplier.GetValueOrDefault((Fixed)((double)this.data.helplessAirAccelerationMultiplier));
		}
	}

	// Token: 0x170006B3 RID: 1715
	// (get) Token: 0x06001ECB RID: 7883 RVA: 0x0009C1DE File Offset: 0x0009A5DE
	public Fixed JumpSpeed
	{
		get
		{
			return this.dataOverride.jumpSpeed.GetValueOrDefault((Fixed)((double)this.data.jumpSpeed));
		}
	}

	// Token: 0x170006B4 RID: 1716
	// (get) Token: 0x06001ECC RID: 7884 RVA: 0x0009C201 File Offset: 0x0009A601
	public Fixed SecondaryJumpSpeed
	{
		get
		{
			return this.dataOverride.secondaryJumpSpeed.GetValueOrDefault((Fixed)((double)this.data.secondaryJumpSpeed));
		}
	}

	// Token: 0x170006B5 RID: 1717
	// (get) Token: 0x06001ECD RID: 7885 RVA: 0x0009C224 File Offset: 0x0009A624
	public Fixed ShortJumpSpeed
	{
		get
		{
			return this.dataOverride.shortJumpSpeed.GetValueOrDefault((Fixed)((double)this.data.shortJumpSpeed));
		}
	}

	// Token: 0x170006B6 RID: 1718
	// (get) Token: 0x06001ECE RID: 7886 RVA: 0x0009C247 File Offset: 0x0009A647
	public int JumpCount
	{
		get
		{
			return this.dataOverride.jumpCount.GetValueOrDefault(this.data.jumpCount);
		}
	}

	// Token: 0x170006B7 RID: 1719
	// (get) Token: 0x06001ECF RID: 7887 RVA: 0x0009C264 File Offset: 0x0009A664
	public Fixed Weight
	{
		get
		{
			return this.dataOverride.weight.GetValueOrDefault((Fixed)((double)this.data.weight));
		}
	}

	// Token: 0x170006B8 RID: 1720
	// (get) Token: 0x06001ED0 RID: 7888 RVA: 0x0009C287 File Offset: 0x0009A687
	public Fixed FastFallSpeed
	{
		get
		{
			return this.dataOverride.fastFallSpeed.GetValueOrDefault((Fixed)((double)this.data.fastFallSpeed));
		}
	}

	// Token: 0x170006B9 RID: 1721
	// (get) Token: 0x06001ED1 RID: 7889 RVA: 0x0009C2AA File Offset: 0x0009A6AA
	public Fixed ShieldBreakSpeed
	{
		get
		{
			return this.dataOverride.shieldBreakSpeed.GetValueOrDefault((Fixed)((double)this.data.shieldBreakSpeed));
		}
	}

	// Token: 0x170006BA RID: 1722
	// (get) Token: 0x06001ED2 RID: 7890 RVA: 0x0009C2CD File Offset: 0x0009A6CD
	public bool IgnorePlatforms
	{
		get
		{
			return this.dataOverride.ignorePlatforms.GetValueOrDefault(this.data.ignorePlatforms);
		}
	}

	// Token: 0x06001ED3 RID: 7891 RVA: 0x0009C2EA File Offset: 0x0009A6EA
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<PhysicsModel>(this.state));
		container.WriteState(this.rollbackStatePooling.Clone<PlayerPhysicsModel>(this.playerState));
		return true;
	}

	// Token: 0x06001ED4 RID: 7892 RVA: 0x0009C320 File Offset: 0x0009A720
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<PhysicsModel>(ref this.state);
		container.ReadState<PlayerPhysicsModel>(ref this.playerState);
		base.transform.position = (Vector3)this.state.position;
		this.context.model = this.state;
		this.context.playerPhysicsModel = this.playerState;
		return true;
	}

	// Token: 0x06001ED5 RID: 7893 RVA: 0x0009C388 File Offset: 0x0009A788
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

	// Token: 0x06001ED6 RID: 7894 RVA: 0x0009C4C9 File Offset: 0x0009A8C9
	private void initEnvironmentBoundsBoneList()
	{
	}

	// Token: 0x06001ED7 RID: 7895 RVA: 0x0009C4CC File Offset: 0x0009A8CC
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

	// Token: 0x06001ED8 RID: 7896 RVA: 0x0009C6DF File Offset: 0x0009AADF
	private bool shouldIgnoreForces()
	{
		return this.player.State.ShouldIgnoreForces;
	}

	// Token: 0x06001ED9 RID: 7897 RVA: 0x0009C6F4 File Offset: 0x0009AAF4
	private bool shouldHaltOnCollision(PhysicsMotionContext motionContext, CollisionData collision)
	{
		return this.player.IsUnderContinuousForce && Vector3F.Dot(collision.normal, Vector3F.down) > 0 && Vector3F.Dot(motionContext.initialMovementVelocity, Vector3F.down) >= 0;
	}

	// Token: 0x06001EDA RID: 7898 RVA: 0x0009C74B File Offset: 0x0009AB4B
	private bool isFallingThroughPlatform(IPhysicsCollider platformCollider)
	{
		return this.player.State.IsPlatformDropping && this.player.IsPlatformLastDropped(platformCollider);
	}

	// Token: 0x06001EDB RID: 7899 RVA: 0x0009C771 File Offset: 0x0009AB71
	private void landCallback(ref Vector3F previousVelocity)
	{
		this.resetStateOnLand();
		this.player.OnLand(ref previousVelocity);
	}

	// Token: 0x06001EDC RID: 7900 RVA: 0x0009C785 File Offset: 0x0009AB85
	private void fallCallback()
	{
		this.player.OnFall();
	}

	// Token: 0x06001EDD RID: 7901 RVA: 0x0009C794 File Offset: 0x0009AB94
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

	// Token: 0x06001EDE RID: 7902 RVA: 0x0009C7FC File Offset: 0x0009ABFC
	private void techCallback(TechType techType, CollisionData collision)
	{
		if (techType == TechType.Ground)
		{
			this.resetStateOnLand();
		}
		this.player.PerformTech(techType, collision);
	}

	// Token: 0x06001EDF RID: 7903 RVA: 0x0009C818 File Offset: 0x0009AC18
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

	// Token: 0x06001EE0 RID: 7904 RVA: 0x0009C87E File Offset: 0x0009AC7E
	public void ClearFastFall()
	{
		this.playerState.isFastFalling = false;
	}

	// Token: 0x06001EE1 RID: 7905 RVA: 0x0009C88C File Offset: 0x0009AC8C
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

	// Token: 0x06001EE2 RID: 7906 RVA: 0x0009C93C File Offset: 0x0009AD3C
	public void BeginFastFalling()
	{
		Vector3F movementVelocity = this.state.movementVelocity;
		movementVelocity.y = -(Fixed)((double)this.data.fastFallSpeed);
		this.state.SetVelocity(movementVelocity, VelocityType.Movement);
		this.playerState.isFastFalling = true;
	}

	// Token: 0x06001EE3 RID: 7907 RVA: 0x0009C98C File Offset: 0x0009AD8C
	public void DelayedFastFall()
	{
		Vector3F movementVelocity = this.state.movementVelocity;
		movementVelocity.y = -(Fixed)((double)this.data.fastFallSpeed);
		this.state.SetVelocity(movementVelocity, VelocityType.Movement);
	}

	// Token: 0x06001EE4 RID: 7908 RVA: 0x0009C9CF File Offset: 0x0009ADCF
	public void BeginDashBrakingOrPivoting()
	{
		if (base.config.knockbackConfig.globalCapBrakePrivotSpeed)
		{
			this.Simulator.CapHorizontalSpeed(this.state, (Fixed)((double)this.data.fastWalkMaxSpeed), VelocityType.Movement);
		}
	}

	// Token: 0x06001EE5 RID: 7909 RVA: 0x0009CA0C File Offset: 0x0009AE0C
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

	// Token: 0x06001EE6 RID: 7910 RVA: 0x0009CA60 File Offset: 0x0009AE60
	public void CancelIntoShorthop()
	{
		if (!this.state.IsGrounded && this.state.movementVelocity.y != (Fixed)((double)this.data.shortJumpSpeed))
		{
			Fixed src = (Fixed)((double)this.data.jumpSpeed) - (Fixed)((double)this.data.shortJumpSpeed);
			Vector3F vector3F = new Vector3F(0, -src, 0);
			this.state.AddVelocity(ref vector3F, VelocityType.Movement);
		}
	}

	// Token: 0x06001EE7 RID: 7911 RVA: 0x0009CAFA File Offset: 0x0009AEFA
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

	// Token: 0x06001EE8 RID: 7912 RVA: 0x0009CB34 File Offset: 0x0009AF34
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

	// Token: 0x06001EE9 RID: 7913 RVA: 0x0009CCA0 File Offset: 0x0009B0A0
	private void tickAirAccelerationHorizontal(ref Vector3F newVelocity)
	{
		this.tickAirAcceleration(ref newVelocity.x, ref this.state.targetMoveVelocityHorizontalFrames, this.state.movementVelocity.x, this.state.targetMoveVelocity.x, (Fixed)((double)this.data.jumpHorizontalAccel));
	}

	// Token: 0x06001EEA RID: 7914 RVA: 0x0009CCF8 File Offset: 0x0009B0F8
	private void tickJumpAccelerationVertical(ref Vector3F newVelocity)
	{
		this.tickAirAcceleration(ref newVelocity.y, ref this.state.targetMoveVelocityVerticalFrames, this.state.movementVelocity.y, this.state.targetMoveVelocity.y, (Fixed)((double)this.data.jumpVerticalAccel));
	}

	// Token: 0x06001EEB RID: 7915 RVA: 0x0009CD50 File Offset: 0x0009B150
	private void tickAirAcceleration(ref Fixed newVelocity, ref int frameCounter, Fixed baseVelocity, Fixed targetVelocity, Fixed accel)
	{
		if (FixedMath.Abs(baseVelocity - targetVelocity) <= accel)
		{
			newVelocity = targetVelocity;
			frameCounter = 0;
		}
		else
		{
			int multi = (!(baseVelocity < targetVelocity)) ? -1 : 1;
			newVelocity += accel * multi;
		}
	}

	// Token: 0x06001EEC RID: 7916 RVA: 0x0009CDB4 File Offset: 0x0009B1B4
	private void onGroundToAir()
	{
		if (!this.player.State.IsDownState)
		{
			Vector3F knockbackVelocity = this.KnockbackVelocity;
			this.state.ClearVelocity(true, true, true, VelocityType.Knockback);
			this.state.AddVelocity(ref knockbackVelocity, VelocityType.Movement);
		}
	}

	// Token: 0x06001EED RID: 7917 RVA: 0x0009CDFC File Offset: 0x0009B1FC
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

	// Token: 0x06001EEE RID: 7918 RVA: 0x0009CE56 File Offset: 0x0009B256
	public void ResetDelayedMovement()
	{
		this.state.pivotJump = false;
		this.state.targetMoveVelocity = Vector2F.zero;
		this.state.targetMoveVelocityHorizontalFrames = 0;
		this.state.targetMoveVelocityVerticalFrames = 0;
	}

	// Token: 0x06001EEF RID: 7919 RVA: 0x0009CE8C File Offset: 0x0009B28C
	public void AddGroundedHorizontalVelocity(Fixed velocity)
	{
		if (!this.IsGrounded || !(this.state.groundedNormal != Vector3F.zero))
		{
			Debug.LogError(string.Concat(new object[]
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

	// Token: 0x06001EF0 RID: 7920 RVA: 0x0009CF40 File Offset: 0x0009B340
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

	// Token: 0x06001EF1 RID: 7921 RVA: 0x0009CFBC File Offset: 0x0009B3BC
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

	// Token: 0x06001EF2 RID: 7922 RVA: 0x0009D06F File Offset: 0x0009B46F
	public void SetOverride(PhysicsOverride input)
	{
		this.state.physicsOverride = input;
	}

	// Token: 0x06001EF3 RID: 7923 RVA: 0x0009D080 File Offset: 0x0009B480
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

	// Token: 0x06001EF4 RID: 7924 RVA: 0x0009D340 File Offset: 0x0009B740
	public void OnCollisionBoundsChanged(bool updateImmediately)
	{
		this.state.bounds.dirty = true;
		if (updateImmediately)
		{
			this.ProcessBoundsIfDirty();
		}
	}

	// Token: 0x06001EF5 RID: 7925 RVA: 0x0009D360 File Offset: 0x0009B760
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

	// Token: 0x06001EF6 RID: 7926 RVA: 0x0009DAA8 File Offset: 0x0009BEA8
	private ECBOverrideData getEcbData()
	{
		if (this.player.State.IsMoveActive && this.player.CurrentMove != null && this.player.CurrentMove.ecbOverrides.Length != 0)
		{
			int internalFrame = this.player.ActiveMove.Model.internalFrame;
			foreach (ECBOverrideData ecboverrideData in this.player.CurrentMove.ecbOverrides)
			{
				if (internalFrame >= ecboverrideData.startFrame && internalFrame <= ecboverrideData.endFrame)
				{
					return ecboverrideData;
				}
			}
		}
		return this.defaultECBData;
	}

	// Token: 0x06001EF7 RID: 7927 RVA: 0x0009DB58 File Offset: 0x0009BF58
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
			Debug.LogWarning("Reached maxIterations when processing dirty bounds for player in state " + this.player.State.ActionState);
		}
	}

	// Token: 0x06001EF8 RID: 7928 RVA: 0x0009DE49 File Offset: 0x0009C249
	public void LoadPhysicsData(CharacterPhysicsData physicsData)
	{
		this.context.defaultCharacterData = physicsData;
	}

	// Token: 0x06001EF9 RID: 7929 RVA: 0x0009DE57 File Offset: 0x0009C257
	public Vector3F GetPosition()
	{
		return this.state.position;
	}

	// Token: 0x06001EFA RID: 7930 RVA: 0x0009DE64 File Offset: 0x0009C264
	public void SetPosition(Vector3F position)
	{
		position.z = 0;
		base.transform.position = (Vector3)position;
		this.state.position = position;
	}

	// Token: 0x06001EFB RID: 7931 RVA: 0x0009DE90 File Offset: 0x0009C290
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

	// Token: 0x06001EFC RID: 7932 RVA: 0x0009DF18 File Offset: 0x0009C318
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

	// Token: 0x06001EFD RID: 7933 RVA: 0x0009E040 File Offset: 0x0009C440
	public void SyncGroundState()
	{
		Vector3F zero = Vector3F.zero;
		this.collisionsBuffer.Clear();
		this.Simulator.CheckIfGrounded(this.context, this.collisionsBuffer, zero);
	}

	// Token: 0x06001EFE RID: 7934 RVA: 0x0009E076 File Offset: 0x0009C476
	public void OnGrabLedge()
	{
		this.StopMovement(true, true, VelocityType.Total);
		this.ResetAirJump();
		this.playerState.isFastFalling = false;
	}

	// Token: 0x06001EFF RID: 7935 RVA: 0x0009E093 File Offset: 0x0009C493
	public void ResetAirJump()
	{
		this.playerState.usedAirJump = false;
	}

	// Token: 0x06001F00 RID: 7936 RVA: 0x0009E0A1 File Offset: 0x0009C4A1
	public void ResetGroundJump()
	{
		this.playerState.usedGroundJump = false;
	}

	// Token: 0x06001F01 RID: 7937 RVA: 0x0009E0AF File Offset: 0x0009C4AF
	public void ResetAllJumps()
	{
		this.ResetAirJump();
		this.ResetGroundJump();
	}

	// Token: 0x06001F02 RID: 7938 RVA: 0x0009E0BD File Offset: 0x0009C4BD
	public void ResetStateOnDeath()
	{
		this.StopMovement(true, true, VelocityType.Total);
		this.ResetAllJumps();
		this.context.playerPhysicsModel.isFastFalling = false;
		this.context.model.isGrounded = true;
	}

	// Token: 0x06001F03 RID: 7939 RVA: 0x0009E0F0 File Offset: 0x0009C4F0
	public bool PerformBoundCast(AbsoluteDirection boundPoint, Vector3F originOffset, Fixed castDist, Vector2F castDirection, int castMask, out RaycastHitData hit)
	{
		Vector3F a = this.getBoundPointFromAbsoluteDirection(boundPoint);
		return PhysicsRaycastCalculator.GetFirstRaycastHit(this.context, a + originOffset, castDirection, castDist + PlayerPhysicsController.BOUNDS_CAST_TOLERANCE, castMask, out hit, default(Fixed));
	}

	// Token: 0x06001F04 RID: 7940 RVA: 0x0009E13D File Offset: 0x0009C53D
	public bool PerformBoundCast(RelativeDirection boundPoint, Vector3F originOffset, Fixed castDist, Vector2F castDirection, int castMask, out RaycastHitData hit)
	{
		return this.PerformBoundCast(this.getAbsoluteFromRelativeDirection(boundPoint), originOffset, castDist, castDirection, castMask, out hit);
	}

	// Token: 0x06001F05 RID: 7941 RVA: 0x0009E154 File Offset: 0x0009C554
	public bool PerformBoundCast(RelativeDirection boundPoint, Vector3F originOffset, Fixed castDist, int castMask, out RaycastHitData hit)
	{
		AbsoluteDirection absoluteFromRelativeDirection = this.getAbsoluteFromRelativeDirection(boundPoint);
		Vector2F castDirectionFromAbsoluteDirection = this.getCastDirectionFromAbsoluteDirection(absoluteFromRelativeDirection);
		return this.PerformBoundCast(absoluteFromRelativeDirection, originOffset, castDist + PlayerPhysicsController.BOUNDS_CAST_TOLERANCE, castDirectionFromAbsoluteDirection, castMask, out hit);
	}

	// Token: 0x06001F06 RID: 7942 RVA: 0x0009E18C File Offset: 0x0009C58C
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

	// Token: 0x06001F07 RID: 7943 RVA: 0x0009E1EC File Offset: 0x0009C5EC
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

	// Token: 0x06001F08 RID: 7944 RVA: 0x0009E2CB File Offset: 0x0009C6CB
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

	// Token: 0x06001F09 RID: 7945 RVA: 0x0009E308 File Offset: 0x0009C708
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

	// Token: 0x06001F0A RID: 7946 RVA: 0x0009E3A0 File Offset: 0x0009C7A0
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

	// Token: 0x06001F0B RID: 7947 RVA: 0x0009E59A File Offset: 0x0009C99A
	private void onCliffProtection(Vector3F delta, Vector3F position)
	{
		this.playerState.teeteringDirection = ((!(delta.x > 0)) ? HorizontalDirection.Left : HorizontalDirection.Right);
		this.playerState.teeteringPosition = position;
	}

	// Token: 0x06001F0C RID: 7948 RVA: 0x0009E5CC File Offset: 0x0009C9CC
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

	// Token: 0x06001F0D RID: 7949 RVA: 0x0009E618 File Offset: 0x0009CA18
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

	// Token: 0x06001F0E RID: 7950 RVA: 0x0009E684 File Offset: 0x0009CA84
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

	// Token: 0x040018BA RID: 6330
	public static int UNCAPPED_MAX_SPEED = 10000;

	// Token: 0x040018BB RID: 6331
	public static Fixed MIN_ENVIRONMENT_BOUNDS_DELTA = (Fixed)0.001;

	// Token: 0x040018BC RID: 6332
	public static readonly Fixed BOUNDS_CAST_TOLERANCE = (Fixed)0.005;

	// Token: 0x040018BD RID: 6333
	public static readonly Vector3F BOUNDS_CAST_UP_OFFSET = Vector3F.down * PlayerPhysicsController.BOUNDS_CAST_TOLERANCE;

	// Token: 0x040018BE RID: 6334
	public static readonly Vector3F BOUNDS_CAST_DOWN_OFFSET = Vector3F.up * PlayerPhysicsController.BOUNDS_CAST_TOLERANCE;

	// Token: 0x040018BF RID: 6335
	public static readonly Vector3F BOUNDS_CAST_LEFT_OFFSET = Vector3F.right * PlayerPhysicsController.BOUNDS_CAST_TOLERANCE;

	// Token: 0x040018C0 RID: 6336
	public static readonly Vector3F BOUNDS_CAST_RIGHT_OFFSET = Vector3F.left * PlayerPhysicsController.BOUNDS_CAST_TOLERANCE;

	// Token: 0x040018C3 RID: 6339
	private PhysicsModel state;

	// Token: 0x040018C4 RID: 6340
	private PlayerPhysicsModel playerState;

	// Token: 0x040018C6 RID: 6342
	private PhysicsContext context;

	// Token: 0x040018C7 RID: 6343
	private IPhysicsDelegate player;

	// Token: 0x040018C8 RID: 6344
	private ECBOverrideData defaultECBData = new ECBOverrideData();

	// Token: 0x040018C9 RID: 6345
	private List<CollisionData> collisionsBuffer = new List<CollisionData>();

	// Token: 0x040018CA RID: 6346
	private EdgeData previousBoundsBuffer;

	// Token: 0x040018CB RID: 6347
	private static Fixed MIN_COLLISION_DIAMOND_WIDTH = (Fixed)0.4000000059604645;

	// Token: 0x040018CC RID: 6348
	private static Fixed MIN_COLLISION_DIAMOND_HEIGHT = (Fixed)0.4000000059604645;
}
