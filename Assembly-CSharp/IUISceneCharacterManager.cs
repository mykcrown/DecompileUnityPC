using System;
using System.Collections.Generic;

// Token: 0x02000A70 RID: 2672
public interface IUISceneCharacterManager
{
	// Token: 0x06004DC8 RID: 19912
	IUISceneCharacter GetCharacter(CharacterID characterId, SkinDefinition initWithSkin);

	// Token: 0x06004DC9 RID: 19913
	void ReleaseCharacter(IUISceneCharacter characterDisplay);

	// Token: 0x06004DCA RID: 19914
	void VRAMPreload(List<UISceneCharacterManager.CharacterWithSkin> characters, Action callback);

	// Token: 0x06004DCB RID: 19915
	void ClearPreload();
}
