using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000946 RID: 2374
public class MenuItemButton : MonoBehaviour, IAnimatableButton
{
	// Token: 0x17000EE0 RID: 3808
	// (get) Token: 0x06003EAF RID: 16047 RVA: 0x0011D484 File Offset: 0x0011B884
	public Image ButtonBackgroundGet
	{
		get
		{
			return this.ButtonBackground;
		}
	}

	// Token: 0x17000EE1 RID: 3809
	// (get) Token: 0x06003EB0 RID: 16048 RVA: 0x0011D48C File Offset: 0x0011B88C
	public List<Image> AdditionalImagesGet
	{
		get
		{
			return this.AdditionalImages;
		}
	}

	// Token: 0x17000EE2 RID: 3810
	// (get) Token: 0x06003EB1 RID: 16049 RVA: 0x0011D494 File Offset: 0x0011B894
	public TextMeshProUGUI TextFieldGet
	{
		get
		{
			return this.TextField;
		}
	}

	// Token: 0x17000EE3 RID: 3811
	// (get) Token: 0x06003EB2 RID: 16050 RVA: 0x0011D49C File Offset: 0x0011B89C
	public CanvasGroup FadeCanvasGet
	{
		get
		{
			return this.FadeCanvas;
		}
	}

	// Token: 0x17000EE4 RID: 3812
	// (get) Token: 0x06003EB3 RID: 16051 RVA: 0x0011D4A4 File Offset: 0x0011B8A4
	// (set) Token: 0x06003EB4 RID: 16052 RVA: 0x0011D4AC File Offset: 0x0011B8AC
	public Color OriginalTextColor { get; set; }

	// Token: 0x17000EE5 RID: 3813
	// (get) Token: 0x06003EB5 RID: 16053 RVA: 0x0011D4B5 File Offset: 0x0011B8B5
	// (set) Token: 0x06003EB6 RID: 16054 RVA: 0x0011D4BD File Offset: 0x0011B8BD
	public bool IsFrozen { get; set; }

	// Token: 0x17000EE6 RID: 3814
	// (get) Token: 0x06003EB7 RID: 16055 RVA: 0x0011D4C6 File Offset: 0x0011B8C6
	// (set) Token: 0x06003EB8 RID: 16056 RVA: 0x0011D4D3 File Offset: 0x0011B8D3
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

	// Token: 0x17000EE7 RID: 3815
	// (get) Token: 0x06003EB9 RID: 16057 RVA: 0x0011D4E1 File Offset: 0x0011B8E1
	// (set) Token: 0x06003EBA RID: 16058 RVA: 0x0011D4EE File Offset: 0x0011B8EE
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

	// Token: 0x17000EE8 RID: 3816
	// (get) Token: 0x06003EBB RID: 16059 RVA: 0x0011D4FC File Offset: 0x0011B8FC
	// (set) Token: 0x06003EBC RID: 16060 RVA: 0x0011D504 File Offset: 0x0011B904
	public IHighlightMode HighlightComponent { get; set; }

	// Token: 0x17000EE9 RID: 3817
	// (get) Token: 0x06003EBD RID: 16061 RVA: 0x0011D50D File Offset: 0x0011B90D
	// (set) Token: 0x06003EBE RID: 16062 RVA: 0x0011D515 File Offset: 0x0011B915
	public Vector2 GridLocationIndex { get; set; }

	// Token: 0x17000EEA RID: 3818
	// (get) Token: 0x06003EBF RID: 16063 RVA: 0x0011D51E File Offset: 0x0011B91E
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

