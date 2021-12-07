// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class MatchResultMessage : GameEvent
{
	public List<TeamNum> winningTeams;

	public MatchResultMessage(List<TeamNum> winningTeams)
	{
		this.winningTeams = winningTeams;
	}
}
