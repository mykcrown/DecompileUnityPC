using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000DE RID: 222
	public class RockBandDrumsMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005B8 RID: 1464 RVA: 0x0001DAE0 File Offset: 0x0001BEE0
		public RockBandDrumsMacProfile()
		{
			base.Name = "Rock Band Drums";
			base.Meta = "Rock Band Drums on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(3)
				}
			};
		}
	}
}
