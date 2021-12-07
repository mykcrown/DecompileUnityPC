using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000597 RID: 1431
[Serializable]
public class CharacterRendererState : RollbackStateTyped<CharacterRendererState>
{
	// Token: 0x06002044 RID: 8260 RVA: 0x000A2D83 File Offset: 0x000A1183
	public override void CopyTo(CharacterRendererState target)
	{
		target.colorModeFlags = this.colorModeFlags;
		target.IsVisible = this.IsVisible;
		target.overrideColorFrameCount = this.overrideColorFrameCount;
		target.overrideColor = this.overrideColor;
		target.materialEmitters = this.materialEmitters;
	}

	// Token: 0x040019BD RID: 6589
	public ColorMode colorModeFlags;

	// Token: 0x040019BE RID: 6590
	public bool IsVisible;

	// Token: 0x040019BF RID: 6591
	public int overrideColorFrameCount;

	// Token: 0x040019C0 RID: 6592
	[IgnoreRollback(IgnoreRollbackType.Static)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public AnimatingColor overrideColor;

	// Token: 0x040019C1 RID: 6593
	[IgnoreRollback(IgnoreRollbackType.Todo)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public Dictionary<GameObject, List<GeneratedEffect>> materialEmitters = new Dictionary<GameObject, List<GeneratedEffect>>();
}
