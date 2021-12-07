// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlsTab : SettingsTabElement
{
	private struct TogglePair
	{
		public OptionToggle toggle;

		public InputOptionToggleType type;
	}

	private sealed class _Awake_c__AnonStorey0
	{
		internal InputBindingButton bindingButton;

		internal ControlsTab _this;

		internal void __m__0()
		{
			this._this.clickedBinding(this.bindingButton.ButtonPress, this.bindingButton.BindingNum);
		}

		internal void __m__1(InputEventData data)
		{
			this._this.rightClickedBinding(this.bindingButton.ButtonPress, this.bindingButton.BindingNum);
		}
	}

	public InputSettingsContainer settingsContainer;

	public TextMeshProUGUI DeviceTitle;

	public MenuItemButton AdvancedControlsButton;

	public Transform UnbindTextAnchor;

	public UnbindText UnbindTextPrefab;

	public Transform MovementUnboundAnchor;

	public GameObject MovementUnboundPrefab;

	private MenuItemList advancedButtons;

	private MenuItemList controlsItems;

	private List<InputBindingButton> bindingButtons = new List<InputBindingButton>();

	private List<ControlsTab.TogglePair> togglePairs = new List<ControlsTab.TogglePair>();

	private UnbindText unbindText;

	private GameObject movementUnboundText;

	[Inject]
	public IUserInputManager userInputManager
	{
		get;
		set;
	}

	[Inject]
	public IInputSettingsScreenAPI api
	{
		get;
		set;
	}

	[Inject]
	public ISettingsScreenAPI settingsScreenAPI
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

	public override void Awake()
	{
		base.Awake();
		if (this.api == null)
		{
			return;
		}
		this.api.Initialize();
		this.advancedButtons = base.injector.GetInstance<MenuItemList>();
		this.controlsItems = base.injector.GetInstance<MenuItemList>();
		this.advancedButtons.AddButton(this.AdvancedControlsButton, new Action(this.showAdvancedDialog));
		int num = 0;
		int num2 = 0;
		foreach (GameObject current in this.settingsContainer.settingsObjects)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(current);
			gameObject.transform.SetParent(this.settingsContainer.transform);
			ToggleUIGroup component = gameObject.GetComponent<ToggleUIGroup>();
			if (component != null)
			{
				foreach (OptionToggle current2 in component.toggles)
				{
					InputOptionToggleType type = this.settingsContainer.toggleOrder[num2];
					ControlsTab.TogglePair item = default(ControlsTab.TogglePair);
					item.toggle = current2;
					item.type = type;
					switch (type)
					{
					case InputOptionToggleType.None:
						current2.gameObject.SetActive(false);
						break;
					case InputOptionToggleType.TapJump:
						current2.Title.text = base.localization.GetText("ui.settings.controls.tapJump");
						this.controlsItems.AddButton(current2.Button, new Action(this.toggleTapJump));
						break;
					case InputOptionToggleType.TapStrike:
						current2.Title.text = base.localization.GetText("ui.settings.controls.tapStrike");
						this.controlsItems.AddButton(current2.Button, new Action(this.toggleTapStrike));
						break;
					case InputOptionToggleType.DoubleTapToRun:
						current2.Title.text = base.localization.GetText("ui.settings.controls.doubleTapToRun");
						this.controlsItems.AddButton(current2.Button, new Action(this.toggleDoubleTapToRun));
						break;
					case InputOptionToggleType.RecoveryJump:
						current2.Title.text = base.localization.GetText("ui.settings.controls.recoveryJump");
						this.controlsItems.AddButton(current2.Button, new Action(this.toggleRecoveryJump));
						break;
					}
					this.togglePairs.Add(item);
					num2++;
				}
			}
			BindingsUIGroup component2 = gameObject.GetComponent<BindingsUIGroup>();
			if (component2 != null)
			{
				ButtonPress buttonPress = this.settingsContainer.bindingOrder[num];
				component2.Title.text = ControlsSettingsTextHelper.GetLocalizedBindingTitle(base.localization, buttonPress);
				for (int i = 0; i < component2.InputBindingButton.Count; i++)
				{
					ControlsTab._Awake_c__AnonStorey0 _Awake_c__AnonStorey = new ControlsTab._Awake_c__AnonStorey0();
					_Awake_c__AnonStorey._this = this;
					_Awake_c__AnonStorey.bindingButton = component2.InputBindingButton[i];
					if (buttonPress == ButtonPress.Shield1 && i > 0)
					{
						_Awake_c__AnonStorey.bindingButton.ButtonPress = ButtonPress.Shield2;
						_Awake_c__AnonStorey.bindingButton.BindingNum = i - 1;
					}
					else
					{
						_Awake_c__AnonStorey.bindingButton.ButtonPress = buttonPress;
						_Awake_c__AnonStorey.bindingButton.BindingNum = i;
					}
					this.controlsItems.AddButton(_Awake_c__AnonStorey.bindingButton.Button, new Action(_Awake_c__AnonStorey.__m__0));
					this.controlsItems.AddRightClick(_Awake_c__AnonStorey.bindingButton.Button, new Action<InputEventData>(_Awake_c__AnonStorey.__m__1));
					this.bindingButtons.Add(_Awake_c__AnonStorey.bindingButton);
				}
				num++;
			}
		}
		this.advancedButtons.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.controlsItems.SetNavigationType(MenuItemList.NavigationType.GridHorizontalFill, 6);
		this.controlsItems.DisableGridWrap();
		this.advancedButtons.AddEdgeNavigation(MoveDirection.Down, this.controlsItems);
		this.advancedButtons.LandingPoint = this.advancedButtons.GetButtons()[0];
		this.controlsItems.AddEdgeNavigation(MoveDirection.Up, this.advancedButtons);
		this.controlsItems.LandingPoint = this.controlsItems.GetButtons()[0];
		this.advancedButtons.Initialize();
		this.controlsItems.Initialize();
		this.unbindText = UnityEngine.Object.Instantiate<UnbindText>(this.UnbindTextPrefab, this.UnbindTextAnchor);
		this.movementUnboundText = UnityEngine.Object.Instantiate<GameObject>(this.MovementUnboundPrefab, this.MovementUnboundAnchor);
		base.listen(InputSettingsScreenAPI.UPDATED, new Action(this.onUpdate));
		base.listen(InputSettingsScreenAPI.BOUND, new Action(this.onBound));
		base.listen(InputSettingsScreenAPI.UNBOUND, new Action(this.onUnbound));
		base.listen(InputSettingsScreenAPI.RESERVED, new Action(this.onReserved));
		base.listen(InputSettingsScreenAPI.SAVED, new Action(this.onSaved));
	}

	public void Start()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.settingsContainer.transform);
		this.onUpdate();
	}

	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		this.syncButtonNavigation();
	}

	public override void OnDestroy()
	{
		this.api.OnExitScreen();
		base.OnDestroy();
	}

	private void syncButtonNavigation()
	{
		if (!(base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode && this.windowDisplay.GetWindowCount() == 0 && this.isNullSelection() && base.allowInteraction())
		{
			this.advancedButtons.AutoSelect(this.AdvancedControlsButton);
		}
	}

	private bool isNullSelection()
	{
		return this.controlsItems.CurrentSelection == null && this.advancedButtons.CurrentSelection == null;
	}

	private void showAdvancedDialog()
	{
		if (this.settingsScreenAPI.InputPort.Device is IControllerInputDevice)
		{
			base.dialogController.ShowAdvancedControllerDialog();
		}
		else
		{
			base.dialogController.ShowAdvancedKeyboardDialog();
		}
	}

	private void toggleTapJump()
	{
		this.api.isTapJump = !this.api.isTapJump;
	}

	private void toggleTapStrike()
	{
		this.api.isTapStrike = !this.api.isTapStrike;
	}

	private void toggleDoubleTapToRun()
	{
		this.api.isDoubleTapToRun = !this.api.isDoubleTapToRun;
	}

	private void toggleRecoveryJump()
	{
		this.api.isRecoveryJump = !this.api.isRecoveryJump;
	}

	private void clickedBinding(ButtonPress button, int buttonIndex)
	{
		this.api.ListenForBindingSource(button, buttonIndex);
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Listen for ",
			button,
			" ",
			buttonIndex
		}));
		base.audioManager.PlayMenuSound(SoundKey.settings_controlBindingSelect, 0f);
	}

	private void rightClickedBinding(ButtonPress button, int buttonIndex)
	{
		this.api.RemoveBinding(button, buttonIndex);
	}

	public override void ResetClicked(InputEventData eventData)
	{
		base.ResetClicked(eventData);
		this.attemptReset();
	}

	public override void SaveClicked(InputEventData eventData)
	{
		base.SaveClicked(eventData);
		this.api.SaveControls();
	}

	private void attemptReset()
	{
		GenericDialog genericDialog = base.dialogController.ShowTwoButtonDialog(base.localization.GetText("ui.settings.resetWarning.title"), base.localization.GetText("ui.settings.resetWarning.body"), base.localization.GetText("ui.settings.resetWarning.confirm"), base.localization.GetText("ui.settings.resetWarning.cancel"));
		GenericDialog expr_4D = genericDialog;
		expr_4D.ConfirmCallback = (Action)Delegate.Combine(expr_4D.ConfirmCallback, new Action(this._attemptReset_m__0));
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.api.CancelListenForBinding();
		}
	}

	private void onUpdate()
	{
		this.DeviceTitle.text = ControlsSettingsTextHelper.GetDeviceTitle(base.localization, this.settingsScreenAPI.InputPort.Device);
		foreach (ControlsTab.TogglePair current in this.togglePairs)
		{
			switch (current.type)
			{
			case InputOptionToggleType.TapJump:
				current.toggle.SetToggle(this.api.isTapJump);
				break;
			case InputOptionToggleType.TapStrike:
				current.toggle.SetToggle(this.api.isTapStrike);
				break;
			case InputOptionToggleType.DoubleTapToRun:
				current.toggle.SetToggle(this.api.isDoubleTapToRun);
				break;
			case InputOptionToggleType.RecoveryJump:
				current.toggle.SetToggle(this.api.isRecoveryJump);
				break;
			}
		}
		foreach (InputBindingButton current2 in this.bindingButtons)
		{
			bool flag = this.api.IsListeningForBinding && current2.ButtonPress == this.api.ListeningButtonPress && current2.BindingNum == this.api.ListeningButtonIndex;
			BindingSource bindingSource = this.api.GetBindingSource(current2.ButtonPress, current2.BindingNum);
			current2.BindingText.text = ControlsSettingsTextHelper.GetBindingText(base.localization, bindingSource, flag);
			current2.SetFlashing(flag);
		}
		this.movementUnboundText.gameObject.SetActive(this.api.IsMovementUnbound);
	}

	private void onBound()
	{
		base.audioManager.PlayMenuSound(SoundKey.settings_controlBindingSet, 0f);
	}

	private void onUnbound()
	{
		if (this.windowDisplay.GetWindowCount() > 0)
		{
			return;
		}
		string unboundBindingText = ControlsSettingsTextHelper.GetUnboundBindingText(base.localization, this.api.LastUnboundButtonPress, this.api.LastUnboundBinding);
		if (!string.IsNullOrEmpty(unboundBindingText))
		{
			this.unbindText.FlashText(unboundBindingText, 0f, 1f, 1f);
		}
	}

	private void onReserved()
	{
		string text = base.localization.GetText("ui.settings.controls.reserved");
		this.unbindText.FlashText(text, 0f, 1f, 1f);
	}

	private void onSaved()
	{
		string text = base.localization.GetText("ui.settings.saved");
		this.SaveText.FlashText(text, 0.3f, 1f, 1f);
	}

	public override bool OnRightTriggerPressed()
	{
		return this.api.IsListeningForBinding;
	}

	public override bool OnLeftTriggerPressed()
	{
		return this.api.IsListeningForBinding;
	}

	public override bool OnCancelPressed()
	{
		return this.api.IsListeningForBinding;
	}

	public override void OnYButtonPressed()
	{
		base.OnYButtonPressed();
		this.attemptReset();
	}

	public override void OnXButtonPressed()
	{
		base.OnXButtonPressed();
		this.api.SaveControls();
	}

	private void _attemptReset_m__0()
	{
		this.api.ResetControls();
		base.audioManager.PlayMenuSound(SoundKey.settings_settingsReset, 0f);
	}
}
