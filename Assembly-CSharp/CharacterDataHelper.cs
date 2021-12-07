using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000581 RID: 1409
public class CharacterDataHelper : ICharacterDataHelper
{
	// Token: 0x170006E8 RID: 1768
	// (get) Token: 0x06001FA4 RID: 8100 RVA: 0x000A16A4 File Offset: 0x0009FAA4
	// (set) Token: 0x06001FA5 RID: 8101 RVA: 0x000A16AC File Offset: 0x0009FAAC
	[Inject]
	public GameDataManager gameDataManager { private get; set; }

	// Token: 0x170006E9 RID: 1769
	// (get) Token: 0x06001FA6 RID: 8102 RVA: 0x000A16B5 File Offset: 0x0009FAB5
	// (set) Token: 0x06001FA7 RID: 8103 RVA: 0x000A16BD File Offset: 0x0009FABD
	[Inject]
	public ILocalization localization { private get; set; }

	// Token: 0x170006EA RID: 1770
	// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x000A16C6 File Offset: 0x0009FAC6
	// (set) Token: 0x06001FA9 RID: 8105 RVA: 0x000A16CE File Offset: 0x0009FACE
	[Inject]
	public IItemLoader itemLoader { private get; set; }

	// Token: 0x170006EB RID: 1771
	// (get) Token: 0x06001FAA RID: 8106 RVA: 0x000A16D7 File Offset: 0x0009FAD7
	// (set) Token: 0x06001FAB RID: 8107 RVA: 0x000A16DF File Offset: 0x0009FADF
	[Inject]
	public ICharacterDataLoader characterDataLoader { private get; set; }

	// Token: 0x170006EC RID: 1772
	// (get) Token: 0x06001FAC RID: 8108 RVA: 0x000A16E8 File Offset: 0x0009FAE8
	// (set) Token: 0x06001FAD RID: 8109 RVA: 0x000A16F0 File Offset: 0x0009FAF0
	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader { private get; set; }

	// Token: 0x170006ED RID: 1773
	// (get) Token: 0x06001FAE RID: 8110 RVA: 0x000A16F9 File Offset: 0x0009FAF9
	// (set) Token: 0x06001FAF RID: 8111 RVA: 0x000A1701 File Offset: 0x0009FB01
	[Inject]
	public ICharacterLists characterLists { private get; set; }

	// Token: 0x170006EE RID: 1774
	// (get) Token: 0x06001FB0 RID: 8112 RVA: 0x000A170A File Offset: 0x0009FB0A
	// (set) Token: 0x06001FB1 RID: 8113 RVA: 0x000A1712 File Offset: 0x0009FB12
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x06001FB2 RID: 8114 RVA: 0x000A171C File Offset: 0x0009FB1C
	public WavedashAnimationData GetDefaultAnimation(CharacterDefinition characterDef, CharacterDefaultAnimationKey type)
	{
		List<WavedashAnimationData> allDefaultAnimations = this.GetAllDefaultAnimations(characterDef, type);
		return (allDefaultAnimations == null) ? null : allDefaultAnimations[0];
	}

	// Token: 0x06001FB3 RID: 8115 RVA: 0x000A1748 File Offset: 0x0009FB48
	public List<WavedashAnimationData> GetAllDefaultAnimations(CharacterDefinition characterDef, CharacterDefaultAnimationKey type)
	{
		List<WavedashAnimationData> list = new List<WavedashAnimationData>();
		CharacterDefaultAnimationData characterDefaultAnimationData = this.findDefaultAnimation(characterDef, type);
		list.Add((characterDefaultAnimationData != null) ? characterDefaultAnimationData.animationData : null);
		CharacterDefinition totemPartner = characterDef.totemPartner;
		if (totemPartner != null)
		{
			if (type != CharacterDefaultAnimationKey.CHARACTER_SELECT_IDLE)
			{
				if (type != CharacterDefaultAnimationKey.STORE_IDLE)
				{
					if (type == CharacterDefaultAnimationKey.SWAP_IN_ANIMATION)
					{
						type = CharacterDefaultAnimationKey.SWAP_OUT_ANIMATION;
					}
				}
				else
				{
					type = CharacterDefaultAnimationKey.SECONDARY_STORE_IDLE;
				}
			}
			else
			{
				type = CharacterDefaultAnimationKey.SECONDARY_CHARACTER_SELECT_IDLE;
			}
			CharacterDefaultAnimationData characterDefaultAnimationData2 = this.findDefaultAnimation(totemPartner, type);
			list.Add((characterDefaultAnimationData2 != null) ? characterDefaultAnimationData2.animationData : null);
		}
		if (list.Contains(null))
		{
			return null;
		}
		return list;
	}

