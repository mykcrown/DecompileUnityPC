using System;
using System.Collections.Generic;
using System.IO;
using Commerce;
using IconsServer;
using IconsTool;
using UnityEngine;

// Token: 0x02000732 RID: 1842
public class MockEquipmentSource : IEquipmentSource
{
	// Token: 0x17000B16 RID: 2838
	// (get) Token: 0x06002D60 RID: 11616 RVA: 0x000E8B2F File Offset: 0x000E6F2F
	// (set) Token: 0x06002D61 RID: 11617 RVA: 0x000E8B37 File Offset: 0x000E6F37
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000B17 RID: 2839
	// (get) Token: 0x06002D62 RID: 11618 RVA: 0x000E8B40 File Offset: 0x000E6F40
	// (set) Token: 0x06002D63 RID: 11619 RVA: 0x000E8B48 File Offset: 0x000E6F48
	[Inject]
	public ICharacterLists characterManager { get; set; }

	// Token: 0x17000B18 RID: 2840
	// (get) Token: 0x06002D64 RID: 11620 RVA: 0x000E8B51 File Offset: 0x000E6F51
	// (set) Token: 0x06002D65 RID: 11621 RVA: 0x000E8B59 File Offset: 0x000E6F59
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000B19 RID: 2841
	// (get) Token: 0x06002D66 RID: 11622 RVA: 0x000E8B62 File Offset: 0x000E6F62
	// (set) Token: 0x06002D67 RID: 11623 RVA: 0x000E8B6A File Offset: 0x000E6F6A
	[Inject]
	public DeveloperConfig developerConfig { get; set; }

	// Token: 0x17000B1A RID: 2842
	// (get) Token: 0x06002D68 RID: 11624 RVA: 0x000E8B73 File Offset: 0x000E6F73
	// (set) Token: 0x06002D69 RID: 11625 RVA: 0x000E8B7B File Offset: 0x000E6F7B
	[Inject]
	public IServerDataConverter dataConverter { get; set; }

	// Token: 0x17000B1B RID: 2843
	// (get) Token: 0x06002D6A RID: 11626 RVA: 0x000E8B84 File Offset: 0x000E6F84
	// (set) Token: 0x06002D6B RID: 11627 RVA: 0x000E8B8C File Offset: 0x000E6F8C
	[Inject]
	public IItemLoader itemLoader { get; set; }

	// Token: 0x06002D6C RID: 11628 RVA: 0x000E8B98 File Offset: 0x000E6F98
	[PostConstruct]
	public void Init()
	{
		GameEnvironmentData gameEnvironmentData = GameEnvironmentData.Load();
		if (gameEnvironmentData.toggles.GetToggle(FeatureID.LocalStore))
		{
			this.generateItemsFromLocalFiles("Assets/Wavedash/Assets/Items", EquipmentTypes.NONE);
			foreach (CharacterDefinition characterDefinition in this.characterManager.GetNonRandomCharacters())
			{
				this.addCharacterItem("character_" + characterDefinition.characterName, "Character - " + characterDefinition.characterName, characterDefinition.characterID, EquipmentTypes.CHARACTER, 100, EquipmentRarity.LEGENDARY, null, false, -1L);
			}
		}
		else
		{
			this.localStaticDbItemGeneration();
		}
		this.allItems = this.list.ToArray();
		this.signalBus.Dispatch(EquipmentModel.SOURCE_UPDATED);
	}

