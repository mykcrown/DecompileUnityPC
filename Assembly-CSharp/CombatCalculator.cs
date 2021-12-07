using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x020003C2 RID: 962
public class CombatCalculator : ICombatCalculator
{
	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x060014E4 RID: 5348 RVA: 0x00073781 File Offset: 0x00071B81
	// (set) Token: 0x060014E5 RID: 5349 RVA: 0x00073789 File Offset: 0x00071B89
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x060014E6 RID: 5350 RVA: 0x00073792 File Offset: 0x00071B92
	// (set) Token: 0x060014E7 RID: 5351 RVA: 0x0007379A File Offset: 0x00071B9A
	[Inject]
	public DebugKeys debugKeys { get; set; }

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x060014E8 RID: 5352 RVA: 0x000737A3 File Offset: 0x00071BA3
	// (set) Token: 0x060014E9 RID: 5353 RVA: 0x000737AB File Offset: 0x00071BAB
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x060014EA RID: 5354 RVA: 0x000737B4 File Offset: 0x00071BB4
	// (set) Token: 0x060014EB RID: 5355 RVA: 0x000737BC File Offset: 0x00071BBC
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x060014EC RID: 5356 RVA: 0x000737C5 File Offset: 0x00071BC5
	float ICombatCalculator.CalculateHitVibration(HitData hitData, Fixed damage, bool isAttacker)
	{
		if (isAttacker)
		{
			return (float)damage * this.config.knockbackConfig.hitlagVibrationAttacker;
		}
		return (float)damage * this.config.knockbackConfig.hitlagVibration;
	}

	// Token: 0x060014ED RID: 5357 RVA: 0x000737FC File Offset: 0x00071BFC
	int ICombatCalculator.CalculateHitLagFrames(HitData hitData, IHitOwner other, Fixed damage, bool wasBlocked)
	{
		if (hitData != null && hitData.useNoHitLag)
		{
			return 1;
		}
		Fixed @fixed = damage * this.config.knockbackConfig.hitlagMultiplier + this.config.knockbackConfig.hitlagAdd;
		if (hitData != null)
		{
			if (hitData.hitlag != 0)
			{
				@fixed = hitData.hitlag;
			}
			else if (wasBlocked && hitData.hitlagShieldMulti >= 0f)
			{
				@fixed *= hitData.hitlagShieldMulti;
			}
			else
			{
				@fixed *= hitData.hitlagMulti;
			}
		}
		if (@fixed < this.config.knockbackConfig.hitlagMin)
		{
			@fixed = this.config.knockbackConfig.hitlagMin;
		}
		if (wasBlocked)
		{
			@fixed *= this.config.knockbackConfig.hitlagShieldMulti;
		}
		@fixed *= other.HitLagMultiplier;
		int num = FixedMath.Ceil(@fixed);
		return (!hitData.uncappedHitLag) ? Mathf.Min(this.config.knockbackConfig.maxHitLagFrames, num) : num;
	}

	// Token: 0x060014EE RID: 5358 RVA: 0x0007392C File Offset: 0x00071D2C
	int ICombatCalculator.CalculateHitLagForClank(HitData hitData, Fixed damage)
	{
		Fixed @fixed = damage * this.config.knockbackConfig.clankHitlagMultiplier + this.config.knockbackConfig.clankHitlagAdd;
		if (@fixed < this.config.knockbackConfig.clankHitlagMin)
		{
			@fixed = this.config.knockbackConfig.clankHitlagMin;
		}
		int b = FixedMath.Ceil(@fixed);
		return Mathf.Min(this.config.knockbackConfig.maxHitLagFrames, b);
	}

	// Token: 0x060014EF RID: 5359 RVA: 0x000739B3 File Offset: 0x00071DB3
	int ICombatCalculator.CalculateHitLagForSpikeBounce(Fixed knockbackMagnitude)
	{
		return (int)(this.config.spikeConfig.spikeBounceHitlagBaseFrames + this.config.spikeConfig.spikeBounceHitlagFromVelocity * knockbackMagnitude);
	}

