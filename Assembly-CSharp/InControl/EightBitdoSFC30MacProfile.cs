﻿using System;

namespace InControl
{
	// Token: 0x0200015A RID: 346
	[AutoDiscover]
	public class EightBitdoSFC30MacProfile : UnityInputDeviceProfile
	{
		// Token: 0x06000756 RID: 1878 RVA: 0x00034094 File Offset: 0x00032494
		public EightBitdoSFC30MacProfile()
		{
			base.Name = "8Bitdo SFC30 Controller";
			base.Meta = "8Bitdo SFC30 Controller on Mac";
			base.DeviceClass = InputDeviceClass.Controller;
			base.DeviceStyle = InputDeviceStyle.NintendoSNES;
			base.IncludePlatforms = new string[]
			{
				"OS X"
			};
			this.JoystickNames = new string[]
			{
				"Unknown 8Bitdo SFC30 GamePad",
				"SFC30              SFC30 Joystick"
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
					Target = InputControlType.LeftBumper,
					Source = UnityInputDeviceProfile.Button(6)
				},
				new InputControlMapping
				{
					Handle = "R",
					Target = InputControlType.RightBumper,
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
