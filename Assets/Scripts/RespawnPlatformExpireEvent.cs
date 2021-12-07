// Decompile from assembly: Assembly-CSharp.dll

using System;

public class RespawnPlatformExpireEvent : GameEvent
{
	public PlayerNum playerNum
	{
		get;
		private set;
	}

	public RespawnPlatformExpireEvent(PlayerNum playerNum)
	{
		this.playerNum = playerNum;
	}
}
