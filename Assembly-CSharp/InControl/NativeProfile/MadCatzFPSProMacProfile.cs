using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000AE RID: 174
	public class MadCatzFPSProMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000588 RID: 1416 RVA: 0x0001C340 File Offset: 0x0001A740
		public MadCatzFPSProMacProfile()
		{
			base.Name = "Mad Catz FPS Pro";
			base.Meta = "Mad Catz FPS Pro on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61479)
				}
			};
		}
	}
}
