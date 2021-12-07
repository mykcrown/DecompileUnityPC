// Decompile from assembly: Assembly-CSharp.dll

using Commerce;
using IconsServer;
using IconsTool;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class StaticDataValidator
{
	private static readonly HashSet<EItemType> equipmentTypesNoValidate = new HashSet<EItemType>
	{
		EItemType.PremiumAccount,
		EItemType.Character,
		EItemType.Currency,
		EItemType.UnlockToken
	};

	public static bool ValidateLocalStoreDb(StringBuilder error)
	{
		TextAsset textAsset = (TextAsset)Resources.Load("StaticDb");
		StaticDataReadOnly.StaticDataReader staticDataReader = new StaticDataReadOnly.StaticDataReader();
		if (!staticDataReader.LoadFromText(textAsset.text, false))
		{
			error.Append("Unable to load StaticDb.xml\n");
			return false;
		}
		if (staticDataReader.CharacterUnlockTokenId == 0uL)
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
		foreach (StaticDataReadOnly.StaticLootBoxData current in staticDataReader.LootBoxMap.Values)
		{
			foreach (StaticDataReadOnly.StaticLootBoxItemData current2 in current.m_lootBoxItemList)
			{
				int key = (int)current2.m_itemId;
				if (!dictionary2.ContainsKey(key))
				{
					dictionary2.Add(key, new List<int>());
				}
				dictionary2[key].Add((int)current2.m_creditCurrencyValue);
			}
		}
		int num = 0;
		int num2 = 0;
		foreach (KeyValuePair<ulong, StaticDataReadOnly.StaticPackageData> current3 in staticDataReader.PackageMap)
		{
			ulong key2 = current3.Key;
			StaticDataReadOnly.StaticPackageData value = current3.Value;
			if (value.m_state == EPackageState.Promoted)
			{
				foreach (StaticDataReadOnly.StaticPackageItemData current4 in value.m_packageItemList)
				{
					if (current4.m_classificationType != EClassification.LootBox)
					{
						StaticDataReadOnly.StaticItemBase staticItemBase;
						if (!staticDataReader.ItemMap.TryGetValue(current4.m_staticId, out staticItemBase))
						{
							error.AppendFormat("\tPackage Referring to Missing ItemId {0}\n", current4.m_staticId);
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
							if (current4.m_currencyCost != 0u && value.m_currencyType == ECurrencyType.Soft)
							{
								int key3 = (int)staticItemBase.m_ID;
								if (dictionary2.ContainsKey(key3))
								{
									foreach (int current5 in dictionary2[key3])
									{
										if ((long)current5 != (long)((ulong)(current4.m_currencyCost / 5u)))
										{
											error.AppendFormat("\tIncorrectly priced item loot box refund: {0} cost {1} is not 5x refund {2}\n", staticItemBase.m_friendlyName, current4.m_currencyCost, current5);
											num2++;
										}
									}
									if (current4.m_creditCurrencyValue != current4.m_currencyCost / 5u)
									{
										error.AppendFormat("\tIncorrectly priced item refund: {0} cost {1} is not 5x refund {2}\n", staticItemBase.m_friendlyName, current4.m_currencyCost, current4.m_creditCurrencyValue);
										num2++;
									}
									if ((long)dictionary[staticItemBase.m_rarity] != (long)((ulong)current4.m_currencyCost))
									{
										error.AppendFormat("\tIncorrectly priced item: {0} cost {1} does not match expected cost for rarity {2}\n", staticItemBase.m_friendlyName, current4.m_currencyCost, staticItemBase.m_rarity);
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
}
