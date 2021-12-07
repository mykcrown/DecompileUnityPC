using System;
using System.Collections.Generic;

// Token: 0x0200074E RID: 1870
public interface IUserTauntsModel : IStartupLoader
{
	// Token: 0x06002E6F RID: 11887
	Dictionary<TauntSlot, EquipmentID> GetSlotsForCharacter(CharacterID characterId, int portId);

	// Token: 0x06002E70 RID: 11888
	void Save(CharacterID characterId, SerializableDictionary<TauntSlot, EquipmentID> data, int portId, bool alertServer = true);

	// Token: 0x06002E71 RID: 11889
	bool IsEquipped(EquippableItem item, CharacterID characterId, int portId);

	// Token: 0x06002E72 RID: 11890
	EquipmentID GetItemInSlot(CharacterID characterId, TauntSlot slot, int portId);

	// Token: 0x06002E73 RID: 11891
	UserTaunts GetDataObject(int portId);
}
