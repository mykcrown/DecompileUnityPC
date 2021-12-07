// Decompile from assembly: Assembly-CSharp.dll

using System;

public class AttemptFriendlyAssistCommand : GameEvent
{
	public PlayerNum playerNum;

	public TeamNum team;

	public bool debugMode;

	public AttemptFriendlyAssistCommand(PlayerNum player, TeamNum team)
	{
		this.playerNum = player;
		this.team = team;
	}

	public AttemptFriendlyAssistCommand(PlayerNum player, TeamNum team, bool debugMode)
	{
		this.playerNum = player;
		this.team = team;
		this.debugMode = debugMode;
	}
}
