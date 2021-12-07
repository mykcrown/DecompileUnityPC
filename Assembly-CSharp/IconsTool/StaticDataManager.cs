using System;
using System.Collections.Generic;
using System.IO;
using IconsServer;
using Utils;

namespace IconsTool
{
	// Token: 0x020006FF RID: 1791
	public class StaticDataManager
	{
		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06002CA7 RID: 11431 RVA: 0x000E5A66 File Offset: 0x000E3E66
		// (set) Token: 0x06002CA8 RID: 11432 RVA: 0x000E5A6E File Offset: 0x000E3E6E
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

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06002CA9 RID: 11433 RVA: 0x000E5A70 File Offset: 0x000E3E70
		// (set) Token: 0x06002CAA RID: 11434 RVA: 0x000E5A78 File Offset: 0x000E3E78
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

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06002CAB RID: 11435 RVA: 0x000E5A7A File Offset: 0x000E3E7A
		// (set) Token: 0x06002CAC RID: 11436 RVA: 0x000E5A82 File Offset: 0x000E3E82
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

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06002CAD RID: 11437 RVA: 0x000E5A84 File Offset: 0x000E3E84
		// (set) Token: 0x06002CAE RID: 11438 RVA: 0x000E5A8C File Offset: 0x000E3E8C
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

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06002CAF RID: 11439 RVA: 0x000E5A8E File Offset: 0x000E3E8E
		// (set) Token: 0x06002CB0 RID: 11440 RVA: 0x000E5A96 File Offset: 0x000E3E96
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

		// Token: 0x06002CB1 RID: 11441 RVA: 0x000E5A98 File Offset: 0x000E3E98
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

