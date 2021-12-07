﻿using System;

namespace InControl
{
	// Token: 0x020001C7 RID: 455
	[AutoDiscover]
	public class XiamoiWinProfile : UnityInputDeviceProfile
	{
		// Token: 0x060007C3 RID: 1987 RVA: 0x0004937C File Offset: 0x0004777C
		public XiamoiWinProfile()
		{
			base.Name = "Xiamoi Bluetooth Controller";
			base.Meta = "Xiamoi Bluetooth Controller on Windows";
			base.DeviceClass = InputDeviceClass.Controller;
			base.DeviceStyle = InputDeviceStyle.Xbox360;
			base.IncludePlatforms = new string[]
			{
				"Windows"
			};
			this.JoystickNames = new string[]
			{
				"XiaoMi Bluetooth Wireless GameController"
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
					Handle = "B",
					Target = InputControlType.Action2,
					Source = UnityInputDeviceProfile.Button1
				},
				new InputControlMapping
				{
					Handle = "X",
					Target = InputControlType.Action3,
					Source = UnityInputDeviceProfile.Button3
				},
				new InputControlMapping
				{
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = UnityInputDeviceProfile.Button4
				},
				new InputControlMapping
				{
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = UnityInputDeviceProfile.Button6
				},
				new InputControlMapping
				{
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = UnityInputDeviceProfile.Button7
				},
				new InputControlMapping
				{
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = UnityInputDeviceProfile.Button13
				},
				new InputControlMapping
				{
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = UnityInputDeviceProfile.Button14
				},
				new InputControlMapping
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = UnityInputDeviceProfile.Button11
				},
				new InputControlMapping
				{
					Handle = "Back",
					Target = InputControlType.Back,
					Source = UnityInputDeviceProfile.Button10
				},
				new InputControlMapping
				{
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = UnityInputDeviceProfile.Button8
				},
				new InputControlMapping
				{
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = UnityInputDeviceProfile.Button9
				}
			};
			base.AnalogMappings = new InputControlMapping[]
			{
				UnityInputDeviceProfile.LeftStickLeftMapping(UnityInputDeviceProfile.Analog0),
				UnityInputDeviceProfile.LeftStickRightMapping(UnityInputDeviceProfile.Analog0),
				UnityInputDeviceProfile.LeftStickUpMapping(UnityInputDeviceProfile.Analog1),
				UnityInputDeviceProfile.LeftStickDownMapping(UnityInputDeviceProfile.Analog1),
				UnityInputDeviceProfile.RightStickLeftMapping(UnityInputDeviceProfile.Analog2),
				UnityInputDeviceProfile.RightStickRightMapping(UnityInputDeviceProfile.Analog2),
				UnityInputDeviceProfile.RightStickUpMapping(UnityInputDeviceProfile.Analog5),
				UnityInputDeviceProfile.RightStickDownMapping(UnityInputDeviceProfile.Analog5),
				UnityInputDeviceProfile.DPadLeftMapping(UnityInputDeviceProfile.Analog6),
				UnityInputDeviceProfile.DPadRightMapping(UnityInputDeviceProfile.Analog6),
				UnityInputDeviceProfile.DPadUpMapping2(UnityInputDeviceProfile.Analog7),
				UnityInputDeviceProfile.DPadDownMapping2(UnityInputDeviceProfile.Analog7)
			};
		}
	}
}