	// Token: 0x06001FB4 RID: 8116 RVA: 0x000A17F4 File Offset: 0x0009FBF4
	public CharacterDefaultAnimationData findDefaultAnimation(CharacterDefinition characterDef, CharacterDefaultAnimationKey type)
	{
		CharacterMenusData data = this.characterMenusDataLoader.GetData(characterDef);
		foreach (CharacterDefaultAnimationData characterDefaultAnimationData in data.defaultAnimations)
		{
			if (characterDefaultAnimationData.type == type)
			{
				return characterDefaultAnimationData;
			}
		}
		return null;
	}

	// Token: 0x06001FB5 RID: 8117 RVA: 0x000A183C File Offset: 0x0009FC3C
	public UISceneCharacterAnimRequest GetAnimationRequestFromItem(CharacterDefinition characterDef, EquippableItem item)
	{
		UISceneCharacterAnimRequest result = default(UISceneCharacterAnimRequest);
		if (item == null)
		{
			CharacterData data = this.characterDataLoader.GetData(characterDef);
			result.type = UISceneCharacterAnimRequest.AnimRequestType.MoveData;
			result.moveData = data.moveSets[0].moves[0];
			return result;
		}
		switch (item.type)
		{
		case EquipmentTypes.EMOTE:
		{
			result.type = UISceneCharacterAnimRequest.AnimRequestType.MoveData;
			EmoteData emoteData = this.GetEmoteData(item);
			if (emoteData != null)
			{
				if (!characterDef.isPartner)
				{
					result.moveData = emoteData.primaryData;
				}
				else
				{
					result.moveData = emoteData.partnerData;
				}
			}
			break;
		}
		case EquipmentTypes.HOLOGRAM:
			result.type = UISceneCharacterAnimRequest.AnimRequestType.AnimData;
			result.animData = this.GetDefaultAnimation(characterDef, CharacterDefaultAnimationKey.HOLOGRAM);
			break;
		case EquipmentTypes.VOICE_TAUNT:
			result.type = UISceneCharacterAnimRequest.AnimRequestType.AnimData;
			result.animData = this.GetDefaultAnimation(characterDef, CharacterDefaultAnimationKey.VOICELINE);
			break;
		case EquipmentTypes.VICTORY_POSE:
		{
			result.mode = UISceneCharacter.AnimationMode.VICTORY_POSE;
			VictoryPoseData victoryPoseData = this.GetVictoryPoseData(item);
			if (victoryPoseData != null)
			{
				result.type = UISceneCharacterAnimRequest.AnimRequestType.MoveData;
				if (characterDef.isPartner)
				{
					result.moveData = victoryPoseData.partnerMoveData;
					result.loopingMoveData = victoryPoseData.partnerLoopingMoveData;
				}
				else
				{
					result.moveData = victoryPoseData.moveData;
					result.loopingMoveData = victoryPoseData.loopingMoveData;
				}
			}
			else
			{
				result.type = UISceneCharacterAnimRequest.AnimRequestType.AnimData;
				result.animData = this.GetDefaultAnimation(characterDef, CharacterDefaultAnimationKey.VICTORY_POSE);
				result.loopingAnimData = this.GetDefaultAnimation(characterDef, CharacterDefaultAnimationKey.VICTORY_POSE_LOOP);
			}
			break;
		}
		}
		if ((result.type == UISceneCharacterAnimRequest.AnimRequestType.AnimData && result.animData != null) || (result.type == UISceneCharacterAnimRequest.AnimRequestType.MoveData && result.moveData != null))
		{
			return result;
		}
		CharacterData data2 = this.characterDataLoader.GetData(characterDef);
		result.type = UISceneCharacterAnimRequest.AnimRequestType.MoveData;
		result.moveData = data2.moveSets[0].moves[0];
		return result;
	}

	// Token: 0x06001FB6 RID: 8118 RVA: 0x000A1A34 File Offset: 0x0009FE34
	public List<UISceneCharacterAnimRequest> GetAllAnimationRequestsFromItem(CharacterDefinition characterDef, EquippableItem item)
	{
		List<UISceneCharacterAnimRequest> list = new List<UISceneCharacterAnimRequest>();
		CharacterDefinition[] linkedCharacters = this.GetLinkedCharacters(characterDef);
		for (int i = 0; i < linkedCharacters.Length; i++)
		{
			list.Add(this.GetAnimationRequestFromItem(linkedCharacters[i], item));
		}
		return list;
	}

	// Token: 0x06001FB7 RID: 8119 RVA: 0x000A1A74 File Offset: 0x0009FE74
	public GameObject GetSkinPrefab(CharacterDefinition characterDef, SkinData skinData)
	{
		return this.skinDataManager.GetPrefab(skinData, characterDef);
	}

