// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IEndGameConditionModel
{
	bool IsFinished
	{
		get;
		set;
	}

	List<PlayerNum> Victors
	{
		get;
	}

	List<TeamNum> WinningTeams
	{
		get;
	}
}
