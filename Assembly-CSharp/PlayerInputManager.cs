using System;
using System.Collections.Generic;
using InControl;
using UnityEngine;

// Token: 0x020006AC RID: 1708
public class PlayerInputManager : ClientBehavior, ITickable
{
	// Token: 0x17000A5F RID: 2655
	// (get) Token: 0x06002A69 RID: 10857 RVA: 0x000E0210 File Offset: 0x000DE610
	// (set) Token: 0x06002A6A RID: 10858 RVA: 0x000E0218 File Offset: 0x000DE618
	[Inject]
	public CharacterSelectCalculator characterSelectCalculator { get; set; }

	// Token: 0x17000A60 RID: 2656
	// (get) Token: 0x06002A6B RID: 10859 RVA: 0x000E0221 File Offset: 0x000DE621
	// (set) Token: 0x06002A6C RID: 10860 RVA: 0x000E0229 File Offset: 0x000DE629
	[Inject]
	public IUserManager userManager { get; set; }

	// Token: 0x17000A61 RID: 2657
	// (get) Token: 0x06002A6D RID: 10861 RVA: 0x000E0232 File Offset: 0x000DE632
	// (set) Token: 0x06002A6E RID: 10862 RVA: 0x000E023A File Offset: 0x000DE63A
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x17000A62 RID: 2658
	// (get) Token: 0x06002A6F RID: 10863 RVA: 0x000E0243 File Offset: 0x000DE643
	// (set) Token: 0x06002A70 RID: 10864 RVA: 0x000E024B File Offset: 0x000DE64B
	[Inject]
	public IDevConsole devConsole { get; set; }

	// Token: 0x17000A63 RID: 2659
	// (get) Token: 0x06002A71 RID: 10865 RVA: 0x000E0254 File Offset: 0x000DE654
	// (set) Token: 0x06002A72 RID: 10866 RVA: 0x000E025C File Offset: 0x000DE65C
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x17000A64 RID: 2660
	// (get) Token: 0x06002A73 RID: 10867 RVA: 0x000E0265 File Offset: 0x000DE665
	// (set) Token: 0x06002A74 RID: 10868 RVA: 0x000E026D File Offset: 0x000DE66D
	public bool ListenForDevices { get; set; }

	// Token: 0x06002A75 RID: 10869 RVA: 0x000E0278 File Offset: 0x000DE678
	public void Init(ConfigData config)
	{
		this.inControlManager = new GameObject("InControl Manager").AddComponent<InControlManager>();
		this.inControlManager.logDebugInfo = true;
		this.inControlManager.transform.SetParent(base.transform);
		this.inControlManager.enableXInput = config.inputConfig.enableXInput;
		this.inControlManager.Init();
		InputManager.OnDeviceAttached += this.onDeviceAttached;
		InputManager.OnDeviceDetached += this.onDeviceDetached;
		List<string> list = new List<string>(UnityInputDeviceProfileList.Profiles);
		list.AddRange(PlayerInputManager.customInputDevices);
		UnityInputDeviceProfileList.Profiles = list.ToArray();
		this.allDevices.Add(new KeyboardInputDevice());
		foreach (InputDevice item in InputManager.Devices)
		{
			this.allDevices.Add(item);
		}
		base.events.Subscribe(typeof(SetPlayerTypeRequest), new Events.EventHandler(this.onSetPlayerTypeCommand));
		base.events.Subscribe(typeof(UnbindPlayerControlCommand), new Events.EventHandler(this.onUnbindPlayerControlCommand));
		base.events.Subscribe(typeof(ResetPlayerControlsCommand), new Events.EventHandler(this.onResetPlayerControlsCommand));
	}

	// Token: 0x06002A76 RID: 10870 RVA: 0x000E03E4 File Offset: 0x000DE7E4
	public IInputDevice GetKeyboard()
	{
		foreach (IInputDevice inputDevice in this.allDevices)
		{
			if (inputDevice is KeyboardInputDevice)
			{
				return inputDevice;
			}
		}
		return null;
	}

	// Token: 0x06002A77 RID: 10871 RVA: 0x000E0450 File Offset: 0x000DE850
	public void ConsumeButtonPresses()
	{
		foreach (IInputDevice inputDevice in this.allDevices)
		{
			if (inputDevice is KeyboardInputDevice)
			{
				(inputDevice as KeyboardInputDevice).DoUpdate();
			}
		}
		InputManager.UpdateInternal();
	}

	// Token: 0x06002A78 RID: 10872 RVA: 0x000E04C0 File Offset: 0x000DE8C0
	public PlayerInputPort GetFirstPortWithNoUser()
	{
		return this.userInputManager.GetFirstPortWithNoUser();
	}

	// Token: 0x06002A79 RID: 10873 RVA: 0x000E04D0 File Offset: 0x000DE8D0
	private IInputDevice getFirstInactiveDevice(DevicePreference preference = DevicePreference.Any)
	{
		foreach (IInputDevice inputDevice in this.allDevices)
		{
			if (this.isDeviceAvailable(inputDevice))
			{
				if (preference == DevicePreference.Any)
				{
					return inputDevice;
				}
				bool flag = inputDevice is KeyboardInputDevice;
				if (preference == DevicePreference.Keyboard && flag)
				{
					return inputDevice;
				}
				if (preference == DevicePreference.Gamepad && !flag)
				{
					return inputDevice;
				}
			}
		}
		if (preference != DevicePreference.Any)
		{
			return this.getFirstInactiveDevice(DevicePreference.Any);
		}
		return null;
	}

	// Token: 0x06002A7A RID: 10874 RVA: 0x000E0588 File Offset: 0x000DE988
	private PlayerInputPort findPortWithDevice(IInputDevice device)
	{
		return this.userInputManager.GetPortWithDevice(device);
	}

