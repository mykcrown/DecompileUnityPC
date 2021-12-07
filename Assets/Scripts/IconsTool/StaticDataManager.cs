// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using System.Collections.Generic;
using System.IO;
using Utils;

namespace IconsTool
{
	public class StaticDataManager
	{
		private StaticData m_staticData = new StaticData();

		private Dictionary<ulong, StaticItemBase> m_itemMap = new Dictionary<ulong, StaticItemBase>();

		private Dictionary<ulong, StaticPackageData> m_packageMap = new Dictionary<ulong, StaticPackageData>();

		private Dictionary<ulong, StaticLootBoxData> m_lootBoxMap = new Dictionary<ulong, StaticLootBoxData>();

		private bool m_isLoaded;

		private ulong m_nextItemId = 1uL;

		private ulong m_nextPackageId = 1uL;

		private ulong m_nextLootBoxId = 1uL;

		private string m_fileName = string.Empty;

		private readonly ulong m_magic = 192837465uL;

		public static readonly ulong skConfigVersion = 1003uL;

		public string FileName
		{
			get
			{
				return this.m_fileName;
			}
			set
			{
			}
		}

		public Dictionary<ulong, StaticItemBase> ItemMap
		{
			get
			{
				return this.m_itemMap;
			}
			set
			{
			}
		}

		public Dictionary<ulong, StaticPackageData> PackageMap
		{
			get
			{
				return this.m_packageMap;
			}
			set
			{
			}
		}

		public Dictionary<ulong, StaticLootBoxData> LootBoxMap
		{
			get
			{
				return this.m_lootBoxMap;
			}
			set
			{
			}
		}

		public bool IsLoaded
		{
			get
			{
				return this.m_isLoaded;
			}
			set
			{
			}
		}

		public bool LoadFromFile(string filePath)
		{
			bool result;
			using (StreamReader streamReader = new StreamReader(filePath))
			{
				string xmlText = streamReader.ReadToEnd();
				bool flag = this.LoadFromText(xmlText, true);
				this.m_fileName = filePath;
				result = flag;
			}
			return result;
		}