	// Token: 0x060014F0 RID: 5360 RVA: 0x000739E8 File Offset: 0x00071DE8
	public int CalculateHitStunFrames(HitData hitData, Fixed knockbackForce)
	{
		if (hitData.hitType == HitType.SelfHit && !hitData.selfHitData.applyHitStun)
		{
			return 0;
		}
		return (int)(knockbackForce * this.config.knockbackConfig.hitstunMultiplier);
	}

	// Token: 0x060014F1 RID: 5361 RVA: 0x00073A30 File Offset: 0x00071E30
	public int CalculateKnockbackIterator(Fixed knockbackForce)
	{
		Fixed balloonKnockbackThreshold = this.config.knockbackConfig.balloonKnockbackThreshold;
		if (knockbackForce >= balloonKnockbackThreshold)
		{
			return this.config.knockbackConfig.balloonKnockbackMaxIterator;
		}
		return 1;
	}

	// Token: 0x060014F2 RID: 5362 RVA: 0x00073A6C File Offset: 0x00071E6C
	public int CalculateSmokeTrailFrames(HitData hitData, Fixed knockbackForce)
	{
		if (hitData.hitType == HitType.SelfHit && !hitData.selfHitData.applyHitStun)
		{
			return 0;
		}
		return (int)(knockbackForce * this.config.knockbackConfig.hitstunMultiplier * this.config.knockbackConfig.smokeTrailMulti);
	}

	// Token: 0x060014F3 RID: 5363 RVA: 0x00073ACC File Offset: 0x00071ECC
	public int CalculateShieldStunFrames(HitData hitData, IHitOwner owner)
	{
		Fixed one = this.CalculateModifiedDamage(hitData, owner);
		return (int)(one * this.config.knockbackConfig.shieldStunMultiplier + this.config.knockbackConfig.shieldStunAdd);
	}

	// Token: 0x060014F4 RID: 5364 RVA: 0x00073B14 File Offset: 0x00071F14
	int ICombatCalculator.CalculateShieldBreakFrames(Fixed currentDamage)
	{
		return (int)(currentDamage * this.config.shieldConfig.shieldBreakFrameDamageMultiplier + this.config.shieldConfig.maxShieldBreakFrames);
	}

	// Token: 0x060014F5 RID: 5365 RVA: 0x00073B46 File Offset: 0x00071F46
	public Fixed CalculateModifiedDamage(HitData hitData, IHitOwner owner)
	{
		return this.calculateDamage(hitData, owner, true);
	}

	// Token: 0x060014F6 RID: 5366 RVA: 0x00073B51 File Offset: 0x00071F51
	public Fixed CalculateModifiedDamageUnstaled(HitData hitData, IHitOwner owner)
	{
		return this.calculateDamage(hitData, owner, false);
	}

	// Token: 0x060014F7 RID: 5367 RVA: 0x00073B5C File Offset: 0x00071F5C
	private Fixed calculateDamage(HitData hitData, IHitOwner owner, bool useStaleMultiplier)
	{
		if (hitData.hitType == HitType.SelfHit)
		{
			return (Fixed)((double)hitData.selfHitData.damage);
		}
		Fixed one = (Fixed)((double)hitData.damage);
		if (hitData.useBonusDamageFromSeed && this.hasMoveSeed(owner))
		{
			one += FixedMath.Clamp(this.getMoveSeed(owner).damage * hitData.seedDamageMultiplier, hitData.seedDamageBonusMin, hitData.seedDamageBonusMax);
		}
		if (useStaleMultiplier && !this.debugKeys.StaleMovesDisabled)
		{
			one *= owner.StaleDamageMultiplier;
		}
		return one * owner.DamageMultiplier;
	}

	// Token: 0x060014F8 RID: 5368 RVA: 0x00073C10 File Offset: 0x00072010
	private bool hasMoveSeed(IHitOwner owner)
	{
		MoveController activeMove = owner.ActiveMove;
		if (activeMove == null || !activeMove.IsActive)
		{
			return false;
		}
		MoveModel model = activeMove.Model;
		return model != null && model.seedData.isActive;
	}

