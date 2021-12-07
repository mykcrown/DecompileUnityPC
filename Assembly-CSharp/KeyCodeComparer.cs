using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002EE RID: 750
public struct KeyCodeComparer : IEqualityComparer<KeyCode>
{
	// Token: 0x06000FC2 RID: 4034 RVA: 0x0005EC76 File Offset: 0x0005D076
	public bool Equals(KeyCode x, KeyCode y)
	{
		return x == y;
	}

	// Token: 0x06000FC3 RID: 4035 RVA: 0x0005EC7C File Offset: 0x0005D07C
	public int GetHashCode(KeyCode obj)
	{
		return (int)obj;
	}
}
