// Decompile from assembly: Assembly-CSharp.dll

using System;

public class LogStatEvent : GameEvent
{
	public StatType stat;

	public int value;

	public PlayerNum player;

	public TeamNum team;

	public PointsValueType operation;

	public LogStatEvent(StatType stat, int value, PointsValueType operation, PlayerNum player, TeamNum team)
	{
		this.stat = stat;
		this.value = value;
		this.player = player;
		this.team = team;
		this.operation = operation;
	}
}
