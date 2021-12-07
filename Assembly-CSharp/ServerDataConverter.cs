using System;
using Commerce;

// Token: 0x02000770 RID: 1904
public class ServerDataConverter : IServerDataConverter
{
	// Token: 0x06002F2B RID: 12075 RVA: 0x000ECDA4 File Offset: 0x000EB1A4
	public CharacterID ConvertCharacterTypesBitMask(ulong characterTypesBitMask)
	{
		ushort num = 0;
		while (num != 64)
		{
			ulong num2 = 1UL;
			num2 <<= (int)num;
			if ((characterTypesBitMask & num2) != 0UL)
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

	// Token: 0x06002F2C RID: 12076 RVA: 0x000ECDE8 File Offset: 0x000EB1E8
	public CharacterID ConvertCharID(ulong id)
	{
		return (CharacterID)(id + 1UL);
	}

	// Token: 0x06002F2D RID: 12077 RVA: 0x000ECDEF File Offset: 0x000EB1EF
	public CurrencyType ConvertCurrencyType(ECurrencyType type)
	{
		if (type == ECurrencyType.Soft)
		{
			return CurrencyType.Soft;
		}
		return CurrencyType.Hard;
	}
}
