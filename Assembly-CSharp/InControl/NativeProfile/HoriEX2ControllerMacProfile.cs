using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200008D RID: 141
	public class HoriEX2ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000567 RID: 1383 RVA: 0x0001B588 File Offset: 0x00019988
		public HoriEX2ControllerMacProfile()
		{
			base.Name = "Hori EX2 Controller";
			base.Meta = "Hori EX2 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(13)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62721)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21760)
				}
			};
		}
	}
}
