using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200008B RID: 139
	public class HoriControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000565 RID: 1381 RVA: 0x0001B4A0 File Offset: 0x000198A0
		public HoriControllerMacProfile()
		{
			base.Name = "Hori Controller";
			base.Meta = "Hori Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(21760)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(654)
				}
			};
		}
	}
}
