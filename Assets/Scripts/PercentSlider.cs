// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PercentSlider : ClientBehavior
{
	private sealed class _Initialize_c__AnonStorey0
	{
		internal Action<float> setter;

		internal MenuItemList buttonList;

		internal PercentSlider _this;

		internal void __m__0(InputEventData eventData)
		{
			if (eventData.isMouseEvent)
			{
				this.setter(this._this.GetValue(eventData.mousePosition));
			}
			if (this._this.onSubmit != null)
			{
				this._this.onSubmit(eventData);
			}
		}

		internal void __m__1(MenuItemButton button, InputEventData eventData)
		{
			if (this.buttonList.CurrentSelection != button)
			{
				this.buttonList.AutoSelect(button);
			}
			this.buttonList.Lock();
			this._this.IsDragging = true;
		}

		internal void __m__2(MenuItemButton button, InputEventData eventData)
		{
			this.setter(this._this.GetValue(eventData.mousePosition));
		}

		internal void __m__3(MenuItemButton button, InputEventData eventData)
		{
			this.buttonList.Unlock();
			this._this.IsDragging = false;
		}

		internal void __m__4(MenuItemButton button, BaseEventData eventData)
		{
			if (button == this._this.Button)
			{
				this._this.setSelectDelayDuration(false);
			}
		}

		internal void __m__5(MenuItemButton button, BaseEventData eventData)
		{
			if (button == this._this.Button)
			{
				this._this.setSelectDelayDuration(true);
			}
		}
	}

	public TextMeshProUGUI Title;

	public TextMeshProUGUI PercentText;

	public Transform SliderTick;

	public MenuItemButton Button;

	public Image BackgroundImage;

	public Action<InputEventData> onSubmit;

	public const float SLIDER_REPEAT_DELAY_DURATION = 0.01f;

	private bool isFlashing;

	private float _flashAmount;

	private Tweener tween;

	[Inject]
	public UIManager uiManager
	{
		get;
		set;
	}

	public Func<float> PercentGetter
	{
		get;
		private set;
	}

	public Action<float> PercentSettter
	{
		get;
		private set;
	}

	public bool IsDragging
	{
		get;
		private set;
	}

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

	public void Initialize(Func<float> getter, Action<float> setter, MenuItemList buttonList)
	{
		PercentSlider._Initialize_c__AnonStorey0 _Initialize_c__AnonStorey = new PercentSlider._Initialize_c__AnonStorey0();
		_Initialize_c__AnonStorey.setter = setter;
		_Initialize_c__AnonStorey.buttonList = buttonList;
		_Initialize_c__AnonStorey._this = this;
		this.PercentGetter = getter;
		this.PercentSettter = _Initialize_c__AnonStorey.setter;
		this.IsDragging = false;
		this.SetValue(getter());
		_Initialize_c__AnonStorey.buttonList.AddButton(this.Button, new Action<InputEventData>(_Initialize_c__AnonStorey.__m__0));
		MenuItemButton expr_64 = this.Button;
		expr_64.DragBegin = (Action<MenuItemButton, InputEventData>)Delegate.Combine(expr_64.DragBegin, new Action<MenuItemButton, InputEventData>(_Initialize_c__AnonStorey.__m__1));
		MenuItemButton expr_8B = this.Button;
		expr_8B.Drag = (Action<MenuItemButton, InputEventData>)Delegate.Combine(expr_8B.Drag, new Action<MenuItemButton, InputEventData>(_Initialize_c__AnonStorey.__m__2));
		MenuItemButton expr_B2 = this.Button;
		expr_B2.DragEnd = (Action<MenuItemButton, InputEventData>)Delegate.Combine(expr_B2.DragEnd, new Action<MenuItemButton, InputEventData>(_Initialize_c__AnonStorey.__m__3));
		MenuItemList expr_D9 = _Initialize_c__AnonStorey.buttonList;
		expr_D9.OnSelected = (Action<MenuItemButton, BaseEventData>)Delegate.Combine(expr_D9.OnSelected, new Action<MenuItemButton, BaseEventData>(_Initialize_c__AnonStorey.__m__4));
		MenuItemList expr_100 = _Initialize_c__AnonStorey.buttonList;
		expr_100.OnDeselected = (Action<MenuItemButton, BaseEventData>)Delegate.Combine(expr_100.OnDeselected, new Action<MenuItemButton, BaseEventData>(_Initialize_c__AnonStorey.__m__5));
		if (this.BackgroundImage != null)
		{
			this.BackgroundImage.material = UnityEngine.Object.Instantiate<Material>(this.BackgroundImage.material);
		}
	}

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

	public float GetValue(Vector2 position)
	{
		RectTransform rectTransform = (RectTransform)this.SliderTick.transform;
		RectTransform rectTransform2 = (RectTransform)this.SliderTick.transform.parent;
		float num = rectTransform2.rect.width * rectTransform2.lossyScale.x;
		float num2 = position.x - rectTransform2.position.x + num * 0.5f;
		return Mathf.Clamp01(num2 / num);
	}

	private void killTween()
	{
		TweenUtil.Destroy(ref this.tween);
	}

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
					this.tween = DOTween.To(new DOGetter<float>(this.get_flashAmount), new DOSetter<float>(this._SetFlashing_m__0), 0f, 0.5f).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo);
				}
			}
			else
			{
				TweenUtil.Destroy(ref this.tween);
				this.flashAmount = 0f;
			}
		}
	}

	private void setSelectDelayDuration(bool reset)
	{
		UIInputModule uIInputModule = this.uiManager.CurrentInputModule as UIInputModule;
		if (reset)
		{
			uIInputModule.ResetHorizontalRepeatDelay();
		}
		else
		{
			uIInputModule.SetHorizontalRepeatDelay(0.01f);
		}
	}

	private void _SetFlashing_m__0(float x)
	{
		this.flashAmount = x;
	}
}
