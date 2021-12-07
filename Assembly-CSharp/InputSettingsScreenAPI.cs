using System;
using System.Collections.Generic;
using InControl;

// Token: 0x0200097E RID: 2430
public class InputSettingsScreenAPI : IInputSettingsScreenAPI
{
	// Token: 0x17000F60 RID: 3936
	// (get) Token: 0x06004171 RID: 16753 RVA: 0x0012643B File Offset: 0x0012483B
	// (set) Token: 0x06004172 RID: 16754 RVA: 0x00126443 File Offset: 0x00124843
	[Inject]
	public ISettingsScreenAPI settingsScreenAPI { get; set; }

	// Token: 0x17000F61 RID: 3937
	// (get) Token: 0x06004173 RID: 16755 RVA: 0x0012644C File Offset: 0x0012484C
	// (set) Token: 0x06004174 RID: 16756 RVA: 0x00126454 File Offset: 0x00124854
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000F62 RID: 3938
	// (get) Token: 0x06004175 RID: 16757 RVA: 0x0012645D File Offset: 0x0012485D
	// (set) Token: 0x06004176 RID: 16758 RVA: 0x00126465 File Offset: 0x00124865
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x17000F63 RID: 3939
	// (get) Token: 0x06004177 RID: 16759 RVA: 0x0012646E File Offset: 0x0012486E
	// (set) Token: 0x06004178 RID: 16760 RVA: 0x00126476 File Offset: 0x00124876
	[Inject]
	public IKeyBindingManager keybindingManager { get; set; }

	// Token: 0x17000F64 RID: 3940
	// (get) Token: 0x06004179 RID: 16761 RVA: 0x0012647F File Offset: 0x0012487F
	// (set) Token: 0x0600417A RID: 16762 RVA: 0x00126487 File Offset: 0x00124887
	[Inject]
	public IInputSettingsSaveManager settingsSaveManager { get; set; }

	// Token: 0x17000F65 RID: 3941
	// (get) Token: 0x0600417B RID: 16763 RVA: 0x00126490 File Offset: 0x00124890
	public bool IsListeningForBinding
	{
		get
		{
			return this.ListeningButtonPress != ButtonPress.None;
		}
	}

	// Token: 0x17000F66 RID: 3942
	// (get) Token: 0x0600417C RID: 16764 RVA: 0x0012649F File Offset: 0x0012489F
	public ButtonPress ListeningButtonPress
	{
		get
		{
			return this.listeningButtonPress;
		}
	}

	// Token: 0x17000F67 RID: 3943
	// (get) Token: 0x0600417D RID: 16765 RVA: 0x001264A7 File Offset: 0x001248A7
	// (set) Token: 0x0600417E RID: 16766 RVA: 0x001264AF File Offset: 0x001248AF
	public int ListeningButtonIndex { get; private set; }

	// Token: 0x17000F68 RID: 3944
	// (get) Token: 0x0600417F RID: 16767 RVA: 0x001264B8 File Offset: 0x001248B8
	// (set) Token: 0x06004180 RID: 16768 RVA: 0x001264C0 File Offset: 0x001248C0
	public ButtonPress LastUnboundButtonPress { get; set; }

	// Token: 0x17000F69 RID: 3945
	// (get) Token: 0x06004181 RID: 16769 RVA: 0x001264C9 File Offset: 0x001248C9
	// (set) Token: 0x06004182 RID: 16770 RVA: 0x001264D1 File Offset: 0x001248D1
	public BindingSource LastUnboundBinding { get; set; }

	// Token: 0x17000F6A RID: 3946
	// (get) Token: 0x06004183 RID: 16771 RVA: 0x001264DA File Offset: 0x001248DA
	private IInputDevice device
	{
		get
		{
			return this.userInputManager.GetDeviceWithPort(this.settingsScreenAPI.InputPort);
		}
	}

	// Token: 0x06004184 RID: 16772 RVA: 0x001264F4 File Offset: 0x001248F4
	public void Initialize()
	{
		this.setEditedSettings((InputSettingsData)this.settingsScreenAPI.InputPort.Settings.Clone());
		this.savedSettings = (InputSettingsData)this.settingsScreenAPI.InputPort.Settings.Clone();
		this.reservedControllerInput = new HashSet<InputControlType>
		{
			InputControlType.Start,
			InputControlType.Menu,
			InputControlType.Options,
			InputControlType.Back,
			InputControlType.LeftStickUp,
			InputControlType.LeftStickDown,
			InputControlType.LeftStickLeft,
			InputControlType.LeftStickRight,
			InputControlType.RightStickUp,
			InputControlType.RightStickDown,
			InputControlType.RightStickLeft,
			InputControlType.RightStickRight,
			InputControlType.DPadUp,
			InputControlType.DPadDown,
			InputControlType.DPadLeft,
			InputControlType.DPadRight
		};
		this.reservedKeyboardInput = new HashSet<Key>
		{
			Key.Escape
		};
		this.endKeybindMode();
		this.LastUnboundButtonPress = ButtonPress.None;
		this.LastUnboundBinding = null;
	}

