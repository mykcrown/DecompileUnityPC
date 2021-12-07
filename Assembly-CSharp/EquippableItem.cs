using System;

// Token: 0x0200072C RID: 1836
public class EquippableItem
{
	// Token: 0x06002D4E RID: 11598 RVA: 0x000E8889 File Offset: 0x000E6C89
	public EquippableItem()
	{
	}

	// Token: 0x06002D4F RID: 11599 RVA: 0x000E8891 File Offset: 0x000E6C91
	public EquippableItem(long id, EquipmentTypes type, CharacterID character, int price, EquipmentRarity rarity = EquipmentRarity.COMMON)
	{
		this.id = new EquipmentID(id);
		this.type = type;
		this.price = price;
		this.character = character;
		this.rarity = rarity;
	}

	// Token: 0x17000B15 RID: 2837
	// (get) Token: 0x06002D50 RID: 11600 RVA: 0x000E88C3 File Offset: 0x000E6CC3
	public bool unique
	{
		get
		{
			return this.type != EquipmentTypes.CURRENCY && this.type != EquipmentTypes.UNLOCK_TOKEN;
		}
	}

	// Token: 0x04002023 RID: 8227
	public EquipmentID id;

	// Token: 0x04002024 RID: 8228
	public EquipmentTypes type;

	// Token: 0x04002025 RID: 8229
	public CharacterID character;

	// Token: 0x04002026 RID: 8230
	public int price;

	// Token: 0x04002027 RID: 8231
	public EquipmentRarity rarity;

	// Token: 0x04002028 RID: 8232
	public string backupNameText;

	// Token: 0x04002029 RID: 8233
	public string overrideLocalizationKey;

	// Token: 0x0400202A RID: 8234
	public string localAssetId;

	// Token: 0x0400202B RID: 8235
	public bool isDefault;

	// Token: 0x0400202C RID: 8236
	public ulong serverPackageId;

	// Token: 0x0400202D RID: 8237
	public bool promoted;

	// Token: 0x0400202E RID: 8238
	public ulong currencyAward;

	// Token: 0x0400202F RID: 8239
	public string developmentIdString;
}
