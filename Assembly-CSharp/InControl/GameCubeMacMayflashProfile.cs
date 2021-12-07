using System;

namespace InControl
{
	// Token: 0x0200068F RID: 1679
	[AutoDiscover]
	public class GameCubeMacMayflashProfile : GameCubeDeviceProfile
	{
		// Token: 0x06002983 RID: 10627 RVA: 0x000DD3C0 File Offset: 0x000DB7C0
		public GameCubeMacMayflashProfile()
		{
			base.Name = "GameCube Controller (Mayflash, Mac)";
			base.Meta = "GameCube Controller (Mayflash, Mac)";
			base.IncludePlatforms = new string[]
			{
				"OS X"
			};
			this.JoystickNames = new string[]
			{
				"mayflash limited MAYFLASH GameCube Controller Adapter"
			};
			this.LastResortRegex = "mayflash";
			base.ButtonMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action1,
					Source = UnityInputDeviceProfile.Button1
				},
				new InputControlMapping
				{
					Handle = "X",
					Target = InputControlType.Action3,
					Source = UnityInputDeviceProfile.Button0
				},
				new InputControlMapping
				{
					Handle = "B",
					Target = InputControlType.Action2,
					Source = UnityInputDeviceProfile.Button2
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
					Source = UnityInputDeviceProfile.Button9
				},
				new InputControlMapping
				{
					Handle = "Z",
					Target = InputControlType.RightBumper,
					Source = UnityInputDeviceProfile.Button7
				},
				new InputControlMapping
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = UnityInputDeviceProfile.Button12
				},
				new InputControlMapping
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = UnityInputDeviceProfile.Button14
				},
				new InputControlMapping
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = UnityInputDeviceProfile.Button15
				},
				new InputControlMapping
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = UnityInputDeviceProfile.Button13
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
				UnityInputDeviceProfile.RightStickUpMapping(UnityInputDeviceProfile.Analog2),
				UnityInputDeviceProfile.RightStickDownMapping(UnityInputDeviceProfile.Analog2),
				UnityInputDeviceProfile.LeftTriggerMapping(UnityInputDeviceProfile.Analog4),
				UnityInputDeviceProfile.RightTriggerMapping(UnityInputDeviceProfile.Analog5)
			};
		}
	}
}
