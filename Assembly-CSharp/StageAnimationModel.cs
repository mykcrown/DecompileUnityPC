using System;
using UnityEngine;

// Token: 0x0200063A RID: 1594
[RollbackStatePoolMultiplier(4)]
[Serializable]
public class StageAnimationModel : StageObjectModel<StageAnimationModel>
{
	// Token: 0x0600270A RID: 9994 RVA: 0x000BF075 File Offset: 0x000BD475
	public override void CopyTo(StageAnimationModel target)
	{
		base.CopyTo(target);
		target.StartFrame = this.StartFrame;
		target.AnimFrameLength = this.AnimFrameLength;
		target.Clip = this.Clip;
	}

	// Token: 0x04001C9F RID: 7327
	[IgnoreRollback(IgnoreRollbackType.Visual)]
	public int StartFrame;

	// Token: 0x04001CA0 RID: 7328
	[IgnoreRollback(IgnoreRollbackType.Visual)]
	public int AnimFrameLength;

	// Token: 0x04001CA1 RID: 7329
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public AnimationClip Clip;
}
