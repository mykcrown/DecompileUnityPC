// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using System.Collections.Generic;

public class InputSettingsScreenAPI : IInputSettingsScreenAPI
{
	public static string UPDATED = "InputSettingsScreenAPI.UPDATED";

	public static string BOUND = "InputSettingsScreenAPI.BOUND";

	public static string UNBOUND = "InputSettingsScreenAPI.UNBOUND";

	public static string RESERVED = "InputSettingsScreenAPI.RESERVED";

	public static string SAVED = "InputSettingsScreenAPI.SAVED";

	private ButtonPress listeningButtonPress;

	private PlayerAction currentListenForBinding;

	private InputSettingsData savedSettings;

	private InputSettingsData editedSettings;

	private Dictionary<BindingSource, int> bindingIndicies = new Dictionary<BindingSource, int>();

	private HashSet<InputControlType> reservedControllerInput;

	private HashSet<Key> reservedKeyboardInput;

	[Inject]
	public ISettingsScreenAPI settingsScreenAPI
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IUserInputManager userInputManager
	{
		get;
		set;
	}

	[Inject]
	public IKeyBindingManager keybindingManager
	{
		get;
		set;
	}

	[Inject]
	public IInputSettingsSaveManager settingsSaveManager
	{
		get;
		set;
	}

	public bool IsListeningForBinding
	{
		get
		{
			return this.ListeningButtonPress != ButtonPress.None;
		}
	}

	public ButtonPress ListeningButtonPress
	{
		get
		{
			return this.listeningButtonPress;
		}
	}

	public int ListeningButtonIndex
	{
		get;
		private set;
	}

	public ButtonPress LastUnboundButtonPress
	{
		get;
		set;
	}

	public BindingSource LastUnboundBinding
	{
		get;
		set;
	}

	private IInputDevice device
	{
		get
		{
			return this.userInputManager.GetDeviceWithPort(this.settingsScreenAPI.InputPort);
		}
	}

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

	public bool SettingsChanged
	{
		get
		{
			return this.editedSettings.Equals(this.savedSettings);
		}
	}

	public bool IsMovementUnbound
	{
		get
		{
			return this.GetBindingSource(ButtonPress.Up, 0) == null || this.GetBindingSource(ButtonPress.Down, 0) == null || this.GetBindingSource(ButtonPress.Backward, 0) == null || this.GetBindingSource(ButtonPress.Forward, 0) == null;
		}
	}

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

	private void setEditedSettings(InputSettingsData settingsData)
	{
		if (this.editedSettings != null)
		{
			BindingListenOptions expr_1B = this.editedSettings.inputActions.ListenOptions;
			expr_1B.OnBindingAdded = (Action<PlayerAction, BindingSource>)Delegate.Remove(expr_1B.OnBindingAdded, new Action<PlayerAction, BindingSource>(this.onBindingAdded));
			this.editedSettings.inputActions.Destroy();
		}
		this.editedSettings = settingsData;
		BindingListenOptions expr_63 = this.editedSettings.inputActions.ListenOptions;
		expr_63.OnBindingFound = (Func<PlayerAction, BindingSource, bool>)Delegate.Combine(expr_63.OnBindingFound, new Func<PlayerAction, BindingSource, bool>(this.onBindingFound));
		BindingListenOptions expr_94 = this.editedSettings.inputActions.ListenOptions;
		expr_94.OnBindingAdded = (Action<PlayerAction, BindingSource>)Delegate.Combine(expr_94.OnBindingAdded, new Action<PlayerAction, BindingSource>(this.onBindingAdded));
		this.editedSettings.inputActions.ListenOptions.AllowDuplicateBindingsPerSet = true;
		this.bindingIndicies.Clear();
		foreach (PlayerAction current in this.editedSettings.inputActions.Actions)
		{
			if (!this.isIgnoredAction(current))
			{
				for (int i = 0; i < current.Bindings.Count; i++)
				{
					BindingSource key = current.Bindings[i];
					this.bindingIndicies[key] = i;
				}
			}
		}
	}

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

