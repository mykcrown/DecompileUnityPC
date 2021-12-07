using System;

// Token: 0x02000644 RID: 1604
[RollbackStatePoolMultiplier(6)]
[Serializable]
public class StageParticleSystemModel : StageObjectModel<StageParticleSystemModel>
{
	// Token: 0x0600274A RID: 10058 RVA: 0x000BFA37 File Offset: 0x000BDE37
	public override void CopyTo(StageParticleSystemModel target)
	{
		base.CopyTo(target);
		target.isPlaying = this.isPlaying;
		target.stopFrame = this.stopFrame;
	}

	// Token: 0x04001CD0 RID: 7376
	[IgnoreRollback(IgnoreRollbackType.Visual)]
	public bool isPlaying;

	// Token: 0x04001CD1 RID: 7377
	[IgnoreRollback(IgnoreRollbackType.Visual)]
	public int stopFrame = -1;
}
