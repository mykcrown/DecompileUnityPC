// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class PhysicsCalculator : IPhysicsCalculator
{
	private static Fixed UNLIMITED = (Fixed)1000000000.0;

	private static Fixed PLATFORM_COLLISION_VELOCITY_THRESHOLD = (Fixed)0.0001;

	public bool IsCheckGrounded(PhysicsContext context)
	{
		return context.characterData != null;
	}

	public bool IgnoreCollisions(PhysicsContext context)
	{
		return (context.ignoreCollisionsCallback != null && context.ignoreCollisionsCallback()) || (context.articleData != null && !context.articleData.DoesCollide);
	}

	public bool IsCheckPlatformForGrounded(PhysicsContext context, Vector2F deltaPos)
	{
		return this.isCorrectVelocityForPlatformCollision(deltaPos) && !context.isFallThroughPlatformsInput && !context.ignorePlatforms;
	}

	public bool IsGroundCollision(PhysicsContext context, Vector2F deltaPos)
	{
		return !(context.articleData != null) || context.articleData.collideWithTerrain;
	}

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

	public bool IsPlatformUndersideCollision(PhysicsContext context, Vector2F deltaPos)
	{
		return context.articleData != null && context.articleData.collideWithPlatforms && context.articleData.reflectPlatformUnderside && this.isCorrectVelocityForPlatformUndersideCollision(deltaPos);
	}

	private bool isCorrectVelocityForPlatformCollision(Vector2F deltaPos)
	{
		return deltaPos.y < PhysicsCalculator.PLATFORM_COLLISION_VELOCITY_THRESHOLD;
	}

	private bool isCorrectVelocityForPlatformUndersideCollision(Vector2F deltaPos)
	{
		return deltaPos.y > -PhysicsCalculator.PLATFORM_COLLISION_VELOCITY_THRESHOLD;
	}

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

	public Fixed GetGroundFriction(PhysicsContext context, ref Vector3F newMovementVelocity)
	{
		if (context.characterData != null)
		{
			return (!(FixedMath.Abs(newMovementVelocity.x) > context.characterData.FastWalkMaxSpeed)) ? context.characterData.Friction : context.characterData.HighSpeedFriction;
		}
		return (Fixed)0.0;
	}

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

	public Fixed GetMaxHorizontalAirVelocity(PhysicsContext context)
	{
		if (context.characterData != null)
		{
			return context.characterData.AirMaxSpeed;
		}
		return PhysicsCalculator.UNLIMITED;
	}
}
