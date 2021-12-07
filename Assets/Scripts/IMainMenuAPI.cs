// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IMainMenuAPI : IStartupLoader
{
	CharacterID characterHighlight
	{
		get;
		set;
	}

	bool ShowedLoginBonus
	{
		get;
		set;
	}

	void RandomizeCharacter();

	string GetCurrentStage();

	SkinDefinition GetCharacterSkin();

	SoundKey GetCurrentMusic();
}