		// Token: 0x06002CB2 RID: 11442 RVA: 0x000E5AE8 File Offset: 0x000E3EE8
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
			foreach (StaticItemBase staticItemBase in this.m_staticData.m_items.m_itemSkinList)
			{
				this.m_itemMap.Add(staticItemBase.m_ID, staticItemBase);
			}
			foreach (StaticItemBase staticItemBase2 in this.m_staticData.m_items.m_itemEmoteList)
			{
				this.m_itemMap.Add(staticItemBase2.m_ID, staticItemBase2);
			}
			foreach (StaticItemBase staticItemBase3 in this.m_staticData.m_items.m_itemHologramList)
			{
				this.m_itemMap.Add(staticItemBase3.m_ID, staticItemBase3);
			}
			foreach (StaticItemBase staticItemBase4 in this.m_staticData.m_items.m_itemVoiceLineList)
			{
				this.m_itemMap.Add(staticItemBase4.m_ID, staticItemBase4);
			}
			foreach (StaticItemBase staticItemBase5 in this.m_staticData.m_items.m_itemVictoryPoseList)
			{
				this.m_itemMap.Add(staticItemBase5.m_ID, staticItemBase5);
			}
			foreach (StaticItemBase staticItemBase6 in this.m_staticData.m_items.m_itemRespawnPlatformList)
			{
				this.m_itemMap.Add(staticItemBase6.m_ID, staticItemBase6);
			}
			foreach (StaticItemBase staticItemBase7 in this.m_staticData.m_items.m_itemNetsukeList)
			{
				this.m_itemMap.Add(staticItemBase7.m_ID, staticItemBase7);
			}
			foreach (StaticItemBase staticItemBase8 in this.m_staticData.m_items.m_itemTokenList)
			{
				this.m_itemMap.Add(staticItemBase8.m_ID, staticItemBase8);
			}
			foreach (StaticItemBase staticItemBase9 in this.m_staticData.m_items.m_itemPlayerImageList)
			{
				this.m_itemMap.Add(staticItemBase9.m_ID, staticItemBase9);
			}
			foreach (StaticItemBase staticItemBase10 in this.m_staticData.m_items.m_itemBlastZoneList)
			{
				this.m_itemMap.Add(staticItemBase10.m_ID, staticItemBase10);
			}
			foreach (StaticItemBase staticItemBase11 in this.m_staticData.m_items.m_itemCharacterList)
			{
				this.m_itemMap.Add(staticItemBase11.m_ID, staticItemBase11);
			}
			foreach (StaticItemBase staticItemBase12 in this.m_staticData.m_items.m_itemPremiumAccountList)
			{
				this.m_itemMap.Add(staticItemBase12.m_ID, staticItemBase12);
			}
			foreach (StaticItemBase staticItemBase13 in this.m_staticData.m_items.m_itemUnlockTokenList)
			{
				this.m_itemMap.Add(staticItemBase13.m_ID, staticItemBase13);
			}
			foreach (StaticItemBase staticItemBase14 in this.m_staticData.m_items.m_itemCurrencyList)
			{
				this.m_itemMap.Add(staticItemBase14.m_ID, staticItemBase14);
			}
			foreach (KeyValuePair<ulong, StaticItemBase> keyValuePair in this.m_itemMap)
			{
				keyValuePair.Value.m_itemTypeAsInt = keyValuePair.Value.m_itemTypeAsInt;
				keyValuePair.Value.m_rarityAsInt = keyValuePair.Value.m_rarityAsInt;
				if (keyValuePair.Value.m_ID > this.m_nextItemId)
				{
					this.m_nextItemId = keyValuePair.Value.m_ID;
				}
			}
			foreach (StaticPackageData staticPackageData in this.m_staticData.m_packages.m_packageList)
			{
				this.m_packageMap.Add(staticPackageData.m_ID, staticPackageData);
				if (staticPackageData.m_ID > this.m_nextPackageId)
				{
					this.m_nextPackageId = staticPackageData.m_ID;
				}
			}
			foreach (StaticLootBoxData staticLootBoxData in this.m_staticData.m_lootBoxes.m_lootBoxList)
			{
				this.m_lootBoxMap.Add(staticLootBoxData.m_ID, staticLootBoxData);
				if (staticLootBoxData.m_ID > this.m_nextLootBoxId)
				{
					this.m_nextLootBoxId = staticLootBoxData.m_ID;
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

		// Token: 0x06002CB3 RID: 11443 RVA: 0x000E6288 File Offset: 0x000E4688
		private ulong CalcCheckSum()
		{
			ulong num = this.m_staticData.m_version;
			foreach (KeyValuePair<ulong, StaticItemBase> keyValuePair in this.m_itemMap)
			{
				num += keyValuePair.Value.m_ID + (ulong)keyValuePair.Value.m_version;
			}
			foreach (KeyValuePair<ulong, StaticPackageData> keyValuePair2 in this.m_packageMap)
			{
				num += keyValuePair2.Value.m_ID + (ulong)keyValuePair2.Value.m_version;
			}
			foreach (KeyValuePair<ulong, StaticLootBoxData> keyValuePair3 in this.m_lootBoxMap)
			{
				num += keyValuePair3.Value.m_ID + (ulong)keyValuePair3.Value.m_version;
			}
			num ^= this.m_magic;
			return num;
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x000E63D8 File Offset: 0x000E47D8
		public void Save()
		{
			this.SaveTo(this.m_fileName);
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x000E63E6 File Offset: 0x000E47E6
		public void SaveTo(string fileName)
		{
			this.m_staticData.m_checksum = this.CalcCheckSum();
			XmlSerialization.WriteToXmlFile<StaticData>(fileName, this.m_staticData, false);
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x000E6408 File Offset: 0x000E4808
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
			staticItemBase.m_ID = (this.m_nextItemId += 1UL);
			staticItemBase.m_itemType = type;
			staticItemBase.m_itemTypeAsInt = (uint)type;
			staticItemBase.m_rarityAsInt = 0U;
			staticItemBase.m_friendlyName = friendlyName;
			this.m_itemMap.Add(staticItemBase.m_ID, staticItemBase);
			return staticItemBase;
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x000E66B8 File Offset: 0x000E4AB8
		public StaticPackageData CreatePackage(string friendlyName)
		{
			StaticPackageData staticPackageData = new StaticPackageData();
			staticPackageData.m_ID = (this.m_nextPackageId += 1UL);
			staticPackageData.m_friendlyName = friendlyName;
			this.m_staticData.m_packages.m_packageList.Add(staticPackageData);
			this.m_packageMap.Add(staticPackageData.m_ID, staticPackageData);
			return staticPackageData;
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x000E6714 File Offset: 0x000E4B14
		public StaticLootBoxData CreateLootBox(string friendlyName)
		{
			StaticLootBoxData staticLootBoxData = new StaticLootBoxData();
			staticLootBoxData.m_ID = (this.m_nextLootBoxId += 1UL);
			staticLootBoxData.m_friendlyName = friendlyName;
			this.m_staticData.m_lootBoxes.m_lootBoxList.Add(staticLootBoxData);
			this.m_lootBoxMap.Add(staticLootBoxData.m_ID, staticLootBoxData);
			return staticLootBoxData;
		}

		// Token: 0x04001FB6 RID: 8118
		private StaticData m_staticData = new StaticData();

		// Token: 0x04001FB7 RID: 8119
		private Dictionary<ulong, StaticItemBase> m_itemMap = new Dictionary<ulong, StaticItemBase>();

		// Token: 0x04001FB8 RID: 8120
		private Dictionary<ulong, StaticPackageData> m_packageMap = new Dictionary<ulong, StaticPackageData>();

		// Token: 0x04001FB9 RID: 8121
		private Dictionary<ulong, StaticLootBoxData> m_lootBoxMap = new Dictionary<ulong, StaticLootBoxData>();

		// Token: 0x04001FBA RID: 8122
		private bool m_isLoaded;

		// Token: 0x04001FBB RID: 8123
		private ulong m_nextItemId = 1UL;

		// Token: 0x04001FBC RID: 8124
		private ulong m_nextPackageId = 1UL;

		// Token: 0x04001FBD RID: 8125
		private ulong m_nextLootBoxId = 1UL;

		// Token: 0x04001FBE RID: 8126
		private string m_fileName = string.Empty;

		// Token: 0x04001FBF RID: 8127
		private readonly ulong m_magic = 192837465UL;

		// Token: 0x04001FC0 RID: 8128
		public static readonly ulong skConfigVersion = 1003UL;
	}
}
