using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000091 RID: 145
	public class HoriPadEXTurboControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600056B RID: 1387 RVA: 0x0001B7D0 File Offset: 0x00019BD0
		public HoriPadEXTurboControllerMacProfile()
		{
			base.Name = "Hori Pad EX Turbo Controller";
			base.Meta = "Hori Pad EX Turbo Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(12)
				}
			};
		}
	}
}
