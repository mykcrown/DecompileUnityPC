// Decompile from assembly: Assembly-CSharp.dll

using Commerce;
using IconsServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using Utils;

namespace IconsTool
{
	public class StaticDataReadOnly
	{
		public class StaticDataBase
		{
			[XmlElement("ID")]
			public ulong m_ID;

			[XmlElement("Version")]
			public uint m_version;

			[XmlElement("FriendlyName")]
			public string m_friendlyName = "StaticItemBase";

			[XmlElement("Description")]
			public string m_description = string.Empty;

			public override string ToString()
			{
				return this.m_friendlyName;
			}
		}

		public class StaticItemBase : StaticDataReadOnly.StaticDataBase
		{
			[XmlElement("Type")]
			public uint m_itemTypeAsInt;

			[XmlElement("Rarity")]
			public uint m_rarityAsInt;

			[XmlElement("Flags")]
			public ulong m_flags;

			[XmlElement("Asset")]
			public string m_asset = string.Empty;

			[XmlElement("Enabled")]
			public bool m_enabled;

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

			public override string ToString()
			{
				return this.m_friendlyName;
			}
		}

		public class StaticItemCharMask : StaticDataReadOnly.StaticItemBase
		{
			[XmlElement("CharacterTypeBitMask")]
			public ulong m_characterTypeBitMask;
		}

		public class StaticItemSkin : StaticDataReadOnly.StaticItemCharMask
		{
			internal StaticItemSkin()
			{
				base.m_itemType = EItemType.Skin;
			}
		}

		public class StaticItemToken : StaticDataReadOnly.StaticItemBase
		{
			internal StaticItemToken()
			{
				base.m_itemType = EItemType.Token;
			}
		}

		public class StaticItemHologram : StaticDataReadOnly.StaticItemCharMask
		{
			internal StaticItemHologram()
			{
				base.m_itemType = EItemType.Hologram;
			}
		}

		public class StaticItemBlastZone : StaticDataReadOnly.StaticItemBase
		{
			internal StaticItemBlastZone()
			{
				base.m_itemType = EItemType.BlastZone;
			}
		}

		public class StaticItemEmote : StaticDataReadOnly.StaticItemCharMask
		{
			internal StaticItemEmote()
			{
				base.m_itemType = EItemType.Emote;
			}
		}

		public class StaticItemNetsuke : StaticDataReadOnly.StaticItemBase
		{
			internal StaticItemNetsuke()
			{
				base.m_itemType = EItemType.Netsuke;
			}
		}

		public class StaticItemPlayerImage : StaticDataReadOnly.StaticItemBase
		{
			internal StaticItemPlayerImage()
			{
				base.m_itemType = EItemType.PlayerImage;
			}
		}

		public class StaticItemRespawnPlatform : StaticDataReadOnly.StaticItemCharMask
		{
			internal StaticItemRespawnPlatform()
			{
				base.m_itemType = EItemType.RespawnPlatform;
			}
		}

		public class StaticItemVictoryPose : StaticDataReadOnly.StaticItemCharMask
		{
			internal StaticItemVictoryPose()
			{
				base.m_itemType = EItemType.VictoryPose;
			}
		}

		public class StaticItemVoiceLine : StaticDataReadOnly.StaticItemCharMask
		{
			internal StaticItemVoiceLine()
			{
				base.m_itemType = EItemType.VoiceLine;
			}
		}

		public class StaticItemCharacter : StaticDataReadOnly.StaticItemBase
		{
			[XmlElement("CharacterType")]
			public uint m_characterTypeAsInt;

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

			internal StaticItemCharacter()
			{
				base.m_itemType = EItemType.Character;
			}
		}

		public class StaticItemPremiumAccount : StaticDataReadOnly.StaticItemBase
		{
			[XmlElement("PremiumType")]
			public uint m_premiumTypeAsInt;

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

			internal StaticItemPremiumAccount()
			{
				base.m_itemType = EItemType.PremiumAccount;
			}
		}

		public class StaticItemUnlockToken : StaticDataReadOnly.StaticItemBase
		{
			internal StaticItemUnlockToken()
			{
				base.m_itemType = EItemType.UnlockToken;
			}
		}

		public class StaticItemCurrency : StaticDataReadOnly.StaticItemBase
		{
			[XmlElement("CurrencyType")]
			public uint m_currencyTypeAsInt;