	// Token: 0x060014F9 RID: 5369 RVA: 0x00073C54 File Offset: 0x00072054
	private MoveSeedData getMoveSeed(IHitOwner owner)
	{
		MoveController activeMove = owner.ActiveMove;
		MoveModel model = activeMove.Model;
		return model.seedData;
	}

	// Token: 0x060014FA RID: 5370 RVA: 0x00073C75 File Offset: 0x00072075
	private bool HitIsDirectionAgnostic(HitData hitData)
	{
		return hitData.hitType == HitType.Gust || (hitData.hitType == HitType.SelfHit && hitData.selfHitData.knockbackType != KnockbackType.FixedAngle);
	}

	// Token: 0x060014FB RID: 5371 RVA: 0x00073CA8 File Offset: 0x000720A8
	public bool CheckReverseHit(HitData hitData, IHitOwner attacker, IHitOwner defender, out int forceDirection)
	{
		if (this.HitIsDirectionAgnostic(hitData))
		{
			forceDirection = 1;
			return false;
		}
		forceDirection = ((attacker.Facing != HorizontalDirection.Right) ? -1 : 1);
		bool result = false;
		if (attacker != null && hitData.enableReverseHitboxes)
		{
			if (attacker.Facing == HorizontalDirection.Right && defender.Position.x < attacker.Position.x)
			{
				result = true;
				forceDirection = -1;
			}
			else if (attacker.Facing == HorizontalDirection.Left && defender.Position.x > attacker.Position.x)
			{
				result = true;
				forceDirection = 1;
			}
		}
		return result;
	}

	// Token: 0x060014FC RID: 5372 RVA: 0x00073D68 File Offset: 0x00072168
	private Vector2F getGustAwayAngle(IPlayerDelegate hitReceiver, IHitOwner hitOwner, Vector3F hitPosition, HitData hitData)
	{
		Vector2F result;
		if (hitReceiver.Physics.IsGrounded && !hitData.knockbackCausesFlinching)
		{
			Vector2F a = MathUtil.GetPerpendicularVector(hitReceiver.Physics.GroundedNormal);
			Vector2F lhs = hitOwner.Position;
			Vector2F rhs = hitReceiver.Position;
			if (lhs == rhs)
			{
				result = InputUtils.GetDirectionMultiplier(hitOwner.Facing) * a;
			}
			else
			{
				result = MathUtil.GetSign(hitReceiver.Position.x - hitOwner.Position.x) * a;
			}
		}
		else if (hitReceiver.Position == hitPosition)
		{
			result = new Vector3F((Fixed)0.0, (Fixed)1.0);
		}
		else
		{
			result = (hitReceiver.Physics.Center - hitPosition).normalized;
			int num = (int)MathUtil.VectorToAngle(ref result);
			if (hitData.knockbackAngle > 0f)
			{
				int num2 = (int)hitData.knockbackAngle / 2;
				int num3 = 90 - num2;
				int num4 = 90 + num2;
				if (num >= 0)
				{
					num = Mathf.Clamp(num, num3, num4);
				}
				else
				{
					num = ((num >= -90) ? num3 : num4);
				}
			}
			result = MathUtil.AngleToVector(num);
		}
		return result;
	}