	// Token: 0x06002D6D RID: 11629 RVA: 0x000E8C50 File Offset: 0x000E7050
	private void localStaticDbItemGeneration()
	{
		TextAsset textAsset = (TextAsset)Resources.Load("StaticDb");
		StaticDataReadOnly.StaticDataReader staticDataReader = new StaticDataReadOnly.StaticDataReader();
		if (!staticDataReader.LoadFromText(textAsset.text, false))
		{
			Debug.LogError("Unable to load StaticDb.xml");
			return;
		}
		foreach (KeyValuePair<ulong, StaticDataReadOnly.StaticPackageData> keyValuePair in staticDataReader.PackageMap)
		{
			StaticDataReadOnly.StaticPackageData value = keyValuePair.Value;
			if (value.m_state == EPackageState.Promoted)
			{
				if (value.m_packageItemList.Count == 1)
				{
					StaticDataReadOnly.StaticPackageItemData staticPackageItemData = value.m_packageItemList[0];
					if (staticPackageItemData.m_classificationType != EClassification.LootBox)
					{
						StaticDataReadOnly.StaticItemBase staticItemBase = staticDataReader.ItemMap[staticPackageItemData.m_staticId];
						if (staticItemBase.m_enabled)
						{
							if (!(staticItemBase is StaticDataReadOnly.StaticItemCharacter))
							{
								EquipmentTypes itemType = (EquipmentTypes)staticItemBase.m_itemType;
								CharacterID characterID = CharacterID.None;
								if (staticItemBase is StaticDataReadOnly.StaticItemCharMask)
								{
									ulong characterTypeBitMask = (staticItemBase as StaticDataReadOnly.StaticItemCharMask).m_characterTypeBitMask;
									characterID = this.dataConverter.ConvertCharacterTypesBitMask(characterTypeBitMask);
								}
								if (characterID == CharacterID.None && MockEquipmentSource.isCharacterEquipmentAvailableOnAny(itemType))
								{
									characterID = CharacterID.Any;
								}
								string friendlyName = staticItemBase.m_friendlyName;
								EquipmentTypes type = itemType;
								CharacterID character = characterID;
								string friendlyName2 = staticItemBase.m_friendlyName;
								int currencyCost = (int)staticPackageItemData.m_currencyCost;
								EquipmentRarity rarity = (EquipmentRarity)staticItemBase.m_rarity;
								string asset = staticItemBase.m_asset;
								long id = (long)staticItemBase.m_ID;
								EquippableItem equippableItem = this.addItem(friendlyName, type, character, friendlyName2, currencyCost, rarity, asset, false, id);
								if (equippableItem != null && staticItemBase is StaticDataReadOnly.StaticItemCurrency)
								{
									equippableItem.currencyAward = (staticItemBase as StaticDataReadOnly.StaticItemCurrency).m_currencyAmount;
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06002D6E RID: 11630 RVA: 0x000E8E38 File Offset: 0x000E7238
	public bool IsReady()
	{
		return true;
	}

	// Token: 0x06002D6F RID: 11631 RVA: 0x000E8E3B File Offset: 0x000E723B
	public EquippableItem[] GetAll()
	{
		return this.allItems;
	}

	// Token: 0x06002D70 RID: 11632 RVA: 0x000E8E44 File Offset: 0x000E7244
	private EquippableItem addCharacterItem(string developmentIdString, string mockName, CharacterID character, EquipmentTypes type, int price, EquipmentRarity rarity, string localAssetId = null, bool isDefault = false, long idOverride = -1L)
	{
		long id;
		if (idOverride >= 0L)
		{
			id = idOverride;
		}
		else
		{
			long num;
			this.idCounter = (num = this.idCounter) + 1L;
			id = num;
		}
		EquippableItem equippableItem = new EquippableItem(id, type, character, price, rarity);
		equippableItem.developmentIdString = developmentIdString;
		equippableItem.backupNameText = mockName;
		equippableItem.localAssetId = localAssetId;
		equippableItem.isDefault = isDefault;
		equippableItem.promoted = true;
		if (type == EquipmentTypes.SKIN)
		{
			if (localAssetId == null)
			{
				throw new UnityException("Skins must ref something");
			}
			SkinDefinition skinDefinition = this.itemLoader.LoadAsset<SkinDefinition>(equippableItem);
			if (skinDefinition == null || skinDefinition.isDefault)
			{
				return null;
			}
		}
		this.list.Add(equippableItem);
		return equippableItem;
	}

	// Token: 0x06002D71 RID: 11633 RVA: 0x000E8EF4 File Offset: 0x000E72F4
	private EquippableItem addGlobalItem(string developmentIdString, string mockName, EquipmentTypes type, int price, EquipmentRarity rarity, string localAssetId = null, bool isDefault = false, long idOverride = -1L)
	{
		return this.addCharacterItem(developmentIdString, mockName, CharacterID.None, type, price, rarity, localAssetId, isDefault, idOverride);
	}

	// Token: 0x06002D72 RID: 11634 RVA: 0x000E8F18 File Offset: 0x000E7318
	private EquippableItem addItem(string devId, EquipmentTypes type, CharacterID character, string mockName, int price, EquipmentRarity rarity, string localAssetId, bool isDefault = false, long idOverride = -1L)
	{
		if (character == CharacterID.None)
		{
			return this.addGlobalItem(devId, mockName, type, price, rarity, localAssetId, isDefault, idOverride);
		}
		return this.addCharacterItem(devId, mockName, character, type, price, rarity, localAssetId, isDefault, idOverride);
	}

	// Token: 0x06002D73 RID: 11635 RVA: 0x000E8F55 File Offset: 0x000E7355
	private void generateItemsFromLocalFiles(string itemsPath, EquipmentTypes useType = EquipmentTypes.NONE)
	{
	}

	// Token: 0x06002D74 RID: 11636 RVA: 0x000E8F58 File Offset: 0x000E7358
	private void internalLocalFileGeneration(string itemsPath, EquipmentTypes useType)
	{
		List<FileInfo> list = FileTools.FindAllFilesInTreeAtRoot(itemsPath);
		list = this.filterFilesToItemAssets(list);
		foreach (FileInfo fileInfo in list)
		{
			EquipmentTypes equipmentTypes = EquipmentTypes.NONE;
			CharacterID characterID = CharacterID.None;
			FileTools.GetEquipmentAndCharacterFromPath(fileInfo.DirectoryName, ref equipmentTypes, ref characterID);
			if (equipmentTypes != EquipmentTypes.NONE)
			{
				if (useType == EquipmentTypes.NONE || equipmentTypes == useType)
				{
					if (characterID == CharacterID.None && MockEquipmentSource.isCharacterEquipmentAvailableOnAny(equipmentTypes))
					{
						characterID = CharacterID.Any;
					}
					string localAssetIdFromFile = this.getLocalAssetIdFromFile(fileInfo);
					bool isDefault = localAssetIdFromFile.EqualsIgnoreCase("default");
					this.rarityInt++;
					if (this.rarityInt >= 4)
					{
						this.rarityInt = 0;
					}
					EquipmentRarity rarity = (EquipmentRarity)this.rarityInt;
					string text = ItemLoader.ResourceDirectories[equipmentTypes] + localAssetIdFromFile;
					if (equipmentTypes == EquipmentTypes.SKIN)
					{
						UnityEngine.Object @object = Resources.Load(text);
						if (@object == null)
						{
							throw new Exception(string.Format("Attempted to load {0} but it came back null.  The Unity library is probably corrupted.  Reimport the directory the asset is contained in.", text));
						}
						if (@object is IDefaultableData && (@object as IDefaultableData).IsDefaultData)
						{
							continue;
						}
					}
					this.addItem(localAssetIdFromFile.ToLower(), equipmentTypes, characterID, localAssetIdFromFile.ToUpper(), 100, rarity, localAssetIdFromFile, isDefault, -1L);
				}
			}
		}
	}

	// Token: 0x06002D75 RID: 11637 RVA: 0x000E90D4 File Offset: 0x000E74D4
	private static bool isCharacterEquipmentAvailableOnAny(EquipmentTypes equipmentType)
	{
		return equipmentType == EquipmentTypes.PLATFORM || equipmentType == EquipmentTypes.HOLOGRAM || equipmentType == EquipmentTypes.VOICE_TAUNT;
	}

	// Token: 0x06002D76 RID: 11638 RVA: 0x000E90EB File Offset: 0x000E74EB
	private List<FileInfo> filterFilesToItemAssets(List<FileInfo> files)
	{
		return FileTools.FilterFileList(files, (FileInfo file) => !file.FullName.Contains(".meta") && file.DirectoryName.Contains("Resources"));
	}

	// Token: 0x06002D77 RID: 11639 RVA: 0x000E9110 File Offset: 0x000E7510
	private string getLocalAssetIdFromFile(FileInfo file)
	{
		string text = Path.GetFileNameWithoutExtension(file.Name);
		string[] array = file.DirectoryName.Split(new char[]
		{
			Path.DirectorySeparatorChar
		});
		for (int i = array.Length - 1; i >= 0; i--)
		{
			string text2 = array[i];
			EquipmentTypes equipmentTypes = EquipmentTypes.NONE;
			if (FileTools.IsEquipmentTypeDirectoryName(text2, ref equipmentTypes))
			{
				break;
			}
			text = text2 + '/' + text;
		}
		return text;
	}

	// Token: 0x04002043 RID: 8259
	private List<EquippableItem> list = new List<EquippableItem>();

	// Token: 0x04002044 RID: 8260
	private EquippableItem[] allItems = new EquippableItem[0];

	// Token: 0x04002045 RID: 8261
	private long idCounter;

	// Token: 0x04002046 RID: 8262
	private int rarityInt;
}
