// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class TogglePlayerVisibilityCommand : GameEvent
{
	public List<PlayerNum> players;

	public bool visibility;

	public TogglePlayerVisibilityCommand(List<PlayerNum> players, bool visibility)
	{
		this.players = players;
		this.visibility = visibility;
	}
}
