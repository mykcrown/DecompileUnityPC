// Decompile from assembly: Assembly-CSharp.dll

using Commerce;
using IconsServer;
using IconsTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MockEquipmentSource : IEquipmentSource
{
	private List<EquippableItem> list = new List<EquippableItem>();

	private EquippableItem[] allItems = new EquippableItem[0];

	private long idCounter;

	private int rarityInt;

	private static Func<FileInfo, bool> __f__am_cache0;

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public ICharacterLists characterManager
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public DeveloperConfig developerConfig
	{
		get;
		set;
	}

	[Inject]
	public IServerDataConverter dataConverter
	{
		get;
		set;
	}

	[Inject]
	public IItemLoader itemLoader
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		GameEnvironmentData gameEnvironmentData = GameEnvironmentData.Load();
		if (gameEnvironmentData.toggles.GetToggle(FeatureID.LocalStore))
		{
			this.generateItemsFromLocalFiles("Assets/Wavedash/Assets/Items", EquipmentTypes.NONE);
			CharacterDefinition[] nonRandomCharacters = this.characterManager.GetNonRandomCharacters();
			for (int i = 0; i < nonRandomCharacters.Length; i++)
			{
				CharacterDefinition characterDefinition = nonRandomCharacters[i];
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

	private void localStaticDbItemGeneration()
	{
		TextAsset textAsset = (TextAsset)Resources.Load("StaticDb");
		StaticDataReadOnly.StaticDataReader staticDataReader = new StaticDataReadOnly.StaticDataReader();
		if (!staticDataReader.LoadFromText(textAsset.text, false))
		{
			UnityEngine.Debug.LogError("Unable to load StaticDb.xml");
			return;
		}
		foreach (KeyValuePair<ulong, StaticDataReadOnly.StaticPackageData> current in staticDataReader.PackageMap)
		{
			StaticDataReadOnly.StaticPackageData value = current.Value;
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
								long iD = (long)staticItemBase.m_ID;
								EquippableItem equippableItem = this.addItem(friendlyName, type, character, friendlyName2, currencyCost, rarity, asset, false, iD);
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

	public bool IsReady()
	{
		return true;
	}

	public EquippableItem[] GetAll()
	{
		return this.allItems;
	}

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

	private EquippableItem addGlobalItem(string developmentIdString, string mockName, EquipmentTypes type, int price, EquipmentRarity rarity, string localAssetId = null, bool isDefault = false, long idOverride = -1L)
	{
		return this.addCharacterItem(developmentIdString, mockName, CharacterID.None, type, price, rarity, localAssetId, isDefault, idOverride);
	}

	private EquippableItem addItem(string devId, EquipmentTypes type, CharacterID character, string mockName, int price, EquipmentRarity rarity, string localAssetId, bool isDefault = false, long idOverride = -1L)
	{
		if (character == CharacterID.None)
		{
			return this.addGlobalItem(devId, mockName, type, price, rarity, localAssetId, isDefault, idOverride);
		}
		return this.addCharacterItem(devId, mockName, character, type, price, rarity, localAssetId, isDefault, idOverride);
	}

	private void generateItemsFromLocalFiles(string itemsPath, EquipmentTypes useType = EquipmentTypes.NONE)
	{
	}

	private void internalLocalFileGeneration(string itemsPath, EquipmentTypes useType)
	{
		List<FileInfo> list = FileTools.FindAllFilesInTreeAtRoot(itemsPath);
		list = this.filterFilesToItemAssets(list);
		foreach (FileInfo current in list)
		{
			EquipmentTypes equipmentTypes = EquipmentTypes.NONE;
			CharacterID characterID = CharacterID.None;
			FileTools.GetEquipmentAndCharacterFromPath(current.DirectoryName, ref equipmentTypes, ref characterID);
			if (equipmentTypes != EquipmentTypes.NONE)
			{
				if (useType == EquipmentTypes.NONE || equipmentTypes == useType)
				{
					if (characterID == CharacterID.None && MockEquipmentSource.isCharacterEquipmentAvailableOnAny(equipmentTypes))
					{
						characterID = CharacterID.Any;
					}
					string localAssetIdFromFile = this.getLocalAssetIdFromFile(current);
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

	private static bool isCharacterEquipmentAvailableOnAny(EquipmentTypes equipmentType)
	{
		return equipmentType == EquipmentTypes.PLATFORM || equipmentType == EquipmentTypes.HOLOGRAM || equipmentType == EquipmentTypes.VOICE_TAUNT;
	}

	private List<FileInfo> filterFilesToItemAssets(List<FileInfo> files)
	{
		if (MockEquipmentSource.__f__am_cache0 == null)
		{
			MockEquipmentSource.__f__am_cache0 = new Func<FileInfo, bool>(MockEquipmentSource._filterFilesToItemAssets_m__0);
		}
		return FileTools.FilterFileList(files, MockEquipmentSource.__f__am_cache0);
	}

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

	private static bool _filterFilesToItemAssets_m__0(FileInfo file)
	{
		return !file.FullName.Contains(".meta") && file.DirectoryName.Contains("Resources");
	}
}
