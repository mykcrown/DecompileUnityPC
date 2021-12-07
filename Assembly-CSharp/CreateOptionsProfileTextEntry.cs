using System;
using UnityEngine;

// Token: 0x020008E4 RID: 2276
public class CreateOptionsProfileTextEntry : WavedashTextEntry
{
	// Token: 0x17000E07 RID: 3591
	// (get) Token: 0x06003A50 RID: 14928 RVA: 0x00111106 File Offset: 0x0010F506
	// (set) Token: 0x06003A51 RID: 14929 RVA: 0x0011110E File Offset: 0x0010F50E
	public bool IsErrorState { get; set; }

	// Token: 0x06003A52 RID: 14930 RVA: 0x00111118 File Offset: 0x0010F518
	public override void UpdateHighlightState()
	{
		this.ErrorText.SetActive(false);
		this.RedBorder.SetActive(false);
		this.HighlightGameObject.SetActive(false);
		if (this.IsErrorState)
		{
			this.ErrorText.gameObject.SetActive(true);
			this.RedBorder.SetActive(true);
		}
		else
		{
			base.UpdateHighlightState();
		}
	}

	// Token: 0x0400280F RID: 10255
	public GameObject RedBorder;

	// Token: 0x04002810 RID: 10256
	public GameObject ErrorText;
}
