using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000094 RID: 148
	public class HoriRealArcadeProEXPremiumVLXMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600056E RID: 1390 RVA: 0x0001B8EC File Offset: 0x00019CEC
		public HoriRealArcadeProEXPremiumVLXMacProfile()
		{
			base.Name = "Hori Real Arcade Pro EX Premium VLX";
			base.Meta = "Hori Real Arcade Pro EX Premium VLX on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62726)
				}
			};
		}
	}
}
