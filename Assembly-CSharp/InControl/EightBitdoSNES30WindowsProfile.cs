﻿using System;

namespace InControl
{
	// Token: 0x0200015E RID: 350
	[AutoDiscover]
	public class EightBitdoSNES30WindowsProfile : UnityInputDeviceProfile
	{
		// Token: 0x0600075A RID: 1882 RVA: 0x00034BA4 File Offset: 0x00032FA4
		public EightBitdoSNES30WindowsProfile()
		{
			base.Name = "8Bitdo SNES30 Controller";
			base.Meta = "8Bitdo SNES30 Controller on Windows";
			base.DeviceClass = InputDeviceClass.Controller;
			base.DeviceStyle = InputDeviceStyle.NintendoSNES;
			base.IncludePlatforms = new string[]
			{
				"Windows"
			};
			this.JoystickNames = new string[]
			{
				"SNES30 Joy    "
			};
			base.ButtonMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action2,
					Source = UnityInputDeviceProfile.Button(0)
				},
				new InputControlMapping
				{
					Handle = "B",
					Target = InputControlType.Action1,
					Source = UnityInputDeviceProfile.Button(1)
				},
				new InputControlMapping
				{
					Handle = "X",
					Target = InputControlType.Action4,
					Source = UnityInputDeviceProfile.Button(3)
				},
				new InputControlMapping
				{
					Handle = "Y",
					Target = InputControlType.Action3,
					Source = UnityInputDeviceProfile.Button(4)
				},
				new InputControlMapping
				{
					Handle = "L",
					Target = InputControlType.LeftTrigger,
					Source = UnityInputDeviceProfile.Button(6)
				},
				new InputControlMapping
				{
					Handle = "R",
					Target = InputControlType.RightTrigger,
					Source = UnityInputDeviceProfile.Button(7)
				},
				new InputControlMapping
				{
					Handle = "Select",
					Target = InputControlType.Select,
					Source = UnityInputDeviceProfile.Button(10)
				},
				new InputControlMapping
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = UnityInputDeviceProfile.Button(11)
				}
			};
			base.AnalogMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = UnityInputDeviceProfile.Analog(0),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = UnityInputDeviceProfile.Analog(0),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = UnityInputDeviceProfile.Analog(1),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = UnityInputDeviceProfile.Analog(1),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				}
			};
		}
	}
}
