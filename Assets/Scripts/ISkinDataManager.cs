// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public interface ISkinDataManager
{
	SkinDefinition[] GetSkins(CharacterID characterId);

	SkinDefinition GetDefaultSkin(CharacterID characterId);

	GameObject GetPrefab(SkinData skin, CharacterDefinition characterDef);

	void Preload(SkinDefinition skinDefinition, Action callback);

	void GetSkinData(SkinDefinition skinDefinition, Action<SkinData> callback);

	SkinData GetPreloadedSkinData(SkinDefinition skinDefinition);

	void ClearUnused(List<string> keepList);
}
