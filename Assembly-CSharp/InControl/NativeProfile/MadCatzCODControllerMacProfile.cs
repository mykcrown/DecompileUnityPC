using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000A8 RID: 168
	public class MadCatzCODControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000582 RID: 1410 RVA: 0x0001C080 File Offset: 0x0001A480
		public MadCatzCODControllerMacProfile()
		{
			base.Name = "Mad Catz COD Controller";
			base.Meta = "Mad Catz COD Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61477)
				}
			};
		}
	}
}
