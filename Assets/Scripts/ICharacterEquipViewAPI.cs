// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ICharacterEquipViewAPI
{
	CharacterID SelectedCharacter
	{
		get;
		set;
	}

	CharacterDefinition[] SelectedCharacterLinkedCharacters
	{
		get;
	}

	SkinDefinition EquippedSkin
	{
		get;
	}

	int SelectedCharacterIndex
	{
		get;
		set;
	}

	CharacterID[] GetCharacters();

	EquippableItem[] GetItems(CharacterID characterId, EquipmentTypes type);

	EquipmentTypes[] GetValidEquipTypes();

	SkinDefinition GetSkinFromItem(EquippableItem item, CharacterID characterId);

	CustomPlatform GetRespawnPlatformDataFromItem(EquippableItem item);

	string GetCharacterDisplayName(CharacterID characterId);

	int GetTotalItemsPossible(CharacterID characterId, EquipmentTypes type);

	int GetItemOwnedCount(CharacterID characterId, EquipmentTypes type);

	string GetCharacterPriceString(CharacterID characterId);

	string GetProAccountPriceString();

	bool IsCharacterUnlocked(CharacterID characterId);

	bool IsProAccountUnlocked();
}
