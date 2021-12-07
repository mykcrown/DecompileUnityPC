// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class UpdateDamageCommand : GameEvent
{
	public PlayerNum PlayerNum;

	public Fixed Damage;

	public UpdateDamageCommand(Fixed damage, PlayerNum playerNum)
	{
		this.PlayerNum = playerNum;
		this.Damage = damage;
	}
}
