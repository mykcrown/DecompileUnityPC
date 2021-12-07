using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200068D RID: 1677
	[AutoDiscover]
	public class GameCubeWinMayflashProfile : NativeInputDeviceProfile
	{
		// Token: 0x06002981 RID: 10625 RVA: 0x000DD130 File Offset: 0x000DB530
		public GameCubeWinMayflashProfile()
		{
			base.Name = "GameCube Controller (Mayflash, Win)";
			base.Meta = "GameCube Controller (Mayflash, Win)";
			base.IncludePlatforms = new string[]
			{
				"Windows"
			};
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(121),
					ProductID = new ushort?(6215)
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
					Handle = "X",
					Target = InputControlType.Action3,
					Source = NativeInputDeviceProfile.Button(0)
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
					Handle = "Start",
					Target = InputControlType.Start,
					Source = NativeInputDeviceProfile.Button(9)
				},
				new InputControlMapping
				{
					Handle = "Z",
					Target = InputControlType.RightBumper,
					Source = NativeInputDeviceProfile.Button(7)
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
					Source = NativeInputDeviceProfile.Button(14)
				},
				new InputControlMapping
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = NativeInputDeviceProfile.Button(15)
				},
				new InputControlMapping
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = NativeInputDeviceProfile.Button(13)
				}
			};
			base.AnalogMappings = new InputControlMapping[]
			{
				NativeInputDeviceProfile.LeftStickLeftMapping(0),
				NativeInputDeviceProfile.LeftStickRightMapping(0),
				NativeInputDeviceProfile.LeftStickUpMapping(1),
				NativeInputDeviceProfile.LeftStickDownMapping(1),
				NativeInputDeviceProfile.RightStickLeftMapping(5),
				NativeInputDeviceProfile.RightStickRightMapping(5),
				NativeInputDeviceProfile.RightStickUpMapping(2),
				NativeInputDeviceProfile.RightStickDownMapping(2),
				NativeInputDeviceProfile.LeftTriggerMapping(3),
				NativeInputDeviceProfile.RightTriggerMapping(4)
			};
		}
	}
}
