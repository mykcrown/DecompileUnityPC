using System;
using System.Collections.Generic;
using System.Text;
using Commerce;
using IconsServer;
using IconsTool;
using UnityEngine;

// Token: 0x0200071C RID: 1820
public static class StaticDataValidator
{
	// Token: 0x06002D01 RID: 11521 RVA: 0x000E7548 File Offset: 0x000E5948
	public static bool ValidateLocalStoreDb(StringBuilder error)
	{
		TextAsset textAsset = (TextAsset)Resources.Load("StaticDb");
		StaticDataReadOnly.StaticDataReader staticDataReader = new StaticDataReadOnly.StaticDataReader();
		if (!staticDataReader.LoadFromText(textAsset.text, false))
		{
			error.Append("Unable to load StaticDb.xml\n");
			return false;
		}
		if (staticDataReader.CharacterUnlockTokenId == 0UL)
		{
			error.Append("No Character Unlock Token in StaticDb.");
			return false;
		}
		error.Append("Items missing assets:\n");
		Dictionary<EItemRarity, int> dictionary = new Dictionary<EItemRarity, int>
		{
			{
				EItemRarity.Common,
				25
			},
			{
				EItemRarity.Uncommon,
				75
			},
			{
				EItemRarity.Rare,
				250
			},
			{
				EItemRarity.Iconic,
				1000
			}
		};
		Dictionary<int, List<int>> dictionary2 = new Dictionary<int, List<int>>();
		foreach (StaticDataReadOnly.StaticLootBoxData staticLootBoxData in staticDataReader.LootBoxMap.Values)
		{
			foreach (StaticDataReadOnly.StaticLootBoxItemData staticLootBoxItemData in staticLootBoxData.m_lootBoxItemList)
			{
				int key = (int)staticLootBoxItemData.m_itemId;
				if (!dictionary2.ContainsKey(key))
				{
					dictionary2.Add(key, new List<int>());
				}
				dictionary2[key].Add((int)staticLootBoxItemData.m_creditCurrencyValue);
			}
		}
		int num = 0;
		int num2 = 0;
		foreach (KeyValuePair<ulong, StaticDataReadOnly.StaticPackageData> keyValuePair in staticDataReader.PackageMap)
		{
			ulong key2 = keyValuePair.Key;
			StaticDataReadOnly.StaticPackageData value = keyValuePair.Value;
			if (value.m_state == EPackageState.Promoted)
			{
				foreach (StaticDataReadOnly.StaticPackageItemData staticPackageItemData in value.m_packageItemList)
				{
					if (staticPackageItemData.m_classificationType != EClassification.LootBox)
					{
						StaticDataReadOnly.StaticItemBase staticItemBase;
						if (!staticDataReader.ItemMap.TryGetValue(staticPackageItemData.m_staticId, out staticItemBase))
						{
							error.AppendFormat("\tPackage Referring to Missing ItemId {0}\n", staticPackageItemData.m_staticId);
						}
						else if (!StaticDataValidator.equipmentTypesNoValidate.Contains(staticItemBase.m_itemType))
						{
							EquipmentTypes itemType = (EquipmentTypes)staticItemBase.m_itemType;
							string text;
							bool flag = ItemLoader.ResourceDirectories.TryGetValue(itemType, out text);
							string text2 = (!flag) ? staticItemBase.m_asset : (ItemLoader.ResourceDirectories[itemType] + staticItemBase.m_asset);
							UnityEngine.Object x = (!flag) ? null : Resources.Load<UnityEngine.Object>(text2);
							if (x == null)
							{
								error.AppendFormat("\t{0}: Asset Path - {1}, Equipment Type - {2}\n", staticItemBase.m_friendlyName, text2, itemType);
								num++;
							}
							if (staticPackageItemData.m_currencyCost != 0U && value.m_currencyType == ECurrencyType.Soft)
							{
								int key3 = (int)staticItemBase.m_ID;
								if (dictionary2.ContainsKey(key3))
								{
									foreach (int num3 in dictionary2[key3])
									{
										if ((long)num3 != (long)((ulong)(staticPackageItemData.m_currencyCost / 5U)))
										{
											error.AppendFormat("\tIncorrectly priced item loot box refund: {0} cost {1} is not 5x refund {2}\n", staticItemBase.m_friendlyName, staticPackageItemData.m_currencyCost, num3);
											num2++;
										}
									}
									if (staticPackageItemData.m_creditCurrencyValue != staticPackageItemData.m_currencyCost / 5U)
									{
										error.AppendFormat("\tIncorrectly priced item refund: {0} cost {1} is not 5x refund {2}\n", staticItemBase.m_friendlyName, staticPackageItemData.m_currencyCost, staticPackageItemData.m_creditCurrencyValue);
										num2++;
									}
									if ((long)dictionary[staticItemBase.m_rarity] != (long)((ulong)staticPackageItemData.m_currencyCost))
									{
										error.AppendFormat("\tIncorrectly priced item: {0} cost {1} does not match expected cost for rarity {2}\n", staticItemBase.m_friendlyName, staticPackageItemData.m_currencyCost, staticItemBase.m_rarity);
										num2++;
									}
								}
							}
						}
					}
				}
			}
		}
		return num == 0 && num2 == 0;
	}

	// Token: 0x04001FFB RID: 8187
	private static readonly HashSet<EItemType> equipmentTypesNoValidate = new HashSet<EItemType>
	{
		EItemType.PremiumAccount,
		EItemType.Character,
		EItemType.Currency,
		EItemType.UnlockToken
	};
}
