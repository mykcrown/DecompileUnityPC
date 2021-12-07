using System;

// Token: 0x0200063D RID: 1597
[RollbackStatePoolMultiplier(16)]
[Serializable]
public class StageBehaviourGroupModel : StageObjectModel<StageBehaviourGroupModel>
{
	// Token: 0x06002719 RID: 10009 RVA: 0x000BF329 File Offset: 0x000BD729
	public override void CopyTo(StageBehaviourGroupModel target)
	{
		base.CopyTo(target);
		target.hasBeenTriggered = this.hasBeenTriggered;
		target.startFrame = this.startFrame;
		target.context = this.context;
	}

	// Token: 0x04001CA7 RID: 7335
	public bool hasBeenTriggered;

	// Token: 0x04001CA8 RID: 7336
	public int startFrame;

	// Token: 0x04001CA9 RID: 7337
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public object context;
}
