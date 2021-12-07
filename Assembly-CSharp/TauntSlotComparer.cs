using System;
using System.Collections.Generic;

// Token: 0x02000751 RID: 1873
[Serializable]
public struct TauntSlotComparer : IEqualityComparer<TauntSlot>
{
	// Token: 0x06002E75 RID: 11893 RVA: 0x000EB0BF File Offset: 0x000E94BF
	public bool Equals(TauntSlot x, TauntSlot y)
	{
		return x == y;
	}

	// Token: 0x06002E76 RID: 11894 RVA: 0x000EB0C5 File Offset: 0x000E94C5
	public int GetHashCode(TauntSlot obj)
	{
		return (int)obj;
	}
}
