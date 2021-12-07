using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Commerce;
using IconsServer;
using UnityEngine;
using Utils;

namespace IconsTool
{
	// Token: 0x02000700 RID: 1792
	public class StaticDataReadOnly
	{
		// Token: 0x02000701 RID: 1793
		public class StaticDataBase
		{
			// Token: 0x06002CBC RID: 11452 RVA: 0x000E67A2 File Offset: 0x000E4BA2
			public override string ToString()
			{
				return this.m_friendlyName;
			}

			// Token: 0x04001FC1 RID: 8129
			[XmlElement("ID")]
			public ulong m_ID;

			// Token: 0x04001FC2 RID: 8130
			[XmlElement("Version")]
			public uint m_version;

			// Token: 0x04001FC3 RID: 8131
			[XmlElement("FriendlyName")]
			public string m_friendlyName = "StaticItemBase";

			// Token: 0x04001FC4 RID: 8132
			[XmlElement("Description")]
			public string m_description = string.Empty;
		}

		// Token: 0x02000702 RID: 1794
		public class StaticItemBase : StaticDataReadOnly.StaticDataBase
		{
			// Token: 0x17000AFC RID: 2812
			// (get) Token: 0x06002CBE RID: 11454 RVA: 0x000E67BD File Offset: 0x000E4BBD
			// (set) Token: 0x06002CBF RID: 11455 RVA: 0x000E67C5 File Offset: 0x000E4BC5
			[XmlIgnore]
			public EItemType m_itemType
			{
				get
				{
					return (EItemType)this.m_itemTypeAsInt;
				}
				set
				{
					this.m_rarityAsInt = (uint)value;
				}
			}

			// Token: 0x17000AFD RID: 2813
			// (get) Token: 0x06002CC0 RID: 11456 RVA: 0x000E67CE File Offset: 0x000E4BCE
			// (set) Token: 0x06002CC1 RID: 11457 RVA: 0x000E67D6 File Offset: 0x000E4BD6
			[XmlIgnore]
			public EItemRarity m_rarity
			{
				get
				{
					return (EItemRarity)this.m_rarityAsInt;
				}
				set
				{
					this.m_rarityAsInt = (uint)value;
				}
			}

			// Token: 0x06002CC2 RID: 11458 RVA: 0x000E67DF File Offset: 0x000E4BDF
			public override string ToString()
			{
				return this.m_friendlyName;
			}

			// Token: 0x04001FC5 RID: 8133
			[XmlElement("Type")]
			public uint m_itemTypeAsInt;

			// Token: 0x04001FC6 RID: 8134
			[XmlElement("Rarity")]
			public uint m_rarityAsInt;

			// Token: 0x04001FC7 RID: 8135
			[XmlElement("Flags")]
			public ulong m_flags;

			// Token: 0x04001FC8 RID: 8136
			[XmlElement("Asset")]
			public string m_asset = string.Empty;

			// Token: 0x04001FC9 RID: 8137
			[XmlElement("Enabled")]
			public bool m_enabled;
		}

		// Token: 0x02000703 RID: 1795
		public class StaticItemCharMask : StaticDataReadOnly.StaticItemBase
		{
			// Token: 0x04001FCA RID: 8138
			[XmlElement("CharacterTypeBitMask")]
			public ulong m_characterTypeBitMask;
		}

		// Token: 0x02000704 RID: 1796
		public class StaticItemSkin : StaticDataReadOnly.StaticItemCharMask
		{
			// Token: 0x06002CC4 RID: 11460 RVA: 0x000E67EF File Offset: 0x000E4BEF
			internal StaticItemSkin()
			{
				base.m_itemType = EItemType.Skin;
			}
		}

		// Token: 0x02000705 RID: 1797
		public class StaticItemToken : StaticDataReadOnly.StaticItemBase
		{
			// Token: 0x06002CC5 RID: 11461 RVA: 0x000E67FE File Offset: 0x000E4BFE
			internal StaticItemToken()
			{
				base.m_itemType = EItemType.Token;
			}
		}

		// Token: 0x02000706 RID: 1798
		public class StaticItemHologram : StaticDataReadOnly.StaticItemCharMask
		{
			// Token: 0x06002CC6 RID: 11462 RVA: 0x000E680D File Offset: 0x000E4C0D
			internal StaticItemHologram()
			{
				base.m_itemType = EItemType.Hologram;
			}
		}