	// Token: 0x06004185 RID: 16773 RVA: 0x00126604 File Offset: 0x00124A04
	private void setEditedSettings(InputSettingsData settingsData)
	{
		if (this.editedSettings != null)
		{
			BindingListenOptions listenOptions = this.editedSettings.inputActions.ListenOptions;
			listenOptions.OnBindingAdded = (Action<PlayerAction, BindingSource>)Delegate.Remove(listenOptions.OnBindingAdded, new Action<PlayerAction, BindingSource>(this.onBindingAdded));
			this.editedSettings.inputActions.Destroy();
		}
		this.editedSettings = settingsData;
		BindingListenOptions listenOptions2 = this.editedSettings.inputActions.ListenOptions;
		listenOptions2.OnBindingFound = (Func<PlayerAction, BindingSource, bool>)Delegate.Combine(listenOptions2.OnBindingFound, new Func<PlayerAction, BindingSource, bool>(this.onBindingFound));
		BindingListenOptions listenOptions3 = this.editedSettings.inputActions.ListenOptions;
		listenOptions3.OnBindingAdded = (Action<PlayerAction, BindingSource>)Delegate.Combine(listenOptions3.OnBindingAdded, new Action<PlayerAction, BindingSource>(this.onBindingAdded));
		this.editedSettings.inputActions.ListenOptions.AllowDuplicateBindingsPerSet = true;
		this.bindingIndicies.Clear();
		foreach (PlayerAction playerAction in this.editedSettings.inputActions.Actions)
		{
			if (!this.isIgnoredAction(playerAction))
			{
				for (int i = 0; i < playerAction.Bindings.Count; i++)
				{
					BindingSource key = playerAction.Bindings[i];
					this.bindingIndicies[key] = i;
				}
			}
		}
	}

	// Token: 0x06004186 RID: 16774 RVA: 0x00126780 File Offset: 0x00124B80
	public void SetBinding(ButtonPress buttonPress, BindingSource binding, int buttonIndex)
	{
		PlayerAction actionFromButtonPress = this.getActionFromButtonPress(this.editedSettings, buttonPress);
		if (binding == null)
		{
			this.removeBinding(actionFromButtonPress, buttonPress, buttonIndex);
		}
		else
		{
			this.addBinding(actionFromButtonPress, buttonPress, binding, buttonIndex);
		}
		this.dispatchUpdate();
	}

	// Token: 0x06004187 RID: 16775 RVA: 0x001267C8 File Offset: 0x00124BC8
	private void addBinding(PlayerAction action, ButtonPress buttonPress, BindingSource binding, int buttonIndex)
	{
		this.bindingIndicies[binding] = buttonIndex;
		foreach (PlayerAction playerAction in this.editedSettings.inputActions.Actions)
		{
			if (!this.isIgnoredAction(playerAction) && playerAction.HasBinding(binding) && playerAction != action)
			{
				if ((playerAction != this.editedSettings.inputActions.Shield1 || action != this.editedSettings.inputActions.Shield2) && (playerAction != this.editedSettings.inputActions.Shield2 || action != this.editedSettings.inputActions.Shield1))
				{
					this.LastUnboundButtonPress = this.editedSettings.inputActions.GetButtonPressFromAction(playerAction);
					this.LastUnboundBinding = binding;
					this.dispatchUnboundEvent();
				}
			}
		}
		foreach (PlayerAction playerAction2 in this.editedSettings.inputActions.Actions)
		{
			if (!this.isIgnoredAction(playerAction2))
			{
				playerAction2.HardRemoveBinding(binding);
			}
		}
		this.removeBinding(action, buttonPress, buttonIndex);
		action.AddBinding(binding);
		this.dispatchBoundEvent();
	}

	// Token: 0x06004188 RID: 16776 RVA: 0x00126948 File Offset: 0x00124D48
	public BindingSource GetBindingSource(ButtonPress buttonPress, int buttonIndex)
	{
		PlayerAction actionFromButtonPress = this.getActionFromButtonPress(this.editedSettings, buttonPress);
		if (actionFromButtonPress != null)
		{
			foreach (BindingSource bindingSource in actionFromButtonPress.Bindings)
			{
				int num = this.bindingIndicies[bindingSource];
				if (num == buttonIndex)
				{
					return bindingSource;
				}
			}
		}
		return null;
	}

