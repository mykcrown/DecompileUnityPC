using System;
using System.Collections.Generic;

// Token: 0x02000B7F RID: 2943
public struct ColorModeComparer : IEqualityComparer<ColorMode>
{
	// Token: 0x060054F2 RID: 21746 RVA: 0x001B4211 File Offset: 0x001B2611
	public bool Equals(ColorMode x, ColorMode y)
	{
		return x == y;
	}

	// Token: 0x060054F3 RID: 21747 RVA: 0x001B4217 File Offset: 0x001B2617
	public int GetHashCode(ColorMode obj)
	{
		return (int)obj;
	}
}
