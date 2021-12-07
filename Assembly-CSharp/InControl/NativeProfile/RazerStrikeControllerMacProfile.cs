using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000DB RID: 219
	public class RazerStrikeControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005B5 RID: 1461 RVA: 0x0001D998 File Offset: 0x0001BD98
		public RazerStrikeControllerMacProfile()
		{
			base.Name = "Razer Strike Controller";
			base.Meta = "Razer Strike Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5769),
					ProductID = new ushort?(1)
				}
			};
		}
	}
}