	// Token: 0x06004189 RID: 16777 RVA: 0x001269D0 File Offset: 0x00124DD0
	public void CancelListenForBinding()
	{
		if (this.currentListenForBinding != null)
		{
			this.currentListenForBinding.StopListeningForBinding();
			this.endKeybindMode();
		}
	}

	// Token: 0x0600418A RID: 16778 RVA: 0x001269EE File Offset: 0x00124DEE
	public void ListenForBindingSource(ButtonPress buttonPress, int buttonIndex)
	{
		this.currentListenForBinding = this.getActionFromButtonPress(this.editedSettings, buttonPress);
		this.currentListenForBinding.ListenForBinding();
		this.listeningButtonPress = buttonPress;
		this.ListeningButtonIndex = buttonIndex;
		this.beginKeybindMode();
	}

	// Token: 0x0600418B RID: 16779 RVA: 0x00126A24 File Offset: 0x00124E24
	public void RemoveBinding(ButtonPress buttonPress, int buttonIndex)
	{
		PlayerAction actionFromButtonPress = this.getActionFromButtonPress(this.editedSettings, buttonPress);
		this.removeBinding(actionFromButtonPress, buttonPress, buttonIndex);
		this.endKeybindMode();
	}

	// Token: 0x0600418C RID: 16780 RVA: 0x00126A50 File Offset: 0x00124E50
	private bool onBindingFound(PlayerAction action, BindingSource binding)
	{
		if (binding is KeyBindingSource)
		{
			if (!(this.device is KeyboardInputDevice))
			{
				return false;
			}
			KeyBindingSource keyBindingSource = (KeyBindingSource)binding;
			if (this.reservedKeyboardInput.Contains(keyBindingSource.Control.GetInclude(0)))
			{
				this.signalBus.Dispatch(InputSettingsScreenAPI.RESERVED);
				return false;
			}
		}
		else if (binding is DeviceBindingSource)
		{
			if (!(this.device is InputDevice))
			{
				return false;
			}
			DeviceBindingSource deviceBindingSource = (DeviceBindingSource)binding;
			if (this.reservedControllerInput.Contains(deviceBindingSource.Control))
			{
				if (deviceBindingSource.Control == InputControlType.Start || deviceBindingSource.Control == InputControlType.Options)
				{
					this.CancelListenForBinding();
				}
				else
				{
					this.signalBus.Dispatch(InputSettingsScreenAPI.RESERVED);
				}
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600418D RID: 16781 RVA: 0x00126B29 File Offset: 0x00124F29
	private void onBindingAdded(PlayerAction action, BindingSource binding)
	{
		this.addBinding(action, this.listeningButtonPress, binding, this.ListeningButtonIndex);
		this.endKeybindMode();
	}

	// Token: 0x0600418E RID: 16782 RVA: 0x00126B48 File Offset: 0x00124F48
	private void removeBinding(PlayerAction action, ButtonPress buttonPress, int buttonIndex)
	{
		BindingSource bindingSource = this.GetBindingSource(buttonPress, buttonIndex);
		if (bindingSource != null)
		{
			action.HardRemoveBinding(bindingSource);
		}
	}

	// Token: 0x0600418F RID: 16783 RVA: 0x00126B74 File Offset: 0x00124F74
	public void SaveControls()
	{
		if (!this.IsMovementUnbound)
		{
			this.settingsScreenAPI.InputPort.LoadSettings((InputSettingsData)this.editedSettings.Clone());
			this.savedSettings = (InputSettingsData)this.editedSettings.Clone();
			this.settingsSaveManager.SaveInputSettings(this.editedSettings, this.device, this.settingsScreenAPI.InputPort.ShouldPersistSettings);
			this.dispatchUpdate();
			this.dispatchSaved();
		}
	}

	// Token: 0x06004190 RID: 16784 RVA: 0x00126BF5 File Offset: 0x00124FF5
	public void ResetControls()
	{
		this.setEditedSettings(this.settingsSaveManager.GetDefaultInputSettings(this.device, false));
		this.settingsScreenAPI.InputPort.LoadSettings((InputSettingsData)this.editedSettings.Clone());
		this.dispatchUpdate();
	}

	// Token: 0x17000F6B RID: 3947
	// (get) Token: 0x06004191 RID: 16785 RVA: 0x00126C35 File Offset: 0x00125035
	// (set) Token: 0x06004192 RID: 16786 RVA: 0x00126C42 File Offset: 0x00125042
	public bool isTapJump
	{
		get
		{
			return this.editedSettings.tapToJumpEnabled;
		}
		set
		{
			this.editedSettings.tapToJumpEnabled = value;
			this.dispatchUpdate();
		}
	}

	// Token: 0x17000F6C RID: 3948
	// (get) Token: 0x06004193 RID: 16787 RVA: 0x00126C56 File Offset: 0x00125056
	// (set) Token: 0x06004194 RID: 16788 RVA: 0x00126C63 File Offset: 0x00125063
	public bool isTapStrike
	{
		get
		{
			return this.editedSettings.tapToStrikeEnabled;
		}
		set
		{
			this.editedSettings.tapToStrikeEnabled = value;
			this.dispatchUpdate();
		}
	}

	// Token: 0x17000F6D RID: 3949
	// (get) Token: 0x06004195 RID: 16789 RVA: 0x00126C77 File Offset: 0x00125077
	// (set) Token: 0x06004196 RID: 16790 RVA: 0x00126C84 File Offset: 0x00125084
	public bool isRecoveryJump
	{
		get
		{
			return this.editedSettings.recoveryJumpingEnabled;
		}
		set
		{
			this.editedSettings.recoveryJumpingEnabled = value;
			this.dispatchUpdate();
		}
	}

	// Token: 0x17000F6E RID: 3950
	// (get) Token: 0x06004197 RID: 16791 RVA: 0x00126C98 File Offset: 0x00125098
	// (set) Token: 0x06004198 RID: 16792 RVA: 0x00126CA5 File Offset: 0x001250A5
	public bool isDoubleTapToRun
	{
		get
		{
			return this.editedSettings.doubleTapToRun;
		}
		set
		{
			this.editedSettings.doubleTapToRun = value;
			this.dispatchUpdate();
		}
	}

	// Token: 0x17000F6F RID: 3951
	// (get) Token: 0x06004199 RID: 16793 RVA: 0x00126CB9 File Offset: 0x001250B9
	// (set) Token: 0x0600419A RID: 16794 RVA: 0x00126CC6 File Offset: 0x001250C6
	public float LeftStickDeadZone
	{
		get
		{
			return this.editedSettings.leftStickDeadZone;
		}
		set
		{
			this.editedSettings.leftStickDeadZone = value;
			this.settingsScreenAPI.InputPort.LoadSettings((InputSettingsData)this.editedSettings.Clone());
			this.dispatchUpdate();
		}
	}

	// Token: 0x17000F70 RID: 3952
	// (get) Token: 0x0600419B RID: 16795 RVA: 0x00126CFA File Offset: 0x001250FA
	// (set) Token: 0x0600419C RID: 16796 RVA: 0x00126D07 File Offset: 0x00125107
	public float RightStickDeadZone
	{
		get
		{
			return this.editedSettings.rightStickDeadZone;
		}
		set
		{
			this.editedSettings.rightStickDeadZone = value;
			this.settingsScreenAPI.InputPort.LoadSettings((InputSettingsData)this.editedSettings.Clone());
			this.dispatchUpdate();
		}
	}

	// Token: 0x17000F71 RID: 3953
	// (get) Token: 0x0600419D RID: 16797 RVA: 0x00126D3B File Offset: 0x0012513B
	// (set) Token: 0x0600419E RID: 16798 RVA: 0x00126D48 File Offset: 0x00125148
	public float LeftTriggerDeadZone
	{
		get
		{
			return this.editedSettings.leftTriggerDeadZone;
		}
		set
		{
			this.editedSettings.leftTriggerDeadZone = value;
			this.settingsScreenAPI.InputPort.LoadSettings((InputSettingsData)this.editedSettings.Clone());
			this.dispatchUpdate();
		}
	}

	// Token: 0x17000F72 RID: 3954
	// (get) Token: 0x0600419F RID: 16799 RVA: 0x00126D7C File Offset: 0x0012517C
	// (set) Token: 0x060041A0 RID: 16800 RVA: 0x00126D89 File Offset: 0x00125189
	public float RightTriggerDeadZone
	{
		get
		{
			return this.editedSettings.rightTriggerDeadZone;
		}
		set
		{
			this.editedSettings.rightTriggerDeadZone = value;
			this.settingsScreenAPI.InputPort.LoadSettings((InputSettingsData)this.editedSettings.Clone());
			this.dispatchUpdate();
		}
	}

	// Token: 0x17000F73 RID: 3955
	// (get) Token: 0x060041A1 RID: 16801 RVA: 0x00126DBD File Offset: 0x001251BD
	public bool SettingsChanged
	{
		get
		{
			return this.editedSettings.Equals(this.savedSettings);
		}
	}

	// Token: 0x17000F74 RID: 3956
	// (get) Token: 0x060041A2 RID: 16802 RVA: 0x00126DD0 File Offset: 0x001251D0
	public bool IsMovementUnbound
	{
		get
		{
			return this.GetBindingSource(ButtonPress.Up, 0) == null || this.GetBindingSource(ButtonPress.Down, 0) == null || this.GetBindingSource(ButtonPress.Backward, 0) == null || this.GetBindingSource(ButtonPress.Forward, 0) == null;
		}
	}

	// Token: 0x060041A3 RID: 16803 RVA: 0x00126E27 File Offset: 0x00125227
	private void dispatchUpdate()
	{
		this.signalBus.Dispatch(InputSettingsScreenAPI.UPDATED);
	}

	// Token: 0x060041A4 RID: 16804 RVA: 0x00126E39 File Offset: 0x00125239
	private void dispatchBoundEvent()
	{
		this.signalBus.Dispatch(InputSettingsScreenAPI.BOUND);
	}

	// Token: 0x060041A5 RID: 16805 RVA: 0x00126E4B File Offset: 0x0012524B
	private void dispatchUnboundEvent()
	{
		this.signalBus.Dispatch(InputSettingsScreenAPI.UNBOUND);
	}

	// Token: 0x060041A6 RID: 16806 RVA: 0x00126E5D File Offset: 0x0012525D
	private void dispatchSaved()
	{
		this.signalBus.Dispatch(InputSettingsScreenAPI.SAVED);
	}

	// Token: 0x060041A7 RID: 16807 RVA: 0x00126E70 File Offset: 0x00125270
	private bool isIgnoredAction(PlayerAction action)
	{
		return action == this.editedSettings.inputActions.Submit || action == this.editedSettings.inputActions.Cancel || action == this.editedSettings.inputActions.Start;
	}

	// Token: 0x060041A8 RID: 16808 RVA: 0x00126EBF File Offset: 0x001252BF
	private PlayerAction getActionFromButtonPress(InputSettingsData settings, ButtonPress buttonPress)
	{
		if (PlayerInputActions.IsAxis(buttonPress))
		{
			return settings.inputActions.GetAxisAction(buttonPress);
		}
		return settings.inputActions.GetButtonAction(buttonPress);
	}

	// Token: 0x060041A9 RID: 16809 RVA: 0x00126EE5 File Offset: 0x001252E5
	private void beginKeybindMode()
	{
		this.keybindingManager.Begin();
		this.dispatchUpdate();
	}

	// Token: 0x060041AA RID: 16810 RVA: 0x00126EF8 File Offset: 0x001252F8
	private void endKeybindMode()
	{
		this.listeningButtonPress = ButtonPress.None;
		this.ListeningButtonIndex = -1;
		this.currentListenForBinding = null;
		this.keybindingManager.End();
		this.dispatchUpdate();
	}

	// Token: 0x060041AB RID: 16811 RVA: 0x00126F21 File Offset: 0x00125321
	public void OnExitScreen()
	{
		this.settingsScreenAPI.InputPort.LoadSettings((InputSettingsData)this.savedSettings.Clone());
		this.endKeybindMode();
	}

	// Token: 0x04002C1A RID: 11290
	public static string UPDATED = "InputSettingsScreenAPI.UPDATED";

	// Token: 0x04002C1B RID: 11291
	public static string BOUND = "InputSettingsScreenAPI.BOUND";

	// Token: 0x04002C1C RID: 11292
	public static string UNBOUND = "InputSettingsScreenAPI.UNBOUND";

	// Token: 0x04002C1D RID: 11293
	public static string RESERVED = "InputSettingsScreenAPI.RESERVED";

	// Token: 0x04002C1E RID: 11294
	public static string SAVED = "InputSettingsScreenAPI.SAVED";

	// Token: 0x04002C24 RID: 11300
	private ButtonPress listeningButtonPress;

	// Token: 0x04002C28 RID: 11304
	private PlayerAction currentListenForBinding;

	// Token: 0x04002C29 RID: 11305
	private InputSettingsData savedSettings;

	// Token: 0x04002C2A RID: 11306
	private InputSettingsData editedSettings;

	// Token: 0x04002C2B RID: 11307
	private Dictionary<BindingSource, int> bindingIndicies = new Dictionary<BindingSource, int>();

	// Token: 0x04002C2C RID: 11308
	private HashSet<InputControlType> reservedControllerInput;

	// Token: 0x04002C2D RID: 11309
	private HashSet<Key> reservedKeyboardInput;
}
