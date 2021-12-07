using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace InControl
{
	// Token: 0x020001CF RID: 463
	public class UnityInputDeviceProfile : UnityInputDeviceProfileBase
	{
		// Token: 0x060007EE RID: 2030 RVA: 0x0002F363 File Offset: 0x0002D763
		public UnityInputDeviceProfile()
		{
			base.Sensitivity = 1f;
			base.LowerDeadZone = 0.2f;
			base.UpperDeadZone = 0.9f;
			this.MinUnityVersion = VersionInfo.Min;
			this.MaxUnityVersion = VersionInfo.Max;
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0002F3A2 File Offset: 0x0002D7A2
		// (set) Token: 0x060007F0 RID: 2032 RVA: 0x0002F3AA File Offset: 0x0002D7AA
		[SerializeField]
		public VersionInfo MinUnityVersion { get; protected set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0002F3B3 File Offset: 0x0002D7B3
		// (set) Token: 0x060007F2 RID: 2034 RVA: 0x0002F3BB File Offset: 0x0002D7BB
		[SerializeField]
		public VersionInfo MaxUnityVersion { get; protected set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0002F3C4 File Offset: 0x0002D7C4
		public override bool IsJoystick
		{
			get
			{
				return this.LastResortRegex != null || (this.JoystickNames != null && this.JoystickNames.Length > 0) || (this.JoystickRegex != null && this.JoystickRegex.Length > 0);
			}
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0002F414 File Offset: 0x0002D814
		public override bool HasJoystickName(string joystickName)
		{
			if (base.IsNotJoystick)
			{
				return false;
			}
			if (this.JoystickNames != null && this.JoystickNames.Contains(joystickName, StringComparer.OrdinalIgnoreCase))
			{
				return true;
			}
			if (this.JoystickRegex != null)
			{
				for (int i = 0; i < this.JoystickRegex.Length; i++)
				{
					if (Regex.IsMatch(joystickName, this.JoystickRegex[i], RegexOptions.IgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x0002F48C File Offset: 0x0002D88C
		public override bool HasLastResortRegex(string joystickName)
		{
			return !base.IsNotJoystick && this.LastResortRegex != null && Regex.IsMatch(joystickName, this.LastResortRegex, RegexOptions.IgnoreCase);
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x0002F4B5 File Offset: 0x0002D8B5
		public override bool HasJoystickOrRegexName(string joystickName)
		{
			return this.HasJoystickName(joystickName) || this.HasLastResortRegex(joystickName);
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x0002F4CD File Offset: 0x0002D8CD
		public override bool IsSupportedOnThisPlatform
		{
			get
			{
				return this.IsSupportedOnThisVersionOfUnity && base.IsSupportedOnThisPlatform;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x0002F4E4 File Offset: 0x0002D8E4
		private bool IsSupportedOnThisVersionOfUnity
		{
			get
			{
				VersionInfo a = VersionInfo.UnityVersion();
				return a >= this.MinUnityVersion && a <= this.MaxUnityVersion;
			}
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0002F517 File Offset: 0x0002D917
		protected static InputControlSource Button(int index)
		{
			return new UnityButtonSource(index);
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0002F51F File Offset: 0x0002D91F
		protected static InputControlSource Analog(int index)
		{
			return new UnityAnalogSource(index);
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0002F528 File Offset: 0x0002D928
		protected static InputControlMapping LeftStickLeftMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Left",
				Target = InputControlType.LeftStickLeft,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne,
				LowerDeadZone = 0.2f,
				UpperDeadZone = 0.7f
			};
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0002F584 File Offset: 0x0002D984
		protected static InputControlMapping LeftStickRightMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Right",
				Target = InputControlType.LeftStickRight,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne,
				LowerDeadZone = 0.2f,
				UpperDeadZone = 0.7f
			};
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0002F5E0 File Offset: 0x0002D9E0
		protected static InputControlMapping LeftStickUpMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Up",
				Target = InputControlType.LeftStickUp,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne,
				LowerDeadZone = 0.2f,
				UpperDeadZone = 0.7f
			};
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0002F63C File Offset: 0x0002DA3C
		protected static InputControlMapping LeftStickDownMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Down",
				Target = InputControlType.LeftStickDown,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne,
				LowerDeadZone = 0.2f,
				UpperDeadZone = 0.7f
			};
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0002F698 File Offset: 0x0002DA98
		protected static InputControlMapping RightStickLeftMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Left",
				Target = InputControlType.RightStickLeft,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne,
				LowerDeadZone = 0.1f,
				UpperDeadZone = 0.7f
			};
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0002F6F4 File Offset: 0x0002DAF4
		protected static InputControlMapping RightStickRightMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Right",
				Target = InputControlType.RightStickRight,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne,
				LowerDeadZone = 0.1f,
				UpperDeadZone = 0.7f
			};
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0002F750 File Offset: 0x0002DB50
		protected static InputControlMapping RightStickUpMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Up",
				Target = InputControlType.RightStickUp,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne,
				LowerDeadZone = 0.1f,
				UpperDeadZone = 0.7f
			};
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0002F7AC File Offset: 0x0002DBAC
		protected static InputControlMapping RightStickDownMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Down",
				Target = InputControlType.RightStickDown,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne,
				LowerDeadZone = 0.1f,
				UpperDeadZone = 0.7f
			};
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0002F808 File Offset: 0x0002DC08
		protected static InputControlMapping LeftTriggerMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Trigger",
				Target = InputControlType.LeftTrigger,
				Source = analog,
				SourceRange = InputRange.MinusOneToOne,
				TargetRange = InputRange.ZeroToOne,
				LowerDeadZone = 0.4f,
				UpperDeadZone = 0.9f,
				IgnoreInitialZeroValue = true
			};
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0002F86C File Offset: 0x0002DC6C
		protected static InputControlMapping RightTriggerMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Trigger",
				Target = InputControlType.RightTrigger,
				Source = analog,
				SourceRange = InputRange.MinusOneToOne,
				TargetRange = InputRange.ZeroToOne,
				LowerDeadZone = 0.4f,
				UpperDeadZone = 0.9f,
				IgnoreInitialZeroValue = true
			};
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0002F8D0 File Offset: 0x0002DCD0
		protected static InputControlMapping DPadLeftMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Left",
				Target = InputControlType.DPadLeft,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0002F914 File Offset: 0x0002DD14
		protected static InputControlMapping DPadRightMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Right",
				Target = InputControlType.DPadRight,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0002F958 File Offset: 0x0002DD58
		protected static InputControlMapping DPadUpMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Up",
				Target = InputControlType.DPadUp,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0002F99C File Offset: 0x0002DD9C
		protected static InputControlMapping DPadDownMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Down",
				Target = InputControlType.DPadDown,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0002F9E0 File Offset: 0x0002DDE0
		protected static InputControlMapping DPadUpMapping2(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Up",
				Target = InputControlType.DPadUp,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0002FA24 File Offset: 0x0002DE24
		protected static InputControlMapping DPadDownMapping2(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Down",
				Target = InputControlType.DPadDown,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x0400059C RID: 1436
		private const float LowerDeadZone_RightStick = 0.1f;

		// Token: 0x0400059D RID: 1437
		private const float UpperDeadZone_RightStick = 0.7f;

		// Token: 0x0400059E RID: 1438
		private const float LowerDeadZone_LeftStick = 0.2f;

		// Token: 0x0400059F RID: 1439
		private const float UpperDeadZone_LeftStick = 0.7f;

		// Token: 0x040005A0 RID: 1440
		private const float LowerDeadZone_Triggers = 0.4f;

		// Token: 0x040005A1 RID: 1441
		private const float UpperDeadZone_Triggers = 0.9f;

		// Token: 0x040005A2 RID: 1442
		[SerializeField]
		protected string[] JoystickNames;

		// Token: 0x040005A3 RID: 1443
		[SerializeField]
		protected string[] JoystickRegex;

		// Token: 0x040005A4 RID: 1444
		[SerializeField]
		protected string LastResortRegex;

		// Token: 0x040005A7 RID: 1447
		protected static InputControlSource Button0 = UnityInputDeviceProfile.Button(0);

		// Token: 0x040005A8 RID: 1448
		protected static InputControlSource Button1 = UnityInputDeviceProfile.Button(1);

		// Token: 0x040005A9 RID: 1449
		protected static InputControlSource Button2 = UnityInputDeviceProfile.Button(2);

		// Token: 0x040005AA RID: 1450
		protected static InputControlSource Button3 = UnityInputDeviceProfile.Button(3);

		// Token: 0x040005AB RID: 1451
		protected static InputControlSource Button4 = UnityInputDeviceProfile.Button(4);

		// Token: 0x040005AC RID: 1452
		protected static InputControlSource Button5 = UnityInputDeviceProfile.Button(5);

		// Token: 0x040005AD RID: 1453
		protected static InputControlSource Button6 = UnityInputDeviceProfile.Button(6);

		// Token: 0x040005AE RID: 1454
		protected static InputControlSource Button7 = UnityInputDeviceProfile.Button(7);

		// Token: 0x040005AF RID: 1455
		protected static InputControlSource Button8 = UnityInputDeviceProfile.Button(8);

		// Token: 0x040005B0 RID: 1456
		protected static InputControlSource Button9 = UnityInputDeviceProfile.Button(9);

		// Token: 0x040005B1 RID: 1457
		protected static InputControlSource Button10 = UnityInputDeviceProfile.Button(10);

		// Token: 0x040005B2 RID: 1458
		protected static InputControlSource Button11 = UnityInputDeviceProfile.Button(11);

		// Token: 0x040005B3 RID: 1459
		protected static InputControlSource Button12 = UnityInputDeviceProfile.Button(12);

		// Token: 0x040005B4 RID: 1460
		protected static InputControlSource Button13 = UnityInputDeviceProfile.Button(13);

		// Token: 0x040005B5 RID: 1461
		protected static InputControlSource Button14 = UnityInputDeviceProfile.Button(14);

		// Token: 0x040005B6 RID: 1462
		protected static InputControlSource Button15 = UnityInputDeviceProfile.Button(15);

		// Token: 0x040005B7 RID: 1463
		protected static InputControlSource Button16 = UnityInputDeviceProfile.Button(16);

		// Token: 0x040005B8 RID: 1464
		protected static InputControlSource Button17 = UnityInputDeviceProfile.Button(17);

		// Token: 0x040005B9 RID: 1465
		protected static InputControlSource Button18 = UnityInputDeviceProfile.Button(18);

		// Token: 0x040005BA RID: 1466
		protected static InputControlSource Button19 = UnityInputDeviceProfile.Button(19);

		// Token: 0x040005BB RID: 1467
		protected static InputControlSource Analog0 = UnityInputDeviceProfile.Analog(0);

		// Token: 0x040005BC RID: 1468
		protected static InputControlSource Analog1 = UnityInputDeviceProfile.Analog(1);

		// Token: 0x040005BD RID: 1469
		protected static InputControlSource Analog2 = UnityInputDeviceProfile.Analog(2);

		// Token: 0x040005BE RID: 1470
		protected static InputControlSource Analog3 = UnityInputDeviceProfile.Analog(3);

		// Token: 0x040005BF RID: 1471
		protected static InputControlSource Analog4 = UnityInputDeviceProfile.Analog(4);

		// Token: 0x040005C0 RID: 1472
		protected static InputControlSource Analog5 = UnityInputDeviceProfile.Analog(5);

		// Token: 0x040005C1 RID: 1473
		protected static InputControlSource Analog6 = UnityInputDeviceProfile.Analog(6);

		// Token: 0x040005C2 RID: 1474
		protected static InputControlSource Analog7 = UnityInputDeviceProfile.Analog(7);

		// Token: 0x040005C3 RID: 1475
		protected static InputControlSource Analog8 = UnityInputDeviceProfile.Analog(8);

		// Token: 0x040005C4 RID: 1476
		protected static InputControlSource Analog9 = UnityInputDeviceProfile.Analog(9);

		// Token: 0x040005C5 RID: 1477
		protected static InputControlSource Analog10 = UnityInputDeviceProfile.Analog(10);

		// Token: 0x040005C6 RID: 1478
		protected static InputControlSource Analog11 = UnityInputDeviceProfile.Analog(11);

		// Token: 0x040005C7 RID: 1479
		protected static InputControlSource Analog12 = UnityInputDeviceProfile.Analog(12);

		// Token: 0x040005C8 RID: 1480
		protected static InputControlSource Analog13 = UnityInputDeviceProfile.Analog(13);

		// Token: 0x040005C9 RID: 1481
		protected static InputControlSource Analog14 = UnityInputDeviceProfile.Analog(14);

		// Token: 0x040005CA RID: 1482
		protected static InputControlSource Analog15 = UnityInputDeviceProfile.Analog(15);

		// Token: 0x040005CB RID: 1483
		protected static InputControlSource Analog16 = UnityInputDeviceProfile.Analog(16);

		// Token: 0x040005CC RID: 1484
		protected static InputControlSource Analog17 = UnityInputDeviceProfile.Analog(17);

		// Token: 0x040005CD RID: 1485
		protected static InputControlSource Analog18 = UnityInputDeviceProfile.Analog(18);

		// Token: 0x040005CE RID: 1486
		protected static InputControlSource Analog19 = UnityInputDeviceProfile.Analog(19);

		// Token: 0x040005CF RID: 1487
		protected static InputControlSource MenuKey = new UnityKeyCodeSource(new KeyCode[]
		{
			KeyCode.Menu
		});

		// Token: 0x040005D0 RID: 1488
		protected static InputControlSource EscapeKey = new UnityKeyCodeSource(new KeyCode[]
		{
			KeyCode.Escape
		});

		// Token: 0x040005D1 RID: 1489
		protected static InputControlSource MouseButton0 = new UnityMouseButtonSource(0);

		// Token: 0x040005D2 RID: 1490
		protected static InputControlSource MouseButton1 = new UnityMouseButtonSource(1);

		// Token: 0x040005D3 RID: 1491
		protected static InputControlSource MouseButton2 = new UnityMouseButtonSource(2);

		// Token: 0x040005D4 RID: 1492
		protected static InputControlSource MouseXAxis = new UnityMouseAxisSource("x");

		// Token: 0x040005D5 RID: 1493
		protected static InputControlSource MouseYAxis = new UnityMouseAxisSource("y");

		// Token: 0x040005D6 RID: 1494
		protected static InputControlSource MouseScrollWheel = new UnityMouseAxisSource("z");
	}
}
