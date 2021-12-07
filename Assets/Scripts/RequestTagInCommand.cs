// Decompile from assembly: Assembly-CSharp.dll

using System;

public class RequestTagInCommand : GameEvent
{
	public PlayerNum playerNum;

	public TeamNum team;

	public RequestTagInCommand(PlayerNum player, TeamNum team)
	{
		this.playerNum = player;
		this.team = team;
	}
}
