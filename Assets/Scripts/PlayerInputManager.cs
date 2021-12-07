// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : ClientBehavior, ITickable
{
	private InControlManager inControlManager;

	private List<IInputDevice> allDevices = new List<IInputDevice>();

	private List<IInputDevice> disabledThisFrame = new List<IInputDevice>();

	private static List<string> customInputDevices = new List<string>
	{
		"InControl.GameCubeMacMayflashProfile",
		"InControl.GameCubeWiiUWinProfile",
		"InControl.GameCubeWinMayflashProfile"
	};

	[Inject]
	public CharacterSelectCalculator characterSelectCalculator
	{
		get;
		set;
	}

	[Inject]
	public IUserManager userManager
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
	public IDevConsole devConsole
	{
		get;
		set;
	}

	[Inject]
	public UIManager uiManager
	{
		get;
		set;
	}

	public bool ListenForDevices
	{
		get;
		set;
	}

	public bool IsControllerConnected
	{
		get
		{
			return Input.GetJoystickNames().Length > 0;
		}
	}

	public void Init(ConfigData config)
	{
		this.inControlManager = new GameObject("InControl Manager").AddComponent<InControlManager>();
		this.inControlManager.logDebugInfo = true;
		this.inControlManager.transform.SetParent(base.transform);
		this.inControlManager.enableXInput = config.inputConfig.enableXInput;
		this.inControlManager.Init();
		InputManager.OnDeviceAttached += new Action<InputDevice>(this.onDeviceAttached);
		InputManager.OnDeviceDetached += new Action<InputDevice>(this.onDeviceDetached);
		List<string> list = new List<string>(UnityInputDeviceProfileList.Profiles);
		list.AddRange(PlayerInputManager.customInputDevices);
		UnityInputDeviceProfileList.Profiles = list.ToArray();
		this.allDevices.Add(new KeyboardInputDevice());
		foreach (InputDevice current in InputManager.Devices)
		{
			this.allDevices.Add(current);
		}
		base.events.Subscribe(typeof(SetPlayerTypeRequest), new Events.EventHandler(this.onSetPlayerTypeCommand));
		base.events.Subscribe(typeof(UnbindPlayerControlCommand), new Events.EventHandler(this.onUnbindPlayerControlCommand));
		base.events.Subscribe(typeof(ResetPlayerControlsCommand), new Events.EventHandler(this.onResetPlayerControlsCommand));
	}

	public IInputDevice GetKeyboard()
	{
		foreach (IInputDevice current in this.allDevices)
		{
			if (current is KeyboardInputDevice)
			{
				return current;
			}
		}
		return null;
	}

	public void ConsumeButtonPresses()
	{
		foreach (IInputDevice current in this.allDevices)
		{
			if (current is KeyboardInputDevice)
			{
				(current as KeyboardInputDevice).DoUpdate();
			}
		}
		InputManager.UpdateInternal();
	}

	public PlayerInputPort GetFirstPortWithNoUser()
	{
		return this.userInputManager.GetFirstPortWithNoUser();
	}

	private IInputDevice getFirstInactiveDevice(DevicePreference preference = DevicePreference.Any)
	{
		foreach (IInputDevice current in this.allDevices)
		{
			if (this.isDeviceAvailable(current))
			{
				if (preference == DevicePreference.Any)
				{
					IInputDevice result = current;
					return result;
				}
				bool flag = current is KeyboardInputDevice;
				if (preference == DevicePreference.Keyboard && flag)
				{
					IInputDevice result = current;
					return result;
				}
				if (preference == DevicePreference.Gamepad && !flag)
				{
					IInputDevice result = current;
					return result;
				}
			}
		}
		if (preference != DevicePreference.Any)
		{
			return this.getFirstInactiveDevice(DevicePreference.Any);
		}
		return null;
	}

	private PlayerInputPort findPortWithDevice(IInputDevice device)
	{
		return this.userInputManager.GetPortWithDevice(device);
	}

	private bool isDeviceAvailable(IInputDevice device)
	{
		return this.userInputManager.GetPortWithDevice(device) == null;
	}

	private void onDeviceAttached(InputDevice device)
	{
		if (device != null && this.isDeviceAvailable(device) && !this.allDevices.Contains(device))
		{
			this.allDevices.Add(device);
		}
	}

	private void onDeviceDetached(InputDevice device)
	{
		PlayerInputPort playerInputPort = this.findPortWithDevice(device);
		if (playerInputPort != null)
		{
			this.userInputManager.Unbind(playerInputPort);
		}
		if (this.allDevices.Contains(device))
		{
			this.allDevices.Remove(device);
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		base.events.Unsubscribe(typeof(SetPlayerTypeRequest), new Events.EventHandler(this.onSetPlayerTypeCommand));
		base.events.Unsubscribe(typeof(UnbindPlayerControlCommand), new Events.EventHandler(this.onUnbindPlayerControlCommand));
		base.events.Unsubscribe(typeof(ResetPlayerControlsCommand), new Events.EventHandler(this.onResetPlayerControlsCommand));
	}

	public PlayerInputPort GetPort(PlayerNum player)
	{
		return this.userInputManager.GetPortWithPlayerNum(player);
	}

	public PlayerInputPort GetPort(InputDevice device)
	{
		return this.userInputManager.GetPortWithDevice(device);
	}

	public void Update()
	{
		if (this.ListenForDevices)
		{
			this.updateDeviceActivity();
		}
		foreach (KeyValuePair<int, PlayerInputPort> current in this.userInputManager.Ports.Raw())
		{
			int key = current.Key;
			this.userInputManager.Ports[key].DoUpdate();
		}
	}

	public void TickFrame()
	{
		foreach (KeyValuePair<int, PlayerInputPort> current in this.userInputManager.Ports.Raw())
		{
			int key = current.Key;
			this.userInputManager.Ports[key].TickFrame();
		}
	}

	private void updateDeviceActivity()
	{
		for (int i = this.allDevices.Count - 1; i >= 0; i--)
		{
			IInputDevice inputDevice = this.allDevices[i];
			if (inputDevice is KeyboardInputDevice && !this.uiManager.IsTextEntryMode)
			{
				(inputDevice as KeyboardInputDevice).DoUpdate();
			}
			if (base.battleServerAPI.IsOnlineMatchReady && base.battleServerAPI.IsSinglePlayerNetworkGame)
			{
				this.assignEveryDeviceToPrimaryUser(inputDevice);
			}
			else if (this.userInputManager.GetUserWithDevice(inputDevice) == null && !this.disabledThisFrame.Contains(inputDevice) && InputUtils.IsDeviceActivity(inputDevice))
			{
				PlayerInputPort firstPortWithNoDevice = this.userInputManager.GetFirstPortWithNoDevice();
				if (firstPortWithNoDevice != null)
				{
					this.bindDevice(inputDevice, firstPortWithNoDevice);
				}
			}
		}
		this.disabledThisFrame.Clear();
	}

	private void assignEveryDeviceToPrimaryUser(IInputDevice device)
	{
		if (InputUtils.IsDeviceActivity(device))
		{
			IUser primaryUser = this.userManager.PrimaryUser;
			PlayerNum getPrimaryLocalPlayer = base.battleServerAPI.GetPrimaryLocalPlayer;
			IInputDevice deviceWithPlayerNum = this.userInputManager.GetDeviceWithPlayerNum(getPrimaryLocalPlayer);
			if (deviceWithPlayerNum != device)
			{
				PlayerInputPort portWithPlayerNum = this.userInputManager.GetPortWithPlayerNum(getPrimaryLocalPlayer);
				if (portWithPlayerNum != null)
				{
					this.userInputManager.Unbind(portWithPlayerNum);
					PlayerInputPort portWithDevice = this.userInputManager.GetPortWithDevice(device);
					if (portWithDevice != null)
					{
						this.userInputManager.Unbind(portWithDevice);
					}
					this.userInputManager.AssignPlayerNum(portWithPlayerNum.Id, getPrimaryLocalPlayer);
					this.userInputManager.Bind(portWithPlayerNum, device, primaryUser);
					base.signalBus.GetSignal<SetPlayerDeviceSignal>().Dispatch(portWithPlayerNum.Id, device);
				}
			}
		}
	}

	public void AssignFirstAvailableDevice(PlayerInputPort inputPort, DevicePreference devicePreference = DevicePreference.Any)
	{
		IInputDevice firstInactiveDevice = this.getFirstInactiveDevice(devicePreference);
		this.bindDevice(firstInactiveDevice, inputPort);
	}

	private void bindDevice(IInputDevice device, PlayerInputPort inputPort)
	{
		if (this.userInputManager.BindToAvailableUser(inputPort, device))
		{
			base.signalBus.GetSignal<SetPlayerDeviceSignal>().Dispatch(inputPort.Id, device);
		}
	}

	private void onSetPlayerTypeCommand(GameEvent message)
	{
		SetPlayerTypeRequest setPlayerTypeRequest = message as SetPlayerTypeRequest;
		PlayerInputPort port = this.GetPort(setPlayerTypeRequest.playerNum);
		if (port != null && setPlayerTypeRequest.playerType == PlayerType.None)
		{
			IInputDevice deviceWithPort = this.userInputManager.GetDeviceWithPort(port);
			if (deviceWithPort != null)
			{
				this.disabledThisFrame.Add(deviceWithPort);
			}
			this.userInputManager.ResetPlayerMappingForPort(port);
		}
	}

	private void onUnbindPlayerControlCommand(GameEvent message)
	{
		UnbindPlayerControlCommand unbindPlayerControlCommand = message as UnbindPlayerControlCommand;
		foreach (int current in this.userInputManager.Ports)
		{
			this.userInputManager.Ports[current].ClearBinding(unbindPlayerControlCommand.binding);
		}
	}

	private void onResetPlayerControlsCommand(GameEvent message)
	{
		foreach (int current in this.userInputManager.Ports)
		{
			this.userInputManager.Ports[current].ResetBindings();
		}
	}
}
