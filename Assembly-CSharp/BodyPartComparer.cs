using System;
using System.Collections.Generic;

// Token: 0x02000394 RID: 916
public struct BodyPartComparer : IEqualityComparer<BodyPart>
{
	// Token: 0x060013A5 RID: 5029 RVA: 0x00070219 File Offset: 0x0006E619
	public bool Equals(BodyPart x, BodyPart y)
	{
		return x == y;
	}

	// Token: 0x060013A6 RID: 5030 RVA: 0x0007021F File Offset: 0x0006E61F
	public int GetHashCode(BodyPart obj)
	{
		return (int)obj;
	}
}
