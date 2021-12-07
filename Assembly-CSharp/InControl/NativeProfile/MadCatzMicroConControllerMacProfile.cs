using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000AF RID: 175
	public class MadCatzMicroConControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000589 RID: 1417 RVA: 0x0001C3A0 File Offset: 0x0001A7A0
		public MadCatzMicroConControllerMacProfile()
		{
			base.Name = "Mad Catz MicroCon Controller";
			base.Meta = "Mad Catz MicroCon Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18230)
				}
			};
		}
	}
}
