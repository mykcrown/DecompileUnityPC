using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000E4 RID: 228
	public class ThrustmasterFerrari458RacingWheelMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005BE RID: 1470 RVA: 0x0001DD98 File Offset: 0x0001C198
		public ThrustmasterFerrari458RacingWheelMacProfile()
		{
			base.Name = "Thrustmaster Ferrari 458 Racing Wheel";
			base.Meta = "Thrustmaster Ferrari 458 Racing Wheel on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(23296)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(23299)
				}
			};
		}
	}
}
