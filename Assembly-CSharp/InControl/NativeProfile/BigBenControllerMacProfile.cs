using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000086 RID: 134
	public class BigBenControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000560 RID: 1376 RVA: 0x0001B240 File Offset: 0x00019640
		public BigBenControllerMacProfile()
		{
			base.Name = "Big Ben Controller";
			base.Meta = "Big Ben Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5227),
					ProductID = new ushort?(1537)
				}
			};
		}
	}
}
