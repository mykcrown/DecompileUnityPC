using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000093 RID: 147
	public class HoriRealArcadeProEXMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600056D RID: 1389 RVA: 0x0001B88C File Offset: 0x00019C8C
		public HoriRealArcadeProEXMacProfile()
		{
			base.Name = "Hori Real Arcade Pro EX";
			base.Meta = "Hori Real Arcade Pro EX on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62724)
				}
			};
		}
	}
}
