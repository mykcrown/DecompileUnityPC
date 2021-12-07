// Decompile from assembly: Assembly-CSharp.dll

using System;

public class DisconnectPlayerCommand : GameEvent
{
	public int despawnFrame;

	public PlayerNum playerNum;

	public DisconnectPlayerCommand(PlayerNum playerNum, int despawnFrame)
	{
		this.playerNum = playerNum;
		this.despawnFrame = despawnFrame;
	}
}
