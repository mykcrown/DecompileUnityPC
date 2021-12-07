using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000A3 RID: 163
	public class LogitechF510ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600057D RID: 1405 RVA: 0x0001BEA0 File Offset: 0x0001A2A0
		public LogitechF510ControllerMacProfile()
		{
			base.Name = "Logitech F510 Controller";
			base.Meta = "Logitech F510 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(49694)
				}
			};
		}
	}
}
