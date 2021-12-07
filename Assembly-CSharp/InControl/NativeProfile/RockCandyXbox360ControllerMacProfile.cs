using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000E1 RID: 225
	public class RockCandyXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005BB RID: 1467 RVA: 0x0001DBF8 File Offset: 0x0001BFF8
		public RockCandyXbox360ControllerMacProfile()
		{
			base.Name = "Rock Candy Xbox 360 Controller";
			base.Meta = "Rock Candy Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(543)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(64254)
				}
			};
		}
	}
}
