using System;
using UnityEngine;

// Token: 0x02000950 RID: 2384
public class TabDefinition
{
	// Token: 0x06003F52 RID: 16210 RVA: 0x001200A3 File Offset: 0x0011E4A3
	public TabDefinition(SettingsTab id, GameObject prefab = null)
	{
		this.id = (int)id;
		this.prefab = prefab;
	}

	// Token: 0x06003F53 RID: 16211 RVA: 0x001200B9 File Offset: 0x0011E4B9
	public TabDefinition(StoreTab id, GameObject prefab = null)
	{
		this.id = (int)id;
		this.prefab = prefab;
	}

	// Token: 0x04002AED RID: 10989
	public int id;

	// Token: 0x04002AEE RID: 10990
	public GameObject prefab;

	// Token: 0x04002AEF RID: 10991
	public int ordinal;
}
