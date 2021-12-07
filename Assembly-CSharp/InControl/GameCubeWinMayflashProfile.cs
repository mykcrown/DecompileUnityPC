using System;

namespace InControl
{
	// Token: 0x02000691 RID: 1681
	[AutoDiscover]
	public class GameCubeWinMayflashProfile : GameCubeDeviceProfile
	{
		// Token: 0x06002985 RID: 10629 RVA: 0x000DD92C File Offset: 0x000DBD2C
		public GameCubeWinMayflashProfile()
		{
			base.Name = "GameCube Controller (Mayflash, Win)";
			base.Meta = "GameCube Controller (Mayflash, Win)";
			base.IncludePlatforms = new string[]
			{
				"Windows"
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
					Handle = "L",
					Target = InputControlType.LeftTrigger,
					Source = UnityInputDeviceProfile.Button4
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
				UnityInputDeviceProfile.RightStickLeftMapping(UnityInputDeviceProfile.Analog5),
				UnityInputDeviceProfile.RightStickRightMapping(UnityInputDeviceProfile.Analog5),
				UnityInputDeviceProfile.RightStickUpMapping(UnityInputDeviceProfile.Analog2),
				UnityInputDeviceProfile.RightStickDownMapping(UnityInputDeviceProfile.Analog2),
				UnityInputDeviceProfile.LeftTriggerMapping(UnityInputDeviceProfile.Analog3),
				UnityInputDeviceProfile.RightTriggerMapping(UnityInputDeviceProfile.Analog4)
			};
		}
	}
}
