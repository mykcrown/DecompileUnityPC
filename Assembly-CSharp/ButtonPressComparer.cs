using System;
using System.Collections.Generic;

// Token: 0x020004F2 RID: 1266
public struct ButtonPressComparer : IEqualityComparer<ButtonPress>
{
	// Token: 0x06001BA1 RID: 7073 RVA: 0x0008BEE3 File Offset: 0x0008A2E3
	public bool Equals(ButtonPress x, ButtonPress y)
	{
		return x == y;
	}

	// Token: 0x06001BA2 RID: 7074 RVA: 0x0008BEE9 File Offset: 0x0008A2E9
	public int GetHashCode(ButtonPress obj)
	{
		return (int)obj;
	}
}
