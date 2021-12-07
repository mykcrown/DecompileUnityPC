using System;

// Token: 0x0200072A RID: 1834
public interface IEquipmentModel
{
	// Token: 0x06002D3C RID: 11580
	EquippableItem[] GetAllCharacterItems(CharacterID characterId);

	// Token: 0x06002D3D RID: 11581
	EquippableItem[] GetCharacterItems(CharacterID characterId, EquipmentTypes type);

	// Token: 0x06002D3E RID: 11582
	EquippableItem[] GetGlobalItems(EquipmentTypes type);

	// Token: 0x06002D3F RID: 11583
	EquippableItem GetItem(EquipmentID id);

	// Token: 0x06002D40 RID: 11584
	EquippableItem GetItemByDevelopmentString(string developmentIdString);

	// Token: 0x06002D41 RID: 11585
	string GetLocalizedItemName(EquippableItem item);

	// Token: 0x06002D42 RID: 11586
	SkinDefinition GetSkinFromItem(EquipmentID itemId);

	// Token: 0x06002D43 RID: 11587
	EquippableItem GetItemFromSkinKey(string skinKey);

	// Token: 0x06002D44 RID: 11588
	CustomPlatform GetRespawnPlatformFromItem(EquipmentID itemId);

	// Token: 0x06002D45 RID: 11589
	Netsuke GetNetsukeFromItem(EquipmentID itemId);

	// Token: 0x06002D46 RID: 11590
	EquippableItem GetDefaultItem(CharacterID characterId, EquipmentTypes type);

	// Token: 0x06002D47 RID: 11591
	EquippableItem GetDefaultItem(EquipmentTypes type);

	// Token: 0x06002D48 RID: 11592
	PlayerToken GetPlayerTokenFromItem(EquipmentID itemId);

	// Token: 0x06002D49 RID: 11593
	HologramData GetHologramFromItem(EquipmentID itemId);

	// Token: 0x06002D4A RID: 11594
	PlayerCardIconData GetPlayerIconFromItem(EquipmentID itemId);

	// Token: 0x06002D4B RID: 11595
	VoiceTauntData GetVoiceTauntFromItem(EquipmentID itemId);
}
