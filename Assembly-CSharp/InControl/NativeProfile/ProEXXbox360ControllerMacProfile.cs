using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000D4 RID: 212
	public class ProEXXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005AE RID: 1454 RVA: 0x0001D648 File Offset: 0x0001BA48
		public ProEXXbox360ControllerMacProfile()
		{
			base.Name = "Pro EX Xbox 360 Controller";
			base.Meta = "Pro EX Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21258)
				}
			};
		}
	}
}
