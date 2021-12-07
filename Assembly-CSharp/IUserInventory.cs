using System;

// Token: 0x02000749 RID: 1865
public interface IUserInventory
{
	// Token: 0x06002E45 RID: 11845
	void AddItem(EquipmentID itemId, bool dispatchUpdate = true);

	// Token: 0x06002E46 RID: 11846
	void MarkAsNotNew(EquipmentID itemId, bool dispatchUpdate = true);

	// Token: 0x06002E47 RID: 11847
	void MarkAsNotNewAny(bool dispatchUpdate = true);

	// Token: 0x06002E48 RID: 11848
	void MarkAsNotNewCharacter(CharacterID characterId, bool dispatchUpdate = true);

	// Token: 0x06002E49 RID: 11849
	void MarkAsNotNewGlobal(EquipmentTypes type, bool dispatchUpdate = true);

	// Token: 0x06002E4A RID: 11850
	bool HasItem(EquipmentID itemId);

	// Token: 0x06002E4B RID: 11851
	bool HasSkin(SkinDefinition skin);

	// Token: 0x06002E4C RID: 11852
	bool IsNew(EquipmentID itemId);

	// Token: 0x06002E4D RID: 11853
	bool HasAnyNewItem();

	// Token: 0x06002E4E RID: 11854
	bool HasNewCharacterItem(CharacterID characterID);

	// Token: 0x06002E4F RID: 11855
	bool HasNewGlobalItem(EquipmentTypes type);

	// Token: 0x06002E50 RID: 11856
	int GetAllOwnedCharacterItemCount(CharacterID characterID);

	// Token: 0x06002E51 RID: 11857
	int GetOwnedCharacterItemCount(CharacterID characterID, EquipmentTypes type);

	// Token: 0x06002E52 RID: 11858
	int GetOwnedGlobalItemCount(EquipmentTypes type);
}
