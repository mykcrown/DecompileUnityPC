// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WavedashUIButton : Button, IDeviceSubmitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IEventSystemHandler
{
	public bool Unselectable;

	public Action<BaseEventData> OnSelectEvent;

	public Action<BaseEventData> OnDeselectEvent;

	public Action<InputEventData> OnSubmitEvent;

	public Action<InputEventData> OnRightClickEvent;

	public Action<InputEventData> OnPointerClickEvent;

	public Action<InputEventData> OnPointerEnterEvent;

	public Action<InputEventData> OnPointerExitEvent;

	public Action<AxisEventData> OnMoveEvent;

	public Action<InputEventData> OnDragEvent;

	public Action<InputEventData> OnDragBeginEvent;

	public Action<InputEventData> OnDragEndEvent;

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	public bool UseOverrideHighlightSound
	{
		get;
		set;
	}

	public AudioData OverrideHighlightSound
	{
		get;
		set;
	}

	public AudioData SubmitSound
	{
		get;
		set;
	}

	protected override void Awake()
	{
		base.Awake();
		StaticInject.Inject(this);
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		if (this.OnDeselectEvent != null)
		{
			this.OnDeselectEvent(eventData);
		}
	}

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

	public override void OnMove(AxisEventData eventData)
	{
		if (this.OnMoveEvent != null)
		{
			this.OnMoveEvent(eventData);
		}
	}

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
}
