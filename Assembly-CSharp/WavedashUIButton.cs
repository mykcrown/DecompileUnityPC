using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200095C RID: 2396
public class WavedashUIButton : Button, IDeviceSubmitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IEventSystemHandler
{
	// Token: 0x17000F15 RID: 3861
	// (get) Token: 0x06003FAB RID: 16299 RVA: 0x00120B5B File Offset: 0x0011EF5B
	// (set) Token: 0x06003FAC RID: 16300 RVA: 0x00120B63 File Offset: 0x0011EF63
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x17000F16 RID: 3862
	// (get) Token: 0x06003FAD RID: 16301 RVA: 0x00120B6C File Offset: 0x0011EF6C
	// (set) Token: 0x06003FAE RID: 16302 RVA: 0x00120B74 File Offset: 0x0011EF74
	public bool UseOverrideHighlightSound { get; set; }

	// Token: 0x17000F17 RID: 3863
	// (get) Token: 0x06003FAF RID: 16303 RVA: 0x00120B7D File Offset: 0x0011EF7D
	// (set) Token: 0x06003FB0 RID: 16304 RVA: 0x00120B85 File Offset: 0x0011EF85
	public AudioData OverrideHighlightSound { get; set; }

	// Token: 0x17000F18 RID: 3864
	// (get) Token: 0x06003FB1 RID: 16305 RVA: 0x00120B8E File Offset: 0x0011EF8E
	// (set) Token: 0x06003FB2 RID: 16306 RVA: 0x00120B96 File Offset: 0x0011EF96
	public AudioData SubmitSound { get; set; }

	// Token: 0x06003FB3 RID: 16307 RVA: 0x00120B9F File Offset: 0x0011EF9F
	protected override void Awake()
	{
		base.Awake();
		StaticInject.Inject(this);
	}

	// Token: 0x06003FB4 RID: 16308 RVA: 0x00120BAD File Offset: 0x0011EFAD
	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		if (this.OnDeselectEvent != null)
		{
			this.OnDeselectEvent(eventData);
		}
	}

	// Token: 0x06003FB5 RID: 16309 RVA: 0x00120BD0 File Offset: 0x0011EFD0
	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		if (!base.interactable)
		{
			return;
		}
		if (this.UseOverrideHighlightSound)
		{
			this.audioManager.PlayMenuSound(this.OverrideHighlightSound, 0f);
		}
		else
		{
			this.audioManager.PlayMenuSound(SoundKey.generic_buttonHighlight, 0f);
		}
		if (this.OnSelectEvent != null)
		{
			this.OnSelectEvent(eventData);
		}
	}

	// Token: 0x06003FB6 RID: 16310 RVA: 0x00120C3E File Offset: 0x0011F03E
	public void OnSubmit(InputEventData data)
	{
		if (!base.interactable)
		{
			return;
		}
		this.audioManager.PlayMenuSound(this.SubmitSound, 0f);
		if (this.OnSubmitEvent != null)
		{
			this.OnSubmitEvent(data);
		}
	}

	// Token: 0x06003FB7 RID: 16311 RVA: 0x00120C7C File Offset: 0x0011F07C
	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			if (this.OnRightClickEvent != null)
			{
				InputEventData obj = default(InputEventData);
				obj.isMouseEvent = true;
				obj.mousePosition = eventData.position;
				this.OnRightClickEvent(obj);
			}
		}
		else if (this.OnPointerClickEvent != null)
		{
			InputEventData obj2 = default(InputEventData);
			obj2.isMouseEvent = true;
			obj2.mousePosition = eventData.position;
			this.OnPointerClickEvent(obj2);
		}
	}

	// Token: 0x06003FB8 RID: 16312 RVA: 0x00120D0C File Offset: 0x0011F10C
	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		if (this.OnPointerEnterEvent != null)
		{
			InputEventData obj = default(InputEventData);
			obj.isMouseEvent = true;
			obj.mousePosition = eventData.position;
			this.OnPointerEnterEvent(obj);
		}
	}

	// Token: 0x06003FB9 RID: 16313 RVA: 0x00120D54 File Offset: 0x0011F154
	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		if (this.OnPointerExitEvent != null)
		{
			InputEventData obj = default(InputEventData);
			obj.isMouseEvent = true;
			obj.mousePosition = eventData.position;
			this.OnPointerExitEvent(obj);
		}
	}

	// Token: 0x06003FBA RID: 16314 RVA: 0x00120D9C File Offset: 0x0011F19C
	public override void OnMove(AxisEventData eventData)
	{
		if (this.OnMoveEvent != null)
		{
			this.OnMoveEvent(eventData);
		}
	}

	// Token: 0x06003FBB RID: 16315 RVA: 0x00120DB8 File Offset: 0x0011F1B8
	public void OnDrag(PointerEventData eventData)
	{
		if (this.OnDragEvent != null)
		{
			InputEventData obj = default(InputEventData);
			obj.isMouseEvent = true;
			obj.mousePosition = eventData.position;
			this.OnDragEvent(obj);
		}
	}

	// Token: 0x06003FBC RID: 16316 RVA: 0x00120DFC File Offset: 0x0011F1FC
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (this.OnDragBeginEvent != null)
		{
			InputEventData obj = default(InputEventData);
			obj.isMouseEvent = true;
			obj.mousePosition = eventData.position;
			this.OnDragBeginEvent(obj);
		}
	}

	// Token: 0x06003FBD RID: 16317 RVA: 0x00120E40 File Offset: 0x0011F240
	public void OnEndDrag(PointerEventData eventData)
	{
		if (this.OnDragEndEvent != null)
		{
			InputEventData obj = default(InputEventData);
			obj.isMouseEvent = true;
			obj.mousePosition = eventData.position;
			this.OnDragEndEvent(obj);
		}
	}

	// Token: 0x04002B21 RID: 11041
	public bool Unselectable;

	// Token: 0x04002B22 RID: 11042
	public Action<BaseEventData> OnSelectEvent;

	// Token: 0x04002B23 RID: 11043
	public Action<BaseEventData> OnDeselectEvent;

	// Token: 0x04002B24 RID: 11044
	public Action<InputEventData> OnSubmitEvent;

	// Token: 0x04002B25 RID: 11045
	public Action<InputEventData> OnRightClickEvent;

	// Token: 0x04002B26 RID: 11046
	public Action<InputEventData> OnPointerClickEvent;

	// Token: 0x04002B27 RID: 11047
	public Action<InputEventData> OnPointerEnterEvent;

	// Token: 0x04002B28 RID: 11048
	public Action<InputEventData> OnPointerExitEvent;

	// Token: 0x04002B29 RID: 11049
	public Action<AxisEventData> OnMoveEvent;

	// Token: 0x04002B2A RID: 11050
	public Action<InputEventData> OnDragEvent;

	// Token: 0x04002B2B RID: 11051
	public Action<InputEventData> OnDragBeginEvent;

	// Token: 0x04002B2C RID: 11052
	public Action<InputEventData> OnDragEndEvent;
}
