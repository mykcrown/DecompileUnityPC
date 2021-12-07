using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000608 RID: 1544
public interface ISkinDataManager
{
	// Token: 0x06002610 RID: 9744
	SkinDefinition[] GetSkins(CharacterID characterId);

	// Token: 0x06002611 RID: 9745
	SkinDefinition GetDefaultSkin(CharacterID characterId);

	// Token: 0x06002612 RID: 9746
	GameObject GetPrefab(SkinData skin, CharacterDefinition characterDef);

	// Token: 0x06002613 RID: 9747
	void Preload(SkinDefinition skinDefinition, Action callback);

	// Token: 0x06002614 RID: 9748
	void GetSkinData(SkinDefinition skinDefinition, Action<SkinData> callback);

	// Token: 0x06002615 RID: 9749
	SkinData GetPreloadedSkinData(SkinDefinition skinDefinition);

	// Token: 0x06002616 RID: 9750
	void ClearUnused(List<string> keepList);
}
