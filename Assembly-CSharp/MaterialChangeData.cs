using System;
using UnityEngine;

// Token: 0x020004CB RID: 1227
[Serializable]
public class MaterialChangeData
{
	// Token: 0x0400146A RID: 5226
	public int startFrame;

	// Token: 0x0400146B RID: 5227
	public int lerpFrames = 10;

	// Token: 0x0400146C RID: 5228
	public Material targetMaterial;

	// Token: 0x0400146D RID: 5229
	public bool restoreDefaultMaterial;
}
