using System;
using System.Collections.Generic;

// Token: 0x02000A35 RID: 2613
public struct StoreTabComparer : IEqualityComparer<StoreTab>
{
	// Token: 0x06004C7F RID: 19583 RVA: 0x001449FA File Offset: 0x00142DFA
	public bool Equals(StoreTab x, StoreTab y)
	{
		return x == y;
	}

	// Token: 0x06004C80 RID: 19584 RVA: 0x00144A00 File Offset: 0x00142E00
	public int GetHashCode(StoreTab obj)
	{
		return (int)obj;
	}
}
