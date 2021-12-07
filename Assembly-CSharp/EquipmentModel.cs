using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200071F RID: 1823
public class EquipmentModel : IEquipmentModel
{
	// Token: 0x17000B0C RID: 2828
	// (get) Token: 0x06002D07 RID: 11527 RVA: 0x000E7BD2 File Offset: 0x000E5FD2
	// (set) Token: 0x06002D08 RID: 11528 RVA: 0x000E7BDA File Offset: 0x000E5FDA
	[Inject]
	public IEquipmentSource source { private get; set; }

	// Token: 0x17000B0D RID: 2829
	// (get) Token: 0x06002D09 RID: 11529 RVA: 0x000E7BE3 File Offset: 0x000E5FE3
	// (set) Token: 0x06002D0A RID: 11530 RVA: 0x000E7BEB File Offset: 0x000E5FEB
	[Inject]
	public ILocalization localization { private get; set; }

	// Token: 0x17000B0E RID: 2830
	// (get) Token: 0x06002D0B RID: 11531 RVA: 0x000E7BF4 File Offset: 0x000E5FF4
	// (set) Token: 0x06002D0C RID: 11532 RVA: 0x000E7BFC File Offset: 0x000E5FFC
	[Inject]
	public IEquipMethodMap equipMethodMap { private get; set; }

	// Token: 0x17000B0F RID: 2831
	// (get) Token: 0x06002D0D RID: 11533 RVA: 0x000E7C05 File Offset: 0x000E6005
	// (set) Token: 0x06002D0E RID: 11534 RVA: 0x000E7C0D File Offset: 0x000E600D
	[Inject]
	public ICharacterLists characterLists { private get; set; }

	// Token: 0x17000B10 RID: 2832
	// (get) Token: 0x06002D0F RID: 11535 RVA: 0x000E7C16 File Offset: 0x000E6016
	// (set) Token: 0x06002D10 RID: 11536 RVA: 0x000E7C1E File Offset: 0x000E601E
	[Inject]
	public ICharacterDataHelper characterDataHelper { private get; set; }

	// Token: 0x17000B11 RID: 2833
	// (get) Token: 0x06002D11 RID: 11537 RVA: 0x000E7C27 File Offset: 0x000E6027
	// (set) Token: 0x06002D12 RID: 11538 RVA: 0x000E7C2F File Offset: 0x000E602F
	[Inject]
	public IItemLoader itemLoader { private get; set; }

	// Token: 0x17000B12 RID: 2834
	// (get) Token: 0x06002D13 RID: 11539 RVA: 0x000E7C38 File Offset: 0x000E6038
	// (set) Token: 0x06002D14 RID: 11540 RVA: 0x000E7C40 File Offset: 0x000E6040
	[Inject]
	public ISignalBus signalBus { private get; set; }

	// Token: 0x17000B13 RID: 2835
	// (get) Token: 0x06002D15 RID: 11541 RVA: 0x000E7C49 File Offset: 0x000E6049
	// (set) Token: 0x06002D16 RID: 11542 RVA: 0x000E7C51 File Offset: 0x000E6051
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x17000B14 RID: 2836
	// (get) Token: 0x06002D17 RID: 11543 RVA: 0x000E7C5A File Offset: 0x000E605A
	// (set) Token: 0x06002D18 RID: 11544 RVA: 0x000E7C62 File Offset: 0x000E6062
	[Inject]
	public GameEnvironmentData environmentData { private get; set; }

