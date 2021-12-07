// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class UnboxingSceneItem : MonoBehaviour
{
	private sealed class _SetVisible_c__AnonStorey0
	{
		internal Transform transform;

		internal Vector3 __m__0()
		{
			return this.transform.localScale;
		}

		internal void __m__1(Vector3 valueIn)
		{
			this.transform.localScale = valueIn;
		}
	}

	public TextMeshProUGUI Title;

	public TextMeshProUGUI Category;

	public MenuItemButton PreviewButton;

	public MenuItemButton EquipButton;

	public InputInstructions InputInstructions;

	public GameObject BottomElement;

	public TMP_ColorGradient commonGradient;

	public TMP_ColorGradient uncommonGradient;

	public TMP_ColorGradient rareGradient;

	public TMP_ColorGradient legendaryGradient;

	public Vector3 highlightOffset = Vector3.zero;

	public float highlightScalar = 1f;

	public float highlightTime = 0.1f;

	private bool isMouseoverDirty;

	private bool previewAreaMouseOver;

	private bool equipButtonMouseOver;

	private Tweener nameScaleTween;

	private float categoryHighlightValue;

	private float categoryHighlightVelocity;

	private float categoryHighlightTarget;

	public Action<bool> MouseoverStateCallback
	{
		get;
		set;
	}

	public bool mouseoverState
	{
		get;
		private set;
	}

	public bool controllerSelectState
	{
		get;
		set;
	}

	public bool highlightState
	{
		get;
		set;
	}

	private void Awake()
	{
		if (this.PreviewButton != null)
		{
			this.BottomElement.SetActive(false);
			this.EquipButton.DisableType = ButtonAnimator.VisualDisableType.Grey;
			WavedashUIButton expr_34 = this.PreviewButton.InteractableButton;
			expr_34.OnPointerEnterEvent = (Action<InputEventData>)Delegate.Combine(expr_34.OnPointerEnterEvent, new Action<InputEventData>(this.onPreviewMouseover));
			WavedashUIButton expr_60 = this.PreviewButton.InteractableButton;
			expr_60.OnPointerExitEvent = (Action<InputEventData>)Delegate.Combine(expr_60.OnPointerExitEvent, new Action<InputEventData>(this.onPreviewMouseout));
			WavedashUIButton expr_8C = this.EquipButton.InteractableButton;
			expr_8C.OnPointerEnterEvent = (Action<InputEventData>)Delegate.Combine(expr_8C.OnPointerEnterEvent, new Action<InputEventData>(this.onEquipButtonMouseOver));
			WavedashUIButton expr_B8 = this.EquipButton.InteractableButton;
			expr_B8.OnPointerExitEvent = (Action<InputEventData>)Delegate.Combine(expr_B8.OnPointerExitEvent, new Action<InputEventData>(this.onEquipButtonMouseOut));
		}
	}

	private void OnDestroy()
	{
		if (this.PreviewButton != null)
		{
			WavedashUIButton expr_1C = this.PreviewButton.InteractableButton;
			expr_1C.OnPointerEnterEvent = (Action<InputEventData>)Delegate.Remove(expr_1C.OnPointerEnterEvent, new Action<InputEventData>(this.onPreviewMouseover));
			WavedashUIButton expr_48 = this.PreviewButton.InteractableButton;
			expr_48.OnPointerExitEvent = (Action<InputEventData>)Delegate.Remove(expr_48.OnPointerExitEvent, new Action<InputEventData>(this.onPreviewMouseout));
			WavedashUIButton expr_74 = this.EquipButton.InteractableButton;
			expr_74.OnPointerEnterEvent = (Action<InputEventData>)Delegate.Remove(expr_74.OnPointerEnterEvent, new Action<InputEventData>(this.onEquipButtonMouseOver));
			WavedashUIButton expr_A0 = this.EquipButton.InteractableButton;
			expr_A0.OnPointerExitEvent = (Action<InputEventData>)Delegate.Remove(expr_A0.OnPointerExitEvent, new Action<InputEventData>(this.onEquipButtonMouseOut));
		}
	}

	public void UpdateMouseMode(ControlMode mode)
	{
		this.InputInstructions.SetControlMode(mode);
	}

	public void SetEquipVisible(bool value)
	{
		this.InputInstructions.gameObject.SetActive(value);
		this.InputInstructions.KeyboardInstructions.gameObject.SetActive(value);
		this.InputInstructions.ControllerInstructions.gameObject.SetActive(value);
	}

	public void SetEquipEnabled(bool value)
	{
		this.InputInstructions.KeyboardInstructions.gameObject.SetActive(value);
		this.InputInstructions.ControllerInstructions.gameObject.SetActive(value);
	}

	private void onEquipButtonMouseOver(InputEventData eventData)
	{
		this.equipButtonMouseOver = true;
		this.isMouseoverDirty = true;
	}

	private void onEquipButtonMouseOut(InputEventData eventData)
	{
		this.equipButtonMouseOver = false;
		this.isMouseoverDirty = true;
	}

	private void onPreviewMouseover(InputEventData eventData)
	{
		this.previewAreaMouseOver = true;
		this.isMouseoverDirty = true;
	}

	private void onPreviewMouseout(InputEventData eventData)
	{
		this.previewAreaMouseOver = false;
		this.isMouseoverDirty = true;
	}

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

	private void setMouseoverState(bool value)
	{
		if (this.mouseoverState != value)
		{
			this.mouseoverState = value;
			this.MouseoverStateCallback(value);
		}
	}

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
			UnboxingSceneItem._SetVisible_c__AnonStorey0 _SetVisible_c__AnonStorey = new UnboxingSceneItem._SetVisible_c__AnonStorey0();
			this.categoryHighlightTarget = 1f;
			float duration = 0.2f;
			this.BottomElement.SetActive(true);
			_SetVisible_c__AnonStorey.transform = this.BottomElement.transform;
			_SetVisible_c__AnonStorey.transform.localScale = new Vector3(0.4f, 1f, 1f);
			this.nameScaleTween = DOTween.To(new DOGetter<Vector3>(_SetVisible_c__AnonStorey.__m__0), new DOSetter<Vector3>(_SetVisible_c__AnonStorey.__m__1), Vector3.one, duration).SetEase(Ease.OutBack).OnComplete(new TweenCallback(this.killScaleTween));
		}
	}

	private void killScaleTween()
	{
		TweenUtil.Destroy(ref this.nameScaleTween);
	}

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
		TMP_ColorGradient tMP_ColorGradient;
		switch (rarity)
		{
		case EquipmentRarity.COMMON:
			IL_65:
			tMP_ColorGradient = this.commonGradient;
			goto IL_95;
		case EquipmentRarity.UNCOMMON:
			tMP_ColorGradient = this.uncommonGradient;
			goto IL_95;
		case EquipmentRarity.RARE:
			tMP_ColorGradient = this.rareGradient;
			goto IL_95;
		case EquipmentRarity.LEGENDARY:
			tMP_ColorGradient = this.legendaryGradient;
			goto IL_95;
		}
		goto IL_65;
		IL_95:
		this.Category.colorGradientPreset = tMP_ColorGradient;
		this.Category.fontMaterial.SetColor("_OutlineColor", tMP_ColorGradient.topLeft);
	}

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
}
