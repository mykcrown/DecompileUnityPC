using System;
using System.Collections.Generic;
using InControl;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200096F RID: 2415
public class ControlsTab : SettingsTabElement
{
	// Token: 0x17000F47 RID: 3911
	// (get) Token: 0x060040BA RID: 16570 RVA: 0x00123AEA File Offset: 0x00121EEA
	// (set) Token: 0x060040BB RID: 16571 RVA: 0x00123AF2 File Offset: 0x00121EF2
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x17000F48 RID: 3912
	// (get) Token: 0x060040BC RID: 16572 RVA: 0x00123AFB File Offset: 0x00121EFB
	// (set) Token: 0x060040BD RID: 16573 RVA: 0x00123B03 File Offset: 0x00121F03
	[Inject]
	public IInputSettingsScreenAPI api { get; set; }

	// Token: 0x17000F49 RID: 3913
	// (get) Token: 0x060040BE RID: 16574 RVA: 0x00123B0C File Offset: 0x00121F0C
	// (set) Token: 0x060040BF RID: 16575 RVA: 0x00123B14 File Offset: 0x00121F14
	[Inject]
	public ISettingsScreenAPI settingsScreenAPI { get; set; }

	// Token: 0x17000F4A RID: 3914
	// (get) Token: 0x060040C0 RID: 16576 RVA: 0x00123B1D File Offset: 0x00121F1D
	// (set) Token: 0x060040C1 RID: 16577 RVA: 0x00123B25 File Offset: 0x00121F25
	[Inject]
	public IWindowDisplay windowDisplay { get; set; }

	// Token: 0x060040C2 RID: 16578 RVA: 0x00123B30 File Offset: 0x00121F30
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
		foreach (GameObject original in this.settingsContainer.settingsObjects)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			gameObject.transform.SetParent(this.settingsContainer.transform);
			ToggleUIGroup component = gameObject.GetComponent<ToggleUIGroup>();
			if (component != null)
			{
				foreach (OptionToggle optionToggle in component.toggles)
				{
					InputOptionToggleType type = this.settingsContainer.toggleOrder[num2];
					ControlsTab.TogglePair item = default(ControlsTab.TogglePair);
					item.toggle = optionToggle;
					item.type = type;
					switch (type)
					{
					case InputOptionToggleType.None:
						optionToggle.gameObject.SetActive(false);
						break;
					case InputOptionToggleType.TapJump:
						optionToggle.Title.text = base.localization.GetText("ui.settings.controls.tapJump");
						this.controlsItems.AddButton(optionToggle.Button, new Action(this.toggleTapJump));
						break;
					case InputOptionToggleType.TapStrike:
						optionToggle.Title.text = base.localization.GetText("ui.settings.controls.tapStrike");
						this.controlsItems.AddButton(optionToggle.Button, new Action(this.toggleTapStrike));
						break;
					case InputOptionToggleType.DoubleTapToRun:
						optionToggle.Title.text = base.localization.GetText("ui.settings.controls.doubleTapToRun");
						this.controlsItems.AddButton(optionToggle.Button, new Action(this.toggleDoubleTapToRun));
						break;
					case InputOptionToggleType.RecoveryJump:
						optionToggle.Title.text = base.localization.GetText("ui.settings.controls.recoveryJump");
						this.controlsItems.AddButton(optionToggle.Button, new Action(this.toggleRecoveryJump));
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
					InputBindingButton bindingButton = component2.InputBindingButton[i];
					if (buttonPress == ButtonPress.Shield1 && i > 0)
					{
						bindingButton.ButtonPress = ButtonPress.Shield2;
						bindingButton.BindingNum = i - 1;
					}
					else
					{
						bindingButton.ButtonPress = buttonPress;
						bindingButton.BindingNum = i;
					}
					this.controlsItems.AddButton(bindingButton.Button, delegate()
					{
						this.clickedBinding(bindingButton.ButtonPress, bindingButton.BindingNum);
					});
					this.controlsItems.AddRightClick(bindingButton.Button, delegate(InputEventData data)
					{
						this.rightClickedBinding(bindingButton.ButtonPress, bindingButton.BindingNum);
					});
					this.bindingButtons.Add(bindingButton);
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

	// Token: 0x060040C3 RID: 16579 RVA: 0x00124050 File Offset: 0x00122450
	public void Start()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.settingsContainer.transform);
		this.onUpdate();
	}

	// Token: 0x060040C4 RID: 16580 RVA: 0x0012406D File Offset: 0x0012246D
	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		this.syncButtonNavigation();
	}

