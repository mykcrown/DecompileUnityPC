// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IUserTauntsModel : IStartupLoader
{
	Dictionary<TauntSlot, EquipmentID> GetSlotsForCharacter(CharacterID characterId, int portId);

	void Save(CharacterID characterId, SerializableDictionary<TauntSlot, EquipmentID> data, int portId, bool alertServer = true);

	bool IsEquipped(EquippableItem item, CharacterID characterId, int portId);

	EquipmentID GetItemInSlot(CharacterID characterId, TauntSlot slot, int portId);

	UserTaunts GetDataObject(int portId);
}
