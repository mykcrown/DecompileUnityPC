using System;

// Token: 0x02000584 RID: 1412
public interface ICharacterDataLoader
{
	// Token: 0x06001FEA RID: 8170
	CharacterData GetData(CharacterDefinition characterDef);

	// Token: 0x06001FEB RID: 8171
	CharacterData GetData(CharacterID id);

	// Token: 0x06001FEC RID: 8172
	void Preload(CharacterDefinition characterDef, Action callback);
}
