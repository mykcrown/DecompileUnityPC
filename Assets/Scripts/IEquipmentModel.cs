// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IEquipmentModel
{
	EquippableItem[] GetAllCharacterItems(CharacterID characterId);

	EquippableItem[] GetCharacterItems(CharacterID characterId, EquipmentTypes type);

	EquippableItem[] GetGlobalItems(EquipmentTypes type);

	EquippableItem GetItem(EquipmentID id);

	EquippableItem GetItemByDevelopmentString(string developmentIdString);

	string GetLocalizedItemName(EquippableItem item);

	SkinDefinition GetSkinFromItem(EquipmentID itemId);

	EquippableItem GetItemFromSkinKey(string skinKey);

	CustomPlatform GetRespawnPlatformFromItem(EquipmentID itemId);

	Netsuke GetNetsukeFromItem(EquipmentID itemId);

	EquippableItem GetDefaultItem(CharacterID characterId, EquipmentTypes type);

	EquippableItem GetDefaultItem(EquipmentTypes type);

	PlayerToken GetPlayerTokenFromItem(EquipmentID itemId);

	HologramData GetHologramFromItem(EquipmentID itemId);

	PlayerCardIconData GetPlayerIconFromItem(EquipmentID itemId);

	VoiceTauntData GetVoiceTauntFromItem(EquipmentID itemId);
}
