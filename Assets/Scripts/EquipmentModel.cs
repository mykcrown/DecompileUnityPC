// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentModel : IEquipmentModel
{
	public static string SOURCE_UPDATED = "EquipmentModel.SOURCE_UPDATED";

	public static string UPDATED = "EquipmentModel.UPDATED";

	private EquipmentIndexById index = new EquipmentIndexById();

	private EquipmentIndexByDevString byStringIndex = new EquipmentIndexByDevString();

	private EquipmentTypeCharIndex byTypeCharIndex = new EquipmentTypeCharIndex();

	private EquipmentTypeIndex byTypeIndex = new EquipmentTypeIndex();

	private EquipmentCharIndex byCharIndex = new EquipmentCharIndex();

	private DefaultEquipmentTypeCharIndex defaultItemsByCharacter = new DefaultEquipmentTypeCharIndex();

	private DefaultEquipmentTypeIndex defaultItemsGlobal = new DefaultEquipmentTypeIndex();

	private Dictionary<string, EquippableItem> skinKeyToItem = new Dictionary<string, EquippableItem>();

	private Dictionary<long, SkinDefinition> equipmentKeyToSkin = new Dictionary<long, SkinDefinition>();

	private int defaultIdCounter;

	private static HashSet<EquipmentTypes> dontCreateDefaultsForTypes = new HashSet<EquipmentTypes>
	{
		EquipmentTypes.NETSUKE
	};

	[Inject]
	public IEquipmentSource source
	{
		private get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		private get;
		set;
	}

	[Inject]
	public IEquipMethodMap equipMethodMap
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterLists characterLists
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		private get;
		set;
	}

	[Inject]
	public IItemLoader itemLoader
	{
		private get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		private get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
	{
		private get;
		set;
	}

	[Inject]
	public GameEnvironmentData environmentData
	{
		private get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(EquipmentModel.SOURCE_UPDATED, new Action(this.writeIndices));
		if (this.source.IsReady())
		{
			this.writeIndices();
		}
	}

	private void writeIndices()
	{
		this.index = new EquipmentIndexById();
		this.byStringIndex = new EquipmentIndexByDevString();
		this.byTypeCharIndex = new EquipmentTypeCharIndex();
		this.byTypeIndex = new EquipmentTypeIndex();
		this.byCharIndex = new EquipmentCharIndex();
		this.defaultItemsByCharacter = new DefaultEquipmentTypeCharIndex();
		this.defaultItemsGlobal = new DefaultEquipmentTypeIndex();
		this.skinKeyToItem = new Dictionary<string, EquippableItem>();
		this.defaultIdCounter = -100;
		EquipmentTypeCharIndexBuilder equipmentTypeCharIndexBuilder = new EquipmentTypeCharIndexBuilder();
		EquipmentTypeIndexBuilder equipmentTypeIndexBuilder = new EquipmentTypeIndexBuilder();
		EquipmentCharIndexBuilder equipmentCharIndexBuilder = new EquipmentCharIndexBuilder();
		CharacterDefinition[] nonRandomCharacters = this.characterLists.GetNonRandomCharacters();
		for (int i = 0; i < nonRandomCharacters.Length; i++)
		{
			CharacterDefinition characterDefinition = nonRandomCharacters[i];
			foreach (EquipmentTypes current in this.equipMethodMap.GetTypesWithMethod(EquipMethod.CHARACTER))
			{
				EquippableItem item = this.addCharacterDefault(characterDefinition.characterID, current);
				this.addItemToAllIndexes(item, ref equipmentTypeCharIndexBuilder, ref equipmentTypeIndexBuilder, ref equipmentCharIndexBuilder);
			}
		}
		foreach (EquipmentTypes current2 in this.equipMethodMap.GetTypesWithMethod(EquipMethod.GLOBAL))
		{
			if (!EquipmentModel.dontCreateDefaultsForTypes.Contains(current2))
			{
				EquippableItem item2 = this.addGlobalDefault(current2);
				this.addItemToAllIndexes(item2, ref equipmentTypeCharIndexBuilder, ref equipmentTypeIndexBuilder, ref equipmentCharIndexBuilder);
			}
		}
		EquippableItem[] all = this.source.GetAll();
		for (int j = 0; j < all.Length; j++)
		{
			EquippableItem item3 = all[j];
			this.addItemToAllIndexes(item3, ref equipmentTypeCharIndexBuilder, ref equipmentTypeIndexBuilder, ref equipmentCharIndexBuilder);
		}
		foreach (CharacterID current3 in equipmentTypeCharIndexBuilder.Keys)
		{
			if (!this.byTypeCharIndex.ContainsKey(current3))
			{
				this.byTypeCharIndex[current3] = new Dictionary<EquipmentTypes, EquippableItem[]>();
			}
			foreach (EquipmentTypes current4 in equipmentTypeCharIndexBuilder[current3].Keys)
			{
				this.byTypeCharIndex[current3][current4] = equipmentTypeCharIndexBuilder[current3][current4].ToArray();
			}
		}
		foreach (EquipmentTypes current5 in equipmentTypeIndexBuilder.Keys)
		{
			this.byTypeIndex[current5] = equipmentTypeIndexBuilder[current5].ToArray();
		}
		foreach (CharacterID current6 in equipmentCharIndexBuilder.Keys)
		{
			this.byCharIndex[current6] = equipmentCharIndexBuilder[current6].ToArray();
		}
		this.linkSkinData();
		this.signalBus.Dispatch(EquipmentModel.UPDATED);
	}

	private void linkSkinData()
	{
		EquippableItem[] array = this.byTypeIndex[EquipmentTypes.SKIN];
		for (int i = 0; i < array.Length; i++)
		{
			EquippableItem equippableItem = array[i];
			SkinDefinition skinDefinition = (!equippableItem.isDefault) ? this.itemLoader.LoadAsset<SkinDefinition>(equippableItem) : this.skinDataManager.GetDefaultSkin(equippableItem.character);
			if (skinDefinition != null)
			{
				if (this.skinKeyToItem.ContainsKey(skinDefinition.uniqueKey))
				{
					DataAlert.Fatal("Duplicate skin id " + skinDefinition.uniqueKey + " " + skinDefinition.skinName);
				}
				this.skinKeyToItem[skinDefinition.uniqueKey] = equippableItem;
				this.equipmentKeyToSkin[equippableItem.id.id] = skinDefinition;
			}
		}
	}

	private void addItemToAllIndexes(EquippableItem item, ref EquipmentTypeCharIndexBuilder byTypeCharIndex, ref EquipmentTypeIndexBuilder byTypeIndex, ref EquipmentCharIndexBuilder byCharIndex)
	{
		if (this.index.ContainsKey(item.id))
		{
			throw new UnityException(string.Concat(new object[]
			{
				"DUPLICATE EQUIPMENT ID ",
				item.type,
				" ",
				item.id.id
			}));
		}
		this.index[item.id] = item;
		if (item.isDefault)
		{
			this.setGlobalDefault(item.type, item);
		}
		if (item.developmentIdString != null)
		{
			this.byStringIndex[item.developmentIdString] = item;
		}
		if (item.character != CharacterID.None)
		{
			if (!byTypeCharIndex.ContainsKey(item.character))
			{
				byTypeCharIndex[item.character] = new Dictionary<EquipmentTypes, List<EquippableItem>>();
			}
			if (!byTypeCharIndex[item.character].ContainsKey(item.type))
			{
				byTypeCharIndex[item.character][item.type] = new List<EquippableItem>();
			}
			byTypeCharIndex[item.character][item.type].Add(item);
			if (!byCharIndex.ContainsKey(item.character))
			{
				byCharIndex[item.character] = new List<EquippableItem>();
			}
			byCharIndex[item.character].Add(item);
		}
		if (!byTypeIndex.ContainsKey(item.type))
		{
			byTypeIndex[item.type] = new List<EquippableItem>();
		}
		byTypeIndex[item.type].Add(item);
	}

	private EquippableItem addCharacterDefault(CharacterID characterId, EquipmentTypes type)
	{
		if (!this.defaultItemsByCharacter.ContainsKey(characterId))
		{
			this.defaultItemsByCharacter[characterId] = new Dictionary<EquipmentTypes, EquippableItem>();
		}
		EquippableItem equippableItem = this.createDefault(characterId, type);
		this.defaultItemsByCharacter[characterId][type] = equippableItem;
		return equippableItem;
	}

	private EquippableItem addGlobalDefault(EquipmentTypes type)
	{
		EquippableItem equippableItem = this.createDefault(CharacterID.None, type);
		this.defaultItemsGlobal[type] = equippableItem;
		return equippableItem;
	}

	private void setGlobalDefault(EquipmentTypes type, EquippableItem item)
	{
		this.defaultItemsGlobal[type] = item;
	}

	private EquippableItem createDefault(CharacterID characterId, EquipmentTypes type)
	{
		this.defaultIdCounter--;
		EquippableItem equippableItem = new EquippableItem();
		equippableItem.id = new EquipmentID((long)this.defaultIdCounter);
		equippableItem.type = type;
		equippableItem.character = characterId;
		equippableItem.overrideLocalizationKey = "equip.item.default";
		equippableItem.isDefault = true;
		if (type == EquipmentTypes.VICTORY_POSE)
		{
			CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(characterId);
			string str = FileTools.CharacterIdToCharacterName(characterId);
			string str2 = (!(characterDefinition == null) && !string.IsNullOrEmpty(characterDefinition.defaultVictoryPose)) ? characterDefinition.defaultVictoryPose : (str + " Victory Pose 01");
			equippableItem.localAssetId = str + "/" + str2;
			VictoryPoseData victoryPoseData = this.itemLoader.LoadAsset<VictoryPoseData>(equippableItem);
			if (victoryPoseData)
			{
				if (!string.IsNullOrEmpty(victoryPoseData.localizedNameWhenIsDefault))
				{
					equippableItem.overrideLocalizationKey = victoryPoseData.localizedNameWhenIsDefault;
					equippableItem.backupNameText = victoryPoseData.localizedNameWhenIsDefault.ToUpper();
				}
				else
				{
					UnityEngine.Debug.LogError("Missing victory pose localization key for the default victory pose: " + equippableItem.localAssetId + "\nMark as 'default' and specify a friendly name");
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Missing victory pose asset at path: " + equippableItem.localAssetId);
			}
		}
		else if (type == EquipmentTypes.PLAYER_ICON)
		{
			equippableItem.localAssetId = "default";
		}
		return equippableItem;
	}

	public EquippableItem[] GetAllCharacterItems(CharacterID characterID)
	{
		if (!this.byCharIndex.ContainsKey(characterID))
		{
			this.byCharIndex[characterID] = new EquippableItem[0];
		}
		return this.byCharIndex[characterID];
	}

	public EquippableItem[] GetCharacterItems(CharacterID characterId, EquipmentTypes type)
	{
		if (!this.byTypeCharIndex.ContainsKey(characterId))
		{
			this.byTypeCharIndex[characterId] = new Dictionary<EquipmentTypes, EquippableItem[]>();
		}
		if (!this.byTypeCharIndex[characterId].ContainsKey(type))
		{
			this.byTypeCharIndex[characterId][type] = new EquippableItem[0];
		}
		return this.byTypeCharIndex[characterId][type];
	}

	public EquippableItem[] GetGlobalItems(EquipmentTypes type)
	{
		if (!this.byTypeIndex.ContainsKey(type))
		{
			this.byTypeIndex[type] = new EquippableItem[0];
		}
		return this.byTypeIndex[type];
	}

	public EquippableItem GetItem(EquipmentID id)
	{
		EquippableItem result;
		this.index.TryGetValue(id, out result);
		return result;
	}

	public EquippableItem GetItemByDevelopmentString(string developmentIdString)
	{
		EquippableItem result = null;
		if (!this.byStringIndex.TryGetValue(developmentIdString, out result))
		{
			UnityEngine.Debug.LogErrorFormat("Could not find developmentIdString: {0} in EquipmentModel.byStringIndex!", new object[]
			{
				developmentIdString
			});
		}
		return result;
	}

	public EquippableItem GetDefaultItem(CharacterID characterId, EquipmentTypes type)
	{
		if (this.defaultItemsByCharacter.ContainsKey(characterId))
		{
			return this.defaultItemsByCharacter[characterId][type];
		}
		return null;
	}

	public EquippableItem GetDefaultItem(EquipmentTypes type)
	{
		EquippableItem result;
		this.defaultItemsGlobal.TryGetValue(type, out result);
		return result;
	}

	public string GetLocalizedItemName(EquippableItem item)
	{
		string key = "equipment.name." + item.id;
		if (!string.IsNullOrEmpty(item.overrideLocalizationKey))
		{
			key = item.overrideLocalizationKey;
		}
		string text = this.localization.GetText(key);
		if (text == null)
		{
			text = item.backupNameText;
		}
		if (text != null)
		{
			text = text.ToUpper();
		}
		return text;
	}

	public SkinDefinition GetSkinFromItem(EquipmentID itemId)
	{
		if (itemId.id == 0L)
		{
			return null;
		}
		SkinDefinition skinDefinition = null;
		if (this.equipmentKeyToSkin.TryGetValue(itemId.id, out skinDefinition) && skinDefinition != null)
		{
			return skinDefinition;
		}
		if (this.GetItem(itemId) == null)
		{
			return null;
		}
		return this.skinDataManager.GetDefaultSkin(this.GetItem(itemId).character);
	}

	public EquippableItem GetItemFromSkinKey(string skinKey)
	{
		EquippableItem result;
		this.skinKeyToItem.TryGetValue(skinKey, out result);
		return result;
	}

	public CustomPlatform GetRespawnPlatformFromItem(EquipmentID itemId)
	{
		EquippableItem item = this.GetItem(itemId);
		if (item.isDefault)
		{
			return null;
		}
		return this.itemLoader.LoadPrefab<CustomPlatform>(item);
	}

	public Netsuke GetNetsukeFromItem(EquipmentID itemId)
	{
		EquippableItem item = this.GetItem(itemId);
		if (item == null || item.isDefault)
		{
			return null;
		}
		return this.itemLoader.LoadPrefab<Netsuke>(item);
	}

	public PlayerToken GetPlayerTokenFromItem(EquipmentID itemId)
	{
		EquippableItem item = this.GetItem(itemId);
		if (item.isDefault)
		{
			return null;
		}
		return this.itemLoader.LoadPrefab<PlayerToken>(item);
	}

	public HologramData GetHologramFromItem(EquipmentID itemId)
	{
		EquippableItem item = this.GetItem(itemId);
		if (item.isDefault)
		{
			return null;
		}
		return this.itemLoader.LoadAsset<HologramData>(item);
	}

	public PlayerCardIconData GetPlayerIconFromItem(EquipmentID itemId)
	{
		EquippableItem equippableItem;
		if (itemId.IsNull())
		{
			equippableItem = this.GetDefaultItem(EquipmentTypes.PLAYER_ICON);
		}
		else
		{
			equippableItem = this.GetItem(itemId);
		}
		if (equippableItem == null)
		{
			return null;
		}
		return this.itemLoader.LoadAsset<PlayerCardIconData>(equippableItem);
	}

	public VoiceTauntData GetVoiceTauntFromItem(EquipmentID itemId)
	{
		EquippableItem item = this.GetItem(itemId);
		if (item.isDefault)
		{
			return null;
		}
		return this.itemLoader.LoadAsset<VoiceTauntData>(item);
	}
}
