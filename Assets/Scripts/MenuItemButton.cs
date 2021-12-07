// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuItemButton : MonoBehaviour, IAnimatableButton
{
	public Action<MenuItemButton, BaseEventData> Select;

	public Action<MenuItemButton, BaseEventData> Deselect;

	public Action<MenuItemButton, InputEventData> Submit;

	public Action<MenuItemButton, InputEventData> RightClick;

	public Action<MenuItemButton, AxisEventData> Move;

	public Action<MenuItemButton, InputEventData> Drag;

	public Action<MenuItemButton, InputEventData> DragBegin;

	public Action<MenuItemButton, InputEventData> DragEnd;

	public Action<MenuItemButton, InputEventData> PointerEnter;

	public Action<MenuItemButton, InputEventData> PointerExit;

	public Action<MenuItemButton> KeyboardMoveSelection;

	public WavedashUIButton InteractableButton;

	public GameObject ScaleTarget;

	public Image ButtonBackground;

	public List<Image> AdditionalImages;

	public TextMeshProUGUI TextField;

	public CanvasGroup FadeCanvas;

	public bool ButtonEnabled = true;

	public float ScaleUpSize = 1.3f;

	public float ScaleUpTime = 0.07f;

	public float ScaleDownTime = 0.05f;

	public bool UseTextColorHighlight;

	public Color TextColorHighlight;

	public Image OverlayImage;

	public Image SwapImage;

	public Sprite SwapSprite;

	private ButtonAnimator _buttonAnimator;

	public Image ButtonBackgroundGet
	{
		get
		{
			return this.ButtonBackground;
		}
	}

	public List<Image> AdditionalImagesGet
	{
		get
		{
			return this.AdditionalImages;
		}
	}

	public TextMeshProUGUI TextFieldGet
	{
		get
		{
			return this.TextField;
		}
	}

	public CanvasGroup FadeCanvasGet
	{
		get
		{
			return this.FadeCanvas;
		}
	}

	public Color OriginalTextColor
	{
		get;
		set;
	}

	public bool IsFrozen
	{
		get;
		set;
	}

	public ButtonAnimator.VisualDisableType DisableType
	{
		get
		{
			return this.buttonAnimator.DisableType;
		}
		set
		{
			this.buttonAnimator.DisableType = value;
		}
	}

	public float DisableDuration
	{
		get
		{
			return this.buttonAnimator.DisableDuration;
		}
		set
		{
			this.buttonAnimator.DisableDuration = value;
		}
	}

	public IHighlightMode HighlightComponent
	{
		get;
		set;
	}

	public Vector2 GridLocationIndex
	{
		get;
		set;
	}

	private ButtonAnimator buttonAnimator
	{
		get
		{
			if (this._buttonAnimator == null)
			{
				this._buttonAnimator = new ButtonAnimator(this);
			}
			return this._buttonAnimator;
		}
	}

	public void Awake()
	{
		if (this.OverlayImage != null)
		{
			this.OverlayImage.gameObject.SetActive(false);
		}
		this.HighlightComponent = base.GetComponent<IHighlightMode>();
		if (this.InteractableButton == null)
		{
			throw new UnityException("Must have interactable button!! " + base.name);
		}
		WavedashUIButton expr_5B = this.InteractableButton;
		expr_5B.OnSelectEvent = (Action<BaseEventData>)Delegate.Combine(expr_5B.OnSelectEvent, new Action<BaseEventData>(this.onSelect));
		WavedashUIButton expr_82 = this.InteractableButton;
		expr_82.OnDeselectEvent = (Action<BaseEventData>)Delegate.Combine(expr_82.OnDeselectEvent, new Action<BaseEventData>(this.onDeselect));
		WavedashUIButton expr_A9 = this.InteractableButton;
		expr_A9.OnPointerClickEvent = (Action<InputEventData>)Delegate.Combine(expr_A9.OnPointerClickEvent, new Action<InputEventData>(this.onSubmit));
		WavedashUIButton expr_D0 = this.InteractableButton;
		expr_D0.OnSubmitEvent = (Action<InputEventData>)Delegate.Combine(expr_D0.OnSubmitEvent, new Action<InputEventData>(this.onSubmit));
		WavedashUIButton expr_F7 = this.InteractableButton;
		expr_F7.OnRightClickEvent = (Action<InputEventData>)Delegate.Combine(expr_F7.OnRightClickEvent, new Action<InputEventData>(this.onRightClick));
		WavedashUIButton expr_11E = this.InteractableButton;
		expr_11E.OnMoveEvent = (Action<AxisEventData>)Delegate.Combine(expr_11E.OnMoveEvent, new Action<AxisEventData>(this.onMoveEvent));
		WavedashUIButton expr_145 = this.InteractableButton;
		expr_145.OnDragEvent = (Action<InputEventData>)Delegate.Combine(expr_145.OnDragEvent, new Action<InputEventData>(this.onDrag));
		WavedashUIButton expr_16C = this.InteractableButton;
		expr_16C.OnDragBeginEvent = (Action<InputEventData>)Delegate.Combine(expr_16C.OnDragBeginEvent, new Action<InputEventData>(this.onDragBegin));
		WavedashUIButton expr_193 = this.InteractableButton;
		expr_193.OnDragEndEvent = (Action<InputEventData>)Delegate.Combine(expr_193.OnDragEndEvent, new Action<InputEventData>(this.onDragEnd));
		WavedashUIButton expr_1BA = this.InteractableButton;
		expr_1BA.OnPointerEnterEvent = (Action<InputEventData>)Delegate.Combine(expr_1BA.OnPointerEnterEvent, new Action<InputEventData>(this.onPointerEnter));
		WavedashUIButton expr_1E1 = this.InteractableButton;
		expr_1E1.OnPointerExitEvent = (Action<InputEventData>)Delegate.Combine(expr_1E1.OnPointerExitEvent, new Action<InputEventData>(this.onPointerExit));
	}

	protected virtual void OnDestroy()
	{
		if (this.InteractableButton == null)
		{
			throw new UnityException("Must have interactable button!! " + base.name);
		}
		WavedashUIButton expr_2D = this.InteractableButton;
		expr_2D.OnSelectEvent = (Action<BaseEventData>)Delegate.Remove(expr_2D.OnSelectEvent, new Action<BaseEventData>(this.onSelect));
		WavedashUIButton expr_54 = this.InteractableButton;
		expr_54.OnDeselectEvent = (Action<BaseEventData>)Delegate.Remove(expr_54.OnDeselectEvent, new Action<BaseEventData>(this.onDeselect));
		WavedashUIButton expr_7B = this.InteractableButton;
		expr_7B.OnPointerClickEvent = (Action<InputEventData>)Delegate.Remove(expr_7B.OnPointerClickEvent, new Action<InputEventData>(this.onSubmit));
		WavedashUIButton expr_A2 = this.InteractableButton;
		expr_A2.OnSubmitEvent = (Action<InputEventData>)Delegate.Remove(expr_A2.OnSubmitEvent, new Action<InputEventData>(this.onSubmit));
		WavedashUIButton expr_C9 = this.InteractableButton;
		expr_C9.OnRightClickEvent = (Action<InputEventData>)Delegate.Remove(expr_C9.OnRightClickEvent, new Action<InputEventData>(this.onRightClick));
		WavedashUIButton expr_F0 = this.InteractableButton;
		expr_F0.OnMoveEvent = (Action<AxisEventData>)Delegate.Remove(expr_F0.OnMoveEvent, new Action<AxisEventData>(this.onMoveEvent));
		WavedashUIButton expr_117 = this.InteractableButton;
		expr_117.OnDragEvent = (Action<InputEventData>)Delegate.Remove(expr_117.OnDragEvent, new Action<InputEventData>(this.onDrag));
		WavedashUIButton expr_13E = this.InteractableButton;
		expr_13E.OnDragBeginEvent = (Action<InputEventData>)Delegate.Remove(expr_13E.OnDragBeginEvent, new Action<InputEventData>(this.onDragBegin));
		WavedashUIButton expr_165 = this.InteractableButton;
		expr_165.OnDragEndEvent = (Action<InputEventData>)Delegate.Remove(expr_165.OnDragEndEvent, new Action<InputEventData>(this.onDragEnd));
		WavedashUIButton expr_18C = this.InteractableButton;
		expr_18C.OnPointerEnterEvent = (Action<InputEventData>)Delegate.Remove(expr_18C.OnPointerEnterEvent, new Action<InputEventData>(this.onPointerEnter));
		WavedashUIButton expr_1B3 = this.InteractableButton;
		expr_1B3.OnPointerExitEvent = (Action<InputEventData>)Delegate.Remove(expr_1B3.OnPointerExitEvent, new Action<InputEventData>(this.onPointerExit));
		this.buttonAnimator.OnDestroy();
	}

	public void SetInteractable(bool value)
	{
		this.InteractableButton.interactable = value;
		if (!value)
		{
			this.InteractableButton.OnDeselect(null);
			this.buttonAnimator.PlayDisable();
		}
		else
		{
			this.buttonAnimator.PlayEnable();
		}
	}

	public void Freeze()
	{
		this.IsFrozen = true;
	}

	public void Unfreeze()
	{
		this.IsFrozen = false;
	}

	public void SetText(string text)
	{
		this.TextField.text = text;
		base.name = text;
	}

	private void onSelect(BaseEventData eventData)
	{
		if (this.Select != null)
		{
			this.Select(this, eventData);
		}
	}

	private void onDeselect(BaseEventData eventData)
	{
		if (this.Deselect != null)
		{
			this.Deselect(this, eventData);
		}
	}

	private void onSubmit(InputEventData data)
	{
		if (this.Submit != null)
		{
			this.Submit(this, data);
		}
	}

	private void onPointerEnter(InputEventData data)
	{
		if (this.PointerEnter != null)
		{
			this.PointerEnter(this, data);
		}
	}

	private void onPointerExit(InputEventData data)
	{
		if (this.PointerExit != null)
		{
			this.PointerExit(this, data);
		}
	}

	private void onDrag(InputEventData data)
	{
		if (this.Drag != null)
		{
			this.Drag(this, data);
		}
	}

	private void onDragBegin(InputEventData data)
	{
		if (this.DragBegin != null)
		{
			this.DragBegin(this, data);
		}
	}

	private void onDragEnd(InputEventData data)
	{
		if (this.DragEnd != null)
		{
			this.DragEnd(this, data);
		}
	}

	private void onRightClick(InputEventData data)
	{
		if (this.RightClick != null)
		{
			this.RightClick(this, data);
		}
	}

	private void onMoveEvent(AxisEventData eventData)
	{
		if (this.Move != null)
		{
			this.Move(this, eventData);
		}
	}

	public void OnKeyboardMoveSelection()
	{
		if (this.KeyboardMoveSelection != null)
		{
			this.KeyboardMoveSelection(this);
		}
	}
}