			[XmlElement("CurrencyAmount")]
			public ulong m_currencyAmount;

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

			internal StaticItemCurrency()
			{
				base.m_itemType = EItemType.Currency;
			}
		}

		public class StaticDataItemList
		{
			[XmlElement("StaticItemSkin")]
			public List<StaticDataReadOnly.StaticItemSkin> m_itemSkinList = new List<StaticDataReadOnly.StaticItemSkin>();

			[XmlElement("StaticItemEmote")]
			public List<StaticDataReadOnly.StaticItemEmote> m_itemEmoteList = new List<StaticDataReadOnly.StaticItemEmote>();

			[XmlElement("StaticItemHologram")]
			public List<StaticDataReadOnly.StaticItemHologram> m_itemHologramList = new List<StaticDataReadOnly.StaticItemHologram>();

			[XmlElement("StaticItemVoiceLine")]
			public List<StaticDataReadOnly.StaticItemVoiceLine> m_itemVoiceLineList = new List<StaticDataReadOnly.StaticItemVoiceLine>();

			[XmlElement("StaticItemVictoryPose")]
			public List<StaticDataReadOnly.StaticItemVictoryPose> m_itemVictoryPoseList = new List<StaticDataReadOnly.StaticItemVictoryPose>();

			[XmlElement("StaticItemRespawnPlatform")]
			public List<StaticDataReadOnly.StaticItemRespawnPlatform> m_itemRespawnPlatformList = new List<StaticDataReadOnly.StaticItemRespawnPlatform>();

			[XmlElement("StaticItemNetsuke")]
			public List<StaticDataReadOnly.StaticItemNetsuke> m_itemNetsukeList = new List<StaticDataReadOnly.StaticItemNetsuke>();

			[XmlElement("StaticItemToken")]
			public List<StaticDataReadOnly.StaticItemToken> m_itemTokenList = new List<StaticDataReadOnly.StaticItemToken>();

			[XmlElement("StaticItemPlayerImage")]
			public List<StaticDataReadOnly.StaticItemPlayerImage> m_itemPlayerImageList = new List<StaticDataReadOnly.StaticItemPlayerImage>();

			[XmlElement("StaticItemBlastZone")]
			public List<StaticDataReadOnly.StaticItemBlastZone> m_itemBlastZoneList = new List<StaticDataReadOnly.StaticItemBlastZone>();

			[XmlElement("StaticItemCharacter")]
			public List<StaticDataReadOnly.StaticItemCharacter> m_itemCharacterList = new List<StaticDataReadOnly.StaticItemCharacter>();

			[XmlElement("StaticItemPremiumAccount")]
			public List<StaticDataReadOnly.StaticItemPremiumAccount> m_itemPremiumAccountList = new List<StaticDataReadOnly.StaticItemPremiumAccount>();

			[XmlElement("StaticItemUnlockToken")]
			public List<StaticDataReadOnly.StaticItemUnlockToken> m_itemUnlockTokenList = new List<StaticDataReadOnly.StaticItemUnlockToken>();

			[XmlElement("StaticItemCurrency")]
			public List<StaticDataReadOnly.StaticItemCurrency> m_itemCurrencyList = new List<StaticDataReadOnly.StaticItemCurrency>();
		}

		public class StaticPackageItemData
		{
			[XmlElement("ClassificationType")]
			public uint m_classificationTypeAsInt;

			[XmlElement("StaticId")]
			public ulong m_staticId;

			[XmlElement("CurrencyCost")]
			public uint m_currencyCost;

			[XmlElement("CreditCurrencyValue")]
			public uint m_creditCurrencyValue;

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
		}

		public class StaticPackageData : StaticDataReadOnly.StaticDataBase
		{
			[XmlElement("CurrencyType")]
			public uint m_currencyTypeAsInt;

			[XmlElement("State")]
			public uint m_stateAsInt;

			[XmlElement("StateTimeStamp")]
			public DateTime m_stateTimeStamp = default(DateTime);

			[XmlElement("StaticPackageItems")]
			public List<StaticDataReadOnly.StaticPackageItemData> m_packageItemList = new List<StaticDataReadOnly.StaticPackageItemData>();

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
		}

		public class StaticPackageDataList
		{
			[XmlElement("StaticPackage")]
			public List<StaticDataReadOnly.StaticPackageData> m_packageList = new List<StaticDataReadOnly.StaticPackageData>();
		}

