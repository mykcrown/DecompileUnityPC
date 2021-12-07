// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class EndGameConditionModel : RollbackStateTyped<EndGameConditionModel>, IEndGameConditionModel
{
	public bool isFinished;

	[IgnoreCopyValidation, IsClonedManually]
	public List<PlayerNum> victors = new List<PlayerNum>(8);

	[IgnoreCopyValidation, IsClonedManually]
	public List<TeamNum> winningTeams = new List<TeamNum>(8);

	public bool IsFinished
	{
		get
		{
			return this.isFinished;
		}
		set
		{
			this.isFinished = value;
		}
	}

	public List<PlayerNum> Victors
	{
		get
		{
			return this.victors;
		}
	}

	public List<TeamNum> WinningTeams
	{
		get
		{
			return this.winningTeams;
		}
	}

	public override void CopyTo(EndGameConditionModel target)
	{
		target.isFinished = this.isFinished;
		base.copyList<TeamNum>(this.winningTeams, target.winningTeams);
		base.copyList<PlayerNum>(this.victors, target.victors);
	}

	public override object Clone()
	{
		EndGameConditionModel endGameConditionModel = new EndGameConditionModel();
		this.CopyTo(endGameConditionModel);
		return endGameConditionModel;
	}
}