	// Token: 0x060014FD RID: 5373 RVA: 0x00073EF4 File Offset: 0x000722F4
	public Fixed CalculateKnockback(HitData hitData, IHitOwner hitOwner, IPlayerDelegate hitReceiver, Fixed hitReceiverTotalDamage, Fixed damage, Vector3F hitPosition, out Vector2F knockbackVelocity)
	{
		Fixed @fixed = 0;
		knockbackVelocity = Vector2F.zero;
		if (hitData.hitType == HitType.Throw && !hitData.releaseGrabbedOpponent)
		{
			return 0;
		}
		GauntletCustomData gauntletCustomData = this.gameController.currentGame.ModeData.customData as GauntletCustomData;
		if (gauntletCustomData != null)
		{
			hitReceiverTotalDamage += gauntletCustomData.knockbackIncrease;
		}
		if (hitData.hitType == HitType.Hit || hitData.hitType == HitType.Throw || hitData.hitType == HitType.Gust)
		{
			Vector2F v = (hitData.hitType != HitType.Gust) ? new Vector2F((Fixed)((double)Mathf.Cos(MathUtil.toRadians(hitData.knockbackAngle))), (Fixed)((double)Mathf.Sin(MathUtil.toRadians(hitData.knockbackAngle)))) : this.getGustAwayAngle(hitReceiver, hitOwner, hitPosition, hitData);
			Fixed one = hitReceiverTotalDamage / this.config.knockbackConfig.currentDamageScale + hitReceiverTotalDamage * damage / this.config.knockbackConfig.impactDamageScale;
			Fixed one2 = hitReceiver.Physics.Weight;
			if (hitData.ignoreWeight)
			{
				one2 = this.config.knockbackConfig.baseWeight;
			}
			Fixed other = this.config.knockbackConfig.weightMultiplier / (one2 + this.config.knockbackConfig.weightOffset);
			Fixed one3 = hitData.knockbackScaling * (one * other * this.config.knockbackConfig.knockbackMultiplier + this.config.knockbackConfig.knockbackAdd);
			Fixed other2 = (!hitReceiver.State.IsCrouching) ? 1 : this.config.knockbackConfig.crouchCancelMultiplier;
			@fixed = (one3 + (Fixed)((double)hitData.baseKnockback)) * other2;
			@fixed = FixedMath.Min(@fixed, this.config.knockbackConfig.maxKnockbackValue);
			knockbackVelocity.Set(@fixed * v.x, @fixed * v.y);
			if (hitReceiver.State.IsGrounded && Vector3F.Dot(v, hitReceiver.Physics.GroundedNormal) < 0)
			{
				if (@fixed > this.config.knockbackConfig.tumbleKnockbackThreshold)
				{
					knockbackVelocity = FixedMath.Reflect(knockbackVelocity, hitReceiver.Physics.GroundedNormal);
				}
				else
				{
					Vector3F v2 = Vector3F.Project(knockbackVelocity, hitReceiver.Physics.GroundedNormal);
					knockbackVelocity -= v2;
				}
			}
		}
		else if (hitData.hitType == HitType.SelfHit)
		{
			if (hitData.selfHitData.interruptMove && hitReceiver.State.IsBusyWithMove)
			{
				knockbackVelocity = Vector2F.zero;
			}
			else
			{
				@fixed = (Fixed)((double)hitData.selfHitData.baseKnockback);
				if (hitData.selfHitData.knockbackType == KnockbackType.FixedAngle)
				{
					knockbackVelocity.Set(@fixed * FixedMath.Cos(MathUtil.toRadians((Fixed)((double)hitData.knockbackAngle))), @fixed * FixedMath.Sin(MathUtil.toRadians((Fixed)((double)hitData.knockbackAngle))));
				}
				else
				{
					Vector2F vector2F = (hitReceiver.Physics.Center - hitPosition).normalized;
					int angle = (int)MathUtil.VectorToAngle(ref vector2F);
					Fixed degrees = MathUtil.RoundAngleWithGranularity(angle, hitData.selfHitData.angleGranularity);
					vector2F = MathUtil.AngleToVector(degrees);
					knockbackVelocity.Set(@fixed * vector2F.x, @fixed * vector2F.y);
					if (hitData.selfHitData.knockbackType == KnockbackType.AwayUp)
					{
						knockbackVelocity.y = FixedMath.Abs(knockbackVelocity.y);
					}
				}
			}
		}
		knockbackVelocity *= this.config.knockbackConfig.knockbackToSpeedConversion;
		return @fixed;
	}

