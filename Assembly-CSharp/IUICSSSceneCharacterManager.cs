using System;

// Token: 0x020008FE RID: 2302
public interface IUICSSSceneCharacterManager : IStartupLoader
{
	// Token: 0x06003BB1 RID: 15281
	void Preload();

	// Token: 0x06003BB2 RID: 15282
	UICSSSceneCharacter GetCharacter(CharacterID characterId, SkinDefinition initWithSkin);

	// Token: 0x06003BB3 RID: 15283
	void ReleaseCharacter(UICSSSceneCharacter characterDisplay);
}
