// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class TimeEndGameConditionModel : RollbackStateTyped<TimeEndGameConditionModel>, IEndGameConditionModel
{
	public bool isFinished;

	[IgnoreCopyValidation, IsClonedManually]
	public List<PlayerNum> victors = new List<PlayerNum>(8);

	[IgnoreCopyValidation, IsClonedManually]
	public List<TeamNum> winningTeams = new List<TeamNum>(8);

	[IgnoreCopyValidation, IsClonedManually(IsClonedManuallyType.ShouldAutomate)]
	public int[] scores = new int[8];

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

	public override void CopyTo(TimeEndGameConditionModel target)
	{
		target.isFinished = this.isFinished;
		base.copyList<TeamNum>(this.winningTeams, target.winningTeams);
		base.copyList<PlayerNum>(this.victors, target.victors);
		for (int i = 0; i < this.scores.Length; i++)
		{
			target.scores[i] = this.scores[i];
		}
	}

	public override object Clone()
	{
		TimeEndGameConditionModel timeEndGameConditionModel = new TimeEndGameConditionModel();
		this.CopyTo(timeEndGameConditionModel);
		return timeEndGameConditionModel;
	}
}
