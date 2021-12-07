using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009AE RID: 2478
public class PromoButton : MenuItemButton
{
	// Token: 0x0600447D RID: 17533 RVA: 0x0012D9DA File Offset: 0x0012BDDA
	public void SetImage(Sprite sprite)
	{
		this.MainImage.sprite = sprite;
	}

	// Token: 0x04002D95 RID: 11669
	public Image MainImage;
}
