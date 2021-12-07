using System;

// Token: 0x0200063F RID: 1599
[Serializable]
public class StageCameraInfluencerModel : StageObjectModel<StageCameraInfluencerModel>
{
	// Token: 0x06002727 RID: 10023 RVA: 0x000BF5E6 File Offset: 0x000BD9E6
	public override void CopyTo(StageCameraInfluencerModel target)
	{
		base.CopyTo(target);
		target.IsActive = this.IsActive;
		target.ToggleFrame = this.ToggleFrame;
	}

	// Token: 0x04001CB1 RID: 7345
	public bool IsActive;

	// Token: 0x04001CB2 RID: 7346
	public int ToggleFrame = -1;
}