		// Token: 0x02000707 RID: 1799
		public class StaticItemBlastZone : StaticDataReadOnly.StaticItemBase
		{
			// Token: 0x06002CC7 RID: 11463 RVA: 0x000E681C File Offset: 0x000E4C1C
			internal StaticItemBlastZone()
			{
				base.m_itemType = EItemType.BlastZone;
			}
		}

		// Token: 0x02000708 RID: 1800
		public class StaticItemEmote : StaticDataReadOnly.StaticItemCharMask
		{
			// Token: 0x06002CC8 RID: 11464 RVA: 0x000E682C File Offset: 0x000E4C2C
			internal StaticItemEmote()
			{
				base.m_itemType = EItemType.Emote;
			}
		}

		// Token: 0x02000709 RID: 1801
		public class StaticItemNetsuke : StaticDataReadOnly.StaticItemBase
		{
			// Token: 0x06002CC9 RID: 11465 RVA: 0x000E683B File Offset: 0x000E4C3B
			internal StaticItemNetsuke()
			{
				base.m_itemType = EItemType.Netsuke;
			}
		}

		// Token: 0x0200070A RID: 1802
		public class StaticItemPlayerImage : StaticDataReadOnly.StaticItemBase
		{
			// Token: 0x06002CCA RID: 11466 RVA: 0x000E684A File Offset: 0x000E4C4A
			internal StaticItemPlayerImage()
			{
				base.m_itemType = EItemType.PlayerImage;
			}
		}

		// Token: 0x0200070B RID: 1803
		public class StaticItemRespawnPlatform : StaticDataReadOnly.StaticItemCharMask
		{
			// Token: 0x06002CCB RID: 11467 RVA: 0x000E6859 File Offset: 0x000E4C59
			internal StaticItemRespawnPlatform()
			{
				base.m_itemType = EItemType.RespawnPlatform;
			}
		}

		// Token: 0x0200070C RID: 1804
		public class StaticItemVictoryPose : StaticDataReadOnly.StaticItemCharMask
		{
			// Token: 0x06002CCC RID: 11468 RVA: 0x000E6868 File Offset: 0x000E4C68
			internal StaticItemVictoryPose()
			{
				base.m_itemType = EItemType.VictoryPose;
			}
		}

		// Token: 0x0200070D RID: 1805
		public class StaticItemVoiceLine : StaticDataReadOnly.StaticItemCharMask
		{
			// Token: 0x06002CCD RID: 11469 RVA: 0x000E6877 File Offset: 0x000E4C77
			internal StaticItemVoiceLine()
			{
				base.m_itemType = EItemType.VoiceLine;
			}
		}

		// Token: 0x0200070E RID: 1806
		public class StaticItemCharacter : StaticDataReadOnly.StaticItemBase
		{
			// Token: 0x06002CCE RID: 11470 RVA: 0x000E6886 File Offset: 0x000E4C86
			internal StaticItemCharacter()
			{
				base.m_itemType = EItemType.Character;
			}

			// Token: 0x17000AFE RID: 2814
			// (get) Token: 0x06002CCF RID: 11471 RVA: 0x000E6896 File Offset: 0x000E4C96
			// (set) Token: 0x06002CD0 RID: 11472 RVA: 0x000E689E File Offset: 0x000E4C9E
			[XmlIgnore]
			public ECharacterType m_characterType
			{
				get
				{
					return (ECharacterType)this.m_characterTypeAsInt;
				}
				set
				{
					this.m_characterTypeAsInt = (uint)value;
				}
			}

			// Token: 0x04001FCB RID: 8139
			[XmlElement("CharacterType")]
			public uint m_characterTypeAsInt;
		}

		// Token: 0x0200070F RID: 1807
		public class StaticItemPremiumAccount : StaticDataReadOnly.StaticItemBase
		{
			// Token: 0x06002CD1 RID: 11473 RVA: 0x000E68A7 File Offset: 0x000E4CA7
			internal StaticItemPremiumAccount()
			{
				base.m_itemType = EItemType.PremiumAccount;
			}

