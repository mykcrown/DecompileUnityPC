using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000085 RID: 133
	public class BETAOPControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600055F RID: 1375 RVA: 0x0001B1E0 File Offset: 0x000195E0
		public BETAOPControllerMacProfile()
		{
			base.Name = "BETAOP Controller";
			base.Meta = "BETAOP Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(4544),
					ProductID = new ushort?(21766)
				}
			};
		}
	}
}
