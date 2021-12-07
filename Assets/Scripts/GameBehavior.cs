// Decompile from assembly: Assembly-CSharp.dll

using System;

public class GameBehavior : ClientBehavior
{
	protected GameManager gameManager
	{
		get
		{
			return base.gameController.currentGame;
		}
	}

	protected GameLog log
	{
		get
		{
			return (!(this.gameManager == null)) ? this.gameManager.Log : null;
		}
	}
}