	// Token: 0x060014FE RID: 5374 RVA: 0x00074344 File Offset: 0x00072744
	bool ICombatCalculator.IsLethalHit(HitData hitData, IHitOwner attacker, IHitOwner defender, Vector3F impactPosition, Fixed multiplyVector)
	{
		PlayerController playerController = defender as PlayerController;
		if (playerController == null)
		{
			return false;
		}
		bool printDebug = this.config.flourishConfig.printDebug;
		Fixed other = this.CalculateModifiedDamage(hitData, attacker);
		Fixed damage = this.CalculateModifiedDamageUnstaled(hitData, attacker);
		Fixed @fixed = playerController.Damage + other;
		Vector2F vector2F;
		Fixed knockbackForce = this.CalculateKnockback(hitData, attacker, playerController, @fixed, damage, impactPosition, out vector2F);
		int multi;
		this.CheckReverseHit(hitData, attacker, playerController, out multi);
		vector2F.x *= multi;
		if (this.config.flourishConfig.printDebug)
		{
			Debug.Log("Simulated damage " + @fixed);
			Debug.Log("Simulated knockback " + vector2F);
		}
		int num = this.CalculateHitStunFrames(hitData, knockbackForce);
		num += this.config.hitConfig.lethalHitStunFrameBuffer;
		bool flag;
		if (this.config.flourishConfig.predictDI)
		{
			Fixed fixed2;
			if (this.config.comboEscapeConfig.scaling)
			{
				fixed2 = this.config.comboEscapeConfig.scalingMin;
			}
			else
			{
				fixed2 = this.config.comboEscapeConfig.maxRotationAngle;
			}
			Vector3F knockbackVelocity = MathUtil.RotateVector(vector2F, fixed2);
			flag = this.projectAngle(num, playerController.Physics.Context, knockbackVelocity, playerController.Position, multiplyVector, printDebug);
			if (!flag && !printDebug)
			{
				return false;
			}
			knockbackVelocity = MathUtil.RotateVector(vector2F, -fixed2);
			bool flag2 = this.projectAngle(num, playerController.Physics.Context, knockbackVelocity, playerController.Position, multiplyVector, printDebug);
			if (!flag2 && !printDebug)
			{
				return false;
			}
			bool flag3 = this.projectAngle(num, playerController.Physics.Context, vector2F, playerController.Position, multiplyVector, printDebug);
			if (!flag3 && !printDebug)
			{
				return false;
			}
			flag = (flag && flag2 && flag3);
		}
		else
		{
			flag = this.projectAngle(num, playerController.Physics.Context, vector2F, playerController.Position, multiplyVector, printDebug);
		}
		return flag;
	}

	// Token: 0x060014FF RID: 5375 RVA: 0x00074580 File Offset: 0x00072980
	private bool projectAngle(int projectionFrames, PhysicsContext context, Vector3F knockbackVelocity, Vector3F position, Fixed multi, bool debugDisplay)
	{
		Vector2F vector2F = knockbackVelocity.normalized;
		this.debugPointBuffer.Clear();
		if (this.raycastCollide(projectionFrames, ref vector2F, ref position, debugDisplay))
		{
			return false;
		}
		FixedRect fixedRect = (FixedRect)this.gameController.currentGame.Stage.SimulationData.BlastZoneBounds;
		Vector3F zero = Vector3F.zero;
		Vector3F vector3F = knockbackVelocity;
		Vector3F windVelocity = context.model.windVelocity;
		if (this.config.flourishConfig.printDebug)
		{
			Debug.Log("New movement velocity " + zero);
			Debug.Log("New knockback velocity " + vector3F);
		}
		bool isGrounded = context.model.isGrounded;
		PhysicsOverride physicsOverride = context.physicsState.PhysicsOverride;
		context.model.IsGrounded = false;
		context.physicsState.PhysicsOverride = null;
		bool flag = false;
		for (int i = 0; i < projectionFrames; i++)
		{
			this.gameController.currentGame.Physics.CalculateNextModelVelocity(context, true, ref vector3F, ref zero, ref windVelocity);
			if (this.config.flourishConfig.printDebug)
			{
				Debug.Log("New movement velocity " + zero);
				Debug.Log("New knockback velocity " + vector3F);
			}
			position += (zero + vector3F + windVelocity) * WTime.fixedDeltaTime * multi;
			if (debugDisplay)
			{
				this.pointBuffer.Add((Vector3)position);
			}
			if (this.config.flourishConfig.printDebug)
			{
				this.debugPointBuffer.Add(position);
			}
			if (!fixedRect.ContainsPoint(position))
			{
				flag = true;
				break;
			}
		}
		context.model.isGrounded = isGrounded;
		context.physicsState.PhysicsOverride = physicsOverride;
		if (debugDisplay)
		{
			Color color = (!flag) ? Color.blue : Color.red;
			PointListRenderer pointListRenderer = new GameObject("Point List Renderer").AddComponent<PointListRenderer>();
			this.injector.Inject(pointListRenderer);
			pointListRenderer.Init();
			pointListRenderer.Init(this.pointBuffer, color, color, this.pointBuffer.Count, 0.5f, 0f);
		}
		if (this.config.flourishConfig.printDebug && !this.config.flourishConfig.predictDI)
		{
			context.LoadDebugPrediction(this.debugPointBuffer);
		}
		return flag;
	}

