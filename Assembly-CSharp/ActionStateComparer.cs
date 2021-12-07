using System;
using System.Collections.Generic;

// Token: 0x020004EF RID: 1263
public struct ActionStateComparer : IEqualityComparer<ActionState>
{
	// Token: 0x06001B9A RID: 7066 RVA: 0x0008BDBF File Offset: 0x0008A1BF
	public bool Equals(ActionState x, ActionState y)
	{
		return x == y;
	}

	// Token: 0x06001B9B RID: 7067 RVA: 0x0008BDC5 File Offset: 0x0008A1C5
	public int GetHashCode(ActionState obj)
	{
		return (int)obj;
	}
}
