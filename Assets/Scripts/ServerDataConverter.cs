// Decompile from assembly: Assembly-CSharp.dll

using Commerce;
using System;

public class ServerDataConverter : IServerDataConverter
{
	public CharacterID ConvertCharacterTypesBitMask(ulong characterTypesBitMask)
	{
		ushort num = 0;
		while (num != 64)
		{
			ulong num2 = 1uL;
			num2 <<= (int)num;
			if ((characterTypesBitMask & num2) != 0uL)
			{
				if (characterTypesBitMask == num2)
				{
					return (int)num + CharacterID.Kidd;
				}
				return CharacterID.Any;
			}
			else
			{
				num += 1;
			}
		}
		return CharacterID.None;
	}

	public CharacterID ConvertCharID(ulong id)
	{
		return (CharacterID)(id + 1uL);
	}

	public CurrencyType ConvertCurrencyType(ECurrencyType type)
	{
		if (type == ECurrencyType.Soft)
		{
			return CurrencyType.Soft;
		}
		return CurrencyType.Hard;
	}
}
