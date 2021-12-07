using System;
using System.Collections.Generic;
using InControl;

// Token: 0x02000616 RID: 1558
public class InputSettingsSaveManager : IInputSettingsSaveManager
{
	// Token: 0x17000978 RID: 2424
	// (get) Token: 0x0600266B RID: 9835 RVA: 0x000BCD4F File Offset: 0x000BB14F
	// (set) Token: 0x0600266C RID: 9836 RVA: 0x000BCD57 File Offset: 0x000BB157
	[Inject]
	public ISaveFileData saveFileData { get; set; }

	// Token: 0x17000979 RID: 2425
	// (get) Token: 0x0600266D RID: 9837 RVA: 0x000BCD60 File Offset: 0x000BB160
	// (set) Token: 0x0600266E RID: 9838 RVA: 0x000BCD68 File Offset: 0x000BB168
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x0600266F RID: 9839 RVA: 0x000BCD74 File Offset: 0x000BB174
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

	// Token: 0x06002670 RID: 9840 RVA: 0x000BCDB0 File Offset: 0x000BB1B0
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

	// Token: 0x06002671 RID: 9841 RVA: 0x000BCE2C File Offset: 0x000BB22C
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

	// Token: 0x06002672 RID: 9842 RVA: 0x000BCE7C File Offset: 0x000BB27C
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

	// Token: 0x06002673 RID: 9843 RVA: 0x000BCF4C File Offset: 0x000BB34C
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

	// Token: 0x06002674 RID: 9844 RVA: 0x000BD024 File Offset: 0x000BB424
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

	// Token: 0x06002675 RID: 9845 RVA: 0x000BD088 File Offset: 0x000BB488
	private void addControllerBindings(PlayerInputActions playerInputAction, IEnumerable<DefaultInputBinding> bindings)
	{
		foreach (DefaultInputBinding defaultInputBinding in bindings)
		{
			PlayerAction playerAction;
			if (PlayerInputActions.IsAxis(defaultInputBinding.button))
			{
				playerAction = playerInputAction.GetAxisAction(defaultInputBinding.button);
			}
			else
			{
				playerAction = playerInputAction.GetButtonAction(defaultInputBinding.button);
			}
			if (playerAction != null)
			{
				playerAction.ClearBindings();
			}
		}
		foreach (DefaultInputBinding defaultInputBinding2 in bindings)
		{
			PlayerAction playerAction2;
			if (PlayerInputActions.IsAxis(defaultInputBinding2.button))
			{
				playerAction2 = playerInputAction.GetAxisAction(defaultInputBinding2.button);
			}
			else
			{
				playerAction2 = playerInputAction.GetButtonAction(defaultInputBinding2.button);
			}
			if (playerAction2 != null)
			{
				playerAction2.AddBinding(new DeviceBindingSource(defaultInputBinding2.controlType));
			}
		}
	}

	// Token: 0x06002676 RID: 9846 RVA: 0x000BD1A4 File Offset: 0x000BB5A4
	private static void addDebugKeyboardBindings(PlayerInputActions playerInputAction, IEnumerable<DefaultInputBinding> bindings)
	{
		InputSettingsSaveManager.addKeyboardBindings(playerInputAction, bindings, true);
	}

	// Token: 0x06002677 RID: 9847 RVA: 0x000BD1B0 File Offset: 0x000BB5B0
	private static void addKeyboardBindings(PlayerInputActions playerInputAction, IEnumerable<DefaultInputBinding> bindings, bool useP2Debug = false)
	{
		foreach (DefaultInputBinding defaultInputBinding in bindings)
		{
			PlayerAction playerAction;
			if (PlayerInputActions.IsAxis(defaultInputBinding.button))
			{
				playerAction = playerInputAction.GetAxisAction(defaultInputBinding.button);
			}
			else
			{
				playerAction = playerInputAction.GetButtonAction(defaultInputBinding.button);
			}
			if (playerAction != null)
			{
				Key key = defaultInputBinding.defaultP1Key;
				if (useP2Debug)
				{
					key = defaultInputBinding.defaultP2Key;
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

	// Token: 0x04001C21 RID: 7201
	private const string KEYBOARD_FILE = "settings/keyboard.settings";

	// Token: 0x04001C22 RID: 7202
	private const string GC_CONTROLLER_FILE = "settings/gcController.settings";

	// Token: 0x04001C23 RID: 7203
	private const string XBOX_CONTROLLER_FILE = "settings/xBoxController.settings";
}
