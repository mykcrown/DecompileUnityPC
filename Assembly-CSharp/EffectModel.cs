using System;

// Token: 0x02000432 RID: 1074
[RollbackStatePoolMultiplier(16)]
[Serializable]
public class EffectModel : RollbackStateTyped<EffectModel>
{
	// Token: 0x06001629 RID: 5673 RVA: 0x0007905C File Offset: 0x0007745C
	public override void CopyTo(EffectModel target)
	{
		target.lifespanFrames = this.lifespanFrames;
		target.ticksAlive = this.ticksAlive;
		target.pauseTicks = this.pauseTicks;
		target.billboard = this.billboard;
		target.isDead = this.isDead;
		target.softKillFrame = this.softKillFrame;
		target.prewarmFrames = this.prewarmFrames;
		target.softKillFrameDuration = this.softKillFrameDuration;
		target.frameTime = this.frameTime;
		target.effectName = this.effectName;
	}

	// Token: 0x04001102 RID: 4354
	public int lifespanFrames;

	// Token: 0x04001103 RID: 4355
	public int ticksAlive;

	// Token: 0x04001104 RID: 4356
	public int pauseTicks;

	// Token: 0x04001105 RID: 4357
	public bool billboard;

	// Token: 0x04001106 RID: 4358
	public bool isDead;

	// Token: 0x04001107 RID: 4359
	public int softKillFrame = -1;

	// Token: 0x04001108 RID: 4360
	public int prewarmFrames;

	// Token: 0x04001109 RID: 4361
	public int softKillFrameDuration;

	// Token: 0x0400110A RID: 4362
	[IgnoreFloatValidation]
	public float frameTime = WTime.frameTime;

	// Token: 0x0400110B RID: 4363
	[IgnoreRollback(IgnoreRollbackType.Visual)]
	public string effectName;
}
