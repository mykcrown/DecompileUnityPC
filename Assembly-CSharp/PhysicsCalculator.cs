using System;
using FixedPoint;

// Token: 0x02000545 RID: 1349
public class PhysicsCalculator : IPhysicsCalculator
{
	// Token: 0x06001D80 RID: 7552 RVA: 0x00096EDD File Offset: 0x000952DD
	public bool IsCheckGrounded(PhysicsContext context)
	{
		return context.characterData != null;
	}

	// Token: 0x06001D81 RID: 7553 RVA: 0x00096EEC File Offset: 0x000952EC
	public bool IgnoreCollisions(PhysicsContext context)
	{
		return (context.ignoreCollisionsCallback != null && context.ignoreCollisionsCallback()) || (context.articleData != null && !context.articleData.DoesCollide);
	}

	// Token: 0x06001D82 RID: 7554 RVA: 0x00096F3A File Offset: 0x0009533A
	public bool IsCheckPlatformForGrounded(PhysicsContext context, Vector2F deltaPos)
	{
		return this.isCorrectVelocityForPlatformCollision(deltaPos) && !context.isFallThroughPlatformsInput && !context.ignorePlatforms;
	}

	// Token: 0x06001D83 RID: 7555 RVA: 0x00096F63 File Offset: 0x00095363
	public bool IsGroundCollision(PhysicsContext context, Vector2F deltaPos)
	{
		return !(context.articleData != null) || context.articleData.collideWithTerrain;
	}

	// Token: 0x06001D84 RID: 7556 RVA: 0x00096F84 File Offset: 0x00095384
	public bool IsPlatformCollision(PhysicsContext context, Vector2F deltaPos)
	{
		if (context.articleData != null)
		{
			if (context.articleData.collideWithPlatforms)
			{
				if (context.articleData.environmentCollisionBehavior == ArticleCollisionBehavior.Reflect && context.articleData.reflectPlatformUnderside && this.isCorrectVelocityForPlatformUndersideCollision(deltaPos))
				{
					return true;
				}
				if (this.isCorrectVelocityForPlatformCollision(deltaPos))
				{
					return true;
				}
			}
			return false;
		}
		if (!context.ignorePlatforms)
		{
			if (this.isCorrectVelocityForPlatformCollision(deltaPos))
			{
				return true;
			}
			if (context.isFallThroughPlatformsInput)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001D85 RID: 7557 RVA: 0x00097018 File Offset: 0x00095418
	public bool IsPlatformUndersideCollision(PhysicsContext context, Vector2F deltaPos)
	{
		return context.articleData != null && context.articleData.collideWithPlatforms && context.articleData.reflectPlatformUnderside && this.isCorrectVelocityForPlatformUndersideCollision(deltaPos);
	}

	// Token: 0x06001D86 RID: 7558 RVA: 0x00097054 File Offset: 0x00095454
	private bool isCorrectVelocityForPlatformCollision(Vector2F deltaPos)
	{
		return deltaPos.y < PhysicsCalculator.PLATFORM_COLLISION_VELOCITY_THRESHOLD;
	}

	// Token: 0x06001D87 RID: 7559 RVA: 0x00097067 File Offset: 0x00095467
	private bool isCorrectVelocityForPlatformUndersideCollision(Vector2F deltaPos)
	{
		return deltaPos.y > -PhysicsCalculator.PLATFORM_COLLISION_VELOCITY_THRESHOLD;
	}

	// Token: 0x06001D88 RID: 7560 RVA: 0x00097080 File Offset: 0x00095480
	public Fixed GetGravity(PhysicsContext context)
	{
		if (context.playerPhysicsModel != null && context.playerPhysicsModel.hitOverrideGravityFrames > 0)
		{
			return context.playerPhysicsModel.hitOverrideGravity;
		}
		if (context.characterData != null)
		{
			if (context.knockbackConfig.knockbackSuppressGravity && context.model.knockbackVelocity.sqrMagnitude >= context.knockbackConfig.knockbackSuppressGravityThreshold * context.knockbackConfig.knockbackSuppressGravityThreshold)
			{
				return context.knockbackConfig.knockbackGravity;
			}
			return context.characterData.Gravity;
		}
		else
		{
			if (context.articleData != null)
			{
				return (Fixed)((double)context.articleData.physics.gravity);
			}
			return (Fixed)0.0;
		}
	}

	// Token: 0x06001D89 RID: 7561 RVA: 0x00097158 File Offset: 0x00095558
	public Vector2F GetAirFriction(PhysicsContext context)
	{
		if (context.characterData != null)
		{
			return Vector2F.one * context.characterData.AirFriction;
		}
		if (context.articleData != null)
		{
			return Vector2F.one * (Fixed)((double)context.articleData.physics.airFriction);
		}
		return Vector2F.zero;
	}

	// Token: 0x06001D8A RID: 7562 RVA: 0x000971C0 File Offset: 0x000955C0
	public Fixed GetGroundFriction(PhysicsContext context, ref Vector3F newMovementVelocity)
	{
		if (context.characterData != null)
		{
			return (!(FixedMath.Abs(newMovementVelocity.x) > context.characterData.FastWalkMaxSpeed)) ? context.characterData.Friction : context.characterData.HighSpeedFriction;
		}
		return (Fixed)0.0;
	}

	// Token: 0x06001D8B RID: 7563 RVA: 0x00097224 File Offset: 0x00095624
	public Fixed GetMaxDownwardVelocity(PhysicsContext context)
	{
		if (context.physicsState.PhysicsOverride != null && context.physicsState.PhysicsOverride.overrideMaxFallSpeed && (!context.physicsState.PhysicsOverride.ignoreOverrideWhenFastFalling || !context.playerPhysicsModel.isFastFalling))
		{
			return -context.physicsState.PhysicsOverride.maxFallSpeed * context.playerPhysicsModel.fallSpeedMultiplier;
		}
		if (context.characterData != null)
		{
			return ((!context.playerPhysicsModel.isFastFalling) ? (-context.characterData.MaxFallSpeed) : (-context.characterData.FastFallSpeed)) * context.playerPhysicsModel.fallSpeedMultiplier;
		}
		return -PhysicsCalculator.UNLIMITED;
	}

	// Token: 0x06001D8C RID: 7564 RVA: 0x000972FC File Offset: 0x000956FC
	public Fixed GetMaxHorizontalAirVelocity(PhysicsContext context)
	{
		if (context.characterData != null)
		{
			return context.characterData.AirMaxSpeed;
		}
		return PhysicsCalculator.UNLIMITED;
	}

	// Token: 0x0400180D RID: 6157
	private static Fixed UNLIMITED = (Fixed)1000000000.0;

	// Token: 0x0400180E RID: 6158
	private static Fixed PLATFORM_COLLISION_VELOCITY_THRESHOLD = (Fixed)0.0001;
}
