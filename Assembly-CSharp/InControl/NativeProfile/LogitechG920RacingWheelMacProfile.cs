using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000A5 RID: 165
	public class LogitechG920RacingWheelMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600057F RID: 1407 RVA: 0x0001BF60 File Offset: 0x0001A360
		public LogitechG920RacingWheelMacProfile()
		{
			base.Name = "Logitech G920 Racing Wheel";
			base.Meta = "Logitech G920 Racing Wheel on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(49761)
				}
			};
		}
	}
}
