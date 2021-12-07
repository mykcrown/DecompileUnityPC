// Decompile from assembly: Assembly-CSharp.dll

using System;

[RollbackStatePoolMultiplier(6)]
[Serializable]
public class StageParticleSystemModel : StageObjectModel<StageParticleSystemModel>
{
	[IgnoreRollback(IgnoreRollbackType.Visual)]
	public bool isPlaying;

	[IgnoreRollback(IgnoreRollbackType.Visual)]
	public int stopFrame = -1;

	public override void CopyTo(StageParticleSystemModel target)
	{
		base.CopyTo(target);
		target.isPlaying = this.isPlaying;
		target.stopFrame = this.stopFrame;
	}
}
