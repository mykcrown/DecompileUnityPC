using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000B9 RID: 185
	public class MadCatzSoulCaliberFightStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000593 RID: 1427 RVA: 0x0001C760 File Offset: 0x0001AB60
		public MadCatzSoulCaliberFightStickMacProfile()
		{
			base.Name = "Mad Catz Soul Caliber Fight Stick";
			base.Meta = "Mad Catz Soul Caliber Fight Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61503)
				}
			};
		}
	}
}
