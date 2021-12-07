using System;
using System.Collections.Generic;

// Token: 0x0200067A RID: 1658
public struct StatTypeComparer : IEqualityComparer<StatType>
{
	// Token: 0x060028FB RID: 10491 RVA: 0x000C6139 File Offset: 0x000C4539
	public bool Equals(StatType x, StatType y)
	{
		return x == y;
	}

	// Token: 0x060028FC RID: 10492 RVA: 0x000C613F File Offset: 0x000C453F
	public int GetHashCode(StatType obj)
	{
		return (int)obj;
	}
}