		public bool LoadFromText(string xmlText, bool requireChecksum = true)
		{
			if (this.m_isLoaded)
			{
				return false;
			}
			this.m_staticData = XmlSerialization.ReadFromXmlText<StaticData>(xmlText);
			if (this.m_staticData == null)
			{
				return false;
			}
			foreach (StaticItemBase current in this.m_staticData.m_items.m_itemSkinList)
			{
				this.m_itemMap.Add(current.m_ID, current);
			}
			foreach (StaticItemBase current2 in this.m_staticData.m_items.m_itemEmoteList)
			{
				this.m_itemMap.Add(current2.m_ID, current2);
			}
			foreach (StaticItemBase current3 in this.m_staticData.m_items.m_itemHologramList)
			{
				this.m_itemMap.Add(current3.m_ID, current3);
			}
			foreach (StaticItemBase current4 in this.m_staticData.m_items.m_itemVoiceLineList)
			{
				this.m_itemMap.Add(current4.m_ID, current4);
			}
			foreach (StaticItemBase current5 in this.m_staticData.m_items.m_itemVictoryPoseList)
			{
				this.m_itemMap.Add(current5.m_ID, current5);
			}
			foreach (StaticItemBase current6 in this.m_staticData.m_items.m_itemRespawnPlatformList)
			{
				this.m_itemMap.Add(current6.m_ID, current6);
			}
			foreach (StaticItemBase current7 in this.m_staticData.m_items.m_itemNetsukeList)
			{
				this.m_itemMap.Add(current7.m_ID, current7);
			}
			foreach (StaticItemBase current8 in this.m_staticData.m_items.m_itemTokenList)
			{
				this.m_itemMap.Add(current8.m_ID, current8);
			}
			foreach (StaticItemBase current9 in this.m_staticData.m_items.m_itemPlayerImageList)
			{
				this.m_itemMap.Add(current9.m_ID, current9);
			}
			foreach (StaticItemBase current10 in this.m_staticData.m_items.m_itemBlastZoneList)
			{
				this.m_itemMap.Add(current10.m_ID, current10);
			}
			foreach (StaticItemBase current11 in this.m_staticData.m_items.m_itemCharacterList)
			{
				this.m_itemMap.Add(current11.m_ID, current11);
			}
			foreach (StaticItemBase current12 in this.m_staticData.m_items.m_itemPremiumAccountList)
			{
				this.m_itemMap.Add(current12.m_ID, current12);
			}
			foreach (StaticItemBase current13 in this.m_staticData.m_items.m_itemUnlockTokenList)
			{
				this.m_itemMap.Add(current13.m_ID, current13);
			}
			foreach (StaticItemBase current14 in this.m_staticData.m_items.m_itemCurrencyList)
			{
				this.m_itemMap.Add(current14.m_ID, current14);
			}
			foreach (KeyValuePair<ulong, StaticItemBase> current15 in this.m_itemMap)
			{
				current15.Value.m_itemTypeAsInt = current15.Value.m_itemTypeAsInt;
				current15.Value.m_rarityAsInt = current15.Value.m_rarityAsInt;
				if (current15.Value.m_ID > this.m_nextItemId)
				{
					this.m_nextItemId = current15.Value.m_ID;
				}
			}
			foreach (StaticPackageData current16 in this.m_staticData.m_packages.m_packageList)
			{
				this.m_packageMap.Add(current16.m_ID, current16);
				if (current16.m_ID > this.m_nextPackageId)
				{
					this.m_nextPackageId = current16.m_ID;
				}
			}
			foreach (StaticLootBoxData current17 in this.m_staticData.m_lootBoxes.m_lootBoxList)
			{
				this.m_lootBoxMap.Add(current17.m_ID, current17);
				if (current17.m_ID > this.m_nextLootBoxId)
				{
					this.m_nextLootBoxId = current17.m_ID;
				}
			}
			this.m_isLoaded = true;
			this.m_fileName = string.Empty;
			if (requireChecksum)
			{
				ulong num = this.CalcCheckSum();
				if (num != this.m_staticData.m_checksum)
				{
					this.m_itemMap.Clear();
					this.m_packageMap.Clear();
					this.m_staticData = null;
					return false;
				}
			}
			return true;
		}

		private ulong CalcCheckSum()
		{
			ulong num = this.m_staticData.m_version;
			foreach (KeyValuePair<ulong, StaticItemBase> current in this.m_itemMap)
			{
				num += current.Value.m_ID + (ulong)current.Value.m_version;
			}
			foreach (KeyValuePair<ulong, StaticPackageData> current2 in this.m_packageMap)
			{
				num += current2.Value.m_ID + (ulong)current2.Value.m_version;
			}
			foreach (KeyValuePair<ulong, StaticLootBoxData> current3 in this.m_lootBoxMap)
			{
				num += current3.Value.m_ID + (ulong)current3.Value.m_version;
			}
			num ^= this.m_magic;
			return num;
		}

		public void Save()
		{
			this.SaveTo(this.m_fileName);
		}

		public void SaveTo(string fileName)
		{
			this.m_staticData.m_checksum = this.CalcCheckSum();
			XmlSerialization.WriteToXmlFile<StaticData>(fileName, this.m_staticData, false);
		}

