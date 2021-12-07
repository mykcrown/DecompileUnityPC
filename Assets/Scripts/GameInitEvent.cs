// Decompile from assembly: Assembly-CSharp.dll

using System;

public class GameInitEvent : GameEvent
{
	public GameManager gameManager
	{
		get;
		private set;
	}

	public GameInitEvent(GameManager gameManager)
	{
		this.gameManager = gameManager;
	}
}
