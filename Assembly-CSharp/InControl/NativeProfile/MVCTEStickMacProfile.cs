using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000C3 RID: 195
	public class MVCTEStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600059D RID: 1437 RVA: 0x0001CD44 File Offset: 0x0001B144
		public MVCTEStickMacProfile()
		{
			base.Name = "MVC TE Stick";
			base.Meta = "MVC TE Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61497)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(46904)
				}
			};
		}
	}
}