	// Token: 0x06003EC0 RID: 16064 RVA: 0x0011D540 File Offset: 0x0011B940
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
		WavedashUIButton interactableButton = this.InteractableButton;
		interactableButton.OnSelectEvent = (Action<BaseEventData>)Delegate.Combine(interactableButton.OnSelectEvent, new Action<BaseEventData>(this.onSelect));
		WavedashUIButton interactableButton2 = this.InteractableButton;
		interactableButton2.OnDeselectEvent = (Action<BaseEventData>)Delegate.Combine(interactableButton2.OnDeselectEvent, new Action<BaseEventData>(this.onDeselect));
		WavedashUIButton interactableButton3 = this.InteractableButton;
		interactableButton3.OnPointerClickEvent = (Action<InputEventData>)Delegate.Combine(interactableButton3.OnPointerClickEvent, new Action<InputEventData>(this.onSubmit));
		WavedashUIButton interactableButton4 = this.InteractableButton;
		interactableButton4.OnSubmitEvent = (Action<InputEventData>)Delegate.Combine(interactableButton4.OnSubmitEvent, new Action<InputEventData>(this.onSubmit));
		WavedashUIButton interactableButton5 = this.InteractableButton;
		interactableButton5.OnRightClickEvent = (Action<InputEventData>)Delegate.Combine(interactableButton5.OnRightClickEvent, new Action<InputEventData>(this.onRightClick));
		WavedashUIButton interactableButton6 = this.InteractableButton;
		interactableButton6.OnMoveEvent = (Action<AxisEventData>)Delegate.Combine(interactableButton6.OnMoveEvent, new Action<AxisEventData>(this.onMoveEvent));
		WavedashUIButton interactableButton7 = this.InteractableButton;
		interactableButton7.OnDragEvent = (Action<InputEventData>)Delegate.Combine(interactableButton7.OnDragEvent, new Action<InputEventData>(this.onDrag));
		WavedashUIButton interactableButton8 = this.InteractableButton;
		interactableButton8.OnDragBeginEvent = (Action<InputEventData>)Delegate.Combine(interactableButton8.OnDragBeginEvent, new Action<InputEventData>(this.onDragBegin));
		WavedashUIButton interactableButton9 = this.InteractableButton;
		interactableButton9.OnDragEndEvent = (Action<InputEventData>)Delegate.Combine(interactableButton9.OnDragEndEvent, new Action<InputEventData>(this.onDragEnd));
		WavedashUIButton interactableButton10 = this.InteractableButton;
		interactableButton10.OnPointerEnterEvent = (Action<InputEventData>)Delegate.Combine(interactableButton10.OnPointerEnterEvent, new Action<InputEventData>(this.onPointerEnter));
		WavedashUIButton interactableButton11 = this.InteractableButton;
		interactableButton11.OnPointerExitEvent = (Action<InputEventData>)Delegate.Combine(interactableButton11.OnPointerExitEvent, new Action<InputEventData>(this.onPointerExit));
	}

	// Token: 0x06003EC1 RID: 16065 RVA: 0x0011D750 File Offset: 0x0011BB50
	protected virtual void OnDestroy()
	{
		if (this.InteractableButton == null)
		{
			throw new UnityException("Must have interactable button!! " + base.name);
		}
		WavedashUIButton interactableButton = this.InteractableButton;
		interactableButton.OnSelectEvent = (Action<BaseEventData>)Delegate.Remove(interactableButton.OnSelectEvent, new Action<BaseEventData>(this.onSelect));
		WavedashUIButton interactableButton2 = this.InteractableButton;
		interactableButton2.OnDeselectEvent = (Action<BaseEventData>)Delegate.Remove(interactableButton2.OnDeselectEvent, new Action<BaseEventData>(this.onDeselect));
		WavedashUIButton interactableButton3 = this.InteractableButton;
		interactableButton3.OnPointerClickEvent = (Action<InputEventData>)Delegate.Remove(interactableButton3.OnPointerClickEvent, new Action<InputEventData>(this.onSubmit));
		WavedashUIButton interactableButton4 = this.InteractableButton;
		interactableButton4.OnSubmitEvent = (Action<InputEventData>)Delegate.Remove(interactableButton4.OnSubmitEvent, new Action<InputEventData>(this.onSubmit));
		WavedashUIButton interactableButton5 = this.InteractableButton;
		interactableButton5.OnRightClickEvent = (Action<InputEventData>)Delegate.Remove(interactableButton5.OnRightClickEvent, new Action<InputEventData>(this.onRightClick));
		WavedashUIButton interactableButton6 = this.InteractableButton;
		interactableButton6.OnMoveEvent = (Action<AxisEventData>)Delegate.Remove(interactableButton6.OnMoveEvent, new Action<AxisEventData>(this.onMoveEvent));
		WavedashUIButton interactableButton7 = this.InteractableButton;
		interactableButton7.OnDragEvent = (Action<InputEventData>)Delegate.Remove(interactableButton7.OnDragEvent, new Action<InputEventData>(this.onDrag));
		WavedashUIButton interactableButton8 = this.InteractableButton;
		interactableButton8.OnDragBeginEvent = (Action<InputEventData>)Delegate.Remove(interactableButton8.OnDragBeginEvent, new Action<InputEventData>(this.onDragBegin));
		WavedashUIButton interactableButton9 = this.InteractableButton;
		interactableButton9.OnDragEndEvent = (Action<InputEventData>)Delegate.Remove(interactableButton9.OnDragEndEvent, new Action<InputEventData>(this.onDragEnd));
		WavedashUIButton interactableButton10 = this.InteractableButton;
		interactableButton10.OnPointerEnterEvent = (Action<InputEventData>)Delegate.Remove(interactableButton10.OnPointerEnterEvent, new Action<InputEventData>(this.onPointerEnter));
		WavedashUIButton interactableButton11 = this.InteractableButton;
		interactableButton11.OnPointerExitEvent = (Action<InputEventData>)Delegate.Remove(interactableButton11.OnPointerExitEvent, new Action<InputEventData>(this.onPointerExit));
		this.buttonAnimator.OnDestroy();
	}

	// Token: 0x06003EC2 RID: 16066 RVA: 0x0011D93C File Offset: 0x0011BD3C
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

	// Token: 0x06003EC3 RID: 16067 RVA: 0x0011D977 File Offset: 0x0011BD77
	public void Freeze()
	{
		this.IsFrozen = true;
	}

	// Token: 0x06003EC4 RID: 16068 RVA: 0x0011D980 File Offset: 0x0011BD80
	public void Unfreeze()
	{
		this.IsFrozen = false;
	}

	// Token: 0x06003EC5 RID: 16069 RVA: 0x0011D989 File Offset: 0x0011BD89
	public void SetText(string text)
	{
		this.TextField.text = text;
		base.name = text;
	}

	// Token: 0x06003EC6 RID: 16070 RVA: 0x0011D99E File Offset: 0x0011BD9E
	private void onSelect(BaseEventData eventData)
	{
		if (this.Select != null)
		{
			this.Select(this, eventData);
		}
	}

	// Token: 0x06003EC7 RID: 16071 RVA: 0x0011D9B8 File Offset: 0x0011BDB8
	private void onDeselect(BaseEventData eventData)
	{
		if (this.Deselect != null)
		{
			this.Deselect(this, eventData);
		}
	}

	// Token: 0x06003EC8 RID: 16072 RVA: 0x0011D9D2 File Offset: 0x0011BDD2
	private void onSubmit(InputEventData data)
	{
		if (this.Submit != null)
		{
			this.Submit(this, data);
		}
	}

	// Token: 0x06003EC9 RID: 16073 RVA: 0x0011D9EC File Offset: 0x0011BDEC
	private void onPointerEnter(InputEventData data)
	{
		if (this.PointerEnter != null)
		{
			this.PointerEnter(this, data);
		}
	}

	// Token: 0x06003ECA RID: 16074 RVA: 0x0011DA06 File Offset: 0x0011BE06
	private void onPointerExit(InputEventData data)
	{
		if (this.PointerExit != null)
		{
			this.PointerExit(this, data);
		}
	}

	// Token: 0x06003ECB RID: 16075 RVA: 0x0011DA20 File Offset: 0x0011BE20
	private void onDrag(InputEventData data)
	{
		if (this.Drag != null)
		{
			this.Drag(this, data);
		}
	}

	// Token: 0x06003ECC RID: 16076 RVA: 0x0011DA3A File Offset: 0x0011BE3A
	private void onDragBegin(InputEventData data)
	{
		if (this.DragBegin != null)
		{
			this.DragBegin(this, data);
		}
	}

	// Token: 0x06003ECD RID: 16077 RVA: 0x0011DA54 File Offset: 0x0011BE54
	private void onDragEnd(InputEventData data)
	{
		if (this.DragEnd != null)
		{
			this.DragEnd(this, data);
		}
	}

	// Token: 0x06003ECE RID: 16078 RVA: 0x0011DA6E File Offset: 0x0011BE6E
	private void onRightClick(InputEventData data)
	{
		if (this.RightClick != null)
		{
			this.RightClick(this, data);
		}
	}

	// Token: 0x06003ECF RID: 16079 RVA: 0x0011DA88 File Offset: 0x0011BE88
	private void onMoveEvent(AxisEventData eventData)
	{
		if (this.Move != null)
		{
			this.Move(this, eventData);
		}
	}

	// Token: 0x06003ED0 RID: 16080 RVA: 0x0011DAA2 File Offset: 0x0011BEA2
	public void OnKeyboardMoveSelection()
	{
		if (this.KeyboardMoveSelection != null)
		{
			this.KeyboardMoveSelection(this);
		}
	}

	// Token: 0x04002A80 RID: 10880
	public Action<MenuItemButton, BaseEventData> Select;

	// Token: 0x04002A81 RID: 10881
	public Action<MenuItemButton, BaseEventData> Deselect;

	// Token: 0x04002A82 RID: 10882
	public Action<MenuItemButton, InputEventData> Submit;

	// Token: 0x04002A83 RID: 10883
	public Action<MenuItemButton, InputEventData> RightClick;

	// Token: 0x04002A84 RID: 10884
	public Action<MenuItemButton, AxisEventData> Move;

	// Token: 0x04002A85 RID: 10885
	public Action<MenuItemButton, InputEventData> Drag;

	// Token: 0x04002A86 RID: 10886
	public Action<MenuItemButton, InputEventData> DragBegin;

	// Token: 0x04002A87 RID: 10887
	public Action<MenuItemButton, InputEventData> DragEnd;

	// Token: 0x04002A88 RID: 10888
	public Action<MenuItemButton, InputEventData> PointerEnter;

	// Token: 0x04002A89 RID: 10889
	public Action<MenuItemButton, InputEventData> PointerExit;

	// Token: 0x04002A8A RID: 10890
	public Action<MenuItemButton> KeyboardMoveSelection;

	// Token: 0x04002A8B RID: 10891
	public WavedashUIButton InteractableButton;

	// Token: 0x04002A8C RID: 10892
	public GameObject ScaleTarget;

	// Token: 0x04002A8D RID: 10893
	public Image ButtonBackground;

	// Token: 0x04002A8E RID: 10894
	public List<Image> AdditionalImages;

	// Token: 0x04002A8F RID: 10895
	public TextMeshProUGUI TextField;

	// Token: 0x04002A90 RID: 10896
	public CanvasGroup FadeCanvas;

	// Token: 0x04002A91 RID: 10897
	public bool ButtonEnabled = true;

	// Token: 0x04002A92 RID: 10898
	public float ScaleUpSize = 1.3f;

	// Token: 0x04002A93 RID: 10899
	public float ScaleUpTime = 0.07f;

	// Token: 0x04002A94 RID: 10900
	public float ScaleDownTime = 0.05f;

	// Token: 0x04002A95 RID: 10901
	public bool UseTextColorHighlight;

	// Token: 0x04002A96 RID: 10902
	public Color TextColorHighlight;

	// Token: 0x04002A97 RID: 10903
	public Image OverlayImage;

	// Token: 0x04002A98 RID: 10904
	public Image SwapImage;

	// Token: 0x04002A99 RID: 10905
	public Sprite SwapSprite;

	// Token: 0x04002A9E RID: 10910
	private ButtonAnimator _buttonAnimator;
}
