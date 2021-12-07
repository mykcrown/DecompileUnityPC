using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000084 RID: 132
	public class BatarangControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600055E RID: 1374 RVA: 0x0001B180 File Offset: 0x00019580
		public BatarangControllerMacProfile()
		{
			base.Name = "Batarang Controller";
			base.Meta = "Batarang Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5604),
					ProductID = new ushort?(16144)
				}
			};
		}
	}
}
