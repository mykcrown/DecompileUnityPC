using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000095 RID: 149
	public class HoriRealArcadeProEXSEMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600056F RID: 1391 RVA: 0x0001B94C File Offset: 0x00019D4C
		public HoriRealArcadeProEXSEMacProfile()
		{
			base.Name = "Hori Real Arcade Pro EX SE";
			base.Meta = "Hori Real Arcade Pro EX SE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(22)
				}
			};
		}
	}
}
