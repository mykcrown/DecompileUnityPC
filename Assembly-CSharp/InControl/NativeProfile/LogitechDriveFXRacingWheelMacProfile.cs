using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000A1 RID: 161
	public class LogitechDriveFXRacingWheelMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600057B RID: 1403 RVA: 0x0001BDE0 File Offset: 0x0001A1E0
		public LogitechDriveFXRacingWheelMacProfile()
		{
			base.Name = "Logitech DriveFX Racing Wheel";
			base.Meta = "Logitech DriveFX Racing Wheel on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(51875)
				}
			};
		}
	}
}
