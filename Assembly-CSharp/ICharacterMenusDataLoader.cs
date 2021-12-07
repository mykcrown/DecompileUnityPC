using System;

// Token: 0x0200058C RID: 1420
public interface ICharacterMenusDataLoader
{
	// Token: 0x06002004 RID: 8196
	CharacterMenusData GetData(CharacterDefinition characterDef);

	// Token: 0x06002005 RID: 8197
	CharacterMenusData GetData(CharacterID id);
}
