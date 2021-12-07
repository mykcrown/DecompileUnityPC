// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace InControl
{
	[AutoDiscover]
	public class EightBitdoSNES30WindowsNativeProfile : NativeInputDeviceProfile
	{
		public EightBitdoSNES30WindowsNativeProfile()
		{
			base.Name = "8Bitdo SNES30 Controller";
			base.Meta = "8Bitdo SNES30 Controller on Windows";
			base.DeviceClass = InputDeviceClass.Controller;
			base.DeviceStyle = InputDeviceStyle.NintendoSNES;
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(11720),
					ProductID = new ushort?(43808)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(11720),
					ProductID = new ushort?(10304)
				}
			};
			base.ButtonMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action2,
					Source = NativeInputDeviceProfile.Button(0)
				},
				new InputControlMapping
				{
					Handle = "B",
					Target = InputControlType.Action1,
					Source = NativeInputDeviceProfile.Button(1)
				},
				new InputControlMapping
				{
					Handle = "X",
					Target = InputControlType.Action4,
					Source = NativeInputDeviceProfile.Button(3)
				},
				new InputControlMapping
				{
					Handle = "Y",
					Target = InputControlType.Action3,
					Source = NativeInputDeviceProfile.Button(4)
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
					Handle = "Select",
					Target = InputControlType.Select,
					Source = NativeInputDeviceProfile.Button(10)
				},
				new InputControlMapping
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = NativeInputDeviceProfile.Button(11)
				}
			};
			base.AnalogMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = NativeInputDeviceProfile.Analog(2),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = NativeInputDeviceProfile.Analog(2),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = NativeInputDeviceProfile.Analog(3),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = NativeInputDeviceProfile.Analog(3),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				}
			};
		}
	}
}