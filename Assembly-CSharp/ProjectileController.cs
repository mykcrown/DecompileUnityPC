using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x02000371 RID: 881
public class ProjectileController : ArticleController, ITrailOwner
{
	// Token: 0x17000356 RID: 854
	// (get) Token: 0x060012BC RID: 4796 RVA: 0x0006B188 File Offset: 0x00069588
	public override HorizontalDirection Facing
	{
		get
		{
			return this.model.currentFacing;
		}
	}

	// Token: 0x17000357 RID: 855
	// (get) Token: 0x060012BD RID: 4797 RVA: 0x0006B195 File Offset: 0x00069595
	public bool EmitTrail
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000358 RID: 856
	// (get) Token: 0x060012BE RID: 4798 RVA: 0x0006B198 File Offset: 0x00069598
	public Vector3F EmitPosition
	{
		get
		{
			return this.model.physicsModel.position;
		}
	}

	// Token: 0x060012BF RID: 4799 RVA: 0x0006B1AA File Offset: 0x000695AA
	public override void Init(ArticleData data)
	{
		base.Init(data);
		if (this.emitter != null)
		{
			this.emitter.Kill();
		}
	}

	// Token: 0x060012C0 RID: 4800 RVA: 0x0006B1D0 File Offset: 0x000695D0
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

	// Token: 0x060012C1 RID: 4801 RVA: 0x0006B330 File Offset: 0x00069730
	public override bool ShouldCancelClankedMove(Hit myHit, Hit otherHit, IHitOwner other)
	{
		Fixed one = base.combatCalculator.CalculateModifiedDamage(myHit.data, this);
		Fixed other2 = base.combatCalculator.CalculateModifiedDamage(otherHit.data, other);
		return one - other2 < base.config.priorityConfig.priorityThreshold;
	}

	// Token: 0x060012C2 RID: 4802 RVA: 0x0006B380 File Offset: 0x00069780
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

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x060012C3 RID: 4803 RVA: 0x0006B47F File Offset: 0x0006987F
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

	// Token: 0x060012C4 RID: 4804 RVA: 0x0006B49C File Offset: 0x0006989C
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

	// Token: 0x060012C5 RID: 4805 RVA: 0x0006B507 File Offset: 0x00069907
	protected override void OnKilled()
	{
		base.OnKilled();
		if (this.emitter != null)
		{
			this.emitter.Kill();
		}
	}

	// Token: 0x04000C33 RID: 3123
	private TrailEmitter emitter;
}
