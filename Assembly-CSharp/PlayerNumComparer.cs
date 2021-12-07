using System;
using System.Collections.Generic;

// Token: 0x02000B44 RID: 2884
[Serializable]
public struct PlayerNumComparer : IEqualityComparer<PlayerNum>
{
	// Token: 0x060053CA RID: 21450 RVA: 0x001B02E3 File Offset: 0x001AE6E3
	public bool Equals(PlayerNum x, PlayerNum y)
	{
		return x == y;
	}

	// Token: 0x060053CB RID: 21451 RVA: 0x001B02E9 File Offset: 0x001AE6E9
	public int GetHashCode(PlayerNum obj)
	{
		return (int)obj;
	}
}
