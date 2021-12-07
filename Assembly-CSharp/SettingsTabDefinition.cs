using System;
using UnityEngine;

// Token: 0x02000986 RID: 2438
public class SettingsTabDefinition
{
	// Token: 0x0600420F RID: 16911 RVA: 0x001271FA File Offset: 0x001255FA
	public SettingsTabDefinition(SettingsTab id)
	{
		this.id = id;
	}

	// Token: 0x06004210 RID: 16912 RVA: 0x00127209 File Offset: 0x00125609
	public SettingsTabDefinition(SettingsTab id, GameObject prefab)
	{
		this.id = id;
		this.prefab = prefab;
	}

	// Token: 0x04002C4A RID: 11338
	public SettingsTab id;

	// Token: 0x04002C4B RID: 11339
	public GameObject prefab;

	// Token: 0x04002C4C RID: 11340
	public int ordinal;
}
