using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000C9 RID: 201
	public class PDPTronControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005A3 RID: 1443 RVA: 0x0001D12C File Offset: 0x0001B52C
		public PDPTronControllerMacProfile()
		{
			base.Name = "PDP Tron Controller";
			base.Meta = "PDP Tron Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(63747)
				}
			};
		}
	}
}
