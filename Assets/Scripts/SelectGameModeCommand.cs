// Decompile from assembly: Assembly-CSharp.dll

using System;

public class SelectGameModeCommand : GameEvent
{
	public GameMode mode
	{
		get;
		private set;
	}

	public SelectGameModeCommand(GameMode mode)
	{
		this.mode = mode;
	}
}
