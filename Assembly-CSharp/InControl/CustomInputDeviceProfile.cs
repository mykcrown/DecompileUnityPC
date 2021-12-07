using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000141 RID: 321
	[Obsolete("Custom profiles are deprecated. Use the bindings API instead.", false)]
	public class CustomInputDeviceProfile : UnityInputDeviceProfileBase
	{
		// Token: 0x06000736 RID: 1846 RVA: 0x0002F27C File Offset: 0x0002D67C
		public CustomInputDeviceProfile()
		{
			base.Name = "Custom Device Profile";
			base.Meta = "Custom Device Profile";
			base.IncludePlatforms = new string[]
			{
				"Windows",
				"Mac",
				"Linux"
			};
			base.Sensitivity = 1f;
			base.LowerDeadZone = 0f;
			base.UpperDeadZone = 1f;
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x0002F2EA File Offset: 0x0002D6EA
		public sealed override bool IsJoystick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0002F2ED File Offset: 0x0002D6ED
		public sealed override bool HasJoystickName(string joystickName)
		{
			return false;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0002F2F0 File Offset: 0x0002D6F0
		public sealed override bool HasLastResortRegex(string joystickName)
		{
			return false;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0002F2F3 File Offset: 0x0002D6F3
		public sealed override bool HasJoystickOrRegexName(string joystickName)
		{
			return false;
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0002F2F6 File Offset: 0x0002D6F6
		protected static InputControlSource KeyCodeButton(params KeyCode[] keyCodeList)
		{
			return new UnityKeyCodeSource(keyCodeList);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0002F2FE File Offset: 0x0002D6FE
		protected static InputControlSource KeyCodeComboButton(params KeyCode[] keyCodeList)
		{
			return new UnityKeyCodeComboSource(keyCodeList);
		}

		// Token: 0x04000586 RID: 1414
		protected static InputControlSource MouseButton0 = new UnityMouseButtonSource(0);

		// Token: 0x04000587 RID: 1415
		protected static InputControlSource MouseButton1 = new UnityMouseButtonSource(1);

		// Token: 0x04000588 RID: 1416
		protected static InputControlSource MouseButton2 = new UnityMouseButtonSource(2);

		// Token: 0x04000589 RID: 1417
		protected static InputControlSource MouseXAxis = new UnityMouseAxisSource("x");

		// Token: 0x0400058A RID: 1418
		protected static InputControlSource MouseYAxis = new UnityMouseAxisSource("y");

		// Token: 0x0400058B RID: 1419
		protected static InputControlSource MouseScrollWheel = new UnityMouseAxisSource("z");
	}
}
