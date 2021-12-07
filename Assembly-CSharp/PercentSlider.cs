using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200094A RID: 2378
public class PercentSlider : ClientBehavior
{
	// Token: 0x17000EF3 RID: 3827
	// (get) Token: 0x06003F16 RID: 16150 RVA: 0x0011ED88 File Offset: 0x0011D188
	// (set) Token: 0x06003F17 RID: 16151 RVA: 0x0011ED90 File Offset: 0x0011D190
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x17000EF4 RID: 3828
	// (get) Token: 0x06003F18 RID: 16152 RVA: 0x0011ED99 File Offset: 0x0011D199
	// (set) Token: 0x06003F19 RID: 16153 RVA: 0x0011EDA1 File Offset: 0x0011D1A1
	public Func<float> PercentGetter { get; private set; }

	// Token: 0x17000EF5 RID: 3829
	// (get) Token: 0x06003F1A RID: 16154 RVA: 0x0011EDAA File Offset: 0x0011D1AA
	// (set) Token: 0x06003F1B RID: 16155 RVA: 0x0011EDB2 File Offset: 0x0011D1B2
	public Action<float> PercentSettter { get; private set; }

	// Token: 0x17000EF6 RID: 3830
	// (get) Token: 0x06003F1C RID: 16156 RVA: 0x0011EDBB File Offset: 0x0011D1BB
	// (set) Token: 0x06003F1D RID: 16157 RVA: 0x0011EDC3 File Offset: 0x0011D1C3
	public bool IsDragging { get; private set; }

	// Token: 0x06003F1E RID: 16158 RVA: 0x0011EDCC File Offset: 0x0011D1CC
	public void Initialize(Func<float> getter, Action<float> setter, MenuItemList buttonList)
	{
		this.PercentGetter = getter;
		this.PercentSettter = setter;
		this.IsDragging = false;
		this.SetValue(getter());
		buttonList.AddButton(this.Button, delegate(InputEventData eventData)
		{
			if (eventData.isMouseEvent)
			{
				setter(this.GetValue(eventData.mousePosition));
			}
			if (this.onSubmit != null)
			{
				this.onSubmit(eventData);
			}
		});
		MenuItemButton button4 = this.Button;
		button4.DragBegin = (Action<MenuItemButton, InputEventData>)Delegate.Combine(button4.DragBegin, new Action<MenuItemButton, InputEventData>(delegate(MenuItemButton button, InputEventData eventData)
		{
			if (buttonList.CurrentSelection != button)
			{
				buttonList.AutoSelect(button);
			}
			buttonList.Lock();
			this.IsDragging = true;
		}));
		MenuItemButton button2 = this.Button;
		button2.Drag = (Action<MenuItemButton, InputEventData>)Delegate.Combine(button2.Drag, new Action<MenuItemButton, InputEventData>(delegate(MenuItemButton button, InputEventData eventData)
		{
			setter(this.GetValue(eventData.mousePosition));
		}));
		MenuItemButton button3 = this.Button;
		button3.DragEnd = (Action<MenuItemButton, InputEventData>)Delegate.Combine(button3.DragEnd, new Action<MenuItemButton, InputEventData>(delegate(MenuItemButton button, InputEventData eventData)
		{
			buttonList.Unlock();
			this.IsDragging = false;
		}));
		MenuItemList buttonList2 = buttonList;
		buttonList2.OnSelected = (Action<MenuItemButton, BaseEventData>)Delegate.Combine(buttonList2.OnSelected, new Action<MenuItemButton, BaseEventData>(delegate(MenuItemButton button, BaseEventData eventData)
		{
			if (button == this.Button)
			{
				this.setSelectDelayDuration(false);
			}
		}));
		MenuItemList buttonList3 = buttonList;
		buttonList3.OnDeselected = (Action<MenuItemButton, BaseEventData>)Delegate.Combine(buttonList3.OnDeselected, new Action<MenuItemButton, BaseEventData>(delegate(MenuItemButton button, BaseEventData eventData)
		{
			if (button == this.Button)
			{
				this.setSelectDelayDuration(true);
			}
		}));
		if (this.BackgroundImage != null)
		{
			this.BackgroundImage.material = UnityEngine.Object.Instantiate<Material>(this.BackgroundImage.material);
		}
	}

