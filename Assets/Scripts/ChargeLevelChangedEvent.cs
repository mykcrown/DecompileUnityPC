// Decompile from assembly: Assembly-CSharp.dll

using Beebyte.Obfuscator;
using FixedPoint;
using System;

[SkipRename]
public class ChargeLevelChangedEvent : GameEvent, IPlayerOwnedQuantity
{
	public PlayerNum Player
	{
		get;
		set;
	}

	public Fixed Count
	{
		get;
		set;
	}

	public ChargeLevelChangedEvent(PlayerNum player, Fixed chargeLevel)
	{
		this.Player = player;
		this.Count = chargeLevel;
	}
}
