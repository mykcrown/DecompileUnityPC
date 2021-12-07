using System;
using System.Collections.Generic;

// Token: 0x02000B41 RID: 2881
public struct TeamNumComparer : IEqualityComparer<TeamNum>
{
	// Token: 0x060053C8 RID: 21448 RVA: 0x001B02DA File Offset: 0x001AE6DA
	public bool Equals(TeamNum x, TeamNum y)
	{
		return x == y;
	}

	// Token: 0x060053C9 RID: 21449 RVA: 0x001B02E0 File Offset: 0x001AE6E0
	public int GetHashCode(TeamNum obj)
	{
		return (int)obj;
	}
}
