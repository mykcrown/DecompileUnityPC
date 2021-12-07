// Decompile from assembly: Assembly-CSharp.dll

using System;

public class EquippableItem
{
	public EquipmentID id;

	public EquipmentTypes type;

	public CharacterID character;

	public int price;

	public EquipmentRarity rarity;

	public string backupNameText;

	public string overrideLocalizationKey;

	public string localAssetId;

	public bool isDefault;

	public ulong serverPackageId;

	public bool promoted;

	public ulong currencyAward;

	public string developmentIdString;

	public bool unique
	{
		get
		{
			return this.type != EquipmentTypes.CURRENCY && this.type != EquipmentTypes.UNLOCK_TOKEN;
		}
	}

	public EquippableItem()
	{
	}

	public EquippableItem(long id, EquipmentTypes type, CharacterID character, int price, EquipmentRarity rarity = EquipmentRarity.COMMON)
	{
		this.id = new EquipmentID(id);
		this.type = type;
		this.price = price;
		this.character = character;
		this.rarity = rarity;
	}
}
