// Decompile from assembly: Assembly-CSharp.dll

using System;

[RollbackStatePoolMultiplier(16)]
[Serializable]
public class EffectModel : RollbackStateTyped<EffectModel>
{
	public int lifespanFrames;

	public int ticksAlive;

	public int pauseTicks;

	public bool billboard;

	public bool isDead;

	public int softKillFrame = -1;

	public int prewarmFrames;

	public int softKillFrameDuration;

	[IgnoreFloatValidation]
	public float frameTime = WTime.frameTime;

	[IgnoreRollback(IgnoreRollbackType.Visual)]
	public string effectName;

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
}
