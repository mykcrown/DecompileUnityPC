using System;
using System.Collections.Generic;

// Token: 0x020004B9 RID: 1209
[Serializable]
public struct MoveLabelComparer : IEqualityComparer<MoveLabel>
{
	// Token: 0x06001AB9 RID: 6841 RVA: 0x00089845 File Offset: 0x00087C45
	public bool Equals(MoveLabel x, MoveLabel y)
	{
		return x == y;
	}

	// Token: 0x06001ABA RID: 6842 RVA: 0x0008984B File Offset: 0x00087C4B
	public int GetHashCode(MoveLabel obj)
	{
		return (int)obj;
	}
}