	// Token: 0x06001500 RID: 5376 RVA: 0x00074810 File Offset: 0x00072C10
	private bool raycastCollide(int projectionFrames, ref Vector2F knockbackDirection, ref Vector3F position, bool debugDisplay)
	{
		PointListRenderer pointListRenderer = null;
		if (debugDisplay)
		{
			pointListRenderer = new GameObject("Point List Renderer").AddComponent<PointListRenderer>();
			this.injector.Inject(pointListRenderer);
			pointListRenderer.Init();
			this.pointBuffer.Clear();
			this.pointBuffer.Add((Vector3)position);
			this.pointBuffer.Add((Vector3)knockbackDirection * (float)CombatCalculator.LETHAL_HIT_CAST_DIST);
		}
		int num = this.gameController.currentGame.PhysicsWorld.RaycastTerrain(position, knockbackDirection, CombatCalculator.LETHAL_HIT_CAST_DIST, PhysicsSimulator.GroundAndPlatformMask, this.raycastBuffer, RaycastFlags.Default, default(Fixed));
		if (num > 0)
		{
			if (pointListRenderer != null)
			{
				pointListRenderer.Init(this.pointBuffer, Color.yellow, Color.yellow, projectionFrames, 0.1f, 0.1f);
			}
			return true;
		}
		Vector3F v = position;
		v.y += this.config.flourishConfig.secondRaycastOffsetY;
		num = this.gameController.currentGame.PhysicsWorld.RaycastTerrain(v, knockbackDirection, CombatCalculator.LETHAL_HIT_CAST_DIST, PhysicsSimulator.GroundAndPlatformMask, this.raycastBuffer, RaycastFlags.Default, default(Fixed));
		if (num > 0)
		{
			if (pointListRenderer != null)
			{
				this.pointBuffer.Clear();
				this.pointBuffer.Add((Vector3)v);
				this.pointBuffer.Add((Vector3)knockbackDirection * (float)CombatCalculator.LETHAL_HIT_CAST_DIST);
				pointListRenderer.Init(this.pointBuffer, Color.yellow, Color.yellow, projectionFrames, 0.1f, 0.1f);
			}
			return true;
		}
		if (pointListRenderer != null)
		{
			pointListRenderer.Init(this.pointBuffer, Color.green, Color.green, projectionFrames, 0.1f, 0.1f);
			this.pointBuffer.Clear();
		}
		return false;
	}

	// Token: 0x04000DD0 RID: 3536
	private static readonly int LETHAL_HIT_CAST_DIST = 100;

	// Token: 0x04000DD1 RID: 3537
	private RaycastHitData[] raycastBuffer = new RaycastHitData[5];

	// Token: 0x04000DD2 RID: 3538
	private List<Vector3> pointBuffer = new List<Vector3>();

	// Token: 0x04000DD3 RID: 3539
	private List<Vector3F> debugPointBuffer = new List<Vector3F>();
}
