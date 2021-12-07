using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000A2 RID: 162
	public class LogitechF310ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600057C RID: 1404 RVA: 0x0001BE40 File Offset: 0x0001A240
		public LogitechF310ControllerMacProfile()
		{
			base.Name = "Logitech F310 Controller";
			base.Meta = "Logitech F310 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(49693)
				}
			};
		}
	}
}
