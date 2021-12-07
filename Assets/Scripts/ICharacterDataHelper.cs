// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterDataHelper
{
	int GetIndexOfLinkedCharacterData(CharacterDefinition characterData, CharacterDefinition linkedData);

	WavedashAnimationData GetDefaultAnimation(CharacterDefinition characterDef, CharacterDefaultAnimationKey type);

	List<WavedashAnimationData> GetAllDefaultAnimations(CharacterDefinition characterDef, CharacterDefaultAnimationKey type);

	UISceneCharacterAnimRequest GetAnimationRequestFromItem(CharacterDefinition characterDef, EquippableItem item);

	List<UISceneCharacterAnimRequest> GetAllAnimationRequestsFromItem(CharacterDefinition characterDef, EquippableItem item);

	SkinDefinition GetSkinDefinition(CharacterID characterId, int skinId);

	SkinDefinition GetSkinDefinition(CharacterID characterId, string skinKey);

	GameObject GetSkinPrefab(CharacterDefinition characterDef, SkinData skinData);

	EmoteData GetEmoteData(EquippableItem item);

	VictoryPoseData GetVictoryPoseData(EquippableItem item);

	CharacterDefinition GetCharacterDefinition(PlayerSelectionInfo selection);

	CharacterDefinition GetPrimary(CharacterDefinition data);

	string GetDisplayName(CharacterID characterId);

	CharacterDefinition[] GetLinkedCharacters(CharacterDefinition characterDef);

	bool LinkedCharactersContains(CharacterDefinition[] linkedChars, CharacterDefinition characterDef);

	int LinkedCharactersindex(CharacterDefinition[] linkedChars, CharacterDefinition characterDef);
}
