using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000A4 RID: 164
	public class LogitechF710ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600057E RID: 1406 RVA: 0x0001BF00 File Offset: 0x0001A300
		public LogitechF710ControllerMacProfile()
		{
			base.Name = "Logitech F710 Controller";
			base.Meta = "Logitech F710 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(49695)
				}
			};
		}
	}
}