			// Token: 0x17000AFF RID: 2815
			// (get) Token: 0x06002CD2 RID: 11474 RVA: 0x000E68B7 File Offset: 0x000E4CB7
			// (set) Token: 0x06002CD3 RID: 11475 RVA: 0x000E68BF File Offset: 0x000E4CBF
			[XmlIgnore]
			public EPremiumAccount m_characterType
			{
				get
				{
					return (EPremiumAccount)this.m_premiumTypeAsInt;
				}
				set
				{
					this.m_premiumTypeAsInt = (uint)value;
				}
			}

			// Token: 0x04001FCC RID: 8140
			[XmlElement("PremiumType")]
			public uint m_premiumTypeAsInt;
		}

		// Token: 0x02000710 RID: 1808
		public class StaticItemUnlockToken : StaticDataReadOnly.StaticItemBase
		{
			// Token: 0x06002CD4 RID: 11476 RVA: 0x000E68C8 File Offset: 0x000E4CC8
			internal StaticItemUnlockToken()
			{
				base.m_itemType = EItemType.UnlockToken;
			}
		}

		// Token: 0x02000711 RID: 1809
		public class StaticItemCurrency : StaticDataReadOnly.StaticItemBase
		{
			// Token: 0x06002CD5 RID: 11477 RVA: 0x000E68D8 File Offset: 0x000E4CD8
			internal StaticItemCurrency()
			{
				base.m_itemType = EItemType.Currency;
			}

			// Token: 0x17000B00 RID: 2816
			// (get) Token: 0x06002CD6 RID: 11478 RVA: 0x000E68E8 File Offset: 0x000E4CE8
			// (set) Token: 0x06002CD7 RID: 11479 RVA: 0x000E68F0 File Offset: 0x000E4CF0
			[XmlIgnore]
			public ECurrencyType m_currencyType
			{
				get
				{
					return (ECurrencyType)this.m_currencyTypeAsInt;
				}
				set
				{
					this.m_currencyTypeAsInt = (uint)value;
				}
			}

			// Token: 0x04001FCD RID: 8141
			[XmlElement("CurrencyType")]
			public uint m_currencyTypeAsInt;

			// Token: 0x04001FCE RID: 8142
			[XmlElement("CurrencyAmount")]
			public ulong m_currencyAmount;
		}

		// Token: 0x02000712 RID: 1810
		public class StaticDataItemList
		{
			// Token: 0x04001FCF RID: 8143
			[XmlElement("StaticItemSkin")]
			public List<StaticDataReadOnly.StaticItemSkin> m_itemSkinList = new List<StaticDataReadOnly.StaticItemSkin>();

			// Token: 0x04001FD0 RID: 8144
			[XmlElement("StaticItemEmote")]
			public List<StaticDataReadOnly.StaticItemEmote> m_itemEmoteList = new List<StaticDataReadOnly.StaticItemEmote>();

			// Token: 0x04001FD1 RID: 8145
			[XmlElement("StaticItemHologram")]
			public List<StaticDataReadOnly.StaticItemHologram> m_itemHologramList = new List<StaticDataReadOnly.StaticItemHologram>();

			// Token: 0x04001FD2 RID: 8146
			[XmlElement("StaticItemVoiceLine")]
			public List<StaticDataReadOnly.StaticItemVoiceLine> m_itemVoiceLineList = new List<StaticDataReadOnly.StaticItemVoiceLine>();

			// Token: 0x04001FD3 RID: 8147
			[XmlElement("StaticItemVictoryPose")]
			public List<StaticDataReadOnly.StaticItemVictoryPose> m_itemVictoryPoseList = new List<StaticDataReadOnly.StaticItemVictoryPose>();

			// Token: 0x04001FD4 RID: 8148
			[XmlElement("StaticItemRespawnPlatform")]
			public List<StaticDataReadOnly.StaticItemRespawnPlatform> m_itemRespawnPlatformList = new List<StaticDataReadOnly.StaticItemRespawnPlatform>();

			// Token: 0x04001FD5 RID: 8149
			[XmlElement("StaticItemNetsuke")]
			public List<StaticDataReadOnly.StaticItemNetsuke> m_itemNetsukeList = new List<StaticDataReadOnly.StaticItemNetsuke>();

