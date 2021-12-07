// Decompile from assembly: Assembly-CSharp.dll

using System;

public class HologramCancelCommand : GameEvent
{
	public PlayerNum playerNum;

	public HologramCancelCommand(PlayerNum playerNum)
	{
		this.playerNum = playerNum;
	}
}
