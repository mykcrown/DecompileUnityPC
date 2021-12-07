// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ICharacterLists
{
	CharacterDefinition[] GetLegalCharacters();

	CharacterDefinition[] GetNonRandomCharacters();

	CharacterDefinition GetRandom();

	bool IsEnabled(CharacterDefinition character);

	bool IsEnabled(SkinDefinition skin);

	CharacterDefinition GetCharacterDefinition(CharacterID characterId);

	CharacterDefinition GetCharacterDefinition(string characterName);
}
