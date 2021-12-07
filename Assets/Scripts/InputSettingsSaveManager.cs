// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using System.Collections.Generic;

public class InputSettingsSaveManager : IInputSettingsSaveManager
{
	private const string KEYBOARD_FILE = "settings/keyboard.settings";

	private const string GC_CONTROLLER_FILE = "settings/gcController.settings";

	private const string XBOX_CONTROLLER_FILE = "settings/xBoxController.settings";

	[Inject]
	public ISaveFileData saveFileData
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	public InputSettingsData LoadInputSettings(IInputDevice device, bool isP2Debug, bool shouldPersist)
	{
		InputSettingsData inputSettingsData = null;
		if (shouldPersist)
		{
			inputSettingsData = this.getSavedInputSettings(device);
		}
		if (inputSettingsData == null)
		{
			inputSettingsData = this.GetDefaultInputSettings(device, isP2Debug);
		}
		else
		{
			this.updateInputSettings(inputSettingsData, device, isP2Debug);
		}
		return inputSettingsData;
	}

	public void SaveInputSettings(InputSettingsData settingsData, IInputDevice device, bool shouldPersist)
	{
		if (shouldPersist)
		{
			UniqueDeviceType deviceType = InputUtils.GetDeviceType(device);
			if (deviceType != UniqueDeviceType.GC)
			{
				if (deviceType != UniqueDeviceType.Other)
				{
					if (deviceType == UniqueDeviceType.Keyboard)
					{
						this.saveFileData.SaveToFile<InputSettingsData>("settings/keyboard.settings", settingsData);
					}
				}
				else
				{
					this.saveFileData.SaveToFile<InputSettingsData>("settings/xBoxController.settings", settingsData);
				}
			}
			else
			{
				this.saveFileData.SaveToFile<InputSettingsData>("settings/gcController.settings", settingsData);
			}
		}
	}

	private void initControllerDeadZones(ref InputSettingsData settingsData, IInputDevice device)
	{
		IControllerInputDevice controllerInputDevice = device as IControllerInputDevice;
		if (controllerInputDevice != null)
		{
			settingsData.leftStickDeadZone = controllerInputDevice.DefaultLeftStickLowerDeadZone;
			settingsData.rightStickDeadZone = controllerInputDevice.DefaultRightStickLowerDeadZone;
			settingsData.leftTriggerDeadZone = controllerInputDevice.DefaultLeftTriggerDeadZone;
			settingsData.rightTriggerDeadZone = controllerInputDevice.DefaultRightTriggerDeadZone;
		}
	}

	public InputSettingsData GetDefaultInputSettings(IInputDevice device, bool isP2Debug)
	{
		InputSettingsData inputSettingsData = new InputSettingsData();
		device.MangleDefaultSettings(inputSettingsData);
		IEnumerable<DefaultInputBinding> defaultGCBindings = this.gameDataManager.ConfigData.inputConfig.defaultGCBindings;
		IEnumerable<DefaultInputBinding> defaultBindings = this.gameDataManager.ConfigData.inputConfig.defaultBindings;
		if (device != null)
		{
			if (isP2Debug)
			{
				InputSettingsSaveManager.addDebugKeyboardBindings(inputSettingsData.inputActions, defaultBindings);
			}
			else
			{
				UniqueDeviceType deviceType = InputUtils.GetDeviceType(device);
				if (deviceType != UniqueDeviceType.GC)
				{
					if (deviceType != UniqueDeviceType.Other)
					{
						if (deviceType == UniqueDeviceType.Keyboard)
						{
							InputSettingsSaveManager.addKeyboardBindings(inputSettingsData.inputActions, defaultBindings, false);
						}
					}
					else
					{
						this.addControllerBindings(inputSettingsData.inputActions, defaultBindings);
					}
				}
				else
				{
					this.addControllerBindings(inputSettingsData.inputActions, defaultGCBindings);
				}
			}
		}
		this.initControllerDeadZones(ref inputSettingsData, device);
		inputSettingsData.version = InputSettingsData.CURRENT_VERSION;
		return inputSettingsData;
	}

