// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IUISceneCharacterManager
{
	IUISceneCharacter GetCharacter(CharacterID characterId, SkinDefinition initWithSkin);

	void ReleaseCharacter(IUISceneCharacter characterDisplay);

	void VRAMPreload(List<UISceneCharacterManager.CharacterWithSkin> characters, Action callback);

	void ClearPreload();
}
