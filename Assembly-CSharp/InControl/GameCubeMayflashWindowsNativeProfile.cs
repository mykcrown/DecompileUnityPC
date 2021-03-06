using System;

namespace InControl
{
	// Token: 0x02000100 RID: 256
	[AutoDiscover]
	public class GameCubeMayflashWindowsNativeProfile : NativeInputDeviceProfile
	{
		// Token: 0x060005DA RID: 1498 RVA: 0x000225DC File Offset: 0x000209DC
		public GameCubeMayflashWindowsNativeProfile()
		{
			base.Name = "GameCube Controller";
			base.Meta = "GameCube Controller on Windows via MAYFLASH adapter";
			base.DeviceClass = InputDeviceClass.Controller;
			base.DeviceStyle = InputDeviceStyle.NintendoGameCube;
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(121),
					ProductID = new ushort?(6211)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(121),
					ProductID = new ushort?(6212)
				}
			};
			base.ButtonMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Handle = "X",
					Target = InputControlType.Action3,
					Source = NativeInputDeviceProfile.Button(0)
				},
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
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = NativeInputDeviceProfile.Button(3)
				},
				new InputControlMapping
				{
					Handle = "Left Trigger Button",
					Target = InputControlType.Action5,
					Source = NativeInputDeviceProfile.Button(4)
				},
				new InputControlMapping
				{
					Handle = "Right Trigger Button",
					Target = InputControlType.Action6,
					Source = NativeInputDeviceProfile.Button(5)
				},
				new InputControlMapping
				{
					Handle = "Z",
					Target = InputControlType.RightBumper,
					Source = NativeInputDeviceProfile.Button(7)
				},
				new InputControlMapping
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = NativeInputDeviceProfile.Button(9)
				},
				new InputControlMapping
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = NativeInputDeviceProfile.Button(12)
				},
				new InputControlMapping
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = NativeInputDeviceProfile.Button(13)
				},
				new InputControlMapping
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = NativeInputDeviceProfile.Button(14)
				},
				new InputControlMapping
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = NativeInputDeviceProfile.Button(15)
				}
			};
			base.AnalogMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Handle = "C Left",
					Target = InputControlType.RightStickLeft,
					Source = NativeInputDeviceProfile.Analog(0),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "C Right",
					Target = InputControlType.RightStickRight,
					Source = NativeInputDeviceProfile.Analog(0),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "C Up",
					Target = InputControlType.RightStickUp,
					Source = NativeInputDeviceProfile.Analog(1),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "C Down",
					Target = InputControlType.RightStickDown,
					Source = NativeInputDeviceProfile.Analog(1),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "Left Stick Up",
					Target = InputControlType.LeftStickUp,
					Source = NativeInputDeviceProfile.Analog(2),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "Left Stick Down",
					Target = InputControlType.LeftStickDown,
					Source = NativeInputDeviceProfile.Analog(2),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "Left Stick Left",
					Target = InputControlType.LeftStickLeft,
					Source = NativeInputDeviceProfile.Analog(3),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "Left Stick Right",
					Target = InputControlType.LeftStickRight,
					Source = NativeInputDeviceProfile.Analog(3),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = NativeInputDeviceProfile.Analog(4),
					SourceRange = InputRange.MinusOneToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = NativeInputDeviceProfile.Analog(5),
					SourceRange = InputRange.MinusOneToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = NativeInputDeviceProfile.Analog(6),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = NativeInputDeviceProfile.Analog(6),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = NativeInputDeviceProfile.Analog(7),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = NativeInputDeviceProfile.Analog(7),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				}
			};
		}
	}
}
