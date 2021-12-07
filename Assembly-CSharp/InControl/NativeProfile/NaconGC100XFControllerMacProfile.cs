using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000C4 RID: 196
	public class NaconGC100XFControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600059E RID: 1438 RVA: 0x0001CDD0 File Offset: 0x0001B1D0
		public NaconGC100XFControllerMacProfile()
		{
			base.Name = "Nacon GC-100XF Controller";
			base.Meta = "Nacon GC-100XF Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(4553),
					ProductID = new ushort?(22000)
				}
			};
		}
	}
}
