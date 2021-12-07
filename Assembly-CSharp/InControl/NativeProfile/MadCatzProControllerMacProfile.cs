using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000B4 RID: 180
	public class MadCatzProControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600058E RID: 1422 RVA: 0x0001C580 File Offset: 0x0001A980
		public MadCatzProControllerMacProfile()
		{
			base.Name = "Mad Catz Pro Controller";
			base.Meta = "Mad Catz Pro Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18214)
				}
			};
		}
	}
}