			// Token: 0x04001FD6 RID: 8150
			[XmlElement("StaticItemToken")]
			public List<StaticDataReadOnly.StaticItemToken> m_itemTokenList = new List<StaticDataReadOnly.StaticItemToken>();

			// Token: 0x04001FD7 RID: 8151
			[XmlElement("StaticItemPlayerImage")]
			public List<StaticDataReadOnly.StaticItemPlayerImage> m_itemPlayerImageList = new List<StaticDataReadOnly.StaticItemPlayerImage>();

			// Token: 0x04001FD8 RID: 8152
			[XmlElement("StaticItemBlastZone")]
			public List<StaticDataReadOnly.StaticItemBlastZone> m_itemBlastZoneList = new List<StaticDataReadOnly.StaticItemBlastZone>();

			// Token: 0x04001FD9 RID: 8153
			[XmlElement("StaticItemCharacter")]
			public List<StaticDataReadOnly.StaticItemCharacter> m_itemCharacterList = new List<StaticDataReadOnly.StaticItemCharacter>();

			// Token: 0x04001FDA RID: 8154
			[XmlElement("StaticItemPremiumAccount")]
			public List<StaticDataReadOnly.StaticItemPremiumAccount> m_itemPremiumAccountList = new List<StaticDataReadOnly.StaticItemPremiumAccount>();

			// Token: 0x04001FDB RID: 8155
			[XmlElement("StaticItemUnlockToken")]
			public List<StaticDataReadOnly.StaticItemUnlockToken> m_itemUnlockTokenList = new List<StaticDataReadOnly.StaticItemUnlockToken>();

			// Token: 0x04001FDC RID: 8156
			[XmlElement("StaticItemCurrency")]
			public List<StaticDataReadOnly.StaticItemCurrency> m_itemCurrencyList = new List<StaticDataReadOnly.StaticItemCurrency>();
		}

		// Token: 0x02000713 RID: 1811
		public class StaticPackageItemData
		{
			// Token: 0x17000B01 RID: 2817
			// (get) Token: 0x06002CDA RID: 11482 RVA: 0x000E69B1 File Offset: 0x000E4DB1
			// (set) Token: 0x06002CDB RID: 11483 RVA: 0x000E69B9 File Offset: 0x000E4DB9
			[XmlIgnore]
			public EClassification m_classificationType
			{
				get
				{
					return (EClassification)this.m_classificationTypeAsInt;
				}
				set
				{
					this.m_classificationTypeAsInt = (uint)value;
				}
			}

