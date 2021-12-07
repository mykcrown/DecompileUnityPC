using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000582 RID: 1410
public interface ICharacterDataHelper
{
	// Token: 0x06001FC3 RID: 8131
	int GetIndexOfLinkedCharacterData(CharacterDefinition characterData, CharacterDefinition linkedData);

	// Token: 0x06001FC4 RID: 8132
	WavedashAnimationData GetDefaultAnimation(CharacterDefinition characterDef, CharacterDefaultAnimationKey type);

	// Token: 0x06001FC5 RID: 8133
	List<WavedashAnimationData> GetAllDefaultAnimations(CharacterDefinition characterDef, CharacterDefaultAnimationKey type);

	// Token: 0x06001FC6 RID: 8134
	UISceneCharacterAnimRequest GetAnimationRequestFromItem(CharacterDefinition characterDef, EquippableItem item);

	// Token: 0x06001FC7 RID: 8135
	List<UISceneCharacterAnimRequest> GetAllAnimationRequestsFromItem(CharacterDefinition characterDef, EquippableItem item);

	// Token: 0x06001FC8 RID: 8136
	SkinDefinition GetSkinDefinition(CharacterID characterId, int skinId);

	// Token: 0x06001FC9 RID: 8137
	SkinDefinition GetSkinDefinition(CharacterID characterId, string skinKey);

	// Token: 0x06001FCA RID: 8138
	GameObject GetSkinPrefab(CharacterDefinition characterDef, SkinData skinData);

	// Token: 0x06001FCB RID: 8139
	EmoteData GetEmoteData(EquippableItem item);

	// Token: 0x06001FCC RID: 8140
	VictoryPoseData GetVictoryPoseData(EquippableItem item);

	// Token: 0x06001FCD RID: 8141
	CharacterDefinition GetCharacterDefinition(PlayerSelectionInfo selection);

	// Token: 0x06001FCE RID: 8142
	CharacterDefinition GetPrimary(CharacterDefinition data);

	// Token: 0x06001FCF RID: 8143
	string GetDisplayName(CharacterID characterId);

	// Token: 0x06001FD0 RID: 8144
	CharacterDefinition[] GetLinkedCharacters(CharacterDefinition characterDef);

	// Token: 0x06001FD1 RID: 8145
	bool LinkedCharactersContains(CharacterDefinition[] linkedChars, CharacterDefinition characterDef);

	// Token: 0x06001FD2 RID: 8146
	int LinkedCharactersindex(CharacterDefinition[] linkedChars, CharacterDefinition characterDef);
}
