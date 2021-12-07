using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

// Token: 0x02000A13 RID: 2579
public class UnboxingSceneItem : MonoBehaviour
{
	// Token: 0x170011D6 RID: 4566
	// (get) Token: 0x06004AE2 RID: 19170 RVA: 0x001404BE File Offset: 0x0013E8BE
	// (set) Token: 0x06004AE3 RID: 19171 RVA: 0x001404C6 File Offset: 0x0013E8C6
	public Action<bool> MouseoverStateCallback { get; set; }

	// Token: 0x170011D7 RID: 4567
	// (get) Token: 0x06004AE4 RID: 19172 RVA: 0x001404CF File Offset: 0x0013E8CF
	// (set) Token: 0x06004AE5 RID: 19173 RVA: 0x001404D7 File Offset: 0x0013E8D7
	public bool mouseoverState { get; private set; }

	// Token: 0x170011D8 RID: 4568
	// (get) Token: 0x06004AE6 RID: 19174 RVA: 0x001404E0 File Offset: 0x0013E8E0
	// (set) Token: 0x06004AE7 RID: 19175 RVA: 0x001404E8 File Offset: 0x0013E8E8
	public bool controllerSelectState { get; set; }

	// Token: 0x170011D9 RID: 4569
	// (get) Token: 0x06004AE8 RID: 19176 RVA: 0x001404F1 File Offset: 0x0013E8F1
	// (set) Token: 0x06004AE9 RID: 19177 RVA: 0x001404F9 File Offset: 0x0013E8F9
	public bool highlightState { get; set; }

	// Token: 0x06004AEA RID: 19178 RVA: 0x00140504 File Offset: 0x0013E904
	private void Awake()
	{
		if (this.PreviewButton != null)
		{
			this.BottomElement.SetActive(false);
			this.EquipButton.DisableType = ButtonAnimator.VisualDisableType.Grey;
			WavedashUIButton interactableButton = this.PreviewButton.InteractableButton;
			interactableButton.OnPointerEnterEvent = (Action<InputEventData>)Delegate.Combine(interactableButton.OnPointerEnterEvent, new Action<InputEventData>(this.onPreviewMouseover));
			WavedashUIButton interactableButton2 = this.PreviewButton.InteractableButton;
			interactableButton2.OnPointerExitEvent = (Action<InputEventData>)Delegate.Combine(interactableButton2.OnPointerExitEvent, new Action<InputEventData>(this.onPreviewMouseout));
			WavedashUIButton interactableButton3 = this.EquipButton.InteractableButton;
			interactableButton3.OnPointerEnterEvent = (Action<InputEventData>)Delegate.Combine(interactableButton3.OnPointerEnterEvent, new Action<InputEventData>(this.onEquipButtonMouseOver));
			WavedashUIButton interactableButton4 = this.EquipButton.InteractableButton;
			interactableButton4.OnPointerExitEvent = (Action<InputEventData>)Delegate.Combine(interactableButton4.OnPointerExitEvent, new Action<InputEventData>(this.onEquipButtonMouseOut));
		}
	}

	// Token: 0x06004AEB RID: 19179 RVA: 0x001405EC File Offset: 0x0013E9EC
	private void OnDestroy()
	{
		if (this.PreviewButton != null)
		{
			WavedashUIButton interactableButton = this.PreviewButton.InteractableButton;
			interactableButton.OnPointerEnterEvent = (Action<InputEventData>)Delegate.Remove(interactableButton.OnPointerEnterEvent, new Action<InputEventData>(this.onPreviewMouseover));
			WavedashUIButton interactableButton2 = this.PreviewButton.InteractableButton;
			interactableButton2.OnPointerExitEvent = (Action<InputEventData>)Delegate.Remove(interactableButton2.OnPointerExitEvent, new Action<InputEventData>(this.onPreviewMouseout));
			WavedashUIButton interactableButton3 = this.EquipButton.InteractableButton;
			interactableButton3.OnPointerEnterEvent = (Action<InputEventData>)Delegate.Remove(interactableButton3.OnPointerEnterEvent, new Action<InputEventData>(this.onEquipButtonMouseOver));
			WavedashUIButton interactableButton4 = this.EquipButton.InteractableButton;
			interactableButton4.OnPointerExitEvent = (Action<InputEventData>)Delegate.Remove(interactableButton4.OnPointerExitEvent, new Action<InputEventData>(this.onEquipButtonMouseOut));
		}
	}

