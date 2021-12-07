// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataHelper : ICharacterDataHelper
{
	private Dictionary<string, CharacterDefinition[]> linkedCharacterIndex = new Dictionary<string, CharacterDefinition[]>();

	[Inject]
	public GameDataManager gameDataManager
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
	public IItemLoader itemLoader
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterDataLoader characterDataLoader
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader
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
	public ISkinDataManager skinDataManager
	{
		private get;
		set;
	}

	public WavedashAnimationData GetDefaultAnimation(CharacterDefinition characterDef, CharacterDefaultAnimationKey type)
	{
		List<WavedashAnimationData> allDefaultAnimations = this.GetAllDefaultAnimations(characterDef, type);
		return (allDefaultAnimations == null) ? null : allDefaultAnimations[0];
	}

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

	public CharacterDefaultAnimationData findDefaultAnimation(CharacterDefinition characterDef, CharacterDefaultAnimationKey type)
	{
		CharacterMenusData data = this.characterMenusDataLoader.GetData(characterDef);
		CharacterDefaultAnimationData[] defaultAnimations = data.defaultAnimations;
		for (int i = 0; i < defaultAnimations.Length; i++)
		{
			CharacterDefaultAnimationData characterDefaultAnimationData = defaultAnimations[i];
			if (characterDefaultAnimationData.type == type)
			{
				return characterDefaultAnimationData;
			}
		}
		return null;
	}

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

	public GameObject GetSkinPrefab(CharacterDefinition characterDef, SkinData skinData)
	{
		return this.skinDataManager.GetPrefab(skinData, characterDef);
	}

	public SkinDefinition GetSkinDefinition(CharacterID characterId, string skinKey)
	{
		SkinDefinition[] skins = this.skinDataManager.GetSkins(characterId);
		SkinDefinition[] array = skins;
		for (int i = 0; i < array.Length; i++)
		{
			SkinDefinition skinDefinition = array[i];
			if (skinDefinition.uniqueKey == skinKey)
			{
				return skinDefinition;
			}
		}
		UnityEngine.Debug.LogError(string.Concat(new object[]
		{
			"Could not find character skin by id '",
			skinKey,
			"' on character '",
			characterId,
			"'."
		}));
		return this.skinDataManager.GetDefaultSkin(characterId);
	}

	public SkinDefinition GetSkinDefinition(CharacterID characterId, int skinId)
	{
		SkinDefinition[] skins = this.skinDataManager.GetSkins(characterId);
		SkinDefinition[] array = skins;
		for (int i = 0; i < array.Length; i++)
		{
			SkinDefinition skinDefinition = array[i];
			if (skinDefinition.ID == skinId)
			{
				return skinDefinition;
			}
		}
		UnityEngine.Debug.LogError(string.Concat(new object[]
		{
			"Could not find character skin by id '",
			skinId,
			"' on character '",
			characterId,
			"'."
		}));
		return this.skinDataManager.GetDefaultSkin(characterId);
	}

	public EmoteData GetEmoteData(EquippableItem item)
	{
		return this.itemLoader.LoadAsset<EmoteData>(item);
	}

	public VictoryPoseData GetVictoryPoseData(EquippableItem item)
	{
		return this.itemLoader.LoadAsset<VictoryPoseData>(item);
	}

	public CharacterDefinition GetCharacterDefinition(PlayerSelectionInfo selection)
	{
		CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(selection.characterID);
		CharacterDefinition[] linkedCharacters = this.GetLinkedCharacters(characterDefinition);
		return linkedCharacters[selection.characterIndex];
	}

	public CharacterDefinition GetPrimary(CharacterDefinition dataIn)
	{
		return this.characterLists.GetCharacterDefinition(dataIn.characterID);
	}

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

	public bool LinkedCharactersContains(CharacterDefinition[] linkedChars, CharacterDefinition characterDef)
	{
		for (int i = 0; i < linkedChars.Length; i++)
		{
			CharacterDefinition x = linkedChars[i];
			if (x == characterDef)
			{
				return true;
			}
		}
		return false;
	}

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

	public int GetIndexOfLinkedCharacterData(CharacterDefinition characterDef, CharacterDefinition linkedData)
	{
		CharacterDefinition[] linkedCharacters = this.GetLinkedCharacters(characterDef);
		return this.LinkedCharactersindex(linkedCharacters, linkedData);
	}
}
