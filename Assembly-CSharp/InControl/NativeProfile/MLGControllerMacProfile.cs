using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000C2 RID: 194
	public class MLGControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600059C RID: 1436 RVA: 0x0001CCE4 File Offset: 0x0001B0E4
		public MLGControllerMacProfile()
		{
			base.Name = "MLG Controller";
			base.Meta = "MLG Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61475)
				}
			};
		}
	}
}
