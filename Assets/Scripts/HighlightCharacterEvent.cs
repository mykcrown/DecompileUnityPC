// Decompile from assembly: Assembly-CSharp.dll

using System;

public class HighlightCharacterEvent : UIEvent
{
	public PlayerNum playerNum;

	public CharacterDefinition characterDef;

	public HighlightCharacterEvent(PlayerNum playerNum, CharacterDefinition characterDef)
	{
		this.playerNum = playerNum;
		this.characterDef = characterDef;
	}
}
