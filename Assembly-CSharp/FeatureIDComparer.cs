using System;
using System.Collections.Generic;

// Token: 0x0200040B RID: 1035
public struct FeatureIDComparer : IEqualityComparer<FeatureID>
{
	// Token: 0x060015B4 RID: 5556 RVA: 0x00077381 File Offset: 0x00075781
	public bool Equals(FeatureID x, FeatureID y)
	{
		return x == y;
	}

	// Token: 0x060015B5 RID: 5557 RVA: 0x00077387 File Offset: 0x00075787
	public int GetHashCode(FeatureID obj)
	{
		return (int)obj;
	}
}
