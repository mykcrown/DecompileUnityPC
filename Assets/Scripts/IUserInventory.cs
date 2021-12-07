// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IUserInventory
{
	void AddItem(EquipmentID itemId, bool dispatchUpdate = true);

	void MarkAsNotNew(EquipmentID itemId, bool dispatchUpdate = true);

	void MarkAsNotNewAny(bool dispatchUpdate = true);

	void MarkAsNotNewCharacter(CharacterID characterId, bool dispatchUpdate = true);

	void MarkAsNotNewGlobal(EquipmentTypes type, bool dispatchUpdate = true);

	bool HasItem(EquipmentID itemId);

	bool HasSkin(SkinDefinition skin);

	bool IsNew(EquipmentID itemId);

	bool HasAnyNewItem();

	bool HasNewCharacterItem(CharacterID characterID);

	bool HasNewGlobalItem(EquipmentTypes type);

	int GetAllOwnedCharacterItemCount(CharacterID characterID);

	int GetOwnedCharacterItemCount(CharacterID characterID, EquipmentTypes type);

	int GetOwnedGlobalItemCount(EquipmentTypes type);
}