	private void addBinding(PlayerAction action, ButtonPress buttonPress, BindingSource binding, int buttonIndex)
	{
		this.bindingIndicies[binding] = buttonIndex;
		foreach (PlayerAction current in this.editedSettings.inputActions.Actions)
		{
			if (!this.isIgnoredAction(current) && current.HasBinding(binding) && current != action)
			{
				if ((current != this.editedSettings.inputActions.Shield1 || action != this.editedSettings.inputActions.Shield2) && (current != this.editedSettings.inputActions.Shield2 || action != this.editedSettings.inputActions.Shield1))
				{
					this.LastUnboundButtonPress = this.editedSettings.inputActions.GetButtonPressFromAction(current);
					this.LastUnboundBinding = binding;
					this.dispatchUnboundEvent();
				}
			}
		}
		foreach (PlayerAction current2 in this.editedSettings.inputActions.Actions)
		{
			if (!this.isIgnoredAction(current2))
			{
				current2.HardRemoveBinding(binding);
			}
		}
		this.removeBinding(action, buttonPress, buttonIndex);
		action.AddBinding(binding);
		this.dispatchBoundEvent();
	}

	public BindingSource GetBindingSource(ButtonPress buttonPress, int buttonIndex)
	{
		PlayerAction actionFromButtonPress = this.getActionFromButtonPress(this.editedSettings, buttonPress);
		if (actionFromButtonPress != null)
		{
			foreach (BindingSource current in actionFromButtonPress.Bindings)
			{
				int num = this.bindingIndicies[current];
				if (num == buttonIndex)
				{
					return current;
				}
			}
		}
		return null;
	}

	public void CancelListenForBinding()
	{
		if (this.currentListenForBinding != null)
		{
			this.currentListenForBinding.StopListeningForBinding();
			this.endKeybindMode();
		}
	}

	public void ListenForBindingSource(ButtonPress buttonPress, int buttonIndex)
	{
		this.currentListenForBinding = this.getActionFromButtonPress(this.editedSettings, buttonPress);
		this.currentListenForBinding.ListenForBinding();
		this.listeningButtonPress = buttonPress;
		this.ListeningButtonIndex = buttonIndex;
		this.beginKeybindMode();
	}

	public void RemoveBinding(ButtonPress buttonPress, int buttonIndex)
	{
		PlayerAction actionFromButtonPress = this.getActionFromButtonPress(this.editedSettings, buttonPress);
		this.removeBinding(actionFromButtonPress, buttonPress, buttonIndex);
		this.endKeybindMode();
	}

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

	private void onBindingAdded(PlayerAction action, BindingSource binding)
	{
		this.addBinding(action, this.listeningButtonPress, binding, this.ListeningButtonIndex);
		this.endKeybindMode();
	}

	private void removeBinding(PlayerAction action, ButtonPress buttonPress, int buttonIndex)
	{
		BindingSource bindingSource = this.GetBindingSource(buttonPress, buttonIndex);
		if (bindingSource != null)
		{
			action.HardRemoveBinding(bindingSource);
		}
	}

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

	public void ResetControls()
	{
		this.setEditedSettings(this.settingsSaveManager.GetDefaultInputSettings(this.device, false));
		this.settingsScreenAPI.InputPort.LoadSettings((InputSettingsData)this.editedSettings.Clone());
		this.dispatchUpdate();
	}

	private void dispatchUpdate()
	{
		this.signalBus.Dispatch(InputSettingsScreenAPI.UPDATED);
	}

	private void dispatchBoundEvent()
	{
		this.signalBus.Dispatch(InputSettingsScreenAPI.BOUND);
	}

	private void dispatchUnboundEvent()
	{
		this.signalBus.Dispatch(InputSettingsScreenAPI.UNBOUND);
	}

	private void dispatchSaved()
	{
		this.signalBus.Dispatch(InputSettingsScreenAPI.SAVED);
	}

	private bool isIgnoredAction(PlayerAction action)
	{
		return action == this.editedSettings.inputActions.Submit || action == this.editedSettings.inputActions.Cancel || action == this.editedSettings.inputActions.Start;
	}

	private PlayerAction getActionFromButtonPress(InputSettingsData settings, ButtonPress buttonPress)
	{
		if (PlayerInputActions.IsAxis(buttonPress))
		{
			return settings.inputActions.GetAxisAction(buttonPress);
		}
		return settings.inputActions.GetButtonAction(buttonPress);
	}

	private void beginKeybindMode()
	{
		this.keybindingManager.Begin();
		this.dispatchUpdate();
	}

	private void endKeybindMode()
	{
		this.listeningButtonPress = ButtonPress.None;
		this.ListeningButtonIndex = -1;
		this.currentListenForBinding = null;
		this.keybindingManager.End();
		this.dispatchUpdate();
	}

	public void OnExitScreen()
	{
		this.settingsScreenAPI.InputPort.LoadSettings((InputSettingsData)this.savedSettings.Clone());
		this.endKeybindMode();
	}
}
