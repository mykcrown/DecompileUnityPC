using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000DF RID: 223
	public class RockBandGuitarMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005B9 RID: 1465 RVA: 0x0001DB3C File Offset: 0x0001BF3C
		public RockBandGuitarMacProfile()
		{
			base.Name = "Rock Band Guitar";
			base.Meta = "Rock Band Guitar on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(2)
				}
			};
		}
	}
}
