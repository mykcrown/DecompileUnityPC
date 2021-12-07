using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000087 RID: 135
	public class EASportsControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000561 RID: 1377 RVA: 0x0001B2A0 File Offset: 0x000196A0
		public EASportsControllerMacProfile()
		{
			base.Name = "EA Sports Controller";
			base.Meta = "EA Sports Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(305)
				}
			};
		}
	}
}
