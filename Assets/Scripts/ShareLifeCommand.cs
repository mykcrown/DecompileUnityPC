// Decompile from assembly: Assembly-CSharp.dll

using System;

public class ShareLifeCommand : GameEvent
{
	public PlayerNum player;

	public TeamNum team;

	public ShareLifeCommand(PlayerNum player, TeamNum team)
	{
		this.player = player;
		this.team = team;
	}
}
