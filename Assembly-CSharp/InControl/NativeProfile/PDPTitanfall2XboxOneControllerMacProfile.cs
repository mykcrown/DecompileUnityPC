using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000C8 RID: 200
	public class PDPTitanfall2XboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x060005A2 RID: 1442 RVA: 0x0001D0CC File Offset: 0x0001B4CC
		public PDPTitanfall2XboxOneControllerMacProfile()
		{
			base.Name = "PDP Titanfall 2 Xbox One Controller";
			base.Meta = "PDP Titanfall 2 Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(357)
				}
			};
		}
	}
}
