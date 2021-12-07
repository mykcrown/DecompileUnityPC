using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000D8 RID: 216
	public class RazerOnzaControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005B2 RID: 1458 RVA: 0x0001D7F4 File Offset: 0x0001BBF4
		public RazerOnzaControllerMacProfile()
		{
			base.Name = "Razer Onza Controller";
			base.Meta = "Razer Onza Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(64769)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5769),
					ProductID = new ushort?(64769)
				}
			};
		}
	}
}
