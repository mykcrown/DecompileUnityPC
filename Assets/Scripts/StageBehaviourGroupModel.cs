// Decompile from assembly: Assembly-CSharp.dll

using System;

[RollbackStatePoolMultiplier(16)]
[Serializable]
public class StageBehaviourGroupModel : StageObjectModel<StageBehaviourGroupModel>
{
	public bool hasBeenTriggered;

	public int startFrame;

	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public object context;

	public override void CopyTo(StageBehaviourGroupModel target)
	{
		base.CopyTo(target);
		target.hasBeenTriggered = this.hasBeenTriggered;
		target.startFrame = this.startFrame;
		target.context = this.context;
	}
}