		public class StaticLootBoxItemData
		{
			[XmlElement("ItemId")]
			public ulong m_itemId;

			[XmlElement("CurrencyType")]
			public uint m_currencyTypeAsInt;

			[XmlElement("CreditValue")]
			public ulong m_creditCurrencyValue;

			[XmlElement("Flags")]
			public ulong m_flags;

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
		}

		public class StaticLootBoxData : StaticDataReadOnly.StaticDataBase
		{
			[XmlElement("LootBoxType")]
			public uint m_lootBoxTypeAsInt;

			[XmlElement("StaticLootBoxItems")]
			public List<StaticDataReadOnly.StaticLootBoxItemData> m_lootBoxItemList = new List<StaticDataReadOnly.StaticLootBoxItemData>();

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
		}

		public class StaticLootBoxDataList
		{
			[XmlElement("StaticLootBox")]
			public List<StaticDataReadOnly.StaticLootBoxData> m_lootBoxList = new List<StaticDataReadOnly.StaticLootBoxData>();
		}

		public class StaticData
		{
			[XmlElement("Version")]
			public ulong m_version = (ulong)StaticDataReadOnly.StaticDataReader.skConfigVersion;

			[XmlElement("Checksum")]
			public ulong m_checksum;

			[XmlElement("Items")]
			public StaticDataReadOnly.StaticDataItemList m_items = new StaticDataReadOnly.StaticDataItemList();

			[XmlElement("Packages")]
			public StaticDataReadOnly.StaticPackageDataList m_packages = new StaticDataReadOnly.StaticPackageDataList();

			[XmlElement("LootBoxes")]
			public StaticDataReadOnly.StaticLootBoxDataList m_lootBoxes = new StaticDataReadOnly.StaticLootBoxDataList();
		}

		public struct ItemId
		{
			public ulong id;

			public ItemId(ulong id)
			{
				this.id = id;
			}

			public bool IsNull()
			{
				return this.id == 0uL;
			}

			public override bool Equals(object other)
			{
				return other is StaticDataReadOnly.ItemId && (StaticDataReadOnly.ItemId)other == this;
			}

			public override int GetHashCode()
			{
				int num = 17;
				return num * 7 + ((this.id != 0uL) ? this.id.GetHashCode() : 0);
			}

			public static bool operator ==(StaticDataReadOnly.ItemId a, StaticDataReadOnly.ItemId b)
			{
				return a.id == b.id;
			}

			public static bool operator !=(StaticDataReadOnly.ItemId a, StaticDataReadOnly.ItemId b)
			{
				return !(a == b);
			}
		}

		public class StaticDataReader
		{
			private StaticDataReadOnly.StaticData m_staticData = new StaticDataReadOnly.StaticData();

			private Dictionary<ulong, StaticDataReadOnly.StaticItemBase> m_itemMap = new Dictionary<ulong, StaticDataReadOnly.StaticItemBase>();

			private Dictionary<ulong, StaticDataReadOnly.StaticPackageData> m_packageMap = new Dictionary<ulong, StaticDataReadOnly.StaticPackageData>();

			private Dictionary<ulong, StaticDataReadOnly.StaticLootBoxData> m_lootBoxMap = new Dictionary<ulong, StaticDataReadOnly.StaticLootBoxData>();

			private bool m_isLoaded;

			private readonly ulong m_magic = 192837465uL;

			public static readonly uint skConfigVersion = 1003u;