	// Token: 0x060040C5 RID: 16581 RVA: 0x0012407B File Offset: 0x0012247B
	public override void OnDestroy()
	{
		this.api.OnExitScreen();
		base.OnDestroy();
	}

	// Token: 0x060040C6 RID: 16582 RVA: 0x00124090 File Offset: 0x00122490
	private void syncButtonNavigation()
	{
		if (!(base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode && this.windowDisplay.GetWindowCount() == 0 && this.isNullSelection() && base.allowInteraction())
		{
			this.advancedButtons.AutoSelect(this.AdvancedControlsButton);
		}
	}

	// Token: 0x060040C7 RID: 16583 RVA: 0x001240EE File Offset: 0x001224EE
	private bool isNullSelection()
	{
		return this.controlsItems.CurrentSelection == null && this.advancedButtons.CurrentSelection == null;
	}

	// Token: 0x060040C8 RID: 16584 RVA: 0x0012411A File Offset: 0x0012251A
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

	// Token: 0x060040C9 RID: 16585 RVA: 0x00124153 File Offset: 0x00122553
	private void toggleTapJump()
	{
		this.api.isTapJump = !this.api.isTapJump;
	}

	// Token: 0x060040CA RID: 16586 RVA: 0x0012416E File Offset: 0x0012256E
	private void toggleTapStrike()
	{
		this.api.isTapStrike = !this.api.isTapStrike;
	}

	// Token: 0x060040CB RID: 16587 RVA: 0x00124189 File Offset: 0x00122589
	private void toggleDoubleTapToRun()
	{
		this.api.isDoubleTapToRun = !this.api.isDoubleTapToRun;
	}

	// Token: 0x060040CC RID: 16588 RVA: 0x001241A4 File Offset: 0x001225A4
	private void toggleRecoveryJump()
	{
		this.api.isRecoveryJump = !this.api.isRecoveryJump;
	}

	// Token: 0x060040CD RID: 16589 RVA: 0x001241C0 File Offset: 0x001225C0
	private void clickedBinding(ButtonPress button, int buttonIndex)
	{
		this.api.ListenForBindingSource(button, buttonIndex);
		Debug.Log(string.Concat(new object[]
		{
			"Listen for ",
			button,
			" ",
			buttonIndex
		}));
		base.audioManager.PlayMenuSound(SoundKey.settings_controlBindingSelect, 0f);
	}

	// Token: 0x060040CE RID: 16590 RVA: 0x0012421E File Offset: 0x0012261E
	private void rightClickedBinding(ButtonPress button, int buttonIndex)
	{
		this.api.RemoveBinding(button, buttonIndex);
	}

	// Token: 0x060040CF RID: 16591 RVA: 0x0012422D File Offset: 0x0012262D
	public override void ResetClicked(InputEventData eventData)
	{
		base.ResetClicked(eventData);
		this.attemptReset();
	}

	// Token: 0x060040D0 RID: 16592 RVA: 0x0012423C File Offset: 0x0012263C
	public override void SaveClicked(InputEventData eventData)
	{
		base.SaveClicked(eventData);
		this.api.SaveControls();
	}

	// Token: 0x060040D1 RID: 16593 RVA: 0x00124250 File Offset: 0x00122650
	private void attemptReset()
	{
		GenericDialog genericDialog = base.dialogController.ShowTwoButtonDialog(base.localization.GetText("ui.settings.resetWarning.title"), base.localization.GetText("ui.settings.resetWarning.body"), base.localization.GetText("ui.settings.resetWarning.confirm"), base.localization.GetText("ui.settings.resetWarning.cancel"));
		GenericDialog genericDialog2 = genericDialog;
		genericDialog2.ConfirmCallback = (Action)Delegate.Combine(genericDialog2.ConfirmCallback, new Action(delegate()
		{
			this.api.ResetControls();
			base.audioManager.PlayMenuSound(SoundKey.settings_settingsReset, 0f);
		}));
	}

	// Token: 0x060040D2 RID: 16594 RVA: 0x001242CB File Offset: 0x001226CB
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.api.CancelListenForBinding();
		}
	}

	// Token: 0x060040D3 RID: 16595 RVA: 0x001242E4 File Offset: 0x001226E4
	private void onUpdate()
	{
		this.DeviceTitle.text = ControlsSettingsTextHelper.GetDeviceTitle(base.localization, this.settingsScreenAPI.InputPort.Device);
		foreach (ControlsTab.TogglePair togglePair in this.togglePairs)
		{
			switch (togglePair.type)
			{
			case InputOptionToggleType.TapJump:
				togglePair.toggle.SetToggle(this.api.isTapJump);
				break;
			case InputOptionToggleType.TapStrike:
				togglePair.toggle.SetToggle(this.api.isTapStrike);
				break;
			case InputOptionToggleType.DoubleTapToRun:
				togglePair.toggle.SetToggle(this.api.isDoubleTapToRun);
				break;
			case InputOptionToggleType.RecoveryJump:
				togglePair.toggle.SetToggle(this.api.isRecoveryJump);
				break;
			}
		}
		foreach (InputBindingButton inputBindingButton in this.bindingButtons)
		{
			bool flag = this.api.IsListeningForBinding && inputBindingButton.ButtonPress == this.api.ListeningButtonPress && inputBindingButton.BindingNum == this.api.ListeningButtonIndex;
			BindingSource bindingSource = this.api.GetBindingSource(inputBindingButton.ButtonPress, inputBindingButton.BindingNum);
			inputBindingButton.BindingText.text = ControlsSettingsTextHelper.GetBindingText(base.localization, bindingSource, flag);
			inputBindingButton.SetFlashing(flag);
		}
		this.movementUnboundText.gameObject.SetActive(this.api.IsMovementUnbound);
	}

	// Token: 0x060040D4 RID: 16596 RVA: 0x001244D4 File Offset: 0x001228D4
	private void onBound()
	{
		base.audioManager.PlayMenuSound(SoundKey.settings_controlBindingSet, 0f);
	}

	// Token: 0x060040D5 RID: 16597 RVA: 0x001244E8 File Offset: 0x001228E8
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

	// Token: 0x060040D6 RID: 16598 RVA: 0x00124550 File Offset: 0x00122950
	private void onReserved()
	{
		string text = base.localization.GetText("ui.settings.controls.reserved");
		this.unbindText.FlashText(text, 0f, 1f, 1f);
	}

	// Token: 0x060040D7 RID: 16599 RVA: 0x0012458C File Offset: 0x0012298C
	private void onSaved()
	{
		string text = base.localization.GetText("ui.settings.saved");
		this.SaveText.FlashText(text, 0.3f, 1f, 1f);
	}

	// Token: 0x060040D8 RID: 16600 RVA: 0x001245C5 File Offset: 0x001229C5
	public override bool OnRightTriggerPressed()
	{
		return this.api.IsListeningForBinding;
	}

	// Token: 0x060040D9 RID: 16601 RVA: 0x001245D2 File Offset: 0x001229D2
	public override bool OnLeftTriggerPressed()
	{
		return this.api.IsListeningForBinding;
	}

	// Token: 0x060040DA RID: 16602 RVA: 0x001245DF File Offset: 0x001229DF
	public override bool OnCancelPressed()
	{
		return this.api.IsListeningForBinding;
	}

	// Token: 0x060040DB RID: 16603 RVA: 0x001245EC File Offset: 0x001229EC
	public override void OnYButtonPressed()
	{
		base.OnYButtonPressed();
		this.attemptReset();
	}

	// Token: 0x060040DC RID: 16604 RVA: 0x001245FA File Offset: 0x001229FA
	public override void OnXButtonPressed()
	{
		base.OnXButtonPressed();
		this.api.SaveControls();
	}

	// Token: 0x04002BA9 RID: 11177
	public InputSettingsContainer settingsContainer;

	// Token: 0x04002BAA RID: 11178
	public TextMeshProUGUI DeviceTitle;

	// Token: 0x04002BAB RID: 11179
	public MenuItemButton AdvancedControlsButton;

	// Token: 0x04002BAC RID: 11180
	public Transform UnbindTextAnchor;

	// Token: 0x04002BAD RID: 11181
	public UnbindText UnbindTextPrefab;

	// Token: 0x04002BAE RID: 11182
	public Transform MovementUnboundAnchor;

	// Token: 0x04002BAF RID: 11183
	public GameObject MovementUnboundPrefab;

	// Token: 0x04002BB0 RID: 11184
	private MenuItemList advancedButtons;

	// Token: 0x04002BB1 RID: 11185
	private MenuItemList controlsItems;

	// Token: 0x04002BB2 RID: 11186
	private List<InputBindingButton> bindingButtons = new List<InputBindingButton>();

	// Token: 0x04002BB3 RID: 11187
	private List<ControlsTab.TogglePair> togglePairs = new List<ControlsTab.TogglePair>();

	// Token: 0x04002BB4 RID: 11188
	private UnbindText unbindText;

	// Token: 0x04002BB5 RID: 11189
	private GameObject movementUnboundText;

	// Token: 0x02000970 RID: 2416
	private struct TogglePair
	{
		// Token: 0x04002BB6 RID: 11190
		public OptionToggle toggle;

		// Token: 0x04002BB7 RID: 11191
		public InputOptionToggleType type;
	}
}