	// Token: 0x06003F1F RID: 16159 RVA: 0x0011EF28 File Offset: 0x0011D328
	public void SetValue(float value)
	{
		RectTransform rectTransform = (RectTransform)this.SliderTick.transform;
		RectTransform rectTransform2 = (RectTransform)this.SliderTick.transform.parent;
		Vector3 localPosition = rectTransform.localPosition;
		localPosition.x = rectTransform2.rect.width * (value - 0.5f);
		rectTransform.localPosition = localPosition;
		int num = (int)Math.Round((double)(value * 100f));
		this.PercentText.text = string.Format("{0}%", num);
	}

	// Token: 0x06003F20 RID: 16160 RVA: 0x0011EFB4 File Offset: 0x0011D3B4
	public float GetValue(Vector2 position)
	{
		RectTransform rectTransform = (RectTransform)this.SliderTick.transform;
		RectTransform rectTransform2 = (RectTransform)this.SliderTick.transform.parent;
		float num = rectTransform2.rect.width * rectTransform2.lossyScale.x;
		float num2 = position.x - rectTransform2.position.x + num * 0.5f;
		return Mathf.Clamp01(num2 / num);
	}

	// Token: 0x06003F21 RID: 16161 RVA: 0x0011F035 File Offset: 0x0011D435
	private void killTween()
	{
		TweenUtil.Destroy(ref this.tween);
	}

	// Token: 0x06003F22 RID: 16162 RVA: 0x0011F044 File Offset: 0x0011D444
	public void SetFlashing(bool isFlashing)
	{
		if (isFlashing != this.isFlashing)
		{
			this.isFlashing = isFlashing;
			if (isFlashing)
			{
				if (this.tween == null)
				{
					this.flashAmount = 0.2f;
					this.tween = DOTween.To(new DOGetter<float>(this.get_flashAmount), delegate(float x)
					{
						this.flashAmount = x;
					}, 0f, 0.5f).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo);
				}
			}
			else
			{
				TweenUtil.Destroy(ref this.tween);
				this.flashAmount = 0f;
			}
		}
	}

	// Token: 0x06003F23 RID: 16163 RVA: 0x0011F0D8 File Offset: 0x0011D4D8
	private void setSelectDelayDuration(bool reset)
	{
		UIInputModule uiinputModule = this.uiManager.CurrentInputModule as UIInputModule;
		if (reset)
		{
			uiinputModule.ResetHorizontalRepeatDelay();
		}
		else
		{
			uiinputModule.SetHorizontalRepeatDelay(0.01f);
		}
	}

	// Token: 0x17000EF7 RID: 3831
	// (get) Token: 0x06003F24 RID: 16164 RVA: 0x0011F112 File Offset: 0x0011D512
	// (set) Token: 0x06003F25 RID: 16165 RVA: 0x0011F11A File Offset: 0x0011D51A
	private float flashAmount
	{
		get
		{
			return this._flashAmount;
		}
		set
		{
			this._flashAmount = value;
			if (this.BackgroundImage != null)
			{
				this.BackgroundImage.material.SetFloat("_FlashAmount", this._flashAmount);
			}
		}
	}

	// Token: 0x04002ABE RID: 10942
	public TextMeshProUGUI Title;

	// Token: 0x04002ABF RID: 10943
	public TextMeshProUGUI PercentText;

	// Token: 0x04002AC0 RID: 10944
	public Transform SliderTick;

	// Token: 0x04002AC1 RID: 10945
	public MenuItemButton Button;

	// Token: 0x04002AC2 RID: 10946
	public Image BackgroundImage;

	// Token: 0x04002AC3 RID: 10947
	public Action<InputEventData> onSubmit;

	// Token: 0x04002AC7 RID: 10951
	public const float SLIDER_REPEAT_DELAY_DURATION = 0.01f;

	// Token: 0x04002AC8 RID: 10952
	private bool isFlashing;

	// Token: 0x04002AC9 RID: 10953
	private float _flashAmount;

	// Token: 0x04002ACA RID: 10954
	private Tweener tween;
}
