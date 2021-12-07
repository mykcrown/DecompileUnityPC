using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000CB RID: 203
	public class PDPXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005A5 RID: 1445 RVA: 0x0001D1EC File Offset: 0x0001B5EC
		public PDPXbox360ControllerMacProfile()
		{
			base.Name = "PDP Xbox 360 Controller";
			base.Meta = "PDP Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(1281)
				}
			};
		}
	}
}
