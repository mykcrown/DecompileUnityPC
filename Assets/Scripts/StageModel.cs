// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class StageModel : RollbackStateTyped<StageModel>
{
	private const int MAX_ACTIVE_RESPAWN_POINTS = 8;

	[IgnoreCopyValidation, IsClonedManually]
	public PlayerNum[] respawnPointsInUse = new PlayerNum[8];

	public StageModel()
	{
		for (int i = 0; i < 8; i++)
		{
			this.respawnPointsInUse[i] = PlayerNum.None;
		}
	}

	public override void CopyTo(StageModel target)
	{
		for (int i = 0; i < 8; i++)
		{
			target.respawnPointsInUse[i] = this.respawnPointsInUse[i];
		}
	}

	public override object Clone()
	{
		StageModel stageModel = new StageModel();
		this.CopyTo(stageModel);
		return stageModel;
	}
}
