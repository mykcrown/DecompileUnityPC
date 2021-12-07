using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000DC RID: 220
	public class RazerWildcatControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005B6 RID: 1462 RVA: 0x0001D9F4 File Offset: 0x0001BDF4
		public RazerWildcatControllerMacProfile()
		{
			base.Name = "Razer Wildcat Controller";
			base.Meta = "Razer Wildcat Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5426),
					ProductID = new ushort?(2563)
				}
			};
		}
	}
}
