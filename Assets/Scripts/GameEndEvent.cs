// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class GameEndEvent : GameEvent
{
	public List<PlayerNum> winners;

	public List<TeamNum> winningTeams;

	public GameEndEvent(List<PlayerNum> winners, List<TeamNum> winningTeams)
	{
		this.winners = winners;
		this.winningTeams = winningTeams;
	}
}
