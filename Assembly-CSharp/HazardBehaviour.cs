using System;
using FixedPoint;

// Token: 0x0200062D RID: 1581
public class HazardBehaviour : StageBehaviour
{
	// Token: 0x060026E2 RID: 9954 RVA: 0x000BE408 File Offset: 0x000BC808
	public override void Play(object context)
	{
		HazardPlayContext hazardPlayContext = context as HazardPlayContext;
		if (hazardPlayContext == null)
		{
			return;
		}
		HazardBehaviour.HazardType hazardType = this.hazardType;
		if (hazardType == HazardBehaviour.HazardType.Spike || hazardType != HazardBehaviour.HazardType.Wind)
		{
			this.applySpike(hazardPlayContext);
		}
		else
		{
			this.applyWind(hazardPlayContext);
		}
	}

	// Token: 0x060026E3 RID: 9955 RVA: 0x000BE454 File Offset: 0x000BC854
	private void applySpike(HazardPlayContext hazardContext)
	{
		PlayerReference playerReference = base.gameController.currentGame.GetPlayerReference(hazardContext.playerNum);
		CollisionData collision = hazardContext.collision;
		Fixed knockbackAngle = MathUtil.VectorToAngle(ref collision.normal);
		playerReference.Controller.ForceImpact(this.spikeDamage, knockbackAngle, this.spikeBaseKnockback, this.spikeKnockbackScaling);
	}

	// Token: 0x060026E4 RID: 9956 RVA: 0x000BE4AC File Offset: 0x000BC8AC
	private void applyWind(HazardPlayContext hazardContext)
	{
		PlayerReference playerReference = base.gameController.currentGame.GetPlayerReference(hazardContext.playerNum);
		playerReference.Controller.ForceWindHit(this.windAngle, this.windForce, this.windResetXVelocity, this.windResetYVelocity);
	}

	// Token: 0x04001C6B RID: 7275
	public HazardBehaviour.HazardType hazardType;

	// Token: 0x04001C6C RID: 7276
	public Fixed spikeDamage = Fixed.Create(10.0);

	// Token: 0x04001C6D RID: 7277
	public Fixed spikeBaseKnockback = Fixed.Create(10.0);

	// Token: 0x04001C6E RID: 7278
	public Fixed spikeKnockbackScaling = Fixed.Create(2.0);

	// Token: 0x04001C6F RID: 7279
	public Fixed windAngle = Fixed.Create(0.0);

	// Token: 0x04001C70 RID: 7280
	public Fixed windForce = Fixed.Create(0.0);

	// Token: 0x04001C71 RID: 7281
	public bool windResetXVelocity = true;

	// Token: 0x04001C72 RID: 7282
	public bool windResetYVelocity = true;

	// Token: 0x0200062E RID: 1582
	public enum HazardType
	{
		// Token: 0x04001C74 RID: 7284
		Spike,
		// Token: 0x04001C75 RID: 7285
		Wind
	}
}
