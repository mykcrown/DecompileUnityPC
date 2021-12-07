using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000A7 RID: 167
	public class MadCatzBrawlStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000581 RID: 1409 RVA: 0x0001C020 File Offset: 0x0001A420
		public MadCatzBrawlStickMacProfile()
		{
			base.Name = "Mad Catz Brawl Stick";
			base.Meta = "Mad Catz Brawl Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61465)
				}
			};
		}
	}
}
