using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000CA RID: 202
	public class PDPVersusControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005A4 RID: 1444 RVA: 0x0001D18C File Offset: 0x0001B58C
		public PDPVersusControllerMacProfile()
		{
			base.Name = "PDP Versus Controller";
			base.Meta = "PDP Versus Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(63748)
				}
			};
		}
	}
}
