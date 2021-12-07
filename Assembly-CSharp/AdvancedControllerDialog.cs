using System;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200096B RID: 2411
public class AdvancedControllerDialog : BaseWindow
{
	// Token: 0x17000F40 RID: 3904
	// (get) Token: 0x06004071 RID: 16497 RVA: 0x00122076 File Offset: 0x00120476
	// (set) Token: 0x06004072 RID: 16498 RVA: 0x0012207E File Offset: 0x0012047E
	[Inject]
	public IInputSettingsScreenAPI api { get; set; }

	// Token: 0x17000F41 RID: 3905
	// (get) Token: 0x06004073 RID: 16499 RVA: 0x00122087 File Offset: 0x00120487
	// (set) Token: 0x06004074 RID: 16500 RVA: 0x0012208F File Offset: 0x0012048F
	[Inject]
	public IWindowDisplay windowDisplay { get; set; }

	// Token: 0x17000F42 RID: 3906
	// (get) Token: 0x06004075 RID: 16501 RVA: 0x00122098 File Offset: 0x00120498
	// (set) Token: 0x06004076 RID: 16502 RVA: 0x001220A0 File Offset: 0x001204A0
	[Inject]
	public DirectionalBindingHelper bindingHelper { get; set; }

	// Token: 0x06004077 RID: 16503 RVA: 0x001220AC File Offset: 0x001204AC
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

