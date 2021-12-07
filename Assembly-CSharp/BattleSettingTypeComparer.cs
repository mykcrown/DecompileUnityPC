using System;
using System.Collections.Generic;

// Token: 0x020008EC RID: 2284
[Serializable]
public struct BattleSettingTypeComparer : IEqualityComparer<BattleSettingType>
{
	// Token: 0x06003A6B RID: 14955 RVA: 0x00111997 File Offset: 0x0010FD97
	public bool Equals(BattleSettingType x, BattleSettingType y)
	{
		return x == y;
	}

	// Token: 0x06003A6C RID: 14956 RVA: 0x0011199D File Offset: 0x0010FD9D
	public int GetHashCode(BattleSettingType obj)
	{
		return (int)obj;
	}
}
