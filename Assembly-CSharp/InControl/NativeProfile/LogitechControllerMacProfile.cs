using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000A0 RID: 160
	public class LogitechControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600057A RID: 1402 RVA: 0x0001BD80 File Offset: 0x0001A180
		public LogitechControllerMacProfile()
		{
			base.Name = "Logitech Controller";
			base.Meta = "Logitech Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(62209)
				}
			};
		}
	}
}
