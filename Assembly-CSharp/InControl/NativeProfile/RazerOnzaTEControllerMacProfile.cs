using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000D9 RID: 217
	public class RazerOnzaTEControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005B3 RID: 1459 RVA: 0x0001D880 File Offset: 0x0001BC80
		public RazerOnzaTEControllerMacProfile()
		{
			base.Name = "Razer Onza TE Controller";
			base.Meta = "Razer Onza TE Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(64768)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5769),
					ProductID = new ushort?(64768)
				}
			};
		}
	}
}
