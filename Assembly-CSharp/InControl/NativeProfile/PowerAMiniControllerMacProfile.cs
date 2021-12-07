using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000D0 RID: 208
	public class PowerAMiniControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005AA RID: 1450 RVA: 0x0001D474 File Offset: 0x0001B874
		public PowerAMiniControllerMacProfile()
		{
			base.Name = "PowerA Mini Controller";
			base.Meta = "PowerA Mini Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21530)
				}
			};
		}
	}
}
