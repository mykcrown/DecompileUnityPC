using System;

namespace InControl
{
	// Token: 0x020001C3 RID: 451
	[AutoDiscover]
	public class XboxOneWin10AEProfile : UnityInputDeviceProfile
	{
		// Token: 0x060007BF RID: 1983 RVA: 0x0004865C File Offset: 0x00046A5C
		public XboxOneWin10AEProfile()
		{
			base.Name = "XBox One Controller";
			base.Meta = "XBox One Controller on Windows";
			base.DeviceClass = InputDeviceClass.Controller;
			base.DeviceStyle = InputDeviceStyle.XboxOne;
			base.IncludePlatforms = new string[]
			{
				"Windows"
			};
			base.ExcludePlatforms = new string[]
			{
				"Windows 7",
				"Windows 8"
			};
			base.MinSystemBuildNumber = 14393;
			this.JoystickNames = new string[]
			{
				"Controller (Xbox One For Windows)",
				"Xbox Bluetooth Gamepad"
			};
			base.ButtonMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action1,
					Source = UnityInputDeviceProfile.Button(0)
				},
				new InputControlMapping
				{
					Handle = "B",
					Target = InputControlType.Action2,
					Source = UnityInputDeviceProfile.Button(1)
				},
				new InputControlMapping
				{
					Handle = "X",
					Target = InputControlType.Action3,
					Source = UnityInputDeviceProfile.Button(2)
				},
				new InputControlMapping
				{
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = UnityInputDeviceProfile.Button(3)
				},
				new InputControlMapping
				{
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = UnityInputDeviceProfile.Button(4)
				},
				new InputControlMapping
				{
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = UnityInputDeviceProfile.Button(5)
				},
				new InputControlMapping
				{
					Handle = "View",
					Target = InputControlType.View,
					Source = UnityInputDeviceProfile.Button(6)
				},
				new InputControlMapping
				{
					Handle = "Menu",
					Target = InputControlType.Menu,
					Source = UnityInputDeviceProfile.Button(7)
				},
				new InputControlMapping
				{
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = UnityInputDeviceProfile.Button(8)
				},
				new InputControlMapping
				{
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = UnityInputDeviceProfile.Button(9)
				},
				new InputControlMapping
				{
					Handle = "Guide",
					Target = InputControlType.System,
					Source = UnityInputDeviceProfile.Button(10)
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
				UnityInputDeviceProfile.DPadLeftMapping(UnityInputDeviceProfile.Analog5),
				UnityInputDeviceProfile.DPadRightMapping(UnityInputDeviceProfile.Analog5),
				UnityInputDeviceProfile.DPadUpMapping2(UnityInputDeviceProfile.Analog6),
				UnityInputDeviceProfile.DPadDownMapping2(UnityInputDeviceProfile.Analog6),
				new InputControlMapping
				{
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = UnityInputDeviceProfile.Analog(8),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = UnityInputDeviceProfile.Analog(9),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				}
			};
		}
	}
}
