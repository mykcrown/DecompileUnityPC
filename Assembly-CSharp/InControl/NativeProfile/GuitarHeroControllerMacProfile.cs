using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000089 RID: 137
	public class GuitarHeroControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000563 RID: 1379 RVA: 0x0001B3E0 File Offset: 0x000197E0
		public GuitarHeroControllerMacProfile()
		{
			base.Name = "Guitar Hero Controller";
			base.Meta = "Guitar Hero Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5168),
					ProductID = new ushort?(18248)
				}
			};
		}
	}
}
