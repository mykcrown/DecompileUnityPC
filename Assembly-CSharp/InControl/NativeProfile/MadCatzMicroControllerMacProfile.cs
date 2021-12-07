using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000B0 RID: 176
	public class MadCatzMicroControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600058A RID: 1418 RVA: 0x0001C400 File Offset: 0x0001A800
		public MadCatzMicroControllerMacProfile()
		{
			base.Name = "Mad Catz Micro Controller";
			base.Meta = "Mad Catz Micro Controller on Mac";
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
