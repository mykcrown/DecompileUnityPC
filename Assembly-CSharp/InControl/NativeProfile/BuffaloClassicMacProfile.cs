﻿using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000EB RID: 235
	public class BuffaloClassicMacProfile : NativeInputDeviceProfile
	{
		// Token: 0x060005C5 RID: 1477 RVA: 0x0001E0B8 File Offset: 0x0001C4B8
		public BuffaloClassicMacProfile()
		{
			base.Name = "iBuffalo Classic Controller";
			base.Meta = "iBuffalo Classic Controller on Mac";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[]
			{
				"OS X"
			};
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1411),
					ProductID = new ushort?(8288),
					VersionNumber = new uint?(256U)
				}
			};
			base.ButtonMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Handle = "B",
					Target = InputControlType.Action1,
					Source = NativeInputDeviceProfile.Button(1)
				},
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action2,
					Source = NativeInputDeviceProfile.Button(0)
				},
				new InputControlMapping
				{
					Handle = "Y",
					Target = InputControlType.Action3,
					Source = NativeInputDeviceProfile.Button(3)
				},
				new InputControlMapping
				{
					Handle = "X",
					Target = InputControlType.Action4,
					Source = NativeInputDeviceProfile.Button(2)
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
					Handle = "Select",
					Target = InputControlType.Select,
					Source = NativeInputDeviceProfile.Button(6)
				},
				new InputControlMapping
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = NativeInputDeviceProfile.Button(7)
				}
			};
			base.AnalogMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = NativeInputDeviceProfile.Analog(0),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = NativeInputDeviceProfile.Analog(0),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = NativeInputDeviceProfile.Analog(1),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = NativeInputDeviceProfile.Analog(1),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				}
			};
		}
	}
}
