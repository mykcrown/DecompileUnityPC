using System;
using System.Collections.Generic;

// Token: 0x0200092A RID: 2346
public struct DebugDrawChannelComparer : IEqualityComparer<DebugDrawChannel>
{
	// Token: 0x06003D27 RID: 15655 RVA: 0x0011AAA2 File Offset: 0x00118EA2
	public bool Equals(DebugDrawChannel x, DebugDrawChannel y)
	{
		return x == y;
	}

	// Token: 0x06003D28 RID: 15656 RVA: 0x0011AAA8 File Offset: 0x00118EA8
	public int GetHashCode(DebugDrawChannel obj)
	{
		return (int)obj;
	}
}