	// Token: 0x06002D19 RID: 11545 RVA: 0x000E7C6B File Offset: 0x000E606B
	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(EquipmentModel.SOURCE_UPDATED, new Action(this.writeIndices));
		if (this.source.IsReady())
		{
			this.writeIndices();
		}
	}

	// Token: 0x06002D1A RID: 11546 RVA: 0x000E7CA0 File Offset: 0x000E60A0
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
		foreach (CharacterDefinition characterDefinition in this.characterLists.GetNonRandomCharacters())
		{
			foreach (EquipmentTypes type in this.equipMethodMap.GetTypesWithMethod(EquipMethod.CHARACTER))
			{
				EquippableItem item = this.addCharacterDefault(characterDefinition.characterID, type);
				this.addItemToAllIndexes(item, ref equipmentTypeCharIndexBuilder, ref equipmentTypeIndexBuilder, ref equipmentCharIndexBuilder);
			}
		}
		foreach (EquipmentTypes equipmentTypes in this.equipMethodMap.GetTypesWithMethod(EquipMethod.GLOBAL))
		{
			if (!EquipmentModel.dontCreateDefaultsForTypes.Contains(equipmentTypes))
			{
				EquippableItem item2 = this.addGlobalDefault(equipmentTypes);
				this.addItemToAllIndexes(item2, ref equipmentTypeCharIndexBuilder, ref equipmentTypeIndexBuilder, ref equipmentCharIndexBuilder);
			}
		}
		foreach (EquippableItem item3 in this.source.GetAll())
		{
			this.addItemToAllIndexes(item3, ref equipmentTypeCharIndexBuilder, ref equipmentTypeIndexBuilder, ref equipmentCharIndexBuilder);
		}
		foreach (CharacterID key in equipmentTypeCharIndexBuilder.Keys)
		{
			if (!this.byTypeCharIndex.ContainsKey(key))
			{
				this.byTypeCharIndex[key] = new Dictionary<EquipmentTypes, EquippableItem[]>();
			}
			foreach (EquipmentTypes key2 in equipmentTypeCharIndexBuilder[key].Keys)
			{
				this.byTypeCharIndex[key][key2] = equipmentTypeCharIndexBuilder[key][key2].ToArray();
			}
		}
		foreach (EquipmentTypes key3 in equipmentTypeIndexBuilder.Keys)
		{
			this.byTypeIndex[key3] = equipmentTypeIndexBuilder[key3].ToArray();
		}
		foreach (CharacterID key4 in equipmentCharIndexBuilder.Keys)
		{
			this.byCharIndex[key4] = equipmentCharIndexBuilder[key4].ToArray();
		}
		this.linkSkinData();
		this.signalBus.Dispatch(EquipmentModel.UPDATED);
	}

	// Token: 0x06002D1B RID: 11547 RVA: 0x000E8024 File Offset: 0x000E6424
	private void linkSkinData()
	{
		foreach (EquippableItem equippableItem in this.byTypeIndex[EquipmentTypes.SKIN])
		{
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

	// Token: 0x06002D1C RID: 11548 RVA: 0x000E80F0 File Offset: 0x000E64F0
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

	// Token: 0x06002D1D RID: 11549 RVA: 0x000E8294 File Offset: 0x000E6694
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

	// Token: 0x06002D1E RID: 11550 RVA: 0x000E82E0 File Offset: 0x000E66E0
	private EquippableItem addGlobalDefault(EquipmentTypes type)
	{
		EquippableItem equippableItem = this.createDefault(CharacterID.None, type);
		this.defaultItemsGlobal[type] = equippableItem;
		return equippableItem;
	}

	// Token: 0x06002D1F RID: 11551 RVA: 0x000E8304 File Offset: 0x000E6704
	private void setGlobalDefault(EquipmentTypes type, EquippableItem item)
	{
		this.defaultItemsGlobal[type] = item;
	}

	// Token: 0x06002D20 RID: 11552 RVA: 0x000E8314 File Offset: 0x000E6714
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
					Debug.LogError("Missing victory pose localization key for the default victory pose: " + equippableItem.localAssetId + "\nMark as 'default' and specify a friendly name");
				}
			}
			else
			{
				Debug.LogError("Missing victory pose asset at path: " + equippableItem.localAssetId);
			}
		}
		else if (type == EquipmentTypes.PLAYER_ICON)
		{
			equippableItem.localAssetId = "default";
		}
		return equippableItem;
	}

	// Token: 0x06002D21 RID: 11553 RVA: 0x000E8462 File Offset: 0x000E6862
	public EquippableItem[] GetAllCharacterItems(CharacterID characterID)
	{
		if (!this.byCharIndex.ContainsKey(characterID))
		{
			this.byCharIndex[characterID] = new EquippableItem[0];
		}
		return this.byCharIndex[characterID];
	}

	// Token: 0x06002D22 RID: 11554 RVA: 0x000E8494 File Offset: 0x000E6894
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

	// Token: 0x06002D23 RID: 11555 RVA: 0x000E8504 File Offset: 0x000E6904
	public EquippableItem[] GetGlobalItems(EquipmentTypes type)
	{
		if (!this.byTypeIndex.ContainsKey(type))
		{
			this.byTypeIndex[type] = new EquippableItem[0];
		}
		return this.byTypeIndex[type];
	}

	// Token: 0x06002D24 RID: 11556 RVA: 0x000E8538 File Offset: 0x000E6938
	public EquippableItem GetItem(EquipmentID id)
	{
		EquippableItem result;
		this.index.TryGetValue(id, out result);
		return result;
	}

	// Token: 0x06002D25 RID: 11557 RVA: 0x000E8558 File Offset: 0x000E6958
	public EquippableItem GetItemByDevelopmentString(string developmentIdString)
	{
		EquippableItem result = null;
		if (!this.byStringIndex.TryGetValue(developmentIdString, out result))
		{
			Debug.LogErrorFormat("Could not find developmentIdString: {0} in EquipmentModel.byStringIndex!", new object[]
			{
				developmentIdString
			});
		}
		return result;
	}

	// Token: 0x06002D26 RID: 11558 RVA: 0x000E858F File Offset: 0x000E698F
	public EquippableItem GetDefaultItem(CharacterID characterId, EquipmentTypes type)
	{
		if (this.defaultItemsByCharacter.ContainsKey(characterId))
		{
			return this.defaultItemsByCharacter[characterId][type];
		}
		return null;
	}

	// Token: 0x06002D27 RID: 11559 RVA: 0x000E85B8 File Offset: 0x000E69B8
	public EquippableItem GetDefaultItem(EquipmentTypes type)
	{
		EquippableItem result;
		this.defaultItemsGlobal.TryGetValue(type, out result);
		return result;
	}

	// Token: 0x06002D28 RID: 11560 RVA: 0x000E85D8 File Offset: 0x000E69D8
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

	// Token: 0x06002D29 RID: 11561 RVA: 0x000E863C File Offset: 0x000E6A3C
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

	// Token: 0x06002D2A RID: 11562 RVA: 0x000E86AC File Offset: 0x000E6AAC
	public EquippableItem GetItemFromSkinKey(string skinKey)
	{
		EquippableItem result;
		this.skinKeyToItem.TryGetValue(skinKey, out result);
		return result;
	}

	// Token: 0x06002D2B RID: 11563 RVA: 0x000E86CC File Offset: 0x000E6ACC
	public CustomPlatform GetRespawnPlatformFromItem(EquipmentID itemId)
	{
		EquippableItem item = this.GetItem(itemId);
		if (item.isDefault)
		{
			return null;
		}
		return this.itemLoader.LoadPrefab<CustomPlatform>(item);
	}

	// Token: 0x06002D2C RID: 11564 RVA: 0x000E86FC File Offset: 0x000E6AFC
	public Netsuke GetNetsukeFromItem(EquipmentID itemId)
	{
		EquippableItem item = this.GetItem(itemId);
		if (item == null || item.isDefault)
		{
			return null;
		}
		return this.itemLoader.LoadPrefab<Netsuke>(item);
	}

	// Token: 0x06002D2D RID: 11565 RVA: 0x000E8730 File Offset: 0x000E6B30
	public PlayerToken GetPlayerTokenFromItem(EquipmentID itemId)
	{
		EquippableItem item = this.GetItem(itemId);
		if (item.isDefault)
		{
			return null;
		}
		return this.itemLoader.LoadPrefab<PlayerToken>(item);
	}

	// Token: 0x06002D2E RID: 11566 RVA: 0x000E8760 File Offset: 0x000E6B60
	public HologramData GetHologramFromItem(EquipmentID itemId)
	{
		EquippableItem item = this.GetItem(itemId);
		if (item.isDefault)
		{
			return null;
		}
		return this.itemLoader.LoadAsset<HologramData>(item);
	}

	// Token: 0x06002D2F RID: 11567 RVA: 0x000E8790 File Offset: 0x000E6B90
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

	// Token: 0x06002D30 RID: 11568 RVA: 0x000E87D4 File Offset: 0x000E6BD4
	public VoiceTauntData GetVoiceTauntFromItem(EquipmentID itemId)
	{
		EquippableItem item = this.GetItem(itemId);
		if (item.isDefault)
		{
			return null;
		}
		return this.itemLoader.LoadAsset<VoiceTauntData>(item);
	}

	// Token: 0x0400200D RID: 8205
	public static string SOURCE_UPDATED = "EquipmentModel.SOURCE_UPDATED";

	// Token: 0x0400200E RID: 8206
	public static string UPDATED = "EquipmentModel.UPDATED";

	// Token: 0x04002018 RID: 8216
	private EquipmentIndexById index = new EquipmentIndexById();

	// Token: 0x04002019 RID: 8217
	private EquipmentIndexByDevString byStringIndex = new EquipmentIndexByDevString();

	// Token: 0x0400201A RID: 8218
	private EquipmentTypeCharIndex byTypeCharIndex = new EquipmentTypeCharIndex();

	// Token: 0x0400201B RID: 8219
	private EquipmentTypeIndex byTypeIndex = new EquipmentTypeIndex();

	// Token: 0x0400201C RID: 8220
	private EquipmentCharIndex byCharIndex = new EquipmentCharIndex();

	// Token: 0x0400201D RID: 8221
	private DefaultEquipmentTypeCharIndex defaultItemsByCharacter = new DefaultEquipmentTypeCharIndex();

	// Token: 0x0400201E RID: 8222
	private DefaultEquipmentTypeIndex defaultItemsGlobal = new DefaultEquipmentTypeIndex();

	// Token: 0x0400201F RID: 8223
	private Dictionary<string, EquippableItem> skinKeyToItem = new Dictionary<string, EquippableItem>();

	// Token: 0x04002020 RID: 8224
	private Dictionary<long, SkinDefinition> equipmentKeyToSkin = new Dictionary<long, SkinDefinition>();

	// Token: 0x04002021 RID: 8225
	private int defaultIdCounter;

	// Token: 0x04002022 RID: 8226
	private static HashSet<EquipmentTypes> dontCreateDefaultsForTypes = new HashSet<EquipmentTypes>
	{
		EquipmentTypes.NETSUKE
	};
}
