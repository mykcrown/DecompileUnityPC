// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IUserCharacterEquippedModel : IStartupLoader
{
	bool IsEquipped(EquippableItem item, CharacterID characterID, int portId);

	void Equip(EquippableItem item, CharacterID characterID, int portId, bool alertServer = true);

	EquipmentID GetEquippedByType(EquipmentTypes type, CharacterID characterId, int portId);

	EquippableItem GetEquippedItem(EquipmentTypes type, CharacterID characterId, int portId);

	SkinDefinition GetEquippedSkin(CharacterID characterId, int portId);
}
