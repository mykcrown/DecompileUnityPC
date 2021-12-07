using System;
using Commerce;

// Token: 0x02000771 RID: 1905
public interface IServerDataConverter
{
	// Token: 0x06002F2E RID: 12078
	CharacterID ConvertCharacterTypesBitMask(ulong characterTypesBitMask);

	// Token: 0x06002F2F RID: 12079
	CharacterID ConvertCharID(ulong id);

	// Token: 0x06002F30 RID: 12080
	CurrencyType ConvertCurrencyType(ECurrencyType type);
}