	private void updateInputSettings(InputSettingsData settingsData, IInputDevice device, bool isP2Debug)
	{
		InputSettingsData defaultInputSettings = this.GetDefaultInputSettings(device, isP2Debug);
		if (settingsData.version < 2)
		{
			settingsData.inputActions.Taunt = defaultInputSettings.inputActions.Taunt;
			settingsData.inputActions.TauntLeft = defaultInputSettings.inputActions.TauntLeft;
			settingsData.inputActions.TauntRight = defaultInputSettings.inputActions.TauntRight;
			settingsData.inputActions.TauntUp = defaultInputSettings.inputActions.TauntUp;
			settingsData.inputActions.TauntDown = defaultInputSettings.inputActions.TauntDown;
		}
		if (settingsData.version < 3)
		{
			settingsData.leftStickDeadZone = defaultInputSettings.leftStickDeadZone;
			settingsData.rightStickDeadZone = defaultInputSettings.rightStickDeadZone;
			settingsData.leftTriggerDeadZone = defaultInputSettings.leftTriggerDeadZone;
			settingsData.rightTriggerDeadZone = defaultInputSettings.rightTriggerDeadZone;
		}
		settingsData.version = InputSettingsData.CURRENT_VERSION;
	}

	private InputSettingsData getSavedInputSettings(IInputDevice device)
	{
		UniqueDeviceType deviceType = InputUtils.GetDeviceType(device);
		if (deviceType == UniqueDeviceType.GC)
		{
			return this.saveFileData.GetFromFile<InputSettingsData>("settings/gcController.settings");
		}
		if (deviceType == UniqueDeviceType.Keyboard)
		{
			return this.saveFileData.GetFromFile<InputSettingsData>("settings/keyboard.settings");
		}
		if (deviceType != UniqueDeviceType.Other)
		{
			return null;
		}
		return this.saveFileData.GetFromFile<InputSettingsData>("settings/xBoxController.settings");
	}

	private void addControllerBindings(PlayerInputActions playerInputAction, IEnumerable<DefaultInputBinding> bindings)
	{
		foreach (DefaultInputBinding current in bindings)
		{
			PlayerAction playerAction;
			if (PlayerInputActions.IsAxis(current.button))
			{
				playerAction = playerInputAction.GetAxisAction(current.button);
			}
			else
			{
				playerAction = playerInputAction.GetButtonAction(current.button);
			}
			if (playerAction != null)
			{
				playerAction.ClearBindings();
			}
		}
		foreach (DefaultInputBinding current2 in bindings)
		{
			PlayerAction playerAction2;
			if (PlayerInputActions.IsAxis(current2.button))
			{
				playerAction2 = playerInputAction.GetAxisAction(current2.button);
			}
			else
			{
				playerAction2 = playerInputAction.GetButtonAction(current2.button);
			}
			if (playerAction2 != null)
			{
				playerAction2.AddBinding(new DeviceBindingSource(current2.controlType));
			}
		}
	}

	private static void addDebugKeyboardBindings(PlayerInputActions playerInputAction, IEnumerable<DefaultInputBinding> bindings)
	{
		InputSettingsSaveManager.addKeyboardBindings(playerInputAction, bindings, true);
	}

	private static void addKeyboardBindings(PlayerInputActions playerInputAction, IEnumerable<DefaultInputBinding> bindings, bool useP2Debug = false)
	{
		foreach (DefaultInputBinding current in bindings)
		{
			PlayerAction playerAction;
			if (PlayerInputActions.IsAxis(current.button))
			{
				playerAction = playerInputAction.GetAxisAction(current.button);
			}
			else
			{
				playerAction = playerInputAction.GetButtonAction(current.button);
			}
			if (playerAction != null)
			{
				Key key = current.defaultP1Key;
				if (useP2Debug)
				{
					key = current.defaultP2Key;
				}
				if (key != Key.None)
				{
					playerAction.AddBinding(new KeyBindingSource(new Key[]
					{
						key
					}));
				}
			}
		}
	}
}
