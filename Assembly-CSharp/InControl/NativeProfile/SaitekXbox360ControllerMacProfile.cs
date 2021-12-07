using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000E3 RID: 227
	public class SaitekXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005BD RID: 1469 RVA: 0x0001DD38 File Offset: 0x0001C138
		public SaitekXbox360ControllerMacProfile()
		{
			base.Name = "Saitek Xbox 360 Controller";
			base.Meta = "Saitek Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(51970)
				}
			};
		}
	}
}