		public StaticItemBase CreateItem(EItemType type, string friendlyName)
		{
			StaticItemBase staticItemBase;
			switch (type)
			{
			case EItemType.Skin:
				staticItemBase = new StaticItemSkin();
				this.m_staticData.m_items.m_itemSkinList.Add(staticItemBase as StaticItemSkin);
				break;
			case EItemType.Emote:
				staticItemBase = new StaticItemEmote();
				this.m_staticData.m_items.m_itemEmoteList.Add(staticItemBase as StaticItemEmote);
				break;
			case EItemType.Hologram:
				staticItemBase = new StaticItemHologram();
				this.m_staticData.m_items.m_itemHologramList.Add(staticItemBase as StaticItemHologram);
				break;
			case EItemType.VoiceLine:
				staticItemBase = new StaticItemVoiceLine();
				this.m_staticData.m_items.m_itemVoiceLineList.Add(staticItemBase as StaticItemVoiceLine);
				break;
			case EItemType.VictoryPose:
				staticItemBase = new StaticItemVictoryPose();
				this.m_staticData.m_items.m_itemVictoryPoseList.Add(staticItemBase as StaticItemVictoryPose);
				break;
			case EItemType.RespawnPlatform:
				staticItemBase = new StaticItemRespawnPlatform();
				this.m_staticData.m_items.m_itemRespawnPlatformList.Add(staticItemBase as StaticItemRespawnPlatform);
				break;
			case EItemType.Netsuke:
				staticItemBase = new StaticItemNetsuke();
				this.m_staticData.m_items.m_itemNetsukeList.Add(staticItemBase as StaticItemNetsuke);
				break;
			case EItemType.Token:
				staticItemBase = new StaticItemToken();
				this.m_staticData.m_items.m_itemTokenList.Add(staticItemBase as StaticItemToken);
				break;
			case EItemType.PlayerImage:
				staticItemBase = new StaticItemPlayerImage();
				this.m_staticData.m_items.m_itemPlayerImageList.Add(staticItemBase as StaticItemPlayerImage);
				break;
			case EItemType.BlastZone:
				staticItemBase = new StaticItemBlastZone();
				this.m_staticData.m_items.m_itemBlastZoneList.Add(staticItemBase as StaticItemBlastZone);
				break;
			case EItemType.Character:
				staticItemBase = new StaticItemCharacter();
				this.m_staticData.m_items.m_itemCharacterList.Add(staticItemBase as StaticItemCharacter);
				break;
			case EItemType.PremiumAccount:
				staticItemBase = new StaticItemPremiumAccount();
				this.m_staticData.m_items.m_itemPremiumAccountList.Add(staticItemBase as StaticItemPremiumAccount);
				break;
			case EItemType.UnlockToken:
				staticItemBase = new StaticItemUnlockToken();
				this.m_staticData.m_items.m_itemUnlockTokenList.Add(staticItemBase as StaticItemUnlockToken);
				break;
			case EItemType.Currency:
				staticItemBase = new StaticItemCurrency();
				this.m_staticData.m_items.m_itemCurrencyList.Add(staticItemBase as StaticItemCurrency);
				break;
			default:
				return null;
			}
			staticItemBase.m_ID = (this.m_nextItemId += 1uL);
			staticItemBase.m_itemType = type;
			staticItemBase.m_itemTypeAsInt = (uint)type;
			staticItemBase.m_rarityAsInt = 0u;
			staticItemBase.m_friendlyName = friendlyName;
			this.m_itemMap.Add(staticItemBase.m_ID, staticItemBase);
			return staticItemBase;
		}

		public StaticPackageData CreatePackage(string friendlyName)
		{
			StaticPackageData staticPackageData = new StaticPackageData();
			staticPackageData.m_ID = (this.m_nextPackageId += 1uL);
			staticPackageData.m_friendlyName = friendlyName;
			this.m_staticData.m_packages.m_packageList.Add(staticPackageData);
			this.m_packageMap.Add(staticPackageData.m_ID, staticPackageData);
			return staticPackageData;
		}

		public StaticLootBoxData CreateLootBox(string friendlyName)
		{
			StaticLootBoxData staticLootBoxData = new StaticLootBoxData();
			staticLootBoxData.m_ID = (this.m_nextLootBoxId += 1uL);
			staticLootBoxData.m_friendlyName = friendlyName;
			this.m_staticData.m_lootBoxes.m_lootBoxList.Add(staticLootBoxData);
			this.m_lootBoxMap.Add(staticLootBoxData.m_ID, staticLootBoxData);
			return staticLootBoxData;
		}
	}
}