	// Token: 0x06004AEC RID: 19180 RVA: 0x001406BA File Offset: 0x0013EABA
	public void UpdateMouseMode(ControlMode mode)
	{
		this.InputInstructions.SetControlMode(mode);
	}

	// Token: 0x06004AED RID: 19181 RVA: 0x001406C8 File Offset: 0x0013EAC8
	public void SetEquipVisible(bool value)
	{
		this.InputInstructions.gameObject.SetActive(value);
		this.InputInstructions.KeyboardInstructions.gameObject.SetActive(value);
		this.InputInstructions.ControllerInstructions.gameObject.SetActive(value);
	}

	// Token: 0x06004AEE RID: 19182 RVA: 0x00140707 File Offset: 0x0013EB07
	public void SetEquipEnabled(bool value)
	{
		this.InputInstructions.KeyboardInstructions.gameObject.SetActive(value);
		this.InputInstructions.ControllerInstructions.gameObject.SetActive(value);
	}

	// Token: 0x06004AEF RID: 19183 RVA: 0x00140735 File Offset: 0x0013EB35
	private void onEquipButtonMouseOver(InputEventData eventData)
	{
		this.equipButtonMouseOver = true;
		this.isMouseoverDirty = true;
	}

	// Token: 0x06004AF0 RID: 19184 RVA: 0x00140745 File Offset: 0x0013EB45
	private void onEquipButtonMouseOut(InputEventData eventData)
	{
		this.equipButtonMouseOver = false;
		this.isMouseoverDirty = true;
	}

	// Token: 0x06004AF1 RID: 19185 RVA: 0x00140755 File Offset: 0x0013EB55
	private void onPreviewMouseover(InputEventData eventData)
	{
		this.previewAreaMouseOver = true;
		this.isMouseoverDirty = true;
	}

	// Token: 0x06004AF2 RID: 19186 RVA: 0x00140765 File Offset: 0x0013EB65
	private void onPreviewMouseout(InputEventData eventData)
	{
		this.previewAreaMouseOver = false;
		this.isMouseoverDirty = true;
	}

	// Token: 0x06004AF3 RID: 19187 RVA: 0x00140775 File Offset: 0x0013EB75
	private void onMouseoverStateUpdate()
	{
		if (this.previewAreaMouseOver || this.equipButtonMouseOver)
		{
			this.setMouseoverState(true);
		}
		else
		{
			this.setMouseoverState(false);
		}
	}

	// Token: 0x06004AF4 RID: 19188 RVA: 0x001407A0 File Offset: 0x0013EBA0
	private void setMouseoverState(bool value)
	{
		if (this.mouseoverState != value)
		{
			this.mouseoverState = value;
			this.MouseoverStateCallback(value);
		}
	}

	// Token: 0x06004AF5 RID: 19189 RVA: 0x001407C4 File Offset: 0x0013EBC4
	public void SetVisible(bool value)
	{
		this.killScaleTween();
		if (!value)
		{
			this.BottomElement.SetActive(false);
			this.categoryHighlightTarget = 0f;
		}
		else
		{
			this.categoryHighlightTarget = 1f;
			float duration = 0.2f;
			this.BottomElement.SetActive(true);
			Transform transform = this.BottomElement.transform;
			transform.localScale = new Vector3(0.4f, 1f, 1f);
			this.nameScaleTween = DOTween.To(() => transform.localScale, delegate(Vector3 valueIn)
			{
				transform.localScale = valueIn;
			}, Vector3.one, duration).SetEase(Ease.OutBack).OnComplete(new TweenCallback(this.killScaleTween));
		}
	}