	// Token: 0x06001FB8 RID: 8120 RVA: 0x000A1A84 File Offset: 0x0009FE84
	public SkinDefinition GetSkinDefinition(CharacterID characterId, string skinKey)
	{
		SkinDefinition[] skins = this.skinDataManager.GetSkins(characterId);
		foreach (SkinDefinition skinDefinition in skins)
		{
			if (skinDefinition.uniqueKey == skinKey)
			{
				return skinDefinition;
			}
		}
		Debug.LogError(string.Concat(new object[]
		{
			"Could not find character skin by id '",
			skinKey,
			"' on character '",
			characterId,
			"'."
		}));
		return this.skinDataManager.GetDefaultSkin(characterId);
	}

	// Token: 0x06001FB9 RID: 8121 RVA: 0x000A1B0C File Offset: 0x0009FF0C
	public SkinDefinition GetSkinDefinition(CharacterID characterId, int skinId)
	{
		SkinDefinition[] skins = this.skinDataManager.GetSkins(characterId);
		foreach (SkinDefinition skinDefinition in skins)
		{
			if (skinDefinition.ID == skinId)
			{
				return skinDefinition;
			}
		}
		Debug.LogError(string.Concat(new object[]
		{
			"Could not find character skin by id '",
			skinId,
			"' on character '",
			characterId,
			"'."
		}));
		return this.skinDataManager.GetDefaultSkin(characterId);
	}

	// Token: 0x06001FBA RID: 8122 RVA: 0x000A1B94 File Offset: 0x0009FF94
	public EmoteData GetEmoteData(EquippableItem item)
	{
		return this.itemLoader.LoadAsset<EmoteData>(item);
	}

	// Token: 0x06001FBB RID: 8123 RVA: 0x000A1BA2 File Offset: 0x0009FFA2
	public VictoryPoseData GetVictoryPoseData(EquippableItem item)
	{
		return this.itemLoader.LoadAsset<VictoryPoseData>(item);
	}

	// Token: 0x06001FBC RID: 8124 RVA: 0x000A1BB0 File Offset: 0x0009FFB0
	public CharacterDefinition GetCharacterDefinition(PlayerSelectionInfo selection)
	{
		CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(selection.characterID);
		CharacterDefinition[] linkedCharacters = this.GetLinkedCharacters(characterDefinition);
		return linkedCharacters[selection.characterIndex];
	}

	// Token: 0x06001FBD RID: 8125 RVA: 0x000A1BE1 File Offset: 0x0009FFE1
	public CharacterDefinition GetPrimary(CharacterDefinition dataIn)
	{
		return this.characterLists.GetCharacterDefinition(dataIn.characterID);
	}

	// Token: 0x06001FBE RID: 8126 RVA: 0x000A1BF4 File Offset: 0x0009FFF4
	public string GetDisplayName(CharacterID characterId)
	{
		CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(characterId);
		this.characterMenusDataLoader.GetData(characterDefinition);
		string text = this.localization.GetText("gameData.characters.name." + characterDefinition.characterName);
		if (text != null)
		{
			text = text.ToUpper();
		}
		return text;
	}

	// Token: 0x06001FBF RID: 8127 RVA: 0x000A1C48 File Offset: 0x000A0048
	public CharacterDefinition[] GetLinkedCharacters(CharacterDefinition characterDef)
	{
		CharacterDefinition[] array;
		if (!this.linkedCharacterIndex.TryGetValue(characterDef.characterName, out array))
		{
			List<CharacterDefinition> list = new List<CharacterDefinition>();
			list.Add(characterDef);
			if (characterDef.totemPartner != null)
			{
				list.Add(characterDef.totemPartner);
			}
			array = list.ToArray();
			this.linkedCharacterIndex[characterDef.characterName] = array;
		}
		return array;
	}

	// Token: 0x06001FC0 RID: 8128 RVA: 0x000A1CB4 File Offset: 0x000A00B4
	public bool LinkedCharactersContains(CharacterDefinition[] linkedChars, CharacterDefinition characterDef)
	{
		foreach (CharacterDefinition x in linkedChars)
		{
			if (x == characterDef)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001FC1 RID: 8129 RVA: 0x000A1CEC File Offset: 0x000A00EC
	public int LinkedCharactersindex(CharacterDefinition[] linkedChars, CharacterDefinition characterDef)
	{
		for (int i = 0; i < linkedChars.Length; i++)
		{
			if (linkedChars[i] == characterDef)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06001FC2 RID: 8130 RVA: 0x000A1D20 File Offset: 0x000A0120
	public int GetIndexOfLinkedCharacterData(CharacterDefinition characterDef, CharacterDefinition linkedData)
	{
		CharacterDefinition[] linkedCharacters = this.GetLinkedCharacters(characterDef);
		return this.LinkedCharactersindex(linkedCharacters, linkedData);
	}

	// Token: 0x0400193A RID: 6458
	private Dictionary<string, CharacterDefinition[]> linkedCharacterIndex = new Dictionary<string, CharacterDefinition[]>();
}
