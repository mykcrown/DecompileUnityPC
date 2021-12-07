﻿using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000F0 RID: 240
	public class LogitechF310ModeDMacProfile : NativeInputDeviceProfile
	{
		// Token: 0x060005CA RID: 1482 RVA: 0x0001F3B4 File Offset: 0x0001D7B4
		public LogitechF310ModeDMacProfile()
		{
			base.Name = "Logitech F310 Controller";
			base.Meta = "Logitech F310 Controller on Mac";
			base.DeviceClass = InputDeviceClass.Controller;
			base.DeviceStyle = InputDeviceStyle.Xbox360;
			base.IncludePlatforms = new string[]
			{
				"OS X"
			};
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(49686)
				}
			};
			base.ButtonMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action1,
					Source = NativeInputDeviceProfile.Button(1)
				},
				new InputControlMapping
				{
					Handle = "B",
					Target = InputControlType.Action2,
					Source = NativeInputDeviceProfile.Button(2)
				},
				new InputControlMapping
				{
					Handle = "X",
					Target = InputControlType.Action3,
					Source = NativeInputDeviceProfile.Button(0)
				},
				new InputControlMapping
				{
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = NativeInputDeviceProfile.Button(3)
				},
				new InputControlMapping
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = NativeInputDeviceProfile.Button(12)
				},
				new InputControlMapping
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = NativeInputDeviceProfile.Button(13)
				},
				new InputControlMapping
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = NativeInputDeviceProfile.Button(14)
				},
				new InputControlMapping
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = NativeInputDeviceProfile.Button(15)
				},
				new InputControlMapping
				{
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = NativeInputDeviceProfile.Button(4)
				},
				new InputControlMapping
				{
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = NativeInputDeviceProfile.Button(5)
				},
				new InputControlMapping
				{
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = NativeInputDeviceProfile.Button(6)
				},
				new InputControlMapping
				{
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = NativeInputDeviceProfile.Button(7)
				},
				new InputControlMapping
				{
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = NativeInputDeviceProfile.Button(10)
				},
				new InputControlMapping
				{
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = NativeInputDeviceProfile.Button(11)
				},
				new InputControlMapping
				{
					Handle = "Back",
					Target = InputControlType.Back,
					Source = NativeInputDeviceProfile.Button(8)
				},
				new InputControlMapping
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = NativeInputDeviceProfile.Button(9)
				}
			};
			base.AnalogMappings = new InputControlMapping[]
			{
				NativeInputDeviceProfile.LeftStickLeftMapping(0),
				NativeInputDeviceProfile.LeftStickRightMapping(0),
				NativeInputDeviceProfile.LeftStickUpMapping(1),
				NativeInputDeviceProfile.LeftStickDownMapping(1),
				NativeInputDeviceProfile.RightStickLeftMapping(2),
				NativeInputDeviceProfile.RightStickRightMapping(2),
				NativeInputDeviceProfile.RightStickUpMapping(3),
				NativeInputDeviceProfile.RightStickDownMapping(3)
			};
		}
	}
}
