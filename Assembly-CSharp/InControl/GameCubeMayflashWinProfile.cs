using System;

namespace InControl
{
	// Token: 0x02000162 RID: 354
	[AutoDiscover]
	public class GameCubeMayflashWinProfile : UnityInputDeviceProfile
	{
		// Token: 0x0600075E RID: 1886 RVA: 0x0003578C File Offset: 0x00033B8C
		public GameCubeMayflashWinProfile()
		{
			base.Name = "GameCube Controller";
			base.Meta = "GameCube Controller on Windows via MAYFLASH adapter";
			base.DeviceClass = InputDeviceClass.Controller;
			base.DeviceStyle = InputDeviceStyle.NintendoGameCube;
			base.IncludePlatforms = new string[]
			{
				"Windows"
			};
			this.JoystickNames = new string[]
			{
				"MAYFLASH GameCube Controller Adapter"
			};
			base.ButtonMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Handle = "X",
					Target = InputControlType.Action3,
					Source = UnityInputDeviceProfile.Button(0)
				},
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action1,
					Source = UnityInputDeviceProfile.Button(1)
				},
				new InputControlMapping
				{
					Handle = "B",
					Target = InputControlType.Action2,
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
					Handle = "Z",
					Target = InputControlType.RightBumper,
					Source = UnityInputDeviceProfile.Button(7)
				},
				new InputControlMapping
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = UnityInputDeviceProfile.Button(9)
				},
				new InputControlMapping
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = UnityInputDeviceProfile.Button(12)
				},
				new InputControlMapping
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = UnityInputDeviceProfile.Button(13)
				},
				new InputControlMapping
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = UnityInputDeviceProfile.Button(14)
				},
				new InputControlMapping
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = UnityInputDeviceProfile.Button(15)
				}
			};
			base.AnalogMappings = new InputControlMapping[]
			{
				UnityInputDeviceProfile.LeftStickLeftMapping(UnityInputDeviceProfile.Analog(0)),
				UnityInputDeviceProfile.LeftStickRightMapping(UnityInputDeviceProfile.Analog(0)),
				UnityInputDeviceProfile.LeftStickUpMapping(UnityInputDeviceProfile.Analog(1)),
				UnityInputDeviceProfile.LeftStickDownMapping(UnityInputDeviceProfile.Analog(1)),
				UnityInputDeviceProfile.RightStickLeftMapping(UnityInputDeviceProfile.Analog(5)),
				UnityInputDeviceProfile.RightStickRightMapping(UnityInputDeviceProfile.Analog(5)),
				UnityInputDeviceProfile.RightStickUpMapping(UnityInputDeviceProfile.Analog(2)),
				UnityInputDeviceProfile.RightStickDownMapping(UnityInputDeviceProfile.Analog(2)),
				UnityInputDeviceProfile.LeftTriggerMapping(UnityInputDeviceProfile.Analog(3)),
				UnityInputDeviceProfile.RightTriggerMapping(UnityInputDeviceProfile.Analog(4)),
				new InputControlMapping
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = UnityInputDeviceProfile.Analog(6),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = UnityInputDeviceProfile.Analog(6),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = UnityInputDeviceProfile.Analog(7),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = UnityInputDeviceProfile.Analog(7),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				}
			};
		}
	}
}
