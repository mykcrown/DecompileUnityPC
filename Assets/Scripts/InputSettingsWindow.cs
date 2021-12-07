// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputSettingsWindow : ClientBehavior
{
	public Sprite ButtonSprite;

	public Sprite ButtonHoverSprite;

	public CursorTargetButton SaveButton;

	public CursorTargetButton CancelButton;

	public Action OnCloseRequest;

	private List<CursorTargetButton> allButtons;

	public void Initialize(PlayerNum playerNum)
	{
		this.allButtons = new List<CursorTargetButton>();
		this.allButtons.Add(this.SaveButton);
		this.allButtons.Add(this.CancelButton);
		foreach (CursorTargetButton current in this.allButtons)
		{
			current.RequireAuthorization(playerNum);
		}
		this.SaveButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClickSave);
		this.CancelButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClickCancel);
	}

	private void onClickSave(CursorTargetButton target, PointerEventData eventData)
	{
		this.close();
	}

	public void onClickCancel(CursorTargetButton target, PointerEventData eventData)
	{
		this.close();
	}

	private void close()
	{
		if (this.OnCloseRequest != null)
		{
			this.OnCloseRequest();
		}
		foreach (CursorTargetButton current in this.allButtons)
		{
			current.Removed();
		}
	}
}