	// Token: 0x06004078 RID: 16504 RVA: 0x00122470 File Offset: 0x00120870
	private void Start()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.Column1.transform);
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.Column2.transform);
		this.onUpdate();
	}

	// Token: 0x06004079 RID: 16505 RVA: 0x001224A2 File Offset: 0x001208A2
	private void Update()
	{
		if ((base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode)
		{
			this.setSliderMode(false);
		}
	}

	// Token: 0x0600407A RID: 16506 RVA: 0x001224C8 File Offset: 0x001208C8
	private VideoDropdownElement addDropdown(Transform parent, string text, Action<int> valueSelectedCallback)
	{
		VideoDropdownElement dropdown = UnityEngine.Object.Instantiate<VideoDropdownElement>(this.DropdownPrefab, parent);
		base.injector.Inject(dropdown.Dropdown);
		dropdown.Title.text = text;
		this.columnsMenu.AddButton(dropdown.Dropdown.Button, delegate()
		{
			this.columnsMenu.Lock();
			dropdown.Dropdown.Open();
			this.syncButtonNavigation();
		});
		DropdownElement dropdown3 = dropdown.Dropdown;
		dropdown3.ValueSelected = (Action<int>)Delegate.Combine(dropdown3.ValueSelected, new Action<int>(delegate(int value)
		{
			dropdown.Dropdown.Close();
			valueSelectedCallback(value);
		}));
		DropdownElement dropdown2 = dropdown.Dropdown;
		dropdown2.OnDropdownClosed = (Action)Delegate.Combine(dropdown2.OnDropdownClosed, new Action(delegate()
		{
			this.columnsMenu.Unlock();
			this.columnsMenu.AutoSelect(dropdown.Dropdown.Button);
		}));
		this.dropdowns.Add(dropdown);
		return dropdown;
	}

	// Token: 0x0600407B RID: 16507 RVA: 0x001225B8 File Offset: 0x001209B8
	private PercentSlider createSlider(Func<float> getter, Action<float> setter, string text)
	{
		PercentSlider percentSlider = UnityEngine.Object.Instantiate<PercentSlider>(this.PercentSliderPrefab, this.Column2.transform);
		percentSlider.Initialize(getter, setter, this.columnsMenu);
		percentSlider.Title.text = base.localization.GetText(text);
		PercentSlider percentSlider2 = percentSlider;
		percentSlider2.onSubmit = (Action<InputEventData>)Delegate.Combine(percentSlider2.onSubmit, new Action<InputEventData>(delegate(InputEventData eventData)
		{
			if (!eventData.isMouseEvent && !this.isSliderMode)
			{
				this.setSliderMode(true);
			}
			else
			{
				this.setSliderMode(false);
			}
		}));
		this.sliders[percentSlider.Button] = percentSlider;
		return percentSlider;
	}

	// Token: 0x0600407C RID: 16508 RVA: 0x00122638 File Offset: 0x00120A38
	private void onUpdate()
	{
		this.movementDropdown.Dropdown.SetValue(this.getCurrentDropdownValue(ButtonPress.Up));
		this.emotesDropdown.Dropdown.SetValue(this.getCurrentDropdownValue(ButtonPress.TauntUp));
		this.tiltsDropdown.Dropdown.SetValue(this.getCurrentDropdownValue(ButtonPress.UpTilt));
		this.strikesDropdown.Dropdown.SetValue(this.getCurrentDropdownValue(ButtonPress.UpStrike));
		this.specialsDropdown.Dropdown.SetValue(this.getCurrentDropdownValue(ButtonPress.UpSpecial));
		foreach (PercentSlider percentSlider in this.sliders.Values)
		{
			percentSlider.SetValue(percentSlider.PercentGetter());
			bool flashing = this.isSliderMode && this.columnsMenu.CurrentSelection == percentSlider.Button;
			percentSlider.SetFlashing(flashing);
		}
		this.movementUnboundText.gameObject.SetActive(this.api.IsMovementUnbound);
	}

	// Token: 0x0600407D RID: 16509 RVA: 0x00122764 File Offset: 0x00120B64
	private int getCurrentDropdownValue(ButtonPress buttonPress)
	{
		BindingSource bindingSource = this.api.GetBindingSource(buttonPress, 0);
		DirectionalBindingHelper.DirectionalDevice deviceFromBinding = this.bindingHelper.GetDeviceFromBinding(bindingSource);
		return this.bindingHelper.LegalDirectionalInputs.IndexOf(deviceFromBinding);
	}

	// Token: 0x0600407E RID: 16510 RVA: 0x001227A0 File Offset: 0x00120BA0
	private void syncButtonNavigation()
	{
		if (!(base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode)
		{
			foreach (VideoDropdownElement videoDropdownElement in this.dropdowns)
			{
				if (videoDropdownElement.Dropdown.IsOpen())
				{
					videoDropdownElement.Dropdown.AutoSelectElement();
					return;
				}
			}
			if (this.columnsMenu.CurrentSelection == null)
			{
				this.columnsMenu.AutoSelect(this.columnsMenu.GetButtons()[0]);
			}
		}
	}

	// Token: 0x0600407F RID: 16511 RVA: 0x00122860 File Offset: 0x00120C60
	private void onConfirmButton()
	{
		this.Close();
	}

	// Token: 0x06004080 RID: 16512 RVA: 0x00122868 File Offset: 0x00120C68
	private void onResetButton()
	{
		GenericDialog genericDialog = base.dialogController.ShowTwoButtonDialog(base.localization.GetText("ui.settings.resetWarning.title"), base.localization.GetText("ui.settings.resetWarning.body"), base.localization.GetText("ui.settings.resetWarning.confirm"), base.localization.GetText("ui.settings.resetWarning.cancel"));
		GenericDialog genericDialog2 = genericDialog;
		genericDialog2.ConfirmCallback = (Action)Delegate.Combine(genericDialog2.ConfirmCallback, new Action(delegate()
		{
			this.api.ResetControls();
			base.audioManager.PlayMenuSound(SoundKey.settings_settingsReset, 0f);
		}));
	}

	// Token: 0x06004081 RID: 16513 RVA: 0x001228E4 File Offset: 0x00120CE4
	public override void OnCancelPressed()
	{
		if (this.isSliderMode)
		{
			this.setSliderMode(false);
			return;
		}
		foreach (VideoDropdownElement videoDropdownElement in this.dropdowns)
		{
			if (videoDropdownElement.Dropdown.IsOpen())
			{
				videoDropdownElement.Dropdown.Close();
				return;
			}
		}
		this.Close();
	}

	// Token: 0x06004082 RID: 16514 RVA: 0x00122974 File Offset: 0x00120D74
	private void setMovement(int value)
	{
		this.addDirectionalBindings(value, ButtonPress.Up, ButtonPress.Down, ButtonPress.Backward, ButtonPress.Forward);
	}

	// Token: 0x06004083 RID: 16515 RVA: 0x00122981 File Offset: 0x00120D81
	private void setEmotes(int value)
	{
		this.addDirectionalBindings(value, ButtonPress.TauntUp, ButtonPress.TauntDown, ButtonPress.TauntLeft, ButtonPress.TauntRight);
	}

	// Token: 0x06004084 RID: 16516 RVA: 0x00122992 File Offset: 0x00120D92
	private void setTilts(int value)
	{
		this.addDirectionalBindings(value, ButtonPress.UpTilt, ButtonPress.DownTilt, ButtonPress.BackwardTilt, ButtonPress.ForwardTilt);
	}

	// Token: 0x06004085 RID: 16517 RVA: 0x001229A3 File Offset: 0x00120DA3
	private void setStrikes(int value)
	{
		this.addDirectionalBindings(value, ButtonPress.UpStrike, ButtonPress.DownStrike, ButtonPress.BackwardStrike, ButtonPress.ForwardStrike);
	}

	// Token: 0x06004086 RID: 16518 RVA: 0x001229B4 File Offset: 0x00120DB4
	private void setSpecials(int value)
	{
		this.addDirectionalBindings(value, ButtonPress.UpSpecial, ButtonPress.DownSpecial, ButtonPress.BackwardSpecial, ButtonPress.ForwardSpecial);
	}

	// Token: 0x06004087 RID: 16519 RVA: 0x001229C8 File Offset: 0x00120DC8
	private void addDirectionalBindings(int value, ButtonPress upButton, ButtonPress downButton, ButtonPress leftButton, ButtonPress rightButton)
	{
		DirectionalBindingHelper.DirectionalDevice directionalInput = this.bindingHelper.LegalDirectionalInputs[value];
		this.api.SetBinding(upButton, this.bindingHelper.GetBindingFromDevice(directionalInput, AbsoluteDirection.Up), 0);
		this.api.SetBinding(downButton, this.bindingHelper.GetBindingFromDevice(directionalInput, AbsoluteDirection.Down), 0);
		this.api.SetBinding(leftButton, this.bindingHelper.GetBindingFromDevice(directionalInput, AbsoluteDirection.Left), 0);
		this.api.SetBinding(rightButton, this.bindingHelper.GetBindingFromDevice(directionalInput, AbsoluteDirection.Right), 0);
	}

	// Token: 0x06004088 RID: 16520 RVA: 0x00122A54 File Offset: 0x00120E54
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

	// Token: 0x06004089 RID: 16521 RVA: 0x00122AA6 File Offset: 0x00120EA6
	private float getLeftStickSensitivity()
	{
		return this.DeadzoneToSensitivity(this.api.LeftStickDeadZone);
	}

	// Token: 0x0600408A RID: 16522 RVA: 0x00122AB9 File Offset: 0x00120EB9
	private float getRightStickSensitivity()
	{
		return this.DeadzoneToSensitivity(this.api.RightStickDeadZone);
	}

	// Token: 0x0600408B RID: 16523 RVA: 0x00122ACC File Offset: 0x00120ECC
	private float getLeftTriggerSensitivity()
	{
		return this.DeadzoneToSensitivity(this.api.LeftTriggerDeadZone);
	}

	// Token: 0x0600408C RID: 16524 RVA: 0x00122ADF File Offset: 0x00120EDF
	private float getRightTriggerSensitivity()
	{
		return this.DeadzoneToSensitivity(this.api.RightTriggerDeadZone);
	}

	// Token: 0x0600408D RID: 16525 RVA: 0x00122AF2 File Offset: 0x00120EF2
	private void setLeftStickSensitivity(float value)
	{
		this.api.LeftStickDeadZone = this.SensitivityToDeadzone(value);
	}

	// Token: 0x0600408E RID: 16526 RVA: 0x00122B06 File Offset: 0x00120F06
	private void setRightStickSensitivity(float value)
	{
		this.api.RightStickDeadZone = this.SensitivityToDeadzone(value);
	}

	// Token: 0x0600408F RID: 16527 RVA: 0x00122B1A File Offset: 0x00120F1A
	private void setLeftTriggerSensitivity(float value)
	{
		this.api.LeftTriggerDeadZone = this.SensitivityToDeadzone(value);
	}

	// Token: 0x06004090 RID: 16528 RVA: 0x00122B2E File Offset: 0x00120F2E
	private void setRightTriggerSensitivity(float value)
	{
		this.api.RightTriggerDeadZone = this.SensitivityToDeadzone(value);
	}

	// Token: 0x06004091 RID: 16529 RVA: 0x00122B42 File Offset: 0x00120F42
	private float DeadzoneToSensitivity(float deadzone)
	{
		return Mathf.Clamp(1f - deadzone, 0f, 1f);
	}

	// Token: 0x06004092 RID: 16530 RVA: 0x00122B5A File Offset: 0x00120F5A
	private float SensitivityToDeadzone(float sensitivity)
	{
		return Mathf.Clamp(1f - sensitivity, 0f, 1f);
	}

	// Token: 0x06004093 RID: 16531 RVA: 0x00122B74 File Offset: 0x00120F74
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

	// Token: 0x06004094 RID: 16532 RVA: 0x00122BC2 File Offset: 0x00120FC2
	public override void OnLeft()
	{
		if (!this.tickSlider(false))
		{
			base.OnLeft();
		}
	}

	// Token: 0x06004095 RID: 16533 RVA: 0x00122BD6 File Offset: 0x00120FD6
	public override void OnRight()
	{
		if (!this.tickSlider(true))
		{
			base.OnRight();
		}
	}

	// Token: 0x06004096 RID: 16534 RVA: 0x00122BEC File Offset: 0x00120FEC
	private bool tickSlider(bool isPositive)
	{
		if (this.columnsMenu.CurrentSelection != null && this.sliders.ContainsKey(this.columnsMenu.CurrentSelection) && this.isSliderMode)
		{
			PercentSlider percentSlider = this.sliders[this.columnsMenu.CurrentSelection];
			if (percentSlider.Button == this.columnsMenu.CurrentSelection)
			{
				int num = (!isPositive) ? -1 : 1;
				float obj = percentSlider.PercentGetter() + (float)num * 0.01f;
				percentSlider.PercentSettter(obj);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004097 RID: 16535 RVA: 0x00122C9C File Offset: 0x0012109C
	private void onUnbound()
	{
		DirectionalBindingHelper.DirectionalDevice deviceFromBinding = this.bindingHelper.GetDeviceFromBinding(this.api.LastUnboundBinding);
		string directionalUnboundBindingText = ControlsSettingsTextHelper.GetDirectionalUnboundBindingText(base.localization, this.api.LastUnboundButtonPress, deviceFromBinding);
		if (!string.IsNullOrEmpty(directionalUnboundBindingText))
		{
			this.unbindText.FlashText(directionalUnboundBindingText, 0f, 1f, 1f);
		}
	}

	// Token: 0x04002B6D RID: 11117
	public VerticalLayoutGroup Column1;

	// Token: 0x04002B6E RID: 11118
	public VerticalLayoutGroup Column2;

	// Token: 0x04002B6F RID: 11119
	public VideoDropdownElement DropdownPrefab;

	// Token: 0x04002B70 RID: 11120
	public Transform DropdownContainer;

	// Token: 0x04002B71 RID: 11121
	public PercentSlider PercentSliderPrefab;

	// Token: 0x04002B72 RID: 11122
	public SensitivityTestWidget sensitivityWidget;

	// Token: 0x04002B73 RID: 11123
	public Transform UnbindTextAnchor;

	// Token: 0x04002B74 RID: 11124
	public UnbindText UnbindTextPrefab;

	// Token: 0x04002B75 RID: 11125
	public Transform MovementUnboundAnchor;

	// Token: 0x04002B76 RID: 11126
	public GameObject MovementUnboundPrefab;

	// Token: 0x04002B77 RID: 11127
	public MenuItemButton ConfirmButton;

	// Token: 0x04002B78 RID: 11128
	public MenuItemButton ResetButton;

	// Token: 0x04002B79 RID: 11129
	private MenuItemList columnsMenu;

	// Token: 0x04002B7A RID: 11130
	private MenuItemList confirmMenu;

	// Token: 0x04002B7B RID: 11131
	private List<VideoDropdownElement> dropdowns = new List<VideoDropdownElement>();

	// Token: 0x04002B7C RID: 11132
	private VideoDropdownElement movementDropdown;

	// Token: 0x04002B7D RID: 11133
	private VideoDropdownElement emotesDropdown;

	// Token: 0x04002B7E RID: 11134
	private VideoDropdownElement tiltsDropdown;

	// Token: 0x04002B7F RID: 11135
	private VideoDropdownElement strikesDropdown;

	// Token: 0x04002B80 RID: 11136
	private VideoDropdownElement specialsDropdown;

	// Token: 0x04002B81 RID: 11137
	private Dictionary<MenuItemButton, PercentSlider> sliders = new Dictionary<MenuItemButton, PercentSlider>();

	// Token: 0x04002B82 RID: 11138
	private UnbindText unbindText;

	// Token: 0x04002B83 RID: 11139
	private GameObject movementUnboundText;

	// Token: 0x04002B84 RID: 11140
	private bool isSliderMode;

	// Token: 0x04002B85 RID: 11141
	private const float SLIDER_CONTROLLER_SPEED = 0.01f;
}
