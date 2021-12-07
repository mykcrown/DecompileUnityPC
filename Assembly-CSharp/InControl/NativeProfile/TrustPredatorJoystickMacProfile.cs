using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000E6 RID: 230
	public class TrustPredatorJoystickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005C0 RID: 1472 RVA: 0x0001DEB0 File Offset: 0x0001C2B0
		public TrustPredatorJoystickMacProfile()
		{
			base.Name = "Trust Predator Joystick";
			base.Meta = "Trust Predator Joystick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(2064),
					ProductID = new ushort?(3)
				}
			};
		}
	}
}
