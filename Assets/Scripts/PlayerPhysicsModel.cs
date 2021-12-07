// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class PlayerPhysicsModel : RollbackStateTyped<PlayerPhysicsModel>
{
	public bool isFastFalling;

	public bool usedAirJump;

	public bool usedGroundJump;

	public Vector3F teeteringPosition;

	public bool wasHit;

	public HorizontalDirection teeteringDirection;

	public int platformDropFrames;

	public int gravityAssistFrames;

	public int gravityAssistTotalFrames;

	public int hitOverrideGravityFrames;

	public Fixed hitOverrideGravity;

	public Fixed fallSpeedMultiplier = 1;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public IPhysicsCollider lastPlatformDroppedThrough;

	[IgnoreCopyValidation, IsClonedManually]
	public PlayerShakeModel hitVibrate = new PlayerShakeModel();

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

	public override object Clone()
	{
		PlayerPhysicsModel playerPhysicsModel = new PlayerPhysicsModel();
		this.CopyTo(playerPhysicsModel);
		return playerPhysicsModel;
	}

	public void Load(PlayerPhysicsModel model)
	{
		this.isFastFalling = model.isFastFalling;
		this.usedAirJump = model.usedAirJump;
		this.usedGroundJump = model.usedGroundJump;
		this.platformDropFrames = model.platformDropFrames;
	}
}
