using System;

// Token: 0x020009D7 RID: 2519
public interface ICharacterEquipViewAPI
{
	// Token: 0x06004724 RID: 18212
	CharacterID[] GetCharacters();

	// Token: 0x06004725 RID: 18213
	EquippableItem[] GetItems(CharacterID characterId, EquipmentTypes type);

	// Token: 0x06004726 RID: 18214
	EquipmentTypes[] GetValidEquipTypes();

	// Token: 0x1700110E RID: 4366
	// (get) Token: 0x06004727 RID: 18215
	// (set) Token: 0x06004728 RID: 18216
	CharacterID SelectedCharacter { get; set; }

	// Token: 0x1700110F RID: 4367
	// (get) Token: 0x06004729 RID: 18217
	CharacterDefinition[] SelectedCharacterLinkedCharacters { get; }

	// Token: 0x17001110 RID: 4368
	// (get) Token: 0x0600472A RID: 18218
	SkinDefinition EquippedSkin { get; }

	// Token: 0x17001111 RID: 4369
	// (get) Token: 0x0600472B RID: 18219
	// (set) Token: 0x0600472C RID: 18220
	int SelectedCharacterIndex { get; set; }

	// Token: 0x0600472D RID: 18221
	SkinDefinition GetSkinFromItem(EquippableItem item, CharacterID characterId);

	// Token: 0x0600472E RID: 18222
	CustomPlatform GetRespawnPlatformDataFromItem(EquippableItem item);

	// Token: 0x0600472F RID: 18223
	string GetCharacterDisplayName(CharacterID characterId);

	// Token: 0x06004730 RID: 18224
	int GetTotalItemsPossible(CharacterID characterId, EquipmentTypes type);

	// Token: 0x06004731 RID: 18225
	int GetItemOwnedCount(CharacterID characterId, EquipmentTypes type);

	// Token: 0x06004732 RID: 18226
	string GetCharacterPriceString(CharacterID characterId);

	// Token: 0x06004733 RID: 18227
	string GetProAccountPriceString();

	// Token: 0x06004734 RID: 18228
	bool IsCharacterUnlocked(CharacterID characterId);

	// Token: 0x06004735 RID: 18229
	bool IsProAccountUnlocked();
}
