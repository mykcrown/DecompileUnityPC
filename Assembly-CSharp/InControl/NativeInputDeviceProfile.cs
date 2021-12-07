using System;

namespace InControl
{
	// Token: 0x0200011F RID: 287
	public abstract class NativeInputDeviceProfile : InputDeviceProfile
	{
		// Token: 0x06000627 RID: 1575 RVA: 0x0001A3F8 File Offset: 0x000187F8
		public NativeInputDeviceProfile()
		{
			base.Sensitivity = 1f;
			base.LowerDeadZone = 0.2f;
			base.UpperDeadZone = 0.9f;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0001A421 File Offset: 0x00018821
		internal bool Matches(NativeDeviceInfo deviceInfo)
		{
			return this.Matches(deviceInfo, this.Matchers);
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0001A430 File Offset: 0x00018830
		internal bool LastResortMatches(NativeDeviceInfo deviceInfo)
		{
			return this.Matches(deviceInfo, this.LastResortMatchers);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0001A440 File Offset: 0x00018840
		private bool Matches(NativeDeviceInfo deviceInfo, NativeInputDeviceMatcher[] matchers)
		{
			if (this.Matchers != null)
			{
				int num = this.Matchers.Length;
				for (int i = 0; i < num; i++)
				{
					if (this.Matchers[i].Matches(deviceInfo))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001A489 File Offset: 0x00018889
		protected static InputControlSource Button(int index)
		{
			return new NativeButtonSource(index);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0001A491 File Offset: 0x00018891
		protected static InputControlSource Analog(int index)
		{
			return new NativeAnalogSource(index);
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001A49C File Offset: 0x0001889C
		protected static InputControlMapping LeftStickLeftMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Left",
				Target = InputControlType.LeftStickLeft,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001A4E4 File Offset: 0x000188E4
		protected static InputControlMapping LeftStickRightMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Right",
				Target = InputControlType.LeftStickRight,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0001A52C File Offset: 0x0001892C
		protected static InputControlMapping LeftStickUpMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Up",
				Target = InputControlType.LeftStickUp,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0001A574 File Offset: 0x00018974
		protected static InputControlMapping LeftStickDownMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Down",
				Target = InputControlType.LeftStickDown,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0001A5BC File Offset: 0x000189BC
		protected static InputControlMapping LeftStickUpMapping2(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Up",
				Target = InputControlType.LeftStickUp,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0001A604 File Offset: 0x00018A04
		protected static InputControlMapping LeftStickDownMapping2(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Down",
				Target = InputControlType.LeftStickDown,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0001A64C File Offset: 0x00018A4C
		protected static InputControlMapping RightStickLeftMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Left",
				Target = InputControlType.RightStickLeft,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0001A694 File Offset: 0x00018A94
		protected static InputControlMapping RightStickRightMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Right",
				Target = InputControlType.RightStickRight,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0001A6E0 File Offset: 0x00018AE0
		protected static InputControlMapping RightStickUpMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Up",
				Target = InputControlType.RightStickUp,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0001A728 File Offset: 0x00018B28
		protected static InputControlMapping RightStickDownMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Down",
				Target = InputControlType.RightStickDown,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0001A770 File Offset: 0x00018B70
		protected static InputControlMapping RightStickUpMapping2(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Up",
				Target = InputControlType.RightStickUp,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0001A7B8 File Offset: 0x00018BB8
		protected static InputControlMapping RightStickDownMapping2(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Down",
				Target = InputControlType.RightStickDown,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0001A800 File Offset: 0x00018C00
		protected static InputControlMapping LeftTriggerMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Trigger",
				Target = InputControlType.LeftTrigger,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.MinusOneToOne,
				TargetRange = InputRange.ZeroToOne,
				IgnoreInitialZeroValue = true
			};
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001A850 File Offset: 0x00018C50
		protected static InputControlMapping RightTriggerMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Trigger",
				Target = InputControlType.RightTrigger,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.MinusOneToOne,
				TargetRange = InputRange.ZeroToOne,
				IgnoreInitialZeroValue = true
			};
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0001A8A0 File Offset: 0x00018CA0
		protected static InputControlMapping DPadLeftMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Left",
				Target = InputControlType.DPadLeft,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0001A8EC File Offset: 0x00018CEC
		protected static InputControlMapping DPadRightMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Right",
				Target = InputControlType.DPadRight,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0001A938 File Offset: 0x00018D38
		protected static InputControlMapping DPadUpMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Up",
				Target = InputControlType.DPadUp,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0001A984 File Offset: 0x00018D84
		protected static InputControlMapping DPadDownMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Down",
				Target = InputControlType.DPadDown,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0001A9D0 File Offset: 0x00018DD0
		protected static InputControlMapping DPadUpMapping2(int analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Up",
				Target = InputControlType.DPadUp,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0001AA1C File Offset: 0x00018E1C
		protected static InputControlMapping DPadDownMapping2(int analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Down",
				Target = InputControlType.DPadDown,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x04000493 RID: 1171
		public NativeInputDeviceMatcher[] Matchers;

		// Token: 0x04000494 RID: 1172
		public NativeInputDeviceMatcher[] LastResortMatchers;
	}
}
