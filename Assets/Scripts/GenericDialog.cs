// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

public class GenericDialog : BaseWindow
{
	public TextMeshProUGUI Body;

	public TextMeshProUGUI Title;

	public TextMeshProUGUI ConfirmButtonText;

	public TextMeshProUGUI CancelButtonText;

	public CursorTargetButton CancelButton;

	public CursorTargetButton ConfirmButton;

	public CursorTargetButton CloseButton;

	public CanvasGroup Contents;

	public Action ConfirmCallback;

	public Action CancelCallback;

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

	public void SetBody(string str)
	{
		this.Body.text = str;
	}

	public void SetTitle(string str)
	{
		this.Title.text = str;
	}

	public void SetConfirm(string str)
	{
		this.ConfirmButtonText.text = str;
	}

	public void SetCancel(string str)
	{
		this.CancelButtonText.text = str;
	}

	public void OnClose()
	{
		this.Close();
	}

	public void OnCancel()
	{
		if (this.CancelCallback != null)
		{
			this.CancelCallback();
		}
		this.Close();
	}

	public virtual void OnConfirm()
	{
		if (this.ConfirmCallback != null)
		{
			this.ConfirmCallback();
		}
		this.Close();
	}

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

	public override void OnCancelPressed()
	{
		this.OnCancel();
	}

	public override void OnCancelPressed(IPlayerCursor cursor)
	{
		this.OnCancel();
	}

	private void onSelection(CursorTargetButton button)
	{
		this.syncDeselect(this.ConfirmButton, button);
		this.syncDeselect(this.CancelButton, button);
	}

	private void syncDeselect(CursorTargetButton theButton, CursorTargetButton currentSelection)
	{
		if (theButton != null && currentSelection != theButton)
		{
			theButton.OnDeselect(null);
		}
	}
}
