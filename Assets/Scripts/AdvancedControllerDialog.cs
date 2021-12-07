// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AdvancedControllerDialog : BaseWindow
{
	private sealed class _addDropdown_c__AnonStorey0
	{
		internal VideoDropdownElement dropdown;

		internal Action<int> valueSelectedCallback;

		internal AdvancedControllerDialog _this;

		internal void __m__0()
		{
			this._this.columnsMenu.Lock();
			this.dropdown.Dropdown.Open();
			this._this.syncButtonNavigation();
		}

		internal void __m__1(int value)
		{
			this.dropdown.Dropdown.Close();
			this.valueSelectedCallback(value);
		}

		internal void __m__2()
		{
			this._this.columnsMenu.Unlock();
			this._this.columnsMenu.AutoSelect(this.dropdown.Dropdown.Button);
		}
	}

	public VerticalLayoutGroup Column1;

	public VerticalLayoutGroup Column2;

	public VideoDropdownElement DropdownPrefab;

	public Transform DropdownContainer;

	public PercentSlider PercentSliderPrefab;

	public SensitivityTestWidget sensitivityWidget;

	public Transform UnbindTextAnchor;

	public UnbindText UnbindTextPrefab;

	public Transform MovementUnboundAnchor;

	public GameObject MovementUnboundPrefab;

	public MenuItemButton ConfirmButton;

	public MenuItemButton ResetButton;

	private MenuItemList columnsMenu;

	private MenuItemList confirmMenu;

	private List<VideoDropdownElement> dropdowns = new List<VideoDropdownElement>();

	private VideoDropdownElement movementDropdown;

	private VideoDropdownElement emotesDropdown;

	private VideoDropdownElement tiltsDropdown;

	private VideoDropdownElement strikesDropdown;

	private VideoDropdownElement specialsDropdown;

	private Dictionary<MenuItemButton, PercentSlider> sliders = new Dictionary<MenuItemButton, PercentSlider>();

	private UnbindText unbindText;

	private GameObject movementUnboundText;

	private bool isSliderMode;

	private const float SLIDER_CONTROLLER_SPEED = 0.01f;

	[Inject]
	public IInputSettingsScreenAPI api
	{
		get;
		set;
	}

	[Inject]
	public IWindowDisplay windowDisplay
	{
		get;
		set;
	}

	[Inject]
	public DirectionalBindingHelper bindingHelper
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.columnsMenu = base.injector.GetInstance<MenuItemList>();
		this.confirmMenu = base.injector.GetInstance<MenuItemList>();
		this.movementDropdown = this.addDropdown(this.Column1.transform, base.localization.GetText("ui.dialog.advancedController.movement"), new Action<int>(this.setMovement));
		this.movementDropdown.Dropdown.Initialize(this.getDirectionalInputOptions(), this.DropdownContainer, null);
		this.emotesDropdown = this.addDropdown(this.Column1.transform, base.localization.GetText("ui.dialog.advancedController.emotes"), new Action<int>(this.setEmotes));
		this.emotesDropdown.Dropdown.Initialize(this.getDirectionalInputOptions(), this.DropdownContainer, null);
		this.tiltsDropdown = this.addDropdown(this.Column1.transform, base.localization.GetText("ui.dialog.advancedController.tilts"), new Action<int>(this.setTilts));
		this.tiltsDropdown.Dropdown.Initialize(this.getDirectionalInputOptions(), this.DropdownContainer, null);
		this.strikesDropdown = this.addDropdown(this.Column1.transform, base.localization.GetText("ui.dialog.advancedController.strikes"), new Action<int>(this.setStrikes));
		this.strikesDropdown.Dropdown.Initialize(this.getDirectionalInputOptions(), this.DropdownContainer, null);
		this.specialsDropdown = this.addDropdown(this.Column1.transform, base.localization.GetText("ui.dialog.advancedController.specials"), new Action<int>(this.setSpecials));
		this.specialsDropdown.Dropdown.Initialize(this.getDirectionalInputOptions(), this.DropdownContainer, null);
		this.createSlider(new Func<float>(this.getLeftStickSensitivity), new Action<float>(this.setLeftStickSensitivity), "ui.dialog.advancedController.leftStickDeadZone");
		this.createSlider(new Func<float>(this.getRightStickSensitivity), new Action<float>(this.setRightStickSensitivity), "ui.dialog.advancedController.rightStickDeadZone");
		this.createSlider(new Func<float>(this.getLeftTriggerSensitivity), new Action<float>(this.setLeftTriggerSensitivity), "ui.dialog.advancedController.leftTriggerDeadZone");
		this.createSlider(new Func<float>(this.getRightTriggerSensitivity), new Action<float>(this.setRightTriggerSensitivity), "ui.dialog.advancedController.rightTriggerDeadZone");
		this.confirmMenu.AddButton(this.ResetButton, new Action(this.onResetButton));
		this.confirmMenu.AddButton(this.ConfirmButton, new Action(this.onConfirmButton));
		int count = this.dropdowns.Count;
		this.columnsMenu.SetNavigationType(MenuItemList.NavigationType.GridVerticalFill, count);
		this.confirmMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.columnsMenu.LandingPoint = this.dropdowns[this.dropdowns.Count - 1].Dropdown.Button;
		this.columnsMenu.AddEdgeNavigation(MoveDirection.Down, this.confirmMenu);
		this.confirmMenu.LandingPoint = this.ConfirmButton;
		this.confirmMenu.AddEdgeNavigation(MoveDirection.Up, this.columnsMenu);
		this.columnsMenu.DisableGridWrap();
		this.columnsMenu.Initialize();
		this.confirmMenu.Initialize();
		this.sensitivityWidget.transform.SetAsLastSibling();
		this.unbindText = UnityEngine.Object.Instantiate<UnbindText>(this.UnbindTextPrefab, this.UnbindTextAnchor);
		this.movementUnboundText = UnityEngine.Object.Instantiate<GameObject>(this.MovementUnboundPrefab, this.MovementUnboundAnchor);
		this.FirstSelected = this.dropdowns[0].Dropdown.Button.InteractableButton.gameObject;
		base.listen(InputSettingsScreenAPI.UPDATED, new Action(this.onUpdate));
		base.listen(InputSettingsScreenAPI.UNBOUND, new Action(this.onUnbound));
	}

	private void Start()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.Column1.transform);
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.Column2.transform);
		this.onUpdate();
	}

	private void Update()
	{
		if ((base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode)
		{
			this.setSliderMode(false);
		}
	}

	private VideoDropdownElement addDropdown(Transform parent, string text, Action<int> valueSelectedCallback)
	{
		AdvancedControllerDialog._addDropdown_c__AnonStorey0 _addDropdown_c__AnonStorey = new AdvancedControllerDialog._addDropdown_c__AnonStorey0();
		_addDropdown_c__AnonStorey.valueSelectedCallback = valueSelectedCallback;
		_addDropdown_c__AnonStorey._this = this;
		_addDropdown_c__AnonStorey.dropdown = UnityEngine.Object.Instantiate<VideoDropdownElement>(this.DropdownPrefab, parent);
		base.injector.Inject(_addDropdown_c__AnonStorey.dropdown.Dropdown);
		_addDropdown_c__AnonStorey.dropdown.Title.text = text;
		this.columnsMenu.AddButton(_addDropdown_c__AnonStorey.dropdown.Dropdown.Button, new Action(_addDropdown_c__AnonStorey.__m__0));
		DropdownElement expr_7F = _addDropdown_c__AnonStorey.dropdown.Dropdown;
		expr_7F.ValueSelected = (Action<int>)Delegate.Combine(expr_7F.ValueSelected, new Action<int>(_addDropdown_c__AnonStorey.__m__1));
		DropdownElement expr_AB = _addDropdown_c__AnonStorey.dropdown.Dropdown;
		expr_AB.OnDropdownClosed = (Action)Delegate.Combine(expr_AB.OnDropdownClosed, new Action(_addDropdown_c__AnonStorey.__m__2));
		this.dropdowns.Add(_addDropdown_c__AnonStorey.dropdown);
		return _addDropdown_c__AnonStorey.dropdown;
	}

	private PercentSlider createSlider(Func<float> getter, Action<float> setter, string text)
	{
		PercentSlider percentSlider = UnityEngine.Object.Instantiate<PercentSlider>(this.PercentSliderPrefab, this.Column2.transform);
		percentSlider.Initialize(getter, setter, this.columnsMenu);
		percentSlider.Title.text = base.localization.GetText(text);
		PercentSlider expr_3D = percentSlider;
		expr_3D.onSubmit = (Action<InputEventData>)Delegate.Combine(expr_3D.onSubmit, new Action<InputEventData>(this._createSlider_m__0));
		this.sliders[percentSlider.Button] = percentSlider;
		return percentSlider;
	}

	private void onUpdate()
	{
		this.movementDropdown.Dropdown.SetValue(this.getCurrentDropdownValue(ButtonPress.Up));
		this.emotesDropdown.Dropdown.SetValue(this.getCurrentDropdownValue(ButtonPress.TauntUp));
		this.tiltsDropdown.Dropdown.SetValue(this.getCurrentDropdownValue(ButtonPress.UpTilt));
		this.strikesDropdown.Dropdown.SetValue(this.getCurrentDropdownValue(ButtonPress.UpStrike));
		this.specialsDropdown.Dropdown.SetValue(this.getCurrentDropdownValue(ButtonPress.UpSpecial));
		foreach (PercentSlider current in this.sliders.Values)
		{
			current.SetValue(current.PercentGetter());
			bool flashing = this.isSliderMode && this.columnsMenu.CurrentSelection == current.Button;
			current.SetFlashing(flashing);
		}
		this.movementUnboundText.gameObject.SetActive(this.api.IsMovementUnbound);
	}

	private int getCurrentDropdownValue(ButtonPress buttonPress)
	{
		BindingSource bindingSource = this.api.GetBindingSource(buttonPress, 0);
		DirectionalBindingHelper.DirectionalDevice deviceFromBinding = this.bindingHelper.GetDeviceFromBinding(bindingSource);
		return this.bindingHelper.LegalDirectionalInputs.IndexOf(deviceFromBinding);
	}

	private void syncButtonNavigation()
	{
		if (!(base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode)
		{
			foreach (VideoDropdownElement current in this.dropdowns)
			{
				if (current.Dropdown.IsOpen())
				{
					current.Dropdown.AutoSelectElement();
					return;
				}
			}
			if (this.columnsMenu.CurrentSelection == null)
			{
				this.columnsMenu.AutoSelect(this.columnsMenu.GetButtons()[0]);
			}
		}
	}

	private void onConfirmButton()
	{
		this.Close();
	}

	private void onResetButton()
	{
		GenericDialog genericDialog = base.dialogController.ShowTwoButtonDialog(base.localization.GetText("ui.settings.resetWarning.title"), base.localization.GetText("ui.settings.resetWarning.body"), base.localization.GetText("ui.settings.resetWarning.confirm"), base.localization.GetText("ui.settings.resetWarning.cancel"));
		GenericDialog expr_4D = genericDialog;
		expr_4D.ConfirmCallback = (Action)Delegate.Combine(expr_4D.ConfirmCallback, new Action(this._onResetButton_m__1));
	}

	public override void OnCancelPressed()
	{
		if (this.isSliderMode)
		{
			this.setSliderMode(false);
			return;
		}
		foreach (VideoDropdownElement current in this.dropdowns)
		{
			if (current.Dropdown.IsOpen())
			{
				current.Dropdown.Close();
				return;
			}
		}
		this.Close();
	}

	private void setMovement(int value)
	{
		this.addDirectionalBindings(value, ButtonPress.Up, ButtonPress.Down, ButtonPress.Backward, ButtonPress.Forward);
	}

	private void setEmotes(int value)
	{
		this.addDirectionalBindings(value, ButtonPress.TauntUp, ButtonPress.TauntDown, ButtonPress.TauntLeft, ButtonPress.TauntRight);
	}

	private void setTilts(int value)
	{
		this.addDirectionalBindings(value, ButtonPress.UpTilt, ButtonPress.DownTilt, ButtonPress.BackwardTilt, ButtonPress.ForwardTilt);
	}

	private void setStrikes(int value)
	{
		this.addDirectionalBindings(value, ButtonPress.UpStrike, ButtonPress.DownStrike, ButtonPress.BackwardStrike, ButtonPress.ForwardStrike);
	}

	private void setSpecials(int value)
	{
		this.addDirectionalBindings(value, ButtonPress.UpSpecial, ButtonPress.DownSpecial, ButtonPress.BackwardSpecial, ButtonPress.ForwardSpecial);
	}

	private void addDirectionalBindings(int value, ButtonPress upButton, ButtonPress downButton, ButtonPress leftButton, ButtonPress rightButton)
	{
		DirectionalBindingHelper.DirectionalDevice directionalInput = this.bindingHelper.LegalDirectionalInputs[value];
		this.api.SetBinding(upButton, this.bindingHelper.GetBindingFromDevice(directionalInput, AbsoluteDirection.Up), 0);
		this.api.SetBinding(downButton, this.bindingHelper.GetBindingFromDevice(directionalInput, AbsoluteDirection.Down), 0);
		this.api.SetBinding(leftButton, this.bindingHelper.GetBindingFromDevice(directionalInput, AbsoluteDirection.Left), 0);
		this.api.SetBinding(rightButton, this.bindingHelper.GetBindingFromDevice(directionalInput, AbsoluteDirection.Right), 0);
	}

	private string[] getDirectionalInputOptions()
	{
		List<DirectionalBindingHelper.DirectionalDevice> legalDirectionalInputs = this.bindingHelper.LegalDirectionalInputs;
		string[] array = new string[legalDirectionalInputs.Count];
		for (int i = 0; i < legalDirectionalInputs.Count; i++)
		{
			array[i] = ControlsSettingsTextHelper.GetDirectionalBindingText(base.localization, legalDirectionalInputs[i]);
		}
		return array;
	}

	private float getLeftStickSensitivity()
	{
		return this.DeadzoneToSensitivity(this.api.LeftStickDeadZone);
	}

	private float getRightStickSensitivity()
	{
		return this.DeadzoneToSensitivity(this.api.RightStickDeadZone);
	}

	private float getLeftTriggerSensitivity()
	{
		return this.DeadzoneToSensitivity(this.api.LeftTriggerDeadZone);
	}

	private float getRightTriggerSensitivity()
	{
		return this.DeadzoneToSensitivity(this.api.RightTriggerDeadZone);
	}

	private void setLeftStickSensitivity(float value)
	{
		this.api.LeftStickDeadZone = this.SensitivityToDeadzone(value);
	}

	private void setRightStickSensitivity(float value)
	{
		this.api.RightStickDeadZone = this.SensitivityToDeadzone(value);
	}

	private void setLeftTriggerSensitivity(float value)
	{
		this.api.LeftTriggerDeadZone = this.SensitivityToDeadzone(value);
	}

	private void setRightTriggerSensitivity(float value)
	{
		this.api.RightTriggerDeadZone = this.SensitivityToDeadzone(value);
	}

	private float DeadzoneToSensitivity(float deadzone)
	{
		return Mathf.Clamp(1f - deadzone, 0f, 1f);
	}

	private float SensitivityToDeadzone(float sensitivity)
	{
		return Mathf.Clamp(1f - sensitivity, 0f, 1f);
	}

	private void setSliderMode(bool activate)
	{
		bool flag = this.isSliderMode;
		this.isSliderMode = activate;
		if (activate)
		{
			this.columnsMenu.Lock();
		}
		else
		{
			this.columnsMenu.Unlock();
		}
		if (flag != this.isSliderMode)
		{
			this.onUpdate();
		}
	}

	public override void OnLeft()
	{
		if (!this.tickSlider(false))
		{
			base.OnLeft();
		}
	}

	public override void OnRight()
	{
		if (!this.tickSlider(true))
		{
			base.OnRight();
		}
	}

	private bool tickSlider(bool isPositive)
	{
		if (this.columnsMenu.CurrentSelection != null && this.sliders.ContainsKey(this.columnsMenu.CurrentSelection) && this.isSliderMode)
		{
			PercentSlider percentSlider = this.sliders[this.columnsMenu.CurrentSelection];
			if (percentSlider.Button == this.columnsMenu.CurrentSelection)
			{
				int num = (!isPositive) ? (-1) : 1;
				float obj = percentSlider.PercentGetter() + (float)num * 0.01f;
				percentSlider.PercentSettter(obj);
				return true;
			}
		}
		return false;
	}

	private void onUnbound()
	{
		DirectionalBindingHelper.DirectionalDevice deviceFromBinding = this.bindingHelper.GetDeviceFromBinding(this.api.LastUnboundBinding);
		string directionalUnboundBindingText = ControlsSettingsTextHelper.GetDirectionalUnboundBindingText(base.localization, this.api.LastUnboundButtonPress, deviceFromBinding);
		if (!string.IsNullOrEmpty(directionalUnboundBindingText))
		{
			this.unbindText.FlashText(directionalUnboundBindingText, 0f, 1f, 1f);
		}
	}

	private void _createSlider_m__0(InputEventData eventData)
	{
		if (!eventData.isMouseEvent && !this.isSliderMode)
		{
			this.setSliderMode(true);
		}
		else
		{
			this.setSliderMode(false);
		}
	}

	private void _onResetButton_m__1()
	{
		this.api.ResetControls();
		base.audioManager.PlayMenuSound(SoundKey.settings_settingsReset, 0f);
	}
}
