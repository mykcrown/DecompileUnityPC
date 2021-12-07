using System;
using UnityEngine;

// Token: 0x02000424 RID: 1060
public class PlayerCardIconData : ScriptableObject, IDefaultableData
{
	// Token: 0x17000449 RID: 1097
	// (get) Token: 0x060015FC RID: 5628 RVA: 0x00077CCD File Offset: 0x000760CD
	public bool IsDefaultData
	{
		get
		{
			return base.name.EqualsIgnoreCase("default");
		}
	}

	// Token: 0x040010F2 RID: 4338
	public Sprite sprite;
}
