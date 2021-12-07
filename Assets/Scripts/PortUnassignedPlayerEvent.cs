// Decompile from assembly: Assembly-CSharp.dll

using System;

public class PortUnassignedPlayerEvent : GameEvent
{
	public int portId;

	public PlayerNum playerNum;

	public PortUnassignedPlayerEvent(int portId, PlayerNum playerNum)
	{
		this.portId = portId;
		this.playerNum = playerNum;
	}
}
