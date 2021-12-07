using System;

namespace InControl
{
	// Token: 0x02000690 RID: 1680
	[AutoDiscover]
	public class GameCubeWiiUWinProfile : GameCubeDeviceProfile
	{
		// Token: 0x06002984 RID: 10628 RVA: 0x000DD64C File Offset: 0x000DBA4C
		public GameCubeWiiUWinProfile()
		{
			base.Name = "GameCube Controller WiiU";
			base.Meta = "GameCube Controller WiiU on Windows";
			base.DeviceClass = InputDeviceClass.Controller;
			base.DeviceStyle = InputDeviceStyle.NintendoGameCube;
			base.IncludePlatforms = new string[]
			{
				"Windows"
			};
			this.JoystickNames = new string[]
			{
				"vJoy - Virtual Joystick"
			};
			base.ButtonMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action1,
					Source = UnityInputDeviceProfile.Button0
				},
				new InputControlMapping
				{
					Handle = "X",
					Target = InputControlType.Action3,
					Source = UnityInputDeviceProfile.Button2
				},
				new InputControlMapping
				{
					Handle = "B",
					Target = InputControlType.Action2,
					Source = UnityInputDeviceProfile.Button1
				},
				new InputControlMapping
				{
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = UnityInputDeviceProfile.Button3
				},
				new InputControlMapping
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = UnityInputDeviceProfile.Button7
				},
				new InputControlMapping
				{
					Handle = "Z",
					Target = InputControlType.RightBumper,
					Source = UnityInputDeviceProfile.Button4
				},
				new InputControlMapping
				{
					Handle = "L",
					Target = InputControlType.LeftTrigger,
					Source = UnityInputDeviceProfile.Button6
				},
				new InputControlMapping
				{
					Handle = "R",
					Target = InputControlType.RightTrigger,
					Source = UnityInputDeviceProfile.Button5
				},
				new InputControlMapping
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = UnityInputDeviceProfile.Button8
				},
				new InputControlMapping
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = UnityInputDeviceProfile.Button9
				},
				new InputControlMapping
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = UnityInputDeviceProfile.Button10
				},
				new InputControlMapping
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = UnityInputDeviceProfile.Button11
				}
			};
			base.AnalogMappings = new InputControlMapping[]
			{
				UnityInputDeviceProfile.LeftStickLeftMapping(UnityInputDeviceProfile.Analog0),
				UnityInputDeviceProfile.LeftStickRightMapping(UnityInputDeviceProfile.Analog0),
				UnityInputDeviceProfile.LeftStickUpMapping(UnityInputDeviceProfile.Analog1),
				UnityInputDeviceProfile.LeftStickDownMapping(UnityInputDeviceProfile.Analog1),
				UnityInputDeviceProfile.RightStickLeftMapping(UnityInputDeviceProfile.Analog3),
				UnityInputDeviceProfile.RightStickRightMapping(UnityInputDeviceProfile.Analog3),
				UnityInputDeviceProfile.RightStickUpMapping(UnityInputDeviceProfile.Analog4),
				UnityInputDeviceProfile.RightStickDownMapping(UnityInputDeviceProfile.Analog4),
				UnityInputDeviceProfile.LeftTriggerMapping(UnityInputDeviceProfile.Analog2),
				UnityInputDeviceProfile.RightTriggerMapping(UnityInputDeviceProfile.Analog5)
			};
		}
	}
}
