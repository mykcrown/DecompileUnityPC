using System;
using System.Collections.Generic;

// Token: 0x020003AE RID: 942
public struct HitBoxStateComparer : IEqualityComparer<HitBoxState>
{
	// Token: 0x06001440 RID: 5184 RVA: 0x00071FA1 File Offset: 0x000703A1
	public bool Equals(HitBoxState x, HitBoxState y)
	{
		return x == y;
	}

	// Token: 0x06001441 RID: 5185 RVA: 0x00071FA7 File Offset: 0x000703A7
	public int GetHashCode(HitBoxState obj)
	{
		return obj.GetHashCode();
	}
}
