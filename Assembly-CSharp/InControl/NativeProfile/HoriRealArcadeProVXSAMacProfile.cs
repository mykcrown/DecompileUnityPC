using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000099 RID: 153
	public class HoriRealArcadeProVXSAMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000573 RID: 1395 RVA: 0x0001BAE8 File Offset: 0x00019EE8
		public HoriRealArcadeProVXSAMacProfile()
		{
			base.Name = "Hori Real Arcade Pro VX SA";
			base.Meta = "Hori Real Arcade Pro VX SA on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62722)
				}
			};
		}
	}
}
