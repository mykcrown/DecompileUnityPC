// Decompile from assembly: Assembly-CSharp.dll

using Commerce;
using System;

public interface IServerDataConverter
{
	CharacterID ConvertCharacterTypesBitMask(ulong characterTypesBitMask);

	CharacterID ConvertCharID(ulong id);

	CurrencyType ConvertCurrencyType(ECurrencyType type);
}
