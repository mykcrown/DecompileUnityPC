using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000971 RID: 2417
public class InputBindingButton : MonoBehaviour
{
	// Token: 0x17000F4B RID: 3915
	// (get) Token: 0x060040DF RID: 16607 RVA: 0x00124682 File Offset: 0x00122A82
	// (set) Token: 0x060040E0 RID: 16608 RVA: 0x0012468A File Offset: 0x00122A8A
	public ButtonPress ButtonPress { get; set; }

	// Token: 0x17000F4C RID: 3916
	// (get) Token: 0x060040E1 RID: 16609 RVA: 0x00124693 File Offset: 0x00122A93
	// (set) Token: 0x060040E2 RID: 16610 RVA: 0x0012469B File Offset: 0x00122A9B
	public int BindingNum { get; set; }

	// Token: 0x060040E3 RID: 16611 RVA: 0x001246A4 File Offset: 0x00122AA4
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
		WavedashUIButton interactableButton = this.Button.InteractableButton;
		interactableButton.OnSelectEvent = (Action<BaseEventData>)Delegate.Combine(interactableButton.OnSelectEvent, new Action<BaseEventData>(this.onButtonSelected));
		WavedashUIButton interactableButton2 = this.Button.InteractableButton;
		interactableButton2.OnDeselectEvent = (Action<BaseEventData>)Delegate.Combine(interactableButton2.OnDeselectEvent, new Action<BaseEventData>(this.onButtonDeselected));
	}

	// Token: 0x060040E4 RID: 16612 RVA: 0x00124790 File Offset: 0x00122B90
	private void OnDestroy()
	{
		if (this.BackgroundImage.material != null && this.FlashMaterial != null)
		{
			UnityEngine.Object.DestroyImmediate(this.BackgroundImage.material);
		}
		WavedashUIButton interactableButton = this.Button.InteractableButton;
		interactableButton.OnSelectEvent = (Action<BaseEventData>)Delegate.Remove(interactableButton.OnSelectEvent, new Action<BaseEventData>(this.onButtonSelected));
		WavedashUIButton interactableButton2 = this.Button.InteractableButton;
		interactableButton2.OnDeselectEvent = (Action<BaseEventData>)Delegate.Remove(interactableButton2.OnDeselectEvent, new Action<BaseEventData>(this.onButtonDeselected));
	}

	// Token: 0x060040E5 RID: 16613 RVA: 0x0012482C File Offset: 0x00122C2C
	public void SetFlashing(bool isFlashing)
	{
		this.isFlashing = isFlashing;
		if (isFlashing)
		{
			if (this._flashTween == null)
			{
				this.flashAmount = 0.5f;
				this._flashTween = DOTween.To(new DOGetter<float>(this.get_flashAmount), delegate(float x)
				{
					this.flashAmount = x;
				}, 0f, 0.5f).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo);
			}
		}
		else
		{
			TweenUtil.Destroy(ref this._flashTween);
			this.flashAmount = 0f;
		}
		this.updateOverlayImage();
	}

	// Token: 0x060040E6 RID: 16614 RVA: 0x001248B7 File Offset: 0x00122CB7
	private void onButtonSelected(BaseEventData obj)
	{
		this.isSelected = true;
		this.updateOverlayImage();
	}

	// Token: 0x060040E7 RID: 16615 RVA: 0x001248C6 File Offset: 0x00122CC6
	private void onButtonDeselected(BaseEventData obj)
	{
		this.isSelected = false;
		this.updateOverlayImage();
	}

	// Token: 0x060040E8 RID: 16616 RVA: 0x001248D8 File Offset: 0x00122CD8
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

	// Token: 0x17000F4D RID: 3917
	// (get) Token: 0x060040E9 RID: 16617 RVA: 0x0012496B File Offset: 0x00122D6B
	// (set) Token: 0x060040EA RID: 16618 RVA: 0x00124974 File Offset: 0x00122D74
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

	// Token: 0x04002BB8 RID: 11192
	public MenuItemButton Button;

	// Token: 0x04002BB9 RID: 11193
	public TextMeshProUGUI BindingText;

	// Token: 0x04002BBA RID: 11194
	public Image BackgroundImage;

	// Token: 0x04002BBB RID: 11195
	public Image SelectedOverlay;

	// Token: 0x04002BBC RID: 11196
	public Image HighlightOverlay;

	// Token: 0x04002BBF RID: 11199
	public Material FlashMaterial;

	// Token: 0x04002BC0 RID: 11200
	private bool isFlashing;

	// Token: 0x04002BC1 RID: 11201
	private bool isSelected;

	// Token: 0x04002BC2 RID: 11202
	private Tweener _flashTween;

	// Token: 0x04002BC3 RID: 11203
	private float _flashAmount;
}
