using System;
using UnityEngine;

// Token: 0x02000768 RID: 1896
public class PlayerCardIconDisplay : BaseItem3DPreviewDisplay
{
	// Token: 0x06002EE7 RID: 12007 RVA: 0x000EC9A4 File Offset: 0x000EADA4
	public void SetIcon(Sprite sprite)
	{
		this.spriteRenderer.sprite = sprite;
	}

	// Token: 0x040020DF RID: 8415
	public SpriteRenderer spriteRenderer;
}