	// Token: 0x06004AF6 RID: 19190 RVA: 0x0014088D File Offset: 0x0013EC8D
	private void killScaleTween()
	{
		TweenUtil.Destroy(ref this.nameScaleTween);
	}

	// Token: 0x06004AF7 RID: 19191 RVA: 0x0014089C File Offset: 0x0013EC9C
	public void UpdateText(string typeText, string title, EquipmentRarity rarity)
	{
		if (string.IsNullOrEmpty(title))
		{
			this.Title.gameObject.SetActive(false);
		}
		else
		{
			this.Title.gameObject.SetActive(true);
			this.Title.text = title;
		}
		this.Category.text = typeText;
		TMP_ColorGradient tmp_ColorGradient;
		switch (rarity)
		{
		default:
			tmp_ColorGradient = this.commonGradient;
			break;
		case EquipmentRarity.UNCOMMON:
			tmp_ColorGradient = this.uncommonGradient;
			break;
		case EquipmentRarity.RARE:
			tmp_ColorGradient = this.rareGradient;
			break;
		case EquipmentRarity.LEGENDARY:
			tmp_ColorGradient = this.legendaryGradient;
			break;
		}
		this.Category.colorGradientPreset = tmp_ColorGradient;
		this.Category.fontMaterial.SetColor("_OutlineColor", tmp_ColorGradient.topLeft);
	}

	// Token: 0x06004AF8 RID: 19192 RVA: 0x00140968 File Offset: 0x0013ED68
	private void LateUpdate()
	{
		if (this.isMouseoverDirty)
		{
			this.onMouseoverStateUpdate();
		}
		this.categoryHighlightValue = Mathf.SmoothDamp(this.categoryHighlightValue, this.categoryHighlightTarget, ref this.categoryHighlightVelocity, this.highlightTime);
		this.Category.transform.localPosition = Vector3.Lerp(Vector3.zero, this.highlightOffset, this.categoryHighlightValue);
		this.Category.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * this.highlightScalar, this.categoryHighlightValue);
	}

	// Token: 0x0400313A RID: 12602
	public TextMeshProUGUI Title;

	// Token: 0x0400313B RID: 12603
	public TextMeshProUGUI Category;

	// Token: 0x0400313C RID: 12604
	public MenuItemButton PreviewButton;

	// Token: 0x0400313D RID: 12605
	public MenuItemButton EquipButton;

	// Token: 0x0400313E RID: 12606
	public InputInstructions InputInstructions;

	// Token: 0x0400313F RID: 12607
	public GameObject BottomElement;

	// Token: 0x04003140 RID: 12608
	public TMP_ColorGradient commonGradient;

	// Token: 0x04003141 RID: 12609
	public TMP_ColorGradient uncommonGradient;

	// Token: 0x04003142 RID: 12610
	public TMP_ColorGradient rareGradient;

	// Token: 0x04003143 RID: 12611
	public TMP_ColorGradient legendaryGradient;

	// Token: 0x04003144 RID: 12612
	public Vector3 highlightOffset = Vector3.zero;

	// Token: 0x04003145 RID: 12613
	public float highlightScalar = 1f;

	// Token: 0x04003146 RID: 12614
	public float highlightTime = 0.1f;

	// Token: 0x0400314B RID: 12619
	private bool isMouseoverDirty;

	// Token: 0x0400314C RID: 12620
	private bool previewAreaMouseOver;

	// Token: 0x0400314D RID: 12621
	private bool equipButtonMouseOver;

	// Token: 0x0400314E RID: 12622
	private Tweener nameScaleTween;

	// Token: 0x0400314F RID: 12623
	private float categoryHighlightValue;

	// Token: 0x04003150 RID: 12624
	private float categoryHighlightVelocity;

	// Token: 0x04003151 RID: 12625
	private float categoryHighlightTarget;
}
