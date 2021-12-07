using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020008E2 RID: 2274
public class InputSettingsWindow : ClientBehavior
{
	// Token: 0x06003A3B RID: 14907 RVA: 0x00110BA4 File Offset: 0x0010EFA4
	public void Initialize(PlayerNum playerNum)
	{
		this.allButtons = new List<CursorTargetButton>();
		this.allButtons.Add(this.SaveButton);
		this.allButtons.Add(this.CancelButton);
		foreach (CursorTargetButton cursorTargetButton in this.allButtons)
		{
			cursorTargetButton.RequireAuthorization(playerNum);
		}
		this.SaveButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClickSave);
		this.CancelButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClickCancel);
	}

	// Token: 0x06003A3C RID: 14908 RVA: 0x00110C5C File Offset: 0x0010F05C
	private void onClickSave(CursorTargetButton target, PointerEventData eventData)
	{
		this.close();
	}

	// Token: 0x06003A3D RID: 14909 RVA: 0x00110C64 File Offset: 0x0010F064
	public void onClickCancel(CursorTargetButton target, PointerEventData eventData)
	{
		this.close();
	}

	// Token: 0x06003A3E RID: 14910 RVA: 0x00110C6C File Offset: 0x0010F06C
	private void close()
	{
		if (this.OnCloseRequest != null)
		{
			this.OnCloseRequest();
		}
		foreach (CursorTargetButton cursorTargetButton in this.allButtons)
		{
			cursorTargetButton.Removed();
		}
	}

	// Token: 0x04002802 RID: 10242
	public Sprite ButtonSprite;

	// Token: 0x04002803 RID: 10243
	public Sprite ButtonHoverSprite;

	// Token: 0x04002804 RID: 10244
	public CursorTargetButton SaveButton;

	// Token: 0x04002805 RID: 10245
	public CursorTargetButton CancelButton;

	// Token: 0x04002806 RID: 10246
	public Action OnCloseRequest;

	// Token: 0x04002807 RID: 10247
	private List<CursorTargetButton> allButtons;
}
