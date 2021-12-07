// Decompile from assembly: Assembly-CSharp.dll

using System;

public class GamePausedEvent : GameEvent
{
	public bool paused;

	public PlayerNum player;

	public GamePausedEvent(bool paused, PlayerNum player)
	{
		this.paused = paused;
		this.player = player;
	}
}
