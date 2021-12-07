using System;
using FixedPoint;

// Token: 0x020005F0 RID: 1520
[Serializable]
public class PlayerPhysicsModel : RollbackStateTyped<PlayerPhysicsModel>
{
	// Token: 0x060023EC RID: 9196 RVA: 0x000B56EC File Offset: 0x000B3AEC
	public override void CopyTo(PlayerPhysicsModel targetIn)
	{
		targetIn.isFastFalling = this.isFastFalling;
		targetIn.usedAirJump = this.usedAirJump;
		targetIn.usedGroundJump = this.usedGroundJump;
		targetIn.teeteringPosition = this.teeteringPosition;
		targetIn.wasHit = this.wasHit;
		targetIn.teeteringDirection = this.teeteringDirection;
		targetIn.platformDropFrames = this.platformDropFrames;
		targetIn.gravityAssistFrames = this.gravityAssistFrames;
		targetIn.gravityAssistTotalFrames = this.gravityAssistTotalFrames;
		targetIn.hitOverrideGravityFrames = this.hitOverrideGravityFrames;
		targetIn.hitOverrideGravity = this.hitOverrideGravity;
		targetIn.lastPlatformDroppedThrough = this.lastPlatformDroppedThrough;
		targetIn.fallSpeedMultiplier = this.fallSpeedMultiplier;
		this.hitVibrate.CopyTo(targetIn.hitVibrate);
	}

	// Token: 0x060023ED RID: 9197 RVA: 0x000B57A8 File Offset: 0x000B3BA8
	public override object Clone()
	{
		PlayerPhysicsModel playerPhysicsModel = new PlayerPhysicsModel();
		this.CopyTo(playerPhysicsModel);
		return playerPhysicsModel;
	}

	// Token: 0x060023EE RID: 9198 RVA: 0x000B57C3 File Offset: 0x000B3BC3
	public void Load(PlayerPhysicsModel model)
	{
		this.isFastFalling = model.isFastFalling;
		this.usedAirJump = model.usedAirJump;
		this.usedGroundJump = model.usedGroundJump;
		this.platformDropFrames = model.platformDropFrames;
	}

	// Token: 0x04001B48 RID: 6984
	public bool isFastFalling;

	// Token: 0x04001B49 RID: 6985
	public bool usedAirJump;

	// Token: 0x04001B4A RID: 6986
	public bool usedGroundJump;

	// Token: 0x04001B4B RID: 6987
	public Vector3F teeteringPosition;

	// Token: 0x04001B4C RID: 6988
	public bool wasHit;

	// Token: 0x04001B4D RID: 6989
	public HorizontalDirection teeteringDirection;

	// Token: 0x04001B4E RID: 6990
	public int platformDropFrames;

	// Token: 0x04001B4F RID: 6991
	public int gravityAssistFrames;

	// Token: 0x04001B50 RID: 6992
	public int gravityAssistTotalFrames;

	// Token: 0x04001B51 RID: 6993
	public int hitOverrideGravityFrames;

	// Token: 0x04001B52 RID: 6994
	public Fixed hitOverrideGravity;

	// Token: 0x04001B53 RID: 6995
	public Fixed fallSpeedMultiplier = 1;

	// Token: 0x04001B54 RID: 6996
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public IPhysicsCollider lastPlatformDroppedThrough;

	// Token: 0x04001B55 RID: 6997
	[IsClonedManually]
	[IgnoreCopyValidation]
	public PlayerShakeModel hitVibrate = new PlayerShakeModel();
}
