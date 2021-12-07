// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class StageCameraInfluencerModel : StageObjectModel<StageCameraInfluencerModel>
{
	public bool IsActive;

	public int ToggleFrame = -1;

	public override void CopyTo(StageCameraInfluencerModel target)
	{
		base.CopyTo(target);
		target.IsActive = this.IsActive;
		target.ToggleFrame = this.ToggleFrame;
	}
}
