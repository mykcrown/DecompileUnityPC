using System;

// Token: 0x02000740 RID: 1856
public interface IUserCharacterEquippedModel : IStartupLoader
{
	// Token: 0x06002E01 RID: 11777
	bool IsEquipped(EquippableItem item, CharacterID characterID, int portId);

	// Token: 0x06002E02 RID: 11778
	void Equip(EquippableItem item, CharacterID characterID, int portId, bool alertServer = true);

	// Token: 0x06002E03 RID: 11779
	EquipmentID GetEquippedByType(EquipmentTypes type, CharacterID characterId, int portId);

	// Token: 0x06002E04 RID: 11780
	EquippableItem GetEquippedItem(EquipmentTypes type, CharacterID characterId, int portId);

	// Token: 0x06002E05 RID: 11781
	SkinDefinition GetEquippedSkin(CharacterID characterId, int portId);
}
