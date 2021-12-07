using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000098 RID: 152
	public class HORIRealArcadeProVXMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000572 RID: 1394 RVA: 0x0001BA8C File Offset: 0x00019E8C
		public HORIRealArcadeProVXMacProfile()
		{
			base.Name = "HORI Real Arcade Pro VX";
			base.Meta = "HORI Real Arcade Pro VX on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(27)
				}
			};
		}
	}
}
