using System;
using UnityEngine;

// Token: 0x02000A1B RID: 2587
public class RadioButton : MonoBehaviour
{
	// Token: 0x06004B28 RID: 19240 RVA: 0x001410F1 File Offset: 0x0013F4F1
	public void SetToggle(bool isOn)
	{
		if (isOn)
		{
			this.Toggle.alpha = 1f;
		}
		else
		{
			this.Toggle.alpha = 0f;
		}
	}

	// Token: 0x0400316C RID: 12652
	public MenuItemButton Button;

	// Token: 0x0400316D RID: 12653
	public CanvasGroup Toggle;
}
