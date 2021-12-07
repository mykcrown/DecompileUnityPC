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

public class InputBindingButton : MonoBehaviour
{
	public MenuItemButton Button;

	public TextMeshProUGUI BindingText;

	public Image BackgroundImage;

	public Image SelectedOverlay;

	public Image HighlightOverlay;

	public Material FlashMaterial;

	private bool isFlashing;

	private bool isSelected;

	private Tweener _flashTween;

	private float _flashAmount;

	public ButtonPress ButtonPress
	{
		get;
		set;
	}

	public int BindingNum
	{
		get;
		set;
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
			if (this.SelectedOverlay != null)
			{
				this.SelectedOverlay.material.SetFloat("_FlashAmount", this._flashAmount);
			}
			if (this.HighlightOverlay != null)
			{
				this.HighlightOverlay.material.SetFloat("_FlashAmount", this._flashAmount);
			}
		}
	}

	private void Awake()
	{
		if (this.FlashMaterial != null)
		{
			if (this.BackgroundImage != null)
			{
				this.BackgroundImage.material = UnityEngine.Object.Instantiate<Material>(this.FlashMaterial);
			}
			if (this.SelectedOverlay != null)
			{
				this.SelectedOverlay.material = UnityEngine.Object.Instantiate<Material>(this.FlashMaterial);
			}
			if (this.HighlightOverlay != null)
			{
				this.HighlightOverlay.material = UnityEngine.Object.Instantiate<Material>(this.FlashMaterial);
			}
		}
		WavedashUIButton expr_91 = this.Button.InteractableButton;
		expr_91.OnSelectEvent = (Action<BaseEventData>)Delegate.Combine(expr_91.OnSelectEvent, new Action<BaseEventData>(this.onButtonSelected));
		WavedashUIButton expr_BD = this.Button.InteractableButton;
		expr_BD.OnDeselectEvent = (Action<BaseEventData>)Delegate.Combine(expr_BD.OnDeselectEvent, new Action<BaseEventData>(this.onButtonDeselected));
	}

	private void OnDestroy()
	{
		if (this.BackgroundImage.material != null && this.FlashMaterial != null)
		{
			UnityEngine.Object.DestroyImmediate(this.BackgroundImage.material);
		}
		WavedashUIButton expr_42 = this.Button.InteractableButton;
		expr_42.OnSelectEvent = (Action<BaseEventData>)Delegate.Remove(expr_42.OnSelectEvent, new Action<BaseEventData>(this.onButtonSelected));
		WavedashUIButton expr_6E = this.Button.InteractableButton;
		expr_6E.OnDeselectEvent = (Action<BaseEventData>)Delegate.Remove(expr_6E.OnDeselectEvent, new Action<BaseEventData>(this.onButtonDeselected));
	}

	public void SetFlashing(bool isFlashing)
	{
		this.isFlashing = isFlashing;
		if (isFlashing)
		{
			if (this._flashTween == null)
			{
				this.flashAmount = 0.5f;
				this._flashTween = DOTween.To(new DOGetter<float>(this.get_flashAmount), new DOSetter<float>(this._SetFlashing_m__0), 0f, 0.5f).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo);
			}
		}
		else
		{
			TweenUtil.Destroy(ref this._flashTween);
			this.flashAmount = 0f;
		}
		this.updateOverlayImage();
	}

	private void onButtonSelected(BaseEventData obj)
	{
		this.isSelected = true;
		this.updateOverlayImage();
	}

	private void onButtonDeselected(BaseEventData obj)
	{
		this.isSelected = false;
		this.updateOverlayImage();
	}

	public void updateOverlayImage()
	{
		if (this.isFlashing)
		{
			this.HighlightOverlay.gameObject.SetActive(true);
			this.SelectedOverlay.gameObject.SetActive(false);
		}
		else if (this.isSelected)
		{
			this.HighlightOverlay.gameObject.SetActive(false);
			this.SelectedOverlay.gameObject.SetActive(true);
		}
		else
		{
			this.HighlightOverlay.gameObject.SetActive(false);
			this.SelectedOverlay.gameObject.SetActive(false);
		}
	}

	private void _SetFlashing_m__0(float x)
	{
		this.flashAmount = x;
	}
}