			public ulong StaticChecksum
			{
				get
				{
					return this.m_staticData.m_checksum;
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

			public ulong CharacterUnlockTokenId
			{
				get;
				private set;
			}

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

			public bool LoadFromText(string xmlText, bool validateChecksum = true)
			{
				if (!this.m_isLoaded)
				{
					this.m_staticData = XmlSerialization.ReadFromXmlText<StaticDataReadOnly.StaticData>(xmlText);
					if (this.m_staticData == null)
					{
						return false;
					}
					foreach (StaticDataReadOnly.StaticItemBase current in this.m_staticData.m_items.m_itemSkinList)
					{
						this.m_itemMap.Add(current.m_ID, current);
					}
					foreach (StaticDataReadOnly.StaticItemBase current2 in this.m_staticData.m_items.m_itemEmoteList)
					{
						this.m_itemMap.Add(current2.m_ID, current2);
					}
					foreach (StaticDataReadOnly.StaticItemBase current3 in this.m_staticData.m_items.m_itemHologramList)
					{
						this.m_itemMap.Add(current3.m_ID, current3);
					}
					foreach (StaticDataReadOnly.StaticItemBase current4 in this.m_staticData.m_items.m_itemVoiceLineList)
					{
						this.m_itemMap.Add(current4.m_ID, current4);
					}
					foreach (StaticDataReadOnly.StaticItemBase current5 in this.m_staticData.m_items.m_itemVictoryPoseList)
					{
						this.m_itemMap.Add(current5.m_ID, current5);
					}
					foreach (StaticDataReadOnly.StaticItemBase current6 in this.m_staticData.m_items.m_itemRespawnPlatformList)
					{
						this.m_itemMap.Add(current6.m_ID, current6);
					}
					foreach (StaticDataReadOnly.StaticItemBase current7 in this.m_staticData.m_items.m_itemNetsukeList)
					{
						this.m_itemMap.Add(current7.m_ID, current7);
					}
					foreach (StaticDataReadOnly.StaticItemBase current8 in this.m_staticData.m_items.m_itemTokenList)
					{
						this.m_itemMap.Add(current8.m_ID, current8);
					}
					foreach (StaticDataReadOnly.StaticItemBase current9 in this.m_staticData.m_items.m_itemPlayerImageList)
					{
						this.m_itemMap.Add(current9.m_ID, current9);
					}
					foreach (StaticDataReadOnly.StaticItemBase current10 in this.m_staticData.m_items.m_itemBlastZoneList)
					{
						this.m_itemMap.Add(current10.m_ID, current10);
					}
					foreach (StaticDataReadOnly.StaticItemBase current11 in this.m_staticData.m_items.m_itemCharacterList)
					{
						this.m_itemMap.Add(current11.m_ID, current11);
					}
					foreach (StaticDataReadOnly.StaticItemBase current12 in this.m_staticData.m_items.m_itemPremiumAccountList)
					{
						this.m_itemMap.Add(current12.m_ID, current12);
					}
					foreach (StaticDataReadOnly.StaticItemBase current13 in this.m_staticData.m_items.m_itemUnlockTokenList)
					{
						this.m_itemMap.Add(current13.m_ID, current13);
						if (this.CharacterUnlockTokenId == 0uL)
						{
							this.CharacterUnlockTokenId = current13.m_ID;
						}
					}
					foreach (StaticDataReadOnly.StaticItemBase current14 in this.m_staticData.m_items.m_itemCurrencyList)
					{
						this.m_itemMap.Add(current14.m_ID, current14);
					}
					foreach (KeyValuePair<ulong, StaticDataReadOnly.StaticItemBase> current15 in this.m_itemMap)
					{
						current15.Value.m_itemTypeAsInt = current15.Value.m_itemTypeAsInt;
						current15.Value.m_rarityAsInt = current15.Value.m_rarityAsInt;
					}
					foreach (StaticDataReadOnly.StaticPackageData current16 in this.m_staticData.m_packages.m_packageList)
					{
						this.m_packageMap.Add(current16.m_ID, current16);
					}
					foreach (StaticDataReadOnly.StaticLootBoxData current17 in this.m_staticData.m_lootBoxes.m_lootBoxList)
					{
						this.m_lootBoxMap.Add(current17.m_ID, current17);
					}
					this.m_isLoaded = true;
					if (validateChecksum)
					{
						ulong num = this.CalcCheckSum();
						if (num != this.m_staticData.m_checksum)
						{
							UnityEngine.Debug.Log(string.Concat(new object[]
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

			private ulong CalcCheckSum()
			{
				ulong num = this.m_staticData.m_version;
				foreach (KeyValuePair<ulong, StaticDataReadOnly.StaticItemBase> current in this.m_itemMap)
				{
					num += current.Value.m_ID + (ulong)current.Value.m_version;
				}
				foreach (KeyValuePair<ulong, StaticDataReadOnly.StaticPackageData> current2 in this.m_packageMap)
				{
					num += current2.Value.m_ID + (ulong)current2.Value.m_version;
				}
				foreach (KeyValuePair<ulong, StaticDataReadOnly.StaticLootBoxData> current3 in this.m_lootBoxMap)
				{
					num += current3.Value.m_ID + (ulong)current3.Value.m_version;
				}
				num ^= this.m_magic;
				return num;
			}
		}
	}
}
