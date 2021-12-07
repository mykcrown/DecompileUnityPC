using System;

// Token: 0x02000649 RID: 1609
[Serializable]
public class StageModel : RollbackStateTyped<StageModel>
{
	// Token: 0x06002766 RID: 10086 RVA: 0x000BFE18 File Offset: 0x000BE218
	public StageModel()
	{
		for (int i = 0; i < 8; i++)
		{
			this.respawnPointsInUse[i] = PlayerNum.None;
		}
	}

	// Token: 0x06002767 RID: 10087 RVA: 0x000BFE54 File Offset: 0x000BE254
	public override void CopyTo(StageModel target)
	{
		for (int i = 0; i < 8; i++)
		{
			target.respawnPointsInUse[i] = this.respawnPointsInUse[i];
		}
	}

	// Token: 0x06002768 RID: 10088 RVA: 0x000BFE84 File Offset: 0x000BE284
	public override object Clone()
	{
		StageModel stageModel = new StageModel();
		this.CopyTo(stageModel);
		return stageModel;
	}

	// Token: 0x04001CD8 RID: 7384
	private const int MAX_ACTIVE_RESPAWN_POINTS = 8;

	// Token: 0x04001CD9 RID: 7385
	[IsClonedManually]
	[IgnoreCopyValidation]
	public PlayerNum[] respawnPointsInUse = new PlayerNum[8];
}
