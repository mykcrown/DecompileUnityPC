// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class HazardBehaviour : StageBehaviour
{
	public enum HazardType
	{
		Spike,
		Wind
	}

	public HazardBehaviour.HazardType hazardType;

	public Fixed spikeDamage = Fixed.Create(10.0);

	public Fixed spikeBaseKnockback = Fixed.Create(10.0);

	public Fixed spikeKnockbackScaling = Fixed.Create(2.0);

	public Fixed windAngle = Fixed.Create(0.0);

	public Fixed windForce = Fixed.Create(0.0);

	public bool windResetXVelocity = true;

	public bool windResetYVelocity = true;

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

	private void applySpike(HazardPlayContext hazardContext)
	{
		PlayerReference playerReference = base.gameController.currentGame.GetPlayerReference(hazardContext.playerNum);
		CollisionData collision = hazardContext.collision;
		Fixed knockbackAngle = MathUtil.VectorToAngle(ref collision.normal);
		playerReference.Controller.ForceImpact(this.spikeDamage, knockbackAngle, this.spikeBaseKnockback, this.spikeKnockbackScaling);
	}

	private void applyWind(HazardPlayContext hazardContext)
	{
		PlayerReference playerReference = base.gameController.currentGame.GetPlayerReference(hazardContext.playerNum);
		playerReference.Controller.ForceWindHit(this.windAngle, this.windForce, this.windResetXVelocity, this.windResetYVelocity);
	}
}
