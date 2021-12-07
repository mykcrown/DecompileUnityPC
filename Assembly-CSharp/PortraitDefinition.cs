using System;
using UnityEngine;

// Token: 0x020008B7 RID: 2231
internal class PortraitDefinition
{
	// Token: 0x0600382D RID: 14381 RVA: 0x00107851 File Offset: 0x00105C51
	public PortraitDefinition(PortraitMode mode, PortraitBgMode bgMode)
	{
		this.mode = mode;
		this.bgMode = bgMode;
	}

	// Token: 0x04002668 RID: 9832
	public PortraitMode mode;

	// Token: 0x04002669 RID: 9833
	public PortraitBgMode bgMode;

	// Token: 0x0400266A RID: 9834
	public Texture2D texture;
}
