// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class ChargeLossWarningEvent : GameEvent, IPlayerOwnedQuantity
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

	public Fixed TimeTilLoss
	{
		get;
		set;
	}

	public ChargeLossWarningEvent(PlayerNum player, Fixed chargeLevel, Fixed timeTilLoss)
	{
		this.Player = player;
		this.Count = chargeLevel;
		this.TimeTilLoss = timeTilLoss;
	}
}
