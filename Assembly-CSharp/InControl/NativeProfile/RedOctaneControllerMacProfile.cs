using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000DD RID: 221
	public class RedOctaneControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005B7 RID: 1463 RVA: 0x0001DA54 File Offset: 0x0001BE54
		public RedOctaneControllerMacProfile()
		{
			base.Name = "Red Octane Controller";
			base.Meta = "Red Octane Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5168),
					ProductID = new ushort?(63489)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5168),
					ProductID = new ushort?(672)
				}
			};
		}
	}
}
