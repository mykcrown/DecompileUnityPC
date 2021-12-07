// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;

public class SettingsScreenAPI : ISettingsScreenAPI
{
	[Inject]
	public IUserInputManager userInputManager
	{
		get;
		set;
	}

	[Inject]
	public PlayerInputLocator playerInput
	{
		get;
		set;
	}

	public PlayerInputPort InputPort
	{
		get;
		private set;
	}

	public void SetPlayer(int portId)
	{
		this.InputPort = this.userInputManager.GetPortWithId(portId);
		if (this.InputPort == null)
		{
			this.InputPort = this.userInputManager.GetLocalPlayer(0);
		}
		if (this.InputPort.Device == InputDevice.Null)
		{
			this.playerInput.Input.AssignFirstAvailableDevice(this.InputPort, DevicePreference.Any);
		}
	}
}
