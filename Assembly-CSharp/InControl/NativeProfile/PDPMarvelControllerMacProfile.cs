using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000C7 RID: 199
	public class PDPMarvelControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005A1 RID: 1441 RVA: 0x0001D06C File Offset: 0x0001B46C
		public PDPMarvelControllerMacProfile()
		{
			base.Name = "PDP Marvel Controller";
			base.Meta = "PDP Marvel Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(327)
				}
			};
		}
	}
}
