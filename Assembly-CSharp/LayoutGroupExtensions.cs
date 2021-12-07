using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B04 RID: 2820
public static class LayoutGroupExtensions
{
	// Token: 0x06005102 RID: 20738 RVA: 0x00150C98 File Offset: 0x0014F098
	public static void Redraw(this LayoutGroup target)
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(target.GetComponent<RectTransform>());
	}
}
