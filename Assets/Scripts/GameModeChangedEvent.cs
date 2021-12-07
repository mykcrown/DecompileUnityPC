// Decompile from assembly: Assembly-CSharp.dll

using System;

public class GameModeChangedEvent : UIEvent
{
	public GameMode rules;

	public GameModeData data;

	public GameModeChangedEvent(GameMode rules, GameModeData data)
	{
		this.rules = rules;
		this.data = data;
	}
}
