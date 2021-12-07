// Decompile from assembly: Assembly-CSharp.dll

using System;

public class CharacterEvent : GameEvent
{
	public PlayerController character
	{
		get;
		private set;
	}

	public CharacterEvent(PlayerController pCharacter)
	{
		this.character = pCharacter;
	}
}