	// Token: 0x06002A7B RID: 10875 RVA: 0x000E0596 File Offset: 0x000DE996
	private bool isDeviceAvailable(IInputDevice device)
	{
		return this.userInputManager.GetPortWithDevice(device) == null;
	}

	// Token: 0x06002A7C RID: 10876 RVA: 0x000E05AA File Offset: 0x000DE9AA
	private void onDeviceAttached(InputDevice device)
	{
		if (device != null && this.isDeviceAvailable(device) && !this.allDevices.Contains(device))
		{
			this.allDevices.Add(device);
		}
	}

	// Token: 0x06002A7D RID: 10877 RVA: 0x000E05DC File Offset: 0x000DE9DC
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

	// Token: 0x06002A7E RID: 10878 RVA: 0x000E0628 File Offset: 0x000DEA28
	public override void OnDestroy()
	{
		base.OnDestroy();
		base.events.Unsubscribe(typeof(SetPlayerTypeRequest), new Events.EventHandler(this.onSetPlayerTypeCommand));
		base.events.Unsubscribe(typeof(UnbindPlayerControlCommand), new Events.EventHandler(this.onUnbindPlayerControlCommand));
		base.events.Unsubscribe(typeof(ResetPlayerControlsCommand), new Events.EventHandler(this.onResetPlayerControlsCommand));
	}

	// Token: 0x06002A7F RID: 10879 RVA: 0x000E069E File Offset: 0x000DEA9E
	public PlayerInputPort GetPort(PlayerNum player)
	{
		return this.userInputManager.GetPortWithPlayerNum(player);
	}

	// Token: 0x06002A80 RID: 10880 RVA: 0x000E06AC File Offset: 0x000DEAAC
	public PlayerInputPort GetPort(InputDevice device)
	{
		return this.userInputManager.GetPortWithDevice(device);
	}

	// Token: 0x06002A81 RID: 10881 RVA: 0x000E06BC File Offset: 0x000DEABC
	public void Update()
	{
		if (this.ListenForDevices)
		{
			this.updateDeviceActivity();
		}
		foreach (KeyValuePair<int, PlayerInputPort> keyValuePair in this.userInputManager.Ports.Raw())
		{
			int key = keyValuePair.Key;
			this.userInputManager.Ports[key].DoUpdate();
		}
	}

	// Token: 0x06002A82 RID: 10882 RVA: 0x000E074C File Offset: 0x000DEB4C
	public void TickFrame()
	{
		foreach (KeyValuePair<int, PlayerInputPort> keyValuePair in this.userInputManager.Ports.Raw())
		{
			int key = keyValuePair.Key;
			this.userInputManager.Ports[key].TickFrame();
		}
	}

	// Token: 0x06002A83 RID: 10883 RVA: 0x000E07CC File Offset: 0x000DEBCC
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

	// Token: 0x06002A84 RID: 10884 RVA: 0x000E08B0 File Offset: 0x000DECB0
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

	// Token: 0x06002A85 RID: 10885 RVA: 0x000E097C File Offset: 0x000DED7C
	public void AssignFirstAvailableDevice(PlayerInputPort inputPort, DevicePreference devicePreference = DevicePreference.Any)
	{
		IInputDevice firstInactiveDevice = this.getFirstInactiveDevice(devicePreference);
		this.bindDevice(firstInactiveDevice, inputPort);
	}

	// Token: 0x06002A86 RID: 10886 RVA: 0x000E0999 File Offset: 0x000DED99
	private void bindDevice(IInputDevice device, PlayerInputPort inputPort)
	{
		if (this.userInputManager.BindToAvailableUser(inputPort, device))
		{
			base.signalBus.GetSignal<SetPlayerDeviceSignal>().Dispatch(inputPort.Id, device);
		}
	}

	// Token: 0x06002A87 RID: 10887 RVA: 0x000E09C4 File Offset: 0x000DEDC4
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

	// Token: 0x06002A88 RID: 10888 RVA: 0x000E0A28 File Offset: 0x000DEE28
	private void onUnbindPlayerControlCommand(GameEvent message)
	{
		UnbindPlayerControlCommand unbindPlayerControlCommand = message as UnbindPlayerControlCommand;
		foreach (int key in this.userInputManager.Ports)
		{
			this.userInputManager.Ports[key].ClearBinding(unbindPlayerControlCommand.binding);
		}
	}

	// Token: 0x06002A89 RID: 10889 RVA: 0x000E0AA4 File Offset: 0x000DEEA4
	private void onResetPlayerControlsCommand(GameEvent message)
	{
		foreach (int key in this.userInputManager.Ports)
		{
			this.userInputManager.Ports[key].ResetBindings();
		}
	}

	// Token: 0x17000A65 RID: 2661
	// (get) Token: 0x06002A8A RID: 10890 RVA: 0x000E0B14 File Offset: 0x000DEF14
	public bool IsControllerConnected
	{
		get
		{
			return Input.GetJoystickNames().Length > 0;
		}
	}

	// Token: 0x04001EAA RID: 7850
	private InControlManager inControlManager;

	// Token: 0x04001EAB RID: 7851
	private List<IInputDevice> allDevices = new List<IInputDevice>();

	// Token: 0x04001EAC RID: 7852
	private List<IInputDevice> disabledThisFrame = new List<IInputDevice>();

	// Token: 0x04001EAD RID: 7853
	private static List<string> customInputDevices = new List<string>
	{
		"InControl.GameCubeMacMayflashProfile",
		"InControl.GameCubeWiiUWinProfile",
		"InControl.GameCubeWinMayflashProfile"
	};
}
