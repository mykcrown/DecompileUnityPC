// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WavedashTMProInput : TMP_InputField
{
	private WavedashTextEntry controller;

	private bool _activated;

	private int prevTextLength;

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	public Action EndEditCallback
	{
		get;
		set;
	}

	public Action ValueChangedCallback
	{
		get;
		set;
	}

	public Action EnterCallback
	{
		get;
		set;
	}

	public Action TabCallback
	{
		get;
		set;
	}

	public Action CursorOverCallback
	{
		get;
		set;
	}

	public Action CursorOutCallback
	{
		get;
		set;
	}

	public bool Activated
	{
		get
		{
			return this._activated;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		StaticInject.Inject(this);
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		if (this.CursorOverCallback != null)
		{
			this.CursorOverCallback();
		}
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		if (this.CursorOutCallback != null)
		{
			this.CursorOutCallback();
		}
	}

	public override void OnSelect(BaseEventData eventData)
	{
	}

	public override void OnDeselect(BaseEventData eventData)
	{
	}

	public void SetController(WavedashTextEntry controller)
	{
		this.controller = controller;
	}

	public void OnSubmitPressed()
	{
		if (this._activated && this.EnterCallback != null)
		{
			this.EnterCallback();
		}
	}

	public void OnTabPressed()
	{
		if (this._activated && this.TabCallback != null)
		{
			this.TabCallback();
		}
	}

	public void SetInputActive(bool active)
	{
		if (active)
		{
			this.activate();
		}
		else
		{
			this.deactivate();
		}
	}

	private void activate()
	{
		if (!this._activated)
		{
			this._activated = true;
			this.updateHighlightState();
			base.ActivateInputField();
			this.fixCaretAlignmentBug();
		}
	}

	private void fixCaretAlignmentBug()
	{
		TMP_SelectionCaret componentInChildren = base.transform.parent.GetComponentInChildren<TMP_SelectionCaret>();
		Vector3 localPosition = componentInChildren.transform.localPosition;
		localPosition.y = 0f;
		localPosition.x = 0f;
		componentInChildren.transform.localPosition = localPosition;
	}

	private void deactivate()
	{
		if (this._activated)
		{
			this._activated = false;
			this.updateHighlightState();
			base.DeactivateInputField();
		}
	}

	private void updateHighlightState()
	{
		this.controller.UpdateHighlightState();
	}

	public void OnEndEdit()
	{
		if (this.EndEditCallback != null)
		{
			this.EndEditCallback();
		}
	}

	public void OnValueChanged()
	{
		if (base.text.Length > this.prevTextLength)
		{
			this.audioManager.PlayMenuSound(SoundKey.generic_typingCharacter, 0f);
		}
		else if (base.text.Length < this.prevTextLength)
		{
			this.audioManager.PlayMenuSound(SoundKey.generic_typingBackspace, 0f);
		}
		this.prevTextLength = base.text.Length;
		if (this.controller.AutoCapitalize)
		{
			base.text = base.text.ToUpper();
		}
		if (this.ValueChangedCallback != null)
		{
			this.ValueChangedCallback();
		}
	}

	public void Removed()
	{
		this.deactivate();
	}

	protected override void OnDestroy()
	{
		this.EndEditCallback = null;
		base.OnDestroy();
	}
}
