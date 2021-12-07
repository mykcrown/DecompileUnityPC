using System;

// Token: 0x0200038A RID: 906
public interface ICharacterLists
{
	// Token: 0x06001375 RID: 4981
	CharacterDefinition[] GetLegalCharacters();

	// Token: 0x06001376 RID: 4982
	CharacterDefinition[] GetNonRandomCharacters();

	// Token: 0x06001377 RID: 4983
	CharacterDefinition GetRandom();

	// Token: 0x06001378 RID: 4984
	bool IsEnabled(CharacterDefinition character);

	// Token: 0x06001379 RID: 4985
	bool IsEnabled(SkinDefinition skin);

	// Token: 0x0600137A RID: 4986
	CharacterDefinition GetCharacterDefinition(CharacterID characterId);

	// Token: 0x0600137B RID: 4987
	CharacterDefinition GetCharacterDefinition(string characterName);
}
