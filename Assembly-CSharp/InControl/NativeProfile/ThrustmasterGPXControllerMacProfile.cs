using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000E5 RID: 229
	public class ThrustmasterGPXControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005BF RID: 1471 RVA: 0x0001DE24 File Offset: 0x0001C224
		public ThrustmasterGPXControllerMacProfile()
		{
			base.Name = "Thrustmaster GPX Controller";
			base.Meta = "Thrustmaster GPX Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1103),
					ProductID = new ushort?(45862)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(23298)
				}
			};
		}
	}
}
