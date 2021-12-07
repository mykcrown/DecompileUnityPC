using System;
using UnityEngine;

// Token: 0x02000A1D RID: 2589
public class Item3DPreview
{
	// Token: 0x170011F4 RID: 4596
	// (get) Token: 0x06004B57 RID: 19287 RVA: 0x00141A3B File Offset: 0x0013FE3B
	// (set) Token: 0x06004B58 RID: 19288 RVA: 0x00141A43 File Offset: 0x0013FE43
	public Action playPreviewFn { private get; set; }

	// Token: 0x06004B59 RID: 19289 RVA: 0x00141A4C File Offset: 0x0013FE4C
	public void PlayPreview()
	{
		if (this.playPreviewFn != null)
		{
			this.playPreviewFn();
		}
	}

	// Token: 0x06004B5A RID: 19290 RVA: 0x00141A64 File Offset: 0x0013FE64
	public void Cleanup()
	{
		this.playPreviewFn = null;
		this.obj = null;
	}

	// Token: 0x0400317C RID: 12668
	public GameObject obj;

	// Token: 0x0400317D RID: 12669
	public EquipmentTypes type;

	// Token: 0x0400317E RID: 12670
	public EquippableItem item;

	// Token: 0x0400317F RID: 12671
	public CharacterMenusData characterMenusData;

	// Token: 0x04003180 RID: 12672
	public bool isOffsetsInitialized;

	// Token: 0x04003181 RID: 12673
	public float anchorOffsetFromCenterY;

	// Token: 0x04003182 RID: 12674
	public float anchorOffsetFromBottomY;

	// Token: 0x04003183 RID: 12675
	public float anchorOffsetFromCenterX;

	// Token: 0x04003184 RID: 12676
	public float anchorOffsetFromBottomX;
}
