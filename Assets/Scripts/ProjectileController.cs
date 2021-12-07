// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : ArticleController, ITrailOwner
{
	private TrailEmitter emitter;

	public override HorizontalDirection Facing
	{
		get
		{
			return this.model.currentFacing;
		}
	}

	public bool EmitTrail
	{
		get
		{
			return true;
		}
	}

	public Vector3F EmitPosition
	{
		get
		{
			return this.model.physicsModel.position;
		}
	}

	public override List<Hit> Hits
	{
		get
		{
			if (this.model.isCanceled)
			{
				return null;
			}
			return base.Hits;
		}
	}

	public override void Init(ArticleData data)
	{
		base.Init(data);
		if (this.emitter != null)
		{
			this.emitter.Kill();
		}
	}

	public override bool OnHitSuccess(Hit hit, IHitOwner other, ImpactType impactType, ref Vector3F hitPosition, ref Vector3F hitVelocity, HitContext hitContext)
	{
		if (impactType == ImpactType.Reflect)
		{
			this.model.team = other.Team;
			Vector2 vector;
			MathUtil.AngleToVector(base.transform.rotation.eulerAngles.z).x = vector.x * -1f;
			if (this.data.refreshLifespanOnReflect)
			{
				this.model.currentFrame = 0;
			}
			Vector3F velocity = this.model.physicsModel.GetVelocity(VelocityType.Movement);
			velocity.x *= -1;
			this.model.physicsModel.SetVelocity(velocity, VelocityType.Movement);
			if (this.data.rotateWithAngle && velocity != Vector3F.zero)
			{
				base.SyncToRotation(MathUtil.VectorToAngle(ref velocity));
			}
			this.model.playerOwner = other.PlayerNum;
			this.Facing = this.OppositeFacing;
			return true;
		}
		bool flag = base.OnHitSuccess(hit, other, impactType, ref hitPosition, ref hitVelocity, hitContext);
		if (!flag)
		{
			bool wasBlocked = impactType == ImpactType.Shield;
			base.beginHitLag(base.combatCalculator.CalculateHitLagFrames(hit.data, other, base.combatCalculator.CalculateModifiedDamage(hit.data, other), wasBlocked));
			this.model.destroyOnHitLag = this.data.destroyOnImpact;
		}
		return flag;
	}

	public override bool ShouldCancelClankedMove(Hit myHit, Hit otherHit, IHitOwner other)
	{
		Fixed one = base.combatCalculator.CalculateModifiedDamage(myHit.data, this);
		Fixed other2 = base.combatCalculator.CalculateModifiedDamage(otherHit.data, other);
		return one - other2 < base.config.priorityConfig.priorityThreshold;
	}

	public override void OnHitBoxCollision(Hit myHit, IHitOwner other, Hit otherHit, ref Vector3F hitPosition, bool cancelMine, bool makeClankEffects)
	{
		Fixed one = base.combatCalculator.CalculateModifiedDamage(myHit.data, this);
		Fixed other2 = base.combatCalculator.CalculateModifiedDamage(otherHit.data, other);
		Fixed damage = (one + other2) / 2;
		int hitLagFrames = base.combatCalculator.CalculateHitLagForClank(myHit.data, damage);
		base.beginHitLag(hitLagFrames);
		if (cancelMine)
		{
			this.model.isCanceled = true;
			this.model.destroyOnHitLag = true;
		}
		if (makeClankEffects)
		{
			if (base.config.priorityConfig.clankParticle != null)
			{
				base.player.GameVFX.PlayParticle(base.config.priorityConfig.clankParticle, 30, (Vector3)hitPosition, false);
			}
			base.gameManager.Audio.PlayGameSound(new AudioRequest(base.config.priorityConfig.clankSound, (Vector3)hitPosition, null));
		}
	}

	public override void TickFrame()
	{
		if (!this.model.isExpired && this.model.hitLagFrames == 0)
		{
			this.model.physicsModel.BeginPhysicsUpdate();
			base.Simulator.AdvanceState(this.context, 1);
			this.model.physicsModel.EndPhysicsUpdate(base.transform);
		}
		base.TickFrame();
	}

	protected override void OnKilled()
	{
		base.OnKilled();
		if (this.emitter != null)
		{
			this.emitter.Kill();
		}
	}
}
