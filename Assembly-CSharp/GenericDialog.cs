using System;
using TMPro;
using UnityEngine;

// Token: 0x0200093E RID: 2366
public class GenericDialog : BaseWindow
{
	// Token: 0x06003E81 RID: 16001 RVA: 0x0011D008 File Offset: 0x0011B408
	public override void Awake()
	{
		base.Awake();
		if (this.CancelButton != null)
		{
			this.CancelButton.BasicSelectCallback = new Action<CursorTargetButton>(this.onSelection);
		}
		if (this.ConfirmButton != null)
		{
			this.ConfirmButton.BasicSelectCallback = new Action<CursorTargetButton>(this.onSelection);
		}
	}

	// Token: 0x06003E82 RID: 16002 RVA: 0x0011D06B File Offset: 0x0011B46B
	public void SetBody(string str)
	{
		this.Body.text = str;
	}

	// Token: 0x06003E83 RID: 16003 RVA: 0x0011D079 File Offset: 0x0011B479
	public void SetTitle(string str)
	{
		this.Title.text = str;
	}

	// Token: 0x06003E84 RID: 16004 RVA: 0x0011D087 File Offset: 0x0011B487
	public void SetConfirm(string str)
	{
		this.ConfirmButtonText.text = str;
	}

	// Token: 0x06003E85 RID: 16005 RVA: 0x0011D095 File Offset: 0x0011B495
	public void SetCancel(string str)
	{
		this.CancelButtonText.text = str;
	}

	// Token: 0x06003E86 RID: 16006 RVA: 0x0011D0A3 File Offset: 0x0011B4A3
	public void OnClose()
	{
		this.Close();
	}

	// Token: 0x06003E87 RID: 16007 RVA: 0x0011D0AB File Offset: 0x0011B4AB
	public void OnCancel()
	{
		if (this.CancelCallback != null)
		{
			this.CancelCallback();
		}
		this.Close();
	}

	// Token: 0x06003E88 RID: 16008 RVA: 0x0011D0C9 File Offset: 0x0011B4C9
	public virtual void OnConfirm()
	{
		if (this.ConfirmCallback != null)
		{
			this.ConfirmCallback();
		}
		this.Close();
	}

	// Token: 0x06003E89 RID: 16009 RVA: 0x0011D0E8 File Offset: 0x0011B4E8
	public override void Close()
	{
		base.Close();
		Action closeCallback = base.CloseCallback;
		base.CloseCallback = null;
		this.ConfirmCallback = null;
		this.CancelCallback = null;
		if (closeCallback != null)
		{
			closeCallback();
		}
	}

	// Token: 0x06003E8A RID: 16010 RVA: 0x0011D123 File Offset: 0x0011B523
	public override void OnCancelPressed()
	{
		this.OnCancel();
	}

	// Token: 0x06003E8B RID: 16011 RVA: 0x0011D12B File Offset: 0x0011B52B
	public override void OnCancelPressed(IPlayerCursor cursor)
	{
		this.OnCancel();
	}

	// Token: 0x06003E8C RID: 16012 RVA: 0x0011D133 File Offset: 0x0011B533
	private void onSelection(CursorTargetButton button)
	{
		this.syncDeselect(this.ConfirmButton, button);
		this.syncDeselect(this.CancelButton, button);
	}

	// Token: 0x06003E8D RID: 16013 RVA: 0x0011D14F File Offset: 0x0011B54F
	private void syncDeselect(CursorTargetButton theButton, CursorTargetButton currentSelection)
	{
		if (theButton != null && currentSelection != theButton)
		{
			theButton.OnDeselect(null);
		}
	}

	// Token: 0x04002A6B RID: 10859
	public TextMeshProUGUI Body;

	// Token: 0x04002A6C RID: 10860
	public TextMeshProUGUI Title;

	// Token: 0x04002A6D RID: 10861
	public TextMeshProUGUI ConfirmButtonText;

	// Token: 0x04002A6E RID: 10862
	public TextMeshProUGUI CancelButtonText;

	// Token: 0x04002A6F RID: 10863
	public CursorTargetButton CancelButton;

	// Token: 0x04002A70 RID: 10864
	public CursorTargetButton ConfirmButton;

	// Token: 0x04002A71 RID: 10865
	public CursorTargetButton CloseButton;

	// Token: 0x04002A72 RID: 10866
	public CanvasGroup Contents;

	// Token: 0x04002A73 RID: 10867
	public Action ConfirmCallback;

	// Token: 0x04002A74 RID: 10868
	public Action CancelCallback;
}
