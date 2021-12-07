using System;
using UnityEngine;

// Token: 0x0200044B RID: 1099
public class CachedMaterial
{
	// Token: 0x060016C9 RID: 5833 RVA: 0x0007B2FF File Offset: 0x000796FF
	public CachedMaterial(Material origin)
	{
		this.Material = origin;
	}

	// Token: 0x0400118B RID: 4491
	public Material Material;

	// Token: 0x0400118C RID: 4492
	public int activeCount;
}
