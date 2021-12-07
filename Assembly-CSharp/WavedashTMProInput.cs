using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000958 RID: 2392
public class WavedashTMProInput : TMP_InputField
{
	// Token: 0x17000F0D RID: 3853
	// (get) Token: 0x06003F86 RID: 16262 RVA: 0x0012088F File Offset: 0x0011EC8F
	// (set) Token: 0x06003F87 RID: 16263 RVA: 0x00120897 File Offset: 0x0011EC97
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x17000F0E RID: 3854
	// (get) Token: 0x06003F88 RID: 16264 RVA: 0x001208A0 File Offset: 0x0011ECA0
	// (set) Token: 0x06003F89 RID: 16265 RVA: 0x001208A8 File Offset: 0x0011ECA8
	public Action EndEditCallback { get; set; }

	// Token: 0x17000F0F RID: 3855
	// (get) Token: 0x06003F8A RID: 16266 RVA: 0x001208B1 File Offset: 0x0011ECB1
	// (set) Token: 0x06003F8B RID: 16267 RVA: 0x001208B9 File Offset: 0x0011ECB9
	public Action ValueChangedCallback { get; set; }

	// Token: 0x17000F10 RID: 3856
	// (get) Token: 0x06003F8C RID: 16268 RVA: 0x001208C2 File Offset: 0x0011ECC2
	// (set) Token: 0x06003F8D RID: 16269 RVA: 0x001208CA File Offset: 0x0011ECCA
	public Action EnterCallback { get; set; }

	// Token: 0x17000F11 RID: 3857
	// (get) Token: 0x06003F8E RID: 16270 RVA: 0x001208D3 File Offset: 0x0011ECD3
	// (set) Token: 0x06003F8F RID: 16271 RVA: 0x001208DB File Offset: 0x0011ECDB
	public Action TabCallback { get; set; }

	// Token: 0x17000F12 RID: 3858
	// (get) Token: 0x06003F90 RID: 16272 RVA: 0x001208E4 File Offset: 0x0011ECE4
	// (set) Token: 0x06003F91 RID: 16273 RVA: 0x001208EC File Offset: 0x0011ECEC
	public Action CursorOverCallback { get; set; }

	// Token: 0x17000F13 RID: 3859
	// (get) Token: 0x06003F92 RID: 16274 RVA: 0x001208F5 File Offset: 0x0011ECF5
	// (set) Token: 0x06003F93 RID: 16275 RVA: 0x001208FD File Offset: 0x0011ECFD
	public Action CursorOutCallback { get; set; }

	// Token: 0x06003F94 RID: 16276 RVA: 0x00120906 File Offset: 0x0011ED06
	protected override void Awake()
	{
		base.Awake();
		StaticInject.Inject(this);
	}

	// Token: 0x06003F95 RID: 16277 RVA: 0x00120914 File Offset: 0x0011ED14
	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		if (this.CursorOverCallback != null)
		{
			this.CursorOverCallback();
		}
	}

	// Token: 0x06003F96 RID: 16278 RVA: 0x00120933 File Offset: 0x0011ED33
	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		if (this.CursorOutCallback != null)
		{
			this.CursorOutCallback();
		}
	}

	// Token: 0x06003F97 RID: 16279 RVA: 0x00120952 File Offset: 0x0011ED52
	public override void OnSelect(BaseEventData eventData)
	{
	}

	// Token: 0x06003F98 RID: 16280 RVA: 0x00120954 File Offset: 0x0011ED54
	public override void OnDeselect(BaseEventData eventData)
	{
	}

	// Token: 0x06003F99 RID: 16281 RVA: 0x00120956 File Offset: 0x0011ED56
	public void SetController(WavedashTextEntry controller)
	{
		this.controller = controller;
	}

	// Token: 0x06003F9A RID: 16282 RVA: 0x0012095F File Offset: 0x0011ED5F
	public void OnSubmitPressed()
	{
		if (this._activated && this.EnterCallback != null)
		{
			this.EnterCallback();
		}
	}

	// Token: 0x06003F9B RID: 16283 RVA: 0x00120982 File Offset: 0x0011ED82
	public void OnTabPressed()
	{
		if (this._activated && this.TabCallback != null)
		{
			this.TabCallback();
		}
	}

	// Token: 0x06003F9C RID: 16284 RVA: 0x001209A5 File Offset: 0x0011EDA5
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

	// Token: 0x06003F9D RID: 16285 RVA: 0x001209BE File Offset: 0x0011EDBE
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

	// Token: 0x06003F9E RID: 16286 RVA: 0x001209E4 File Offset: 0x0011EDE4
	private void fixCaretAlignmentBug()
	{
		TMP_SelectionCaret componentInChildren = base.transform.parent.GetComponentInChildren<TMP_SelectionCaret>();
		Vector3 localPosition = componentInChildren.transform.localPosition;
		localPosition.y = 0f;
		localPosition.x = 0f;
		componentInChildren.transform.localPosition = localPosition;
	}

	// Token: 0x06003F9F RID: 16287 RVA: 0x00120A32 File Offset: 0x0011EE32
	private void deactivate()
	{
		if (this._activated)
		{
			this._activated = false;
			this.updateHighlightState();
			base.DeactivateInputField();
		}
	}

	// Token: 0x06003FA0 RID: 16288 RVA: 0x00120A52 File Offset: 0x0011EE52
	private void updateHighlightState()
	{
		this.controller.UpdateHighlightState();
	}

	// Token: 0x17000F14 RID: 3860
	// (get) Token: 0x06003FA1 RID: 16289 RVA: 0x00120A5F File Offset: 0x0011EE5F
	public bool Activated
	{
		get
		{
			return this._activated;
		}
	}

	// Token: 0x06003FA2 RID: 16290 RVA: 0x00120A67 File Offset: 0x0011EE67
	public void OnEndEdit()
	{
		if (this.EndEditCallback != null)
		{
			this.EndEditCallback();
		}
	}

	// Token: 0x06003FA3 RID: 16291 RVA: 0x00120A80 File Offset: 0x0011EE80
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

	// Token: 0x06003FA4 RID: 16292 RVA: 0x00120B2A File Offset: 0x0011EF2A
	public void Removed()
	{
		this.deactivate();
	}

	// Token: 0x06003FA5 RID: 16293 RVA: 0x00120B32 File Offset: 0x0011EF32
	protected override void OnDestroy()
	{
		this.EndEditCallback = null;
		base.OnDestroy();
	}

	// Token: 0x04002B1D RID: 11037
	private WavedashTextEntry controller;

	// Token: 0x04002B1E RID: 11038
	private bool _activated;

	// Token: 0x04002B1F RID: 11039
	private int prevTextLength;
}
