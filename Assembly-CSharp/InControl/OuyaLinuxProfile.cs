﻿using System;

namespace InControl
{
	// Token: 0x02000199 RID: 409
	[AutoDiscover]
	public class OuyaLinuxProfile : UnityInputDeviceProfile
	{
		// Token: 0x06000795 RID: 1941 RVA: 0x0003FAC0 File Offset: 0x0003DEC0
		public OuyaLinuxProfile()
		{
			base.Name = "OUYA Controller";
			base.Meta = "OUYA Controller on Linux";
			base.DeviceClass = InputDeviceClass.Controller;
			base.DeviceStyle = InputDeviceStyle.Ouya;
			base.IncludePlatforms = new string[]
			{
				"Linux"
			};
			this.JoystickNames = new string[]
			{
				"OUYA Game Controller"
			};
			base.MaxUnityVersion = new VersionInfo(4, 9, 0, 0);
			base.LowerDeadZone = 0.3f;
			base.ButtonMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Handle = "O",
					Target = InputControlType.Action1,
					Source = UnityInputDeviceProfile.Button0
				},
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action2,
					Source = UnityInputDeviceProfile.Button3
				},
				new InputControlMapping
				{
					Handle = "U",
					Target = InputControlType.Action3,
					Source = UnityInputDeviceProfile.Button1
				},
				new InputControlMapping
				{
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = UnityInputDeviceProfile.Button2
				},
				new InputControlMapping
				{
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = UnityInputDeviceProfile.Button4
				},
				new InputControlMapping
				{
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = UnityInputDeviceProfile.Button5
				},
				new InputControlMapping
				{
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = UnityInputDeviceProfile.Button6
				},
				new InputControlMapping
				{
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = UnityInputDeviceProfile.Button7
				},
				new InputControlMapping
				{
					Handle = "System",
					Target = InputControlType.System,
					Source = UnityInputDeviceProfile.MenuKey
				},
				new InputControlMapping
				{
					Handle = "TouchPad Button",
					Target = InputControlType.TouchPadButton,
					Source = UnityInputDeviceProfile.MouseButton0
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
				new InputControlMapping
				{
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = UnityInputDeviceProfile.Analog2
				},
				new InputControlMapping
				{
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = UnityInputDeviceProfile.Analog5
				},
				new InputControlMapping
				{
					Handle = "TouchPad X Axis",
					Target = InputControlType.TouchPadXAxis,
					Source = UnityInputDeviceProfile.MouseXAxis,
					Raw = true
				},
				new InputControlMapping
				{
					Handle = "TouchPad Y Axis",
					Target = InputControlType.TouchPadYAxis,
					Source = UnityInputDeviceProfile.MouseYAxis,
					Raw = true
				}
			};
		}
	}
}
