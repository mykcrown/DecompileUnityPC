// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[RollbackStatePoolMultiplier(4)]
[Serializable]
public class StageAnimationModel : StageObjectModel<StageAnimationModel>
{
	[IgnoreRollback(IgnoreRollbackType.Visual)]
	public int StartFrame;

	[IgnoreRollback(IgnoreRollbackType.Visual)]
	public int AnimFrameLength;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public AnimationClip Clip;

	public override void CopyTo(StageAnimationModel target)
	{
		base.CopyTo(target);
		target.StartFrame = this.StartFrame;
		target.AnimFrameLength = this.AnimFrameLength;
		target.Clip = this.Clip;
	}
}
