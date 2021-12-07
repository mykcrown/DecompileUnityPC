// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IUICSSSceneCharacterManager : IStartupLoader
{
	void Preload();

	UICSSSceneCharacter GetCharacter(CharacterID characterId, SkinDefinition initWithSkin);

	void ReleaseCharacter(UICSSSceneCharacter characterDisplay);
}