			// Token: 0x06002CDC RID: 11484 RVA: 0x000E69C4 File Offset: 0x000E4DC4
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"Type:",
					this.m_classificationType,
					"StaticId:",
					this.m_staticId,
					" Cost:",
					this.m_currencyCost.ToString()
				});
			}

			// Token: 0x04001FDD RID: 8157
			[XmlElement("ClassificationType")]
			public uint m_classificationTypeAsInt;

			// Token: 0x04001FDE RID: 8158
			[XmlElement("StaticId")]
			public ulong m_staticId;

			// Token: 0x04001FDF RID: 8159
			[XmlElement("CurrencyCost")]
			public uint m_currencyCost;

			// Token: 0x04001FE0 RID: 8160
			[XmlElement("CreditCurrencyValue")]
			public uint m_creditCurrencyValue;
		}

		// Token: 0x02000714 RID: 1812
		public class StaticPackageData : StaticDataReadOnly.StaticDataBase
		{
			// Token: 0x17000B02 RID: 2818
			// (get) Token: 0x06002CDE RID: 11486 RVA: 0x000E6A51 File Offset: 0x000E4E51
			// (set) Token: 0x06002CDF RID: 11487 RVA: 0x000E6A59 File Offset: 0x000E4E59
			[XmlIgnore]
			public ECurrencyType m_currencyType
			{
				get
				{
					return (ECurrencyType)this.m_currencyTypeAsInt;
				}
				set
				{
					this.m_currencyTypeAsInt = (uint)value;
				}
			}

			// Token: 0x17000B03 RID: 2819
			// (get) Token: 0x06002CE0 RID: 11488 RVA: 0x000E6A62 File Offset: 0x000E4E62
			// (set) Token: 0x06002CE1 RID: 11489 RVA: 0x000E6A6A File Offset: 0x000E4E6A
			[XmlIgnore]
			public EPackageState m_state
			{
				get
				{
					return (EPackageState)this.m_stateAsInt;
				}
				set
				{
					this.m_stateAsInt = (uint)value;
				}
			}

			// Token: 0x04001FE1 RID: 8161
			[XmlElement("CurrencyType")]
			public uint m_currencyTypeAsInt;

			// Token: 0x04001FE2 RID: 8162
			[XmlElement("State")]
			public uint m_stateAsInt;

			// Token: 0x04001FE3 RID: 8163
			[XmlElement("StateTimeStamp")]
			public DateTime m_stateTimeStamp = default(DateTime);

			// Token: 0x04001FE4 RID: 8164
			[XmlElement("StaticPackageItems")]
			public List<StaticDataReadOnly.StaticPackageItemData> m_packageItemList = new List<StaticDataReadOnly.StaticPackageItemData>();
		}

		// Token: 0x02000715 RID: 1813
		public class StaticPackageDataList
		{
			// Token: 0x04001FE5 RID: 8165
			[XmlElement("StaticPackage")]
			public List<StaticDataReadOnly.StaticPackageData> m_packageList = new List<StaticDataReadOnly.StaticPackageData>();
		}

		// Token: 0x02000716 RID: 1814
		public class StaticLootBoxItemData
		{
			// Token: 0x17000B04 RID: 2820
			// (get) Token: 0x06002CE4 RID: 11492 RVA: 0x000E6A8E File Offset: 0x000E4E8E
			// (set) Token: 0x06002CE5 RID: 11493 RVA: 0x000E6A96 File Offset: 0x000E4E96
			[XmlIgnore]
			public ECurrencyType m_creditCurrencyType
			{
				get
				{
					return (ECurrencyType)this.m_currencyTypeAsInt;
				}
				set
				{
					this.m_currencyTypeAsInt = (uint)value;
				}
			}

			// Token: 0x04001FE6 RID: 8166
			[XmlElement("ItemId")]
			public ulong m_itemId;

			// Token: 0x04001FE7 RID: 8167
			[XmlElement("CurrencyType")]
			public uint m_currencyTypeAsInt;

			// Token: 0x04001FE8 RID: 8168
			[XmlElement("CreditValue")]
			public ulong m_creditCurrencyValue;

			// Token: 0x04001FE9 RID: 8169
			[XmlElement("Flags")]
			public ulong m_flags;
		}

		// Token: 0x02000717 RID: 1815
		public class StaticLootBoxData : StaticDataReadOnly.StaticDataBase
		{
			// Token: 0x17000B05 RID: 2821
			// (get) Token: 0x06002CE7 RID: 11495 RVA: 0x000E6AB2 File Offset: 0x000E4EB2
			// (set) Token: 0x06002CE8 RID: 11496 RVA: 0x000E6ABA File Offset: 0x000E4EBA
			[XmlIgnore]
			public ELootBoxType m_lootBoxType
			{
				get
				{
					return (ELootBoxType)this.m_lootBoxTypeAsInt;
				}
				set
				{
					this.m_lootBoxTypeAsInt = (uint)value;
				}
			}

			// Token: 0x04001FEA RID: 8170
			[XmlElement("LootBoxType")]
			public uint m_lootBoxTypeAsInt;

			// Token: 0x04001FEB RID: 8171
			[XmlElement("StaticLootBoxItems")]
			public List<StaticDataReadOnly.StaticLootBoxItemData> m_lootBoxItemList = new List<StaticDataReadOnly.StaticLootBoxItemData>();
		}

		// Token: 0x02000718 RID: 1816
		public class StaticLootBoxDataList
		{
			// Token: 0x04001FEC RID: 8172
			[XmlElement("StaticLootBox")]
			public List<StaticDataReadOnly.StaticLootBoxData> m_lootBoxList = new List<StaticDataReadOnly.StaticLootBoxData>();
		}

		// Token: 0x02000719 RID: 1817
		public class StaticData
		{
			// Token: 0x04001FED RID: 8173
			[XmlElement("Version")]
			public ulong m_version = (ulong)StaticDataReadOnly.StaticDataReader.skConfigVersion;

			// Token: 0x04001FEE RID: 8174
			[XmlElement("Checksum")]
			public ulong m_checksum;

			// Token: 0x04001FEF RID: 8175
			[XmlElement("Items")]
			public StaticDataReadOnly.StaticDataItemList m_items = new StaticDataReadOnly.StaticDataItemList();

			// Token: 0x04001FF0 RID: 8176
			[XmlElement("Packages")]
			public StaticDataReadOnly.StaticPackageDataList m_packages = new StaticDataReadOnly.StaticPackageDataList();

			// Token: 0x04001FF1 RID: 8177
			[XmlElement("LootBoxes")]
			public StaticDataReadOnly.StaticLootBoxDataList m_lootBoxes = new StaticDataReadOnly.StaticLootBoxDataList();
		}

		// Token: 0x0200071A RID: 1818
		public struct ItemId
		{
			// Token: 0x06002CEB RID: 11499 RVA: 0x000E6B0B File Offset: 0x000E4F0B
			public ItemId(ulong id)
			{
				this.id = id;
			}

			// Token: 0x06002CEC RID: 11500 RVA: 0x000E6B14 File Offset: 0x000E4F14
			public bool IsNull()
			{
				return this.id == 0UL;
			}

			// Token: 0x06002CED RID: 11501 RVA: 0x000E6B20 File Offset: 0x000E4F20
			public override bool Equals(object other)
			{
				return other is StaticDataReadOnly.ItemId && (StaticDataReadOnly.ItemId)other == this;
			}

			// Token: 0x06002CEE RID: 11502 RVA: 0x000E6B40 File Offset: 0x000E4F40
			public override int GetHashCode()
			{
				int num = 17;
				return num * 7 + ((this.id != 0UL) ? this.id.GetHashCode() : 0);
			}

			// Token: 0x06002CEF RID: 11503 RVA: 0x000E6B7A File Offset: 0x000E4F7A
			public static bool operator ==(StaticDataReadOnly.ItemId a, StaticDataReadOnly.ItemId b)
			{
				return a.id == b.id;
			}

			// Token: 0x06002CF0 RID: 11504 RVA: 0x000E6B8C File Offset: 0x000E4F8C
			public static bool operator !=(StaticDataReadOnly.ItemId a, StaticDataReadOnly.ItemId b)
			{
				return !(a == b);
			}

			// Token: 0x04001FF2 RID: 8178
			public ulong id;
		}

		// Token: 0x0200071B RID: 1819
		public class StaticDataReader
		{
			// Token: 0x17000B06 RID: 2822
			// (get) Token: 0x06002CF2 RID: 11506 RVA: 0x000E6BD8 File Offset: 0x000E4FD8
			public ulong StaticChecksum
			{
				get
				{
					return this.m_staticData.m_checksum;
				}
			}

			// Token: 0x17000B07 RID: 2823
			// (get) Token: 0x06002CF3 RID: 11507 RVA: 0x000E6BE5 File Offset: 0x000E4FE5
			// (set) Token: 0x06002CF4 RID: 11508 RVA: 0x000E6BED File Offset: 0x000E4FED
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

			// Token: 0x17000B08 RID: 2824
			// (get) Token: 0x06002CF5 RID: 11509 RVA: 0x000E6BEF File Offset: 0x000E4FEF
			// (set) Token: 0x06002CF6 RID: 11510 RVA: 0x000E6BF7 File Offset: 0x000E4FF7
			public Dictionary<ulong, StaticDataReadOnly.StaticItemBase> ItemMap
			{
				get
				{
					return this.m_itemMap;
				}
				set
				{
				}
			}

			// Token: 0x17000B09 RID: 2825
			// (get) Token: 0x06002CF7 RID: 11511 RVA: 0x000E6BF9 File Offset: 0x000E4FF9
			// (set) Token: 0x06002CF8 RID: 11512 RVA: 0x000E6C01 File Offset: 0x000E5001
			public Dictionary<ulong, StaticDataReadOnly.StaticPackageData> PackageMap
			{
				get
				{
					return this.m_packageMap;
				}
				set
				{
				}
			}

			// Token: 0x17000B0A RID: 2826
			// (get) Token: 0x06002CF9 RID: 11513 RVA: 0x000E6C03 File Offset: 0x000E5003
			// (set) Token: 0x06002CFA RID: 11514 RVA: 0x000E6C0B File Offset: 0x000E500B
			public Dictionary<ulong, StaticDataReadOnly.StaticLootBoxData> LootBoxMap
			{
				get
				{
					return this.m_lootBoxMap;
				}
				set
				{
				}
			}

			// Token: 0x17000B0B RID: 2827
			// (get) Token: 0x06002CFB RID: 11515 RVA: 0x000E6C0D File Offset: 0x000E500D
			// (set) Token: 0x06002CFC RID: 11516 RVA: 0x000E6C15 File Offset: 0x000E5015
			public ulong CharacterUnlockTokenId { get; private set; }

			// Token: 0x06002CFD RID: 11517 RVA: 0x000E6C20 File Offset: 0x000E5020
			public bool LoadFromFile(string filePath)
			{
				bool result;
				using (StreamReader streamReader = new StreamReader(filePath))
				{
					string xmlText = streamReader.ReadToEnd();
					result = this.LoadFromText(xmlText, true);
				}
				return result;
			}

			// Token: 0x06002CFE RID: 11518 RVA: 0x000E6C68 File Offset: 0x000E5068
			public bool LoadFromText(string xmlText, bool validateChecksum = true)
			{
				if (!this.m_isLoaded)
				{
					this.m_staticData = XmlSerialization.ReadFromXmlText<StaticDataReadOnly.StaticData>(xmlText);
					if (this.m_staticData == null)
					{
						return false;
					}
					foreach (StaticDataReadOnly.StaticItemBase staticItemBase in this.m_staticData.m_items.m_itemSkinList)
					{
						this.m_itemMap.Add(staticItemBase.m_ID, staticItemBase);
					}
					foreach (StaticDataReadOnly.StaticItemBase staticItemBase2 in this.m_staticData.m_items.m_itemEmoteList)
					{
						this.m_itemMap.Add(staticItemBase2.m_ID, staticItemBase2);
					}
					foreach (StaticDataReadOnly.StaticItemBase staticItemBase3 in this.m_staticData.m_items.m_itemHologramList)
					{
						this.m_itemMap.Add(staticItemBase3.m_ID, staticItemBase3);
					}
					foreach (StaticDataReadOnly.StaticItemBase staticItemBase4 in this.m_staticData.m_items.m_itemVoiceLineList)
					{
						this.m_itemMap.Add(staticItemBase4.m_ID, staticItemBase4);
					}
					foreach (StaticDataReadOnly.StaticItemBase staticItemBase5 in this.m_staticData.m_items.m_itemVictoryPoseList)
					{
						this.m_itemMap.Add(staticItemBase5.m_ID, staticItemBase5);
					}
					foreach (StaticDataReadOnly.StaticItemBase staticItemBase6 in this.m_staticData.m_items.m_itemRespawnPlatformList)
					{
						this.m_itemMap.Add(staticItemBase6.m_ID, staticItemBase6);
					}
					foreach (StaticDataReadOnly.StaticItemBase staticItemBase7 in this.m_staticData.m_items.m_itemNetsukeList)
					{
						this.m_itemMap.Add(staticItemBase7.m_ID, staticItemBase7);
					}
					foreach (StaticDataReadOnly.StaticItemBase staticItemBase8 in this.m_staticData.m_items.m_itemTokenList)
					{
						this.m_itemMap.Add(staticItemBase8.m_ID, staticItemBase8);
					}
					foreach (StaticDataReadOnly.StaticItemBase staticItemBase9 in this.m_staticData.m_items.m_itemPlayerImageList)
					{
						this.m_itemMap.Add(staticItemBase9.m_ID, staticItemBase9);
					}
					foreach (StaticDataReadOnly.StaticItemBase staticItemBase10 in this.m_staticData.m_items.m_itemBlastZoneList)
					{
						this.m_itemMap.Add(staticItemBase10.m_ID, staticItemBase10);
					}
					foreach (StaticDataReadOnly.StaticItemBase staticItemBase11 in this.m_staticData.m_items.m_itemCharacterList)
					{
						this.m_itemMap.Add(staticItemBase11.m_ID, staticItemBase11);
					}
					foreach (StaticDataReadOnly.StaticItemBase staticItemBase12 in this.m_staticData.m_items.m_itemPremiumAccountList)
					{
						this.m_itemMap.Add(staticItemBase12.m_ID, staticItemBase12);
					}
					foreach (StaticDataReadOnly.StaticItemBase staticItemBase13 in this.m_staticData.m_items.m_itemUnlockTokenList)
					{
						this.m_itemMap.Add(staticItemBase13.m_ID, staticItemBase13);
						if (this.CharacterUnlockTokenId == 0UL)
						{
							this.CharacterUnlockTokenId = staticItemBase13.m_ID;
						}
					}
					foreach (StaticDataReadOnly.StaticItemBase staticItemBase14 in this.m_staticData.m_items.m_itemCurrencyList)
					{
						this.m_itemMap.Add(staticItemBase14.m_ID, staticItemBase14);
					}
					foreach (KeyValuePair<ulong, StaticDataReadOnly.StaticItemBase> keyValuePair in this.m_itemMap)
					{
						keyValuePair.Value.m_itemTypeAsInt = keyValuePair.Value.m_itemTypeAsInt;
						keyValuePair.Value.m_rarityAsInt = keyValuePair.Value.m_rarityAsInt;
					}
					foreach (StaticDataReadOnly.StaticPackageData staticPackageData in this.m_staticData.m_packages.m_packageList)
					{
						this.m_packageMap.Add(staticPackageData.m_ID, staticPackageData);
					}
					foreach (StaticDataReadOnly.StaticLootBoxData staticLootBoxData in this.m_staticData.m_lootBoxes.m_lootBoxList)
					{
						this.m_lootBoxMap.Add(staticLootBoxData.m_ID, staticLootBoxData);
					}
					this.m_isLoaded = true;
					if (validateChecksum)
					{
						ulong num = this.CalcCheckSum();
						if (num != this.m_staticData.m_checksum)
						{
							Debug.Log(string.Concat(new object[]
							{
								"Mismatch ",
								num,
								" ",
								this.m_staticData.m_checksum
							}));
							this.m_itemMap.Clear();
							this.m_packageMap.Clear();
							this.m_staticData = null;
							return false;
						}
					}
				}
				return true;
			}

			// Token: 0x06002CFF RID: 11519 RVA: 0x000E73EC File Offset: 0x000E57EC
			private ulong CalcCheckSum()
			{
				ulong num = this.m_staticData.m_version;
				foreach (KeyValuePair<ulong, StaticDataReadOnly.StaticItemBase> keyValuePair in this.m_itemMap)
				{
					num += keyValuePair.Value.m_ID + (ulong)keyValuePair.Value.m_version;
				}
				foreach (KeyValuePair<ulong, StaticDataReadOnly.StaticPackageData> keyValuePair2 in this.m_packageMap)
				{
					num += keyValuePair2.Value.m_ID + (ulong)keyValuePair2.Value.m_version;
				}
				foreach (KeyValuePair<ulong, StaticDataReadOnly.StaticLootBoxData> keyValuePair3 in this.m_lootBoxMap)
				{
					num += keyValuePair3.Value.m_ID + (ulong)keyValuePair3.Value.m_version;
				}
				num ^= this.m_magic;
				return num;
			}

			// Token: 0x04001FF3 RID: 8179
			private StaticDataReadOnly.StaticData m_staticData = new StaticDataReadOnly.StaticData();

			// Token: 0x04001FF4 RID: 8180
			private Dictionary<ulong, StaticDataReadOnly.StaticItemBase> m_itemMap = new Dictionary<ulong, StaticDataReadOnly.StaticItemBase>();

			// Token: 0x04001FF5 RID: 8181
			private Dictionary<ulong, StaticDataReadOnly.StaticPackageData> m_packageMap = new Dictionary<ulong, StaticDataReadOnly.StaticPackageData>();

			// Token: 0x04001FF6 RID: 8182
			private Dictionary<ulong, StaticDataReadOnly.StaticLootBoxData> m_lootBoxMap = new Dictionary<ulong, StaticDataReadOnly.StaticLootBoxData>();

			// Token: 0x04001FF7 RID: 8183
			private bool m_isLoaded;

			// Token: 0x04001FF8 RID: 8184
			private readonly ulong m_magic = 192837465UL;

			// Token: 0x04001FF9 RID: 8185
			public static readonly uint skConfigVersion = 1003U;
		}
	}
}
