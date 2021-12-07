using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000E0 RID: 224
	public class RockCandyControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005BA RID: 1466 RVA: 0x0001DB98 File Offset: 0x0001BF98
		public RockCandyControllerMacProfile()
		{
			base.Name = "Rock Candy Controller";
			base.Meta = "Rock Candy Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(287)
				}
			};
		}
	}
}
