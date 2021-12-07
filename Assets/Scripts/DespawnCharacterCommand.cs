// Decompile from assembly: Assembly-CSharp.dll

using System;

public class DespawnCharacterCommand : GameEvent
{
	public PlayerNum player;

	public TeamNum team;

	public DespawnCharacterCommand(PlayerNum player, TeamNum team)
	{
		this.player = player;
		this.team = team;
	}
}
