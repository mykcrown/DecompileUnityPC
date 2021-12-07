// Decompile from assembly: Assembly-CSharp.dll

using System;

public class SetChargeLevelCommand : GameEvent
{
	public PlayerNum Player
	{
		get;
		set;
	}

	public int Count
	{
		get;
		set;
	}

	public SetChargeLevelCommand(PlayerNum player, int chargeLevel)
	{
		this.Player = player;
		this.Count = chargeLevel;
	}
}
