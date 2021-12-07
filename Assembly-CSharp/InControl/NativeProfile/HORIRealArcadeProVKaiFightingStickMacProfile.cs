using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000097 RID: 151
	public class HORIRealArcadeProVKaiFightingStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000571 RID: 1393 RVA: 0x0001BA04 File Offset: 0x00019E04
		public HORIRealArcadeProVKaiFightingStickMacProfile()
		{
			base.Name = "HORI Real Arcade Pro V Kai Fighting Stick";
			base.Meta = "HORI Real Arcade Pro V Kai Fighting Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21774)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(120)
				}
			};
		}
	}
}
