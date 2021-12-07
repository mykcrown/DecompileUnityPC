using System;

namespace InControl
{
	// Token: 0x020001D0 RID: 464
	public abstract class UnityInputDeviceProfileBase : InputDeviceProfile
	{
		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600080D RID: 2061
		public abstract bool IsJoystick { get; }

		// Token: 0x0600080E RID: 2062
		public abstract bool HasJoystickName(string joystickName);

		// Token: 0x0600080F RID: 2063
		public abstract bool HasLastResortRegex(string joystickName);

		// Token: 0x06000810 RID: 2064
		public abstract bool HasJoystickOrRegexName(string joystickName);

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x0002F270 File Offset: 0x0002D670
		public bool IsNotJoystick
		{
			get
			{
				return !this.IsJoystick;
			}
		}
	}
}
